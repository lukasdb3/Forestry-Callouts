#region Refrences
//System
using System;
//Rage
using Rage;
#endregion


namespace ForestryCallouts2.Backbone.SpawnSystem.Land.CalloutSpawnpoints
{
    internal static class AnimalOnRoadwaySpawnpoints
    {                                                                                                                                                       
        internal static void PaletoBayForest(out Vector3 spawnpoint, out float heading, out Vector3 safePos)
        {
            Logger.DebugLog("ANIMAL ON ROADWAY SPAWNPOINT CHOOSER", "Choosing spawnpoint in PaletoBayForest chunk");
            spawnpoint = default;
            heading = 0f;
            safePos = default;
            int var = new Random().Next(1, 3);
            Logger.DebugLog("CASE", ""+var+"");
            switch (var)
            {
               case 1:
                   spawnpoint = new Vector3();
                   heading = 0f;
                   safePos = new Vector3();
                   break;
            }
        }

        internal static void AltruistCampArea(out Vector3 spawnpoint, out float heading, out Vector3 safePos)
        {
            Logger.DebugLog("ANIMAL ON ROADWAY SPAWNPOINT CHOOSER", "Choosing spawnpoint in AltruistCampArea chunk");
            spawnpoint = default;
            heading = 0f;
            safePos = default;
            int var = new Random().Next(1, 2);
            Logger.DebugLog("CASE", ""+var+"");
            switch (var)
            {
                case 1:
                    spawnpoint = new Vector3(-1414.124f, 5066.7891f, 60.7062f);
                    heading = 299f;
                    safePos = new Vector3(-1513.897f, 4929.1709f, 65.9402f);
                    break;
            }
        }
        
        internal static void RatonCanyon(out Vector3 spawnpoint, out float heading, out Vector3 safePos)
        {
            Logger.DebugLog("ANIMAL ON ROADWAY SPAWNPOINT CHOOSER", "Choosing spawnpoint in RatonCanyon chunk");
            spawnpoint = default;
            heading = 0f;
            safePos = default;
            int var = new Random().Next(1, 13);
            Logger.DebugLog("CASE", ""+var+"");
            switch (var)
            {
                case 1:
                    spawnpoint = new Vector3(-1647.504f, 4862.7451f, 60.8843f);
                    heading = 33.2505f;
                    safePos = new Vector3(-1513.897f, 4929.1709f, 65.9402f);
                    break;
                case 2:
                    spawnpoint = new Vector3(-1542.996f, 4959.8426f, 61.5130f);
                    heading = 138.2908f;
                    safePos = new Vector3(-1513.897f, 4929.1709f, 65.9402f);
                    break;
                case 3:
                    spawnpoint = new Vector3(-1710.201f, 4807.36f, 59.1351f);
                    heading = 126f;
                    safePos = new Vector3(-1513.897f, 4929.1709f, 65.9402f);
                    break;
                case 4:
                    spawnpoint = new Vector3(-1737.367f, 4770.1035f, 57.2020f);
                    heading = 285.1185f;
                    safePos = new Vector3(-1513.897f, 4929.1709f, 65.9402f);
                    break;
                case 5:
                    spawnpoint = new Vector3(-1654.155f, 4834.7944f, 60.2917f);
                    heading = 312.58f;
                    safePos = new Vector3(-1513.897f, 4929.1709f, 65.9402f);
                    break;
                case 6:
                    spawnpoint = new Vector3(-1579.841f, 4904.1787f, 60.8790f);
                    heading = 317.5886f;
                    safePos = new Vector3(-1513.897f, 4929.1709f, 65.9402f);
                    break;
                case 7:
                    spawnpoint = new Vector3(-1819.728f, 4703.9297f, 56.5454f);
                    heading = 134.5575f;
                    safePos = new Vector3(-1513.897f, 4929.1709f, 65.9402f);
                    break;
                case 8:
                    spawnpoint = new Vector3(-2016.374f, 4507.3247f, 56.5684f);
                    heading = 313.7586f;
                    safePos = new Vector3(-1953.559f, 4446.6851f, 35.8178f);
                    break;
                case 9:
                    spawnpoint = new Vector3(-1939.014f, 4583.4639f, 56.5403f);
                    heading = 314f;
                    safePos = new Vector3(-1953.559f, 4446.6851f, 35.8178f);
                    break;
                case 10:
                    spawnpoint = new Vector3(-1876.060f, 4640.1704f, 56.5305f);
                    heading = 56.5305f;
                    safePos = new Vector3(-1953.559f, 4446.6851f, 35.8178f);
                    break;
                case 11:
                    spawnpoint = new Vector3(-1930.737f, 4583.6621f, 56.5335f);
                    heading = 136.70f;
                    safePos = new Vector3(-1953.559f, 4446.6851f, 35.8178f);
                    break;
                case 12:
                    spawnpoint = new Vector3(-2093.828f, 4462.9336f, 59.7598f);
                    heading = 59.7598f;
                    safePos = new Vector3(-1953.559f, 4446.6851f, 35.8178f);
                    break;
            }
        }
    }
}