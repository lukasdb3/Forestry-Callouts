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

namespace ForestryCallouts.Callouts
{
    [CalloutInfo("IllegalCamping", CalloutProbability.Medium)]
    internal class IllegalCamping : Callout
    {
        private Vector3 Spawnpoint;
        private float Heading;
        private Ped Suspect;
        private Vehicle SusCamper;
        private Blip SusCamperBlip;
        private bool SuspectIsViolent;
        private int Scenario = new Random().Next(1, 2);
        private int ScenarioOneDialogueChooser = new Random().Next(1, 3);
        private bool OnScene;
        private int Counter;
        private bool DialogueOver;
        private string MaleFemale;
        private bool DialogueStarting;
        private LHandle Pursuit;
        private WeaponDescriptor SuspectsWeapon;
        private bool PursuitStarted;

        public override bool OnBeforeCalloutDisplayed()
        {
            Game.LogTrivial("-!!- Forestry Callouts - |IllegalCamping| - Callout Displayed -!!-");
            SimpleFunctions.CFunctions.SuspectViolChooser(out SuspectIsViolent);
            if (SuspectIsViolent)
            {
                CalloutMessage = ("~g~Illegal Camping Reported");
                CalloutAdvisory = ("~b~Dispatch:~w~ Suspect could be ~r~violent~w~. Respond Code 2");
            }
            else
            {
                CalloutMessage = ("~g~Illegal Camping Reported");
                CalloutAdvisory = ("~b~Dispatch:~w~ Illegal camping in a RV reported. Respond Code 2");
            }

            ForestryCallouts.SimpleFunctions.SPFunctions.IllegalCampingSpawnChooser(out Spawnpoint, out Heading);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT_03 CRIME_DISTURBING_THE_PEACE_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_02_02", Spawnpoint);
            CalloutPosition = Spawnpoint;
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, Spawnpoint);
            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("-!!- Forestry Callouts - |IllegalCamping| - Callout accepted -!!-");
            ForestryCallouts.SimpleFunctions.CFunctions.SpawnCamperVeh(out SusCamper, Spawnpoint, Heading);

            SusCamperBlip = SusCamper.AttachBlip();
            SusCamperBlip.Color = Color.Red;
            SusCamperBlip.EnableRoute(System.Drawing.Color.Yellow);

            ForestryCallouts.SimpleFunctions.CFunctions.SpawnCountryPed(out Suspect, Spawnpoint, 0f);
            ForestryCallouts.SimpleFunctions.CFunctions.PedPersonaChooser(Suspect, true, true);

            Suspect.WarpIntoVehicle(SusCamper, -1);

            if (Suspect.IsMale)
            {
                MaleFemale = "sir";
            }
            else
            {
                MaleFemale = "mam";
            }

