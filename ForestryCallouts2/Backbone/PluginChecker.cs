using System;
using System.Linq;

namespace ForestryCallouts.SimpleFunctions
{
    internal static class CiPluginChecker
    {
        private static readonly Func<string, Version, bool> IsVersionLoaded = (plugin, version) =>
            LSPD_First_Response.Mod.API.Functions.GetAllUserPlugins().Any(x => x.GetName().Name.Equals(plugin) && x.GetName().Version.CompareTo(version) >= 0);

        public static readonly bool IsCalloutInterfaceRunning = IsVersionLoaded("CalloutInterface", new Version("1.2"));
    }

    internal static class UbPluginChecker
    {
        private static readonly Func<string, Version, bool> IsVersionLoaded = (plugin, version) =>
            LSPD_First_Response.Mod.API.Functions.GetAllUserPlugins().Any(x => x.GetName().Name.Equals(plugin) && x.GetName().Version.CompareTo(version) >= 0);

        public static readonly bool IsUltimateBackupRunning = IsVersionLoaded("UltimateBackup", new Version("1.8"));
    }
    
    internal static class StpPluginChecker
    {
        private static readonly Func<string, Version, bool> IsVersionLoaded = (plugin, version) =>
            LSPD_First_Response.Mod.API.Functions.GetAllUserPlugins().Any(x => x.GetName().Name.Equals(plugin) && x.GetName().Version.CompareTo(version) >= 0);

        public static readonly bool IsStopThePedRunning = IsVersionLoaded("StopThePed", new Version("4.9"));
    }
}