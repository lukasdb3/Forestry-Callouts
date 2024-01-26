#region Refrences
//System
using System.Drawing;
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
    [CalloutInterface("[FC] DeadAnimalOnRoadway", CalloutProbability.Medium, "Dead Animal", "Code 2", "SASP")]

    internal class DeadAnimalOnRoadway : FcCallout
    {
        #region Variables
        
        internal override string CurrentCall { get; set; } = "DeadAnimalOnRoadway";
        internal override string CurrentCallFriendlyName { get; set; } = "Dead Animal On Roadway";
        protected override Vector3 Spawnpoint { get; set; }
        
        //animal
        private float _animalHeading;
        private Ped _animal;
        private Blip _animalBlip;
        
        //other
        private bool _onScene;
        #endregion
        
        
        public override bool OnBeforeCalloutDisplayed()
        {
            _animalHeading = ChunkChooser.FinalHeading;
            //Normal callout details
            CalloutMessage = ("~g~Dead Animal On Roadway");
            CalloutAdvisory = ("~b~Dispatch:~w~ A dead animal has been reported on a roadway.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT_01 ASSISTANCE_REQUIRED_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_02_01", Spawnpoint);
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            //Spawn victim
            CFunctions.SpawnAnimal(out _animal, Spawnpoint, _animalHeading);
            _animalBlip = CFunctions.CreateBlip(_animal, true, Color.Yellow, Color.Yellow, 1);
            _animal.Health = 10;
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (_animal)
            {
                if (Game.LocalPlayer.Character.DistanceTo(_animal) <= 30f && !_onScene)
                {
                    _onScene = true;
                    _animalBlip.IsRouteEnabled = false;
                    _animalBlip.Color = Color.Purple;
                    _animalBlip.Scale = 0.75f;
                    Game.DisplayHelp("~g~Call ~y~Animal Control ~g~ to take care of the Animal.");
                }
            }
            base.Process();
        }

        public override void End()
        {
            if (_animal) _animal.Dismiss();
            if (_animalBlip) _animalBlip.Delete();
            base.End();
        }
    }
}