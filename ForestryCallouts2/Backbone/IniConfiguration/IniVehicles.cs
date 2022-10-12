using System;
using Rage;

namespace ForestryCallouts2.Backbone.IniConfiguration
{
    public static class IniVehicles
    {
        #region Variabales
        internal static string NormalVehicles;
        internal static String[] NormalVehiclesArray;
        #endregion

        internal static void LoadVehicleListConfigs()
        {
            string IniPath = "Plugins/LSPDFR/ForestryCallouts2/vehicleconfig.ini";
            InitializationFile ini = new InitializationFile(IniPath);
            ini.Create();
            
            NormalVehicles = ini.ReadString("Vehicles", "NormalVehicles", null);
            NormalVehiclesArray = NormalVehicles.Split(':');
        }
    }
}