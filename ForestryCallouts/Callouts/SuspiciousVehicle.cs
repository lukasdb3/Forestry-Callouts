using System;
using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using System.Drawing;
using ForestryCallouts.Ini;
using ForestryCallouts.SimpleFunctions;
using ForestryCallouts.SimpleFunctions.Logger;

namespace ForestryCallouts.Callouts
{
    [CalloutInfo("SuspiciousVehicle", CalloutProbability.Medium)]
    internal class SuspiciousVehicle : Callout
    {
        //Main
        private Vector3 Spawnpoint;
        private Vector3 SearchArea;
        private Vehicle SusVehicle;
        private float Heading;
        private Blip SusVehAreaBlip;
        private Blip SusVehicleBlip;
        private Ped Suspect;
        private Ped Prostitute;
        private LHandle Pursuit;
        private int Counter;

        //Stuff
        private bool SuspectIsViolent;
        private int Scenario = new Random().Next(1, 4);
        private string MaleFemale;
        //Scenario One Stuff
        private int ScenarioOneChooser = new Random().Next(1, 2);
        private int OnFootOrVehicle = new Random().Next(1, 2);
        private int IsSusVehicleOn = new Random().Next(1, 2);
        private int ProstituteJoinPursuit = new Random().Next(1, 4);
        private bool SuspectIsDeadOrCuffed = false;
        private int ScenarioOneDialogueChooser = new Random().Next(1, 3);
        private bool ProstituteReadyForDialgue = false;
        private bool DialogueOver = false;
        private bool PursuitOver = false;
        private bool ProstituteArrestedOrDead = false;
        private bool ProstitutePursuitStarted = false;
        private bool ProstituteJoinedPursuit = false;
        //Scenario 2 Stuff
        private bool S2PursuitStarted = false;
        private int VehicleHavePassenger = new Random().Next(1, 4);
        private Ped Passenger;
        //Scenario 3
        private bool S3ReadyForDialogue;
        private int ScenarioThreeDialogueChooser = new Random().Next(1, 3);
        private bool SuspectLeftVehicle = false;
        private int S3GetOutOrNot = new Random().Next(1, 3);
        private bool S3PursuitStarted = false;

    //If-Loop-Stopper/Checkers
    private bool SearchingForCar;
        private bool OnScene;
        

        public override bool OnBeforeCalloutDisplayed()
        {
            SimpleFunctions.CFunctions.SuspectViolChooser(out SuspectIsViolent);
            SimpleFunctions.SPFunctions.SuspiciousVehicleSpawnChooser(out Spawnpoint, out Heading);
            if (Scenario == 1)
            {
                CalloutMessage = ("~g~Suspicious Vehicle Reported");
                CalloutAdvisory = ("~b~Dispatch:~w~ Reports of possible prostitution. Respond Code 2");
            }
            if (VehicleHavePassenger == 1)
            {
                CalloutMessage = ("~g~Suspicious Vehicle Reported");
                CalloutAdvisory = ("~b~Dispatch:~w~ Reports of possible suspects. Respond Code 2");
            }
            CalloutMessage = ("~g~Suspicious Vehicle Reported");
            CalloutAdvisory = ("~b~Dispatch:~w~ Suspect may be ~r~violent~w~ use caution. Respond Code 2");
            CalloutPosition = Spawnpoint;
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, Spawnpoint);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT_03 CRIME_DISTURBING_THE_PEACE_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_02_02", Spawnpoint);
            return base.OnBeforeCalloutDisplayed();
        }
        
        public override void OnCalloutDisplayed()
        {
            if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendCalloutDetails(this, "CODE 2", "SAPR");
           LFunctions.Log(this, "Callout displayed!");

            base.OnCalloutDisplayed();
        }

        public override void OnCalloutNotAccepted()
        {
            if (!CIPluginChecker.IsCalloutInterfaceRunning) LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("OTHER_UNITS_TAKING_CALL");

            base.OnCalloutNotAccepted();
        }

