﻿#region Refrences
//Rage
using System;
using System.Windows.Forms;
using Rage;
//Lspdfr
using LSPD_First_Response.Mod.API;
//Rage Native UI
using RAGENativeUI;
//Forestry Callouts 2
using ForestryCallouts2.Backbone;
using ForestryCallouts2.Backbone.IniConfiguration;
using ForestryCallouts2.Backbone.Menu;
using ForestryCallouts2.Backbone.Functions;
#endregion

namespace ForestryCallouts2
{
    internal class Main : Plugin
    {
        internal static Random Rnd = new Random();
        internal static MenuPool pool = new();
        private static GameFiber _mainFiber;
        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;
            Game.LogTrivial("Plugin ForestryCallouts2 " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " by lukasdb3 has been initialized");
            Game.LogTrivial("Go on duty to fully load ForestryCallouts2.");
        }
        
        public override void Finally()
        {
            Create.CleanUp();
            _mainFiber.Abort();
            AmbientEvents.Main.CleanUp();
            
            Game.LogTrivial("ForestryCallouts2 has been cleaned up.");
        }

        private static void OnOnDutyStateChangedHandler(bool OnDuty)
        {
            //When player goes on duty FC fully loads
            if (OnDuty)
            {
                Logger.StartLoadingPhase();
                Game.DisplayNotification("commonmenu", "shop_franklin_icon_a", "~g~Forestry Callouts 2", "~g~Plugin Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " ", "Plugin Loaded! Enjoy!");
            }
        }

        //Registers all callouts
        internal static void RegisterCallouts()
        {
            if (IniSettings.IntoxPerson)  Functions.RegisterCallout(typeof(Callouts.LandCallouts.IntoxicatedPerson));
            if (IniSettings.RegularPursuit) Functions.RegisterCallout(typeof(Callouts.LandCallouts.RegularPursuit));
            if (IniSettings.AnimalAttack) Functions.RegisterCallout(typeof(Callouts.LandCallouts.AnimalAttack));
            if (IniSettings.DirtBikePursuit) Functions.RegisterCallout(typeof(Callouts.LandCallouts.DirtBikePursuit));
            if (IniSettings.AtvPursuit) Functions.RegisterCallout(typeof(Callouts.LandCallouts.AtvPursuit));
            if (IniSettings.HighSpeedPursuit) Functions.RegisterCallout(typeof(Callouts.LandCallouts.HighSpeedPursuit));
            if (IniSettings.LoggerTruckPursuit) Functions.RegisterCallout(typeof(Callouts.LandCallouts.LoggerTruckPursuit));
            if (IniSettings.DeadAnimalOnRoadway) Functions.RegisterCallout(typeof(Callouts.LandCallouts.DeadAnimalOnRoadway));
        }
        
        //GameFiber that runs constantly for interaction menu and binoculars
        internal static void RunLoop()
        {
            _mainFiber = GameFiber.StartNew(delegate
            {
                while (true)
                {
                    GameFiber.Yield();
                    
                    //Menu
                    pool.ProcessMenus();
                    if (Game.IsKeyDown(IniSettings.InteractionMenuKey) && !Binoculars.IsRendering)
                    {
                        if (Create.InteractionMenu.Visible) Create.InteractionMenu.Visible = false;
                        else Create.InteractionMenu.Visible = true;
                    }

                    //Binoculars Hotkey
                    if (Game.IsKeyDown(IniSettings.BinocularsKey) && IniSettings.BinocularsEnabled && !Binoculars.IsRendering && Game.LocalPlayer.Character.IsOnFoot)
                    {
                        Binoculars.Enable();
                    }
                }
            });
        }
    }
}