#region Refrences
//System
using System;
//Rage
using Rage;
#endregion


namespace ForestryCallouts2.Backbone.SpawnSystem.Land.CalloutSpawnpoints
{
    internal static class Common
    {
        internal static void PaletoBayForest(out Vector3 spawnpoint, out float heading)
        {
            Logger.DebugLog("COMMON SPAWNPOINT CHOOSER", "Choosing spawnpoint in PaletoBayForest chunk");
            spawnpoint = default;
            heading = 0f;
            var var = new Random().Next(1, 28);
            Logger.DebugLog("CASE", ""+var+"");
            switch (var)
            {
                case 1:
                    spawnpoint = new Vector3(-592.945f, 5460.914f, 59.309f);
                    heading = 81.305f;
                    break;
                case 2:
                    spawnpoint = new Vector3(-508.4965f, 5684.2021f, 49.6173f);
                    heading = 30.7428f;
                    break;
                case 3:
                    spawnpoint = new Vector3(-472.0213f, 5774.2930f, 53.2508f);
                    heading = 152.9303f;
                    break;
                case 4:
                    spawnpoint = new Vector3(-468.4727f, 5558.5293f, 74.1917f);
                    heading = 188.8385f;
                    break;
                case 5:
                    spawnpoint = new Vector3(-469.4032f, 5433.5908f, 71.6343f);
                    heading = 119.5557f;
                    break;
                case 6:
                    spawnpoint = new Vector3(-566.1694f, 5581.9106f, 48.8186f);
                    heading = 152.9647f;
                    break;
                case 7:
                    spawnpoint = new Vector3(-669.1677f, 5503.3584f, 47.5296f);
                    heading = 258.1876f;
                    break;
                case 8:
                    spawnpoint = new Vector3(-692.2323f, 5439.2964f, 46.7640f);
                    heading = 90.8668f;
                    break;
                case 9:
                    spawnpoint = new Vector3(-659.9590f, 5370.8154f, 56.4263f);
                    heading = 182.4655f;
                    break;
                case 10:
                    spawnpoint = new Vector3(-749.3801f, 5323.2993f, 73.3758f);
                    heading = 84.8820f;
                    break;
                case 11:
                    spawnpoint = new Vector3(-796.2667f, 5294.4888f, 84.1730f);
                    heading = 302.1159f;
                    break;
                case 12:
                    spawnpoint = new Vector3(-827.7853f, 5264.9468f, 87.1457f);
                    heading = 97.5045f;
                    break;
                case 13:
                    spawnpoint = new Vector3(-922.4302f, 5288.1650f, 80.1042f);
                    heading = 293.8434f;
                    break;
                case 14:
                    spawnpoint = new Vector3(-691.8478f, 5258.0938f, 76.2853f);
                    heading = 232.4571f;
                    break;
                case 15:
                    spawnpoint = new Vector3(-561.06653f, 5214.7998f, 82.3680f);
                    heading = 93.6321f;
                    break;
                case 16:
                    spawnpoint = new Vector3(-693.6763f, 5486.9639f, 45.9858f);
                    heading = 135.6836f;
                    break;
                case 17:
                    spawnpoint = new Vector3(-788.1378f, 5477.0264f, 27.0525f);
                    heading = 24.5076f;
                    break;
                case 18:
                    spawnpoint = new Vector3(-807.2673f, 5540.7085f, 29.7866f);
                    heading = 24.1304f;
                    break;
                case 19:
                    spawnpoint = new Vector3(-806.9363f, 5602.6802f, 27.2623f);
                    heading = 336.7029f;
                    break;
                case 20:
                    spawnpoint = new Vector3(-723.2274f, 5641.0039f, 28.5254f);
                    heading = 192.7431f;
                    break;
                case 21:
                    spawnpoint = new Vector3(-726.7225f, 5561.6680f, 33.8496f);
                    heading = 325.2479f;
                    break;
                case 22:
                    spawnpoint = new Vector3(-694.2722f, 5691.2158f, 27.185f);
                    heading = 294.1973f;
                    break;
                case 23:
                    spawnpoint = new Vector3(-641.7899f, 5669.9419f, 33.5690f);
                    heading = 45.1423f;
                    break;
                case 24:
                    spawnpoint = new Vector3(-639.4008f, 5741.3423f, 27.7860f);
                    heading = 18.7659f;
                    break;
                case 25:
                    spawnpoint = new Vector3(-625.2606f, 5848.3950f, 22.7845f);
                    heading = 28.5500f;
                    break;
                case 26:
                    spawnpoint = new Vector3(-648.1434f, 5919.4648f, 16.2910f);
                    heading = 197.4303f;
                    break;
                case 27:
                    spawnpoint = new Vector3(-581.1310f, 5878.3813f, 27.5133f);
                    heading = 247.9840f;
                    break;
            }
        }
        
