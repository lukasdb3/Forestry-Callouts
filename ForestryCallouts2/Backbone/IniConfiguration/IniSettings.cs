#region Refrences
//System
using System;
using System.Windows.Forms;
using ForestryCallouts2.Backbone.Functions;
using ForestryCallouts2.Callouts.LandCallouts;
//Rage
using Rage;
#endregion

namespace ForestryCallouts2.Backbone.IniConfiguration
{
    internal static class IniSettings
    {
        #region variables

        //Main
        internal static InitializationFile Ini;
        private static bool _iniError;
        internal static string CurV;
        
        internal static string Callsign;
        internal static bool DebugLogs;
        internal static bool EndNotfiMessages;
        internal static bool WaterCallouts;
        internal static bool EnableDistanceChecker;
        internal static double MaxDistance;
        internal static int MinCalloutDistance;
        internal static bool AICops;
        
        internal static int SearchAreaNotifications;

        //Keys
        internal static Keys DialogueKey;
        internal static Keys EndCalloutKey;
        internal static Keys InteractionMenuKey;
        internal static Keys DialogueKeyModifier;
        internal static Keys EndCalloutKeyModifier;
        internal static Keys InteractionMenuKeyModifier;
        internal static Keys BinocularsKey;
        internal static Keys BinocularsKeyModifier;
        internal static Keys BinocularsZoom;
        internal static Keys BinocularsZoomModifier;
        internal static Keys StopPedKey;
        internal static Keys StopPedKeyModifier;
        internal static Keys GrabPedKey;
        internal static Keys GrabPedKeyModifier;

        //Binoculars
        internal static bool BinocularsEnabled;
        internal static int BinocularsSensitivity;
        internal static string BinocularsImage;

        //Callouts
        internal static bool AnimalAttack;
        internal static bool AnimalOnRoadway;
        internal static bool AtvPursuit;
        internal static bool DangerousPerson;
        internal static bool DeadAnimalOnRoadway;
        internal static bool DirtBikePursuit;
        internal static bool HighSpeedPursuit;
        internal static bool IntoxPerson;
        internal static bool LoggerTruckPursuit;
        internal static bool RegularPursuit;
        
        internal static bool DeadBodyWater;
        internal static bool BoatPursuit;
        
        //LicensesPercents
        internal static int ResidentLicense;
        internal static int NonResidentLicense;
        internal static int OneDayLicense;
        internal static int TwoDayLicense;
        internal static int NoLicense;
        //LicenseStatus
        internal static int Expired;
        internal static int Valid;

        //Vehicles
        private static string _normalVehicles;
        internal static String[] NormalVehicles;
        private static string _offRoadVehicles;
        internal static String[] OffRoadVehicles;
        private static string _offRoadFastVehicles;
        internal static String[] OffRoadFastVehicles;
        private static string _animalControlVehicles;
        internal static String[] AnimalControlVehicles;
        private static string _dirtbikes;
        internal static String[] Dirtbikes;
        private static string _atvVehicles;
        internal static String[] AtvVehicles;
        private static string _semiTrucks;
        internal static String[] SemiTrucks;
        internal static string _boats;
        internal static String[] Boats;
        internal static string _rangerVehicles;
        internal static String[] RangerVehicles;

        #endregion
        
