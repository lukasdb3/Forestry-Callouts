using System;
using Rage;
using LSPD_First_Response.Mod.API;
using ForestryCallouts2.Backbone;

namespace ForestryCallouts2
{
    internal class Main : Plugin
    {
        //When user loads up LSPDFR, FC initializes.
        public override void Initialize()
        {
            LSPD_First_Response.Mod.API.Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;
            Game.LogTrivial("Plugin ForestryCallouts2 " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " by lukasdb3 has been initialized");
            Game.LogTrivial("Go on duty to fully load ForestryCallouts2.");
        }
        
        public override void Finally()
        {
            Game.LogTrivial("ForestryCallouts2 has been cleaned up.");
        }

        private static void OnOnDutyStateChangedHandler(bool OnDuty)
        {
            //When player goes on duty FC fully loads
            if (OnDuty)
            {
                Logger.StartLoadingPhase();
            }
        }

        //Registers all callouts
        internal static void RegisterCallouts()
        {
            //Player is using water calls so we register them and dont register land callouts
            if (IniSettings.WaterCalls)
            {
                
            }
            
            //Player is not using water callouts and wants all land callouts to be loaded
            if (IniSettings.AllCalls && !IniSettings.WaterCalls)
            {
                
            }

            //Player is not using water callouts and only want park ranger related calls to be registered
            if (!IniSettings.WaterCalls)
            {
                if (IniSettings.IntoxPerson) Functions.RegisterCallout(typeof(Callouts.LandCallouts.IntoxicatedPerson));   
                
            }

        }
        
    }
}