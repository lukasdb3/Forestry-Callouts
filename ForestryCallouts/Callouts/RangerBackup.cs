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
using Rage.Native;

namespace ForestryCallouts.Callouts
{
    [CalloutInfo("RangerRequestingBackup", CalloutProbability.Medium)]
    internal class RangerBackup : Callout
    {
        private bool SuspectIsViolent;
        private int Scenario = new Random().Next(1, 3);
        private bool OnScene;


        //cop shit
        private Vector3 CopSpawnpoint;
        private float CopHeading;
        private Vehicle CopVehicle;
        private Ped Cop;
        private Blip CopBlip;
        private WeaponDescriptor CopsWeapon;

        //suspect shit
        private Vector3 SusSpawnpoint;
        private float SusHeading;
        private Vehicle SusVehicle;
        private Ped Suspect;
        private Blip SuspectBlip;
        private string MaleFemale;
        private WeaponDescriptor SuspectsWeapon;

        //passenger shit
        private Ped Passenger;
        private int PassengerChooser = new Random().Next(1, 4);
        private WeaponDescriptor PassengersWeapon;

        //Dialogue shit
        private bool DialogueStarting;
        private bool DialogueOver;
        private int Counter;
        private int ScenarioOneDialogueChooser = new Random().Next(1, 3);

        //Scenario one shit
        private bool SuspectReadyToStartPursuit;
        private bool SuspectActed;
        private LHandle Pursuit;
        private bool PursuitStarted;
        private bool SuspectReadyToShoot;
        private bool CopFireingBack;