        public override bool OnCalloutAccepted()
        {
            LFunctions.Log(this, "Callout accepted!");
            var position = Spawnpoint;
            SearchArea = position.Around2D(30f, 60f);
            SusVehAreaBlip = new Blip(SearchArea, 65f) { Color = Color.Yellow, Alpha = .5f };
            SusVehAreaBlip.EnableRoute(Color.Yellow);
            SimpleFunctions.CFunctions.SpawnOffroadCar(out SusVehicle, Spawnpoint, Heading);

            if (Scenario == 1)
            {
                SimpleFunctions.CFunctions.SpawnSuspiciousVehiclePed(out Suspect, Spawnpoint, 0);
            }
            else
            {
                SimpleFunctions.CFunctions.SpawnCountryPed(out Suspect, Spawnpoint, 0);
            }
            SimpleFunctions.CFunctions.PedPersonaChooser(Suspect, true, true);
            Suspect.WarpIntoVehicle(SusVehicle, -1);
            if (Suspect.IsMale)
            {
                MaleFemale = "sir";
            }
            if (Suspect.IsFemale)
            {
                MaleFemale = "mam";
            }
            if (Scenario == 1)
            {
                SimpleFunctions.CFunctions.SpawnHookerPed(out Prostitute, Spawnpoint, 0);
                SimpleFunctions.CFunctions.PedPersonaChooser(Prostitute, false, true);
                Prostitute.WarpIntoVehicle(SusVehicle, 0);
                //Prostitute.Tasks.PlayAnimation("bullshit", "mini@prostitutes@sexnorm_veh", 8, 8, -1, 0, AnimationFlags.Loop);
                //Rage.Native.NativeFunction.Natives.TASK_PLAY_ANIM("hello", "mini@prostitutes@sexnorm_veh");
            }
            if (Scenario == 2 || Scenario == 3)
            {
                if (VehicleHavePassenger == 1)
                {
                    Passenger = new Ped("cs_omega", Spawnpoint, 0f);
                    Passenger.WarpIntoVehicle(SusVehicle, 0);
                    SimpleFunctions.CFunctions.PedPersonaChooser(Passenger, true, true);
                    Passenger.IsPersistent = true;
                    Passenger.BlockPermanentEvents = true;
                }
                if (VehicleHavePassenger != 1)
                {
                    LFunctions.Log(this, "No passenger selceted!");
                }
            }
            if (IsSusVehicleOn == 1)
            {
                SusVehicle.IsEngineOn = true;
            }
            if (IsSusVehicleOn != 1)
            {
                SusVehicle.IsEngineOn = false;
            }
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (Game.LocalPlayer.Character.Position.DistanceTo(Spawnpoint) <= 85f && !SearchingForCar)
            {
                Game.DisplayNotification("Look for the ~r~Suspicious Vehicle~w~ in the ~y~Yellow~w~ Circle");
                SearchingForCar = true;
                LFunctions.Log(this, "Scenario = " +Scenario+"!");
                SusVehAreaBlip.IsRouteEnabled = false;
            }
            if (Game.LocalPlayer.Character.Position.DistanceTo(SusVehicle.Position) <= 20f && !OnScene)
            {
                switch (Scenario)
                {
                    case 1:
                        if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendMessage(this, "Officer is on scene.");
                        LFunctions.Log(this, "Vehicle found!");
                        SusVehAreaBlip.Delete();
                        SusVehicleBlip = SusVehicle.AttachBlip();
                        SusVehicleBlip.Color = Color.Red;
                        SusVehicleBlip.EnableRoute(Color.Yellow);
                        OnScene = true;
                        LFunctions.Log(this, "ScenarioOneChooser = " +ScenarioOneChooser+"!");
                        if (ScenarioOneChooser == 1)
                        {
                            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                            LFunctions.Log(this, "OnFootOrVehicle = " +OnFootOrVehicle+"!");
                            if (OnFootOrVehicle == 1)
                            {
                                Game.DisplaySubtitle("~r~Suspect:~w~ OH SHITT RUN!", 2500);
                                Suspect.Tasks.LeaveVehicle(SusVehicle, LeaveVehicleFlags.LeaveDoorOpen).WaitForCompletion(2500);
                                SusVehicle.IsDriveable = false;
                                Rage.Native.NativeFunction.Natives.TASK_REACT_AND_FLEE_PED(Suspect, Game.LocalPlayer.Character);
                            }
                            else
                            {
                                Rage.Native.NativeFunction.Natives.TASK_REACT_AND_FLEE_PED(Game.LocalPlayer.Character);
                            }
                            Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                            LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                            if (ProstituteJoinPursuit == 1)
                            {
                                LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Prostitute);
                                ProstituteJoinedPursuit = true;
                            }
                            else
                            {
                                LFunctions.Log(this, "Prostitute not running!");
                            }
                            SusVehicleBlip.Delete();
                            Game.DisplayHelp("Pursue the ~r~SUSPECT~w~");
                        }
                        break;

                    case 2:
                        if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendMessage(this, "Officer is on scene.");
                        LFunctions.Log(this, "Vehicle found!");
                        SusVehAreaBlip.Delete();
                        OnScene = true;
                        Game.DisplaySubtitle("~r~Suspect:~w~ OH SHITT RUN!", 2500);
                        Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                        LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                        LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                        LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                        S2PursuitStarted = true;
                        break;

                    case 3:
                        if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendMessage(this, "Officer is on scene.");
                        LFunctions.Log(this, "Vehicle found!");
                        SusVehAreaBlip.Delete();
                        SusVehicleBlip = SusVehicle.AttachBlip();
                        SusVehicleBlip.Color = Color.Red;
                        SusVehicleBlip.EnableRoute(Color.Yellow);
                        if (!OnScene)
                        {
                            OnScene = true;
                            Game.DisplayHelp("Go talk to the ~r~driver~w~ of the ~r~suspicious vehicle");
                        }
                        break;
                }
            }
            //Scenario one stufffffffffs for prostitute not joining in on the pursuit, pretty much just tells the player to go talk to the passenger after the suspect chase and starts dialgue then
            // choses if the prostitute will take off in the suspects vehicle and start a pursuit or stand there and be arrested for prostitution.
            if (Scenario == 1 && ProstituteJoinPursuit != 1 && OnFootOrVehicle == 1)
            {
                if (!Suspect.IsAlive && !SuspectIsDeadOrCuffed || Suspect.IsCuffed && !SuspectIsDeadOrCuffed)
                {
                    Game.DisplayNotification("Go talk to the passenger of the ~r~suspicious vehicle~w~");
                    SuspectIsDeadOrCuffed = true;
                }
                if (SuspectIsDeadOrCuffed && Game.LocalPlayer.Character.Position.DistanceTo(Prostitute.Position) <= 5f)
                {
                    if (!PursuitOver)
                    {
                        Game.DisplayHelp("Press ~r~'"+IniSettings.InteractionKey+"'~w~ to ask the passengar to leave the vehicle", 5000);
                        PursuitOver = true;
                    }
                    if (Game.IsKeyDown(IniSettings.InputInteractionKey) && !ProstituteReadyForDialgue)
                    {
                        LFunctions.Log(this, "ScenarioOneDialogueChooser = " +ScenarioOneDialogueChooser+"!");
                        Game.DisplaySubtitle("~y~Player:~w~ Hello mam please exit the vehicle");
                        Prostitute.Tasks.LeaveVehicle(SusVehicle, LeaveVehicleFlags.None).WaitForCompletion(2500);
                        Prostitute.Heading = Game.LocalPlayer.Character.Heading + 180f;
                        Prostitute.Tasks.StandStill(-1);
                        ProstituteReadyForDialgue = true;
                        Game.DisplayNotification("To continue dialogue with the ~r~passenger~w~ press ~r~'"+IniSettings.DialogueKey+"'~w~");
                    }
                }
                if (ProstituteReadyForDialgue && !DialogueOver)
                {
                    if (Game.IsKeyDown(IniSettings.InputDialogueKey))
                    {
                        Prostitute.Heading = Game.LocalPlayer.Character.Heading + 180f;
                        ScenarioOneDialogue();
                    }
                }
                if (ScenarioOneDialogueChooser == 2)
                {
                    if (Prostitute.IsCuffed || !Prostitute.IsAlive)
                    {
                        ProstituteArrestedOrDead = true;
                    }
                }
                if (ProstituteArrestedOrDead)
                {
                    if (Ini.IniSettings.EnableEndCalloutHelpMessages)
                    {
                        Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                    }
                }
                if (ProstitutePursuitStarted && !LSPD_First_Response.Mod.API.Functions.IsPursuitStillRunning(Pursuit))
                {
                    if (Ini.IniSettings.EnableEndCalloutHelpMessages)
                    {
                        Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                    }
                }
                if (ProstituteJoinPursuit == 1 && ProstituteJoinedPursuit)
                {
                    if (!LSPD_First_Response.Mod.API.Functions.IsPursuitStillRunning(Pursuit))
                    {
                        if (Ini.IniSettings.EnableEndCalloutHelpMessages)
                        {
                            Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                        }
                    }
                }
            }

