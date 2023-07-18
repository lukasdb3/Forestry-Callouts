#region Refrences
//System
using System.Drawing;
//Rage
using Rage;
//RageNativeUI
using RAGENativeUI;
using RAGENativeUI.Elements;
//ForestryCallouts2
using ForestryCallouts2.Backbone.Functions;
using ForestryCallouts2.Backbone.IniConfiguration;
#endregion

namespace ForestryCallouts2.Backbone.Menu
{
    public class Create
    {
        // Version
        private static readonly string Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        
        // Menus
        internal static UIMenu InteractionMenu = new("", "");
        private static UIMenu _optionsMenu = new("", "");
        private static UIMenu _calloutMenu = new("", "");
        private static UIMenu _settingsMenu = new("", "");

        // Interaction Menu
        private static UIMenuItem _callAnimalControl;
        private static UIMenuItem _endCallout;
        private static UIMenuItem _options;

        // Options Menu
        private static UIMenuItem _startCallout;
        private static UIMenuItem _settings;

        // Settings Menu
        //Main
        internal static UIMenuListItem DebugLogs;
        internal static UIMenuListItem WaterCallouts;
        internal static UIMenuNumericScrollerItem<int> SearchAreaBlipsMax;
        internal static UIMenuListItem EnableDistanceChecker;
        internal static UIMenuNumericScrollerItem<double> MaxDistance;
        internal static UIMenuNumericScrollerItem<int> MinCalloutDistance;
        //Binoculars
        internal static UIMenuListItem EnableBinoculars;
        internal static UIMenuNumericScrollerItem<int> BinocularsSense;
        //Callouts
        internal static UIMenuListItem AnimalAttack;
        internal static UIMenuListItem AtvPursuit;
        internal static UIMenuListItem DangerousPerson;
        internal static UIMenuListItem DeadAnimalOnRoadway;
        internal static UIMenuListItem DirtBikePursuit;
        internal static UIMenuListItem HighSpeedPursuit;
        internal static UIMenuListItem IntoxicatedPerson;
        internal static UIMenuListItem LoggerTruckPursuit;
        internal static UIMenuListItem RegularPursuit;

        internal static UIMenuListItem DeadBodyWater;
        internal static UIMenuListItem BoatPursuit;
        
        private static UIMenuItem _saveSettings;
        private static UIMenuItem _reload;

