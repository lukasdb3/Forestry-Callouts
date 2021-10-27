using System;
using Rage;

namespace SAHighwayCallouts.Functions.SpawnStuff.CalloutSpawnpoints
{
    public class GrandTheftAutoSpawnpoints
    {
        internal static void PbCountySpawns(out Vector3 spawnpoint, out float heading, out Vector3 vicSpawnpoint,
            out float vicHeading)
        {
            spawnpoint = default;
            heading = default;
            vicSpawnpoint = default;
            vicHeading = default;
            Game.LogTrivial(
                "-!!- SAHighwayCallouts - |GrandTheftAutoSpawnChooser| - Choosing Spawnpoint in Paleto Bay Chunk!");
            var chose = new Random().Next(1, 35);
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Case: "+chose+" Out of 34 possible spawns!");
            switch (chose)
            {
                case 1:
                    spawnpoint = new Vector3(-510.843f, 5842.894f, 33.975f);
                    heading = 150.460f;
                    vicSpawnpoint = new Vector3(-511.865f, 5849.227f, 34.235f);
                    vicHeading = 213.244f;
                    break;
                case 2:
                    spawnpoint = new Vector3(-542.186f, 5780.496f, 35.324f);
                    heading = 158.827f;
                    vicSpawnpoint = new Vector3(-544.087f, 5785.691f, 35.647f);
                    vicHeading = 219.777f;
                    break;
                case 3:
                    spawnpoint = new Vector3(-742.868f, 5524.791f, 35.780f);
                    heading = 121.615f;
                    vicSpawnpoint = new Vector3(-738.813f, 5529.277f, 36.299f);
                    vicHeading = 173.654f;
                    break;
                case 4:
                    spawnpoint = new Vector3(-953.681f, 5425.778f, 38.324f);
                    heading = 109.177f;
                    vicSpawnpoint = new Vector3(-950.256f, 5430.154f, 38.466f);
                    vicHeading = 154.684f;
                    break;
                case 5:
                    spawnpoint = new Vector3(-1430.232f, 5084.055f, 60.542f);
                    heading = 123.198f;
                    vicSpawnpoint = new Vector3(-1431.714f, 5090.424f, 61.192f);
                    vicHeading = 192.145f;
                    break;
                case 6:
                    spawnpoint = new Vector3(-1654.933f, 4859.917f, 60.548f);
                    heading = 134.167f;
                    vicSpawnpoint = new Vector3(-1653.735f, 4864.130f, 60.778f);
                    vicHeading = 161.889f;
                    break;
                case 7:
                    spawnpoint = new Vector3(-1543.433f, 4938.793f, 61.470f);
                    heading = 317.284f;
                    vicSpawnpoint = new Vector3(-1545.236f, 4932.806f, 61.751f);
                    vicHeading = 340.109f;
                    break;
                case 8:
                    spawnpoint = new Vector3(-1297.563f, 5204.048f, 56.915f);
                    heading = 349.523f;
                    vicSpawnpoint = new Vector3(-1296.034f, 5198.704f, 57.731f);
                    vicHeading = 21.389f;
                    break;
                case 9:
                    spawnpoint = new Vector3(-1150.600f, 5263.881f, 53.207f);
                    heading = 310.431f;
                    vicSpawnpoint = new Vector3(-1152.122f, 5258.131f, 53.596f);
                    vicHeading = 351.740f;
                    break;
                case 10:
                    spawnpoint = new Vector3(-728.607f, 5504.559f, 35.827f);
                    heading = 302.145f;
                    vicSpawnpoint = new Vector3(-731.755f, 5499.160f, 36.175f);
                    vicHeading = 334.657f;
                    break;
                case 11:
                    spawnpoint = new Vector3(-562.973f, 5661.319f, 37.845f);
                    heading = 336.155f;
                    vicSpawnpoint = new Vector3(-561.366f, 5656.101f, 38.264f);
                    vicHeading = 21.444f;
                    break;
                case 12:
                    spawnpoint = new Vector3(-374.086f, 5980.675f, 31.205f);
                    heading = 317.315f;
                    vicSpawnpoint = new Vector3(-360.793f, 5994.524f, 31.350f);
                    vicHeading = 20.005f;
                    break;
                case 13:
                    spawnpoint = new Vector3(-283.582f, 6074.542f, 31.020f);
                    heading = 354.025f;
                    vicSpawnpoint = new Vector3(-278.707f, 6076.227f, 31.426f);
                    vicHeading = 110.017f;
                    break;
                case 14:
                    spawnpoint = new Vector3(45.134f, 6406.496f, 30.958f);
                    heading = 314.511f;
                    vicSpawnpoint = new Vector3(45.440f, 6402.444f, 31.354f);
                    vicHeading = 58.753f;
                    break;
                case 15:
                    spawnpoint = new Vector3(246.945f, 6553.275f, 30.942f);
                    heading = 280.022f;
                    vicSpawnpoint = new Vector3(243.432f, 6549.183f, 31.366f);
                    vicHeading = 328.799f;
                    break;
                case 16:
                    spawnpoint = new Vector3(640.595f, 6514.359f, 27.924f);
                    heading = 260.490f;
                    vicSpawnpoint = new Vector3(641.684f, 6509.299f, 28.153f);
                    vicHeading = 23.447f;
                    break;
                case 17:
                    spawnpoint = new Vector3(990.391f, 6474.049f, 20.705f);
                    heading = 267.495f;
                    vicSpawnpoint = new Vector3(987.816f, 6471.275f, 21.145f);
                    vicHeading = 350.012f;
                    break;
                case 18:
                    spawnpoint = new Vector3(1328.779f, 6472.886f, 19.653f);
                    heading = 264.380f;
                    vicSpawnpoint = new Vector3(1325.336f, 6469.313f, 19.915f);
                    vicHeading = 318.770f;
                    break;
                case 19:
                    spawnpoint = new Vector3(1503.375f, 6419.556f, 22.449f);
                    heading = 248.323f;
                    vicSpawnpoint = new Vector3(1501.316f, 6415.234f, 22.659f);
                    vicHeading = 328.070f;
                    break;
                case 20:
                    spawnpoint = new Vector3(1763.587f, 6334.249f, 36.084f);
                    heading = 262.739f;
                    vicSpawnpoint = new Vector3(1759.588f, 6334.368f, 36.198f);
                    vicHeading = 313.857f;
                    break;
                case 21:
                    spawnpoint = new Vector3(1944.159f, 6217.907f, 44.117f);
                    heading = 199.252f;
                    vicSpawnpoint = new Vector3(1940.962f, 6221.708f, 44.281f);
                    vicHeading = 249.123f;
                    break;
                case 22:
                    spawnpoint = new Vector3(2074.814f, 6048.771f, 49.293f);
                    heading = 232.646f;
                    vicSpawnpoint = new Vector3(2070.690f, 6049.936f, 49.468f);
                    vicHeading = 278.753f;
                    break;
                case 23:
                    spawnpoint = new Vector3(2106.279f, 6105.496f, 50.517f);
                    heading = 53.746f;
                    vicSpawnpoint = new Vector3(2109.869f, 6104.038f, 50.895f);
                    vicHeading = 141.979f;
                    break;
                case 24:
                    spawnpoint = new Vector3(1958.622f, 6314.896f, 43.805f);
                    heading = 42.213f;
                    vicSpawnpoint = new Vector3(1962.140f, 6313.311f, 44.188f);
                    vicHeading = 93.626f;
                    break;
                case 25:
                    spawnpoint = new Vector3(1686.463f, 6394.198f, 30.873f);
                    heading = 70.728f;
                    vicSpawnpoint = new Vector3(1690.694f, 6394.897f, 31.423f);
                    vicHeading = 107.430f;
                    break;
                case 26:
                    spawnpoint = new Vector3(1702.667f, 6411.426f, 32.613f);
                    heading = 63.264f;
                    vicSpawnpoint = new Vector3(1706.231f, 6406.729f, 33.324f);
                    vicHeading = 0.246f;
                    break;
                case 27:
                    spawnpoint = new Vector3(1329.616f, 6514.743f, 19.316f);
                    heading = 103.122f;
                    vicSpawnpoint = new Vector3(1326.328f, 6518.462f, 19.637f);
                    vicHeading = 177.679f;
                    break;
                case 28:
                    spawnpoint = new Vector3(970.218f, 6502.759f, 20.698f);
                    heading = 87.341f;
                    vicSpawnpoint = new Vector3(973.499f, 6506.144f, 21.054f);
                    vicHeading = 139.274f;
                    break;
                case 29:
                    spawnpoint = new Vector3(522.201f, 6569.241f, 27.099f);
                    heading = 77.814f;
                    vicSpawnpoint = new Vector3(526.819f, 6570.607f, 27.500f);
                    vicHeading = 124.996f;
                    break;
                case 30:
                    spawnpoint = new Vector3(298.967f, 6584.751f, 29.407f);
                    heading = 92.601f;
                    vicSpawnpoint = new Vector3(303.349f, 6585.252f, 29.608f);
                    vicHeading = 139.405f;
                    break;
                case 31:
                    spawnpoint = new Vector3(51.396f, 6456.553f, 31.027f);
                    heading = 128.787f;
                    vicSpawnpoint = new Vector3(51.214f, 6461.041f, 31.425f);
                    vicHeading = 205.861f;
                    break;
                case 32:
                    spawnpoint = new Vector3(-45.410f, 6354.723f, 31.002f);
                    heading = 187.324f;
                    vicSpawnpoint = new Vector3(-51.283f, 6353.416f, 31.323f);
                    vicHeading = 281.605f;
                    break;
                case 33:
                    spawnpoint = new Vector3(-175.907f, 6219.035f, 30.977f);
                    heading = 133.569f;
                    vicSpawnpoint = new Vector3(-175.621f, 6223.298f, 31.431f);
                    vicHeading = 203.388f;
                    break;
                case 34:
                    spawnpoint = new Vector3(-308.141f, 6088.788f, 30.953f);
                    heading = 209.297f;
                    vicSpawnpoint = new Vector3(-306.209f, 6093.432f, 31.398f);
                    vicHeading = 154.102f;
                    break;
            }
        }

