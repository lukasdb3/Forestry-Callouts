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
        internal static bool PursuitBackup;

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
        //police vehicles
        internal static string PaletoBayCountyPoliceVehicles;
        internal static String[] PaletoBayCountyVehiclesArray;
        
        internal static string BlaineCountyPoliceVehicles;
        internal static String[] BlaineCountyVehiclesArray;
        
        internal static string LosSantosCountyPoliceVehicles;
        internal static String[] LosSantosCountyVehiclesArray;
        
        internal static string LosSantosCityPoliceVehicles;
        internal static String[] LosSantosCityVehiclesArray;
        
        //pursuit vehicles
        internal static string LuxuryVehicles;
        internal static String[] luxuryVehiclesArray;
        internal static int LuxuryVehicleAddons;
        
        internal static string NormalVehicles;
        internal static String[] NormalVehiclesArray;
        internal static int NormalVehiclesAddons;

        internal static string SemiTruckVehicles;
        internal static string[] SemiTruckVehiclesArray;

        internal static string SemiTrailerModels;
        internal static string[] SemiTrailerModelsArray;
        
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
            PursuitBackup = ini.ReadBoolean("Main", "PursuitBackup", true);

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

            //Police Vehicle shit
            PaletoBayCountyPoliceVehicles = ini.ReadString("PoliceVehicles", "PaletoBayCounty", null);
            PaletoBayCountyVehiclesArray = PaletoBayCountyPoliceVehicles.Split(':');
            
            BlaineCountyPoliceVehicles = ini.ReadString("PoliceVehicles", "BlaineCounty", null);
            BlaineCountyVehiclesArray = PaletoBayCountyPoliceVehicles.Split(':');
            
            LosSantosCountyPoliceVehicles = ini.ReadString("PoliceVehicles", "LosSantosCounty", null);
            LosSantosCountyVehiclesArray = PaletoBayCountyPoliceVehicles.Split(':');

            LosSantosCityPoliceVehicles = ini.ReadString("PoliceVehicles", "LosSantosCity", null);
            LosSantosCityVehiclesArray = LosSantosCityPoliceVehicles.Split(':');
            
            //Pursuit Vehicle shit
            NormalVehicles = ini.ReadString("Vehicles", "NormalVehicles", null);
            NormalVehiclesArray = NormalVehicles.Split(':');
            if (NormalVehiclesArray.Length > 34)
            {
                //Player added addon vehicles, why do we care, we don't I just added this cuz its pointless but cool ig idk.
                NormalVehiclesAddons = NormalVehiclesArray.Length - 34; //amount of addon cars :)
            }
            else
            {
                NormalVehiclesAddons = 0; //No addons
            }
            
            LuxuryVehicles = ini.ReadString("Vehicles", "LuxuryVehicles", defaultValue: null);
            luxuryVehiclesArray = LuxuryVehicles.Split(':');
            if (luxuryVehiclesArray.Length > 45)
            {
                //Player added addon vehicles, why do we care, we don't I just added this cuz its pointless but cool ig idk.
                LuxuryVehicleAddons = luxuryVehiclesArray.Length - 45; //amount of addon cars :)
            }
            else
            {
                LuxuryVehicleAddons = 0; //No addons homie
            }
            
            SemiTruckVehicles = ini.ReadString("Vehicles", "SemiTruckVehicles", null);
            SemiTruckVehiclesArray = SemiTruckVehicles.Split(':');

            SemiTrailerModels = ini.ReadString("Vehicles", "SemiTrailerModels", null);
            SemiTrailerModelsArray = SemiTrailerModels.Split(':');
            

            //Callout shit
            DisableAllCallouts = ini.ReadBoolean("Callouts", "DisableAllCallouts", false);
            LuxuryVehiclePursuit = ini.ReadBoolean("Callouts", "VehiclePursuit", true);
            Game.Console.Print("-!!- settings loaded!");
        }
    }
}