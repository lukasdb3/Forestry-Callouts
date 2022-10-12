using System;
using Rage;
using LSPD_First_Response.Mod.API;
using ForestryCallouts2.Backbone;
using ForestryCallouts2.Backbone.IniConfiguration;

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
                RegisterCallouts();
            }
        }

        //Registers all callouts
        internal static void RegisterCallouts()
        {
            //Player is using water calls so we register them and dont register land callouts
            if (IniSettings.WaterCalls)
            {
                
            }
            
            //Player is not using water callouts and wants land callouts to be loaded
            if (!IniSettings.WaterCalls)
            {
                if (IniSettings.IntoxPerson)  Functions.RegisterCallout(typeof(Callouts.LandCallouts.IntoxicatedPerson));
                
                //If player wants all land callouts the below calls are also loaded
                if (IniSettings.AllCalls)
                {
                    
                }
            }
            

        }
        
    }
}