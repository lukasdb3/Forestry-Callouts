using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response;
using StopThePed.API;
using LSPD_First_Response.Engine;
using Rage.Native;
using LSPD_First_Response.Engine.Scripting.Entities;
using ForestryCallouts.Ini;

namespace ForestryCallouts.SimpleFunctions
{
    internal class CFunctions
    {
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
            });
        }
        internal static void SpawnNormalCar(out Vehicle cVehicle, Vector3 Spawnpoint, float heading) //Spawn normal random car..
        {
            Model[] vehicleModels = { "DUKES", "BALLER", "BALLER2", "BISON", "BISON2", "BJXL", "CAVALCADE", "CHEETAH", "COGCABRIO", "ASEA", "ADDER", "FELON", "FELON2", "ZENTORNO", "WARRENER", "RAPIDGT", "INTRUDER", "FELTZER2", "FQ2", "RANCHERXL", "REBEL", "SCHWARZER", "COQUETTE", "CARBONIZZARE", "EMPEROR", "SULTAN", "EXEMPLAR", "MASSACRO", "DOMINATOR", "ASTEROPE", "PRAIRIE", "NINEF", "WASHINGTON", "CHINO", "CASCO", "INFERNUS", "ZTYPE", "DILETTANTE", "VIRGO", "F620", "PRIMO", "SULTAN", "EXEMPLAR", "F620", "FELON2", "FELON", "SENTINEL", "WINDSOR", "DOMINATOR", "DUKES", "GAUNTLET", "VIRGO", "ADDER", "BUFFALO", "ZENTORNO", "MASSACRO" };
            cVehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], Spawnpoint, heading);
            cVehicle.IsPersistent = true;
        }
        internal static void SpawnOffroadCar(out Vehicle cVehicle, Vector3 Spawnpoint, float heading) //Spawn offroad random car..
        {
            Model[] vehicleModels = { "BISON", "BODHI2", "BFINJECTION", "REBEL", "REBEL2" };
            cVehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], Spawnpoint, heading);
            cVehicle.IsPersistent = true;
        }
        internal static void SpawnRangerVehicle(out Vehicle vehicle, Vector3 spawnpoint, float heading) //Spawn normal random car..
        {
            Game.LogTrivial("-!!- ForestryCallouts - |SpawnRangerVehicle| - Choosing Vehicle!");
            string[] vehicleModels = IniSettings.RangerBackupModelsArray;
            vehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], spawnpoint, heading);
            vehicle.IsPersistent = true;
            
            Game.LogTrivial("-!!- ForestryCallouts - |SpawnRangerVehicle| - Vehicle Model Choosed: "+vehicle.Model.Name.ToUpper()+"-!!-");
        }
        
        internal static void SpawnAnimalControl(out Vehicle vehicle, Vector3 spawnpoint, float heading) //Spawn normal random car..
        {
            Game.LogTrivial("-!!- ForestryCallouts - |SpawnAnimalControl| - Choosing Vehicle!");
            string[] vehicleModels = IniSettings.RangerBackupModelsArray;
            vehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], spawnpoint, heading);
            vehicle.IsPersistent = true;
            
            Game.LogTrivial("-!!- ForestryCallouts - |SpawnAnimalControl| - Vehicle Model Choosed: "+vehicle.Model.Name.ToUpper()+"-!!-");
        }
        internal static void SpawnSemiTruck(out Vehicle cVehicle, Vector3 Spawnpoint, float heading) //Spawn offroad semi truck..
        {
            Model[] vehicleModels = {"phantom", "phantom3"};
            cVehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], Spawnpoint, heading);
            cVehicle.IsPersistent = true;
        }
        internal static void SpawnFastOffroadVeh(out Vehicle cVehicle, Vector3 Spawnpoint, float heading) //Spawn fast offroad random car..
        {
            Model[] vehicleModels = { "blazer", "dune", "outlaw", "verus", "vagrant", "blazer3", "bf400", "manchez" };
            cVehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], Spawnpoint, heading);
            cVehicle.IsPersistent = true;
        }
        internal static void SpawnCamperVeh(out Vehicle cVehicle, Vector3 Spawnpoint, float heading) //Spawn camper..
        {
            Model[] vehicleModels = { "journey", "camper"};
            cVehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], Spawnpoint, heading);
            cVehicle.IsPersistent = true;
        }

        internal static void SpawnAnyCar(out Vehicle cVehicle, Vector3 Spawnpoint, float heading = 0) //Spawn ANY random car..
        {
            Model[] vehicleModels = { "NINFEF2", "BUS", "COACH", "AIRBUS", "AMBULANCE", "BARRACKS", "BARRACKS2", "BALLER", "BALLER2", "BANSHEE", "BJXL", "BENSON", "BOBCATXL", "BUCCANEER", "BUFFALO", "BUFFALO2", "BULLDOZER", "BULLET", "BURRITO", "BURRITO2", "BURRITO3", "BURRITO4", "BURRITO5", "CAVALCADE", "CAVALCADE2", "POLICET", "GBURRITO", "CAMPER", "CARBONIZZARE", "CHEETAH", "COMET2", "COGCABRIO", "COQUETTE", "GRESLEY", "DUNE2", "HOTKNIFE", "DUBSTA", "DUBSTA2", "DUMP", "DOMINATOR", "EMPEROR", "EMPEROR2", "EMPEROR3", "ENTITYXF", "EXEMPLAR", "ELEGY2", "F620", "FBI", "FBI2", "FELON", "FELON2", "FELTZER2", "FIRETRUK", "FQ2", "FUGITIVE", "FUTO", "GRANGER", "GAUNTLET", "HABANERO", "INFERNUS", "INTRUDER", "JACKAL", "JOURNEY", "JB700", "KHAMELION", "LANDSTALKER", "MESA", "MESA2", "MESA3", "MIXER", "MINIVAN", "MIXER2", "MULE", "MULE2", "ORACLE", "ORACLE2", "MONROE", "PATRIOT", "PBUS", "PACKER", "PENUMBRA", "PEYOTE", "POLICE", "POLICE2", "POLICE3", "POLICE4", "PHANTOM", "PHOENIX", "PICADOR", "POUNDER", "PRANGER", "PRIMO", "RANCHERXL", "RANCHERXL2", "RAPIDGT", "RAPIDGT2", "RENTALBUS", "RUINER", "RIOT", "RIPLEY", "SABREGT", "SADLER", "SADLER2", "SANDKING", "SANDKING2", "SHERIFF", "SHERIFF2", "SPEEDO", "SPEEDO2", "STINGER", "STOCKADE", "STINGERGT", "SUPERD", "STRATUM", "SULTAN", "AKUMA", "PCJ", "FAGGIO2", "DAEMON", "BATI2" };
            cVehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], Spawnpoint);
            cVehicle.IsPersistent = true;
        }
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
        internal static void SpawnHunterPed(out Ped cPed, Vector3 Spawnpoint, float heading) //this dont work mann
        {
            Model[] pedModels = { "cs_hunter", "ig_hunter" };
            cPed = new Ped(pedModels[new Random().Next(pedModels.Length)], Spawnpoint, heading);
            cPed.IsPersistent = true;
            cPed.BlockPermanentEvents = true;
        }
        internal static void SpawnSuspiciousVehiclePed(out Ped cPed, Vector3 Spawnpoint, float heading) //this dont work mann
        {
            Model[] pedModels = { "a_m_m_hillbilly_01", "a_m_m_hillbilly_02", "u_m_y_hippie_01", "a_f_y_hippie_01", "a_m_y_hippy_01", "u_m_y_justin" };
            cPed = new Ped(pedModels[new Random().Next(pedModels.Length)], Spawnpoint, heading);
            cPed.IsPersistent = true;
            cPed.BlockPermanentEvents = true;
        }
        internal static void SpawnHookerPed(out Ped cPed, Vector3 Spawnpoint, float heading)
        {
            String[] pedModels = { "s_f_y_hooker_01", "s_f_y_hooker_02", "s_f_y_hooker_03" };
            cPed = new Ped(pedModels[new Random().Next(pedModels.Length)], Spawnpoint, heading);
            cPed.IsPersistent = true;
            cPed.BlockPermanentEvents = true;
        }
        internal static void SpawnAnimalPed(out Ped cPed, Vector3 Spawnpoint, float heading)
        {
            String[] pedModels = { "a_c_pig", "a_c_mtlion", "a_c_boar", "a_c_coyote", "a_c_deer", "a_c_panther" };
            cPed = new Ped(pedModels[new Random().Next(pedModels.Length)], Spawnpoint, heading);
            cPed.IsPersistent = true;
            cPed.BlockPermanentEvents = true;
        }
        internal static Ped SetWanted(Ped wPed, bool isWanted) //Used to set a ped as wanted.
        {
            Persona thePersona = LSPD_First_Response.Mod.API.Functions.GetPersonaForPed(wPed);
            thePersona.Wanted = true;
            return wPed;
        }

        internal static void FireControl(Vector3 position, int children, bool isGasFire)
        {
            if (children > 25) return;
            NativeFunction.Natives.StartScriptFire(position.X, position.Y, position.Z, children, isGasFire);
        }

        internal static void NormalWeaponChooser(Ped cPed, short ammo, bool isNow)
        {
            String[] weaponModels = { "WEAPON_PISTOL", "WEAPON_COMBATPISTOL", "WEAPON_PISTOL50", "WEAPON_SAWNOFFSHOTGUN" };
            WeaponDescriptor cWeapon = (WeaponDescriptor)weaponModels[new Random().Next(weaponModels.Length)];

            cPed.Inventory.GiveNewWeapon(cWeapon, ammo, isNow);
        }
        internal static void MeleeWeaponChooser(Ped cPed, short ammo, bool isNow)
        {
            String[] weaponModels = { "WEAPON_KNIFE", "WEAPON_DAGGER", "WEAPON_BAT", "WEAPON_BOTTLE", "WEAPON_UNARMED", "WEAPON_HATCHET", "WEAPON_KNUCKLE", "WEAPON_MACHETE", "WEAPON_CROWBAR", "WEAPON_HAMMER"};
            WeaponDescriptor cWeapon = (WeaponDescriptor)weaponModels[new Random().Next(weaponModels.Length)];

            cPed.Inventory.GiveNewWeapon(cWeapon, ammo, isNow);
        }
        
        internal static void RifleWeaponChooser(Ped cPed, short ammo, bool isNow)
        {
            String[] weaponModels = { "weapon_assaultrifle", "weapon_carbinerifle", "weapon_advancedrifle", "weapon_specialcarbine"};
            WeaponDescriptor cWeapon = (WeaponDescriptor)weaponModels[new Random().Next(weaponModels.Length)];

            cPed.Inventory.GiveNewWeapon(cWeapon, ammo, isNow);
        }
        
        internal static void PistolWeaponChooser(Ped cPed, short ammo, bool isNow)
        {
            String[] weaponModels = { "weapon_pistol", "weapon_combatpistol", "weapon_pistol50", "weapon_revolver"};
            WeaponDescriptor cWeapon = (WeaponDescriptor)weaponModels[new Random().Next(weaponModels.Length)];

            cPed.Inventory.GiveNewWeapon(cWeapon, ammo, isNow);
        }
        
        internal static void ShotgunWeaponChooser(Ped cPed, short ammo, bool isNow)
        {
            String[] weaponModels = { "weapon_pumpshotgun", "weapon_sawnoffshotgun", "weapon_dbshotgun"};
            WeaponDescriptor cWeapon = (WeaponDescriptor)weaponModels[new Random().Next(weaponModels.Length)];

            cPed.Inventory.GiveNewWeapon(cWeapon, ammo, isNow);
        }

        internal static void PedPersonaChooser(in Ped cped, bool RunDrunk, bool RunWanted)
        {
            int PedIsDrunk = new Random().Next(1, 5);
            bool DrunkRan = false;
            int PedIsWanted = new Random().Next(1, 5);
            bool WantedRan = false;
            if (PedIsDrunk == 1 && DrunkRan == false && RunDrunk == true)
            {
                SimpleFunctions.CFunctions.SetDrunk(cped, true);
                DrunkRan = true;
            }
            else
            {
                Game.LogTrivial("-!!- Forestry Callouts - |PedPersonaChooser| - Ped is not drunk -!!-");
                DrunkRan = true;
            }

            if (PedIsWanted == 1 && WantedRan == false && RunWanted == true)
            {
                SimpleFunctions.CFunctions.SetWanted(cped, true);
                WantedRan = true;
            }
            else
            {
                Game.LogTrivial("-!!- Forestry Callouts - |PedPersonaChooser| - Ped is not wanted -!!-");
                WantedRan = true;
            }
        }
        internal static void WreckedVehicleCarChooser(out Vehicle cVehicle, Vector3 Spawnpoint, float heading) //Spawn offroad random car..
        {
            Model[] vehicleModels = { "BISON", "BODHI2", "BFINJECTION", "REBEL", "REBEL2" };
            cVehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], Spawnpoint);
            cVehicle.IsPersistent = true;
            
        }
        internal static void SuspectViolChooser(out bool SusIsViolent)
        {
            int ViolentChooser = new Random().Next(1, Ini.IniSettings.SuspectViolentOption);
            if (ViolentChooser == 1)
            {
                Game.LogTrivial("-!!- Forestry Callouts - |SuspectViolChooser| - Suspect is set to be violent -!!-");
                SusIsViolent = true;
            }
            else
            {
                Game.LogTrivial("-!!- Forestry Callouts - |SuspectViolChooser| - Suspect is set not to be violent -!!-");
                SusIsViolent = false;
            }
        }
    }     
}
