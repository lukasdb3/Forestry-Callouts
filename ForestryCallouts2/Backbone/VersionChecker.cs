using System;
using System.Net;
using Rage;
using ForestryCallouts2.Backbone;
using ForestryCallouts2.Backbone.IniConfiguration;

namespace ForestryCallouts2.Backbone
{
    internal class VersionChecker
    {
        internal static bool updateAvailable;
        internal static string receivedData;
        internal static bool updateCheckFailed;
        
        internal static bool IsUpdateAvailable()
        {
            var curVersion = IniSettings.CurV;

            var latestVersionUri =
                new Uri(
                    "https://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=34663&textOnly=1");
            var webClient = new WebClient();

            try
            {
                receivedData = webClient
                    .DownloadString(
                        "https://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=34663&textOnly=1")
                    .Trim();
            }
            catch (WebException)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~g~Forestry Callouts 2 Warning",
                    "~g~Failed to check for an update",
                    "Please check if you are ~o~online~w~, or try to reload the plugin.");
                updateCheckFailed = true;
                // server or connection is having issues
            }

            if (receivedData != IniSettings.CurV)
            {
                updateAvailable = true;
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~g~Forestry Callouts 2 Warning",
                    "~g~A new Update is available!",
                    "Current Version: ~r~" + curVersion + "~w~<br>New Version: ~g~" + receivedData);
                return true;
            }

            return false;
        }
    }
}