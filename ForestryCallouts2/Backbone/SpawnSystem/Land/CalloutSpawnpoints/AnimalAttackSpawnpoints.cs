#region Refrences
//System
using System;
//Rage
using Rage;
#endregion


namespace ForestryCallouts2.Backbone.SpawnSystem.Land.CalloutSpawnpoints
{
    internal static class AnimalAttackSpawnpoints
    {                                                                                                                                                                                                             
        internal static void PaletoBayForest(out Vector3 spawnpoint)
        {
            Logger.DebugLog("ANIMAL ATTACK SPAWNPOINT CHOOSER", "Choosing spawnpoint in PaletoBayForest chunk");
            spawnpoint = default;
            var var = new Random().Next(1, 11);
            Logger.DebugLog("CASE", ""+var+"");
            spawnpoint = var switch
            {
                1 => new Vector3(-791.41f, 5479.48f, 26.51f),
                2 => new Vector3(-817.96f, 5686.05f, 20.2584f),
                3 => new Vector3(-674.7533f, 5672.3379f, 31.8822f),
                4 => new Vector3(-520.2715f, 5619.58f, 56.514f),
                5 => new Vector3(-469.71f, 5554.31f, 74.45f),
                6 => new Vector3(-531.9191f, 5419.2695f, 62.96f),
                7 => new Vector3(-531.4253f, 5209.84f, 81.7115f),
                8 => new Vector3(-573.8650f, 5076.0645f, 129.6978f),
                9 => new Vector3(-778.0753f, 5153.1704f, 129.51f),
                10 => new Vector3(-782.0758f, 5288.1196f, 86.69f),
                _ => spawnpoint
            };
        }

        internal static void AltruistCampArea(out Vector3 spawnpoint)
        {
            Logger.DebugLog("ANIMAL ATTACK SPAWNPOINT CHOOSER", "Choosing spawnpoint in AltruistCampArea chunk");
            spawnpoint = default;
            var var = new Random().Next(1, 9);
            Logger.DebugLog("CASE", ""+var+"");
            spawnpoint = var switch
            {
                1 => new Vector3(-921.68f, 5146.91f, 157.94f),
                2 => new Vector3(-1022.189f, 4969.3906f, 197.7552f),
                3 => new Vector3(-1066.103f, 5005.6104f, 185.8975f),
                4 => new Vector3(-1059.147f, 5064f, 167f),
                5 => new Vector3(-997.30f, 5164.06f, 126.9441f),
                6 => new Vector3(-1218.58f, 5016.66f, 156.16f),
                7 => new Vector3(-1303.494f, 4933.49f, 154.48f),
                8 => new Vector3(-936.4751f, 4834.6475f, 310.2767f),
                _ => spawnpoint
            };
        }
        
        internal static void RatonCanyon(out Vector3 spawnpoint)
        {
            Logger.DebugLog("ANIMAL ATTACK SPAWNPOINT CHOOSER", "Choosing spawnpoint in RatonCanyon chunk");
            spawnpoint = default;
            var var = new Random().Next(1, 12);
            Logger.DebugLog("CASE", ""+var+"");
            spawnpoint = var switch
            {
                1 => new Vector3(-1547.290f, 4842.29f, 68.18f),
                2 => new Vector3(-1617.831f, 4799.2402f, 52.0073f),
                3 => new Vector3(-1615.840f, 4679.32f, 36.8381f),
                4 => new Vector3(-1817.966f, 4691.2261f, 4.08f),
                5 => new Vector3(-1831.665f, 4804.3589f, 4.94f),
                6 => new Vector3(-1571.909f, 4498.8330f, 21.32f),
                7 => new Vector3(-1363.596f, 4460.2588f, 23.62f),
                8 => new Vector3(-1219.53f, 4465.85f, 29.92f),
                9 => new Vector3(-778.0776f, 4405.2378f, 17.5915f),
                10 => new Vector3(-522.3039f, 4353.9609f, 67.46f),
                11 => new Vector3(-414.029f, 4281.9741f, 57.3355f),
                _ => spawnpoint
            };
        }
    }
}