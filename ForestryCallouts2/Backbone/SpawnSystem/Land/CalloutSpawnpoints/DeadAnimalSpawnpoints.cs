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
            Log.Debug("DEAD ANIMAL SPAWNPOINT CHOOSER", "Choosing spawnpoint in PaletoBayForest chunk");
            spawnpoint = default;
            heading = 0f;
            var var = new Random().Next(1, 13);
            Log.Debug("CASE", ""+var+"");
            switch (var)
            {
                case 1:
                    spawnpoint = new Vector3(-576.4970f, 5685.1675f, 37.8666f);
                    heading = 330.0927f;
                    break;
                case 2:
                    spawnpoint = new Vector3(-683.2231f, 5591.5088f, 38.7006f);
                    heading = 133.2252f;
                    break;
                case 3:
                    spawnpoint = new Vector3(-763.9525f, 5505.3667f, 34.7977f);
                    heading = 122.7520f;
                    break;
                case 4:
                    spawnpoint = new Vector3(-788.3622f, 5538.0200f, 33.4537f);
                    heading = 16.4f;
                    break;
                case 5:
                    spawnpoint = new Vector3(-769.6201f, 5668.0576f, 22.7732f);
                    heading = 339.9330f;
                    break;
                case 6:
                    spawnpoint = new Vector3(-692.5513f, 5845.1406f, 16.6653f);
                    heading = 342.8665f;
                    break;
                case 7:
                    spawnpoint = new Vector3(-637.3229f, 6050.8706f, 8.0982f);
                    heading = 333.0563f;
                    break;
                case 8:
                    spawnpoint = new Vector3(-553.8156f, 6052.85f, 20.3573f);
                    heading = 244.5416f;
                    break;
                case 9:
                    spawnpoint = new Vector3(-404.1806f, 5972.3110f, 31.4180f);
                    heading = 183.82f;
                    break;
                case 10:
                    spawnpoint = new Vector3(-525.1663f, 5777.2930f, 35.2343f);
                    heading = 356.8254f;
                    break;
                case 11:
                    spawnpoint = new Vector3(-646.1140f, 5568.22f, 38.5324f);
                    heading = 298.699f;
                    break;
                case 12:
                    spawnpoint = new Vector3(-837.65f, 5448.31f, 33.7010f);
                    heading = 271.5435f;
                    break;
            }
        }

        internal static void AltruistCampArea(out Vector3 spawnpoint, out float heading)
        {
            Log.Debug("DEAD ANIMAL SPAWNPOINT CHOOSER", "Choosing spawnpoint in AltruistCampArea chunk");
            spawnpoint = default;
            heading = 0f;
            var var = new Random().Next(1, 8);
            Log.Debug("CASE", "" + var + "");
            switch (var)
            {
                case 1:
                    spawnpoint = new Vector3(-1065.239f, 5337.6929f, 45.2309f);
                    heading = 110.2f;
                    break;
                case 2:
                    spawnpoint = new Vector3(-1196.731f, 5262.3921f, 51.4431f);
                    heading = 95.8423f;
                    break;
                case 3:
                    spawnpoint = new Vector3(-1344.146f, 5138.9692f, 61.4819f);
                    heading = 151.8277f;
                    break;
                case 4:
                    spawnpoint = new Vector3(-1507.596f, 5006.9263f, 62.4736f);
                    heading = 137.3921f;
                    break;
                case 5:
                    spawnpoint = new Vector3(-1443.198f, 5048.6123f, 61.2626f);
                    heading = 303.3279f;
                    break;
                case 6:
                    spawnpoint = new Vector3(-1240.570f, 5254.3232f, 49.76f);
                    heading = 263.9357f;
                    break;
                case 7:
                    spawnpoint = new Vector3(-1028.105f, 5354.3457f, 42.8698f);
                    heading = 317f;
                    break;
            }
        }
        
        internal static void RatonCanyon(out Vector3 spawnpoint, out float heading)
        {
            Log.Debug("DEAD ANIMAL SPAWNPOINT CHOOSER", "Choosing spawnpoint in RatonCanyon chunk");
            spawnpoint = default;
            heading = 0f;
            var var = new Random().Next(1, 10);
            Log.Debug("CASE", ""+var+"");
            switch (var)
            {
                case 1:
                    spawnpoint = new Vector3(-1611.845f, 4894.4907f, 61.07f);
                    heading = 133f;
                    break;
                case 2:
                    spawnpoint = new Vector3(-1818.147f, 4707.3965f, 56.9444f);
                    heading = 134.5213f;
                    break;
                case 3:
                    spawnpoint = new Vector3(-1905.275f, 4621.5542f, 56.9380f);
                    heading = 133.98f;
                    break;
                case 4:
                    spawnpoint = new Vector3(-1972.413f, 4554.0757f, 56.9629f);
                    heading = 135.3631f;
                    break;
                case 5:
                    spawnpoint = new Vector3(-2063.409f, 4466.7603f, 58.116f);
                    heading = 90.4210f;
                    break;
                case 6:
                    spawnpoint = new Vector3(-2001.072f, 4511.4878f, 56.9755f);
                    heading = 314.7195f;
                    break;
                case 7:
                    spawnpoint = new Vector3(-1906.231f, 4605.9443f, 56.9572f);
                    heading = 315.9071f;
                    break;
                case 8:
                    spawnpoint = new Vector3(-1786.461f, 4724.7783f, 56.9491f);
                    heading = 315.0585f;
                    break;
                case 9:
                    spawnpoint = new Vector3(-1675.988f, 4816.12f, 60.5868f);
                    heading = 307.7568f;
                    break;
            }
        }
    }
}