using System;
using Rage;

namespace SAHighwayCallouts.Functions.SpawnStuff.CalloutSpawnpoints
{
    internal class VehiclePursuitSpawnpoints
    {
        internal static void PbCountySpawns(out Vector3 spawnpoint, out float heading)
        {
            spawnpoint = default;
            heading = default;
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Spawnpoint in Paleto Bay Chunk!");
            var chose = new Random().Next(1, 31);
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Case: "+chose+" Out of 30 possible spawns!");
            switch (chose)
            {
                case 1:
                    spawnpoint = new Vector3(-852.982f, 5454.836f, 34.233f);
                    heading = 117.303f;
                    break;
                case 2:
                    spawnpoint = new Vector3(-541.798f, 5780.679f, 35.397f);
                    heading = 155.640f;
                    break;
                case 3:
                    spawnpoint = new Vector3(-566.625f, 5665.663f, 37.930f);
                    heading = 335.815f;
                    break;
                case 4:
                    spawnpoint = new Vector3(-733.385f, 5530.453f, 36.261f);
                    heading = 120.670f;
                    break;
                case 5:
                    spawnpoint = new Vector3(-1231.711f, 5270.833f, 49.902f);
                    heading = 78.912f;
                    break;
                case 6:
                    spawnpoint = new Vector3(-1371.066f, 5119.200f, 61.133f);
                    heading = 126.316f;
                    break;
                case 7:
                    spawnpoint = new Vector3(-1728.547f, 4799.650f, 58.392f);
                    heading = 131.501f;
                    break;
                case 8:
                    spawnpoint = new Vector3(-1659.393f, 4822.473f, 60.522f);
                    heading = 310.906f;
                    break;
                case 9:
                    spawnpoint = new Vector3(-1425.183f, 5054.604f, 61.108f);
                    heading = 308.354f;
                    break;
                case 10:
                    spawnpoint = new Vector3(-1216.983f, 5245.341f, 50.464f);
                    heading = 269.819f;
                    break;
                case 11:
                    spawnpoint = new Vector3(-747.899f, 5492.694f, 35.109f);
                    heading = 303.076f;
                    break;
                case 12:
                    spawnpoint = new Vector3(-502.100f, 5808.964f, 34.460f);
                    heading = 333.886f;
                    break;
                case 13:
                    spawnpoint = new Vector3(-274.900f, 6086.420f, 31.004f);
                    heading = 314.095f;
                    break;
                case 14:
                    spawnpoint = new Vector3(5.438f, 6365.410f, 31.025f);
                    heading = 314.424f;
                    break;
                case 15:
                    spawnpoint = new Vector3(347.399f, 6560.410f, 28.234f);
                    heading = 270.513f;
                    break;
                case 16:
                    spawnpoint = new Vector3(653.479f, 6512.945f, 27.988f);
                    heading = 258.493f;
                    break;
                case 17:
                    spawnpoint = new Vector3(1064.054f, 6476.520f, 20.810f);
                    heading = 267.276f;
                    break;
                case 18:
                    spawnpoint = new Vector3(-1502.610f, 6425.030f, 22.502f);
                    heading = 248.825f;
                    break;
                case 19:
                    spawnpoint = new Vector3(2010.704f, 6108.402f, 47.268f);
                    heading = 222.311f;
                    break;
                case 20:
                    spawnpoint = new Vector3(2113.487f, 6100.685f, 50.773f);
                    heading = 57.645f;
                    break;
                case 21:
                    spawnpoint = new Vector3(2001.583f, 6238.755f, 46.149f);
                    heading = 23.547f;
                    break;
                case 22:
                    spawnpoint = new Vector3(-1675.237f, 6396.610f, 30.240f);
                    heading = 77.109f;
                    break;
                case 23:
                    spawnpoint = new Vector3(1233.621f, 6502.686f, 20.545f);
                    heading = 90.819f;
                    break;
                case 24:
                    spawnpoint = new Vector3(964.402f, 6501.053f, 20.802f);
                    heading = 88.910f;
                    break;
                case 25:
                    spawnpoint = new Vector3(591.445f, 6551.239f, 27.857f);
                    heading = 76.877f;
                    break;
                case 26:
                    spawnpoint = new Vector3(366.325f, 6584.637f, 27.884f);
                    heading = 89.731f;
                    break;
                case 27:
                    spawnpoint = new Vector3(55.670f, 6458.468f, 21.070f);
                    heading = 133.899f;
                    break;
                case 28:
                    spawnpoint = new Vector3(-177.300f, 6219.154f, 31.115f);
                    heading = 138.086f;
                    break;
                case 29:
                    spawnpoint = new Vector3(-440.248f, 5944.581f, 31.700f);
                    heading = 140.214f;
                    break;
                case 30:
                    spawnpoint = new Vector3(-462.513f, 5915.001f, 32.534f);
                    heading = 143.590f;
                    break;
                
            }
        }

