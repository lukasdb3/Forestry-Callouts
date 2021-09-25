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
    [CalloutInfo("HighSpeedPursuit", CalloutProbability.Medium)]
    internal class HighSpeedPursuit : Callout
    {
        private Vector3 Spawnpoint;
        private float Heading;
        private bool SuspectIsViolent;
        private Ped Suspect;
        private Vehicle SusVehicle;
        private Ped Cop;
        private Vehicle CopVehicle;
        private Vector3 AroundSpawnpoint;
        private Vector3 CopSpawnpoint;
        private LHandle Pursuit;
        private bool PursuitStarted;
        private Blip SusVehicleBlip;
        private bool ToFar;
        private bool OnScene;
        private bool CloseToPursuit;
        private Ped Passenger;
        private int PassengerSeat = 1;
        private int PassengerChooser = new Random().Next(1, 3);
        private bool PassengerAvailable;
        public override bool OnBeforeCalloutDisplayed()
        {
            Game.LogTrivial("-!!- Forestry Callouts - |HighSpeedPursuit| - Callout Displayed");
            SimpleFunctions.CFunctions.SuspectViolChooser(out SuspectIsViolent);
            CalloutMessage = ("~g~High Speed Pursuit Reported");
            CalloutAdvisory = ("~b~Dispatch:~w~ High speed pursuit of a offroad vehicle reported. Respond ~r~Code 3~w~");

            ForestryCallouts.SimpleFunctions.SPFunctions.LoggerPursuitSpawnChooser(out Spawnpoint, out Heading);
            CalloutPosition = Spawnpoint;
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, Spawnpoint);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_01 CRIME_RESIST_ARREST_02 IN_OR_ON_POSITION UNITS_RESPOND_CODE_03_02", Spawnpoint);

            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("-!!- Forestry Callouts - |HighSpeedPursuit| - Callout accepted! -!!-");
            ForestryCallouts.SimpleFunctions.CFunctions.SpawnCountryPed(out Suspect, Spawnpoint, Heading);
            ForestryCallouts.SimpleFunctions.CFunctions.PedPersonaChooser(in Suspect, true, true);

            ForestryCallouts.SimpleFunctions.CFunctions.SpawnFastOffroadVeh(out SusVehicle, Spawnpoint, Heading);
            Suspect.WarpIntoVehicle(SusVehicle, -1);
            Game.LogTrivial("-!!- Forestry Callouts - |HighSpeedPursuit| - Suspect Spawned! -!!-");

            if (SusVehicle.PassengerCapacity >= PassengerSeat)
            {
                Game.LogTrivial("-!!- Forestry Callouts - |HighSpeedPursuit| - Suspect vehicle does have room for passenger -!!-");
                PassengerAvailable = true;
                if (PassengerChooser == 1)
                {
                    ForestryCallouts.SimpleFunctions.CFunctions.SpawnHikerPed(out Passenger, Spawnpoint, 0f);
                    Passenger.WarpIntoVehicle(SusVehicle, 0);
                    ForestryCallouts.SimpleFunctions.CFunctions.PedPersonaChooser(in Passenger, true, true);
                    Game.LogTrivial("-!!- Forestry Callouts - |LoggerPursuit| - Passenger added! -!!-");
                }
                else
                {
                    Game.LogTrivial("-!!- Forestry Callouts - |HighSpeedPursuit| - No passenger added -!!-");
                }
            }

            AroundSpawnpoint = SusVehicle.GetOffsetPositionRight(30f);
            CopSpawnpoint = World.GetNextPositionOnStreet(AroundSpawnpoint);
            Game.LogTrivial("-!!- Forestry Callouts - |HighSpeedPursuit| - CopSpawnpoint Choosed! -!!-");

            Cop = new Ped("s_m_y_ranger_01", CopSpawnpoint, Heading);
            CopVehicle = new Vehicle("pranger", CopSpawnpoint, Heading);
            Cop.WarpIntoVehicle(CopVehicle, -1);
            Game.LogTrivial("-!!- Forestry Callouts - |HighSpeedPursuit| - Cop Spawned! -!!-");
            return base.OnCalloutAccepted();
        }
        public override void Process()
        {
            if (!OnScene && Game.LocalPlayer.Character.DistanceTo(SusVehicle) > 450f)
            {
                if (!ToFar)
                {
                    SusVehicleBlip = SusVehicle.AttachBlip();
                    SusVehicleBlip.Color = Color.Red;
                    SusVehicleBlip.EnableRoute(System.Drawing.Color.Yellow);
                }
                Game.DisplaySubtitle("Go to the area of the ~r~pursuit~w~");
                ToFar = true;
            }
            if (!PursuitStarted && Game.LocalPlayer.Character.DistanceTo(SusVehicle) <= 450f)
            {
                OnScene = true;
                if (ToFar)
                {
                    SusVehicleBlip.Delete();
                }
                Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                if (PassengerChooser == 1 && PassengerAvailable)
                {
                    LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Passenger);
                }
                LSPD_First_Response.Mod.API.Functions.AddCopToPursuit(Pursuit, Cop);
                PursuitStarted = true;

                Game.LogTrivial("-!!- Forestry Callouts - |HighSpeedPursuit| - Pursuit Started! -!!-");

            }
            if (OnScene && !CloseToPursuit && Game.LocalPlayer.Character.DistanceTo(SusVehicle) > 100f)
            {
                Game.DisplaySubtitle("Catch up to the ~r~pursuit~w~");
                CloseToPursuit = true;
            }
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
                Game.LogTrivial("-!!- Forestry Callouts - |HighSpeedPursuit| - Callout was ended due to players death -!!-");
                End();
            }
            if (Game.IsKeyDown(IniSettings.InputEndCalloutKey)) //If player presses "End" it will forcefully clean the callout up
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", Spawnpoint);
                Game.DisplayNotification("~g~Dispatch:~w~ All Units, Pursuit Of Offroad Vehicle Code 4");
                Game.LogTrivial("-!!- Forestry Callouts - |HighSpeedPursuit| - Callout was force ended by player -!!-");
                End();
            }

            base.Process();
        }
        public override void End()
        {
            if (Suspect.Exists())
            {
                Suspect.Dismiss();
            }
            if (SusVehicle.Exists())
            {
                SusVehicle.Dismiss();
            }
            if (Cop.Exists())
            {
                Cop.Dismiss();
            }
            if (SusVehicleBlip.Exists())
            {
                SusVehicleBlip.Delete();
            }
            if (Passenger.Exists())
            {
                Passenger.Dismiss();
            }
            Game.LogTrivial("-!!- Forestry Callouts - |HighSpeedPursuit| - Cleaned Up -!!-");
            base.End();
        }
    }
}
