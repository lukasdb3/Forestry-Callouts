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
            var chose = new Random().Next(1, 2);
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Case: "+chose+"");
            switch (chose)
            {
                case 1:
                    spawnpoint = new Vector3(-852.982f, 5454.836f, 34.233f);
                    heading = 117.303f;
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