        //Blaine County spawn chunk
        internal static void BCountySpawns(out Vector3 spawnpoint, out float heading, out Vector3 vicSpawnpoint,
            out float vicHeading)
        {
            spawnpoint = default;
            heading = default;
            vicSpawnpoint = default;
            vicHeading = default;
            Game.LogTrivial(
                "-!!- SAHighwayCallouts - |GrandTheftAutoSpawnChooser| - Choosing Spawnpoint in Blaine County Chunk!");
            var chose = new Random().Next(1, 31);
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Case: "+chose+" Out of 1 possible spawns!");
            switch (chose)
            {
                case 1:
                    spawnpoint = new Vector3();
                    heading = 0;
                    vicSpawnpoint = new Vector3();
                    vicHeading = 0;
                    break;
            }
        }

        //Los Santos county spawn chunk
        internal static void LsCountySpawns(out Vector3 spawnpoint, out float heading, out Vector3 vicSpawnpoint,
            out float vicHeading)
        {
            spawnpoint = default;
            heading = default;
            vicSpawnpoint = default;
            vicHeading = default;
            Game.LogTrivial(
                "-!!- SAHighwayCallouts - |GrandTheftAutoSpawnChooser| - Choosing Spawnpoint in LosSantos County Chunk!");
            var chose = new Random().Next(1, 31);
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Case: "+chose+" Out of 1 possible spawns!");
            switch (chose)
            {
                case 1:
                    spawnpoint = new Vector3();
                    heading = 0;
                    vicSpawnpoint = new Vector3();
                    vicHeading = 0;
                    break;
            }
        }

