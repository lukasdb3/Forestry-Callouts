#region Refrences
//Rage
using System;
using Rage;
//LSPDFR
using LSPD_First_Response.Mod.Callouts;
//ForestryCallouts2
using ForestryCallouts2.Backbone.IniConfiguration;
using ForestryCallouts2.Backbone.Menu;
using ForestryCallouts2.AmbientEvents;
#endregion

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
            if (IniSettings.DebugLogs) Game.LogTrivial("-!!- ForestryCallouts - [DEBUG] ["+call+"] - "+m+"");
        }

        internal static void DebugLog(string clas, string m)
        {
            if (IniSettings.DebugLogs) Game.LogTrivial("-!!- ForestryCallouts - [DEBUG] [" + clas.ToUpper() + "] - " + m +"");
        }

        internal static void ErrorLog(string clas, string m, Exception e)
        {
            Game.LogTrivial("-!!- ForestryCallouts - [ERROR] ["+clas.ToUpper()+"] - " + m + "\n" +
                            "EXCEPTION: "+e);
        }

        internal static void StartLoadingPhase()
        {
            Game.Console.Print();
            Game.Console.Print("=============== FORESTRY CALLOUTS ===============");
            Game.Console.Print("Loading settings..");
            IniSettings.LoadSettings();
            Game.Console.Print("Checking Forestry Callouts version..");
            Game.Console.Print("Forestry Callouts update available: "+VersionChecker.IsUpdateAvailable()+"");
            Game.Console.Print("Version: "+VersionChecker.ReceivedData+"");
            Game.Console.Print("Running Plugin Checks...");
            Game.Console.Print("Caching players callouts..");
            CalloutsGetter.CacheCallouts();
            Game.Console.Print("Initializing Interaction Menu..");
            Create.Initialize();
            Game.Console.Print("Starting Main Loop..");
            Main.RunLoop();
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
            if (IniSettings.AmbientEventsEnabled) Game.Console.Print("Registering AmbientEvents");
            AmbientEvents.Main.RegisterEvents();
            Game.Console.Print("Registering Callouts..");
            Main.RegisterCallouts();
            Game.Console.Print("=============== FORESTRY CALLOUTS ===============");
            Game.Console.Print();
        }

    }
}