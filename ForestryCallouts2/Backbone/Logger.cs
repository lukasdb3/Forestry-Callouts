#region Refrences
//Rage
using System;
using ForestryCallouts2.Backbone.Functions;
using Rage;
//LSPDFR
using LSPD_First_Response.Mod.Callouts;
//ForestryCallouts2
using ForestryCallouts2.Backbone.IniConfiguration;
using ForestryCallouts2.Backbone.Menu;
using ForestryCallouts2.Backbone.SpawnSystem;

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
            Main.CallsignAudioString = IniSettings.Callsign.TranslateCallsignToAudio();
            Game.Console.Print("Checking Forestry Callouts version..");
            Game.Console.Print("Forestry Callouts update available: "+VersionChecker.IsUpdateAvailable()+"");
            Game.Console.Print("Version: "+VersionChecker.ReceivedData+"");
            Game.Console.Print("Running Plugin Checks...");
            Game.Console.Print("Caching players callouts..");
            CalloutsGetter.CacheCallouts();
            Game.Console.Print("Initializing menus..");
            MainMenu.Initialize();
            StopPedMenu.Initialize();
            Game.Console.Print("Starting Main Loop..");
            Main.RunLoop();
            if (PluginChecker.ForestryCallouts)
            {
                Game.Console.Print();
                Game.Console.Print("===== WARNING =====");
                Game.Console.Print("The first version of ForestryCallouts was detected still in the plugins folder.");
                Game.Console.Print("Please remove ForestryCallouts and reload LSPDFR");
                Game.Console.Print("===== WARNING =====");
                Game.Console.Print();
            }
            Game.Console.Print("Loading needed chunks...");
            if (!IniSettings.WaterCallouts) ChunkLoader.Land();
            else ChunkLoader.Water();
            GrabPedFiber.Main();
            StopPedFiber.Main();
            Game.AddConsoleCommands(new[]{typeof(CCommands)});
            Game.Console.Print("Registering Callouts..");
            Main.RegisterCallouts();
            Game.Console.Print("=============== FORESTRY CALLOUTS ===============");
            Game.Console.Print();
        }

    }
}