using System;
using System.Net;
using Rage;
using SAHighwayCallouts.Ini;

namespace SAHighwayCallouts.Functions
{
    internal class VersionChecker
    {
        internal static bool updateAvailable;
        internal static string receivedData;
        internal static bool updateCheckFailed;
        internal static bool IsUpdateAvailable()
        {
            var curVersion = Settings.calloutVersion;

            var latestVersionUri =
                new Uri(
                    "https://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=34663&textOnly=1"); //Change this to SAHighwayCallouts link
            var webClient = new WebClient();

            try
            {
                receivedData = webClient
                    .DownloadString(
                        "https://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=34663&textOnly=1") //Change this to SAHighwayCallouts link
                    .Trim();
            }
            catch (WebException)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~h~SAHighwayCallouts Warning",
                    "~b~Failed to check for an update",
                    "Please check if you are ~o~online~w~, or try to reload the plugin.");
                updateCheckFailed = true;
                // server or connection is having issues
            }

            if (receivedData != Settings.calloutVersion)
            {
                updateAvailable = true;
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~h~SAHighwayCallouts Warning",
                    "~b~A new Update is available!",
                    "Current Version: ~r~" + curVersion + "~w~<br>New Version: ~g~" + receivedData);
                return true;
            }

            return false;
        }
    }
}
