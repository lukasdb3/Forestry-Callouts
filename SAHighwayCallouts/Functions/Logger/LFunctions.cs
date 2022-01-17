using System;
using Rage;
using LSPD_First_Response.Mod.API;
using System.Reflection;
using LSPD_First_Response.Mod.Callouts;
using SAHighwayCallouts.Ini;
using SAHighwayCallouts.Functions;

namespace SAHighwayCallouts.Functions.Logger
{
    internal class LFunctions
    {
        internal static void BasicLogger(string c, string message)
        {
            Game.LogTrivial("-!!- SAHighwayCallouts - |" + c + "| - " + message + "-!!-");
        }
    }
}