            //Scenario 2 shit
            if (Scenario == 2)
            {
                if (Suspect.IsDead || Suspect.IsCuffed)
                {
                    if (Ini.IniSettings.EnableEndCalloutHelpMessages)
                    {
                        Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                    }
                }
                if (S2PursuitStarted && !LSPD_First_Response.Mod.API.Functions.IsPursuitStillRunning(Pursuit))
                {
                    if (Ini.IniSettings.EnableEndCalloutHelpMessages)
                    {
                        Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                    }
                }

            }
            //Scenario 3 shit
            if (Scenario == 3)
            {
                if (Game.LocalPlayer.Character.Position.DistanceTo(Suspect) <= 5f && !S3ReadyForDialogue)
                {
                    LFunctions.Log(this, "ScenarioThreeDialogueChooser = " +ScenarioThreeDialogueChooser+"!");
                    Game.DisplayHelp("Press ~r~'"+IniSettings.DialogueKey+"'~w~ to talk to the ~r~driver~w~", false);
                    S3ReadyForDialogue = true;
                }
                if (Game.IsKeyDown(IniSettings.InputDialogueKey) && !DialogueOver)
                {
                    ScenarioThreeDialogue();
                }
                if (DialogueOver && ScenarioThreeDialogueChooser == 1)
                {
                    if (VehicleHavePassenger != 1)
                    {
                        if (Ini.IniSettings.EnableEndCalloutHelpMessages)
                        {
                            Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                        }
                    }
                    if (VehicleHavePassenger == 1)
                    {
                        if (S3PursuitStarted)
                        {
                            if (!LSPD_First_Response.Mod.API.Functions.IsPursuitStillRunning(Pursuit))
                            {
                                if (Ini.IniSettings.EnableEndCalloutHelpMessages)
                                {
                                    Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                                }
                            }
                        }
                        if (SuspectLeftVehicle)
                        {
                            if (Ini.IniSettings.EnableEndCalloutHelpMessages)
                            {
                                Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                            }
                        }
                    }
                }
                if (DialogueOver && ScenarioThreeDialogueChooser == 2)
                {
                    if (!LSPD_First_Response.Mod.API.Functions.IsPursuitStillRunning(Pursuit))
                    {
                        if (Ini.IniSettings.EnableEndCalloutHelpMessages)
                        {
                            Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                        }
                    }
                }
            }