        internal static void Initialize()
        {
            // Create Interaction Menu
            InteractionMenu = new("Forestry Callouts", "~b~Interaction Menu ~g~| ~y~v" + Version);
            InteractionMenu.SetBannerType(Color.ForestGreen);
            InteractionMenu.MouseControlsEnabled = false;
            
            _callAnimalControl = new("Animal Control", "Call Animal Control To Pick Up Dead Animal");
            _endCallout = new("End Callout", "End Current Callout");
            _options = new("Options", "");
            InteractionMenu.AddItems(_callAnimalControl,_endCallout,_options);
            InteractionMenu.RefreshIndex();
            InteractionMenu.OnItemSelect += OnInteractionMenuItemSelected;
            Main.pool.Add(InteractionMenu);
            
            // Create Options Menu
            _optionsMenu = new("Forestry Callouts", "~b~Options Menu ~g~| ~y~v" + Version);
            _optionsMenu.SetBannerType(Color.ForestGreen);
            _optionsMenu.MouseControlsEnabled = false;

            _startCallout = new("Callouts", "");
            _settings = new("Settings", "");
            _optionsMenu.AddItems(_startCallout, _settings);
            InteractionMenu.BindMenuToItem(_optionsMenu, _options);
            _optionsMenu.RefreshIndex();
            _optionsMenu.OnItemSelect += OnOptionsMenuItemSelected;
            Main.pool.Add(_optionsMenu);

            // Create Callout Menu
            _calloutMenu = new("Forestry Callouts", "~b~Callout Menu ~g~| ~y~v" + Version);
            _calloutMenu.SetBannerType(Color.ForestGreen);
            _calloutMenu.MouseControlsEnabled = false;

            if (!IniSettings.WaterCallouts)
            {
                foreach (var callout in CalloutsGetter.ForestryCalloutsCalls)
                {
                    if (IniSettings.Ini.ReadBoolean("Callouts", callout)) _calloutMenu.AddItem(new UIMenuItem(callout));
                }
            }
            else
            {
                foreach (var callout in CalloutsGetter.ForestryCalloutsWaterCalls)
                {
                    Game.Console.Print(callout);
                    if (IniSettings.Ini.ReadBoolean("Callouts", callout)) _calloutMenu.AddItem(new UIMenuItem(callout));
                }
            }
            
            _optionsMenu.BindMenuToItem(_calloutMenu, _startCallout);
            _calloutMenu.RefreshIndex();
            _calloutMenu.OnItemSelect += OnCalloutMenuItemSelected;
            Main.pool.Add(_calloutMenu);

            // Create Settings Menu
            _settingsMenu = new UIMenu("Forestry Callouts", "~b~Settings Menu ~g~| ~y~v" + Version);
            _settingsMenu.SetBannerType(Color.ForestGreen);
            _settingsMenu.MouseControlsEnabled = false;
            //Main
            DebugLogs = new UIMenuListItem("DebugsLogs", "For Debugging Forestry Callouts Crashes", IniSettings.DebugLogs.ToString().ToLower(), (IniSettings.DebugLogs) ? "false" : "true");
            WaterCallouts = new UIMenuListItem("WaterCallouts", "Disables And Enables Water Callouts", IniSettings.WaterCallouts.ToString().ToLower(), (IniSettings.WaterCallouts) ? "false" : "true");
            SearchAreaBlipsMax = new UIMenuNumericScrollerItem<int>("SearchAreaBlipsMax", "Amount of Search Areas Sent Before Object is Blipped", 5, 15, 1);
            SearchAreaBlipsMax.Value = IniSettings.SearchAreaNotifications;
            EnableDistanceChecker = new UIMenuListItem("EnableDistanceChecker", "Disables And Enables Distance Checker", IniSettings.EnableDistanceChecker, (IniSettings.EnableDistanceChecker) ? "false" : "true");
            MaxDistance = new UIMenuNumericScrollerItem<double>("MaxDistance", "The Max Distance A Callout Will Spawn Away From You (meters)", 1000, 10000, 100);
            MaxDistance.Value = IniSettings.MaxDistance;
            MinCalloutDistance = new UIMenuNumericScrollerItem<int>("MinimumCalloutSpawnDistance", "The Minimum Distance A Callout Will Spawn Away From You (meters)", 30, 300, 1);
            MinCalloutDistance.Value = IniSettings.MinCalloutDistance;
            //Binoculars
            EnableBinoculars = new UIMenuListItem("EnableBinoculars", "Disables And Enables Binoculars", IniSettings.BinocularsEnabled.ToString().ToLower(), (IniSettings.BinocularsEnabled) ? "false" : "true");
            BinocularsSense = new UIMenuNumericScrollerItem<int>("BinocularsSensitivity", "Binoculars Horizontal Sensitivity",1, 10, 1);
            BinocularsSense.Value = IniSettings.BinocularsSensitivity;
            //Callouts
            AnimalAttack = new UIMenuListItem("AnimalAttack", "",IniSettings.AnimalAttack.ToString().ToLower(), (IniSettings.AnimalAttack) ? "false" : "true");
            AtvPursuit = new UIMenuListItem("AtvPursuit", "", IniSettings.AtvPursuit.ToString().ToLower(), (IniSettings.AtvPursuit) ? "false" : "true");
            DangerousPerson = new UIMenuListItem("DangerousPerson", "", IniSettings.DangerousPerson.ToString().ToLower(), (IniSettings.DangerousPerson) ? "false" : "true");
            DeadAnimalOnRoadway = new UIMenuListItem("DeadAnimalRoadway", "", IniSettings.DeadAnimalOnRoadway.ToString().ToLower(), (IniSettings.DeadAnimalOnRoadway) ? "false" : "true");
            DirtBikePursuit = new UIMenuListItem("DirtBikePursuit", "",IniSettings.DirtBikePursuit.ToString().ToLower(), (IniSettings.DirtBikePursuit) ? "false" : "true");
            HighSpeedPursuit = new UIMenuListItem("HighSpeedPursuit", "", IniSettings.HighSpeedPursuit.ToString().ToLower(), (IniSettings.HighSpeedPursuit) ? "false" : "true");
            IntoxicatedPerson = new UIMenuListItem("IntoxicatedPerson", "",IniSettings.IntoxPerson.ToString().ToLower(), (IniSettings.IntoxPerson) ? "false" : "true");
            LoggerTruckPursuit = new UIMenuListItem("LoggerTruckPursuit", "", IniSettings.LoggerTruckPursuit.ToString().ToLower(), (IniSettings.LoggerTruckPursuit) ? "false" : "true");
            RegularPursuit = new UIMenuListItem("RegularPursuit", "",IniSettings.RegularPursuit.ToString().ToLower(), (IniSettings.RegularPursuit) ? "false" : "true");

            DeadBodyWater = new UIMenuListItem("DeadBodyWater", "", IniSettings.DeadBodyWater.ToString().ToLower(), (IniSettings.DeadBodyWater) ? "false" : "true");
            BoatPursuit = new UIMenuListItem("BoatPursuit", "", IniSettings.BoatPursuit.ToString().ToLower(), (IniSettings.BoatPursuit) ? "false" : "true");
            //Buttons for saving and reloading Ini
            _saveSettings = new UIMenuItem("~g~Save Settings", "~r~Required To Press If Settings Were Just Changed");
            _reload = new UIMenuItem("~b~Reload", "Reloads Forestry Callouts Settings and AmbientEvents");
            _settingsMenu.AddItems(DebugLogs ,SearchAreaBlipsMax, MaxDistance, MinCalloutDistance, EnableBinoculars, BinocularsSense, IntoxicatedPerson, AnimalAttack, AtvPursuit, DangerousPerson, DeadAnimalOnRoadway,
                DirtBikePursuit, HighSpeedPursuit, IntoxicatedPerson, LoggerTruckPursuit, RegularPursuit, DeadBodyWater, BoatPursuit, _saveSettings, _reload);
            _optionsMenu.BindMenuToItem(_settingsMenu, _settings);
            _settingsMenu.RefreshIndex();
            _settingsMenu.OnItemSelect += OnSettingsMenuItemSelected;
            Main.pool.Add(_settingsMenu);
        }

