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
    [CalloutInfo("RecklessDriver", CalloutProbability.Medium)]
    internal class RecklessDriver : Callout
    {
        //MainStuff
        private Ped Suspect;
        private Blip SuspectBlip;
        private LHandle Pursuit;
        private Vehicle susVehicle;
        private Vector3 Spawnpoint;
        //Bools
        private bool OnScene;
        private bool CalloutStarted;
        private bool SuspectIsViolent;
        private bool PursuitStarted;
        //Ints
        private int ScenarioChoice = new Random().Next(1, 6);
        private int ScenarioOnSusAttack = new Random().Next(1, 4);

        public override bool OnBeforeCalloutDisplayed()
        {
            Game.LogTrivial("-!!- Forestry Callouts - |RecklessDriver| - Callout Displayed");
            SimpleFunctions.CFunctions.SuspectViolChooser(out SuspectIsViolent);
            SimpleFunctions.SPFunctions.SpawnChooser(out Spawnpoint);
            CalloutMessage = ("~g~Reckless Driver Reported");
            CalloutAdvisory = ("~b~Dispatch:~w~ Reckless driver reported, pullover the suspect. Respond ~r~Code 3~w~");
            CalloutPosition = Spawnpoint;
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, Spawnpoint);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT_03 CRIME_RESIST_ARREST_02 IN_OR_ON_POSITION UNITS_RESPOND_CODE_03_02", Spawnpoint);
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("-!!- Forestry Callouts - |RecklessDriver| - Callout Accepted");
            SimpleFunctions.CFunctions.SpawnOffroadCar(out susVehicle, Spawnpoint, 0);
            SimpleFunctions.CFunctions.SpawnCountryPed(out Suspect, Spawnpoint, 0);
            SimpleFunctions.CFunctions.PedPersonaChooser(Suspect, true, true);
            Suspect.WarpIntoVehicle(susVehicle, -1);

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.EnableRoute(Color.Yellow);
            SuspectBlip.Color = Color.Red;
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (!CalloutStarted && Game.LocalPlayer.Character.DistanceTo(Suspect) <= 300f) //Suspect starts driving
            {
                Game.LogTrivial("-!!- Forestry Callouts - |RecklessDriver| - Main Process Started -!!-");
                Suspect.Tasks.CruiseWithVehicle(susVehicle, 25f, VehicleDrivingFlags.Normal);
                CalloutStarted = true;

            }
            if (!OnScene && Game.LocalPlayer.Character.DistanceTo(Suspect) <= 30f) //Start main code
            {
                OnScene = true;
                LSPD_First_Response.Mod.API.Functions.SetPedCanBePulledOver(Suspect, false);
                Game.LogTrivial("-!!- Forestry Callouts - |RecklessDriver| - ScenarioChoice, Case: " + ScenarioChoice + " -!!-");
                switch (ScenarioChoice) //switch statement can choose 2 differnt things, 1: wait for player to approach suspect on traffic stop, 2: Start a pursuit as soon as player is behind suspect. 
                {
                    case 1:
                        LSPD_First_Response.Mod.API.Functions.SetPedCanBePulledOver(Suspect, true);
                        LSPD_First_Response.Mod.API.Events.OnPulloverOfficerApproachDriver += Events_OnPulloverOfficerApproachDriver;
                        Game.DisplayHelp("Pullover The ~r~Suspect~w~", false);
                        break;

                    case 2:
                        LSPD_First_Response.Mod.API.Functions.SetPedCanBePulledOver(Suspect, true);
                        LSPD_First_Response.Mod.API.Events.OnPulloverOfficerApproachDriver += Events_OnPulloverOfficerApproachDriver;
                        Game.DisplayHelp("Pullover The ~r~Suspect~w~", false);
                        break;

                    case 3:
                        LSPD_First_Response.Mod.API.Functions.SetPedCanBePulledOver(Suspect, true);
                        LSPD_First_Response.Mod.API.Events.OnPulloverOfficerApproachDriver += Events_OnPulloverOfficerApproachDriver;
                        Game.DisplayHelp("Pullover The ~r~Suspect~w~", false);
                        break;

                    case 4:
                        Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                        LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                        LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                        LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                        SuspectBlip.Delete();
                        PursuitStarted = true;
                        break;
                    case 5:
                        LSPD_First_Response.Mod.API.Functions.SetPedCanBePulledOver(Suspect, true);
                        Game.DisplayHelp("Pullover The ~r~Suspect~w~", false);
                        break;
                } 
            }
            //End Script Shit
            if (Game.LocalPlayer.IsDead) //If suspect is dead or player or suspect not exist callout ends
            {
                Game.LogTrivial("-!!- Forestry Callouts - |RecklessDriver| - Callout was ended due to players death -!!-");
                End();
            }
            if (PursuitStarted && !LSPD_First_Response.Mod.API.Functions.IsPursuitStillRunning(Pursuit))
            {
                if (Ini.IniSettings.EnableEndCalloutHelpMessages)
                {
                    Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                }
            }
            if (Suspect.IsDead)
            {
                if (Ini.IniSettings.EnableEndCalloutHelpMessages)
                {
                    Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                }
            }
            if (Game.IsKeyDown(IniSettings.InputEndCalloutKey)) //If player presses "End" it will forcefully clean the callout up
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", Spawnpoint);
                Game.DisplayNotification("~g~Dispatch:~w~ All Units, Reckless Driver Code 4");
                Game.LogTrivial("-!!- Forestry Callouts - |Reckless Driver| - Callout was force ended by player -!!-");
                End();
            }
            base.Process();
        }

        private void Events_OnPulloverOfficerApproachDriver(LHandle handle)//1st choice for ScenarioAction
        {
            var RightPed = LSPD_First_Response.Mod.API.Functions.GetPulloverSuspect(handle);
            if (RightPed == Suspect)
            {
                if (SuspectIsViolent)
                {
                    switch (ScenarioOnSusAttack)
                    {
                        case 1:
                            SuspectBlip.Delete();
                            Suspect.Inventory.GiveNewWeapon("WEAPON_PISTOL", -1, true);
                            Suspect.Tasks.LeaveVehicle(susVehicle, LeaveVehicleFlags.LeaveDoorOpen).WaitForCompletion(20000);
                            Suspect.Tasks.AimWeaponAt(Game.LocalPlayer.Character, 400).WaitForCompletion(200);
                            Suspect.Tasks.FightAgainst(Game.LocalPlayer.Character);
                            LSPD_First_Response.Mod.API.Functions.ForceEndCurrentPullover();
                            break;

                        case 2:
                            SuspectBlip.Delete();
                            Suspect.Inventory.GiveNewWeapon("WEAPON_KNIFE", -1, true);
                            Suspect.Tasks.LeaveVehicle(susVehicle, LeaveVehicleFlags.LeaveDoorOpen).WaitForCompletion(2000);
                            Suspect.Tasks.FightAgainst(Game.LocalPlayer.Character);
                            LSPD_First_Response.Mod.API.Functions.ForceEndCurrentPullover();
                            break;

                        case 3:
                            SuspectBlip.Delete();
                            Suspect.Inventory.GiveNewWeapon("WEAPON_KNIFE", -1, true);
                            Suspect.Tasks.LeaveVehicle(susVehicle, LeaveVehicleFlags.LeaveDoorOpen).WaitForCompletion(2000);
                            Suspect.Tasks.FightAgainst(Game.LocalPlayer.Character);
                            LSPD_First_Response.Mod.API.Functions.ForceEndCurrentPullover();
                            break;
                    } 
                }
                if (!SuspectIsViolent)
                {
                    SuspectBlip.Delete();
                    LSPD_First_Response.Mod.API.Functions.ForceEndCurrentPullover();
                    LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");    

                    Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                    LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                    LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                    PursuitStarted = true;
                }   
            }
        }


        public override void End()//Clean up script
        {
            Game.SetRelationshipBetweenRelationshipGroups("COP", "SUSP", Relationship.Dislike);

            if (Suspect.Exists())
            {
                Suspect.Dismiss();
            }
            if (SuspectBlip.Exists())
            {
                SuspectBlip.Delete();
            }
            if (susVehicle.Exists())
            {
                susVehicle.Dismiss();
            }
            Game.LogTrivial("-!!- Forestry Callouts - |RecklessDriver| - Cleaned Up -!!-");
            base.End();
        }
    }
}

