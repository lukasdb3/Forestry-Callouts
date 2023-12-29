#region Refrences
//System
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
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

        internal static Blip CreateBlip(Ped ped, bool enableRoute ,Color color, Color routeColor, float scale)
        {
            Blip blip = ped.AttachBlip();
            if (enableRoute) blip.EnableRoute(routeColor);
            blip.Color = color;
            blip.Scale = scale;
            return blip;
        }
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
            Log.Debug("GetValidPedsNearby", "Getting all peds in the world");
            var allPeds = World.GetAllPeds();
            Log.Debug("GetValidPedsNearby", "All Peds Count = " + allPeds.Length);
            //get peds <= 30 from the player
            var pedsInRange = new List<Ped>{};
            Log.Debug("GetValidPedsNearby", "Getting peds in range of player");
            foreach (var ped in allPeds)
            {
                if (ped != Game.LocalPlayer.Character && Game.LocalPlayer.Character.DistanceTo(ped) <= 30f)
                {
                    pedsInRange.Add(ped);
                    Log.Debug("GetValidPedsNearby", "Added " +ped.Model.Name+ " to list");
                }
            }
            //sort the peds closest from farthest from player, if there are peds nearby.
            var closePeds = new List<Ped>();
            if (!pedsInRange.Any()) return closePeds;
            Log.Debug("GetValidPedsNearby", "Ordering peds from closest to farthest");
            var sortedPeds = pedsInRange.OrderBy(x => x.DistanceTo(Game.LocalPlayer.Character));
            Log.Debug("GetValidPedsNearby", "Figuring out if we are taking max peds or the whole list");
            closePeds = sortedPeds.Count() > 10 ? sortedPeds.Take(10).ToList() : sortedPeds.ToList();
            Log.Debug("GetValidPedsNearby", "Returning list!");
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
                var division = split[0];
                var unitType = split[1];
                var beat = split[2];
                if (!divs.Contains(division)) throw new ArgumentException("division is not valid");
                if (!unitTypes.Contains(unitType)) throw new ArgumentException("unitType is not valid");
                if (!beats.Contains(beat)) throw new ArgumentException("beat is not valid");
                return "GP_DIVISION_"+division+" GP_UT_"+unitType+" GP_BEAT_"+beat;
            }
            catch (Exception e)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~g~FORESTRY CALLOUTS WARNING",
                    "~g~CALLSIGN INVALID", 
                    "One or more of the following is invalid, division, unitType, beat. Please check rage log for more info!");
                Game.Console.Print("----------------------------------------------------------------------------------------------------------");
                Game.Console.Print("!!! ERROR !!! - Forestry Callouts Callsign Error");
                Game.Console.Print("A part of, or all, of your callsign is invalid. Please check the readme on how to configure the callsign!");
                Game.Console.Print("ERROR ~ "+e);
                Game.Console.Print("Callsign set to 1-LINCOLN-18");
                Game.Console.Print("----------------------------------------------------------------------------------------------------------");
                return "GP_DIVISION_1 GP_UT_LINCOLN GP_BEAT_18";
            }
        }

        internal static void GetVehiclePassengers(Vehicle vehicle, List<Ped> passengerList, Vector3 spawn)
        {
            var rand = new Random();
            var pChoice = rand.Next(1, 3);
            GameFiber fiber = null;   
            if (pChoice == 1)
            {
                if (vehicle.FreePassengerSeatsCount >= 1)
                {
                    var rnd = rand.Next(1, vehicle.FreePassengerSeatsCount + 1);
                    Log.Debug("GET VEHICLE PASSENGERS", "Free Passenger Seats Count " + vehicle.FreePassengerSeatsCount);
                    Log.Debug("GET VEHICLE PASSENGERS", "Spawning "+ rnd + " criminal passengers");
                    fiber = GameFiber.StartNew(delegate
                    {
                        var i = 0;
                        for (i = 0; i!=rnd; i++)
                        {
                            if (i > 4) break;
                            GameFiber.Yield();
                            Ped cped;
                            Log.Debug("GET VEHICLE PASSENGERS", "Creating Passenger..");
                            CFunctions.SpawnCountryPed(out cped, spawn, 0);
                            cped.WarpIntoVehicle(vehicle, i);
                            passengerList.Add(cped);
                        }
                        Log.Debug("GET VEHICLE PASSENGERS", "There is " + passengerList.Count.ToString() + " passengers");
                        Log.Debug("GET VEHICLE PASSENGERS", "Aborting passenger fiber");
                        fiber.Abort();
                    });
                }    
            }
            else
            {
                Log.Debug("GET VEHICLE PASSENGERS", "No passengers spawning");
            }
        }

        internal static Blip SpawnSearchArea(Vector3 position, float arnMinDistance, float arnMaxDistance, float radius, Color color,
            float alpha)
        {
            var searchArea = position.Around2D(arnMinDistance, arnMaxDistance);
            return new Blip(searchArea, radius) { Color = color, Alpha = alpha };
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
            string model = vehicleModels[new Random().Next(vehicleModels.Length)];
            Log.Debug("SPAWN NORMAL CAR", "Selected: "+model);
            vehicle = new Vehicle(model, spawnpoint, heading);
            vehicle.IsPersistent = true;
        }
        internal static void SpawnOffroadCar(out Vehicle cVehicle, Vector3 Spawnpoint, float heading) 
        {
            string[] vehicleModels = IniSettings.OffRoadVehicles;
            string model = vehicleModels[new Random().Next(vehicleModels.Length)];
            Log.Debug("SPAWN OFFROAD CAR", "Selected: "+model);
            cVehicle = new Vehicle(model, Spawnpoint, heading);
            cVehicle.IsPersistent = true;
        }
        internal static void SpawnFastOffroadCar(out Vehicle cVehicle, Vector3 Spawnpoint, float heading) 
        {
            string[] vehicleModels = IniSettings.OffRoadFastVehicles;
            string model = vehicleModels[new Random().Next(vehicleModels.Length)];
            Log.Debug("SPAWN FAST OFFROAD CAR", "Selected: "+model);
            cVehicle = new Vehicle(model, Spawnpoint, heading);
            cVehicle.IsPersistent = true;
        }
        internal static void SpawnAnimalControl(out Vehicle cVehicle, Vector3 Spawnpoint, float heading) 
        {
            string[] vehicleModels = IniSettings.AnimalControlVehicles;
            string model = vehicleModels[new Random().Next(vehicleModels.Length)];
            Log.Debug("SPAWN ANIMAL CONTROL CAR", "Selected: "+model);
            cVehicle = new Vehicle(model, Spawnpoint, heading);
            cVehicle.IsPersistent = true;
        }
        internal static void SpawnDirtBike(out Vehicle cVehicle, Vector3 Spawnpoint, float heading) 
        {
            string[] vehicleModels = IniSettings.Dirtbikes;
            string model = vehicleModels[new Random().Next(vehicleModels.Length)];
            Log.Debug("SPAWN DIRT BIKE", "Selected: "+model);
            cVehicle = new Vehicle(model, Spawnpoint, heading);
            cVehicle.IsPersistent = true;
        }
        internal static void SpawnAtv(out Vehicle cVehicle, Vector3 Spawnpoint, float heading) 
        {
            string[] vehicleModels = IniSettings.AtvVehicles;
            string model = vehicleModels[new Random().Next(vehicleModels.Length)];
            Log.Debug("SPAWN ATV", "Selected: "+model);
            cVehicle = new Vehicle(model, Spawnpoint, heading);
            cVehicle.IsPersistent = true;
        }
        internal static void SpawnSemiTrucks(out Vehicle cVehicle, Vector3 Spawnpoint, float heading) 
        {
            string[] vehicleModels = IniSettings.SemiTrucks;
            string model = vehicleModels[new Random().Next(vehicleModels.Length)];
            Log.Debug("SPAWN SEMI TRUCK", "Selected: "+model);
            cVehicle = new Vehicle(model, Spawnpoint, heading);
            cVehicle.IsPersistent = true;
        }
        internal static void SpawnBoat(out Vehicle cVehicle, Vector3 Spawnpoint, float heading) 
        {
            string[] vehicleModels = IniSettings.Boats;
            string model = vehicleModels[new Random().Next(vehicleModels.Length)];
            Log.Debug("SPAWN BOAT", "Selected: "+model);
            cVehicle = new Vehicle(model, Spawnpoint, heading);
            cVehicle.IsPersistent = true;
        }
        internal static void SpawnRangerBackup(out Vehicle cVehicle, Vector3 Spawnpoint, float heading) 
        {
            string[] vehicleModels = IniSettings.RangerVehicles;
            string model = vehicleModels[new Random().Next(vehicleModels.Length)];
            Log.Debug("SPAWN RANGER BACKUP CAR", "Selected: "+model);
            cVehicle = new Vehicle(model, Spawnpoint, heading);
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