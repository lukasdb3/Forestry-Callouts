using Rage;
using System.Windows.Forms;

namespace HighwayCallouts.Ini
{
    internal class Settings
    {
        #region iniVariables
        internal static string calloutVersion;

        #endregion iniVariables

        internal static void LoadSettings()
        {
            string IniPath = "Plugins/LSPDFR/SAHighwayCallouts.ini";
            InitializationFile ini = new InitializationFile(IniPath);
            ini.Create();
            
            calloutVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}