        //Loads settings in the users INI file.
        internal static void LoadSettings()
        {
            //If ini error is true we want to set it to false so it doesnt throw a false error
            _iniError = false;
            //Create Ini
            string IniPath = "Plugins/LSPDFR/ForestryCallouts2.ini";
            Ini = new InitializationFile(IniPath);
            Ini.Create();
            
            //Current plugin version installed
            CurV = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            
            //Main
            Callsign = Ini.ReadString("Main", "Callsign", "1-LINCOLN-18");
            DebugLogs = Ini.ReadBoolean("Main", "DebugLogs", false);
            EndNotfiMessages = Ini.ReadBoolean("Main", "CalloutEndMessages", true);
            WaterCallouts = Ini.ReadBoolean("Main", "WaterCallouts", false);
            EnableDistanceChecker = Ini.ReadBoolean("Main", "EnableDistanceChecker", true);
            MaxDistance = Ini.ReadDouble("Main", "MaxDistance", 2500);
            MinCalloutDistance = Ini.ReadInt32("Main", "MinimumCalloutSpawnDistance", 100);
            AICops = Ini.ReadBoolean("Main", "AICops", false);
            //Max number of search blips that can be sent out for callouts that use them, min is 10
            SearchAreaNotifications = Ini.ReadInt32("Main", "SearchAreaBlipsMax", 15);
            if (SearchAreaNotifications < 5) 
            {
                SearchAreaNotifications = 15;
                Game.Console.Print("!!! INVALID CONFIG !!! - Forestry Callouts Config Error");
                Game.Console.Print("Detected SearchAreaBlipsMax was set to a lower integer than the minimum 5. Default has been set (15)");
            }
            //Keys
            try
            {
                DialogueKey = Ini.ReadEnum("Keys", "DialogueKey", Keys.Y);
                DialogueKeyModifier = Ini.ReadEnum("Keys", "DialogueKeyModifier", Keys.None);
                EndCalloutKey = Ini.ReadEnum("Keys", "EndCalloutKey", Keys.End);
                EndCalloutKeyModifier = Ini.ReadEnum("Keys", "EndCalloutKeyModifier", Keys.None); 
                InteractionMenuKey = Ini.ReadEnum("Keys", "InteractionMenuKey", Keys.I);
                InteractionMenuKeyModifier = Ini.ReadEnum("Keys", "InteractionMenuKeyModifier", Keys.None);
                BinocularsKey = Ini.ReadEnum("Keys", "BinocularsKey", Keys.O);
                BinocularsKeyModifier = Ini.ReadEnum("Keys", "BinocularsKeyModifier", Keys.None);
                BinocularsZoom = Ini.ReadEnum("Keys", "BinocularsZoom", Keys.MButton);
                BinocularsZoomModifier = Ini.ReadEnum("Keys", "BinocularsZoomModifier", Keys.None);
                StopPedKey = Ini.ReadEnum("Keys", "StopPedKey", Keys.E);
                StopPedKeyModifier = Ini.ReadEnum("Keys", "StopPedKeyModifier", Keys.None);
                GrabPedKey = Ini.ReadEnum("Keys", "GrabPedKey", Keys.U);
                GrabPedKeyModifier = Ini.ReadEnum("Keys", "GrabPedKeyModifier", Keys.Control);
            }
            catch (Exception e)
            {
                _iniError = true;
                Game.Console.Print("!!! ERROR !!! - Forestry Callouts Keybinding Error");
                Game.Console.Print("One or more of your keybindings are not valid keys");
                Game.Console.Print(e.ToString());
            }
            
            //Binoculars
            BinocularsEnabled = Ini.ReadBoolean("Binoculars", "EnableBinoculars", true);
            BinocularsSensitivity = Ini.ReadInt32("Binoculars", "BinocularsSensitivity", 3);
            BinocularsImage = Ini.ReadString("Binoculars", "BinocularsImage", "1");

            //Callouts
            AnimalAttack = Ini.ReadBoolean("Callouts", "AnimalAttack", true);
            AnimalOnRoadway = Ini.ReadBoolean("Callouts", "AnimalOnRoadway", true);
            AtvPursuit = Ini.ReadBoolean("Callouts", "AtvPursuit", true);
            DangerousPerson = Ini.ReadBoolean("Callouts", "DangerousPerson", true);
            DirtBikePursuit = Ini.ReadBoolean("Callouts", "DirtBikePursuit", true);
            HighSpeedPursuit = Ini.ReadBoolean("Callouts", "HighSpeedPursuit", true);
            IntoxPerson = Ini.ReadBoolean("Callouts", "IntoxicatedPerson", true);
            DeadAnimalOnRoadway = Ini.ReadBoolean("Callouts", "DeadAnimalOnRoadway", true);
            LoggerTruckPursuit = Ini.ReadBoolean("Callouts", "LoggerTruckPursuit", true);
            RegularPursuit = Ini.ReadBoolean("Callouts", "Pursuit", true);

            DeadBodyWater = Ini.ReadBoolean("Callouts", "DeadBodyWater", true);
            BoatPursuit = Ini.ReadBoolean("Callouts", "BoatPursuit", true);
            
            //LicensesPercents
            ResidentLicense = Ini.ReadInt32("Licenses", "Residential", 30);
            NonResidentLicense = Ini.ReadInt32("Licenses", "NonResidential", 30);
            OneDayLicense = Ini.ReadInt32("Licenses", "OneDay", 10);
            TwoDayLicense = Ini.ReadInt32("Licenses", "TwoDay", 10);
            NoLicense = Ini.ReadInt32("Licenses", "NoLicense", 20);
            //LicenseStatuses
            Expired = Ini.ReadInt32("Licenses", "Expired", 30);
            Valid = Ini.ReadInt32("Licenses", "Valid", 70);

            //Vehicles
            _normalVehicles = Ini.ReadString("Vehicles", "NormalVehicles", null);
            NormalVehicles = _normalVehicles.Split(':');
            _offRoadVehicles = Ini.ReadString("Vehicles", "OffRoadVehicles", null);
            OffRoadVehicles = _offRoadVehicles.Split(':');
            _offRoadFastVehicles = Ini.ReadString("Vehicles", "OffRoadFastVehicles", null);
            OffRoadFastVehicles = _offRoadFastVehicles.Split(':');
            _animalControlVehicles = Ini.ReadString("Vehicles", "AnimalControlVehicles", null);
            AnimalControlVehicles = _animalControlVehicles.Split(':');
            _dirtbikes = Ini.ReadString("Vehicles", "Dirtbikes", null);
            Dirtbikes = _dirtbikes.Split(':');
            _atvVehicles = Ini.ReadString("Vehicles", "AtvVehicles", null);
            AtvVehicles = _atvVehicles.Split(':');
            _semiTrucks = Ini.ReadString("Vehicles", "SemiTrucks", null);
            SemiTrucks = _semiTrucks.Split(':');
            _boats = Ini.ReadString("Vehicles", "Boats", null);
            Boats = _boats.Split(':');
            _rangerVehicles = Ini.ReadString("Vehicles", "RangerVehicles", null);
            RangerVehicles = _rangerVehicles.Split(':');

            //Sees if any errors took place and if so a notification is sent to check the log
            if (_iniError)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~g~FORESTRY CALLOUTS WARNING",
                    "~r~CONFIGURATION ERROR",
                    "Please check the rage log for more information!");
            }
        }

        internal static void SaveNewSettings()
        {
            //Main
            Ini.Write("Main", "DebugLogs", Menu.MainMenu.DebugLogs.SelectedValue);
            Ini.Write("Main", "CalloutEndMessages", Menu.MainMenu.EndNotfiMessages.SelectedValue);
            Ini.Write("Main", "WaterCallouts", Menu.MainMenu.WaterCallouts.SelectedValue);
            Ini.Write("Main", "SearchAreaBlipsMax", Menu.MainMenu.SearchAreaBlipsMax.Value);
            Ini.Write("Main", "MaxDistance", Menu.MainMenu.MaxDistance.Value);
            Ini.Write("Main", "EnableDistanceChecker", Menu.MainMenu.EnableDistanceChecker.SelectedValue);
            Ini.Write("Main", "MinimumCalloutSpawnDistance", Menu.MainMenu.MinCalloutDistance.Value);
            Ini.Write("Main", "AICops", Menu.MainMenu.AICops.SelectedValue);
            //Binoculars
            Ini.Write("Binoculars", "EnableBinoculars", Menu.MainMenu.EnableBinoculars.SelectedValue);
            Ini.Write("Binoculars", "BinocularsSensitivity", Menu.MainMenu.BinocularsSense.Value);
            Ini.Write("Binoculars", "BinocularsImage", Menu.MainMenu.BinocularsImage.Value.ToString());
            //Callouts
            Ini.Write("Callouts", "AnimalAttack", Menu.MainMenu.AnimalAttack.SelectedValue);
            Ini.Write("Callouts", "AtvPursuit", Menu.MainMenu.AtvPursuit.SelectedValue);
            Ini.Write("Callouts", "DangerousPerson", Menu.MainMenu.DangerousPerson.SelectedValue);
            Ini.Write("Callouts", "DeadAnimalOnRoadway", Menu.MainMenu.DeadAnimalOnRoadway.SelectedValue);
            Ini.Write("Callouts", "AnimalOnRoadway", Menu.MainMenu.AnimalOnRoadway.SelectedValue);
            Ini.Write("Callouts", "DirtBikePursuit", Menu.MainMenu.DirtBikePursuit.SelectedValue);
            Ini.Write("Callouts", "HighSpeedPursuit", Menu.MainMenu.HighSpeedPursuit.SelectedValue);
            Ini.Write("Callouts", "IntoxicatedPerson", Menu.MainMenu.IntoxicatedPerson.SelectedValue);
            Ini.Write("Callouts", "RegularPursuit", Menu.MainMenu.RegularPursuit.SelectedValue);
            
            Ini.Write("Callouts", "DeadBodyWater", Menu.MainMenu.DeadBodyWater.SelectedValue);
            Ini.Write("Callouts", "BoatPursuit", Menu.MainMenu.BoatPursuit.SelectedValue);
            Game.DisplayNotification("~g~Settings Saved To ForestryCallouts2.ini");
        }
    }
}