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
    internal class SAHC_Functions
    {
        internal static int choseHighwayStatePolice = new Random().Next(1, 3);
        
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

        internal static void LuxVehicleSpawn(out Vehicle vehicle, Vector3 spawnpoint, float heading)
        {
            Game.Console.Print("-!!- SAHighwayCallouts - |LuxVehicleSpawn| - Choosing Vehicle!");
            string[] vehicleModels = Settings.luxuryVehiclesArray;
            vehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], spawnpoint, heading);
            vehicle.IsPersistent = true;
            
            Game.Console.Print("-!!- SAHighwayCallouts - |LuxVehicleSpawn| - Vehicle Model Choosed: "+vehicle.Model.Name.ToUpper()+"-!!-");
        }

        internal static void SpawnNormalCar(out Vehicle vehicle, Vector3 spawnpoint, float heading) //Spawn normal random car..
        {
            Game.Console.Print("-!!- SAHighwayCallouts - |NormalVehicleSpawner| - Choosing Vehicle!");
            string[] vehicleModels = Settings.NormalVehiclesArray;
            vehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], spawnpoint, heading);
            vehicle.IsPersistent = true;
            
            Game.Console.Print("-!!- SAHighwayCallouts - |NormalVehicleSpawner| - Vehicle Model Choosed: "+vehicle.Model.Name.ToUpper()+"-!!-");
        }

        internal static void SpawnPoliceVehicle(out Vehicle vehicle, Vector3 spawnpoint, float heading, string cCounty)
        {
            Game.Console.Print("-!!- SAHighwayCallouts - |SpawnPoliceVehicle| - Choosing Vehicle Based On "+cCounty+"!");
            string[] vehicleModels = default;
            if (Settings.AlwaysChooseStateAIPolice) choseHighwayStatePolice = 1;
            if (choseHighwayStatePolice == 1)
            {
                Game.Console.Print("-!!- SAHighwayCallouts - |SpawnPoliceVehicle| - Choosing State Police Vehicle!");
                vehicleModels = Settings.HighwayStatePoliceVehiclesArray;
            }
            if (choseHighwayStatePolice != 1 && cCounty == "PaletoCounty") vehicleModels = Settings.PaletoBayVehiclesArray;
            if (choseHighwayStatePolice != 1 && cCounty == "BlaineCounty") vehicleModels = Settings.BlaineCountyVehiclesArray;
            if (choseHighwayStatePolice != 1 && cCounty == "LosSantosCounty") vehicleModels = Settings.LosSantosCountyVehiclesArray;
            if (choseHighwayStatePolice != 1 && cCounty == "LosSantosCityCounty") vehicleModels = Settings.LosSantosVehiclesArray;
            vehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], spawnpoint, heading);
            vehicle.IsPersistent = true;
            Game.Console.Print("-!!- SAHighwayCallouts - |SpawnPoliceVehicle| - Vehicle Model Choosed: "+vehicle.Model.Name.ToUpper()+"-!!-");
        }

        internal static void SpawnPolcePed(out Ped cPed, Vector3 spawnpoint, float heading, string cCounty)
        {
            Game.Console.Print("-!!- SAHighwayCallouts - |SpawnPolcePed| - Choosing Ped Based On "+cCounty+"!");
            string[] pedModels = default;
            if (choseHighwayStatePolice == 1)
            {
                Game.Console.Print("-!!- SAHighwayCallouts - |SpawnPolcePed| - Choosing State Police Ped!");
                pedModels = Settings.HighwayStatePolicePedsArray;
            }
            if (choseHighwayStatePolice != 1 && cCounty == "PaletoCounty") pedModels = Settings.PaletoBayPedsArray;
            if (choseHighwayStatePolice != 1 && cCounty == "BlaineCounty") pedModels = Settings.BlaineCountyPedsArray;
            if (choseHighwayStatePolice != 1 && cCounty == "LosSantosCounty") pedModels = Settings.LosSantosCountyPedsArray;
            if (choseHighwayStatePolice != 1 && cCounty == "LosSantosCityCounty") pedModels = Settings.LosSantosPedsArray;
            cPed = new Ped(pedModels[new Random().Next(pedModels.Length)], spawnpoint, heading);
            cPed.IsPersistent = true;
            cPed.BlockPermanentEvents = true;
            Game.Console.Print("-!!- SAHighwayCallouts - |SpawnPolcePed| - Ped Model Choosed: "+cPed.Model.Name.ToUpper()+"-!!-");
        }
        
        internal static void SpawnTaxiVehicle(out Vehicle vehicle, Vector3 spawnpoint, float heading)
        {
            Game.Console.Print("-!!- SAHighwayCallouts - |SpawnTaxiVehicle| - Choosing Vehicle Taxi Vehicle!");
            string[] vehicleModels = Settings.TaxiVehicleArray;
            vehicle = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], spawnpoint, heading);
            vehicle.IsPersistent = true;
            Game.Console.Print("-!!- SAHighwayCallouts - |SpawnTaxiVehicle| - Vehicle Model Choosed: "+vehicle.Model.Name.ToUpper()+"-!!-");
        }
        
        internal static void SpawnSemiTruckAndTrailer(out Vehicle truck, out Vehicle trailer, Vector3 spawnpoint, float heading) //Spawn normal random car..
        {
            Game.Console.Print("-!!- SAHighwayCallouts - |SpawnSemiTruckAndTrailer| - Choosing Vehicle!");
            string[] vehicleModels = Settings.SemiTruckVehiclesArray;
            truck = new Vehicle(vehicleModels[new Random().Next(vehicleModels.Length)], spawnpoint, heading);
            truck.IsPersistent = true;
            
            Game.Console.Print("-!!- SAHighwayCallouts - |SpawnSemiTruckAndTrailer| - Vehicle Model Choosed: "+truck.Model.Name.ToUpper()+"-!!-");
            
            Game.Console.Print("-!!- SAHighwayCallouts - |SpawnSemiTruckAndTrailer| - Choosing Trailer!");
            string[] vehicleModelsT = Settings.SemiTrailerModelsArray;
            trailer = new Vehicle(vehicleModelsT[new Random().Next(vehicleModelsT.Length)], spawnpoint, heading);
            trailer.IsPersistent = true;
            
            Game.Console.Print("-!!- SAHighwayCallouts - |SpawnSemiTruckAndTrailer| - Trailer Model Choosed: "+trailer.Model.Name.ToUpper()+"-!!-");
        }

        internal static void SpawnNormalPed(out Ped cPed, Vector3 Spawnpoint, float heading) //Spawns normal ped
        {
            Model[] pedModels = { "ig_abigail", "a_m_m_afriamer_01", "u_m_m_aldinapoli", "ig_amandatownley", "s_m_y_ammucity_01", "ig_andreas","u_m_y_antonb","csb_anita","g_m_m_armboss_01","g_m_m_armgoon_01","ig_ashley","s_m_m_autoshop_01","ig_money","g_m_y_azteca_01","g_m_y_ballaeast_01","g_m_y_ballaorig_01","ig_ballasog","g_m_y_ballasout_01","u_m_m_bankman","s_m_y_barman_01","s_f_y_bartender_01","u_m_y_baygor","a_m_y_beachvesp_01","ig_beverly","a_f_m_bevhills_01","a_f_y_bevhills_01","a_m_m_bevhills_01","a_m_y_bevhills_01","a_f_m_bevhills_02","u_m_m_bikehire_01","ig_brad","s_m_y_busboy_01","a_f_y_business_01","a_m_m_business_01","a_m_y_business_02","a_f_m_business_02","csb_car3guy2","g_m_m_chigoon_02","ig_claypain","ig_cletus","s_m_m_cntrybar_01","cs_dale","ig_denise","csb_denise_friend","ig_devin","a_m_y_dhill_01","cs_dom","a_f_m_downtown_01","a_f_m_eastsa_01","a_m_m_eastsa_01","a_m_y_eastsa_02","cs_fabien","g_m_y_famca_01","a_m_m_farmer_01","csb_fos_rep","a_m_y_gay_01","s_m_m_gaffer_01","csb_g","a_m_o_genstreet_01","a_m_y_genstreet_01","a_m_y_genstreet_02","a_f_y_golfer_01"};
            cPed = new Ped(pedModels[new Random().Next(pedModels.Length)], Spawnpoint, heading);
            cPed.IsPersistent = true;
            cPed.BlockPermanentEvents = true;
            
            Game.Console.Print("-!!- SAHighwayCallouts - |SpawnNornalPed| - Ped Model Choosed: "+cPed.Model.Name.ToUpper()+"-!!-");
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
            if (wanted == 1) SetDrunk(ped, true);
            if (wanted == 1) NormalWeaponChooser(ped, -1, true);
            Game.LogTrivial("-!!- SAHighwayCallouts - |PedPersonaChooser| - Finished!");
        }

        internal static void ColorPicker(out Color c, out string color)
        {
            color = default;
            c = default;
            int cp = new Random().Next(1, 51);
            switch (cp)
            {
                case 1:
                    c = Color.Aqua;
                    color = "Aqua";
                    break;
                case 2:
                    c = Color.Azure;
                    color = "Azure";
                    break;
                case 3:
                    c = Color.Beige;
                    color = "Beige";
                    break;
                case 4:
                    c = Color.Bisque;
                    color = "Bisque";
                    break;
                case 5:
                    c = Color.Black;
                    color = "Black";
                    break;
                case 6:
                    c = Color.Blue;
                    color = "Blue";
                    break;
                case 7:
                    c = Color.Brown;
                    color = "Brown";
                    break;
                case 8:
                    c = Color.Chartreuse;
                    color = "Light Green";
                    break;
                case 9:
                    c = Color.Chocolate;
                    color = "Light Brown";
                    break;
                case 10:
                    c = Color.Coral;
                    color = "Coral";
                    break;
                case 11:
                    c = Color.Crimson;
                    color = "Crimson";
                    break;
                case 12:
                    c = Color.Cyan;
                    color = "Cyan";
                    break;
                case 13:
                    c = Color.Fuchsia;
                    color = "Bright Purple";
                    break;
                case 14:
                    c = Color.Gold;
                    color = "Light Gold";
                    break;
                case 15:
                    c = Color.Goldenrod;
                    color = "Gold";
                    break;
                case 16:
                    c = Color.Gray;
                    color = "Gray";
                    break;
                case 17:
                    c = Color.Green;
                    color = "Green";
                    break;
                case 18:
                    c = Color.Indigo;
                    color = "Indigo";
                    break;
                case 19:
                    c = Color.Ivory;
                    color = "Ivory";
                    break;
                case 20:
                    c = Color.Lavender;
                    color = "Lavender";
                    break;
                case 21:
                    c = Color.Lime;
                    color = "Lime";
                    break;
                case 22:
                    c = Color.Magenta;
                    color = "Magenta";
                    break;
                case 23:
                    c = Color.Maroon;
                    color = "Maroon";
                    break;
                case 24:
                    c = Color.Moccasin;
                    color = "Cream White";
                    break;
                case 25:
                    c = Color.Navy;
                    color = "Navy";
                    break;
                case 26:
                    c = Color.Olive;
                    color = "Olive";
                    break;
                case 27:
                    c = Color.Orange;
                    color = "Orange";
                    break;
                case 28:
                    c = Color.Pink;
                    color = "Pink";
                    break;
                case 29:
                    c = Color.Purple;
                    color = "Purple";
                    break;
                case 30:
                    c = Color.Red;
                    color = "Red";
                    break;
                case 31:
                    c = Color.Salmon;
                    color = "Salmon";
                    break;
                case 32:
                    c = Color.Sienna;
                    color = "Sienna";
                    break;
                case 33:
                    c = Color.Silver;
                    color = "Silver";
                    break;
                case 34:
                    c = Color.Snow;
                    color = "White";
                    break;
                case 35:
                    c = Color.White;
                    color = "White";
                    break;
                case 36:
                    c = Color.Tan;
                    color = "Tan";
                    break;
                case 37:
                    c = Color.Teal;
                    color = "Teal";
                    break;
                case 38:
                    c = Color.Tomato;
                    color = "Tomato";
                    break;
                case 39:
                    c = Color.SlateGray;
                    color = "Slate Gray";
                    break;
                case 40:
                    c = Color.SlateBlue;
                    color = "Slate Blue";
                    break;
                case 41:
                    c = Color.CadetBlue;
                    color = "Light Blue";
                    break;
                case 42:
                    c = Color.DarkCyan;
                    color = "Dark Cyan";
                    break;
                case 43:
                    c = Color.FloralWhite;
                    color = "Floral White";
                    break;
                case 44:
                    c = Color.PeachPuff;
                    color = "Peach";
                    break;
                case 45:
                    c = Color.RosyBrown;
                    color = "Rose Brown";
                    break;
                case 46:
                    c = Color.PaleVioletRed;
                    color = "Violet Red";
                    break;
                case 47:
                    c = Color.DarkGray;
                    color = "Dark Gray";
                    break;
                case 48:
                    c = Color.SeaGreen;
                    color = "Sea Green";
                    break;
                case 49:
                    c = Color.HotPink;
                    color = "Hot Pink";
                    break;
                case 50:
                    c = Color.Turquoise;
                    color = "Turquoise";
                    break;
            }
            Game.LogTrivial("-!!- SAHighwayCallouts - |ColorPicker| - Picked Color "+c.ToString()+"!");
        }
    }
}