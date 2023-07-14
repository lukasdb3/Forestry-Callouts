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
using ForestryCallouts2.Backbone.SpawnSystem;
using ForestryCallouts2.Backbone.SpawnSystem.Land;
#endregion

namespace ForestryCallouts2.Callouts.LandCallouts
{
    [CalloutInfo("DeadAnimalOnRoadway", CalloutProbability.Medium)]
    
    internal class DeadAnimalOnRoadway : Callout
    {
        #region Variables
        
        internal readonly string CurCall = "DeadAnimalOnRoadway";
        
        //animal
        private Vector3 _animalSpawn;
        private float _animalHeading;
        private Ped _animal;
        private Blip _animalBlip;
        
        //other
        private bool _onScene;
        #endregion
        
        
        public override bool OnBeforeCalloutDisplayed()
        {
            //Gets spawnpoints from closest chunk
            ChunkChooser.Main(in CurCall);
            _animalSpawn = ChunkChooser.FinalSpawnpoint;
            _animalHeading = ChunkChooser.FinalHeading;
            
            //Normal callout details
            ShowCalloutAreaBlipBeforeAccepting(_animalSpawn, 30f);
            CalloutMessage = ("~g~Dead Animal On Roadway");
            CalloutPosition = _animalSpawn; 
            AddMinimumDistanceCheck(IniSettings.MinCalloutDistance, CalloutPosition);
            CalloutAdvisory = ("~b~Dispatch:~w~ A dead animal has been reported on a roadway, requesting assistance on removing the hazard.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT_01 ASSISTANCE_REQUIRED_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_03_01", _animalSpawn);
            return base.OnBeforeCalloutDisplayed();
        }

        public override void OnCalloutDisplayed()
        {
            //Send callout info to Callout Interface
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
            //Spawn victim
            CFunctions.SpawnAnimal(out _animal, _animalSpawn, _animalHeading);
            _animalBlip = _animal.AttachBlip();
            _animalBlip.EnableRoute(Color.Yellow);
            _animal.Health = 10;
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (Game.LocalPlayer.Character.DistanceTo(_animal) <= 30f && !_onScene)
            {
                _onScene = true;
                _animalBlip.IsRouteEnabled = false;
                Game.DisplayHelp("~g~Call ~y~Animal Control ~g~ to take care of the Animal.");
            }
            base.Process();
        }

        public override void End()
        {
            if (_animal) _animal.Dismiss();
            if (_animalBlip) _animalBlip.Delete();
            if (!ChunkChooser.StoppingCurrentCall)
            {
                Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", _animalSpawn);
                Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "Status", "~g~Dead Animal On Roadway Code 4", "");
                if (PluginChecker.CalloutInterface) CFunctions.CISendMessage(this, "Dead Animal On Roadway Code 4");
            }
            Logger.CallDebugLog(this, "Callout ended");
            base.End();
        }
    }
}