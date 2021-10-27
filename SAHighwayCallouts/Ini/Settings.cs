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
        internal static bool AlwaysChooseStateAIPolice;
        internal static bool HelpBlips;

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
        
        //Peds
        //Police Peds
        internal static string PaletoBayPeds;
        internal static String[] PaletoBayPedsArray;
        
        internal static string BlaineCountyPeds;
        internal static String[] BlaineCountyPedsArray;
        
        internal static string LosSantosCountyPeds;
        internal static String[] LosSantosCountyPedsArray;
        
        internal static string LosSantosPeds;
        internal static String[] LosSantosPedsArray;
        
        internal static string HighwayStatePolicePeds;
        internal static String[] HighwayStatePolicePedsArray;
        
        //Vehicles
        //Cop vehicles
        internal static string PaletoBayVehicles;
        internal static String[] PaletoBayVehiclesArray;
        
        internal static string BlaineCountyVehicles;
        internal static String[] BlaineCountyVehiclesArray;
        
        internal static string LosSantosCountyVehicles;
        internal static String[] LosSantosCountyVehiclesArray;
        
        internal static string LosSantosVehicles;
        internal static String[] LosSantosVehiclesArray;
        
        internal static string HighwayStatePoliceVehicles;
        internal static String[] HighwayStatePoliceVehiclesArray;
        
        //Callout Use Vehicles
        internal static string TaxiVehicles;
        internal static String[] TaxiVehicleArray;
        

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
        internal static bool GrandTheftAuto;
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
            AlwaysChooseStateAIPolice = ini.ReadBoolean("Main", "AlwaysChooseStateAIPoliceBackup");
            HelpBlips = ini.ReadBoolean("Main", "EnableHelpBlips", false);

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
            //Peds
            //Police Peds
            PaletoBayPeds = ini.ReadString("PolicePeds", "PaletoBayPeds", null);
            PaletoBayPedsArray = PaletoBayPeds.Split(':');
            
            BlaineCountyPeds = ini.ReadString("PolicePeds", "BlaineCountyPeds", null);
            BlaineCountyPedsArray = BlaineCountyPeds.Split(':');
            
            LosSantosCountyPeds = ini.ReadString("PolicePeds", "LosSantosCountyPeds", null);
            LosSantosCountyPedsArray = LosSantosCountyPeds.Split(':');
            
            LosSantosPeds = ini.ReadString("PolicePeds", "LosSantosPeds", null);
            LosSantosPedsArray = LosSantosPeds.Split(':');
            
            HighwayStatePolicePeds = ini.ReadString("PolicePeds", "HighwayStatePolicePeds", null);
            HighwayStatePolicePedsArray = HighwayStatePolicePeds.Split(':');
            
            //Cop Vehicles
            PaletoBayVehicles = ini.ReadString("PoliceVehicles", "PaletoBayVehicles", null);
            PaletoBayVehiclesArray = PaletoBayVehicles.Split(':');
            
            BlaineCountyVehicles = ini.ReadString("PoliceVehicles", "BlaineCountyVehicles", null);
            BlaineCountyVehiclesArray = BlaineCountyVehicles.Split(':');
            
            LosSantosCountyVehicles = ini.ReadString("PoliceVehicles", "LosSantosCountyVehicles", null);
            LosSantosCountyVehiclesArray = LosSantosCountyVehicles.Split(':');
            
            LosSantosVehicles = ini.ReadString("PoliceVehicles", "LosSantosVehicles", null);
            LosSantosVehiclesArray = LosSantosVehicles.Split(':');

            HighwayStatePoliceVehicles = ini.ReadString("PoliceVehicles", "HighwayStatePoliceVehicles");
            HighwayStatePoliceVehiclesArray = HighwayStatePoliceVehicles.Split(':');
            
            //Callout Use Vehicles
            TaxiVehicles = ini.ReadString("TransportVehicles", "Taxis");
            TaxiVehicleArray = TaxiVehicles.Split(':');

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
            GrandTheftAuto = ini.ReadBoolean("Callouts", "GrandTheftAuto", true);
            Game.Console.Print("-!!- settings loaded!");
        }
    }
}