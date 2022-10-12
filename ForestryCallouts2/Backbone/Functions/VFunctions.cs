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
    }
}