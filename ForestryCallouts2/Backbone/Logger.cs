using System;
using ForestryCallouts2.Backbone.IniConfiguration;
using ForestryCallouts2.Backbone.SpawnSystem;
using LSPD_First_Response.Mod.Callouts;
using Rage;

namespace ForestryCallouts2.Backbone
{
    internal class Logger
    {
        internal static void Log(string m)
        {
            Game.LogTrivial("-!!- ForestryCallouts - "+m+"");
        }

        internal static void InfoLog(string clas, string m)
        {
            Game.LogTrivial("-!!- ForestryCallouts - [INFO] [" + clas.ToUpper() + "] - " + m +"");
        }

        internal static void CallDebugLog(Callout call, string m)
        {
            if (IniSettings.DebugLogs) Game.LogTrivial("-!!- ForestryCallouts - ["+call+"] - "+m+"");
        }

        internal static void DebugLog(string clas, string m)
        {
            if (IniSettings.DebugLogs) Game.LogTrivial("-!!- ForestryCallouts - [DEBUG] [" + clas.ToUpper() + "] - " + m +"");
        }

        internal static void StartLoadingPhase()
        {
            Game.Console.Print();
            Game.Console.Print("=============== FORESTRY CALLOUTS ===============");
            Game.Console.Print("Loading settings..");
            IniSettings.LoadSettings();
            IniVehicles.LoadVehicleListConfigs();
            Game.Console.Print("Checking Forestry Callouts version..");
            Game.Console.Print("Forestry Callouts update available: "+VersionChecker.IsUpdateAvailable()+"");
            Game.Console.Print("Version: "+VersionChecker.receivedData+"");
            Game.Console.Print("Running Plugin Checks...");
            Game.Console.Print("Caching players callouts..");
            CalloutsGetter.CacheCallouts();
            if (PluginChecker.ForestryCallouts)
            {
                Game.Console.Print();
                Game.Console.Print("===== WARNING =====");
                Game.Console.Print("First version of ForestryCallouts was detected still in the plugins folder.");
                Game.Console.Print("Please remove ForestryCallouts and reload LSPDFR");
                Game.Console.Print("===== WARNING =====");
                Game.Console.Print();
            }
            Game.Console.Print("CalloutInterface installed: "+ PluginChecker.CalloutInterface + "");
            Game.Console.Print("UltimateBackup installed: "+ PluginChecker.UltimateBackup + "");
            Game.Console.Print("StopThePed installed: "+ PluginChecker.StopThePed + "");
            Game.Console.Print("Water callouts enabled: "+IniSettings.WaterCalls+"");
            Game.Console.Print("Loading needed chunks...");
            if (!IniSettings.WaterCalls) SpawnSystem.Land.ChunkLoader.Main();
            if (IniSettings.WaterCalls) SpawnSystem.Water.ChunkLoader.Main();
            Game.Console.Print("Loading commands!");
            Game.AddConsoleCommands(new[]{typeof(CCommands)});
            Game.Console.Print("Registering Callouts..");
            Game.Console.Print("=============== FORESTRY CALLOUTS ===============");
            Game.Console.Print();
        }

    }
}