        internal static void AltruistCampArea(out Vector3 spawnpoint, out float heading)
        {
            Logger.DebugLog("COMMON SPAWNPOINT CHOOSER", "Choosing spawnpoint in AltruistCampArea chunk");
            spawnpoint = default;
            heading = 0f;
            var var = new Random().Next(1, 13);
            Logger.DebugLog("CASE", ""+var+"");
            switch (var)
            {
                case 1:
                    spawnpoint = new Vector3(-720.2902f, 5087.2236f, 137.4306f);
                    heading = 78.66f;
                    break;
                case 2:
                    spawnpoint = new Vector3(-876.6685f, 5145.0625f, 151.113f);
                    heading = 64.92f;
                    break;
                case 3:
                    spawnpoint = new Vector3(-970.8989f, 5136.76f, 161.8114f);
                    heading = 139.04f;
                    break;
                case 4:
                    spawnpoint = new Vector3(-973.03f, 4984.72f, 186.34f);
                    heading = 130.1298f;
                    break;
                case 5:
                    spawnpoint = new Vector3(-1048.082f, 5091.8604f, 153.9168f);
                    heading = 291.8707f;
                    break;
                case 6:
                    spawnpoint = new Vector3(-1291.540f, 4944.9541f, 151.3696f);
                    heading = 185.35f;
                    break;
                case 7:
                    spawnpoint = new Vector3(-1295.164f, 4843.7021f, 147.7894f);
                    heading = 247.6328f;
                    break;
                case 8:
                    spawnpoint = new Vector3(-1145.292f, 4852.0522f, 197.9272f);
                    heading = 260.0583f;
                    break;
                case 9:
                    spawnpoint = new Vector3(-1045.259f, 4750.6011f, 235.5020f);
                    heading = 207.3455f;
                    break;
                case 10:
                    spawnpoint = new Vector3(-901.6344f, 4768.0234f, 294.38f);
                    heading = 290.81f;
                    break;
                case 11:
                    spawnpoint = new Vector3(-790.3932f, 4859.45f, 254.3329f);
                    heading = 209.1f;
                    break;
                case 12:
                    spawnpoint = new Vector3(-701.6447f, 4766.82f, 223.6215f);
                    heading = 215.2f;
                    break;
            }
        }
        
        internal static void RatonCanyon(out Vector3 spawnpoint, out float heading)
        {
            Logger.DebugLog("COMMON SPAWNPOINT CHOOSER", "Choosing spawnpoint in RatonCanyon chunk");
            spawnpoint = default;
            heading = 0f;
            var var = new Random().Next(1, 27);
            Logger.DebugLog("CASE", ""+var+"");
            switch (var)
            {
                case 1:
                    spawnpoint = new Vector3(-1523.593f, 4916.0933f, 66.5021f);
                    heading = 141.6875f;
                    break;
                case 2:
                    spawnpoint = new Vector3(-1571.367f, 4854.7915f, 60.5037f);
                    heading = 141.9138f;
                    break;
                case 3:
                    spawnpoint = new Vector3(-1614.450f, 4757.6499f, 51.6670f);
                    heading = 132.1212f;
                    break;
                case 4:
                    spawnpoint = new Vector3(-1598.899f, 4718.8940f, 44.4060f);
                    heading = 116.3642f;
                    break;
                case 5:
                    spawnpoint = new Vector3(-1675.775f, 4667.3501f, 25.80f);
                    heading = 104.2275f;
                    break;
                case 6:
                    spawnpoint = new Vector3(-1534.897f, 4699.0107f, 42.8364f);
                    heading = 224.7848f;
                    break;
                case 7:
                    spawnpoint = new Vector3(-1534.897f, 4699.0107f, 42.8364f);
                    heading = 224.7848f;
                    break;
                case 8:
                    spawnpoint = new Vector3(-1562.165f, 4601.1558f, 20.002f);
                    heading = 177.45f;
                    break;
                case 9:
                    spawnpoint = new Vector3(-1563.534f, 4498.6548f, 20.8687f);
                    heading = 198.2139f;
                    break;
                case 10:
                    spawnpoint = new Vector3(-1479.290f, 4471.8745f, 16.7065f);
                    heading = 277.2596f;
                    break;
                case 11:
                    spawnpoint = new Vector3(-1375.560f, 4469.5747f, 24.4321f);
                    heading = 261.03f;
                    break;
                case 12:
                    spawnpoint = new Vector3(-1292.802f, 4494.3145f, 20.4162f);
                    heading = 292.88f;
                    break;
                case 13:
                    spawnpoint = new Vector3(-1220.317f, 4475.1748f, 29.4546f);
                    heading = 270f;
                    break;
                case 14:
                    spawnpoint = new Vector3(-1145.082f, 4430.7109f, 14.7120f);
                    heading = 237.9264f;
                    break;
                case 15:
                    spawnpoint = new Vector3(-1010.702f, 4418.1162f, 22.5273f);
                    heading = 266.81f;
                    break;
                case 16:
                    spawnpoint = new Vector3(-915.0539f, 4419.5005f, 17.5092f);
                    heading = 282.0385f;
                    break;
                case 17:
                    spawnpoint = new Vector3(-829.9634f, 4411.7505f, 19.7351f);
                    heading = 275.91f;
                    break;
                case 18:
                    spawnpoint = new Vector3(-722.2566f, 4405.8315f, 21.0128f);
                    heading = 243.999f;
                    break;
                case 19:
                    spawnpoint = new Vector3(-618.8926f, 4371.38f, 42.37f);
                    heading = 252.94f;
                    break;
                case 20:
                    spawnpoint = new Vector3(-470.4809f, 4335.3853f, 61.5911f);
                    heading = 233.2231f;
                    break;
                case 21:
                    spawnpoint = new Vector3(-919.4883f, 4369.044f, 14.7787f);
                    heading = 120.5109f;
                    break;
                case 22:
                    spawnpoint = new Vector3(-1022.015f, 4360.3584f, 11.2534f);
                    heading = 69.1234f;
                    break;
                case 23:
                    spawnpoint = new Vector3(-1080.793f, 4368.4429f, 12.6370f);
                    heading = 246.4679f;
                    break;
                case 24:
                    spawnpoint = new Vector3(-1198.985f, 4366.1606f, 6.26f);
                    heading = 86.8461f;
                    break;
                case 25:
                    spawnpoint = new Vector3(-1388.440f, 4301.6934f, 3.4854f);
                    heading = 79.3800f;
                    break;
                case 26:
                    spawnpoint = new Vector3(-1556.321f, 4328.9180f, 3.5556f);
                    heading = 65.8617f;
                    break;
            }
        }
    }
}