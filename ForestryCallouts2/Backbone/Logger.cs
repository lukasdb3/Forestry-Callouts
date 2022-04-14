using System;
using Rage;

namespace ForestryCallouts2.Backbone
{
    internal class Logger
    {
        internal static void Log(string m)
        {
            Game.LogTrivial("-!!- ForestryCallouts2 - "+m+"");
        }

        internal static void StartLoadingPhase()
        {
            Game.Console.Print();
            Game.Console.Print("=============== FORESTRY CALLOUTS ===============");
            Game.Console.Print("Trying to load settings..");
            IniSettings.LoadSettings();
            Game.Console.Print("Settings loaded");
            Game.Console.Print("Checking Forestry Callouts version..");
            Game.Console.Print("Version: "+VersionChecker.receivedData+"");
            Game.Console.Print("Registering Callouts..");
            Game.Console.Print("=============== FORESTRY CALLOUTS ===============");
            Game.Console.Print();
            Main.RegisterCallouts();
        }
    }
}