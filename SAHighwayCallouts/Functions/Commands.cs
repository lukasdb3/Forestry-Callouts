using System;
using Rage;
using LSPD_First_Response.Mod.API;
using System.Reflection;
using SAHighwayCallouts.Ini;
using SAHighwayCallouts.Functions;

namespace SAHighwayCallouts.Functions
{
    public class Commands
    {
        [Rage.Attributes.ConsoleCommand]
        public static void Command_SAHCReloadIni()
        {
            Settings.LoadSettings();
        }
    }
}