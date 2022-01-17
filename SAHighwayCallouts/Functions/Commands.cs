using System;
using Rage;
using LSPD_First_Response.Mod.API;
using System.Reflection;
using LSPD_First_Response.Mod.Callouts;
using SAHighwayCallouts.Ini;
using SAHighwayCallouts.Functions;
using SAHighwayCallouts.Functions.Logger;

namespace SAHighwayCallouts.Functions
{
    public class Commands
    {
        [Rage.Attributes.ConsoleCommand]
        public static void Command_SAHCReloadIni()
        {
            Settings.LoadSettings();
        }

        [Rage.Attributes.ConsoleCommand]
        public static void Command_ENDCallout()
        {
            if (LSPD_First_Response.Mod.API.Functions.IsCalloutRunning())
            {
                LSPD_First_Response.Mod.API.Functions.StopCurrentCallout();
                LFunctions.BasicLogger("Commands","The current callout was ended.");
            }
            else
            {
                LFunctions.BasicLogger("Commands","There is no callout to end.");            }
        }
    }
}