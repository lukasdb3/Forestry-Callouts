using System;
using System.Windows.Forms;
using Rage;

namespace ForestryCallouts2.Backbone.IniConfiguration
{
    internal class IniSettings
    {
        #region variables

        //Main
        private static bool _iniError;
        internal static string CurV;
        internal static bool DebugLogs;
        
        //Distance Checker Variables
        private static int _unit;
        private static double _inputDistance;
        internal static double FinalDistance;
        
        internal static bool WaterCalls;
        internal static int SearchAreaNotifications;
        
        //Callouts
        internal static bool IntoxPerson;

        //Keys
        internal static Keys DialogueKey;
        internal static Keys EndCalloutKey;

        #endregion
        
        //Loads settings in the users INI file. Used in main.cs
        internal static void LoadSettings()
        {
            string IniPath = "Plugins/LSPDFR/ForestryCallouts2/mainconfig.ini";
            InitializationFile ini = new InitializationFile(IniPath);
            ini.Create();
            
            //Current plugin version installed
            CurV = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            
            //If miles is being used for distanceChecker, converts to meters.
            _unit = ini.ReadInt32("DistanceChecker", "UnitOfMeasurement", 1);
            _inputDistance = ini.ReadDouble("DistanceChecker", "MaxDistance", 1);
            if (_unit == 1) Game.Console.Print("Using meters for MaxDistance, value set to: "+_inputDistance+"");
            if (_unit == 2)
            {
                Game.Console.Print("Using miles for MaxDistance, value set to: "+_inputDistance+"");
                FinalDistance = _inputDistance * 1609.344;
                Game.Console.Print("Conversion to meters: "+FinalDistance+"");
            }

            if (_unit != 1 && _unit != 2)
            {
                Game.Console.Print("FORESTRY CALLOUTS ERROR: The MaxDistance Measurement unit was not set to 1 (meters) or 2 (miles)");
                Game.Console.Print("Setting measurement to meters and max distance to 1500");
                _iniError = true;
            }
            
            DebugLogs = ini.ReadBoolean("Main", "DebugLogs", false);
            
            WaterCalls = ini.ReadBoolean("Main", "WaterCallouts", false);

            //Max number of search blips that can be sent out for callouts that use them, min is 10
            SearchAreaNotifications = ini.ReadInt32("Main", "SearchAreaBlipsMax", 15);
            if (SearchAreaNotifications < 5) SearchAreaNotifications = 15;

            //Key stuff
            try
            {
                DialogueKey = ini.ReadEnum("Keys", "DialogueKey", Keys.Y);
                EndCalloutKey = ini.ReadEnum("Keys", "EndCalloutKey", Keys.End);
            }
            catch (Exception e)
            {
                Game.Console.Print("!!! ERROR !!! - Forestry Callouts Keybinding Error");
                Game.Console.Print("One or more of your keybindings are not valid keys, all keys have been set to default");
                Game.Console.Print(e.ToString());
                DialogueKey = Keys.Y;
                EndCalloutKey = Keys.End;
            }

            //Callouts
            IntoxPerson = ini.ReadBoolean("Callouts", "IntoxicatedPerson", true);

            //Sees if any errors took place and if so a notification is sent to check the log
            if (_iniError)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~g~FORESTRY CALLOUTS WARNING",
                    "~r~CONFIGURATION ERROR",
                    "Please check the rage log for more information!");
            }
        }
    }
}