using System;
using Rage;
using LSPD_First_Response.Mod.API;
using System.Reflection;
using LSPD_First_Response.Mod.Callouts;
using SAHighwayCallouts.Ini;
using SAHighwayCallouts.Functions;

namespace SAHighwayCallouts.Functions.Logger
{
    internal static class OnDutyMessage
    {
        internal static void Main()
        {
            Game.Console.Print("========================= SAHighwayCallouts =========================");
            Game.Console.Print("SAHighwayCallouts v"+System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()+" loading settings..");
            Settings.LoadSettings();
            //SAHighwayCallouts.ini main settings if no invalid keys.
            if (!Settings.invalidKeys)
            {
                Game.Console.Print("=============== SAHighwayCallouts Settings ===============");
                Game.Console.Print("DialogueKey = " + Settings.DialogueKey + "");
                Game.Console.Print("EndCalloutKey = " + Settings.EndCalloutKey + "");
                Game.Console.Print("Interaction Key = " + Settings.InteractionKey + "");
                Game.Console.Print("LuxuryVehiclesNumber = " + Settings.luxuryVehiclesArray.Length + " | AddedCars = "+Settings.LuxuryVehicleAddons+"");
                Game.Console.Print("NormalVehiclesNumber = " + Settings.NormalVehiclesArray.Length + " | AddedCars = "+Settings.NormalVehiclesAddons+"");
                Game.Console.Print("=============== SAHighwayCallouts Settings ===============");
            }
            //SAHighwayCallouts.ini showing that main settings were set back to default due to invalid key error.
            if (Settings.invalidKeys)
            {
                Game.Console.Print("=============== SAHighwayCallouts Settings ===============");
                Game.Console.Print("All keys set to default settings due to Invalid Key Error.");
                Game.Console.Print("LuxuryVehiclesNumber = " + Settings.luxuryVehiclesArray.Length + " | AddedCars = "+Settings.LuxuryVehicleAddons+""); //WOW POG
                Game.Console.Print("NormalVehiclesNumber = " + Settings.NormalVehiclesArray.Length + " | AddedCars = "+Settings.NormalVehiclesAddons+"");
                Game.Console.Print();
                Game.Console.Print("=============== INI Settings WARNING ===============");
                Game.Console.Print("Invalid Key detected in SAHighwayCallouts.ini");
                Game.Console.Print("Please make sure that all Key inputs are valid keys.");
                Game.Console.Print(
                    "If your not sure please send your log to me via ticket option in ULSS or message me.");
                Game.Console.Print("=============== INI Settings WARNING ===============");
                Game.Console.Print("=============== SAHighwayCallouts Settings ===============");
            }
            Game.Console.Print();
            Game.Console.Print("Checking for any new updates to SAHighwayCallouts..");
            VersionChecker.IsUpdateAvailable();
            if (VersionChecker.updateCheckFailed)
               {
                   //SAHighwayCallouts version checker showing that its failed to check for an update
                   Game.Console.Print("==================== SAHighwayCallouts Update ====================");
                   Game.Console.Print("Failed to check for a update.");
                   Game.Console.Print("Please check if you are online, or try to reload the plugin.");
                   Game.Console.Print("==================== SAHighwayCallouts Update ====================");
                   Game.Console.Print();
               }
               if (VersionChecker.updateAvailable && !VersionChecker.updateCheckFailed)
               {
                   //SAHighwayCallouts version checker showing that there is an update available
                   Game.Console.Print("==================== SAHighwayCallouts Update ====================");
                   Game.Console.Print("A new version of SAHighwayCallouts was detected!");
                   Game.Console.Print("Please update it to get bug fixes and enhancements for the plugin.");
                   Game.Console.Print("Installed Version = "+Settings.calloutVersion+"");
                   Game.Console.Print("Newest Version = "+VersionChecker.receivedData+"");
                   Game.Console.Print("==================== SAHighwayCallouts Update ====================");
                   Game.Console.Print();
               }

               if (!VersionChecker.updateAvailable && !VersionChecker.updateCheckFailed)
               {
                   //SAHighwayCallouts version checker showing that there is not an update available
                   Game.Console.Print("==================== SAHighwayCallouts Update ====================");
                   Game.Console.Print("No new version of SAHighwayCallouts was detected!");
                   Game.Console.Print("You have the newest version installed.");
                   Game.Console.Print("Installed Version = "+Settings.calloutVersion+"");
                   Game.Console.Print("Newest Version = "+VersionChecker.receivedData+"");
                   Game.Console.Print("==================== SAHighwayCallouts Update ====================");
                   Game.Console.Print();
               }
               Game.Console.Print("Loading commands!");
               Game.AddConsoleCommands(new[]{typeof(Functions.Commands)});
               Game.Console.Print("Registering callouts!");
               Game.Console.Print("========================= SAHighwayCallouts =========================");
        }
    }
}