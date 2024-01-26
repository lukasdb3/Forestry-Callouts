#region Refrences
//System
using System;
using System.Drawing;
using System.Windows.Forms;
using CalloutInterfaceAPI;
//Rage
using Rage;
//LSPDFR
using LSPD_First_Response.Mod.Callouts;
//ForestryCallouts2
using ForestryCallouts2.Backbone;
using ForestryCallouts2.Backbone.Functions;
using ForestryCallouts2.Backbone.IniConfiguration;
using ForestryCallouts2.Backbone.SpawnSystem;
using ForestryCallouts2.Backbone.SpawnSystem.Land;
using Functions = LSPD_First_Response.Mod.API.Functions;

#endregion

namespace ForestryCallouts2.Callouts.LandCallouts
{
    [CalloutInterface("[FC] AnimalAttack", CalloutProbability.Medium, "Domestic Animal Attack", "Code 3", "SASP")]
    internal class AnimalAttack : FcCallout
    {
        #region Variables

        internal override string CurrentCall { get; set; } = "AnimalAttack";
        internal override string CurrentCallFriendlyName { get; set; } = "Animal Attack";
        protected override Vector3 Spawnpoint { get; set; }
        
        //victim variables
        private Ped _victim;
        private Blip _victimBlip;
        
        //animal variables
        private Ped _animal;
        private Blip _animalBlip;
        private Random _rand = new();
        
        //timer variables
        private int _timer = 0;
        private bool _pauseTimer;
        
        //search area variables
        private Blip _areaBlip;
        private Vector3 _searchArea;
        private bool _victimFound;
        private bool _animalFound;
        private bool _maxNotfiSent;
        private bool _firstBlip;
        private int _notfiSentCount;

        //callout variables
        private bool _onScene;
        private bool _hateSet;
        #endregion

