#region Refrences
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
using ForestryCallouts2.Backbone.SpawnSystem;
using MainMenu = ForestryCallouts2.Backbone.Menu.MainMenu;

#endregion

namespace ForestryCallouts2
{
    internal class Main : Plugin
    {
        internal static readonly MenuPool Pool = new();
        private static GameFiber _mainFiber;
        internal static string CallsignAudioString;
        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;
            Game.LogTrivial("Plugin ForestryCallouts2 " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " has been initialized");
            Game.LogTrivial("Go on duty to fully load ForestryCallouts2.");
        }
        
        public override void Finally()
        {
            StopPedFiber.Fiber.Abort();
            GrabPedFiber.Fiber.Abort();
            if (AnimalControl.AnimalControlActive) AnimalControl.DestroyAnimalControl();
            MainMenu.CleanUp();
            _mainFiber.Abort();
            Game.LogTrivial("ForestryCallouts2 has been cleaned up.");
        }

        private static void OnOnDutyStateChangedHandler(bool onDuty)
        {
            //When player goes on duty FC fully loads
            if (onDuty)
            {
                Game.Console.Print();
                Game.Console.Print("=============== FORESTRY CALLOUTS ===============");
                Game.Console.Print("Loading settings..");
                IniSettings.LoadSettings();
                CallsignAudioString = IniSettings.Callsign.TranslateCallsignToAudio();
                Game.Console.Print("Checking Forestry Callouts version..");
                Game.Console.Print("Forestry Callouts update available: "+VersionChecker.IsUpdateAvailable()+"");
                Game.Console.Print("Version: "+VersionChecker.ReceivedData+"");
                Game.Console.Print("Initializing menus..");
                MainMenu.Initialize();
                StopPedMenu.Initialize();
                Game.Console.Print("Starting Main Loop..");
                RunLoop();
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
                Game.Console.Print("Registering Callouts..");
                RegisterCallouts();
                Game.Console.Print("=============== FORESTRY CALLOUTS ===============");
                Game.Console.Print();
                CalloutCache.CacheCallouts();
                Game.DisplayNotification("commonmenu", "shop_franklin_icon_a", "~g~ForestryCallouts2", "~g~Plugin Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " ", "Plugin Loaded! Enjoy!");
            }
        }

        //Registers all callouts
        private static void RegisterCallouts()
        {
            if (!IniSettings.WaterCallouts)
            {
                if (IniSettings.AnimalAttack) Functions.RegisterCallout(typeof(Callouts.LandCallouts.AnimalAttack));
                if (IniSettings.AnimalAttack) Functions.RegisterCallout(typeof(Callouts.LandCallouts.AnimalAttack2));
                if (IniSettings.AnimalOnRoadway) Functions.RegisterCallout(typeof(Callouts.LandCallouts.AnimalOnRoadway));
                if (IniSettings.AtvPursuit) Functions.RegisterCallout(typeof(Callouts.LandCallouts.AtvPursuit));
                if (IniSettings.DangerousPerson) Functions.RegisterCallout(typeof(Callouts.LandCallouts.DangerousPerson));
                if (IniSettings.DeadAnimalOnRoadway) Functions.RegisterCallout(typeof(Callouts.LandCallouts.DeadAnimalOnRoadway));
                if (IniSettings.DeadBody) Functions.RegisterCallout(typeof(Callouts.LandCallouts.DeadBody));
                if (IniSettings.DirtBikePursuit) Functions.RegisterCallout(typeof(Callouts.LandCallouts.DirtBikePursuit));
                if (IniSettings.HighSpeedPursuit) Functions.RegisterCallout(typeof(Callouts.LandCallouts.HighSpeedPursuit)); 
                if (IniSettings.IntoxPerson) Functions.RegisterCallout(typeof(Callouts.LandCallouts.IntoxicatedPerson));
                if (IniSettings.LoggerTruckPursuit) Functions.RegisterCallout(typeof(Callouts.LandCallouts.LoggerTruckPursuit));
                if (IniSettings.RegularPursuit) Functions.RegisterCallout(typeof(Callouts.LandCallouts.RegularPursuit));
            }
        }
        
        //GameFiber that runs constantly for interaction menu and binoculars
        private static void RunLoop()
        {
            _mainFiber = GameFiber.StartNew(delegate
            {
                while (true)
                {
                    GameFiber.Yield();
                    
                    //Menu
                    Pool.ProcessMenus();
                    if (CFunctions.IsKeyAndModifierDown(IniSettings.InteractionMenuKey, IniSettings.InteractionMenuKeyModifier) && !Binoculars.IsRendering)
                    {
                        MainMenu.InteractionMenu.Visible = !MainMenu.InteractionMenu.Visible;
                    }

                    //Binoculars Hotkey
                    if (CFunctions.IsKeyAndModifierDown(IniSettings.BinocularsKey, IniSettings.BinocularsKeyModifier) && IniSettings.BinocularsEnabled && !Binoculars.IsRendering && Game.LocalPlayer.Character.IsOnFoot && Binoculars.BinoKeyEnabled)
                    {
                        Binoculars.Enable();
                    }
                }
            });
        }
    }
}