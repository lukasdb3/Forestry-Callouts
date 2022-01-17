using System;
using Rage;
using LSPD_First_Response.Mod.API;
using System.Reflection;
using LSPD_First_Response.Mod.Callouts;
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
                Functions.Logger.OnDutyMessage.Main();
                RegisterCallouts();
                Game.DisplayNotification("commonmenu", "shop_franklin_icon_a", "~h~SAHighwayCallouts", "~b~Plugin Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " ", "SAHighwayCallouts by lukasdb3 has successfully loaded, enjoy!");
            }
        }

         private static void RegisterCallouts()
         {
             if (Settings.DisableAllCallouts) Game.Console.Print("-!!- SAHighwayCallouts - |Settings| - All callouts disabled.");
             if (!Settings.DisableAllCallouts)
             {
                 if (Settings.NormalVehiclePursuit) LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(Callouts.NormalVehiclePursuit));
                 if (Settings.SemiTruckPursuit) LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(Callouts.SemiTruckPursuit));
                 if (Settings.LuxuryVehiclePursuit) LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(Callouts.LuxuryVehiclePursuit));
                 if (Settings.GrandTheftAuto) LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(Callouts.GrandTheftAuto));
                 if (Settings.AbandonVehicle) LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(Callouts.AbandonVehicle));
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