        public override bool OnBeforeCalloutDisplayed()
        {
            CalloutMessage = ("~g~Animal Attack");
            CalloutAdvisory = ("~b~Dispatch:~w~ Animal attacking person reported. Respond code 3");
            Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT_01 ASSISTANCE_REQUIRED_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_03_01", Spawnpoint);
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            //Spawn victim
            CFunctions.SpawnHikerPed(out _victim, Spawnpoint, _rand.Next(1, 361));
            _victimBlip = CFunctions.CreateBlip(_victim, true, Color.Yellow, Color.Yellow, 1f);
            _victim.Health = 10;
            //Spawn animal
            _animal = new Ped("a_c_mtlion", World.GetNextPositionOnStreet(Spawnpoint.Around(40f, 60f)), _rand.Next(1, 361));
            _animal.IsPersistent = true;
            _animal.BlockPermanentEvents = true;
            _animal.Tasks.Wander();
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (!_animal || !_victim) return;
            
            //If the player is 200 or closer delete route and blip
            if (_victim)
            {
                if (Game.LocalPlayer.Character.DistanceTo(_victim) <= 200f && !_onScene)
                {
                    CalloutInterfaceAPI.Functions.SendMessage(this, "Unit "+IniSettings.Callsign+" proceed with caution.");
                    Functions.PlayScannerAudio("GP_ATTENTION_UNIT "+Main.CallsignAudioString+" GP_CAUTION_02");
                    Log.CallDebug(this, "Process started");
                    _onScene = true;
                    if (_victimBlip) _victimBlip.Delete();
                    _firstBlip = true;
                }   
            }

            if (!_pauseTimer)
            {
                _timer++;
            }

            //If victim isn't found initialize the search area
            if (!_victimFound && _onScene)
            {
                if (_firstBlip && _timer >= 1 || _timer >= 1250)
                {
                    if (_areaBlip) _areaBlip.Delete();
                    _areaBlip = CFunctions.SpawnSearchArea(_victim.Position, 10f, 20f, 35f, Color.Yellow, .5f);
                    _notfiSentCount++;
                    Log.CallDebug(this, "Search areas sent: " + _notfiSentCount + "");
                    _firstBlip = false;
                    _timer = 0;
                }

                //we delete the search area, and blip the victim because the player is taking to long to find the victim
                if (_notfiSentCount == IniSettings.SearchAreaNotifications && !_maxNotfiSent)
                {
                    //Pause the timer so search blips dont keep coming in
                    Log.CallDebug(this, "Blipped victim because player took to long to find them.");
                    _pauseTimer = true;
                    if (_areaBlip) _areaBlip.Delete();
                    _victimBlip = CFunctions.CreateBlip(_victim, true, Color.Orange, Color.Yellow, .75f);
                    _maxNotfiSent = true;
                }
            }
            
            //player found the victim
            if (!_victimFound && Game.LocalPlayer.Character.DistanceTo(_victim) <= 10f)
            {
                Log.CallDebug(this, "Victim found!");
                CalloutInterfaceAPI.Functions.SendMessage(this, "Unit "+IniSettings.Callsign+" on scene with victim.");
                _victimBlip = CFunctions.CreateBlip(_victim, false, Color.Orange, Color.Yellow, .75f);
                if (_areaBlip) _areaBlip.Delete();
                _victimFound = true;
                //reset
                if (_animal.IsAlive)
                {
                    _timer = 0;
                    _pauseTimer = false;
                    _maxNotfiSent = false;
                    _notfiSentCount = 0;
                    _firstBlip = true;
                }
                else
                {
                    _pauseTimer = true;
                }
            }

            if (_victimFound)
            {
                if (_animal.IsAlive)
                {
                    if (_animal.DistanceTo(Game.LocalPlayer.Character) <= 25f && Game.LocalPlayer.Character.IsOnFoot && !_hateSet)
                    {
                        _animal.RelationshipGroup.SetRelationshipWith(Game.LocalPlayer.Character.RelationshipGroup, Relationship.Hate);
                        _animal.Tasks.FightAgainst(Game.LocalPlayer.Character);
                        _hateSet = true;
                    }
                    
                    if (!_animalFound && Game.LocalPlayer.Character.DistanceTo(_animal) <= 10f)
                    {
                        Log.CallDebug(this, "Animal found!");
                        _animalBlip = CFunctions.CreateBlip(_animal, false, Color.Red, Color.Yellow, .75f);
                        if (_areaBlip) _areaBlip.Delete();
                        _animalFound = true;
                        if (_victimFound) _pauseTimer = true;
                    }
                    
                    if (!_animalFound)
                    {
                        if (_firstBlip && _timer >= 1 || _timer >= 1250)
                        {
                            if (_areaBlip) _areaBlip.Delete();
                            _areaBlip = CFunctions.SpawnSearchArea(_animal.Position, 10f, 20f, 35f, Color.Red, .5f);
                            _notfiSentCount++;
                            Log.CallDebug(this, "Search areas sent: " + _notfiSentCount + "");
                            _firstBlip = false;
                            _timer = 0;
                        }

                        //we delete the search area, and blip the animal because the player is taking to long to find the animal
                        if (_notfiSentCount == IniSettings.SearchAreaNotifications && !_maxNotfiSent)
                        {
                            //Pause the timer so search blips dont keep coming in
                            Log.CallDebug(this, "Blipped animal because player took to long to find them.");
                            _pauseTimer = true;
                            if (_areaBlip) _areaBlip.Delete();
                            _animalBlip = CFunctions.CreateBlip(_animal, true, Color.Red, Color.Yellow, .75f);
                            _maxNotfiSent = true;
                        }
                        
                    }
                }
                else
                {
                    if (_animalBlip) _animalBlip.Delete();
                    if (_areaBlip) _areaBlip.Delete();
                }
            }
            base.Process();
        }

        public override void End()
        {
            if (_victim) _victim.Dismiss();
            if (_victimBlip) _victimBlip.Delete();
            if (_animal) _animal.Dismiss();
            if (_animalBlip) _animal.Delete();
            if (_areaBlip) _areaBlip.Delete();
            base.End();
        }
    }
}