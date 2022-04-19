using System;
using Rage;
using ForestryCallouts2.Backbone;
using System.Windows.Forms;

namespace ForestryCallouts2.Backbone
{
    internal class IniSettings
    {
        #region variables

        //Main
        internal static string CurV;
        internal static bool AllCalls;
        internal static bool WaterCalls;
        internal static bool DistanceChecker;
        internal static int MaxDistance;
        
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
            Logger.Log("Trying to load settings...");
            
            string IniPath = "Plugins/LSPDFR/ForestryCallouts2.ini";
            InitializationFile ini = new InitializationFile(IniPath);
            ini.Create();
            
            //Current plugin version installed
            CurV = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //With this disabled only park ranger calls will appear and be playable for example calls like silent alarms or trespassing near
            //the forest wont be playable to the player since this really isn't a call for park rangers.
            AllCalls = ini.ReadBoolean("Main", "AllCallouts", true);
            WaterCalls = ini.ReadBoolean("Main", "WaterCallouts", false);
            DistanceChecker = ini.ReadBoolean("Main", "DistanceChecker", true);
            MaxDistance = ini.ReadInt32("Main", "MaxDistance");
            
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
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~g~Forestry Callouts Warning",
                    "~r~Invalid Key Error",
                    "Please check your ~y~ForestryCallouts2.ini~w~ for ~y~Invalid Key Input~w~, see log for more details.");
                Game.Console.Print("FORESTRY CALLOUTS ERROR - "+e+"");
            }
            
            //Callouts
            IntoxPerson = ini.ReadBoolean("Callouts", "IntoxicatedPerson");
        }
    }
}