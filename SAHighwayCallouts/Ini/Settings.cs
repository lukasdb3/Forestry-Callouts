using System;
using System.Linq;
using Rage;
using System.Windows.Forms;

namespace SAHighwayCallouts.Ini
{
    internal class Settings
    {
        #region iniVariables
        //bool shits
        internal static bool invalidKeys;
        
        //main
        internal static string calloutVersion;
        internal static bool EnableEndCalloutHelpMessages;

        internal static int WantedPedChooserMaxInt;
        internal static int DrunkPedChooserMaxInt;
        internal static int GunPedChooserMaxInt;

        //Keys
        internal static string DialogueKey;
        internal static Keys InputDialogueKey;
        internal static string EndCalloutKey;
        internal static Keys InputEndCalloutKey;
        internal static string InteractionKey;
        internal static Keys InputInteractionKey;
        
        //Vehicles
        internal static string LuxuryVehicles;
        internal static String[] luxuryVehiclesArray;
        
        //Callouts
        internal static bool DisableAllCallouts;
        internal static bool LuxuryVehiclePursuit;
        #endregion iniVariables

        internal static void LoadSettings()
        {
            Game.Console.Print("SAHighwayCallouts: loading settings!");
            string IniPath = "Plugins/LSPDFR/SAHighwayCallouts.ini";
            InitializationFile ini = new InitializationFile(IniPath);
            ini.Create();
            
            //Main stuff
            calloutVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            EnableEndCalloutHelpMessages = ini.ReadBoolean("Main", "EnableEndCalloutHelpMessages", true);

            WantedPedChooserMaxInt = ini.ReadInt32("Main", "MaxValueForWantedPed", 4);
            DrunkPedChooserMaxInt = ini.ReadInt32("Main", "MaxValueForDrunkPed", 4);
            GunPedChooserMaxInt = ini.ReadInt32("Main", "MaxValueForPedHavingGun", 4);
            
            //Key stuff
            DialogueKey = ini.ReadString("Keys", "DialogueKey", "Y");
            EndCalloutKey = ini.ReadString("Keys", "EndCalloutKey", "End");
            InteractionKey = ini.ReadString("Keys", "InteractionKey", "R");

            //Lets us convert strings into actual keys pog
            KeysConverter kc = new KeysConverter();
            //this tries to Convert each of the above strings into keys
            try
            {
                InputDialogueKey = (Keys)kc.ConvertFromString(DialogueKey);
                InputEndCalloutKey = (Keys)kc.ConvertFromString(EndCalloutKey);
                InputInteractionKey = (Keys)kc.ConvertFromString(InteractionKey);
            }
            //This catch, catches Invalid Keys and informs the user of the mistake.
            catch
            {
                invalidKeys = true;
                InputDialogueKey = Keys.Y;
                InputEndCalloutKey = Keys.End;
                InputInteractionKey = Keys.T;
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~h~SAHighway Callouts Warning",
                    "~r~Invalid Key Error",
                    "~b~Please check your ~y~SAHighwayCallouts.ini~w~ for ~y~Invalid Key Input~w~, see log for more details.");
            }

            //Vehicle shit
            LuxuryVehicles = ini.ReadString("Vehicles", "LuxuryVehicles", defaultValue: null);
            luxuryVehiclesArray = LuxuryVehicles.Split(':');
            Game.Console.Print("-!!- settings loaded!");
            
            //Callout shit
            DisableAllCallouts = ini.ReadBoolean("Callouts", "DisableAllCallouts", false);
            LuxuryVehiclePursuit = ini.ReadBoolean("Callouts", "LuxuryVehiclePursuit", true);
        }
    }
}