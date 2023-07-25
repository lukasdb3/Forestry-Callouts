#region Refrences
//System
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
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

        internal static List<Ped> GetValidPedsNearby(int max)
        {
            //get all peds
            Logger.DebugLog("GetValidPedsNearby", "Getting all peds in the world");
            var allPeds = World.GetAllPeds();
            Logger.DebugLog("GetValidPedsNearby", "All Peds Count = " + allPeds.Length);
            //get peds <= 30 from the player
            var pedsInRange = new List<Ped>{};
            Logger.DebugLog("GetValidPedsNearby", "Getting peds in range of player");
            foreach (var ped in allPeds)
            {
                if (ped != Game.LocalPlayer.Character && Game.LocalPlayer.Character.DistanceTo(ped) <= 30f)
                {
                    pedsInRange.Add(ped);
                    Logger.DebugLog("GetValidPedsNearby", "Added " +ped.Model.Name+ " to list");
                }
            }
            //sort the peds closest from farthest from player, if there are peds nearby.
            var closePeds = new List<Ped>();
            if (!pedsInRange.Any()) return closePeds;
            Logger.DebugLog("GetValidPedsNearby", "Ordering peds from closest to farthest");
            var sortedPeds = pedsInRange.OrderBy(x => x.DistanceTo(Game.LocalPlayer.Character));
            Logger.DebugLog("GetValidPedsNearby", "Figuring out if we are taking max peds or the whole list");
            closePeds = sortedPeds.Count() > 10 ? sortedPeds.Take(10).ToList() : sortedPeds.ToList();
            Logger.DebugLog("GetValidPedsNearby", "Returning list!");
            return closePeds;
        }
        
        internal static bool IsKeyAndModifierDown(Keys key, Keys modifier)
        {
            return Game.IsKeyDown(key) && Control.ModifierKeys == modifier;
        }

        internal static String TranslateCallsignToAudio()
        {
            string[] divs = { "GP_DIVISION_1","GP_DIVISION_2","GP_DIVISION_3","GP_DIVISION_4","GP_DIVISION_5","GP_DIVISION_6","GP_DIVISION_7","GP_DIVISION_8","GP_DIVISION_9","GP_DIVISION_10"};
            string[] unitTypes = { "GP_UT_ADAM", "GP_UT_BOY", "GP_UT_CHARLES", "GP_UT_DAVID", "GP_UT_EDWARD", "GP_UT_FRANK", "GP_UT_GEORGE", "GP_UT_HENRY", "GP_UT_HUNDRED", "GP_UT_IDA", "GP_UT_JOHN", "GP_UT_KING", "GP_UT_LINCOLN", "GP_UT_MARY", "GP_UT_NORA", "GP_UT_OCEAN", "GP_UT_OH", "GP_UT_PAUL", "GP_UT_QUEEN", "GP_UT_ROBERT", "GP_UT_SAM", "GP_UT_TOM", "GP_UT_UNION", "GP_UT_VICTOR", "GP_UT_WILLIAM", "GP_UT_XRAY", "GP_UT_YOUNG", "GP_UT_ZEBRA"};
            string[] beats = { "GP_BEAT_1", "GP_BEAT_2", "GP_BEAT_3", "GP_BEAT_4", "GP_BEAT_5", "GP_BEAT_6", "GP_BEAT_7", "GP_BEAT_8", "GP_BEAT_9", "GP_BEAT_10", "GP_BEAT_11", "GP_BEAT_12","GP_BEAT_13", "GP_BEAT_14", "GP_BEAT_15", "GP_BEAT_16", "GP_BEAT_17", "GP_BEAT_18","GP_BEAT_19", "GP_BEAT_20", "GP_BEAT_21", "GP_BEAT_22", "GP_BEAT_23", "GP_BEAT_24","GP_BEAT_30", "", "", "", "", "","", "", "", "", "", "","", "", "", "", "", "" };
            var callsign = IniSettings.Callsign.ToUpper();
            var split = callsign.Split('-');

            try
            {
                var division = split[1];
                var unitType = split[2];
                var beat = split[3];
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return callsign;
            }
            
            return callsign;
        }

        //Spawn Peds
        internal static void SpawnHikerPed(out Ped cPed, Vector3 Spawnpoint, float heading)
        {
            String[] pedModels = { "a_m_y_hiker_01", "a_f_y_hiker_01" };
            cPed = new Ped(pedModels[new Random().Next(pedModels.Length)], Spawnpoint, heading);
            cPed.IsPersistent = true;
            cPed.BlockPermanentEvents = true;
        }
        internal static void SpawnCountryPed(out Ped cPed, Vector3 Spawnpoint, float heading) 
        {
            Model[] pedModels = { "a_m_m_hillbilly_01", "a_m_m_hillbilly_02", "u_m_y_hippie_01", "a_f_y_hippie_01", "a_m_y_hippy_01" };
            cPed = new Ped(pedModels[new Random().Next(pedModels.Length)], Spawnpoint, heading);
            cPed.IsPersistent = true;
            cPed.BlockPermanentEvents = true;
        }
        internal static void SpawnAnimal(out Ped cPed, Vector3 Spawnpoint, float heading) 
        {
            Model[] pedModels = { "a_c_boar", "a_c_coyote", "a_c_deer", "a_c_rabbit_01"};
            cPed = new Ped(pedModels[new Random().Next(pedModels.Length)], Spawnpoint, heading);
            cPed.IsPersistent = true;
            cPed.BlockPermanentEvents = true;
        }
        internal static void SpawnBeachPed(out Ped cPed, Vector3 Spawnpoint, float heading) 
        {
            Model[] pedModels = { "a_f_m_beach_01", "a_f_y_beach_01", "a_m_m_beach_01", "a_m_m_beach_02" };
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
        internal static void SpawnFastOffroadCar(out Vehicle cVehicle, Vector3 Spawnpoint, float heading) 
        {
            string[] vehicleModels = IniSettings.OffRoadFastVehicles;
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
        internal static void SpawnSemiTrucks(out Vehicle cVehicle, Vector3 Spawnpoint, float heading) 
        {
            string[] vehicleModels = IniSettings.SemiTrucks;
            cVehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], Spawnpoint, heading);
            cVehicle.IsPersistent = true;
        }
        internal static void SpawnBoat(out Vehicle cVehicle, Vector3 Spawnpoint, float heading) 
        {
            string[] vehicleModels = IniSettings.Boats;
            cVehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], Spawnpoint, heading);
            cVehicle.IsPersistent = true;
        }
        
        //Weapons
        internal static void MeleeWeaponChooser(Ped cPed, short ammo, bool isNow)
        {
            String[] weaponModels = { "WEAPON_KNIFE", "WEAPON_DAGGER", "WEAPON_BAT", "WEAPON_BOTTLE", "WEAPON_UNARMED", "WEAPON_HATCHET", "WEAPON_KNUCKLE", "WEAPON_MACHETE", "WEAPON_CROWBAR", "WEAPON_HAMMER"};
            WeaponDescriptor cWeapon = weaponModels[new Random().Next(weaponModels.Length)];

            cPed.Inventory.GiveNewWeapon(cWeapon, ammo, isNow);
        }
        
        internal static void RifleWeaponChooser(Ped cPed, short ammo, bool isNow)
        {
            String[] weaponModels = { "weapon_assaultrifle", "weapon_carbinerifle", "weapon_advancedrifle", "weapon_specialcarbine"};
            WeaponDescriptor cWeapon = weaponModels[new Random().Next(weaponModels.Length)];

            cPed.Inventory.GiveNewWeapon(cWeapon, ammo, isNow);
        }
        
        internal static void PistolWeaponChooser(Ped cPed, short ammo, bool isNow)
        {
            String[] weaponModels = { "weapon_pistol", "weapon_combatpistol", "weapon_pistol50", "weapon_revolver"};
            WeaponDescriptor cWeapon = weaponModels[new Random().Next(weaponModels.Length)];

            cPed.Inventory.GiveNewWeapon(cWeapon, ammo, isNow);
        }
        
        internal static void ShotgunWeaponChooser(Ped cPed, short ammo, bool isNow)
        {
            String[] weaponModels = { "weapon_pumpshotgun", "weapon_sawnoffshotgun", "weapon_dbshotgun"};
            WeaponDescriptor cWeapon = weaponModels[new Random().Next(weaponModels.Length)];

            cPed.Inventory.GiveNewWeapon(cWeapon, ammo, isNow);
        }
    }
}