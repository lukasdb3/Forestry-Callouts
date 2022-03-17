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
    [CalloutInfo("LoggerPursuit", CalloutProbability.Low)]
    internal class LoggerPursuit : Callout
    {
        private Vector3 Spawnpoint;
        private Vehicle SusVehTruck;
        private Vehicle SusVehTrailer;
        private Ped Suspect;
        private Blip SusVehTruckBlip;
        private float Heading;
        private LHandle Pursuit;
        private bool OnScene;
        private bool CloseToScene;
        private bool PursuitStarted;
        private bool SuspectIsViolent;
        private Ped Passenger;
        private int PassengerChooser = new Random().Next(1, 3);
        public override bool OnBeforeCalloutDisplayed()
        {
            SimpleFunctions.CFunctions.SuspectViolChooser(out SuspectIsViolent);
            CalloutMessage = ("~g~Logger Pursuit Reported");
            CalloutAdvisory = ("~b~Dispatch:~w~ Pursuit reported, Vehicle is driving recklessly. Respond ~r~Code 3~w~");

            ForestryCallouts.SimpleFunctions.SPFunctions.LoggerPursuitSpawnChooser(out Spawnpoint, out Heading);

            CalloutPosition = Spawnpoint;
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, Spawnpoint);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT_03 CRIME_RESIST_ARREST_02 IN_OR_ON_POSITION UNITS_RESPOND_CODE_03_02", Spawnpoint);
            return base.OnBeforeCalloutDisplayed();
        }
        
        public override void OnCalloutDisplayed()
        {
            LFunctions.Log(this, "Callout displayed!");
            if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendCalloutDetails(this, "CODE 3", "SAPR");

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
            ForestryCallouts.SimpleFunctions.CFunctions.SpawnSemiTruck(out SusVehTruck, Spawnpoint, Heading);
            SusVehTruck.TopSpeed = 25f;

            SusVehTrailer = new Vehicle("trailerlogs", SusVehTruck.GetOffsetPositionFront(10f), Heading);
            SusVehTrailer.IsPersistent = true;
            SusVehTruck.Trailer = SusVehTrailer;

            if (PassengerChooser == 1)
            {
                ForestryCallouts.SimpleFunctions.CFunctions.SpawnHikerPed(out Passenger, Spawnpoint, 0f);
                Passenger.WarpIntoVehicle(SusVehTruck, 0);
                ForestryCallouts.SimpleFunctions.CFunctions.PedPersonaChooser(in Passenger, true, true);
                LFunctions.Log(this, "Passenger added!");
            }
            else
            {
                LFunctions.Log(this, "No passenger added!");
            }

            ForestryCallouts.SimpleFunctions.CFunctions.SpawnCountryPed(out Suspect, Spawnpoint, 0f);
            Suspect.WarpIntoVehicle(SusVehTruck, -1);
            ForestryCallouts.SimpleFunctions.CFunctions.PedPersonaChooser(in Suspect, true, true);
            LFunctions.Log(this, "Suspect spawned!");

            SusVehTruckBlip = SusVehTruck.AttachBlip();
            SusVehTruckBlip.Color = Color.Red;
            SusVehTruckBlip.EnableRoute(Color.Yellow);
            return base.OnCalloutAccepted();
        }
        public override void Process()
        {
            //Main Code Stuff
            if (Game.LocalPlayer.Character.DistanceTo(SusVehTruck) <= 175f && !CloseToScene)
            {
                CloseToScene = true;
                Suspect.Tasks.CruiseWithVehicle(30f, VehicleDrivingFlags.Emergency);
                LFunctions.Log(this, "Suspect cruising with vehicle!");
            }
            if (CloseToScene && Game.LocalPlayer.Character.DistanceTo(SusVehTrailer) <= 30f && !PursuitStarted || CloseToScene && Game.LocalPlayer.Character.DistanceTo(SusVehTruck) <= 30f && !PursuitStarted)
            {
                if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendMessage(this, "Officer is on scene.");
                Suspect.Tasks.CruiseWithVehicle(50, VehicleDrivingFlags.Emergency);

                SusVehTruckBlip.Delete();
                Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                if (PassengerChooser == 1)
                {
                    LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Passenger);
                }
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                PursuitStarted = true;
                LFunctions.Log(this, "Pursuit started!");
            }
            //End Stuff
            if (PursuitStarted && !LSPD_First_Response.Mod.API.Functions.IsPursuitStillRunning(Pursuit))
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
                LFunctions.Log(this, "Callout ended to players death!");
                End();
            }
            if (Game.IsKeyDown(IniSettings.InputEndCalloutKey)) //If player presses "End" it will forcefully clean the callout up
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", Spawnpoint);
                Game.DisplayNotification("~g~Dispatch:~w~ All Units, Logger Pursuit Code 4");
                if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendMessage(this, "Logger Pursuit code 4");
                LFunctions.Log(this, "Callout was force ended by player!");
                End();
            }
            base.Process();
        }
        public override void End()
        {
            if (Ini.IniSettings.DeleteLoggerTruckOnEnd)
            {
                if (SusVehTruck.Exists())
                {
                    SusVehTruck.Delete();
                }
                if (SusVehTrailer.Exists())
                {
                    SusVehTrailer.Delete();
                }
            }
            else
            {
                if (SusVehTruck.Exists())
                {
                    SusVehTruck.Dismiss();
                }
                if (SusVehTrailer.Exists())
                {
                    SusVehTrailer.Dismiss();
                }
            }
            if (SusVehTruckBlip.Exists())
            {
                SusVehTruckBlip.Delete();
            }
            if (Passenger.Exists())
            {
                Passenger.Dismiss();
            }
            LFunctions.Log(this, "Cleaned up!");

            base.End();
        }
    }
}
