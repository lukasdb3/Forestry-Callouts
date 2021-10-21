using System;
using Rage;

namespace SAHighwayCallouts.Functions.SpawnStuff.CalloutSpawnpoints
{
    public class GrandTheftAutoSpawnpoints
    {
        internal static void PbCountySpawns(out Vector3 spawnpoint, out float heading, out Vector3 vicSpawnpoint, out float vicHeading)
        {
            spawnpoint = default;
            heading = default;
            vicSpawnpoint = default;
            vicHeading = default;
            Game.LogTrivial("-!!- SAHighwayCallouts - |GrandTheftAutoSpawnChooser| - Choosing Spawnpoint in Paleto Bay Chunk!");
            var chose = new Random().Next(1, 31);
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Case: "+chose+"");
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

        //Blaine County spawn chunk
        internal static void BCountySpawns(out Vector3 spawnpoint, out float heading, out Vector3 vicSpawnpoint, out float vicHeading)
        {
            spawnpoint = default;
            heading = default;
            vicSpawnpoint = default;
            vicHeading = default;
            Game.LogTrivial("-!!- SAHighwayCallouts - |GrandTheftAutoSpawnChooser| - Choosing Spawnpoint in Paleto Bay Chunk!");
            var chose = new Random().Next(1, 31);
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Case: "+chose+"");
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
        internal static void LsCountySpawns(out Vector3 spawnpoint, out float heading, out Vector3 vicSpawnpoint, out float vicHeading)
        {
            spawnpoint = default;
            heading = default;
            vicSpawnpoint = default;
            vicHeading = default;
            Game.LogTrivial("-!!- SAHighwayCallouts - |GrandTheftAutoSpawnChooser| - Choosing Spawnpoint in Paleto Bay Chunk!");
            var chose = new Random().Next(1, 31);
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Case: "+chose+"");
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

        internal static void PrisonSpawns(out Vector3 spawnpoint, out float heading, out Vector3 vicSpawnpoint, out float vicHeading)
        {
            spawnpoint = default;
            heading = default;
            vicSpawnpoint = default;
            vicHeading = default;
            Game.LogTrivial("-!!- SAHighwayCallouts - |GrandTheftAutoSpawnChooser| - Choosing Spawnpoint in Paleto Bay Chunk!");
            var chose = new Random().Next(1, 31);
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Case: "+chose+"");
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

        internal static void ZancudoSpawns(out Vector3 spawnpoint, out float heading, out Vector3 vicSpawnpoint, out float vicHeading)
        {
            spawnpoint = default;
            heading = default;
            vicSpawnpoint = default;
            vicHeading = default;
            Game.LogTrivial("-!!- SAHighwayCallouts - |GrandTheftAutoSpawnChooser| - Choosing Spawnpoint in Paleto Bay Chunk!");
            var chose = new Random().Next(1, 31);
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Case: "+chose+"");
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
    }
}