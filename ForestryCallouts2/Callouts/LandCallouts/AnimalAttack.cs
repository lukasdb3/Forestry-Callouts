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
    [CalloutInterface("Animal Attack", CalloutProbability.Medium, "Domestic Animal Attack", "Code 3", "SASP")]
    internal class AnimalAttack : Callout
    {
        #region Variables
        
        internal readonly string CurCall = "AnimalAttack";
        
        //victim variables
        private Ped _victim;
        private Vector3 _victimSpawn;
        private float _victimHeading;
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
        private bool _playerCloseToAnimal;
        #endregion
        
        
        public override bool OnBeforeCalloutDisplayed()
        {
            //Gets spawnpoints from closest chunk
            ChunkChooser.Main(in CurCall);
            _victimSpawn = ChunkChooser.FinalSpawnpoint;

            //Normal callout details
            ShowCalloutAreaBlipBeforeAccepting(_victimSpawn, 30f);
            CalloutMessage = ("~g~Animal Attack");
            CalloutPosition = _victimSpawn; 
            AddMinimumDistanceCheck(IniSettings.MinCalloutDistance, CalloutPosition);
            CalloutAdvisory = ("~b~Dispatch:~w~ Animal attacking person reported. Respond code 3");
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
            Logger.CallDebugLog(this, "Callout accepted");
            //Spawn victim
            CFunctions.SpawnHikerPed(out _victim, _victimSpawn, _rand.Next(1, 361));
            _victimBlip = _victim.AttachBlip();
            _victimBlip.EnableRoute(Color.Yellow);
            _victim.Health = 10;
            //Spawn animal
            _animal = new Ped("a_c_mtlion", World.GetNextPositionOnStreet(_victimSpawn.Around(40f, 60f)), _rand.Next(1, 361));
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
                    Logger.CallDebugLog(this, "Process started");
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
                    var position = _victim.Position;
                    _searchArea = position.Around2D(10f, 20f);
                    _areaBlip = new Blip(_searchArea, 35f) {Color = Color.Yellow, Alpha = .5f};
                    _notfiSentCount++;
                    Logger.CallDebugLog(this, "Search areas sent: " + _notfiSentCount + "");
                    _firstBlip = false;
                    _timer = 0;
                }

                //we delete the search area, and blip the victim because the player is taking to long to find the victim
                if (_notfiSentCount == IniSettings.SearchAreaNotifications && !_maxNotfiSent)
                {
                    //Pause the timer so search blips dont keep coming in
                    Logger.CallDebugLog(this, "Blipped victim because player took to long to find them.");
                    _pauseTimer = true;
                    if (_areaBlip) _areaBlip.Delete();
                    _victimBlip = _victim.AttachBlip();
                    _victimBlip.Color = Color.ForestGreen;
                    _victimBlip.Scale = .7f;
                    _victimBlip.IsRouteEnabled = true;
                    _maxNotfiSent = true;
                }
            }
            
            //player found the victim
            if (!_victimFound && Game.LocalPlayer.Character.DistanceTo(_victim) <= 10f)
            {
                Logger.CallDebugLog(this, "Victim found!");
                CalloutInterfaceAPI.Functions.SendMessage(this, "Unit "+IniSettings.Callsign+" on scene with victim.");
                _victimBlip = _victim.AttachBlip();
                _victimBlip.Color = Color.ForestGreen;
                _victimBlip.Scale = .7f;
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
                    if (_animal.DistanceTo(Game.LocalPlayer.Character) <= 25f && Game.LocalPlayer.Character.IsOnFoot)
                    {
                        _animal.RelationshipGroup.SetRelationshipWith(Game.LocalPlayer.Character.RelationshipGroup, Relationship.Hate);
                        _animal.Tasks.FightAgainst(Game.LocalPlayer.Character);
                    }
                    
                    if (!_animalFound && Game.LocalPlayer.Character.DistanceTo(_animal) <= 10f)
                    {
                        Logger.CallDebugLog(this, "Animal found!");
                        _animalBlip = _animal.AttachBlip();
                        _animalBlip.Color = Color.Red;
                        _animalBlip.Scale = .7f;
                        if (_areaBlip) _areaBlip.Delete();
                        _animalFound = true;
                        if (_victimFound) _pauseTimer = true;
                    }
                    
                    if (!_animalFound)
                    {
                        if (_firstBlip && _timer >= 1 || _timer >= 1250)
                        {
                            if (_areaBlip) _areaBlip.Delete();
                            var position = _animal.Position;
                            _searchArea = position.Around2D(10f, 20f);
                            _areaBlip = new Blip(_searchArea, 35f) {Color = Color.Red, Alpha = .5f};
                            _notfiSentCount++;
                            Logger.CallDebugLog(this, "Search areas sent: " + _notfiSentCount + "");
                            _firstBlip = false;
                            _timer = 0;
                        }

                        //we delete the search area, and blip the animal because the player is taking to long to find the animal
                        if (_notfiSentCount == IniSettings.SearchAreaNotifications && !_maxNotfiSent)
                        {
                            //Pause the timer so search blips dont keep coming in
                            Logger.CallDebugLog(this, "Blipped animal because player took to long to find them.");
                            _pauseTimer = true;
                            if (_areaBlip) _areaBlip.Delete();
                            _animalBlip = _animal.AttachBlip();
                            _animalBlip.Color = Color.Red;
                            _animalBlip.Scale = .7f;
                            _animalBlip.IsRouteEnabled = true;
                            _maxNotfiSent = true;
                        }
                        
                    }
                }
                else
                {
                    if (_animalBlip) _animalBlip.Delete();
                }
            }
            

            if (CFunctions.IsKeyAndModifierDown(IniSettings.EndCalloutKey, IniSettings.EndCalloutKeyModifier))
            {
                Logger.CallDebugLog(this, "Callout was force ended by player");
                End();
            }
            if (Game.LocalPlayer.Character.IsDead)
            {
                Logger.CallDebugLog(this, "Player died callout ending");
                End();
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
            if (!ChunkChooser.StoppingCurrentCall)
            {
                Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 GP_CODE4_01", _victimSpawn);
                if (IniSettings.EndNotfiMessages) Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "Status", "~g~Animal Attack Code 4", "");
                CalloutInterfaceAPI.Functions.SendMessage(this, "Unit "+IniSettings.Callsign+" reporting Animal Attack code 4");
            }
            Logger.CallDebugLog(this, "Callout ended");
            base.End();
        }
    }
}