        private static void OnInteractionMenuItemSelected(UIMenu sender, UIMenuItem selecteditem, int index)
        {
            Logger.DebugLog("INTERACTION MENU", "Item " + selecteditem.Text + " was selected!");
            if (selecteditem == _callAnimalControl)
            {
                GameFiber.StartNew(delegate
                {
                    AnimalControl.CallAnimalControl();
                });
            }

            if (selecteditem == _endCallout)
            {
                if (LSPD_First_Response.Mod.API.Functions.IsCalloutRunning())
                {
                    LSPD_First_Response.Mod.API.Functions.StopCurrentCallout();
                    Logger.DebugLog("INTERACTION MENU", "Ended current callout");
                }
                Game.DisplayNotification("~g~There Is No Callout Running");
                Logger.DebugLog("INTERACTION MENU", "There is no callout to end");
            }
        }
        
        private static void OnOptionsMenuItemSelected(UIMenu sender, UIMenuItem selecteditem, int index)
        {
        }
        
        private static void OnCalloutMenuItemSelected(UIMenu sender, UIMenuItem selecteditem, int index)
        {
            Logger.DebugLog("INTERACTION MENU", "Callout Menu Item " + selecteditem.Text + " was selected!");
            if (!LSPD_First_Response.Mod.API.Functions.IsCalloutRunning())
            {
                GameFiber.StartNew(delegate
                {
                    LSPD_First_Response.Mod.API.Functions.StartCallout(selecteditem.Text);
                });
                Logger.DebugLog("INTERACTION MENU", "Starting Callout " + selecteditem.Text);
            }
            else
            {
                Game.DisplayNotification("~g~A Callout is already in progress");
                Logger.DebugLog("INTERACTION MENU", "Callout is already in progress");
            }
        }

        private static void OnAmbientEventsOptionsMenuItemSelected(UIMenu sender, UIMenuItem selcteditem, int index)
        {
        }

        private static void OnSettingsMenuItemSelected(UIMenu sender, UIMenuItem selecteditem, int index)
        {
            Logger.DebugLog("INTERACTION MENU", "Settings Item " + selecteditem.Text + " was selected!");
            if (selecteditem == _saveSettings)
            {
                Logger.DebugLog("INTERACTION MENU", "Forestry Callouts trying to save new settings");
                IniSettings.SaveNewSettings();
                _settingsMenu.RefreshIndex();
            }
            if (selecteditem == _reload)
            {
                IniSettings.LoadSettings();
                Game.DisplayNotification("~g~Forestry Callouts Settings Reloaded");
                GameFiber.Wait(1000);
                Logger.DebugLog("INTERACTION MENU", "Forestry Callouts Reloaded");
            }
        }

        internal static void CleanUp()
        {
            InteractionMenu.Clear();
            _optionsMenu.Clear();
            _settingsMenu.Clear();
            _calloutMenu.Clear();
        }
    }
}