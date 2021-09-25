using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Mod.API;
using System.Reflection;
using System.Windows.Forms;
using ForestryCallouts.SimpleFunctions;
using RAGENativeUI.Elements;

namespace ForestryCallouts.Ini
{
    internal class IniSettings
    {
        #region iniVariables
        internal static string CalloutVersion;

        internal static bool invalidKeys;
        //Main
        internal static int SuspectViolentOption;
        internal static bool DeleteLoggerTruckOnEnd;
        internal static bool EnableEndCalloutHelpMessages;
        
        //Vehicles
        //RangerBackupModel for RangerBackup callout.
        internal static string RangerBackupModel1;
        internal static string RangerBackupModel2;
        internal static string RangerBackupModel3;
        internal static string RangerBackupModel4;
        internal static string RangerBackupModels;
        
        //Animal Control model.
        internal static string AnimalControlModel;
        
        //Keys
        internal static string DialogueKey;
        internal static Keys InputDialogueKey;
        internal static string EndCalloutKey;
        internal static Keys InputEndCalloutKey;
        internal static string InteractionKey;
        internal static Keys InputInteractionKey;

        //Callouts
        internal static bool DisableAllCallouts;
        internal static bool IntoxicatedHiker;
        internal static bool RecklessDriver;
        internal static bool InjuredHiker;
        internal static bool WreckedVehicle;
        internal static bool AnimalAttack;
        internal static bool SuspiciousVehicle;
        internal static bool LoggerPursuit;
        internal static bool HighSpeedPursuit;
        internal static bool VehicleOnFire;
        internal static bool MissingHiker;
        internal static bool IllegalCamping;
        internal static bool RangerRequestingBackup;
        internal static bool DeadAnimalBlockingRoad;
        internal static bool DangerousPerson;
        internal static bool IllegalHunting;
        internal static bool IllegalFishing;
        #endregion iniVariables

        internal static void LoadSettings()
        {
            Game.LogTrivial("-!!- Forestry Callouts - Trying to load settings! -!!-");

            string IniPath = "Plugins/LSPDFR/ForestryCallouts.ini";
            InitializationFile ini = new InitializationFile(IniPath);
            ini.Create();
            
            //Main shit
            CalloutVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            SuspectViolentOption = ini.ReadInt32("Main", "SuspectViolentOption");
            DeleteLoggerTruckOnEnd = ini.ReadBoolean("Main", "DeleteLoggerTruckOnEnd", true);
            EnableEndCalloutHelpMessages = ini.ReadBoolean("Main", "EnableEndCalloutHelpMessages", true);
            
            //Vehicles
            RangerBackupModel1 = ini.ReadString("Vehicles", "RangerBackupModel1", "PRANGER");
            RangerBackupModel2 = ini.ReadString("Vehicles", "RangerBackupModel2", "PRANGER");
            RangerBackupModel3 = ini.ReadString("Vehicles", "RangerBackupModel3", "PRANGER");
            RangerBackupModel4 = ini.ReadString("Vehicles", "RangerBackupModel4", "PRANGER");
            RangerBackupModels = ""+RangerBackupModel1.ToUpper()+", "+RangerBackupModel2.ToUpper()+", "+RangerBackupModel3.ToUpper()+", "+RangerBackupModel4.ToUpper()+"";

            AnimalControlModel = ini.ReadString("Vehicles", "AnimalControlModel", "REBEL2");

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
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~g~Forestry Callouts Warning",
                    "~r~Invalid Key Error",
                    "Please check your ~y~ForestryCallouts.ini~w~ for ~y~Invalid Key Input~w~, see log for more details.");
            }
            
            //Callout shit checkers readers things
            DisableAllCallouts = ini.ReadBoolean("Callouts", "DisableAllCallouts", false);
            IntoxicatedHiker = ini.ReadBoolean("Callouts", "Intoxicated Hiker", true);
            RecklessDriver = ini.ReadBoolean("Callouts", "RecklessDriver", true);
            InjuredHiker = ini.ReadBoolean("Callouts", "InjuredHiker", true);
            WreckedVehicle = ini.ReadBoolean("Callouts", "WreckedVehicle", true);
            AnimalAttack = ini.ReadBoolean("Callouts", "AnimalAttack", true);
            SuspiciousVehicle = ini.ReadBoolean("Callouts", "SuspiciousVehicle", true);
            LoggerPursuit = ini.ReadBoolean("Callouts", "LoggerPursuit", true);
            HighSpeedPursuit = ini.ReadBoolean("Callouts", "HighSpeedPursuit", true);
            VehicleOnFire = ini.ReadBoolean("Callouts", "VehicleOnFire", true);
            MissingHiker = ini.ReadBoolean("Callouts", "MissingHiker", true);
            IllegalCamping = ini.ReadBoolean("Callouts", "IllegalCamping", true);
            RangerRequestingBackup = ini.ReadBoolean("Callouts", "RangerRequestingBackup", true);
            DeadAnimalBlockingRoad = ini.ReadBoolean("Callouts", "DeadAnimalBlockingRoad", true);
            DangerousPerson = ini.ReadBoolean("Callouts", "DangerousPerson", true);
            IllegalHunting = ini.ReadBoolean("Callouts", "IllegalHunting", true);
            IllegalFishing = ini.ReadBoolean("Callouts", "IllegalFishing", true);
        }
    }
}

