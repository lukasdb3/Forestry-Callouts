using System;
using Rage;

namespace SAHighwayCallouts.Functions.SpawnStuff.CalloutSpawnpoints
{
    internal class VehiclePursuitSpawns
    {
        internal static void PbCountySpawns(out Vector3 _spawnpoint, out float _heading)
        {
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - choosing spawnpoint in paleto bay chunk");
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

        //Blaine County spawn chunk
        internal static void BCountySpawns(out Vector3 _spawnpoint, out float _heading)
        {
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - choosing spawnpoint in blaine county chunk");
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
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - choosing spawnpoint in los santos chunk");
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
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - choosing spawnpoint in prison chunk");
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
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - choosing spawnpoint in zancudo chunk");
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