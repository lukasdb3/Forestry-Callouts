#region Refrences
//System
using System.Drawing;
using System.Threading;
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
    public static class MainMenu
    {
        // Version
        private static readonly string Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        
        // Menus
        internal static UIMenu InteractionMenu = new("", "");
        private static UIMenu _settingsMenu = new("", "");

        // Interaction Menu
        private static UIMenuItem _callAnimalControl;
        private static UIMenuItem _endCallout;
        private static UIMenuItem _settings;

        // Settings Menu
        //Main
        internal static UIMenuListItem EndNotfiMessages;
        internal static UIMenuListItem WaterCallouts;
        internal static UIMenuNumericScrollerItem<int> SearchAreaBlipsMax;
        internal static UIMenuListItem EnableDistanceChecker;
        internal static UIMenuNumericScrollerItem<double> MaxDistance;
        internal static UIMenuNumericScrollerItem<int> MinCalloutDistance;
        internal static UIMenuListItem AICops;
        //Binoculars
        internal static UIMenuListItem EnableBinoculars;
        internal static UIMenuNumericScrollerItem<int> BinocularsSense;
        internal static UIMenuNumericScrollerItem<int> BinocularsImage;
        //Callouts
        internal static UIMenuListItem AnimalAttack;
        internal static UIMenuListItem AtvPursuit;
        internal static UIMenuListItem DangerousPerson;
        internal static UIMenuListItem DeadAnimalOnRoadway;
        internal static UIMenuListItem AnimalOnRoadway;
        internal static UIMenuListItem DirtBikePursuit;
        internal static UIMenuListItem HighSpeedPursuit;
        internal static UIMenuListItem IntoxicatedPerson;
        internal static UIMenuListItem LoggerTruckPursuit;
        internal static UIMenuListItem RegularPursuit;
        
        private static UIMenuItem _saveSettings;

        internal static void Initialize()
        {
            // Create Interaction Menu
            InteractionMenu = new("Forestry Callouts", "~b~Interaction Menu ~g~| ~y~v" + Version);
            InteractionMenu.SetBannerType(Color.ForestGreen);
            InteractionMenu.MouseControlsEnabled = false;
            
            _callAnimalControl = new("Animal Control", "Call Animal Control To Pick Up Dead Animal");
            _endCallout = new("End Callout", "End Current Callout");
            _settings = new("Settings", "Forestry Callouts Ini Settings");
            InteractionMenu.AddItems(_callAnimalControl,_endCallout,_settings);
            InteractionMenu.RefreshIndex();
            InteractionMenu.OnItemSelect += OnInteractionMenuItemSelected;
            Main.Pool.Add(InteractionMenu);

            // Create Settings Menu
            _settingsMenu = new UIMenu("Forestry Callouts", "~b~Settings Menu ~g~| ~y~v" + Version);
            _settingsMenu.SetBannerType(Color.ForestGreen);
            _settingsMenu.MouseControlsEnabled = false;
            //Main
            EndNotfiMessages = new UIMenuListItem("CalloutEndMessages", "Disables And Enables End Callout Notifications", IniSettings.EndNotfiMessages.ToString().ToLower(), (IniSettings.EndNotfiMessages) ? "false" : "true");
            WaterCallouts = new UIMenuListItem("WaterCallouts", "Disables And Enables Water Callouts", IniSettings.WaterCallouts.ToString().ToLower(), (IniSettings.WaterCallouts) ? "false" : "true");
            SearchAreaBlipsMax = new UIMenuNumericScrollerItem<int>("SearchAreaBlipsMax", "Amount of Search Areas Sent Before Object is Blipped", 5, 15, 1);
            SearchAreaBlipsMax.Value = IniSettings.SearchAreaNotifications;
            EnableDistanceChecker = new UIMenuListItem("EnableDistanceChecker", "Disables And Enables Distance Checker", IniSettings.EnableDistanceChecker.ToString().ToLower(), (IniSettings.EnableDistanceChecker) ? "false" : "true");
            MaxDistance = new UIMenuNumericScrollerItem<double>("MaxDistance", "The Max Distance A Callout Will Spawn Away From You (meters)", 1000, 10000, 100);
            MaxDistance.Value = IniSettings.MaxDistance;
            MinCalloutDistance = new UIMenuNumericScrollerItem<int>("MinimumCalloutSpawnDistance", "The Minimum Distance A Callout Will Spawn Away From You (meters)", 30, 300, 1);
            MinCalloutDistance.Value = IniSettings.MinCalloutDistance;
            AICops = new UIMenuListItem("AICops", "Disables And Enables AI Cops Spawning For Pursuits", IniSettings.AICops.ToString().ToLower(), (IniSettings.AICops) ? "false" : "true");
            //Binoculars
            EnableBinoculars = new UIMenuListItem("EnableBinoculars", "Disables And Enables Binoculars", IniSettings.BinocularsEnabled.ToString().ToLower(), (IniSettings.BinocularsEnabled) ? "false" : "true");
            BinocularsSense = new UIMenuNumericScrollerItem<int>("BinocularsSensitivity", "Binoculars Horizontal Sensitivity",1, 10, 1);
            BinocularsSense.Value = IniSettings.BinocularsSensitivity;
            BinocularsImage = new UIMenuNumericScrollerItem<int>("BinocularsImage", "Chooses The Texture Binoculars Uses", 1, 6, 1);
            BinocularsImage.Value = int.Parse(IniSettings.BinocularsImage);
            //Callouts
            AnimalAttack = new UIMenuListItem("AnimalAttack", "",IniSettings.AnimalAttack.ToString().ToLower(), (IniSettings.AnimalAttack) ? "false" : "true");
            AtvPursuit = new UIMenuListItem("AtvPursuit", "", IniSettings.AtvPursuit.ToString().ToLower(), (IniSettings.AtvPursuit) ? "false" : "true");
            DangerousPerson = new UIMenuListItem("DangerousPerson", "", IniSettings.DangerousPerson.ToString().ToLower(), (IniSettings.DangerousPerson) ? "false" : "true");
            DeadAnimalOnRoadway = new UIMenuListItem("DeadAnimalRoadway", "", IniSettings.DeadAnimalOnRoadway.ToString().ToLower(), (IniSettings.DeadAnimalOnRoadway) ? "false" : "true");
            AnimalOnRoadway = new UIMenuListItem("AnimalOnRoadway", "", IniSettings.AnimalOnRoadway.ToString().ToLower(), (IniSettings.AnimalOnRoadway) ? "false" : "true");
            DirtBikePursuit = new UIMenuListItem("DirtBikePursuit", "",IniSettings.DirtBikePursuit.ToString().ToLower(), (IniSettings.DirtBikePursuit) ? "false" : "true");
            HighSpeedPursuit = new UIMenuListItem("HighSpeedPursuit", "", IniSettings.HighSpeedPursuit.ToString().ToLower(), (IniSettings.HighSpeedPursuit) ? "false" : "true");
            IntoxicatedPerson = new UIMenuListItem("IntoxicatedPerson", "",IniSettings.IntoxPerson.ToString().ToLower(), (IniSettings.IntoxPerson) ? "false" : "true");
            LoggerTruckPursuit = new UIMenuListItem("LoggerTruckPursuit", "", IniSettings.LoggerTruckPursuit.ToString().ToLower(), (IniSettings.LoggerTruckPursuit) ? "false" : "true");
            RegularPursuit = new UIMenuListItem("Pursuit", "",IniSettings.RegularPursuit.ToString().ToLower(), (IniSettings.RegularPursuit) ? "false" : "true");
            //Buttons for saving and reloading Ini
            _saveSettings = new UIMenuItem("~g~Save Settings", "~r~Required To Do If Settings Were Just Changed");
            _settingsMenu.AddItems(EndNotfiMessages ,SearchAreaBlipsMax, EnableDistanceChecker ,MaxDistance, MinCalloutDistance, AICops, EnableBinoculars, BinocularsSense, BinocularsImage, AnimalAttack, AtvPursuit, DangerousPerson, DeadAnimalOnRoadway,
                AnimalOnRoadway, DirtBikePursuit, HighSpeedPursuit, IntoxicatedPerson, LoggerTruckPursuit, RegularPursuit, _saveSettings);
            InteractionMenu.BindMenuToItem(_settingsMenu, _settings);
            _settingsMenu.RefreshIndex();
            _settingsMenu.OnItemSelect += OnSettingsMenuItemSelected;
            Main.Pool.Add(_settingsMenu);
        }

        private static void OnInteractionMenuItemSelected(UIMenu sender, UIMenuItem selecteditem, int index)
        {
            Log.Debug("INTERACTION MENU", "Item " + selecteditem.Text + " was selected!");
            if (selecteditem == _callAnimalControl) AnimalControl.CallAnimalControl();

            if (selecteditem == _endCallout)
            {
                if (LSPD_First_Response.Mod.API.Functions.IsCalloutRunning())
                {
                    LSPD_First_Response.Mod.API.Functions.StopCurrentCallout();
                    Log.Debug("INTERACTION MENU", "Ended current callout");
                }
                else
                {
                    Game.DisplayNotification("~g~There Is No Callout Running");
                    Log.Debug("INTERACTION MENU", "There is no callout to end");   
                }
            }
        }

        private static void OnSettingsMenuItemSelected(UIMenu sender, UIMenuItem selecteditem, int index)
        {
            Log.Debug("INTERACTION MENU", "Saving and reloading settings!");
            if (selecteditem == _saveSettings)
            {
                IniSettings.SaveNewSettings();
                IniSettings.LoadSettings();
                _settingsMenu.RefreshIndex();
                Game.DisplayNotification("~g~It is recommended to reload LSPDFR after changing INI options, some settings require a reload!");
            }
        }

        internal static void CleanUp()
        {
            InteractionMenu.Clear();
            _settingsMenu.Clear();
        }
    }
}