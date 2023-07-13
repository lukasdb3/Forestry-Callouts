#region Refrences
//System
using System;
//Rage
using Rage;
#endregion


namespace ForestryCallouts2.Backbone.SpawnSystem.Land.CalloutSpawnpoints
{
    internal static class LoggerTruckSpawns
    {
        internal static void PaletoBayForest(out Vector3 spawnpoint, out float heading)
        {
            Logger.DebugLog("LOGGERTRUCK SPAWNPOINT CHOOSER", "Choosing spawnpoint in Paleto Forest chunk");
            spawnpoint = default;
            heading = 0f;
            int var = new Random().Next(1, 10);
            Logger.DebugLog("CASE", ""+var+"");
            switch (var)
            {
                case 1:
                    spawnpoint = new Vector3(-679.1430f, 5537.0957f, 37.6965f);
                    heading = 301.4893f;
                    break;
                case 2:
                    spawnpoint = new Vector3(-601.3793f, 5709.3892f, 36.2225f);
                    heading = 354.0590f;
                    break;
                case 3:
                    spawnpoint = new Vector3(-684.4380f, 5904.2583f, 16.0914f);
                    heading = 172.5557f;
                    break;
                case 4:
                    spawnpoint = new Vector3(-568.2371f, 6136.8027f, 6.5875f);
                    heading = 329.3406f;
                    break;
                case 5:
                    spawnpoint = new Vector3(-523.8046f, 6297.3838f, 10.0447f);
                    heading = 164.7881f;
                    break;
                case 6:
                    spawnpoint = new Vector3(-464.2715f, 5512.9995f, 79.5566f);
                    heading = 188.2308f;
                    break;
                case 7:
                    spawnpoint = new Vector3(-585.2133f, 5457.8252f, 59.7015f);
                    heading = 51.8865f;
                    break;
                case 8:
                    spawnpoint = new Vector3(-712.7615f, 5435.1504f, 44.0875f);
                    heading = 264.0792f;
                    break;
                case 9:
                    spawnpoint = new Vector3(-841.4200f, 5415.9341f, 34.1460f);
                    heading = 262.3035f;
                    break;
            }
        }
    }
}