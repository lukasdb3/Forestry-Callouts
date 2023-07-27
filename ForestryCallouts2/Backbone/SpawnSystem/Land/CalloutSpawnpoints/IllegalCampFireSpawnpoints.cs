#region Refrences
//System
using System;
//Rage
using Rage;
#endregion


namespace ForestryCallouts2.Backbone.SpawnSystem.Land.CalloutSpawnpoints
{
    internal static class IllegalCampFireSpawnpoints
    {                                                                                                                                                       
        internal static void PaletoBayForest(out Vector3 sSpawn, out float sHeading, out Vector3 fSpawn)
        {
            Logger.DebugLog("ILLEGAL CAMP FIRE SPAWNPOINT CHOOSER", "Choosing spawnpoint in PaletoBayForest chunk");
            sSpawn = default;
            sHeading = 0f;
            fSpawn = default;
            var var = new Random().Next(1, 3);
            Logger.DebugLog("CASE", ""+var+"");
            switch (var)
            {
                case 1:
                    sSpawn = new Vector3();
                    sHeading = 0f;
                    fSpawn = new Vector3();
                    break;
            }
        }

        internal static void AltruistCampArea(out Vector3 sSpawn, out float sHeading, out Vector3 fSpawn)
        {
            Logger.DebugLog("ILLEGAL CAMP FIRE SPAWNPOINT CHOOSER", "Choosing spawnpoint in AltruistCampArea chunk");
            sSpawn = default;
            sHeading = 0f;
            fSpawn = default;
            var var = new Random().Next(1, 3);
            Logger.DebugLog("CASE", ""+var+"");
            switch (var)
            {
                case 1:
                    sSpawn = new Vector3();
                    sHeading = 0f;
                    fSpawn = new Vector3();
                    break;
            }
        }
        
        internal static void RatonCanyon(out Vector3 sSpawn, out float sHeading, out Vector3 fSpawn)
        {
            Logger.DebugLog("ILLEGAL CAMP FIRE SPAWNPOINT CHOOSER", "Choosing spawnpoint in RatonCanyon chunk");
            sSpawn = default;
            sHeading = 0f;
            fSpawn = default;
            var var = new Random().Next(1, 3);
            Logger.DebugLog("CASE", ""+var+"");
            switch (var)
            {
                case 1:
                    sSpawn = new Vector3();
                    sHeading = 0f;
                    fSpawn = new Vector3();
                    break;
            }
        }
    }
}