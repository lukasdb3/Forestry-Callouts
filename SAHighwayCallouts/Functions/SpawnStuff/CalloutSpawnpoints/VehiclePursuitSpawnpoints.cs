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
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Case: "+chose+"");
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
            var chose = new Random().Next(1, 2);
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosed Case: "+chose+", pog");
            switch (chose)
            {
                case 1:
                    _spawnpoint = new Vector3(-852.982f, 5454.836f, 34.233f);
                    _heading = 117.303f;
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
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosed Case: "+chose+", pog");
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
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosed Case: "+chose+", pog");
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
            var chose = new Random().Next(1, 2);
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosed Case: "+chose+", pog");
            switch (chose)
            {
                case 1:
                    _spawnpoint = new Vector3(-852.982f, 5454.836f, 34.233f);
                    _heading = 117.303f;
                    break;
            }
        }
    }
}