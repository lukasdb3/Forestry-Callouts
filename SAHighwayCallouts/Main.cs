using System;
using Rage;
using LSPD_First_Response.Mod.API;
using System.Reflection;
using SAHighwayCallouts.Ini;
using SAHighwayCallouts.Functions;

namespace SAHighwayCallouts
{
    internal class Main : Plugin
    {
        public override void Initialize()
        {
            LSPD_First_Response.Mod.API.Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;
            Game.LogTrivial("Plugin SAHighwayCallouts " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " by lukasdb3 has been initialized");
            Game.LogTrivial("Please go on duty to fully load SAHighwayCallouts.");
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(LSPDFRResolveEventHandler);
        }

        public override void Finally()
        {
            Game.LogTrivial("SAHighwayCallouts has been cleaned up.");
        }
        
         private static void OnOnDutyStateChangedHandler(bool OnDuty)
        {
            if (OnDuty)
            {
                Game.Console.Print(
                   "================================================ SAHighwayCallouts =====================================================");
               Game.Console.Print("SAHighwayCallouts v"+System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()+" loading...");
               Settings.LoadSettings();
                if (!Settings.invalidKeys)
                {
                    //SAHighwayCallouts.ini main settings
                    Game.Console.Print("-!!- ==================== SAHighwayCallouts Settings ==================== -!!-");
                    Game.Console.Print("-!!- DialogueKey = " + Settings.DialogueKey + "");
                    Game.Console.Print("-!!- EndCalloutKey = " + Settings.EndCalloutKey + "");
                    Game.Console.Print("-!!- EndCalloutKey = " + Settings.InteractionKey + "");
                    Game.Console.Print("-!!- LuxuryVehiclesNumber = " + Settings.luxuryVehiclesArray.Length + " | AddedCars = "+Settings.LuxuryVehicleAddons+""); //WOW POG
                    Game.Console.Print("-!!- NormalVehiclesNumber = " + Settings.NormalVehiclesArray.Length + " | AddedCars = "+Settings.NormalVehiclesAddons+"");
                    Game.Console.Print("-!!- ==================== SAHighwayCallouts Settings ==================== -!!-");
                    Game.Console.Print("-!!- Loading commands!");
                    Game.AddConsoleCommands(new[]{typeof(Functions.Commands)});
                    Game.Console.Print();
                }

               if (Settings.invalidKeys)
               {
                   //SAHighwayCallouts.ini showing that main settings were set back to default due to invalid key
                   Game.Console.Print("-!!- ==================== SAHighwayCallouts Settings ==================== -!!-");
                   Game.Console.Print("-!!- All keys set to default settings due to Invalid Key Error.");
                   Game.Console.Print("-!!- LuxuryVehiclesNumber = " + Settings.luxuryVehiclesArray.Length + " | AddedCars = "+Settings.LuxuryVehicleAddons+""); //WOW POG
                   Game.Console.Print("-!!- NormalVehiclesNumber = " + Settings.NormalVehiclesArray.Length + " | AddedCars = "+Settings.NormalVehiclesAddons+"");
                   Game.Console.Print("============= SAHighwayCallouts WARNING ==================");
                   Game.Console.Print("Invalid Key detected in SAHighwayCallouts.ini");
                   Game.Console.Print("Please make sure that all Key Inputs are valid keys.");
                   Game.Console.Print(
                       "If your not sure please send your log to me via ticket option in ULSS or message me.");
                   Game.Console.Print("============= SAHighwayCallouts WARNING ==================");
                   Game.Console.Print("-!!- ==================== SAHighwayCallouts Settings ==================== -!!-");
                   Game.Console.Print();
                   Game.Console.Print("SAHighwayCallouts: checking for update!");
                   VersionChecker.IsUpdateAvailable();
               }

               if (VersionChecker.updateCheckFailed)
               {
                   //SAHighwayCallouts version checker showing that its failed to check for an update
                   Game.Console.Print("-!!- ==================== SAHighwayCallouts Update ==================== -!!-");
                   Game.Console.Print("-!!- Failed to check for a update.");
                   Game.Console.Print("-!!- Please check if you are online, or try to reload the plugin.");
                   Game.Console.Print("-!!- ==================== SAHighwayCallouts Update ==================== -!!-");
                   Game.Console.Print();
               }
               if (VersionChecker.updateAvailable && !VersionChecker.updateCheckFailed)
               {
                   //SAHighwayCallouts version checker showing that there is an update available
                   Game.Console.Print("-!!- ==================== SAHighwayCallouts Update ==================== -!!-");
                   Game.Console.Print("-!!- A new version of SAHighwayCallouts is available. Please download the newest version for a better experience");
                   Game.Console.Print("-!!- Current Version:  " + Settings.calloutVersion);
                   Game.Console.Print("-!!- Newest Version:  " + VersionChecker.receivedData);
                   Game.Console.Print("-!!- It's recommended you update to prevent any issues that may have been fixed in the new version!");
                   Game.Console.Print("-!!- ==================== SAHighwayCallouts Update ==================== -!!-");
                   Game.Console.Print();
               }

               if (!VersionChecker.updateAvailable && !VersionChecker.updateCheckFailed)
               {
                   //SAHighwayCallouts version checker showing that there is not an update available
                   Game.Console.Print("-!!- ==================== SAHighwayCallouts Update ==================== -!!-");
                   Game.Console.Print("-!!- You are running the newest version of SAHighwayCallouts!");
                   Game.Console.Print("-!!- Current Version:  " + Settings.calloutVersion); 
                   Game.Console.Print("-!!- Newest Version:  " + VersionChecker.receivedData);
                   Game.Console.Print("-!!- Enjoy!");
                   Game.Console.Print("-!!- ==================== SAHighwayCallouts Update ==================== -!!-");
                   Game.Console.Print();
               }
               Game.Console.Print("SAHighwayCallouts: registering callouts!");
                RegisterCallouts();
                Game.Console.Print(
                    "================================================ SAHighwayCallouts =====================================================");
                Game.DisplayNotification("commonmenu", "shop_franklin_icon_a", "~h~SAHighwayCallouts", "~b~Plugin Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " ", "SAHighwayCallouts by lukasdb3 has successfully loaded, enjoy!");
            }
        }

         private static void RegisterCallouts()
         {
             if (Settings.DisableAllCallouts) Game.Console.Print("-!!- SAHighwayCallouts - |Settings| - All callouts disabled.");
             if (!Settings.DisableAllCallouts)
             {
                 if (Settings.LuxuryVehiclePursuit) LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(Callouts.PursuitCallouts.VehiclePursuit));
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
    }
}