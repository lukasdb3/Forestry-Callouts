using System;
using Rage;

namespace ForestryCallouts2.Backbone.SpawnSystem.Land.CalloutSpawnpoints
{
    internal static class Common
    {
        internal static void nPaletoBayForest(out Vector3 spawnpoint, out float heading)
        {
            Logger.DebugLog("COMMON SPAWNPOINTS", "Choosing spawnpoint in North Paleto Forest chunk");
            spawnpoint = default;
            heading = 0f;
            int var = new Random().Next(1, 2);
            Logger.DebugLog("CASE", ""+var+"");
            switch (var)
            {
                case 1:
                    spawnpoint = new Vector3(-592.945f, 5460.914f, 59.309f);
                    heading = 81.305f;
                    break;
            }
        }
    }
}