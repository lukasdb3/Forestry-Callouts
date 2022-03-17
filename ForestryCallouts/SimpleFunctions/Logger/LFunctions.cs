using Rage;

namespace ForestryCallouts.SimpleFunctions.Logger
{
    public class LFunctions
    {
        internal static void Log(LSPD_First_Response.Mod.Callouts.Callout c, string message)
        {
            Game.LogTrivial("-!!- ForestryCallouts - |" + c + "| - " + message + "-!!-");
        }
    }
}