        //Scenario two shit
        private bool AICopIsDead;
        private bool AICopIsAlive;
        private bool ScenarioTwoMainProcessOver;
        public override bool OnBeforeCalloutDisplayed()
        {
            Game.LogTrivial("-!!- Forestry Callouts - |RangerBackup| - Callout Displayed -!!-");
            SimpleFunctions.CFunctions.SuspectViolChooser(out SuspectIsViolent);
            //Scenario 1 Callout Messeages
            if (Scenario == 1)
            {
                if (SuspectIsViolent)
                {
                    CalloutMessage = ("~g~Ranger Requesting Backup");
                    CalloutAdvisory = ("~b~Dispatch:~w~ Ranger is requesting backup ~r~immediately~w~. Respond ~r~Code 3~w~");
                    LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_01 ASSISTANCE_REQUIRED_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_03_02", CopSpawnpoint);
                }
                else
                {
                    CalloutMessage = ("~g~Ranger Requesting Backup");
                    CalloutAdvisory = ("~b~Dispatch:~w~ Ranger is requesting backup on traffic stop. Respond Code 2");
                    LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_01 ASSISTANCE_REQUIRED_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_02_02", CopSpawnpoint);
                }
            }
            if (Scenario == 2)
            {
                CalloutMessage = ("~g~Ranger Requesting Backup");
                CalloutAdvisory = ("~b~Dispatch:~w~ Ranger is requesting backup, reports of ~r~shots fired~w~. Respond ~r~Code 3~w~");
                LSPD_First_Response.Mod.API .Functions.PlayScannerAudioUsingPosition("WE_HAVE_01 CRIME_SHOTS_FIRED_AT_AN_OFFICER_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_02_03", CopSpawnpoint);
            }

            ForestryCallouts.SimpleFunctions.SPFunctions.RangerBackupSpawnChooser(out CopSpawnpoint, out CopHeading, out SusSpawnpoint, out SusHeading);

            CalloutPosition = CopSpawnpoint;
            ShowCalloutAreaBlipBeforeAccepting(CopSpawnpoint, 30f);
            AddMinimumDistanceCheck(30f, CopSpawnpoint);
            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("-!!- Forestry Callouts - |RangerBackup| - Callout accepted -!!-");
            Cop = new Ped("s_m_y_ranger_01", CopSpawnpoint, CopHeading);
            ForestryCallouts.SimpleFunctions.CFunctions.SpawnRangerVehicle(out CopVehicle, CopSpawnpoint, CopHeading);
            Cop.WarpIntoVehicle(CopVehicle, -1);
            Cop.RelationshipGroup = "COP";
            Rage.Native.NativeFunction.Natives.SET_PED_AS_COP(Cop, true);
            CopBlip = Cop.AttachBlip();
            CopBlip.Color = Color.MediumBlue;
            CopBlip.EnableRoute(Color.Yellow);

            SimpleFunctions.CFunctions.SpawnCountryPed(out Suspect, SusSpawnpoint, SusHeading);
            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.Color = Color.Red;
            SimpleFunctions.CFunctions.SpawnOffroadCar(out SusVehicle, SusSpawnpoint, SusHeading);
            Suspect.WarpIntoVehicle(SusVehicle, -1);
            Suspect.RelationshipGroup = "SUSPECT";
            if (Scenario == 1)
            {
                ForestryCallouts.SimpleFunctions.CFunctions.SetWanted(Suspect, true);
                SusVehicle.IsEngineOn = true;
            }

            if (Suspect.IsMale)
            {
                MaleFemale = "sir";
            }
            else
            {
                MaleFemale = "mam";
            }

            if (PassengerChooser == 1)
            {
                ForestryCallouts.SimpleFunctions.CFunctions.SpawnHikerPed(out Passenger, SusSpawnpoint, SusHeading);
                ForestryCallouts.SimpleFunctions.CFunctions.PedPersonaChooser(in Passenger, true, true);
                Passenger.WarpIntoVehicle(SusVehicle, 0);
                Game.LogTrivial("-!!- Forestry Callouts - |RangerBackup| - Passenger added! -!!-");
                Passenger.RelationshipGroup = "SUSPECT";
            }
            Game.LocalPlayer.Character.RelationshipGroup = "PLAYER";
            Game.SetRelationshipBetweenRelationshipGroups("COP", "PLAYER", Relationship.Respect);
            return base.OnCalloutAccepted();
        }
        public override void Process()
        {
            //Scenario One Stufffffss
            if (Scenario == 1)
            {
                if (Game.LocalPlayer.Character.DistanceTo(CopVehicle) <= 15f && !OnScene)
                {
                    Game.DisplayHelp("Go talk to the ~g~Ranger~w~");
                    LSPD_First_Response.Mod.API.Functions.StartPulloverOnParkedVehicle(SusVehicle, false, true);
                    OnScene = true;
                }
                if (Game.LocalPlayer.Character.DistanceTo(Cop) <= 6f && !DialogueOver)
                {
                    if (!DialogueStarting)
                    {
                        DialogueStarting = true;
                        Game.DisplayHelp("Press ~r~'"+IniSettings.DialogueKey+"'~w~ to talk to the ~g~Ranger~w~");
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
                //Scenario 1: Suspect takes off when hes wanted.
                if (SuspectReadyToStartPursuit)
                {
                    if (Game.LocalPlayer.Character.DistanceTo(Suspect) <= 4f && !SuspectActed)
                    {
                        SuspectActed = true;
                        Game.DisplaySubtitle("~r~Suspect:~w~ FUCK YOU PIGS! YOU WONT GET ME!");
                        Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                        LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                        LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                        LSPD_First_Response.Mod.API.Functions.AddCopToPursuit(Pursuit, Cop);
                        LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                        if (SuspectBlip.Exists())
                        {
                            SuspectBlip.Delete();
                        }
                        if (CopBlip.Exists())
                        {
                            CopBlip.Delete();
                        }
                        PursuitStarted = true;
                    }
                }
                //Scenario 1: Suspect gets out and trys to shoot player when hes wanted.
                if (SuspectReadyToShoot)
                {
                    if (Game.LocalPlayer.Character.DistanceTo(Suspect) <= 6f && !SuspectActed)
                    {
                        Suspect.Tasks.LeaveVehicle(SusVehicle, LeaveVehicleFlags.LeaveDoorOpen).WaitForCompletion(500);
                        Suspect.Tasks.AimWeaponAt(Game.LocalPlayer.Character, -1).WaitForCompletion(400);
                        Suspect.Tasks.FireWeaponAt(Game.LocalPlayer.Character, -1, FiringPattern.DelayFireByOneSecond);
                        SuspectActed = true;
                    }
                    if (Suspect.IsShooting && !CopFireingBack)
                    {
                        Cop.Tasks.FireWeaponAt(Suspect, -1, FiringPattern.DelayFireByOneSecond);
                        CopFireingBack = true;
                    }
                }
            }
            //Scenario 2 Stufffffffffs
            if (Scenario == 2)
            {
                //Scenario 2: Suspect and Cop are in a gunfight apon arival to the scene.
                if (Game.LocalPlayer.Character.DistanceTo(CopVehicle) <= 30f && !OnScene)
                {
                    OnScene = true;
                    CopsWeapon = Cop.Inventory.GiveNewWeapon("weapon_combatpistol", -1, true);
                    Cop.Inventory.EquippedWeapon = CopsWeapon;
                    Cop.Armor = 65;

                    Cop.Tasks.LeaveVehicle(CopVehicle, LeaveVehicleFlags.LeaveDoorOpen).WaitForCompletion(500);
                    Cop.Tasks.FireWeaponAt(Suspect, -1, FiringPattern.DelayFireByOneSecond);

                    SuspectsWeapon = Suspect.Inventory.GiveNewWeapon("weapon_pistol", -1, true);
                    Suspect.Inventory.EquippedWeapon = SuspectsWeapon;
                    Suspect.Armor = 150;

                    Suspect.Tasks.LeaveVehicle(SusVehicle, LeaveVehicleFlags.LeaveDoorOpen).WaitForCompletion(500);
                    Suspect.Tasks.FireWeaponAt(Cop, -1, FiringPattern.DelayFireByOneSecond);
                    Game.LogTrivial("-!!- Forestry Callouts - |RangerBackup| - Cop and Suspect given weapons -!!-");

                    if (PassengerChooser == 1)
                    {
                        PassengersWeapon = Passenger.Inventory.GiveNewWeapon("weapon_pistol", -1, true);
                        Passenger.Inventory.EquippedWeapon = PassengersWeapon;

                        Passenger.Tasks.LeaveVehicle(SusVehicle, LeaveVehicleFlags.LeaveDoorOpen).WaitForCompletion(500);
                        Passenger.Tasks.AimWeaponAt(Cop, -1).WaitForCompletion(200);
                        Game.LogTrivial("-!!- Forestry Callouts - |RangerBackup| - Passenger is shooting along with suspect -!!-");
                    }
                    Game.SetRelationshipBetweenRelationshipGroups("SUSPECT", "COP", Relationship.Hate);
                    Game.SetRelationshipBetweenRelationshipGroups("COP", "SUSPECT", Relationship.Hate);
                    Game.LogTrivial("-!!- Forestry Callouts - |RangerBackup| - Cop covering and shooting -!!-");
                }
                if (Cop.IsDead || Suspect.IsDead)
                {
                    ScenarioTwoMainProcessOver = true;
                }
                if (ScenarioTwoMainProcessOver)
                {
                    if (Cop.IsDead && !AICopIsDead)
                    {
                        AICopIsDead = true;
                        if (Suspect.Exists())
                        {
                            Game.SetRelationshipBetweenRelationshipGroups("SUSPECT", "PLAYER", Relationship.Hate);
                            Suspect.Tasks.FireWeaponAt(Game.LocalPlayer.Character, -1, FiringPattern.DelayFireByOneSecond);
                        }
                        if (Passenger.Exists())
                        {
                            Game.SetRelationshipBetweenRelationshipGroups("SUSPECT", "PLAYER", Relationship.Hate);
                            Passenger.Tasks.FireWeaponAt(Game.LocalPlayer.Character, -1, FiringPattern.DelayFireByOneSecond);
                        }
                        Game.LogTrivial("-!!- Forestry Callouts - |RangerBackup| - AI cop is dead, suspect now shooting player -!!-");
                    }
                    if (Passenger.Exists())
                    {
                        if (Suspect.IsDead && Passenger.IsDead && Cop.IsAlive && !AICopIsAlive)
                        {
                            Game.LogTrivial("-!!- Forestry Callouts - |RangerBackup| - Suspect is dead -!!-");
                            AICopIsAlive = true;
                            Game.SetRelationshipBetweenRelationshipGroups("SUSPECT", "COP", Relationship.Dislike);
                            Game.SetRelationshipBetweenRelationshipGroups("COP", "SUSPECT", Relationship.Dislike);
                        }
                    }
                    if (Cop.Exists())
                    {
                        if (Suspect.IsDead && Cop.IsAlive && !AICopIsAlive)
                        {
                            Game.LogTrivial("-!!- Forestry Callouts - |RangerBackup| - Suspect is dead -!!-");
                            AICopIsAlive = true;
                            Game.SetRelationshipBetweenRelationshipGroups("SUSPECT", "COP", Relationship.Dislike);
                            Game.SetRelationshipBetweenRelationshipGroups("COP", "SUSPECT", Relationship.Dislike);
                        }
                    }
                }
            }

            //EndShit
            if (Game.IsKeyDown(IniSettings.InputEndCalloutKey)) //If player presses "End" it will forcefully clean the callout up
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", CopSpawnpoint);
                Game.DisplayNotification("~g~Dispatch:~w~ All Units, Ranger Requesting Backup Code 4");
                Game.LogTrivial("-!!- Forestry Callouts - |RangerBackup| - Callout was force ended by player -!!-");
                End();
            }
            if (PursuitStarted && !LSPD_First_Response.Mod.API.Functions.IsPursuitStillRunning(Pursuit) || Suspect.IsDead)
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
                Game.LogTrivial("-!!- Forestry Callouts - |RangerBackup| - Callout was ended due to players death -!!-");
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
                        Game.DisplaySubtitle("~y~Player:~w~ Hey, officer what's going on?.");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~g~Ranger:~w~ I ran the suspect's name in the system and his name is coming back ~y~wanted for burglarly~w~.");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Okay, I'll go get him out of the vehicle, stay in your vehicle incase he decides to take off.");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~g~Ranger:~w~ Sounds good. I'll be right here.");
                        Game.DisplayHelp("Use ~y~Stop The Ped~w~ to get the suspect out of the vehicle");
                        DialogueOver = true;
                    }
                    Counter++;
                    break;