            if (Scenario == 1)
            {
                Game.LogTrivial("-!!- Forestry Callouts - |IllegalCamping| - Scenario one choosed!");
            }
            return base.OnCalloutAccepted();
        }
        public override void Process()
        {
            //Scenario one shit
            if (Scenario == 1)
            {
                if (Game.LocalPlayer.Character.DistanceTo(SusCamper) <= 15f && !OnScene)
                {
                    Game.DisplayHelp("Go talk to the driver of the ~r~suspect~w~.");
                    OnScene = true;
                }
                if (Game.LocalPlayer.Character.DistanceTo(Suspect) <= 4f && !DialogueOver)
                {
                    if (!DialogueStarting)
                    {
                        DialogueStarting = true;
                        Game.DisplayHelp("Press ~r~'"+IniSettings.DialogueKey+"'~w~ to talk to the ~r~suspect");
                    }
                    if (Game.IsKeyDown(IniSettings.InputDialogueKey))
                    {
                        if (SuspectIsViolent)
                        {
                            ScenarioOneDialogueMad();
                        }
                        else
                        {
                            ScenarioOneDialogue();
                        }
                    }
                }
            }
            //Scenario two shit

            //End Stuff
            if (Game.IsKeyDown(IniSettings.InputEndCalloutKey)) //If player presses "End" it will forcefully clean the callout up
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", Spawnpoint);
                Game.DisplayNotification("~g~Dispatch:~w~ All Units, Illegal Camping Code 4");
                Game.LogTrivial("-!!- Forestry Callouts - |IllegalCamping| - Callout was force ended by player -!!-");
                End();
            }
            if (PursuitStarted && !LSPD_First_Response.Mod.API.Functions.IsPursuitStillRunning(Pursuit) || Suspect.IsDead || Scenario == 1 && DialogueOver && !SuspectIsViolent)
            {
                if (Ini.IniSettings.EnableEndCalloutHelpMessages)
                {
                    Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                }
            }
            if (Game.LocalPlayer.Character.IsDead)
            {
                if (PursuitStarted)
                {
                    LSPD_First_Response.Mod.API.Functions.ForceEndPursuit(Pursuit);
                }
                Game.LogTrivial("-!!- Forestry Callouts - |IllegalCamping| - Callout was ended due to players death -!!-");
                End();
            }
            base.Process();
        }
        private void ScenarioOneDialogue()
        {
            switch (ScenarioOneDialogueChooser)
            {
                case 1:
                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello, how are you doing today " + MaleFemale + ".");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~g~Suspect:~w~ Im doing great, just doing some camping.");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ it's against the law to camp out here. ");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~g~Suspect:~w~ Are you sure? I dont think it is.");
                    }
                    if (Counter == 5)
                    {
                        Game.DisplaySubtitle("No further dialogue take appropriate acction");
                        DialogueOver = true;
                    }

                    Counter++;
                    break;

                case 2:
                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello, how are you doing today " + MaleFemale + ".");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~g~Suspect:~w~ I'm doing fine, Whats your issue.");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ It's against the law to be camping at this location.");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~g~Suspect:~w~ Ya right, no it's not.");
                    }
                    if (Counter == 5)
                    {
                        Game.DisplaySubtitle("No further dialogue take appropriate acction");
                        DialogueOver = true;
                    }

                    Counter++;
                    break;
            }
        }
        private void ScenarioOneDialogueMad()
        {
            switch (ScenarioOneDialogueChooser)
            {
                case 1:
                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello, how are you doing today " + MaleFemale + ".");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~g~Suspect:~w~ Get the HELL away from me!");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ It's against the law to be camping at this location.");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~g~Suspect:~w~ Fuck off! See you later pig!");
                        Suspect.Tasks.ShuffleToAdjacentSeat().WaitForCompletion(1800);
                        Suspect.Tasks.LeaveVehicle(SusCamper, LeaveVehicleFlags.LeaveDoorOpen).WaitForCompletion(300);
                        Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                        LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                        LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                        LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                        PursuitStarted = true;
                        DialogueOver = true;
                    }

                    Counter++;
                    break;

                case 2:
                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello, how are you doing today " + MaleFemale + ".");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~g~Suspect:~w~ That's none of your business leave me the fuck alone!");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ It's against the law to be camping at this location.");
                        SuspectsWeapon = "WEAPON_COMBATPISTOL";
                        Suspect.Inventory.GiveNewWeapon(SuspectsWeapon, -1, true);
                        Suspect.Inventory.EquippedWeapon = SuspectsWeapon;
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~g~Suspect:~w~ Fuck you! I'm going to kill you!");
                        Suspect.Tasks.LeaveVehicle(SusCamper, LeaveVehicleFlags.LeaveDoorOpen).WaitForCompletion(500);
                        Suspect.Tasks.AimWeaponAt(Game.LocalPlayer.Character, -1).WaitForCompletion(400);
                        Suspect.Tasks.FireWeaponAt(Game.LocalPlayer.Character, -1, FiringPattern.DelayFireByOneSecond);
                        DialogueOver = true;
                    }

                    Counter++;
                    break;
            }
        }
        public override void End()
        {
            if (Suspect.Exists())
            {
                Suspect.Dismiss();
            }
            if (SusCamper.Exists())
            {
                SusCamper.Dismiss();
            }
            if (SusCamperBlip.Exists())
            {
                SusCamperBlip.Delete();
            }
            Game.LogTrivial("-!!- Forestry Callouts - |IllegalCamping| - Cleaned Up -!!-");
            base.End();
        }
    }
}
