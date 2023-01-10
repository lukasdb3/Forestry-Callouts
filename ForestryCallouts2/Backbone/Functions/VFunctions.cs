using System;
using ForestryCallouts2.Backbone.IniConfiguration;
using Rage;

namespace ForestryCallouts2.Backbone.Functions
{
    internal static class VFunctions
    {
        internal static void SpawnNormalCar(out Vehicle vehicle, Vector3 spawnpoint, float heading) //Spawn normal random car..
        {
            string[] vehicleModels = IniVehicles.NormalVehiclesArray;
            vehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], spawnpoint, heading);
            vehicle.IsPersistent = true;
            
            Logger.InfoLog("SPAWN NORMAL CAR", "Vehicle Model Chosen: "+vehicle.Model.Name.ToUpper()+"");
        }
        
        internal static void GetVehicleDirection(in Vehicle vehicle, out string direction)
        {
            direction = "null";
            double badHeading = vehicle.Heading;
            double heading = Math.Round(badHeading, 1);
            //   LESS THAN       GREATER THAN
            if (heading < 22.5f && heading > 337.5f) direction = "NORTH"; //North
            if (heading < 67.5f && heading > 22.5f) direction = "NORTH WEST"; //North West
            if (heading < 112.5f && heading > 67.5f) direction = "WEST"; //West
            if (heading < 157.5f && heading > 112.5f) direction = "SOUTH WEST"; //South West
            if (heading < 202.5f && heading > 157.5f) direction = "SOUTH"; //South
            if (heading < 247.5f && heading > 202.5f) direction = "SOUTH EAST"; //South East
            if (heading < 292.5f && heading > 247.5f) direction = "EAST"; //East
            if (heading < 337.5f && heading > 292.5f) direction = "NORTH EAST"; //North
            if (direction == "null") direction = "~r~NOT KNOWN";
        }
    }
}