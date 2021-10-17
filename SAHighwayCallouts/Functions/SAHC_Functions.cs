using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response;
using LSPD_First_Response.Engine;
using Rage.Native;
using LSPD_First_Response.Engine.Scripting.Entities;
using SAHighwayCallouts.Ini;

namespace SAHighwayCallouts.Functions
{
    internal class SAHC_Functions
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

        internal static void LuxVehicleSpawn(out Vehicle vehicle, Vector3 spawnpoint, float heading)
        {
            Game.Console.Print("-!!- SAHighwayCallouts - |LuxVehicleSpawn| - Choosing Vehicle!");
            string[] vehicleModels = Settings.luxuryVehiclesArray;
            vehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], spawnpoint, heading);
            vehicle.IsPersistent = true;
            
            Game.Console.Print("-!!- SAHighwayCallouts - |LuxVehicleSpawn| - Vehicle Model Choosed: "+vehicle.Model.Name.ToUpper()+"-!!-");
        }

        internal static void SpawnPolicePed(in string closetCounty, out Ped ped, Vector3 spawnpoint, float heading)
        {
            ped = null;
            Game.Console.Print("-!!- SAHighwayCallouts - |PolicePedSpawner| - Spawning new police ped!");
            if (closetCounty == "PaletoBay") ped = new Ped("s_f_y_sheriff_01", spawnpoint, heading);
            if (closetCounty == "BlaineCounty") ped = new Ped("s_m_y_sheriff_01", spawnpoint, heading);
            if (closetCounty == "LosSantosCounty") ped = new Ped("s_m_y_sheriff_01", spawnpoint, heading);
            if (closetCounty == "LosSantosCity") ped = new Ped("s_m_y_cop_01", spawnpoint, heading);
            Game.Console.Print("-!!- SAHighwayCallouts - |PolicePedSpawner| - police ped spawned!");
        }

        internal static void SpawnPoliceCar(in string closetCounty, out Vehicle vehicle, Vector3 spawnpoint, float heading)
        {
            Game.Console.Print("-!!- SAHighwayCallouts - |PoliceCarSpawner| - Picking AI police cruiser!");
            string[] vehicleModels = null;
            vehicle = null;
            if (closetCounty == "PaletoCounty") vehicleModels = Settings.PaletoBayCountyVehiclesArray;
            if (closetCounty == "BlaineCounty") vehicleModels = Settings.BlaineCountyVehiclesArray;
            if (closetCounty == "LosSantosCounty") vehicleModels = Settings.LosSantosCountyVehiclesArray;
            if (closetCounty == "LosSantosCity") vehicleModels = Settings.LosSantosCityVehiclesArray;

            if (vehicleModels == null)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~h~SAHighwayCallouts Warning",
                    "~b~Null Vehicle Models",
                    "There was an issue with choosing a vehicle model. Please see the log for further information");
                
                Game.Console.Print();
                Game.Console.Print("-!!- =============== SAHIGHWAYCALLOUTS WARNING =============== -!!-");
                Game.Console.Print("-!!- There was an issue with spawning a vehicle. Please send -!!-");
                Game.Console.Print("-!!- your log to Blondee or look for null vehicle models. -!!-");
                Game.Console.Print("-!!- The model was set to POLICE                         -!!-");
                Game.Console.Print("-!!- =============== SAHIGHWAYCALLOUTS WARNING =============== -!!-");
            }
            if (vehicleModels != null) vehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], spawnpoint, heading);
            if (vehicleModels == null) vehicle = new Vehicle("POLICE", spawnpoint, heading); 
            vehicle.IsPersistent = true;
            
            Game.Console.Print("-!!- SAHighwayCallouts - |PoliceCarSpawner| - Vehicle Model Chosen: "+vehicle.Model.Name.ToUpper()+"-!!-");
        }
        
        internal static void SpawnNormalCar(out Vehicle vehicle, Vector3 spawnpoint, float heading) //Spawn normal random car..
        {
            Game.Console.Print("-!!- SAHighwayCallouts - |NormalVehicleSpawner| - Choosing Vehicle!");
            string[] vehicleModels = Settings.NormalVehiclesArray;
            vehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], spawnpoint, heading);
            vehicle.IsPersistent = true;
            
            Game.Console.Print("-!!- SAHighwayCallouts - |NormalVehicleSpawner| - Vehicle Model Choosed: "+vehicle.Model.Name.ToUpper()+"-!!-");
        }
        
        internal static void SpawnSemiTruckAndTrailer(out Vehicle truck, out Vehicle trailer, Vector3 spawnpoint, float heading) //Spawn normal random car..
        {
            Game.Console.Print("-!!- SAHighwayCallouts - |SpawnSemiTruckAndTrailer| - Choosing Vehicle!");
            string[] vehicleModels = Settings.NormalVehiclesArray;
            truck = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], spawnpoint, heading);
            truck.IsPersistent = true;
            
            Game.Console.Print("-!!- SAHighwayCallouts - |SpawnSemiTruckAndTrailer| - Vehicle Model Choosed: "+truck.Model.Name.ToUpper()+"-!!-");
            
            Game.Console.Print("-!!- SAHighwayCallouts - |SpawnSemiTruckAndTrailer| - Choosing Trailer!");
            string[] vehicleModelsT = Settings.NormalVehiclesArray;
            trailer = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], spawnpoint, heading);
            trailer.IsPersistent = true;
            
            Game.Console.Print("-!!- SAHighwayCallouts - |SpawnSemiTruckAndTrailer| - Trailer Model Choosed: "+trailer.Model.Name.ToUpper()+"-!!-");
        }

        internal static void SpawnNormalPed(out Ped cPed, Vector3 Spawnpoint, float heading) //Spawns normal ped
        {
            Model[] pedModels = { "ig_abigail", "a_m_m_afriamer_01", "u_m_m_aldinapoli", "ig_amandatownley", "s_m_y_ammucity_01", "ig_andreas","u_m_y_antonb","csb_anita","g_m_m_armboss_01","g_m_m_armgoon_01","ig_ashley","s_m_m_autoshop_01","ig_money","g_m_y_azteca_01","g_m_y_ballaeast_01","g_m_y_ballaorig_01","ig_ballasog","g_m_y_ballasout_01","u_m_m_bankman","s_m_y_barman_01","s_f_y_bartender_01","u_m_y_baygor","a_m_y_beachvesp_01","ig_beverly","a_f_m_bevhills_01","a_f_y_bevhills_01","a_m_m_bevhills_01","a_m_y_bevhills_01","a_f_m_bevhills_02","u_m_m_bikehire_01","ig_brad","s_m_y_busboy_01","a_f_y_business_01","a_m_m_business_01","a_m_y_business_02","a_f_m_business_02","csb_car3guy2","g_m_m_chigoon_02","ig_claypain","ig_cletus","s_m_m_cntrybar_01","cs_dale","ig_denise","csb_denise_friend","ig_devin","a_m_y_dhill_01","cs_dom","a_f_m_downtown_01","a_f_m_eastsa_01","a_m_m_eastsa_01","a_m_y_eastsa_02","cs_fabien","g_m_y_famca_01","a_m_m_farmer_01","csb_fos_rep","a_m_y_gay_01","s_m_m_gaffer_01","csb_g","a_m_o_genstreet_01","a_m_y_genstreet_01","a_m_y_genstreet_02","a_f_y_golfer_01"};
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

        internal static void PedPersonaChooser(in Ped ped) //Chooses basic stuff for ped depending on settings in the ini
        {
            int wanted = new Random().Next(1, Settings.WantedPedChooserMaxInt);
            int drunk = new Random().Next(1, Settings.DrunkPedChooserMaxInt);
            int gun = new Random().Next(1, Settings.GunPedChooserMaxInt);

            if (wanted == 1) SetWanted(ped, true);
            if (drunk == 1) SetDrunk(ped, true);
            if (gun == 1) NormalWeaponChooser(ped, -1, true);
            Game.LogTrivial("-!!- SAHighwayCallouts - |PedPersonaChooser| - Ped is.. Wanted = "+wanted+", Drunk = "+drunk+", Gun = "+gun+"");
        }
    }
}