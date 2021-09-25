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
    [CalloutInfo("WreckedVehicle", CalloutProbability.Medium)]
    internal class WreckedVehicle : Callout
    {
        //ints
        private int Scenario = new Random().Next(1,3);
        private int S1DialogueChooser = new Random().Next(1,5);
        private int S1DialogueChooserViolent = new Random().Next(1,4);
        private int S2Opts = new Random().Next(1,3);
        private int S2DialougeChooser = new Random().Next(1,4);
        private int Counter;
        private int SpawnpointChoosed;
        private int AttackOrPursuit = new Random().Next(1, 3);
        //bools
        private bool STPpedArrested;
        private bool OnScene;
        private bool DialogueChoosed;
        private bool SuspectIsViolent;
        private bool DialogueOver;
        private bool FoundSuspect;
        private bool ScenarioTwoDialougeChoosed;
        private bool IsPursuitCreated;
        private bool PedOnFootChoosed;
        private bool PedWanderingAreaChoosed;
        private bool VehFoundFirst;
        private bool SusFoundFirst;
        private bool FirstSearchArea;
        //vector3s
        private Vector3 Spawnpoint;
        private Vector3 PedSpawnpoint;
        private Vector3 PedWanderSpawnpoint;
        private Vector3 SearchArea;
        //pedss
        private Ped Suspect;
        //vehicles
        private Vehicle susVehicle;
        //blips
        private Blip SuspectBlip;
        private Blip SuspectAreaBlip;
        private Blip SusVehicleBlip;
        //strings
        private string MaleFemale;
        //lhandles
        private LHandle Pursuit;
        //floats
        private float Heading;
        private float PedHeading;
        //timer shit
        private bool TimerSet;
        private float timer;
        public override bool OnBeforeCalloutDisplayed()
        {
            SimpleFunctions.CFunctions.SuspectViolChooser(out SuspectIsViolent);
            SimpleFunctions.SPFunctions.WreckedVehicleSpawnChooser(out Spawnpoint, out Heading, out PedSpawnpoint, out PedHeading, out SpawnpointChoosed, out PedWanderSpawnpoint);
            Game.LogTrivial("-!!- Forestry Callouts - |WreckedVehicle| - SpawnpointChoosed, Case: " + SpawnpointChoosed + " -!!-");
            Game.LogTrivial("-!!- Forestry Callouts - |WreckedVehicle| - PedHeading, Case: " + PedHeading + " -!!-");
            Game.LogTrivial("-!!- Forestry Callouts - |WreckedVehicle| - PedSpawnpoint, Case: " + PedSpawnpoint + " -!!-");
            Game.LogTrivial("-!!- Forestry Callouts - |WreckedVehicle| - Heading, Case: " + Heading + " -!!-");

            if (Scenario == 1)
            {
                CalloutMessage = ("~g~Wrecked Vehicle Reported");
                CalloutAdvisory = ("~b~Dispatch:~w~ Suspect sounds ~r~disorientated~w~ on the phone, Respond ~r~Code 3~");
            }
            if (Scenario == 2)
            {
                CalloutMessage = ("~g~Wrecked Vehicle Reported");
                CalloutAdvisory = ("~b~Dispatch:~w~ Caller reports wrecked vehicle with no driver on scene, Respond ~r~Code 3~");
            }

            CalloutPosition = Spawnpoint;
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, Spawnpoint);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT_03 ASSISTANCE_REQUIRED_02 IN_OR_ON_POSITION UNITS_RESPOND_CODE_03_02", Spawnpoint);
            Game.LogTrivial("-!!- Forestry Callouts - |WreckedVehicle| - Callout Displayed");
           
            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            SimpleFunctions.CFunctions.SpawnOffroadCar(out susVehicle, Spawnpoint, Heading);
            
            if (SpawnpointChoosed == 1)
            {
                susVehicle.IsDeformationEnabled = true;
                SimpleFunctions.DeformationVehicles.WreckedVehicleDeformation1(in susVehicle);
                susVehicle.EngineHealth = 200f;
                susVehicle.FuelTankHealth = 20f;
            }
            if (SpawnpointChoosed == 2)
            {
                susVehicle.IsDeformationEnabled = true;
                SimpleFunctions.DeformationVehicles.WreckedVehicleDeformation1(in susVehicle);
                susVehicle.EngineHealth = 200f;
                susVehicle.FuelTankHealth = 20f;
            }
            if (SpawnpointChoosed == 3)
            {
                susVehicle.IsDeformationEnabled = true;
                SimpleFunctions.DeformationVehicles.WreckedVehicleDeformation1(in susVehicle);
                susVehicle.EngineHealth = 200f;
                susVehicle.FuelTankHealth = 20f;
                susVehicle.SetRotationRoll(180f);
            }
            if (SpawnpointChoosed == 4)
            {
                susVehicle.IsDeformationEnabled = true;
                SimpleFunctions.DeformationVehicles.WreckedVehicleDeformation1(in susVehicle);
                susVehicle.EngineHealth = 200f;
                susVehicle.FuelTankHealth = 20f;
            }
            if (SpawnpointChoosed == 5)
            {
                susVehicle.IsDeformationEnabled = true;
                SimpleFunctions.DeformationVehicles.WreckedVehicleDeformation1(in susVehicle);
                susVehicle.EngineHealth = 200f;
                susVehicle.FuelTankHealth = 20f;
            }
            if (SpawnpointChoosed == 6)
            {
                susVehicle.IsDeformationEnabled = true;
                SimpleFunctions.DeformationVehicles.WreckedVehicleDeformation1(in susVehicle);
                susVehicle.EngineHealth = 200f;
                susVehicle.FuelTankHealth = 20f;
            }
            if (SpawnpointChoosed == 7)
            {
                susVehicle.IsDeformationEnabled = true;
                SimpleFunctions.DeformationVehicles.WreckedVehicleDeformation1(in susVehicle);
                susVehicle.EngineHealth = 200f;
                susVehicle.FuelTankHealth = 20f;
            }
            if (SpawnpointChoosed == 8)
            {
                susVehicle.IsDeformationEnabled = true;
                SimpleFunctions.DeformationVehicles.WreckedVehicleDeformation1(in susVehicle);
                susVehicle.EngineHealth = 200f;
                susVehicle.FuelTankHealth = 20f;
            }
            if (SpawnpointChoosed == 9)
            {
                susVehicle.IsDeformationEnabled = true;
                SimpleFunctions.DeformationVehicles.WreckedVehicleDeformation1(in susVehicle);
                susVehicle.EngineHealth = 200f;
                susVehicle.FuelTankHealth = 20f;
            }


            switch (Scenario)
            {
                case 1:
                    PedOnFootScenario();
                    PedOnFootChoosed = true;
                    Game.LogTrivial("-!!- Forestry Callouts - |WreckedVehicle| - |Scenario| - PedOnFootScenario -!!-");
                    break;

                case 2:
                    PedWanderingArea();
                    PedWanderingAreaChoosed = true;
                    Game.LogTrivial("-!!- Forestry Callouts - |WreckedVehicle| - |Scenario| - PedWanderingAreaScenario -!!-");
                    break;
            }
            if (Suspect.IsMale)
                MaleFemale = "sir";
            else
                MaleFemale = "mam";
                            Counter = 0;
            Game.LogTrivial("-!!- Forestry Callouts - |WreckedVehicle| - Callout accepted! -!!-");
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            //For Scenario 1 and 3 dialogue intro
            if (PedOnFootChoosed)
            {
                if (!OnScene && Game.LocalPlayer.Character.Position.DistanceTo(Suspect) <= 10f && Game.LocalPlayer.Character.IsOnFoot)
                {
                    OnScene = true;
                    Game.DisplayHelp("Press ~r~'"+IniSettings.DialogueKey+"'~w~ to talk to the suspect", false);
                }
                if (Game.IsKeyDown(IniSettings.InputDialogueKey) && !DialogueOver)
                {
                    Suspect.Heading = Game.LocalPlayer.Character.Heading + 180f;
                    if (SuspectIsViolent)
                    {
                        S1DialogueViolent();
                    }
                    else
                    {
                        S1Dialogue();
                    }
                }
                //For Scenario one violent chooser (END OF SCEARIO 1 PROCESS)
                if (SuspectIsViolent && !DialogueChoosed)
                {
                    Game.LogTrivial("-!!- Forestry Callouts - |WreckedVehicle| - DialogueChooserViolent, Case: " + S1DialogueChooserViolent + " -!!-");
                    DialogueChoosed = true;
                }
                if (!SuspectIsViolent && !DialogueChoosed)
                {
                    Game.LogTrivial("-!!- Forestry Callouts - |WreckedVehicle| - DialogueChooser, Case: " + S1DialogueChooser + " -!!-");
                    DialogueChoosed = true;
                }
            }
            if (PedWanderingAreaChoosed)
            {
                if (!OnScene && !VehFoundFirst && Game.LocalPlayer.Character.Position.DistanceTo(Suspect.Position) <= 10f)
                {
                    LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                    Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                    LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                    LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                    SusVehicleBlip.Delete();
                    OnScene = true;
                    SusFoundFirst = true;
                }
                if (PedWanderingAreaChoosed)
                {
                    if (!TimerSet && !OnScene && !SusFoundFirst && (Game.LocalPlayer.Character.Position.DistanceTo(susVehicle.Position) <= 13f))
                    {
                        LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("ALL_UNITS_ATTEMPT_TO_REAQUIRE_01 SUSPECT_LAST_SEEN_01 ON_FOOT_01", Suspect.Position);
                        Game.DisplayNotification("The ~r~Suspect~w~ has left the area, search the ~y~Yellow~w~ Circle for the suspect");
                        TimerSet = true;
                        FirstSearchArea = true;
                        OnScene = true;
                        VehFoundFirst = true;
                    }
                    if (!FoundSuspect && TimerSet)
                    {
                        timer++;
                    }
                    if (FirstSearchArea && !FoundSuspect)
                    {
                        timer = 1250f;
                        FirstSearchArea = false;
                    }
                    if (timer >= 1250f && !FoundSuspect)
                    {
                        if (SuspectAreaBlip.Exists())
                        {
                            SuspectAreaBlip.Delete();
                        }
                        var position = Suspect.Position;
                        SearchArea = position.Around2D(10f, 35f);
                        SuspectAreaBlip = new Blip(SearchArea, 45f) { Color = Color.Yellow, Alpha = .5f };
                        timer = 0f;
                    }
                }

                if (VehFoundFirst)
                {
                    if (!FoundSuspect && Game.LocalPlayer.Character.Position.DistanceTo(Suspect.Position) <= 10f && Game.LocalPlayer.Character.IsOnFoot)
                    {
                        FoundSuspect = true;
                        Game.DisplayHelp("Press ~r~'"+IniSettings.DialogueKey+"'~w~ to talk to the suspect", false);
                        SuspectAreaBlip.Delete();
                        SuspectBlip = Suspect.AttachBlip();
                        SuspectBlip.Color = Color.Red;
                        Suspect.Tasks.StandStill(-1);

                    }
                    if (Game.IsKeyDown(IniSettings.InputDialogueKey) && !DialogueOver)
                    {
                        Suspect.Heading = Game.LocalPlayer.Character.Heading + 180f;
                        S2Dialogue();
                    }
                }
            }
            // For End Script
            if (Suspect.IsDead || Suspect.IsDead && !LSPD_First_Response.Mod.API.Functions.IsPursuitStillRunning(Pursuit))
            {
                if (Ini.IniSettings.EnableEndCalloutHelpMessages)
                {
                    Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                }
            }
            if (Game.IsKeyDown(IniSettings.InputEndCalloutKey))
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", Spawnpoint);
                Game.DisplayNotification("~g~Dispatch:~w~ All Units, Wrecked Vehicle Code 4");
                Game.LogTrivial("-!!- Forestry Callouts - |Wrecked Vehicle| - Callout was force ended by player -!!-");
                End();
            }
            if (Game.LocalPlayer.IsDead)
            {
                End();
                Game.DisplayNotification("Wrecked Vehicle Code 4");
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", Spawnpoint);
            }
            base.Process();
        }
        private void PedOnFootScenario()
        {
            SimpleFunctions.CFunctions.SpawnCountryPed(out Suspect, PedSpawnpoint, PedHeading);
            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.IsRouteEnabled = true;
            SuspectBlip.Color = System.Drawing.Color.Green;
            if (S1DialogueChooser == 4)
            {
                SimpleFunctions.CFunctions.SetDrunk(Suspect, true);
                StopThePed.API.Functions.setPedAlcoholOverLimit(Suspect, true);
            }
            else
            {
                SimpleFunctions.CFunctions.PedPersonaChooser(Suspect, true, true);
            }
            return;
        }

        private void PedWanderingArea()
        {
            PedHeading = new Random().Next(1, 361);
            SimpleFunctions.CFunctions.SpawnCountryPed(out Suspect, PedWanderSpawnpoint, PedHeading);
            SusVehicleBlip = susVehicle.AttachBlip();
            SusVehicleBlip.EnableRoute(Color.Yellow);
            SusVehicleBlip.Color = Color.Red;
            Suspect.Tasks.Wander();
            if (S2DialougeChooser == 3)
            {
                SimpleFunctions.CFunctions.SetDrunk(Suspect, true);
                StopThePed.API.Functions.setPedAlcoholOverLimit(Suspect, true);
            }
            return;
        }

        private void S2Dialogue()
        {
            switch (S2DialougeChooser)
            {
                case 1:

                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + " did you crash a vehicle recently?");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Uhm... Absoultely not officer");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Can you talk to me for a second?");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ NO, $%^# YOU PIG!");
                        if (AttackOrPursuit == 1)
                        {
                            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                            Suspect.Tasks.Wander();
                            Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                            LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                            SuspectBlip.Delete();
                        }
                        if (AttackOrPursuit == 2)
                        {
                            SuspectBlip.Delete();
                            SimpleFunctions.CFunctions.MeleeWeaponChooser(Suspect, -1, true);
                            Suspect.Tasks.FightAgainst(Game.LocalPlayer.Character);  
                        }
                        DialogueOver = true;
                    }

                    Counter++;

                    break;

                case 2:

                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + " did you crash a vehicle recently??");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ I... uhh. Yeah..crashed the car");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Okay why did you leave the scene?");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ No No.. NO. RUN!");
                        LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                        Suspect.Tasks.Wander();
                        Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                        LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                        LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                        SuspectBlip.Delete();
                        DialogueOver = true;
                    }

                    Counter++;


                    break;

                case 3:

                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + " did you crash a vehicle recently?");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Ahhhh yeah. just uh.. was driving around and blacked out");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Okay, have you taken any drugs or drank recently?");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ No.. No. Absoultley Not. I uhh. RUNN!");
                        LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                        Suspect.Tasks.Wander();
                        Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                        LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                        LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                        SuspectBlip.Delete();
                        DialogueOver = true;
                    }

                    Counter++;

                    break;
            }
            return;
        }
    

        private void Events_pedArrestedEvent(Ped Suspect)//You know..
            {
                if (!STPpedArrested)
                    Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);

                if (Game.IsKeyDown(IniSettings.InputEndCalloutKey))
                {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", Spawnpoint);
                Game.DisplayNotification("Wrecked Vehicle Code 4");
                Game.LogTrivial("-!!- Forestry Callouts - |Wrecked Vehicle| - Callout was force ended by player -!!-");
                STPpedArrested = true;
                End();
                }
            }
        private void S1Dialogue()
        {
            var step = 1;
            
            switch (S1DialogueChooser)
            {
                case 1:

                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + " what happend?");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ I was driving and having fun and all of sudden I lost control of the car and crashed");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~Were you going over the speed limit?");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Of course I was its a country road.. rules dont matter");
                    }
                    if (Counter == 5)
                    {
                        Game.DisplaySubtitle("No further dialogue take appropriate action");
                        DialogueOver = true;
                    }


                    Counter++;

                    break;

                case 2:

                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + ", what happend?");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ I... uhh. crashed the car");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Okay are you in need of any medical assistance");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ No I will be okay");
                    }
                    if (Counter == 5)
                    {
                        Game.DisplaySubtitle("No further dialogue take appropriate action");
                        DialogueOver = true;
                    }



                    Counter++;


                    break;

                case 3:

                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + ", what happened?");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Oh I was driving and almost hit a deer so I swerved out of the way");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Do you need medical assitance?");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Yes please my leg is hurting really bad");
                    }
                    if (Counter == 5)
                    {
                        Game.DisplaySubtitle("No further dialogue take appropriate action");
                        DialogueOver = true;
                    }


                    Counter++;

                    break;

                case 4:
                    
                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + ", what happened?");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Ahhhh just uhm.. was driving around and blacked out");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Okay, have you taken any drugs recently?");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ No.. No. Absoultley Not. I uhh. was just drivng around");
                    }
                    if (Counter == 5)
                    {
                        Game.DisplaySubtitle("No further dialogue take appropriate action");
                        DialogueOver = true;
                    }


                    Counter++;

                    break;
            }
            return;
        }
        private void S1DialogueViolent()
        {
            var step = 1;

            switch (S1DialogueChooserViolent)
            {
                case 1:

                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + " what happend?");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ I was driving and having fun and all of sudden I lost control of the car and crashed");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Were you going over the speed limit?");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ THAT DOESNT MATTER! PIG!");
                        if (AttackOrPursuit == 1)
                        {
                            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                            Suspect.Tasks.Wander();
                            Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                            LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                            SuspectBlip.Delete();
                        }
                        if (AttackOrPursuit == 2)
                        {
                            SuspectBlip.Delete();
                            SimpleFunctions.CFunctions.MeleeWeaponChooser(Suspect, -1, true);
                            Suspect.Tasks.FightAgainst(Game.LocalPlayer.Character); 
                        }
                        DialogueOver = true;
                    }
                   
                    Counter++;

                    break;

                case 2:

                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + ", what happend?");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ I... uhh. crashed the car");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Okay are you in need of any medical assistance");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ NO YOU STUPID COP! LEAVE ME ALONE!");
                        LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                        Suspect.Tasks.Wander();
                        Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                        LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                        LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                        SuspectBlip.Delete();
                        DialogueOver = true;
                    }

                    Counter++;


                    break;

                case 3:

                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + ", what happened?");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Ahhhh just uhm.. was driving around and blacked out");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Okay, have you taken any drugs or drank recently?");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ No.. No. Absoultley Not. I uhh. RUNN!");
                        LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                        Suspect.Tasks.Wander();
                        Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                        LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                        LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                        SuspectBlip.Delete();
                        DialogueOver = true;
                    }                    

                    Counter++;

                    break;
            }
            return;
        }
        public override void End()//Clean up script
        {
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
            if (SuspectAreaBlip.Exists())
            {
                SuspectAreaBlip.Delete();
            }
            if (SusVehicleBlip.Exists())
            {
                SusVehicleBlip.Delete();
            }
            Game.LogTrivial("-!!- Forestry Callouts - |WreckedVehicle| - Cleaned Up -!!-");
            base.End();
        }
    }
}