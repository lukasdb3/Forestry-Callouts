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
            var chose = new Random().Next(1, 2);
            Game.LogTrivial("-!!- SAHighwayCallouts - |GrandTheftAutoSpawnChooser| - Choosing Case: " + chose + "");
            switch (chose)
            {
                case 1:
                    spawnpoint = new Vector3(-510.843f, 5842.894f, 33.975f);
                    heading = 150.460f;
                    vicSpawnpoint = new Vector3(-511.865f, 5849.227f, 34.235f);
                    vicHeading = 213.244f;
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
            Game.LogTrivial("-!!- SAHighwayCallouts - |GrandTheftAutoSpawnChooser| - Choosing Case: " + chose + "");
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
            Game.LogTrivial("-!!- SAHighwayCallouts - |GrandTheftAutoSpawnChooser| - Choosing Case: " + chose + "");
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
            Game.LogTrivial("-!!- SAHighwayCallouts - |GrandTheftAutoSpawnChooser| - Choosing Case: " + chose + "");
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
            Game.LogTrivial("-!!- SAHighwayCallouts - |GrandTheftAutoSpawnChooser| - Choosing Case: " + chose + "");
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
            Game.LogTrivial("-!!- SAHighwayCallouts - |GrandTheftAutoSpawnChooser| - Choosing Case: " + chose +
                            " Out of 1 possible spawns!");
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