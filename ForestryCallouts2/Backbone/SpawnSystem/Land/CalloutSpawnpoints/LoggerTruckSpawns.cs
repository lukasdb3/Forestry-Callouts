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
            Log.Debug("LOGGERTRUCK SPAWNPOINT CHOOSER", "Choosing spawnpoint in PaletoBayForest chunk");
            spawnpoint = default;
            heading = 0f;
            var var = new Random().Next(1, 10);
            Log.Debug("CASE", ""+var+"");
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

        internal static void AltruistCampArea(out Vector3 spawnpoint, out float heading)
        {
            Log.Debug("LOGGERTRUCK SPAWNPOINT CHOOSER", "Choosing spawnpoint in AltruistCampArea chunk");
            spawnpoint = default;
            heading = 0f;
            var var = new Random().Next(1, 8);
            Log.Debug("CASE", "" + var + "");
            switch (var)
            {
                case 1:
                    spawnpoint = new Vector3(-1012.149f, 4958.2231f, 195.8984f);
                    heading = 328.7717f;
                    break;
                case 2:
                    spawnpoint = new Vector3(-885.2104f, 5147.0605f, 152.0907f);
                    heading = 71.42f;
                    break;
                case 3:
                    spawnpoint = new Vector3(-595.4525f, 5042.1948f, 138.8509f);
                    heading = 352.0049f;
                    break;
                case 4:
                    spawnpoint = new Vector3(-852.9235f, 5193.6064f, 114.2079f);
                    heading = 55.26f;
                    break;
                case 5:
                    spawnpoint = new Vector3(-1183.454f, 5019.0986f, 153.6552f);
                    heading = 122.9691f;
                    break;
                case 6:
                    spawnpoint = new Vector3(-1043.679f, 4747.2983f, 235.4950f);
                    heading = 204.7373f;
                    break;
                case 7:
                    spawnpoint = new Vector3(-856.4151f, 4848.1353f, 292.778f);
                    heading = 341.0711f;
                    break;
            }
        }
        
        internal static void RatonCanyon(out Vector3 spawnpoint, out float heading)
        {
            Log.Debug("LOGGERTRUCK SPAWNPOINT CHOOSER", "Choosing spawnpoint in RatonCanyon chunk");
            spawnpoint = default;
            heading = 0f;
            var var = new Random().Next(1, 11);
            Log.Debug("CASE", ""+var+"");
            switch (var)
            {
                case 1:
                    spawnpoint = new Vector3(-1517.770f, 4917.5303f, 66.5512f);
                    heading = 139.8065f;
                    break;
                case 2:
                    spawnpoint = new Vector3(-1565.408f, 4730.4072f, 50.0993f);
                    heading = 230.13f;
                    break;
                case 3:
                    spawnpoint = new Vector3(-1566.203f, 4555.7891f, 17.44f);
                    heading = 168.1090f;
                    break;
                case 4:
                    spawnpoint = new Vector3(-1419.392f, 4485.03f, 23.0381f);
                    heading = 266.54f;
                    break;
                case 5:
                    spawnpoint = new Vector3(-1143.090f, 4432.8457f, 15.0351f);
                    heading = 263.1913f;
                    break;
                case 6:
                    spawnpoint = new Vector3(-878.4147f, 4409.8247f, 20.8171f);
                    heading = 213.2464f;
                    break;
                case 7:
                    spawnpoint = new Vector3(-514.6290f, 4361.6763f, 67.5028f);
                    heading = 264.1174f;
                    break;
                case 8:
                    spawnpoint = new Vector3(-342.7090f, 4257.1582f, 43.4821f);
                    heading = 44.3511f;
                    break;
                case 9:
                    spawnpoint = new Vector3(-1042.426f, 4367.5913f, 11.7499f);
                    heading = 68.2488f;
                    break;
                case 10:
                    spawnpoint = new Vector3(-1463.983f, 4304.0669f, 2.7437f);
                    heading = 86.15f;
                    break;
            }
        }
    }
}