        //Blaine County spawn chunk
        internal static void BCountySpawns(out Vector3 _spawnpoint, out float _heading)
        {
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Spawnpoint in Blaine County Chunk!");
            _spawnpoint = default;
            _heading = default;
            var chose = new Random().Next(1, 34);
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Case: "+chose+" Out of 33 possible spawns!");
            switch (chose)
            {
                case 1:
                    _spawnpoint = new Vector3(2382.556f, 5736.244f, 45.547f);
                    _heading = 208.665f;
                    break;
                case 2:
                    _spawnpoint = new Vector3(2445.212f, 5611.661f, 44.841f);
                    _heading = 200.057f;
                    break;
                case 3:
                    _spawnpoint = new Vector3(2503.313f, 5462.646f, 44.465f);
                    _heading = 196.936f;
                    break;
                case 4:
                    _spawnpoint = new Vector3(2558.056f, 5263.972f, 44.555f);
                    _heading = 192.307f;
                    break;
                case 5:
                    _spawnpoint = new Vector3(2616.532f, 5042.813f, 44.695f);
                    _heading = 195.098f;
                    break;
                case 6:
                    _spawnpoint = new Vector3(2651.072f, 4917.595f, 44.603f);
                    _heading = 192.902f;
                    break;
                case 7:
                    _spawnpoint = new Vector3(2701.251f, 4692.160f, 44.207f);
                    _heading = 190.982f;
                    break;
                case 8:
                    _spawnpoint = new Vector3(2729.872f, 4548.531f, 45.844f);
                    _heading = 190.154f;
                    break;
                case 9:
                    _spawnpoint = new Vector3(2791.645f, 4326.450f, 49.928f);
                    _heading = 201.041f;
                    break;
                case 10:
                    _spawnpoint = new Vector3(2856.414f, 4154.823f, 50.106f);
                    _heading = 201.001f;
                    break;
                case 11:
                    _spawnpoint = new Vector3(2909.504f, 3904.742f, 52.054f);
                    _heading = 180.692f;
                    break;
                case 12:
                    _spawnpoint = new Vector3(2861.103f, 3639.537f, 52.528f);
                    _heading = 161.776f;
                    break;
                case 13:
                    _spawnpoint = new Vector3(2847.390f, 3594.175f, 52.980f);
                    _heading = 161.519f;
                    break;
                case 14:
                    _spawnpoint = new Vector3(2752.385f, 3360.460f, 56.015f);
                    _heading = 156.119f;
                    break;
                case 15:
                    _spawnpoint = new Vector3(2607.300f, 3119.526f, 48.225f);
                    _heading = 141.668f;
                    break;
                case 16:
                    _spawnpoint = new Vector3(2400.641f, 2929.729f, 40.168f);
                    _heading = 130.134f;
                    break;
                case 17:
                    _spawnpoint = new Vector3(2421.935f, 2889.064f, 40.131f);
                    _heading = 307.513f;
                    break;
                case 18:
                    _spawnpoint = new Vector3(2619.815f, 3070.558f, 46.781f);
                    _heading = 316.732f;
                    break;
                case 19:
                    _spawnpoint = new Vector3(2733.021f, 3225.832f, 54.613f);
                    _heading = 326.988f;
                    break;
                case 20:
                    _spawnpoint = new Vector3(2790.691f, 3343.540f, 56.027f);
                    _heading = 336.604f;
                    break;
                case 21:
                    _spawnpoint = new Vector3(2846.511f, 3475.169f, 54.827f);
                    _heading = 337.505f;
                    break;
                case 22:
                    _spawnpoint = new Vector3(2909.582f, 3660.015f, 52.556f);
                    _heading = 342.432f;
                    break;
                case 23:
                    _spawnpoint = new Vector3(2950.898f, 3875.438f, 52.235f);
                    _heading = 359.643f;
                    break;
                case 24:
                    _spawnpoint = new Vector3(2923.635f, 4077.600f, 50.710f);
                    _heading = 16.433f;
                    break;
                case 25:
                    _spawnpoint = new Vector3(2859.157f, 4278.650f, 50.071f);
                    _heading = 18.121f;
                    break;
                case 26:
                    _spawnpoint = new Vector3(2791.237f, 4504.488f, 46.940f);
                    _heading = 15.913f;
                    break;
                case 27:
                    _spawnpoint = new Vector3(2744.942f, 4685.484f, 44.248f);
                    _heading = 13.694f;
                    break;
                case 28:
                    _spawnpoint = new Vector3(2684.182f, 4958.951f, 44.632f);
                    _heading = 12.933f;
                    break;
                case 29:
                    _spawnpoint = new Vector3(2626.581f, 5201.918f, 44.617f);
                    _heading = 15.567f;
                    break;
                case 31:
                    _spawnpoint = new Vector3(2561.561f, 5419.201f, 44.432f);
                    _heading = 18.350f;
                    break;
                case 32:
                    _spawnpoint = new Vector3(2497.300f, 5601.948f, 44.769f);
                    _heading = 21.883f;
                    break;
                case 33:
                    _spawnpoint = new Vector3(2288.842f, 5960.696f, 49.457f);
                    _heading = 36.655f;
                    break;
            }
        }
        
