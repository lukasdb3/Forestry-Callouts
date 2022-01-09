using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Mod.API;
using System.Reflection;
using ForestryCallouts.Ini;
using LSPD_First_Response.Mod.Callouts;

namespace ForestryCallouts
{
    internal class Main : Plugin
    {

        public override void Initialize()
        {
            LSPD_First_Response.Mod.API.Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;
            Game.LogTrivial("Plugin ForestryCallouts " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " by lukasdb3 has been initialized");
            Game.LogTrivial("Go on duty to fully load ForestryCallouts.");
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(LSPDFRResolveEventHandler);

        }
        
        public override void Finally()
        {
            Game.LogTrivial("ForestryCallouts has been cleaned up.");
        }

        private static void OnOnDutyStateChangedHandler(bool OnDuty)
        {
            if (OnDuty)
            {
                //Main big log messeage
                Game.Console.Print(
                    "================================================ Forestry Callouts =====================================================");
                Game.Console.Print("Forestry Callouts v"+System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()+" loading...");
                ForestryCallouts.Ini.IniSettings.LoadSettings();
                Game.Console.Print("Forestry Callouts: settings loaded!");
                if (!IniSettings.invalidKeys)
                {
                    //ForestryCallouts.ini main settings
                    Game.Console.Print("-!!- ==================== ForestryCallouts Settings ==================== -!!-");
                    Game.Console.Print("-!!- DialogueKey = " + IniSettings.DialogueKey + "");
                    Game.Console.Print("-!!- EndCalloutKey = " + IniSettings.EndCalloutKey + "");
                    Game.Console.Print("-!!- EndCalloutKey = " + IniSettings.InteractionKey + "");
                    Game.Console.Print("-!!- Other settings loaded!");
                    Game.Console.Print("-!!- ==================== ForestryCallouts Settings ==================== -!!-");
                    Game.Console.Print();
                }

                if (IniSettings.invalidKeys)
                {
                    //ForestryCallouts.ini showing that main settings were set back to default due to invalid key
                    Game.Console.Print("-!!- ==================== ForestryCallouts Settings ==================== -!!-");
                    Game.Console.Print("-!!- All keys set to default settings due to Invalid Key Error.");
                    Game.Console.Print("-!!- Other settings loaded!");
                    Game.Console.Print("============= Forestry Callouts WARNING ==================");
                    Game.Console.Print("Invalid Key detected in ForestryCallouts.Ini");
                    Game.Console.Print("Please make sure that all Key Inputs are valid keys.");
                    Game.Console.Print("If your not sure please send your log to me via ticket option in ULSS or message me.");
                    Game.Console.Print("============= Forestry Callouts WARNING ==================");
                    Game.Console.Print("-!!- ==================== ForestryCallouts Settings ==================== -!!-");
                    Game.Console.Print();
                }
                Game.Console.Print("Forestry Callouts: Checking for update!");
                ForestryCallouts.SimpleFunctions.VersionChecker.IsUpdateAvailable();
                if (SimpleFunctions.VersionChecker.updateCheckFailed)
                {
                    //ForestryCallouts version checker showing that its failed to check for an update
                    Game.Console.Print("-!!- ==================== ForestryCallouts Update ==================== -!!-");
                    Game.Console.Print("-!!- Failed to check for a update.");
                    Game.Console.Print("-!!- Please check if you are online, or try to reload the plugin.");
                    Game.Console.Print("-!!- ==================== ForestryCallouts Update ==================== -!!-");
                    Game.Console.Print();
                }
                
                if (ForestryCallouts.SimpleFunctions.VersionChecker.updateAvailable && !SimpleFunctions.VersionChecker.updateCheckFailed)
                {
                    //ForestryCallouts version checker showing that there is an update available
                    Game.Console.Print("-!!- ==================== ForestryCallouts Update ==================== -!!-");
                    Game.Console.Print("-!!- A new version of Forestry Callouts is available. Please download the newest version for a better experience");
                    Game.Console.Print("-!!- Current Version:  " + IniSettings.CalloutVersion);
                    Game.Console.Print("-!!- Newest Version:  " + SimpleFunctions.VersionChecker.receivedData);
                    Game.Console.Print("-!!- It's recommended you update to prevent any issues that may have been fixed in the new version!");
                    Game.Console.Print("-!!- ==================== ForestryCallouts Update ==================== -!!-");
                    Game.Console.Print();
                }

                if (!ForestryCallouts.SimpleFunctions.VersionChecker.updateAvailable && !SimpleFunctions.VersionChecker.updateCheckFailed)
                {
                    //ForestryCallouts version checker showing that there is not an update available
                    Game.Console.Print("-!!- ==================== ForestryCallouts Update ==================== -!!-");
                    Game.Console.Print("-!!- You are running the newest version of Forestry Callouts!");
                    Game.Console.Print("-!!- Current Version:  " + IniSettings.CalloutVersion);
                    Game.Console.Print("-!!- Newest Version:  " + SimpleFunctions.VersionChecker.receivedData);
                    Game.Console.Print("-!!- Enjoy!");
                    Game.Console.Print("-!!- ==================== ForestryCallouts Update ==================== -!!-");
                    Game.Console.Print();
                }
                
                Game.Console.Print();
                
                //Starts registering callouts
                Game.Console.Print("Forestry Callouts: Registering Callouts!");
                Game.Console.Print(
                    "================================================ Forestry Callouts =====================================================");
                RegisterCallouts();
                Game.DisplayNotification("commonmenu", "shop_franklin_icon_a", "~g~Forestry Callouts", "~g~Plugin Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " ", "Forestry Callouts by lukasdb3 has successfully loaded, enjoy!");
            }
        }

        //Registers callouts
        private static void RegisterCallouts()
        {
            if (!IniSettings.DisableAllCallouts)
            {
                if (IniSettings.IntoxicatedHiker)
                {
                    Functions.RegisterCallout(typeof(Callouts.IntoxicatedHiker));
                }
                if (IniSettings.RecklessDriver)
                {
                    Functions.RegisterCallout(typeof(Callouts.RecklessDriver));
                }
                if (IniSettings.InjuredHiker)
                {
                    Functions.RegisterCallout(typeof(Callouts.InjuredHiker));
                }
                if (IniSettings.WreckedVehicle)
                {
                    Functions.RegisterCallout(typeof(Callouts.WreckedVehicle));
                }
                if (IniSettings.AnimalAttack)
                {
                    Functions.RegisterCallout(typeof(Callouts.AnimalAttack));
                }
                if (IniSettings.SuspiciousVehicle)
                {
                    Functions.RegisterCallout(typeof(Callouts.SuspiciousVehicle));
                }
                if (IniSettings.LoggerPursuit)
                {
                    Functions.RegisterCallout(typeof(Callouts.LoggerPursuit));
                }
                if (IniSettings.HighSpeedPursuit)
                {
                   Functions.RegisterCallout(typeof(Callouts.HighSpeedPursuit));
                }
                if (IniSettings.VehicleOnFire)
                {
                    Functions.RegisterCallout(typeof(Callouts.VehicleOnFire));
                }
                if (IniSettings.MissingHiker)
                {
                    Functions.RegisterCallout(typeof(Callouts.MissingHiker));
                }
                if (IniSettings.IllegalCamping)
                {
                    Functions.RegisterCallout(typeof(Callouts.IllegalCamping));
                }
                if (IniSettings.RangerRequestingBackup)
                {
                    Functions.RegisterCallout(typeof(Callouts.RangerBackup));
                }
                if (IniSettings.DeadAnimalBlockingRoad)
                {
                    Functions.RegisterCallout(typeof(Callouts.DeadAnimal));
                }
                if (IniSettings.DangerousPerson)
                {
                    Functions.RegisterCallout(typeof(Callouts.DangerousPerson));
                }
                if (IniSettings.IllegalHunting)
                {
                    Functions.RegisterCallout(typeof(Callouts.IllegalHunting));
                }
                if (IniSettings.IllegalFishing)
                {
                    Functions.RegisterCallout(typeof(Callouts.IllegalFishing));
                }
            }
            if (IniSettings.DisableAllCallouts)
            {
                Game.LogTrivial("-!!- Forestry Callouts - |Main| - No Callouts Loaded, DisableAllCallouts option is set to true. -!!-");
            }
        }

        public static Assembly LSPDFRResolveEventHandler(object sender, ResolveEventArgs args)
        {
            foreach (Assembly assembly in LSPD_First_Response.Mod.API.Functions.GetAllUserPlugins())
            {
                if (args.Name.ToLower().Contains(assembly.GetName().Name.ToLower()))
                {
                    return assembly;
                }
            }
            return null;
        }

        public static bool IsLSPDFRPluginRunning(string Plugin, Version minversion = null)
        {
            foreach (Assembly assembly in LSPD_First_Response.Mod.API.Functions.GetAllUserPlugins())
            {
                AssemblyName an = assembly.GetName();
                if (an.Name.ToLower() == Plugin.ToLower())
                {
                    if (minversion == null || an.Version.CompareTo(minversion) >= 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}