        internal static void PrisonSpawns(out Vector3 spawnpoint, out float heading, out Vector3 vicSpawnpoint,
            out float vicHeading)
        {
            spawnpoint = default;
            heading = default;
            vicSpawnpoint = default;
            vicHeading = default;
            Game.LogTrivial(
                "-!!- SAHighwayCallouts - |GrandTheftAutoSpawnChooser| - Choosing Spawnpoint in Prison Chunk!");
            var chose = new Random().Next(1, 31);
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Case: "+chose+" Out of 1 possible spawns!");
            switch (chose)
            {
                case 1:
                    spawnpoint = new Vector3();
                    heading = 0;
                    vicSpawnpoint = new Vector3();
                    vicHeading = 0;
                    break;
            }
        }

        internal static void ZancudoSpawns(out Vector3 spawnpoint, out float heading, out Vector3 vicSpawnpoint,
            out float vicHeading)
        {
            spawnpoint = default;
            heading = default;
            vicSpawnpoint = default;
            vicHeading = default;
            Game.LogTrivial(
                "-!!- SAHighwayCallouts - |GrandTheftAutoSpawnChooser| - Choosing Spawnpoint in Zancudo Chunk!");
            var chose = new Random().Next(1, 31);
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Case: "+chose+" Out of 1 possible spawns!");
            switch (chose)
            {
                case 1:
                    spawnpoint = new Vector3();
                    heading = 0;
                    vicSpawnpoint = new Vector3();
                    vicHeading = 0;
                    break;
            }
        }

        internal static void Vespucci(out Vector3 _spawnpoint, out float _heading, out Vector3 vicSpawnpoint, out float vicHeading)
        {
            Game.LogTrivial(
                "-!!- SAHighwayCallouts - |GrandTheftAutoSpawnChooser| - Choosing Spawnpoint in Vespucci County Chunk!");
            _spawnpoint = default;
            _heading = default;
            vicSpawnpoint = default;
            vicHeading = default;
            var chose = new Random().Next(1, 2);
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Case: "+chose+" Out of 1 possible spawns!");
            switch (chose)
            {
                case 1:
                    _spawnpoint = new Vector3();
                    _heading = 0;
                    vicSpawnpoint = new Vector3();
                    vicHeading = 0;
                    break;
            }
        }
    }
}