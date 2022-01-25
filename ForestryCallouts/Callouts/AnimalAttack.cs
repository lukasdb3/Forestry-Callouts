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
    [CalloutInfo("AnimalAttack", CalloutProbability.Low)]
    internal class AnimalAttack : Callout
    {
        //MainStuff
        private Ped Victim;
        private Blip VictimBlip;
        private Ped Animal;
        private Blip AnimalBlip;
        private Vector3 Spawnpoint;
        private Vector3 AnimalSpawmpoint;
        private float AnimalHeading;
        //Bools
        private bool OnScene;
        private bool AnimalDealtWith;
        private bool SearchForAnimal;
        private bool AnimalFound;
        private bool animalKilled;

        //Timer
        private int timer;
        private bool timerPaused;
        public override bool OnBeforeCalloutDisplayed()
        {
            SimpleFunctions.SPFunctions.AnimalAttatckSpawnChooser(out Spawnpoint, out AnimalSpawmpoint, out AnimalHeading);
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, Spawnpoint);
            
            CalloutMessage = ("~g~Animal Attack Reported");
            CalloutAdvisory = ("~b~Dispatch:~w~ Animal violently attacking a civilian reported, Respond ~r~Code 3~w~");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("WE_HAVE ASSISTANCE_REQUIRED_02 IN_OR_ON_POSITION UNITS_RESPOND_CODE_03_02", Spawnpoint);
            CalloutPosition = Spawnpoint;
            return base.OnBeforeCalloutDisplayed();
        }

        public override void OnCalloutDisplayed()
        {
            if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendCalloutDetails(this, "CODE 3", "SAPR");
            Game.LogTrivial("-!!- Forestry Callouts - |AnimalAttack| Callout displayed -!!-");

                base.OnCalloutDisplayed();
        }

        public override void OnCalloutNotAccepted()
        {
            if (!CIPluginChecker.IsCalloutInterfaceRunning) LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("OTHER_UNITS_TAKING_CALL");

                base.OnCalloutNotAccepted();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("-!!- Forestry Callout - |AnimalAttack| - Callout Accepted");
            SimpleFunctions.CFunctions.SpawnCountryPed(out Victim, Spawnpoint, new Random().Next(0, 361));
            VictimBlip = Victim.AttachBlip();
            VictimBlip.EnableRoute(Color.Yellow);
            VictimBlip.Color = Color.OrangeRed;
            Victim.Health = 10;

            Animal = new Ped("a_c_mtlion", AnimalSpawmpoint, AnimalHeading);
            Animal.Tasks.Wander();
            Animal.IsPersistent = true;
            Animal.BlockPermanentEvents = true;
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (!Animal)
            {
                End();
                return;
            }

            if (Game.LocalPlayer.Character.Position.DistanceTo(Victim) <= 8f && !SearchForAnimal)
            {
                Game.LogTrivial("-!!- Forestry Callout - |AnimalAttack| - Main Process Started -!!-");
                if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendMessage(this, "Officer is on scene.");
                Game.DisplayNotification("The ~r~Mountain Lion~w~ left the area, the animal has been blipped");
                VictimBlip.IsRouteEnabled = false;
                SearchForAnimal = true;
                AnimalBlip = Animal.AttachBlip();
                AnimalBlip.Color = Color.Red;
                OnScene = true;
            }
            if (!AnimalFound && OnScene && Game.LocalPlayer.Character.Position.DistanceTo(Animal.Position) <= 20f && SearchForAnimal)
            {
                Animal.Tasks.FightAgainst(Game.LocalPlayer.Character);
                AnimalFound = true;
            }
            if (Animal.Health == 0 && !animalKilled)
            {
                Animal.Health = 1;
                Animal.Kill();
                animalKilled = true;
                if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendMessage(this, "Animal has been neutralized, one injured victim.");
                Game.LogTrivial("-!!- Forestry Callouts - |AnimalAttack| - Animal Killed -!!-");
            }
            if (animalKilled && !AnimalDealtWith)
            {
                AnimalBlip.Delete();
                Game.DisplayNotification("Tend to the injured ~o~Victim~w~");
                AnimalDealtWith = true;
            }

            if (AnimalDealtWith)
            {
                if (!timerPaused)
                {
                    timer++;
                }

                if (timer == 1500)
                {
                    Game.DisplayNotification("Press ~r~'" + IniSettings.InteractionKey + "'~w~ to call ~g~Animal Control~w~ for the dead ~r~animal~w~");
                }

                if (Game.IsKeyDown(IniSettings.InputInteractionKey))
                {
                    SimpleFunctions.AnimalControl.CallAnimalControl(in Animal);
                    if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendMessage(this, "Animal control called for dead animal");
                }
            }
            //End Script stufs
            if (Game.LocalPlayer.Character.IsDead)
            {
                End();
            }
            if (Game.IsKeyDown(IniSettings.InputEndCalloutKey))
            {
                End();
            }

            base.Process();
        }

        public override void End()
        {
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", Spawnpoint);
            Game.DisplayNotification("~g~Dispatch:~w~ All Units, Animal Attack Code 4");
            if (CIPluginChecker.IsCalloutInterfaceRunning)
            {
                MFunctions.SendMessage(this, "Animal Attack code 4");
            }
            Game.LogTrivial("-!!- Forestry Callouts - |AnimalAttack| - Callout was force ended by player -!!-");
            SimpleFunctions.AnimalControl.destroyAnimalControl();
            
            if (Animal)
            {
                Animal.Dismiss();
            }
            if (Victim)
            {
                Victim.Dismiss();
            }
            if (VictimBlip)
            {
                VictimBlip.Delete();
            }
            if (AnimalBlip)
            {
                AnimalBlip.Delete();
            }
            Game.LogTrivial("-!!- Forestry Callout - |AnimalAttack| - Cleaned Up -!!-");

            base.End();
        }
    }
}
