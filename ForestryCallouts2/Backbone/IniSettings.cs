using System;
using Rage;
using ForestryCallouts2.Backbone;

namespace ForestryCallouts2.Backbone
{
    internal class IniSettings
    {
        #region variables

        internal static string curV;
        internal static bool allCalls;
        internal static bool intoxPerson;


        #endregion
        
        //Loads settings in the users INI file. Used in main.cs
        internal static void LoadSettings()
        {
            Logger.Log("Trying to load settings...");
            
            string IniPath = "Plugins/LSPDFR/ForestryCallouts2.ini";
            InitializationFile ini = new InitializationFile(IniPath);
            ini.Create();
            
            //Current plugin version installed
            curV = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //With this disabled only park ranger calls will appear and be playable for example calls like silent alarms or trespassing near
            //the forest wont be playable to the player since this really isn't a call for park rangers.
            allCalls = ini.ReadBoolean("Main", "AllCallouts", true);
            
            
            //Callouts
            intoxPerson = ini.ReadBoolean("Callouts", "IntoxicatedPerson");
        }
    }
}