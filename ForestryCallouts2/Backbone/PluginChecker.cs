using System;
using System.Linq;

namespace ForestryCallouts2.Backbone
{
    internal static class PluginChecker
    {
        private static readonly Func<string, Version, bool> IsVersionLoaded = (plugin, version) => 
            LSPD_First_Response.Mod.API.Functions.GetAllUserPlugins().Any(x => x.GetName().Name.Equals(plugin) && x.GetName().Version.CompareTo(version) >= 0);

        public static readonly bool ForestryCallouts = IsVersionLoaded("ForestryCallouts", new Version("1.6"));
        public static readonly bool CalloutInterface = IsVersionLoaded("CalloutInterface", new Version("1.2"));
        public static readonly bool UltimateBackup = IsVersionLoaded("UltimateBackup", new Version("1.8"));
        public static readonly bool StopThePed = IsVersionLoaded("StopThePed", new Version("4.9"));
    }
}