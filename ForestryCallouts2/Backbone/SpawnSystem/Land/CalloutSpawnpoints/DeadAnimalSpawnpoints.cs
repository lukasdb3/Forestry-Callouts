#region Refrences
//System
using System;
//Rage
using Rage;
#endregion


namespace ForestryCallouts2.Backbone.SpawnSystem.Land.CalloutSpawnpoints
{
    internal static class DeadAnimalSpawnpoints
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
                    spawnpoint = new Vector3(-576.4970f, 5685.1675f, 37.8666f);
                    heading = 330.0927f;
                    break;
                case 2:
                    spawnpoint = new Vector3();
                    heading = 0f;
                    break;
                case 3:
                    spawnpoint = new Vector3();
                    heading = 0f;
                    break;
                case 4:
                    spawnpoint = new Vector3();
                    heading = 0f;
                    break;
                case 5:
                    spawnpoint = new Vector3();
                    heading = 0f;
                    break;
                case 6:
                    spawnpoint = new Vector3();
                    heading = 0f;
                    break;
                case 7:
                    spawnpoint = new Vector3();
                    heading = 0f;
                    break;
                case 8:
                    spawnpoint = new Vector3();
                    heading = 0f;
                    break;
                case 9:
                    spawnpoint = new Vector3();
                    heading = 0f;
                    break;
                case 10:
                    spawnpoint = new Vector3();
                    heading = 0f;
                    break;
                case 11:
                    spawnpoint = new Vector3();
                    heading = 0f;
                    break;
                case 12:
                    spawnpoint = new Vector3();
                    heading = 0f;
                    break;
            }
        }
    }
}