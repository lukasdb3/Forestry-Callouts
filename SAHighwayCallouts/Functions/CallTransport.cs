using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response;
using LSPD_First_Response.Engine;
using Rage.Native;
using LSPD_First_Response.Engine.Scripting.Entities;
using SAHighwayCallouts.Ini;
using StopThePed.API;

namespace SAHighwayCallouts.Functions
{
    internal class Transport
    {
        private static Ped tPed;
        private static Vehicle tVehicle;
        private static Blip tBlip;
        
        internal static void CallTransport(in Ped pickupPed, in Vector3 spawnpoint, in string currentCounty, in int transportKind)
        {
            Game.LogTrivial("-!!- SAHighwayCallouts - |TransportFunction| - Transport Called!");
            Vector3 tSpawnpoint = spawnpoint.Around2D(75f, 100f);
            Vector3 finalTSpawnpoint = World.GetNextPositionOnStreet(tSpawnpoint);

            if (transportKind == 1)
            {
                SAHC_Functions.SpawnPoliceVehicle(out tVehicle, finalTSpawnpoint, 0f, currentCounty);
                SAHC_Functions.SpawnPolcePed(out tPed, finalTSpawnpoint, 0f, currentCounty);
                Game.LogTrivial("-!!- SAHighwayCallouts - |TransportFunction| - Police Transport Spawned Successfully!");   
            }

            if (transportKind == 2)
            {
                SAHC_Functions.SpawnTaxiVehicle(out tVehicle, finalTSpawnpoint, 0f);
                tPed = new Ped("a_m_m_ktown_01", spawnpoint, 0f);
                Game.LogTrivial("-!!- SAHighwayCallouts - |TransportFunction| - Taxi Transport Spawned Successfully!");
            }
            tPed.WarpIntoVehicle(tVehicle, -1);

            tBlip = tPed.AttachBlip();
            tBlip.IsRouteEnabled = false;
            if (transportKind == 1) tBlip.Color = Color.MediumBlue;
            if (transportKind == 2) tBlip.Color = Color.Yellow;
            tBlip.Scale = 0.7f;

            Vector3 _driveToPosition = spawnpoint.Around2D(5f, 7f);
            Vector3 _finalDriveToPosition = World.GetNextPositionOnStreet(_driveToPosition);

            tPed.Tasks.DriveToPosition(_finalDriveToPosition, 10f, VehicleDrivingFlags.Normal).WaitForCompletion();
            pickupPed.Tasks.GoStraightToPosition(tVehicle.Position.Around2D(2f, 4f), 10f, tVehicle.Heading + 180f, 0f, -1).WaitForCompletion();
            pickupPed.Tasks.EnterVehicle(tVehicle, 2).WaitForCompletion();
            tPed.Tasks.CruiseWithVehicle(tVehicle, 10f, VehicleDrivingFlags.Normal);
            if (tBlip) tBlip.Delete();
            Game.LogTrivial("-!!- SAHighwayCallouts - |TransportFunction| - Transport Leaving!");
            return;
        }

        internal static void destroyTransport()
        {
            if (tPed) tPed.Dismiss();
            if (tVehicle) tVehicle.Dismiss();
            if (tBlip) tBlip.Delete();
            Game.LogTrivial("-!!- SAHighwayCallouts - |TransportFunction| - Transport dismissed successfully!");
        }
    }
}