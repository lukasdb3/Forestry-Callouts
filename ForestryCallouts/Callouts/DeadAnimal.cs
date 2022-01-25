using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using System.Drawing;
using ForestryCallouts.Ini;
using ForestryCallouts.SimpleFunctions;


namespace ForestryCallouts.Callouts
{
    [CalloutInfo("Animal Blocking Roadway", CalloutProbability.Medium)]
    internal class DeadAnimal : Callout
    {
        //Main
        internal Ped animal;
        private Blip animal_blip;
        private Vector3 spawnpoint;
        private bool on_Scene;
        private bool animal_dealt_with;
        private Vector3 player_veh_rear_position;

        public override bool OnBeforeCalloutDisplayed()
        {
            SimpleFunctions.SPFunctions.DeadAnimalBlockingRoadway(out spawnpoint);
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, spawnpoint);

            CalloutMessage = ("~g~Dead Animal Blocking Roadway Reported");
            CalloutAdvisory = ("~b~Dispatch:~w~ Dead Animal reported blocking roadway, Respond Code 2");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("WE_HAVE ASSISTANCE_REQUIRED_02 IN_OR_ON_POSITION UNITS_RESPOND_CODE_02_02", spawnpoint);
            CalloutPosition = spawnpoint;
            return base.OnBeforeCalloutDisplayed();
        }
        
        public override void OnCalloutDisplayed()
        {
            if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendCalloutDetails(this, "CODE 2", "SAPR");
            Game.LogTrivial("-!!- Forestry Callouts - |Dead Animal| Callout displayed -!!-");

            base.OnCalloutDisplayed();
        }

        public override void OnCalloutNotAccepted()
        {
            if (!CIPluginChecker.IsCalloutInterfaceRunning) LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("OTHER_UNITS_TAKING_CALL");

            base.OnCalloutNotAccepted();
        }
        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("-!!- Forestry Callouts - |DeadAnimalBlock| - Callout accepted -!!-");
            ForestryCallouts.SimpleFunctions.CFunctions.SpawnAnimalPed(out animal, spawnpoint, 0f);
            animal.Kill();

            animal_blip = animal.AttachBlip();
            animal_blip.Color = Color.Red;
            animal_blip.EnableRoute(System.Drawing.Color.Yellow);

            return base.OnCalloutAccepted();
        }
        public override void Process()
        {
            if (animal.Exists())
            {
                if (Game.LocalPlayer.Character.DistanceTo(animal.Position) <= 25f && !on_Scene)
                {
                    if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendMessage(this, "Officer is on scene.");
                    Game.LogTrivial("-!!- Forestry Callouts - |DeadAnimalBlock| - Player arrived to the scene -!!-");
                    Game.DisplayHelp("Use appropriate action to take care of the ~r~dead animal~w~. Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ when finished with the call.");
                    on_Scene = true;
                }

                if (Game.LocalPlayer.Character.DistanceTo(animal.Position) <= 5f && !animal_dealt_with)
                {
                    Game.DisplayHelp("Press ~y~'"+IniSettings.InteractionKey+"'~w~ to call ~g~Animal Control~w~ to your location");
                    if (Game.IsKeyDown(IniSettings.InputInteractionKey))
                    {
                        if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendMessage(this, "Animal control called for dead animal");
                        animal_dealt_with = true;
                        ForestryCallouts.SimpleFunctions.AnimalControl.CallAnimalControl(in animal);
                    }
                }
            }


            //End Script stufs
            if (Game.LocalPlayer.Character.IsDead)
            {
                End();
            }
            if (Game.IsKeyDown(IniSettings.InputEndCalloutKey))
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", spawnpoint);
                Game.DisplayNotification("~g~Dispatch:~w~ All Units, Dead Animal Code 4");
                if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendMessage(this, "Dead Animal code 4");
                Game.LogTrivial("-!!- Forestry Callouts - |DeadAnimalBlock| - Callout was force ended by player -!!-");
                SimpleFunctions.AnimalControl.destroyAnimalControl();
                End();
            }
            base.Process();
        }
        public override void End()
        {
            if (animal.Exists())
            {
                animal.Delete();
            }
            if (animal_blip.Exists())
            {
                animal_blip.Delete();
            }
            
            base.End();
        }
    }
}
