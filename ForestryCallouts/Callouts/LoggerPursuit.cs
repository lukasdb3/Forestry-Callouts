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
    [CalloutInfo("LoggerPursuit", CalloutProbability.Low)]
    internal class LoggerPursuit : Callout
    {
        // IDea:
        // Have a dangerous driving logger truck through the forest eventually when the player is behind the truck there will
        // Truck starts pursuit right away 

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
            Game.LogTrivial("-!!- Forestry Callouts - |LoggerPursuit| - Callout Displayed -!!-");
            SimpleFunctions.CFunctions.SuspectViolChooser(out SuspectIsViolent);
            CalloutMessage = ("~g~Logger Pursuit Reported");
            CalloutAdvisory = ("~b~Dispatch:~w~ Pursuit reported, Vehicle is driveing recklessly. Respond ~r~Code 3~w~");

            ForestryCallouts.SimpleFunctions.SPFunctions.LoggerPursuitSpawnChooser(out Spawnpoint, out Heading);
            Game.LogTrivial("-!!- Forestry Callouts - |LoggerPursuit| - TruckHeading: " + Heading + " -!!-");
            

            CalloutPosition = Spawnpoint;
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, Spawnpoint);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT_03 CRIME_RESIST_ARREST_02 IN_OR_ON_POSITION UNITS_RESPOND_CODE_03_02", Spawnpoint);
            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("-!!- Forestry Callouts - |LoggerPursuit| - Callout Accepted -!!-");
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
                Game.LogTrivial("-!!- Forestry Callouts - |LoggerPursuit| - Passenger added! -!!-");
            }
            else
            {
                Game.LogTrivial("-!!- Forestry Callouts - |LoggerPursuit| - No passenger added -!!-");
            }

            ForestryCallouts.SimpleFunctions.CFunctions.SpawnCountryPed(out Suspect, Spawnpoint, 0f);
            Suspect.WarpIntoVehicle(SusVehTruck, -1);
            ForestryCallouts.SimpleFunctions.CFunctions.PedPersonaChooser(in Suspect, true, true);
            Game.LogTrivial("-!!- Forestry Callouts - |LoggerPursuit| - Suspect Spawned -!!-");

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
                Game.LogTrivial("-!!- Forestry Callouts - |LoggerPursuit| - Suspect crusing with vehicle -!!-");
            }
            if (CloseToScene && Game.LocalPlayer.Character.DistanceTo(SusVehTrailer) <= 30f && !PursuitStarted || CloseToScene && Game.LocalPlayer.Character.DistanceTo(SusVehTruck) <= 30f && !PursuitStarted)
            {
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
                Game.LogTrivial("-!!- Forestry Callouts - |LoggerPursuit| - Pursuit with logger truck started! -!!-");
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
                Game.LogTrivial("-!!- Forestry Callouts - |LoggerPursuit| - Callout was ended due to players death -!!-");
                End();
            }
            if (Game.IsKeyDown(IniSettings.InputEndCalloutKey)) //If player presses "End" it will forcefully clean the callout up
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", Spawnpoint);
                Game.DisplayNotification("~g~Dispatch:~w~ All Units, Logger Pursuit Code 4");
                Game.LogTrivial("-!!- Forestry Callouts - |LoggerPursuit| - Callout was force ended by player -!!-");
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
                Game.LogTrivial("-!!- Forestry Callouts - |LoggerPursuit| - Logger truck was not deleted, dismissed the truck! -!!-");
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
            Game.LogTrivial("-!!- Forestry Callouts - |LoggerPursuit| - Cleaned Up -!!-");

            base.End();
        }
    }
}
