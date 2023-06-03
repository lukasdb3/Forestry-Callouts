#region Refrences
//System
using System;
using System.Text.RegularExpressions;
//Rage
using Rage;
using Rage.Native;
//ForestryCallouts2
using ForestryCallouts2.Backbone.IniConfiguration;
using LSPD_First_Response.Mod.Callouts;

#endregion

namespace ForestryCallouts2.Backbone.Functions
{
    internal static class  CFunctions
    {
        
        // Main
        internal static void SetDrunk(Ped Bad, bool isDrunk)
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                Bad.Metadata.stpAlcoholDetected = isDrunk;
                var drunkAnimset = new AnimationSet("move_m@drunk@verydrunk");
                drunkAnimset.LoadAndWait();
                Bad.MovementAnimationSet = drunkAnimset;
                Rage.Native.NativeFunction.Natives.SET_PED_IS_DRUNK(Bad, isDrunk);
                StopThePed.API.Functions.setPedAlcoholOverLimit(Bad, true);
            });
        }
        
        public static string RemoveIntegers(this string input)
        {
            return Regex.Replace(input, @"[\d-]", string.Empty);
        }

        //Spawn Peds
        internal static void SpawnHikerPed(out Ped cPed, Vector3 Spawnpoint, float heading)
        {
            String[] pedModels = { "a_m_y_hiker_01", "a_f_y_hiker_01" };
            cPed = new Ped(pedModels[new Random().Next(pedModels.Length)], Spawnpoint, heading);
            cPed.IsPersistent = true;
            cPed.BlockPermanentEvents = true;
        }
        internal static void SpawnCountryPed(out Ped cPed, Vector3 Spawnpoint, float heading) //this dont work mann
        {
            Model[] pedModels = { "a_m_m_hillbilly_01", "a_m_m_hillbilly_02", "u_m_y_hippie_01", "a_f_y_hippie_01", "a_m_y_hippy_01" };
            cPed = new Ped(pedModels[new Random().Next(pedModels.Length)], Spawnpoint, heading);
            cPed.IsPersistent = true;
            cPed.BlockPermanentEvents = true;
        }
        internal static void SpawnAnimal(out Ped cPed, Vector3 Spawnpoint, float heading) //this dont work mann
        {
            Model[] pedModels = { "a_c_boar", "a_c_coyote", "a_c_deer", "a_c_mtlion" };
            cPed = new Ped(pedModels[new Random().Next(pedModels.Length)], Spawnpoint, heading);
            cPed.IsPersistent = true;
            cPed.BlockPermanentEvents = true;
        }
        
        //Spawn Vehicles
        internal static void SpawnNormalCar(out Vehicle vehicle, Vector3 spawnpoint, float heading)
        {
            string[] vehicleModels = IniSettings.NormalVehicles;
            vehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], spawnpoint, heading);
            vehicle.IsPersistent = true;
        }
        internal static void SpawnOffroadCar(out Vehicle cVehicle, Vector3 Spawnpoint, float heading) 
        {
            string[] vehicleModels = IniSettings.OffRoadVehicles;
            cVehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], Spawnpoint, heading);
            cVehicle.IsPersistent = true;
        }
        internal static void SpawnAnimalControl(out Vehicle cVehicle, Vector3 Spawnpoint, float heading) 
        {
            string[] vehicleModels = IniSettings.AnimalControlVehicles;
            cVehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], Spawnpoint, heading);
            cVehicle.IsPersistent = true;
        }
        internal static void SpawnDirtBike(out Vehicle cVehicle, Vector3 Spawnpoint, float heading) 
        {
            string[] vehicleModels = IniSettings.Dirtbikes;
            cVehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], Spawnpoint, heading);
            cVehicle.IsPersistent = true;
        }
        internal static void SpawnAtv(out Vehicle cVehicle, Vector3 Spawnpoint, float heading) 
        {
            string[] vehicleModels = IniSettings.AtvVehicles;
            cVehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], Spawnpoint, heading);
            cVehicle.IsPersistent = true;
        }
        
        
        //Callout Interface Functions
        public static void CISendCalloutDetails(LSPD_First_Response.Mod.Callouts.Callout sender, string priority, string agency)
        {
            try
            {
                CalloutInterface.API.Functions.SendCalloutDetails(sender, priority, agency);
            }
            catch (Exception ex)
            {
                Game.LogTrivial("-!!- Forestry Callouts ERROR - |SendCalloutDetails| - There was en error sending callout details with Callout Interface please send this log to https://dsc.gg/ulss -!!-");
                Game.LogTrivial(ex.Message);
            }
        }
        
        public static void CISendMessage(LSPD_First_Response.Mod.Callouts.Callout sender, string message)
        {
            try
            {
                CalloutInterface.API.Functions.SendMessage(sender, message);
            }
            catch (Exception ex)
            {
                Game.LogTrivial("-!!- Forestry Callouts ERROR - |SendMessage| - There was en error sending a MDT message with Callout Interface please send this log to https://dsc.gg/ulss -!!-");
                Game.LogTrivial(ex.Message);
            }
        }
    }
}