            //End Stuffs
            if (Game.IsKeyDown(System.Windows.Forms.Keys.End)) //If player presses "End" it will forcefully clean the callout up
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", Spawnpoint);
                Game.DisplayNotification("~g~Dispatch:~w~ All Units, Suspicious Vehicle Code 4");
                if (CIPluginChecker.IsCalloutInterfaceRunning)
                {
                    MFunctions.SendMessage(this, "Reckless Driver code 4");
                }
                LFunctions.Log(this, "Callout force ended by player!");
                End();
            }
            if (Game.LocalPlayer.IsDead) //If suspect is dead or player or suspect not exist callout ends
            {
                LFunctions.Log(this, "Callout ended, player died");
                End();
            }
            if (ProstituteJoinPursuit == 1 && OnFootOrVehicle != 1)
            {
                if(!LSPD_First_Response.Mod.API.Functions.IsPursuitStillRunning(Pursuit))
                {
                    if (Ini.IniSettings.EnableEndCalloutHelpMessages)
                    {
                        Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                    }
                }
            }
            
            base.Process();
        }

        private void ScenarioThreeDialogue()
        {
            switch (ScenarioThreeDialogueChooser)
            {
                case 1:
                    if (VehicleHavePassenger != 1)
                    {
                        if (Counter == 1)
                        {
                            Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + " what are you doing out here?");
                        }
                        if (Counter == 2)
                        {
                            Game.DisplaySubtitle("~r~Suspect:~w~ Ohh nothing.. I just came out here to relax");
                        }
                        if (Counter == 3)
                        {
                            Game.DisplaySubtitle("~y~Player:~w~ Okay, this vehicle was reported as suspicious, do you mind talking to me for a second?");
                        }
                        if (Counter == 4)
                        {
                            if (S3GetOutOrNot == 1)
                            {
                                Game.DisplaySubtitle("~r~Suspect:~w~ Yes that is fine officer");
                            }
                            else
                            {
                                Game.DisplaySubtitle("~r~Suspect:~w~ No, I have the right to remain silent and stay in my vehicle");
                            }
                        }
                        if (Counter == 5)
                        {
                            if (S3GetOutOrNot == 1)
                            {
                                Game.DisplaySubtitle("~r~Player:~w~ Okay, please step out of the vehicle");
                                Suspect.Tasks.LeaveVehicle(SusVehicle, LeaveVehicleFlags.None).WaitForCompletion(1500);
                                Suspect.Tasks.StandStill(-1);
                                Suspect.Heading = Game.LocalPlayer.Character.Heading + 180f;
                                SuspectLeftVehicle = true;
                                Game.DisplaySubtitle("No further dialogue take appropriate action");
                            }
                            else
                            {
                                Game.DisplaySubtitle("No further dialogue take appropriate action");
                            }
                            DialogueOver = true;
                        }

                        Counter++;
                    }
                    if (VehicleHavePassenger == 1)
                    {
                        if (Counter == 1)
                        {
                            Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + " what are you two doing out here?");
                        }
                        if (Counter == 2)
                        {
                            Game.DisplaySubtitle("~r~Suspect:~w~ Absolutely nothing officer.. just relaxing");
                        }
                        if (Counter == 3)
                        {
                            Game.DisplaySubtitle("~y~Player:~w~ Okay, this vehicle was reported as suspicious, do you mind talking to me for a second?");
                        }
                        if (Counter == 4)
                        {
                            if (S3GetOutOrNot == 1)
                            {
                                Game.DisplaySubtitle("~r~Suspect:~w~ Yes that is fine officer");
                            }
                            else
                            {
                                Game.DisplaySubtitle("~r~Suspect:~w~ No, I have the right to remain silent and stay in my vehicle");
                            }
                        }
                        if (Counter == 5)
                        {
                            if (S3GetOutOrNot == 1)
                            {
                                Game.DisplaySubtitle("~r~Player:~w~ Okay, please step out of the vehicle");
                                Suspect.Tasks.LeaveVehicle(SusVehicle, LeaveVehicleFlags.None).WaitForCompletion(1500);
                                Suspect.Tasks.StandStill(-1);
                                Suspect.Heading = Game.LocalPlayer.Character.Heading + 180f;
                                SuspectLeftVehicle = true;
                                S3PursuitStarted = false;
                                Game.DisplaySubtitle("No further dialogue take appropriate action");
                            }
                            else
                            {
                                Game.DisplaySubtitle("~r~Suspect:~w~ SEEYA.. LATER! PIG!");
                                Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                                LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                                LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                                LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                                LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Passenger);
                                S3PursuitStarted = true;
                            }
                            DialogueOver = true;
                        }

                        Counter++;
                    }
                    break;

                case 2:
                    if (VehicleHavePassenger != 1)
                    {
                        if (Counter == 1)
                        {
                            Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + " what are you doing out here?");
                        }
                        if (Counter == 2)
                        {
                            Game.DisplaySubtitle("~r~Suspect:~w~ Why does that concern you? I can be out here doing what the fuck ever I want");
                        }
                        if (Counter == 3)
                        {
                            Game.DisplaySubtitle("~y~Player:~w~ This vehicle was called in suspicious, you mind talking to me for a minute?");
                        }
                        if (Counter == 4)
                        {
                            Game.DisplaySubtitle("~r~Suspect:~w~ NO. FUCK YOU! PIG!");
                            Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                            LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                            DialogueOver = true;
                        }

                        Counter++;
                    }
                    if (VehicleHavePassenger == 1)
                    {
                        if (Counter == 1)
                        {
                            Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + " what are you two doing out here?");
                        }
                        if (Counter == 2)
                        {
                            Game.DisplaySubtitle("~r~Suspect:~w~ Why does that concern you? We can be out here doing what the fuck we want");
                        }
                        if (Counter == 3)
                        {
                            Game.DisplaySubtitle("~y~Player:~w~ This vehicle was called in suspicious, you mind talking to me for a minute?");
                        }
                        if (Counter == 4)
                        {
                            Game.DisplaySubtitle("~r~Suspect:~w~ NO. FUCK YOU! PIG!");
                            Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                            LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Passenger);
                            DialogueOver = true;
                        }

                        Counter++;
                    }
                    break;
            }
        }

        private void ScenarioOneDialogue()
        {
            switch (ScenarioOneDialogueChooser)
            {
                case 1:
                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello mam what were you doing with the driver out here.");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Prostitute:~w~ Ohh nothing.. He just picked me up from my spot and drove here.");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ You do relize prostitution is a crime. Correct?");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Prostitute:~w~ Yes officer I know but it's the only thing that can pay the bills, please dont arrest me!");
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
                        Game.DisplaySubtitle("~y~Player:~w~ Hello mam what are you doing with the driver out here?");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Prostitute:~w~ Why does that concern you? We can be out here doing what we want.");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ This vehicle was called in as a suspicious vehicle and the caller said the driver may have picked up a prostitute, is this true?");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Prostitute:~w~ No.. Uh.. Im just going to leave. Bye");
                        Prostitute.Tasks.EnterVehicle(SusVehicle, 0).WaitForCompletion(3000);
                        SusVehicle.IsDriveable = true;
                        Prostitute.Tasks.ShuffleToAdjacentSeat(SusVehicle).WaitForCompletion(4000);

                        Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                        LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                        LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Prostitute);
                        DialogueOver = true;
                        ProstitutePursuitStarted = true;
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
            if (Prostitute.Exists())
            {
                Prostitute.Dismiss();
            }
            if (SusVehicle.Exists())
            {
                SusVehicle.Dismiss();
            }
            if (SusVehAreaBlip.Exists())
            {
                SusVehAreaBlip.Delete();
            }
            if (SusVehicleBlip.Exists())
            {
                SusVehicleBlip.Delete();
            }
            LFunctions.Log(this, "Cleaned up!");
            base.End();
        }
    }
}