                case 2:
                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hey officer, what's going on?");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~g~Ranger:~w~ I ran the suspect's id and he's coming back as ~y~wanted for possesion of illegal drugs~w~.");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Okay, Ill go get him out, you sit back here and have my back.");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~g~Ranger:~w~ Alright, ready when you are!");
                        Game.DisplayHelp("Use ~y~Stop The Ped~w~ to get the suspect out of the vehicle");
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
                        Game.DisplaySubtitle("~y~Player:~w~ Hey, what's happening?.");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~g~Ranger:~w~ I ran the suspect's name in the system and his name is coming back ~r~wanted for fleeing from Law Enforcement~w~.");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Okay, I'll go get him out of the vehicle, stay in your vehicle incase he decides to take off.");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~g~Ranger:~w~ Sounds good. I'll be right here.");
                        DialogueOver = true;
                        SuspectReadyToStartPursuit = true;
                    }
                    if (Counter == 5)
                    {
                        Game.DisplaySubtitle("No further dialogue");
                    }

                    Counter++;
                    break;

                case 2:
                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hey officer, what's going on?");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~g~Ranger:~w~ I ran the suspect's id and he's coming back as ~r~wanted for possesion of illegal firearm~w~.");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Okay, Ill go get him out, you sit back here and have my back.");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~g~Ranger:~w~ Alright, ready when you are!");
                        Cop.Tasks.LeaveVehicle(CopVehicle, LeaveVehicleFlags.None).WaitForCompletion(400);
                        CopsWeapon = Cop.Inventory.GiveNewWeapon("weapon_combatpistol", -1, true);
                        Cop.Inventory.EquippedWeapon = CopsWeapon;
                        Cop.Heading = Suspect.Heading;
                        Cop.Tasks.AimWeaponAt(Suspect, -1);

                        SuspectsWeapon = Suspect.Inventory.GiveNewWeapon("weapon_pistol", -1, true);
                        Suspect.Inventory.EquippedWeapon = SuspectsWeapon;

                        DialogueOver = true;
                        SuspectReadyToShoot = true;
                    }
                    if (Counter == 5)
                    {
                        Game.DisplaySubtitle("No further dialogue");
                    }

                    Counter++;
                    break;
            }
        }
        public override void End()
        {
            Game.SetRelationshipBetweenRelationshipGroups("SUSPECT", "COP", Relationship.Neutral);
            Game.SetRelationshipBetweenRelationshipGroups("COP", "SUSPECT", Relationship.Neutral);
            if (CopVehicle.Exists())
            {
                CopVehicle.Dismiss();
            }
            if (Cop.Exists())
            {
                Cop.Dismiss(); 
            }
            if (SusVehicle.Exists())
            {
                SusVehicle.Dismiss();
            }
            if (Suspect.Exists())
            {
                Suspect.Dismiss(); 
            }
            if (Passenger.Exists())
            {
                Passenger.Dismiss(); 
            }
            if (CopBlip.Exists())
            {
                CopBlip.Delete();
            }
            if (SuspectBlip.Exists())
            {
                SuspectBlip.Delete();
            }

            base.End();
        }
    }
}
