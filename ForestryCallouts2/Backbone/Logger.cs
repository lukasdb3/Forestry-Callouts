using System;
using ForestryCallouts.SimpleFunctions;
using ForestryCallouts2.Backbone.SpawnSystem;
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
            Game.Console.Print("Forestry Callouts update available:"+VersionChecker.IsUpdateAvailable()+"");
            Game.Console.Print("Version: "+VersionChecker.receivedData+"");
            Game.Console.Print("Running Plugin Checks...");
            Game.Console.Print("Checking players plugins for callouts..");
            CalloutsGetter.CacheCallouts();
            Game.Console.Print("CalloutInterface installed: "+ CiPluginChecker.IsCalloutInterfaceRunning + "");
            Game.Console.Print("UltimateBackup installed: "+ UbPluginChecker.IsUltimateBackupRunning + "");
            Game.Console.Print("StopThePed installed: "+ StpPluginChecker.IsStopThePedRunning + "");
            Game.Console.Print("All callouts enabled: "+IniSettings.AllCalls+"");
            Game.Console.Print("Water callouts enabled: "+IniSettings.WaterCalls+"");
            Game.Console.Print("Loading needed chunks...");
            if (!IniSettings.WaterCalls) SpawnSystem.Land.ChunkLoader.Main();
            if (IniSettings.WaterCalls) SpawnSystem.Water.ChunkLoader.Main();
            Game.Console.Print("Registering Callouts..");
            Game.Console.Print("=============== FORESTRY CALLOUTS ===============");
            Game.Console.Print();
            Main.RegisterCallouts();
        }
    }
}