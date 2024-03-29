﻿#region Refrences
//System
using System;
using System.Drawing;
//Rage
using Rage;
//LSPDFR
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
//ForestryCallouts2
using ForestryCallouts2.Backbone;
using ForestryCallouts2.Backbone.Functions;
using ForestryCallouts2.Backbone.IniConfiguration;
using ForestryCallouts2.Backbone.SpawnSystem;
using ForestryCallouts2.Backbone.SpawnSystem.Land;
//CalloutInterface
using CalloutInterfaceAPI;
using Functions = LSPD_First_Response.Mod.API.Functions;

#endregion

namespace ForestryCallouts2.Callouts.LandCallouts
{
    [CalloutInterface("[FC] DangerousPerson", CalloutProbability.Low, "Dangerous Individual", "Code 3", "SASP")]
    
    internal class DangerousPerson : FcCallout
    {
        #region Variables

        internal override string CurrentCall { get; set; } = "DangerousPerson";
        internal override string CurrentCallFriendlyName { get; set; } = "Dangerous Person";
        protected override Vector3 Spawnpoint { get; set; }
        
        //victim variables
        private Ped _suspect;
        private Blip _suspectBlip;

        //timer variables
        private int _timer = 0;
        private bool _pauseTimer;
        
        //search area variables
        private Blip _suspectAreaBlip;
        private bool _maxNotfiSent;
        private bool _firstBlip;
        private int _notfiSentCount;
        
        //callout variables
        private Random _rand = new();
        private int _scenario;
        private int _scenario2;
        private bool _onScene;
        private bool _suspectFound;
        private LHandle _pursuit;
        private bool _pursuitStarted;
        private bool _scenarioShootingStarted;
        #endregion

        public override bool OnBeforeCalloutDisplayed()
        {
            //Normal callout details
            CalloutMessage = ("~g~Dangerous Person");
            _scenario = _rand.Next(1, 5);
            if (_scenario == 1) CalloutAdvisory = ("~b~Dispatch:~w~ Dangerous person reported with a automatic rifle. Respond code 3");
            if (_scenario == 2) CalloutAdvisory = ("~b~Dispatch:~w~ Dangerous person reported with a pistol. Respond code 3");
            if (_scenario == 3) CalloutAdvisory = ("~b~Dispatch:~w~ Dangerous person reported with a shotgun. Respond code 3");
            if (_scenario == 4) CalloutAdvisory = ("~b~Dispatch:~w~ Dangerous person reported with a melee weapon. Respond code 3");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT_01 ASSISTANCE_REQUIRED_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_03_01", Spawnpoint);
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Log.CallDebug(this, "Scenario: " + _scenario);
            //Spawn the suspect
            CFunctions.SpawnCountryPed(out _suspect, Spawnpoint, _rand.Next(1, 361));
            //Give gun
            if (_scenario == 1) CFunctions.RifleWeaponChooser(_suspect, -1, true);
            if (_scenario == 2) CFunctions.PistolWeaponChooser(_suspect, -1, true);
            if (_scenario == 3) CFunctions.ShotgunWeaponChooser(_suspect, -1, true);
            if (_scenario == 4) CFunctions.MeleeWeaponChooser(_suspect, -1, true);
            //Sets a blip on the suspects head and enables route
            _suspectBlip = CFunctions.CreateBlip(_suspect, true, Color.Yellow, Color.Yellow, 1f);
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
                //If the player is 200 or closer delete route and blip
                if (Game.LocalPlayer.Character.DistanceTo(_suspect) <= 200f && !_onScene)
                {
                    _suspect.Tasks.Wander();
                    Log.CallDebug(this, "Process started");
                    _onScene = true;
                    if (_suspectBlip) _suspectBlip.Delete();
                    _firstBlip = true;
                }

                //If suspect isn't found initialize the search area
                if (!_suspectFound && _onScene)
                {
                    if (!_pauseTimer) _timer++;

                    if (_firstBlip && _timer >= 1 || _timer >= 1250)
                    {
                        if (_suspectAreaBlip) _suspectAreaBlip.Delete();
                        _suspectAreaBlip = CFunctions.SpawnSearchArea(_suspect.Position, 10f, 30f, 65f, Color.Yellow, .5f);
                        _notfiSentCount++;
                        
                        Log.CallDebug(this, "Search areas sent: " + _notfiSentCount + "");
                        _firstBlip = false;
                        Functions.PlayScannerAudioUsingPosition("SUSPECT_LAST_SEEN_01 IN_OR_ON_POSITION",
                            _suspect.Position);
                        _timer = 0;
                    }

                    //we delete the search area, and blip the suspect because the player is taking to long to find the suspect
                    if (_notfiSentCount == IniSettings.SearchAreaNotifications && !_maxNotfiSent)
                    {
                        //Pause the timer so search blips dont keep coming in
                        Log.CallDebug(this, "Blipped suspect because player took to long to find them.");
                        _pauseTimer = true;
                        if (_suspectAreaBlip) _suspectAreaBlip.Delete();
                        _suspectBlip = CFunctions.CreateBlip(_suspect, true, Color.Red, Color.Yellow, .75f);
                        _maxNotfiSent = true;
                    }
                }
                
                //player found the intoxicated ped
                if (!_suspectFound && Game.LocalPlayer.Character.DistanceTo(_suspect) <= 20f)
                {
                    Log.CallDebug(this, "Suspect found!");
                    _suspectBlip = CFunctions.CreateBlip(_suspect, false, Color.Red, Color.Yellow, .75f);
                    if (_suspectAreaBlip) _suspectAreaBlip.Delete();
                    _suspectFound = true;
                }

                if (_suspectFound)
                {
                    _scenario2 = 2;
                    if (_scenario2 == 1)
                    {
                        if (!_pursuitStarted)
                        {
                            if (Game.LocalPlayer.Character.DistanceTo(_suspect) <= 300f && !_pursuitStarted)
                            {
                                if (_suspectBlip) _suspectBlip.Delete();
                                _suspect.PlayAmbientSpeech("GENERIC_CURSE_MED");
                                _pursuit = Functions.CreatePursuit();
                                Functions.SetPursuitIsActiveForPlayer(_pursuit, true);
                                Functions.AddPedToPursuit(_pursuit, _suspect);
                                Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                                _pursuitStarted = true;
                            }
                        }
                    }
                    if (_scenario2 == 2 && !_scenarioShootingStarted)
                    {
                        _suspect.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Hate);
                        RelationshipGroup.Cop.SetRelationshipWith(_suspect.RelationshipGroup, Relationship.Hate);
                        _scenarioShootingStarted = true;
                    }
                }
                base.Process();
        }

        public override void End()
        {
            if (_suspect) _suspect.Dismiss();
            if (_suspectBlip) _suspectBlip.Delete();
            if (_suspectAreaBlip) _suspectAreaBlip.Delete();
            if (_scenario2 == 1) if (Functions.IsPursuitStillRunning(_pursuit)) Functions.ForceEndPursuit(_pursuit);
            base.End();
        }
    }
}