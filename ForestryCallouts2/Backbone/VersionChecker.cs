#region Refrences
//System
using System;
using System.Net;
//Rage
using Rage;
//ForestryCallouts2
using ForestryCallouts2.Backbone.IniConfiguration;
#endregion

namespace ForestryCallouts2.Backbone
{
    internal static class VersionChecker
    {
        internal static string ReceivedData;

        internal static bool IsUpdateAvailable()
        {
            var curVersion = IniSettings.CurV;

            try
            {
                var latestVersionUri =
                    new Uri(
                        "https://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=34663&textOnly=1");
                var webClient = new WebClient();
                
                ReceivedData = webClient
                    .DownloadString(
                        "https://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=34663&textOnly=1")
                    .Trim();
            }
            catch (Exception e)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~g~FORESTRY CALLOUTS WARNING",
                    "~g~FAILED UPDATE CHECK", 
                    "Please check if you are ~o~online~w~, or try to reload the plugin.");
                // server or connection is having issues
            }

            var currentVersion = new Version(curVersion);
            var newestVersion = new Version(ReceivedData);
            var result = currentVersion.CompareTo(newestVersion);
            if (result > 0)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~g~FORESTRY CALLOUTS WARNING",
                    "~g~BETA VERSION INSTALLED",
                    "Current Version: ~g~" + curVersion);
            }
            if (result < 0)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~g~FORESTRY CALLOUTS WARNING",
                    "~g~NEW UPDATE AVAILABLE!",
                    "Current Version: ~r~" + curVersion + "~w~<br>New Version: ~g~" + ReceivedData);
                return true;
            }
            return false;
        }
    }
}