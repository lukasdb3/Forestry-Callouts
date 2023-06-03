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
using ForestryCallouts2.AmbientEvents;
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
        internal static UIMenu EventsMenu = new("", "");

        // Interaction Menu
        private static UIMenuItem _callAnimalControl;
        private static UIMenuItem _endCallout;
        private static UIMenuItem _endEvent;
        private static UIMenuItem _options;

        // Options Menu
        private static UIMenuItem _startCallout;
        private static UIMenuItem _settings;
        private static UIMenuItem _ambientEvents;
        
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
        //AmbientEvents
        internal static UIMenuListItem AmbientEventsEnabled;
        internal static UIMenuNumericScrollerItem<int> MinimumWaitTimeBetweenEvents;
        internal static UIMenuNumericScrollerItem<int> MaximumWaitTimeBetweenEvents;
        //Callouts
        internal static UIMenuListItem IntoxicatedPerson;
        internal static UIMenuListItem RegularPursuit;
        internal static UIMenuListItem AnimalAttack;
        internal static UIMenuListItem DirtBikePursuit;
        internal static UIMenuListItem AtvPursuit;


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
            _endEvent = new("End Event", "End Current Event");
            _options = new("Options", "");
            InteractionMenu.AddItems(_callAnimalControl,_endCallout,_endEvent,_options);
            InteractionMenu.RefreshIndex();
            InteractionMenu.OnItemSelect += OnInteractionMenuItemSelected;
            Main.pool.Add(InteractionMenu);
            
            // Create Options Menu
            _optionsMenu = new("Forestry Callouts", "~b~Options Menu ~g~| ~y~v" + Version);
            _optionsMenu.SetBannerType(Color.ForestGreen);
            _optionsMenu.MouseControlsEnabled = false;

            _startCallout = new("Callouts", "");
            _ambientEvents = new("Ambient Events", "");
            _settings = new("Settings", "");
            _optionsMenu.AddItems(_startCallout, _ambientEvents, _settings);
            InteractionMenu.BindMenuToItem(_optionsMenu, _options);
            _optionsMenu.RefreshIndex();
            _optionsMenu.OnItemSelect += OnOptionsMenuItemSelected;
            Main.pool.Add(_optionsMenu);

            // Create Callout Menu
            _calloutMenu = new("Forestry Callouts", "~b~Callout Menu ~g~| ~y~v" + Version);
            _calloutMenu.SetBannerType(Color.ForestGreen);
            _calloutMenu.MouseControlsEnabled = false;

            foreach (var callout in CalloutsGetter.ForestryCalloutsCalls)
            {
                if (IniSettings.Ini.ReadBoolean("Callouts", callout)) _calloutMenu.AddItem(new UIMenuItem(callout));
            }
            
            _optionsMenu.BindMenuToItem(_calloutMenu, _startCallout);
            _calloutMenu.RefreshIndex();
            _calloutMenu.OnItemSelect += OnCalloutMenuItemSelected;
            Main.pool.Add(_calloutMenu);

            //Create events menu
            EventsMenu = new("Forestry Callouts", "~b~Ambient Events Menu ~g~| ~y~v" + Version);
            EventsMenu.SetBannerType(Color.ForestGreen);
            EventsMenu.MouseControlsEnabled = false;
            
            //We add the events to the menu in AmbientEvents.Main.RegisterEvent()
            
            _optionsMenu.BindMenuToItem(EventsMenu, _ambientEvents);
            EventsMenu.RefreshIndex();
            EventsMenu.OnItemSelect += OnEventMenuItemSelected;
            Main.pool.Add(EventsMenu);
            
            // Create Settings Menu
            _settingsMenu = new UIMenu("Forestry Callouts", "~b~Settings Menu ~g~| ~y~v" + Version);
            _settingsMenu.SetBannerType(Color.ForestGreen);
            _settingsMenu.MouseControlsEnabled = false;
            //Main
            DebugLogs = new UIMenuListItem("DebugsLogs", "For Debugging ForestryCallouts Crashes", IniSettings.DebugLogs.ToString().ToLower(), (IniSettings.DebugLogs) ? "false" : "true");
            WaterCallouts = new UIMenuListItem("WaterCallouts", "Enabling Water Callouts will Disable Land Callouts", IniSettings.WaterCalls.ToString().ToLower(), (IniSettings.WaterCalls) ? "false" : "true");
            SearchAreaBlipsMax = new UIMenuNumericScrollerItem<int>("SearchAreaBlipsMax", "Amount of Search Areas sent out before object is Blipped", 5, 15, 1);
            SearchAreaBlipsMax.Value = IniSettings.SearchAreaNotifications;
            EnableDistanceChecker = new UIMenuListItem("EnableDistanceChecker", "If true Distance Checker is enabled", IniSettings.EnableDistanceChecker, (IniSettings.EnableDistanceChecker) ? "false" : "true");
            MaxDistance = new UIMenuNumericScrollerItem<double>("MaxDistance", "Max Distance a Callout will spawn away from you in meters", 1000, 10000, 100);
            MaxDistance.Value = IniSettings.MaxDistance;
            MinCalloutDistance = new UIMenuNumericScrollerItem<int>("MinimumCalloutSpawnDistance", "Minimum distance a callout can spawn from you", 30, 300, 1);
            MinCalloutDistance.Value = IniSettings.MinCalloutDistance;
            //Binoculars
            EnableBinoculars = new UIMenuListItem("EnableBinoculars", "If true Binoculars are Enabled", IniSettings.BinocularsEnabled.ToString().ToLower(), (IniSettings.BinocularsEnabled) ? "false" : "true");
            BinocularsSense = new UIMenuNumericScrollerItem<int>("BinocularsSensitivity", "How fast the Binoculars move left and right",1, 10, 1);
            BinocularsSense.Value = IniSettings.BinocularsSensitivity;
            //AmbientEvents
            AmbientEventsEnabled = new UIMenuListItem("AmbientEventsEnabled", "If False Ambient Events Are Disabled", IniSettings.AmbientEventsEnabled.ToString().ToLower(), (IniSettings.AmbientEventsEnabled) ? "false" : "true");
            MinimumWaitTimeBetweenEvents = new UIMenuNumericScrollerItem<int>("MinWaitTimeEvents", "Minimum Wait Time Between Events", 3, IniSettings.MaximumWaitTime - 1, 1);
            MinimumWaitTimeBetweenEvents.Value = IniSettings.MinimumWaitTime;
            MaximumWaitTimeBetweenEvents = new UIMenuNumericScrollerItem<int>("MaxWaitTimeEvents", "Maximum Wait Time Between Events", IniSettings.MinimumWaitTime + 1, 20, 1);
            MaximumWaitTimeBetweenEvents.Value = IniSettings.MaximumWaitTime;
            //Callouts
            IntoxicatedPerson = new UIMenuListItem("IntoxicatedPerson", "",IniSettings.IntoxPerson.ToString().ToLower(), (IniSettings.IntoxPerson) ? "false" : "true");
            RegularPursuit = new UIMenuListItem("RegularPursuit", "",IniSettings.RegularPursuit.ToString().ToLower(), (IniSettings.RegularPursuit) ? "false" : "true");
            AnimalAttack = new UIMenuListItem("AnimalAttack", "",IniSettings.AnimalAttack.ToString().ToLower(), (IniSettings.AnimalAttack) ? "false" : "true");
            DirtBikePursuit = new UIMenuListItem("DirtBikePursuit", "",IniSettings.DirtBikePursuit.ToString().ToLower(), (IniSettings.DirtBikePursuit) ? "false" : "true");
            AtvPursuit = new UIMenuListItem("AtvPursuit", "", IniSettings.AtvPursuit.ToString().ToLower(), (IniSettings.AtvPursuit) ? "false" : "true");
            //Buttons for saving and reloading Ini
            _saveSettings = new UIMenuItem("~g~Save Settings", "~r~Required To Press If Settings Were Just Changed");
            _reload = new UIMenuItem("~b~Reload", "Reloads Forestry Callouts Settings and AmbientEvents");
            
            _settingsMenu.AddItems(DebugLogs, WaterCallouts, SearchAreaBlipsMax, MaxDistance, MinCalloutDistance, EnableBinoculars, BinocularsSense, AmbientEventsEnabled, MinimumWaitTimeBetweenEvents, MaximumWaitTimeBetweenEvents, IntoxicatedPerson, RegularPursuit, AnimalAttack, DirtBikePursuit, AtvPursuit, _saveSettings, _reload);
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

            if (selecteditem == _endEvent)
            {
                if (AmbientEvents.Main.IsAnyEventRunning)
                {
                    AmbientEvent currentEvent = AmbientEvents.Main.currentEvent;
                    currentEvent.End();
                }
                else
                {
                    Logger.DebugLog("INTERACTION MENU", "There is no active event");
                    Game.DisplayNotification("~g~There Is No Active Event");
                }
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
        
        private static void OnEventMenuItemSelected(UIMenu sender, UIMenuItem selecteditem, int index)
        {
            Logger.DebugLog("INTERACTION MENU", "Ambient Event Item " + selecteditem.Text + " was selected!");
            foreach (var ae in AmbientEvents.Main.EventNamesList)
            {
                if (ae == selecteditem.Text)
                {
                    AmbientEvents.Main.StartEvent(ae);
                }
            }
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
                AmbientEvents.Main.CleanUp();
                Game.DisplayNotification("~g~Forestry Callouts Events Cleaned Up");
                GameFiber.Wait(1000);
                if (IniSettings.AmbientEventsEnabled)
                {
                    Game.DisplayNotification("~g~Forestry Callouts Events Reloaded");
                    AmbientEvents.Main.RegisterEvents();
                }
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