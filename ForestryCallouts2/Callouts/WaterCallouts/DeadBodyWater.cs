#region Refrences
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

namespace ForestryCallouts2.Callouts.WaterCallouts
{
    [CalloutInterface("Dead Body In Water", CalloutProbability.Low, "Deceased Victim", "Code 3", "SASP")]
    
    internal class DeadBodyWater : Callout
    {
        #region Variables
        
        internal readonly string CurCall = "DeadBodyWater";
        
        //victim variables
        private Ped _victim;
        private Vector3 _victimSpawn;
        private float _victimHeading;
        private Blip _victimBlip;

        //timer variables
        private int _timer = 0;
        private bool _pauseTimer;
        
        //search area variables
        private Blip _victimSearchArBlip;
        private Vector3 _searchArea;
        private bool _victimFound;
        private bool _maxNotfiSent;
        private bool _firstBlip;
        private int _notfiSentCount;
        
        //callout variables
        private bool _onScene;
        private Random _rand = new();
        #endregion
        
        public override bool OnBeforeCalloutDisplayed()
        {
            //Gets spawnpoints from closest chunk
            ChunkChooser.Main(in CurCall);
            _victimSpawn = ChunkChooser.FinalSpawnpoint;
            _victimHeading = _rand.Next(1, 361);
            
            //Normal callout details
            ShowCalloutAreaBlipBeforeAccepting(_victimSpawn, 30f);
            CalloutMessage = ("~g~Possible Dead Body");
            CalloutPosition = _victimSpawn; 
            AddMinimumDistanceCheck(IniSettings.MinCalloutDistance, CalloutPosition);
            CalloutAdvisory = ("~b~Dispatch:~w~ Possible dead body has been reported. Respond code 3");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT_01 ASSISTANCE_REQUIRED_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_03_01", _victimSpawn);
            return base.OnBeforeCalloutDisplayed();
        }

        public override void OnCalloutNotAccepted()
        {
            Functions.PlayScannerAudio("OTHER_UNITS_TAKING_CALL");
            base.OnCalloutNotAccepted();
        }

        public override bool OnCalloutAccepted()
        {
            Log.CallDebug(this, "Callout accepted");
            //Spawn victim
            CFunctions.SpawnBeachPed(out _victim, _victimSpawn, _victimHeading);
            _victimBlip = _victim.AttachBlip();
            _victimBlip.EnableRoute(Color.Yellow);
            _victim.Health = 10;
            _victim.IsInvincible = true;
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            //If the player is 200 or closer delete route and blip
            if (_victim)
            {
                if (Game.LocalPlayer.Character.DistanceTo(_victim) <= 200f && !_onScene)
                {
                    Log.CallDebug(this, "Process started");
                    _onScene = true;
                    if (_victimBlip) _victimBlip.Delete();
                    _firstBlip = true;
                }   
            }

            //If suspect isn't found initialize the search area
            if (!_victimFound && _onScene)
            {
                if (!_pauseTimer) _timer++;

                if (_firstBlip && _timer >= 1 || _timer >= 1250)
                {
                    if (_victimSearchArBlip) _victimSearchArBlip.Delete();
                    var position = _victim.Position;
                    _searchArea = position.Around2D(10f, 50f);
                    _victimSearchArBlip = new Blip(_searchArea, 65f) {Color = Color.Yellow, Alpha = .5f};
                    _notfiSentCount++;
                    Log.CallDebug(this, "Search areas sent: " + _notfiSentCount + "");
                    _firstBlip = false;
                    _timer = 0;
                }

                //we delete the search area, and blip the suspect because the player is taking to long to find the suspect
                if (_notfiSentCount == IniSettings.SearchAreaNotifications && !_maxNotfiSent)
                {
                    //Pause the timer so search blips dont keep coming in
                    Log.CallDebug(this, "Blipped victim because player took to long to find them.");
                    _pauseTimer = true;
                    if (_victimSearchArBlip) _victimSearchArBlip.Delete();
                    _victimBlip = _victim.AttachBlip();
                    _victimBlip.Color = Color.Red;
                    _victimBlip.IsRouteEnabled = true;
                    _maxNotfiSent = true;
                }
                
                //player found the animal
                if (!_victimFound && Game.LocalPlayer.Character.DistanceTo(_victim) <= 8f)
                {
                    Log.CallDebug(this, "Suspect found!");
                    _victimBlip = _victim.AttachBlip();
                    _victimBlip.Color = Color.Red;
                    if (_victimSearchArBlip) _victimSearchArBlip.Delete();
                    _victim.Tasks.FightAgainst(Game.LocalPlayer.Character);
                    _victimFound = true;
                }
            }

            if (_victimFound)
            {
               
            }
            
            if (CFunctions.IsKeyAndModifierDown(IniSettings.EndCalloutKey, IniSettings.EndCalloutKeyModifier))
            {
                Log.CallDebug(this, "Callout was force ended by player");
                End();
            }
            if (Game.LocalPlayer.Character.IsDead)
            {
                Log.CallDebug(this, "Player died callout ending");
                End();
            }
            base.Process();
        }
        
        public override void End()
        {
            if (_victimBlip) _victimBlip.Delete();
            if (_victim) _victim.Dismiss();
            if (_victimSearchArBlip) _victimSearchArBlip.Delete();
            if (!ChunkChooser.StoppingCurrentCall)
            {
                Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", _victimSpawn);
                Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "Status", "~g~Dead Body Code 4", "");
                CalloutInterfaceAPI.Functions.SendMessage(this, "Dead Body Code 4");
            }
            Log.CallDebug(this, "Callout ended");
            
            base.End();
        }
    }
}