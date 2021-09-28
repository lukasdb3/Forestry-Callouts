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
        
        //Keys
        internal static string DialogueKey;
        internal static Keys InputDialogueKey;
        internal static string EndCalloutKey;
        internal static Keys InputEndCalloutKey;
        internal static string InteractionKey;
        internal static Keys InputInteractionKey;
        #endregion iniVariables

        internal static void LoadSettings()
        {
            string IniPath = "Plugins/LSPDFR/SAHighwayCallouts.ini";
            InitializationFile ini = new InitializationFile(IniPath);
            ini.Create();
            
            calloutVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            
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
        }
    }
}