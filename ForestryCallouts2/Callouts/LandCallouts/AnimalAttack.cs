#region Refrences
//System
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
using ForestryCallouts2.Backbone.SpawnSystem.Land;
#endregion

namespace ForestryCallouts2.Callouts.LandCallouts
{
    [CalloutInfo("AnimalAttack", CalloutProbability.Medium)]
    
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
        
        //timer variables
        private int _timer = 0;
        private bool _pauseTimer;
        
        //search area variables
        private Blip _animalAreaBlip;
        private Vector3 _searchArea;
        private bool _animalFound;
        private bool _maxNotfiSent;
        private bool _firstBlip;
        private int _notfiSentCount;
        
        //callout variables
        private bool _onScene;
        #endregion
        
        
        public override bool OnBeforeCalloutDisplayed()
        {
            //Gets spawnpoints from closest chunk
            ChunkChooser.Main(in CurCall);
            _victimSpawn = ChunkChooser.FinalSpawnpoint;
            _victimHeading = ChunkChooser.FinalHeading;
            
            //Normal callout details
            ShowCalloutAreaBlipBeforeAccepting(_victimSpawn, 30f);
            CalloutMessage = ("~g~Animal Attack Reported");
            CalloutPosition = _victimSpawn; 
            AddMinimumDistanceCheck(IniSettings.MinCalloutDistance, CalloutPosition);
            CalloutAdvisory = ("~b~Dispatch:~w~ Animal attacking person reported. Respond code 3");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT_01 ASSISTANCE_REQUIRED_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_03_01", _victimSpawn);
            return base.OnBeforeCalloutDisplayed();
        }

        public override void OnCalloutDisplayed()
        {
            //Send callout info to Callout Interface
            if (PluginChecker.CalloutInterface) CFunctions.CISendCalloutDetails(this, "CODE 3", "SASP");
            Logger.CallDebugLog(this, "Callout displayed");
            base.OnCalloutDisplayed();
        }

        public override void OnCalloutNotAccepted()
        {
            if (PluginChecker.CalloutInterface) Functions.PlayScannerAudio("OTHER_UNITS_TAKING_CALL");

            base.OnCalloutNotAccepted();
        }

        public override bool OnCalloutAccepted()
        {
            Logger.CallDebugLog(this, "Callout accepted");
            //Spawn victim
            CFunctions.SpawnHikerPed(out _victim, _victimSpawn, _victimHeading);
            _victimBlip = _victim.AttachBlip();
            _victimBlip.EnableRoute(Color.Yellow);
            _victim.Health = 10;
            //Spawn animal
            _animal = new Ped("a_c_mtlion", World.GetNextPositionOnStreet(_victimSpawn.Around(30, 60f)), 0f);
            _animal.IsPersistent = true;
            _animal.BlockPermanentEvents = true;
            _animal.Tasks.Wander();
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (_animal)
            {
                if (!_animal.IsDead)
                { 
                    //If the player is 200 or closer delete route and blip
                    if (_victim)
                    {
                        if (Game.LocalPlayer.Character.DistanceTo(_victim) <= 20f && !_onScene)
                        {
                            Logger.CallDebugLog(this, "Process started");
                            Game.DisplayHelp("~g~Tend to the ~y~injured person~g~ and eliminate the ~r~animal");
                            _onScene = true;
                            if (_victimBlip) _victimBlip.Delete();
                            _firstBlip = true;
                        }   
                    }

                    //If suspect isn't found initialize the search area
                    if (!_animalFound && _onScene)
                    {
                        if (!_pauseTimer) _timer++;

                        if (_firstBlip && _timer >= 1 || _timer >= 1250)
                        {
                            if (_animalAreaBlip) _animalAreaBlip.Delete();
                            var position = _animal.Position;
                            _searchArea = position.Around2D(10f, 50f);
                            _animalAreaBlip = new Blip(_searchArea, 65f) {Color = Color.Yellow, Alpha = .5f};
                            _notfiSentCount++;
                            Logger.CallDebugLog(this, "Search areas sent: " + _notfiSentCount + "");
                            _firstBlip = false;
                            _timer = 0;
                        }

                        //we delete the search area, and blip the suspect because the player is taking to long to find the suspect
                        if (_notfiSentCount == IniSettings.SearchAreaNotifications && !_maxNotfiSent)
                        {
                            //Pause the timer so search blips dont keep coming in
                            Logger.CallDebugLog(this, "Blipped animal because player took to long to find them.");
                            _pauseTimer = true;
                            if (_animalAreaBlip) _animalAreaBlip.Delete();
                            _animalBlip = _animal.AttachBlip();
                            _animalBlip.Color = Color.Red;
                            _animalBlip.IsRouteEnabled = true;
                            _maxNotfiSent = true;
                        }
                    }
            
                    //player found the animal
                    if (!_animalFound && Game.LocalPlayer.Character.DistanceTo(_animal) <= 20f && Game.LocalPlayer.Character.IsOnFoot)
                    {
                        Logger.CallDebugLog(this, "Suspect found!");
                        _animalBlip = _animal.AttachBlip();
                        _animalBlip.Color = Color.Red;
                        if (_animalAreaBlip) _animalAreaBlip.Delete();
                        _animal.Tasks.FightAgainst(Game.LocalPlayer.Character);
                        _animalFound = true;
                    }
                }
                else
                {
                    if (_animalBlip) _animalBlip.Delete();
                }   
            }

            if (Game.IsKeyDown(IniSettings.EndCalloutKey)) //If player presses "End" it will forcefully clean the callout up
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
            if (!ChunkChooser.CalloutForceEnded)
            {
                Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", _victimSpawn);
                Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "Status", "~g~Animal Attack Code 4", "");
                if (PluginChecker.CalloutInterface) CFunctions.CISendMessage(this, "Animal Attack Code 4");
            }
            Logger.CallDebugLog(this, "Callout ended");
            base.End();
        }
    }
}