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

        internal static string TranslateCallsignToAudio(this string callsign)
        {
            string[] divs = { "1","2","3","4","5","6","7","8","9","10"};
            string[] unitTypes = { "ADAM", "BOY", "CHARLES", "DAVID", "EDWARD", "FRANK", "GEORGE", "HENRY", "HUNDRED", "IDA", "JOHN", "KING", "LINCOLN", "MARY", "NORA", "OCEAN", "OH", "PAUL", "QUEEN", "ROBERT", "SAM", "TOM", "UNION", "VICTOR", "WILLIAM", "XRAY", "YOUNG", "ZEBRA"};
            string[] beats = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12","13", "14", "15", "16", "17", "18","19", "20", "21", "22", "23", "24","30", "40", "50", "60", "70", "80","90", "100"};
            var split = callsign.Split('-');

            try
            {
                var division = split[1];
                var unitType = split[2];
                var beat = split[3];
                if (divs.TakeWhile(div => div != division).Any()) throw new ArgumentException("division is not valid");
                if (unitTypes.TakeWhile(unit => unit != unitType).Any()) throw new ArgumentException("unit not valid");
                if (beats.TakeWhile(beet => beet != beat).Any()) throw new ArgumentException("beat is invalid");
                return "GP_DIVISION_"+division+" GP_UT_"+unitType+" GP_BEAT_"+beat;
            }
            catch (Exception e)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~g~FORESTRY CALLOUTS WARNING",
                    "~g~CALLSIGN INVALID", 
                    "One or more of the following is invalid, division, unitType, beat. Please check rage log for more info!");
                Game.Console.Print("!!! ERROR !!! - Forestry Callouts Callsign Error");
                Game.Console.Print("A part of, or all, of your callsign is invalid. Please check the readme on how to configure the callsign!");
                Game.Console.Print("ERROR ~ "+e);
                Game.Console.Print("Callsign set to 1-LINCOLN-18");
                return "GP_DIVISION_1 GP_UT_LINCOLN GP_BEAT_18";
            }
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