        //Los Santos county spawn chunk
        internal static void LsCountySpawns(out Vector3 _spawnpoint, out float _heading)
        {
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Spawnpoint in LosSantos County Chunk!");
            _spawnpoint = default;
            _heading = default;
            var chose = new Random().Next(1, 2);
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Case: "+chose+" Out of 1 possible spawns!");
            switch (chose)
            {
                case 1:
                    _spawnpoint = new Vector3(-852.982f, 5454.836f, 34.233f);
                    _heading = 117.303f;
                    break;
            }
        }

        internal static void PrisonSpawns(out Vector3 _spawnpoint, out float _heading)
        {
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Spawnpoint in Prison Chunk!");
            _spawnpoint = default;
            _heading = default;
            var chose = new Random().Next(1, 2);
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Case: "+chose+" Out of 1 possible spawns!");
            switch (chose)
            {
                case 1:
                    _spawnpoint = new Vector3(-852.982f, 5454.836f, 34.233f);
                    _heading = 117.303f;
                    break;
            }
        }

        internal static void ZancudoSpawns(out Vector3 _spawnpoint, out float _heading)
        {
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Spawnpoint in Zancudo Chunk!");
            _spawnpoint = default;
            _heading = default;
            var chose = new Random().Next(1, 29);
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Case: "+chose+" Out of 28 possible spawns!");
            switch (chose)
            {
                case 1:
                    _spawnpoint = new Vector3(-2206.404f, 4383.886f, 54.434f);
                    _heading = 162.717f;
                    break;
                case 2:
                    _spawnpoint = new Vector3(-2320.511f, 4171.651f, 38.687f);
                    _heading = 156.166f;
                    break;
                case 3:
                    _spawnpoint = new Vector3(-2402.687f, 3940.360f, 24.648f);
                    _heading = 159.381f;
                    break;
                case 4:
                    _spawnpoint = new Vector3(-2479.738f, 3736.720f, 16.473f);
                    _heading = 169.502f;
                    break;
                case 5:
                    _spawnpoint = new Vector3(-2526.694f, 3527.892f, 14.328f);
                    _heading = 14.328f;
                    break;
                case 6:
                    _spawnpoint = new Vector3(-2636.647f, 2864.592f, 16.614f);
                    _heading = 171.888f;
                    break;
                case 7:
                    _spawnpoint = new Vector3(-2670.991f, 2627.342f, 16.611f);
                    _heading = 171.131f;
                    break;
                case 8:
                    _spawnpoint = new Vector3(-2709.818f, 2364.404f, 16.728f);
                    _heading = 167.890f;
                    break;
                case 9:
                    _spawnpoint = new Vector3(-2755.305f, 2257.682f, 21.791f);
                    _heading = 133.991f;
                    break;
                case 10:
                    _spawnpoint = new Vector3(-2859.807f, 2195.936f, 33.394f);
                    _heading = 116.944f;
                    break;
                case 11:
                    _spawnpoint = new Vector3(-2999.808f, 2020.063f, 34.305f);
                    _heading = 170.510f;
                    break;
                case 12:
                    _spawnpoint = new Vector3(-3053.455f, 1737.637f, 36.586f);
                    _heading = 186.815f;
                    break;
                case 13:
                    _spawnpoint = new Vector3(-3072.103f, 1399.256f, 20.980f);
                    _heading = 149.872f;
                    break;
                case 14:
                    _spawnpoint = new Vector3(-3118.595f, 1184.031f, 20.281f);
                    _heading = 176.756f;
                    break;
                case 15:
                    _spawnpoint = new Vector3(-3109.154f, 1052.372f, 20.137f);
                    _heading = 345.364f;
                    break;
                case 16:
                    _spawnpoint = new Vector3(-3086.088f, 1267.057f, 20.148f);
                    _heading = 353.342f;
                    break;
                case 17:
                    _spawnpoint = new Vector3(-3015.867f, 1428.004f, 25.546f);
                    _heading = 320.243f;
                    break;
                case 18:
                    _spawnpoint = new Vector3(-3025.864f, 1730.298f, 36.674f);
                    _heading = 8.386f;
                    break;
                case 19:
                    _spawnpoint = new Vector3(-2975.661f, 2007.671f, 33.298f);
                    _heading = 353.242f;
                    break;
                case 20:
                    _spawnpoint = new Vector3(-2852.095f, 2169.773f, 33.930f);
                    _heading = 295.813f;
                    break;
                case 21:
                    _spawnpoint = new Vector3(-2682.060f, 2391.881f, 16.615f);
                    _heading = 349.775f;
                    break;
                case 22:
                    _spawnpoint = new Vector3(-2636.691f, 2700.156f, 16.611f);
                    _heading = 350.696f;
                    break;
                case 23:
                    _spawnpoint = new Vector3(-2607.471f, 2898.170f, 16.613f);
                    _heading = 350.961f;
                    break;
                case 24:
                    _spawnpoint = new Vector3(-2543.045f, 3415.113f, 13.187f);
                    _heading = 345.474f;
                    break;
                case 25:
                    _spawnpoint = new Vector3(-2454.899f, 3734.228f, 16.622f);
                    _heading = 345.748f;
                    break;
                case 26:
                    _spawnpoint = new Vector3(-2398.623f, 3875.436f, 24.346f);
                    _heading = 343.062f;
                    break;
                case 27:
                    _spawnpoint = new Vector3(-2310.297f, 4132.346f, 37.484f);
                    _heading = 338.941f;
                    break;
                case 28:
                    _spawnpoint = new Vector3(-2176.532f, 4394.190f, 56.773f);
                    _heading = 337.798f;
                    break;
            }
        }
    }
}