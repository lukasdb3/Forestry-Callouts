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
using ForestryCallouts2.Backbone.SpawnSystem.Land;
#endregion

namespace ForestryCallouts2.Callouts.LandCallouts
{
    [CalloutInfo("DeadAnimal", CalloutProbability.Medium)]

    internal class DeadAnimal : Callout
    {
        #region Variables

        internal readonly string CurCall = "DeadAnimal";
        
        //animal Variables
        private Ped _animal;
        private Blip _animalBlip;
        private Vector3 _animalSpawn;

        #endregion
        
        public override bool OnBeforeCalloutDisplayed()
        {
            //Gets spawnpoints from closest chunk
            ChunkChooser.Main(in CurCall);
            _animalSpawn = ChunkChooser.FinalSpawnpoint;

            //Normal callout details
            ShowCalloutAreaBlipBeforeAccepting(_animalSpawn, 30f);
            CalloutMessage = ("~g~Dead Animal Reported");
            CalloutPosition = _animalSpawn; 
            AddMinimumDistanceCheck(IniSettings.MinCalloutDistance, CalloutPosition);
            CalloutAdvisory = ("~b~Dispatch:~w~ Dead animal has been reported. Respond Code 2");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_02 CRIME_SUSPECT_ON_THE_RUN_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_02_01", _animalSpawn);
            return base.OnBeforeCalloutDisplayed();
        }

        public override void OnCalloutDisplayed()
        {
            //Send info to callout interface
            if (PluginChecker.CalloutInterface) CFunctions.CISendCalloutDetails(this, "CODE 2", "SASP");
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
            //Spawn animal
            CFunctions.SpawnAnimal(out _animal, _animalSpawn, new Random().Next(1, 361));
            _animal.Kill();
            
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();
        }

        public override void End()
        {
            base.End();
        }
    }
}