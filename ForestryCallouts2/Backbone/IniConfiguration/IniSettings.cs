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
        private static bool _disableDistacneChecker;
        private static int _unit;
        private static double _inputDistance;
        internal static double FinalDistance;
        
        internal static bool AllCalls;
        internal static bool WaterCalls;
        internal static int SearchAreaNotfis;
        
        //Callouts
        internal static bool IntoxPerson;

        //Keys
        internal static string DialogueKey;
        internal static Keys InputDialogueKey;
        internal static string EndCalloutKey;
        internal static Keys InputEndCalloutKey;
        internal static string InteractionKey;
        internal static Keys InputInteractionKey;

        #endregion
        
        //Loads settings in the users INI file. Used in main.cs
        internal static void LoadSettings()
        {
            string IniPath = "Plugins/LSPDFR/ForestryCallouts2/mainconfig.ini";
            InitializationFile ini = new InitializationFile(IniPath);
            ini.Create();
            
            //Current plugin version installed
            CurV = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            _disableDistacneChecker = ini.ReadBoolean("Main", "DisableDistanceChecker", false);
            if (_disableDistacneChecker) Game.Console.Print("Distance checker disabled");
            if (!_disableDistacneChecker)
            {
                _unit = ini.ReadInt32("DistanceChecker", "UnitOfMeasurement", 1);
                _inputDistance = ini.ReadInt32("DistanceChecker", "MaxDistance");
                if (_unit == 1) Game.Console.Print("Using meters for MaxDistance, value set to: "+_inputDistance+"");
                if (_unit == 2)
                {
                    Game.Console.Print("Using miles for MaxDistance, value set to: "+_inputDistance+"");
                    FinalDistance = _inputDistance * 1609.344;
                    Game.Console.Print("Conversion to meters: "+FinalDistance+"");
                }

                if (_unit != 1 || _unit != 2)
                {
                    Game.Console.Print("FORESTRY CALLOUTS ERROR: The MaxDistance Measurement unit was not set to 1 (meters) or 2 (miles)");
                    Game.Console.Print("Setting measurement to meters and max distance to 1500");
                    _iniError = true;
                }
            }
            DebugLogs = ini.ReadBoolean("Main", "DebugLogs", false);
            
            //With this disabled only park ranger calls will appear and be playable for example calls like silent alarms or trespassing near
            //the forest wont be playable to the player since this really isn't a call for park rangers.
            AllCalls = ini.ReadBoolean("Main", "AllCallouts", true);
            WaterCalls = ini.ReadBoolean("Main", "WaterCallouts", false);

            //Max number of search blips that cna be sent out for callouts that use them, min is 10
            SearchAreaNotfis = ini.ReadInt32("Main", "SearchAreaBlipsMax", 15);
            if (SearchAreaNotfis < 10) SearchAreaNotfis = 15;

            //Key stuff
            DialogueKey = ini.ReadString("Keys", "DialogueKey", "Y");
            EndCalloutKey = ini.ReadString("Keys", "EndCalloutKey", "End");
            InteractionKey = ini.ReadString("Keys", "InteractionKey", "R");

            //Lets us convert strings into keys
            KeysConverter kc = new KeysConverter();
            //this tries to Convert each of the above strings into keys
            try
            {
                InputDialogueKey = (Keys)kc.ConvertFromString(DialogueKey);
                InputEndCalloutKey = (Keys)kc.ConvertFromString(EndCalloutKey);
                InputInteractionKey = (Keys)kc.ConvertFromString(InteractionKey);
            }
            //This catch, catches Invalid Keys and informs the user of the mistake.
            catch (Exception e)
            {
                InputDialogueKey = Keys.Y;
                InputEndCalloutKey = Keys.End;
                InputInteractionKey = Keys.T;
                Game.Console.Print("FORESTRY CALLOUTS ERROR - A invalid key has been detected and all keys have been set to default options");
                Game.Console.Print("FORESTRY CALLOUTS ERROR - "+e+"");
                _iniError = true;
            }
            
            //Callouts
            IntoxPerson = ini.ReadBoolean("Callouts", "IntoxicatedPerson", true);

            //Sees if any errors took place and if so a notification is sent to check the log
            if (_iniError)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~g~FORESTRY CALLOUTS WARNING",
                    "~r~CONFIGURATION ERROR",
                    "Please check the log for forestry callouts errors for more info!");
            }
        }
    }
}