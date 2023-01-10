using Rage;
using System;
using ForestryCallouts2.Backbone.IniConfiguration;

namespace ForestryCallouts2.Backbone
{
    public class CCommands
    {
        [Rage.Attributes.ConsoleCommand]
        public static void FC_ReloadINI()
        {
            IniSettings.LoadSettings();
            Logger.Log("[Commands] - Reloading ForestryCallouts2.ini");
        }
        
        [Rage.Attributes.ConsoleCommand]
        public static void FC_EndCallout()
        {
            if (LSPD_First_Response.Mod.API.Functions.IsCalloutRunning())
            {
                LSPD_First_Response.Mod.API.Functions.StopCurrentCallout();
                Logger.Log("[Commands] - The current callout was ended.");
            }
            else
            {
                Logger.Log("[Commands] - There is no callout to end.");            }
        }
    }
    
}