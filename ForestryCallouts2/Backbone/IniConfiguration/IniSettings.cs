﻿#region Refrences
//System
using System;
using System.Windows.Forms;
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

        internal static bool DebugLogs;
        internal static bool EnableDistanceChecker;
        internal static double MaxDistance;
        internal static int MinCalloutDistance;
        
        internal static int SearchAreaNotifications;

        //Keys
        internal static Keys DialogueKey;
        internal static Keys EndCalloutKey;
        internal static Keys InteractionMenuKey;

        //Binoculars
        internal static bool BinocularsEnabled;
        internal static Keys BinocularsKey;
        internal static int BinocularsSensitivity;
        
        //AmbientEvents
        internal static bool AmbientEventsEnabled;
        internal static int MinimumWaitTime;
        internal static int MaximumWaitTime;
        
        //Callouts
        internal static bool IntoxPerson;
        internal static bool RegularPursuit;
        internal static bool AnimalAttack;
        internal static bool DeadAnimalOnRoadway;
        internal static bool DangerousPerson;
        internal static bool DirtBikePursuit;
        internal static bool AtvPursuit;
        internal static bool HighSpeedPursuit;
        internal static bool LoggerTruckPursuit;

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
            DebugLogs = Ini.ReadBoolean("Main", "DebugLogs", false);
            EnableDistanceChecker = Ini.ReadBoolean("Main", "EnableDistanceChecker", true);
            MaxDistance = Ini.ReadDouble("Main", "MaxDistance", 2500);
            MinCalloutDistance = Ini.ReadInt32("Main", "MinimumCalloutSpawnDistance", 100);
            //Max number of search blips that can be sent out for callouts that use them, min is 10
            SearchAreaNotifications = Ini.ReadInt32("Main", "SearchAreaBlipsMax", 15);
            if (SearchAreaNotifications < 5) 
            {
                SearchAreaNotifications = 15;
                Game.Console.Print("!!! INVALID CONFIG !!! - Forestry Callouts Config Error");
                Game.Console.Print("We detected SearchAreaBlipsMax was set to a lower integer than the minimum 5. Default has been set (15)");
            }
            //Keys
            try
            {
                DialogueKey = Ini.ReadEnum("Keys", "DialogueKey", Keys.Y);
                EndCalloutKey = Ini.ReadEnum("Keys", "EndCalloutKey", Keys.End);
                InteractionMenuKey = Ini.ReadEnum("Keys", "InteractionMenuKey", Keys.I);
                BinocularsKey = Ini.ReadEnum("Keys", "BinocularsKey", Keys.O);
            }
            catch (Exception e)
            {
                _iniError = true;
                Game.Console.Print("!!! ERROR !!! - Forestry Callouts Keybinding Error");
                Game.Console.Print("One or more of your keybindings are not valid keys, all keys have been set to default");
                Game.Console.Print(e.ToString());
                DialogueKey = Keys.Y;
                EndCalloutKey = Keys.End;
            }
            
            //Binoculars
            BinocularsEnabled = Ini.ReadBoolean("Binoculars", "EnableBinoculars", true);
            BinocularsSensitivity = Ini.ReadInt32("Binoculars", "BinocularsSensitivity", 3);
            
            //AmbientEvents
            AmbientEventsEnabled = Ini.ReadBoolean("AmbientEvents", "AmbientEventsEnabled", true);
            MinimumWaitTime = Ini.ReadInt32("AmbientEvents", "MinimumWaitTimeBetweenEvents", 5);
            MaximumWaitTime = Ini.ReadInt32("AmbientEvents", "MaximumWaitTimeBetweenEvents", 10);

            //Callouts
            IntoxPerson = Ini.ReadBoolean("Callouts", "IntoxicatedPerson", true);
            RegularPursuit = Ini.ReadBoolean("Callouts", "Pursuit", true);
            AnimalAttack = Ini.ReadBoolean("Callouts", "AnimalAttack", true);
            DeadAnimalOnRoadway = Ini.ReadBoolean("Callouts", "DeadAnimalOnRoadway", true);
            DangerousPerson = Ini.ReadBoolean("Callouts", "DangerousPerson", true); 
            DirtBikePursuit = Ini.ReadBoolean("Callouts", "DirtBikePursuit", true);
            AtvPursuit = Ini.ReadBoolean("Callouts", "AtvPursuit", true);
            HighSpeedPursuit = Ini.ReadBoolean("Callouts", "HighSpeedPursuit", true);
            LoggerTruckPursuit = Ini.ReadBoolean("Callouts", "LoggerTruckPursuit", true);


            //Vehicles
            _normalVehicles = Ini.ReadString("Vehicles", "NormalVehicles", null);
            NormalVehicles = _normalVehicles.Split(':');
            _offRoadVehicles = Ini.ReadString("Vehicles", "OffRoadVehicles", null);
            OffRoadVehicles = _offRoadVehicles.Split(':');
            _offRoadFastVehicles = Ini.ReadString("Vehicles", "OffRoadFastVehicles", null);
            Game.Console.Print(_offRoadFastVehicles);
            OffRoadFastVehicles = _offRoadFastVehicles.Split(':');
            _animalControlVehicles = Ini.ReadString("Vehicles", "AnimalControlVehicles", null);
            AnimalControlVehicles = _animalControlVehicles.Split(':');
            _dirtbikes = Ini.ReadString("Vehicles", "Dirtbikes", null);
            Dirtbikes = _dirtbikes.Split(':');
            _atvVehicles = Ini.ReadString("Vehicles", "AtvVehicles", null);
            AtvVehicles = _atvVehicles.Split(':');
            _semiTrucks = Ini.ReadString("Vehicles", "SemiTrucks", null);
            SemiTrucks = _semiTrucks.Split(':');

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
            Ini.Write("Main", "DebugLogs", Menu.Create.DebugLogs.SelectedValue);
            Ini.Write("Main", "SearchAreaBlipsMax", Menu.Create.SearchAreaBlipsMax.Value);
            Ini.Write("Main", "MaxDistance", Menu.Create.MaxDistance.Value);
            Ini.Write("Main", "EnableDistanceChecker", Menu.Create.EnableDistanceChecker.SelectedValue);
            Ini.Write("Main", "MinimumCalloutSpawnDistance", Menu.Create.MinCalloutDistance.Value);
            //Binoculars
            Ini.Write("Binoculars", "EnableBinoculars", Menu.Create.EnableBinoculars.SelectedValue);
            Ini.Write("Binoculars", "BinocularsSensitivity", Menu.Create.BinocularsSense.Value);
            //AmbientEvents
            Ini.Write("AmbientEvents", "AmbientEventsEnabled", Menu.Create.AmbientEventsEnabled.SelectedValue);
            Ini.Write("AmbientEvents", "MinimumWaitTimeBetweenEvents", Menu.Create.MinimumWaitTimeBetweenEvents.Value);
            Ini.Write("AmbientEvents", "MaximumWaitTimeBetweenEvents", Menu.Create.MaximumWaitTimeBetweenEvents.Value);
            //Callouts
            Ini.Write("Callouts", "IntoxicatedPerson", Menu.Create.IntoxicatedPerson.SelectedValue);
            Ini.Write("Callouts", "RegularPursuit", Menu.Create.RegularPursuit.SelectedValue);
            Ini.Write("Callouts", "AnimalAttack", Menu.Create.AnimalAttack.SelectedValue);
            Ini.Write("Callouts", "DirtBikePursuit", Menu.Create.DirtBikePursuit.SelectedValue);
            Ini.Write("Callouts", "AtvPursuit", Menu.Create.AtvPursuit.SelectedValue);
            Game.DisplayNotification("~g~Settings Saved To ForestryCallouts2.ini\n" +
                                     "~r~If you changed any Callouts to True or False, LSPDFR will have to be reloaded for changes to be made!");
        }
    }
}