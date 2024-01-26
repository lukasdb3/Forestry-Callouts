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
    [CalloutInterface("[FC] AnimalAttack2", CalloutProbability.Medium, "Domestic Animal Attack", "Code 3", "SASP")]
    internal class AnimalAttack2 : FcCallout
    {
        #region Variables

        internal override string CurrentCall { get; set; } = "AnimalAttack2";
        internal override string CurrentCallFriendlyName { get; set; } = "Animal Attack";
        protected override Vector3 Spawnpoint { get; set; }
        
        //victim variables
        private Ped _victim;
        private float _victimHeading;
        private Blip _victimBlip;
        
        //animal variables
        private Ped _animal;
        private Blip _animalBlip;
        private Random _rand = new();

        //callout variables
        private bool _onScene;
        private bool _animalDeadVicAlive;
        private bool _victimHasDied;
        #endregion

        public override bool OnBeforeCalloutDisplayed()
        {
            //Extra spawnpoint / heading stuff
            _victimHeading = ChunkChooser.FinalHeading;
            
            //Normal callout details
            CalloutMessage = ("~g~Animal Attack");
            CalloutAdvisory = ("~b~Dispatch:~w~ Animal mauling person in progress. Respond code 3");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT_01 ASSISTANCE_REQUIRED_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_03_01", Spawnpoint);
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            //Spawn victim
            CFunctions.SpawnHikerPed(out _victim, Spawnpoint, _victimHeading);
            _victimBlip = CFunctions.CreateBlip(_victim, true, Color.Yellow, Color.Yellow, 1f);
            _victim.Health = 1000;
            //Spawn animal
            _animal = new Ped("a_c_mtlion", World.GetNextPositionOnStreet(Spawnpoint), _victimHeading + 180f);
            _animal.IsPersistent = true;
            _animal.BlockPermanentEvents = true;
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            //If the player is 200 or closer delete route and blip
            if (Game.LocalPlayer.Character.DistanceTo(_victim) <= 200f && !_onScene)
            {
                CalloutInterfaceAPI.Functions.SendMessage(this, "Unit "+IniSettings.Callsign+" proceed with caution.");
                Functions.PlayScannerAudio("GP_ATTENTION_UNIT "+Main.CallsignAudioString+" GP_CAUTION_02");
                Log.CallDebug(this, "Process started");
                    
                _animal.RelationshipGroup.SetRelationshipWith(Game.LocalPlayer.Character.RelationshipGroup, Relationship.Hate);
                _animal.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Hate);
                RelationshipGroup.Cop.SetRelationshipWith(_animal.RelationshipGroup, Relationship.Hate);
                _victim.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Neutral);
                RelationshipGroup.Cop.SetRelationshipWith(_animal.RelationshipGroup, Relationship.Neutral);
                _victim.Tasks.ReactAndFlee(_animal);
                GameFiber.Wait(150);
                _animal.Tasks.FightAgainst(_victim);
                
                if (_victimBlip) _victimBlip.Delete();
                _animalBlip = CFunctions.CreateBlip(_animal, false, Color.Red, Color.Yellow, .75f);
                _victimBlip = CFunctions.CreateBlip(_victim, true, Color.Orange, Color.Yellow, .75f);
                _onScene = true;
            }

            if (Game.LocalPlayer.Character.DistanceTo(_animal) <= 30f) _victimBlip.IsRouteEnabled = false;

            if (_victim.IsDead)
            {
                _animal.Tasks.Wander();
                _victimHasDied = true;
            }

            if (_animal.IsDead && _victim.IsAlive && !_animalDeadVicAlive && !_victimHasDied)
            {
                _animalDeadVicAlive = true;
                GameFiber.Wait(500);
                _victim.Tasks.Clear();
                _victim.Tasks.StandStill(-1).WaitForCompletion();
                _victim.Heading = Game.LocalPlayer.Character.Heading + 180;
                Game.DisplaySubtitle("~g~Victim:~w~ Oh my god.. you saved my life!");
            }
            base.Process();
        }

        public override void End()
        {
            if (_victim) _victim.Dismiss();
            if (_victimBlip) _victimBlip.Delete();
            if (_animal) _animal.Dismiss();
            if (_animalBlip) _animal.Delete();
            base.End();
        }
    }
}