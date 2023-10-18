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
            Log.Debug("ANIMAL ON ROADWAY SPAWNPOINT CHOOSER", "Choosing spawnpoint in PaletoBayForest chunk");
            spawnpoint = default;
            heading = 0f;
            safePos = default;
            var var = new Random().Next(1, 9);
            Log.Debug("CASE", ""+var+"");
            switch (var)
            {
               case 1:
                   spawnpoint = new Vector3(-491.3061f, 5868.85f, 33.3474f);
                   heading = 146.5537f;
                   safePos = new Vector3(-557.9032f, 5871.9941f, 31.46f);
                   break;
               case 2:
                   spawnpoint = new Vector3(-504.6572f, 5828.9170f, 34.0549f);
                   heading = 326.1937f;
                   safePos = new Vector3(-562.1722f, 5864.2734f, 30.76f);
                   break;
               case 3:
                   spawnpoint = new Vector3(-533.6594f, 5776.8760f, 35.3211f);
                   heading = 157.7f;
                   safePos = new Vector3(-562.1722f, 5864.2734f, 30.76f);
                   break;
               case 4:
                   spawnpoint = new Vector3(-582.8653f, 5687.5474f, 37.60f);
                   heading = 159.4734f;
                   safePos = new Vector3(-562.1722f, 5864.2734f, 30.76f);
                   break;
               case 5:
                   spawnpoint = new Vector3(-710.3641f, 5548.4751f, 37.0184f);
                   heading = 138f;
                   safePos = new Vector3(-709.0730f, 5420.1455f, 46.85f);
                   break;
               case 6:
                   spawnpoint = new Vector3(-836.8792f, 5452.8892f, 33.67f);
                   heading = 93f;
                   safePos = new Vector3(-709.0730f, 5420.1455f, 46.85f);
                   break;
               case 7:
                   spawnpoint = new Vector3(-792.5322f, 5547.7310f, 32.94f);
                   heading = 4.1643f;
                   safePos = new Vector3(-802.8455f, 5508.1060f, 25.6415f);
                   break;
               case 8:
                   spawnpoint = new Vector3(-782.7314f, 5618.12f, 26.6f);
                   heading = 339f;
                   safePos = new Vector3(-711.42f, 5642.68f, 29.83f);
                   break;
            }
        }

        internal static void AltruistCampArea(out Vector3 spawnpoint, out float heading, out Vector3 safePos)
        {
            Log.Debug("ANIMAL ON ROADWAY SPAWNPOINT CHOOSER", "Choosing spawnpoint in AltruistCampArea chunk");
            spawnpoint = default;
            heading = 0f;
            safePos = default;
            var var = new Random().Next(1, 2);
            Log.Debug("CASE", ""+var+"");
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
            Log.Debug("ANIMAL ON ROADWAY SPAWNPOINT CHOOSER", "Choosing spawnpoint in RatonCanyon chunk");
            spawnpoint = default;
            heading = 0f;
            safePos = default;
            var var = new Random().Next(1, 13);
            Log.Debug("CASE", ""+var+"");
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