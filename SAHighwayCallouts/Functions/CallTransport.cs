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
        private static bool IsTransportDriving;

        private static bool startLoop;
        private static bool transportStopped;
        
        internal static void CallTransport(in Ped pickupPed, in Vector3 spawnpoint, in string currentCounty, in int transportKind)
        {
            //Sets spawnpoints for the spawnpoint of the transportation.
            Game.LogTrivial("-!!- SAHighwayCallouts - |TransportFunction| - Transport Called!");
            Vector3 tSpawnpoint = spawnpoint.Around2D(200f, 225f);
            Vector3 finalTSpawnpoint = World.GetNextPositionOnStreet(tSpawnpoint);

            //If transport kind is 1, transport is police.
            if (transportKind == 1)
            {
                SAHC_Functions.SpawnPoliceVehicle(out tVehicle, finalTSpawnpoint, 0f, currentCounty);
                SAHC_Functions.SpawnPolcePed(out tPed, finalTSpawnpoint, 0f, currentCounty);
                Game.LogTrivial("-!!- SAHighwayCallouts - |TransportFunction| - Police Transport Spawned Successfully!");   
            }

            //If transport kind is 2, transport is taxi.
            if (transportKind == 2)
            {
                SAHC_Functions.SpawnTaxiVehicle(out tVehicle, finalTSpawnpoint, 0f);
                tPed = new Ped("a_m_m_ktown_01", spawnpoint, 0f);
                Game.LogTrivial("-!!- SAHighwayCallouts - |TransportFunction| - Taxi Transport Spawned Successfully!");
            }
            //Warps transport into transport vehicle
            tPed.WarpIntoVehicle(tVehicle, -1);

            //Makes transport blip depending on which transport is choosen
            tBlip = tPed.AttachBlip();
            tBlip.IsRouteEnabled = false;
            if (transportKind == 1) tBlip.Color = Color.MediumBlue;
            if (transportKind == 2) tBlip.Color = Color.Yellow;
            tBlip.Scale = 0.7f;

            //Gets drive position for transport
            Vector3 _driveToPosition = pickupPed.Position.Around2D(6f, 7f);

            //Makes the transport drive to the position if not parked in certain time will park the vehicle.
            tPed.Tasks.DriveToPosition(_driveToPosition, 10f, VehicleDrivingFlags.Normal).WaitForCompletion();
            startLoop = true;
            //Ped walks to the position. Then enters the vehicle
            pickupPed.Tasks.GoStraightToPosition(tVehicle.Position.Around2D(2f, 4f), 10f, tVehicle.Heading + 180f, 0f, -1).WaitForCompletion();
            pickupPed.Tasks.EnterVehicle(tVehicle, 2).WaitForCompletion();
            //Transport cruises with vehicle
            tPed.Tasks.CruiseWithVehicle(tVehicle, 10f, VehicleDrivingFlags.Normal);
            //Deletes transport blip
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