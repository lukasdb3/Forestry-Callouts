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

            var latestVersionUri =
                new Uri(
                    "https://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=34663&textOnly=1");
            var webClient = new WebClient();

            try
            {
                ReceivedData = webClient
                    .DownloadString(
                        "https://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=34663&textOnly=1")
                    .Trim();
            }
            catch (WebException)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~g~FORESTRY CALLOUTS WARNING",
                    "~g~FAILED UPDATE CHECK", 
                    "Please check if you are ~o~online~w~, or try to reload the plugin.");
                // server or connection is having issues
            }

            if (ReceivedData != IniSettings.CurV)
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