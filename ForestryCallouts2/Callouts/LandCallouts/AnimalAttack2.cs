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
    internal class AnimalAttack2 : Callout
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
            _victimHeading = ChunkChooser.FinalHeading;
            
            //Normal callout details
            ShowCalloutAreaBlipBeforeAccepting(_victimSpawn, 30f);
            CalloutMessage = ("~g~Animal Attack");
            CalloutPosition = _victimSpawn; 
            AddMinimumDistanceCheck(IniSettings.MinCalloutDistance, CalloutPosition);
            CalloutAdvisory = ("~b~Dispatch:~w~ Animal mauling person in progress. Respond code 3");
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
            CFunctions.SpawnHikerPed(out _victim, _victimSpawn, _victimHeading);
            _victimBlip = _victim.AttachBlip();
            _victimBlip.EnableRoute(Color.Yellow);
            _victim.Health = 1000;
            //Spawn animal
            _animal = new Ped("a_c_mtlion", World.GetNextPositionOnStreet(_victimSpawn), _victimHeading + 180f);
            _animal.IsPersistent = true;
            _animal.BlockPermanentEvents = true;
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            //If the player is 200 or closer delete route and blip
            if (_victim.IsAlive)
            {
                if (Game.LocalPlayer.Character.DistanceTo(_victim) <= 200f && !_onScene)
                {
                    CalloutInterfaceAPI.Functions.SendMessage(this, "Unit "+IniSettings.Callsign+" proceed with caution.");
                    Functions.PlayScannerAudio("GP_ATTENTION_UNIT "+Main.CallsignAudioString+" GP_CAUTION_02");
                    Logger.CallDebugLog(this, "Process started");
                    _animal.RelationshipGroup.SetRelationshipWith(Game.LocalPlayer.Character.RelationshipGroup, Relationship.Hate);
                    _animal.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Hate);
                    RelationshipGroup.Cop.SetRelationshipWith(_animal.RelationshipGroup, Relationship.Hate);
                    _victim.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Neutral);
                    RelationshipGroup.Cop.SetRelationshipWith(_animal.RelationshipGroup, Relationship.Neutral);
                    _animal.Tasks.FightAgainst(_victim);
                    _victim.Tasks.ReactAndFlee(_animal);
                    _onScene = true;
                    if (_victimBlip) _victimBlip.Delete();
                }

                if (Game.LocalPlayer.Character.DistanceTo(_animal) <= 15f && !_animalFound)
                {
                    _animalFound = true;
                    _animalBlip = _animal.AttachBlip();
                    _animalBlip.Scale = .7f;
                    _animalBlip.Color = Color.Red;
                    _victimBlip = _victim.AttachBlip();
                    _victimBlip.Scale = .7f;
                    _victimBlip.Color = Color.Green;
                    
                }
            }
            else
            {
                _animal.Tasks.Wander();
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
                Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "Status", "~g~Animal Attack Code 4", "");
                CalloutInterfaceAPI.Functions.SendMessage(this, "Unit "+IniSettings.Callsign+" reporting Animal Attack code 4");
            }
            Logger.CallDebugLog(this, "Callout ended");
            base.End();
        }
    }
}