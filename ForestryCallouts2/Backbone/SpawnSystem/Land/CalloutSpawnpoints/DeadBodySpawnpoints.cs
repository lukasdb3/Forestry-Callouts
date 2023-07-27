#region Refrences
//System
using System;
//Rage
using Rage;
#endregion


namespace ForestryCallouts2.Backbone.SpawnSystem.Land.CalloutSpawnpoints
{
    internal static class DeadBodySpawnpoints
    {                                                                                                                                                       
        internal static void PaletoBayForest(out Vector3 vSpawn, out Vector3 rSpawn, out float heading)
        {
            Logger.DebugLog("DEAD BODY SPAWNPOINT CHOOSER", "Choosing spawnpoint in PaletoBayForest chunk");
            vSpawn = default;
            rSpawn = default;
            heading = 0f;
            var var = new Random().Next(1, 3);
            Logger.DebugLog("CASE", ""+var+"");
            switch (var)
            {
                case 1:
                    vSpawn = new Vector3();
                    rSpawn = new Vector3();
                    heading = 0f;
                    break;
            }
        }

        internal static void AltruistCampArea(out Vector3 vSpawn, out Vector3 rSpawn, out float heading)
        {
            Logger.DebugLog("DEAD BODY SPAWNPOINT CHOOSER", "Choosing spawnpoint in AltruistCampArea chunk");
            vSpawn = default;
            rSpawn = default;
            heading = 0f;
            var var = new Random().Next(1, 2);
            Logger.DebugLog("CASE", ""+var+"");
            switch (var)
            {
                case 1:
                    vSpawn = new Vector3();
                    rSpawn = new Vector3();
                    heading = 0f;
                    break;
            }
        }
        
        internal static void RatonCanyon(out Vector3 vSpawn, out Vector3 rSpawn, out float heading)
        {
            Logger.DebugLog("DEAD BODY SPAWNPOINT CHOOSER", "Choosing spawnpoint in RatonCanyon chunk");
            vSpawn = default;
            rSpawn = default;
            heading = 0f;
            var var = new Random().Next(1, 13);
            Logger.DebugLog("CASE", ""+var+"");
            switch (var)
            {
                case 1:
                    vSpawn = new Vector3();
                    rSpawn = new Vector3();
                    heading = 0f;
                    break;
            }
        }
    }
}