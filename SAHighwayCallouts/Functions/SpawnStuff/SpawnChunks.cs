using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LSPD_First_Response.Mod.API;
using Rage;
using SAHighwayCallouts.Functions.SpawnStuff.CalloutSpawnpoints;

namespace SAHighwayCallouts.Functions.SpawnStuff
{
    internal class SpawnChunks
    {
        //Used by most callouts
        internal static Vector3 finalSpawnpoint; //Final spawnpoint that will be used in callout
        internal static float finalHeading; //Final heading that will be used in callout
        
        //Used by: GrandTheftAuto
        internal static Vector3 finalVicSpawnpoint; //Final victim spawnpoint that will be used in callout
        internal static float finalVicHeading; //Final heading for victim that will be used in the callout
        
        //Used by: IllegalCarMeet
        internal static Vector3 badSpawn1;
        internal static float badHeading1;
        internal static Vector3 badSpawn2;
        internal static float badHeading2;
        internal static Vector3 badSpawn3;
        internal static float badHeading3;
        internal static Vector3 badSpawn4;
        internal static float badHeading4;
        internal static Vector3 badSpawn5;
        internal static float badHeading5;
        internal static Vector3 badSpawn6;
        internal static float badHeading6;
        internal static Vector3 badSpawn7;
        internal static float badHeading7;
        internal static Vector3 badSpawn8;
        internal static float badHeading8;
        internal static Vector3 badSpawn9;
        internal static float badHeading9;
        internal static Vector3 badSpawn10;
        internal static float badHeading10;

        internal static void ChunkGetter(in string cCallout, out string cCounty)
        {
            cCounty = null;
            //Main
            Vector3 playerPos = Game.LocalPlayer.Character.Position;
            List<Vector3> policeStations = new List<Vector3>(); //All stations
            Vector3 closestStation = new Vector3(0f, 0f, 0f); //Closet station to player

            //Bools
            bool gettingClosetChunk = false;
            bool gettingClosetSpawnpoint = false;

            //PoliceStations
            Vector3 pbPoliceStation = new Vector3(-440.185f, 6019.718f, 31.490f); //Paleto Bay Station
            Vector3 bCountyPoliceStation = new Vector3(1856.132f, 3681.852f, 34.268f); //Blaine County station
            Vector3 prisonPoliceStation = new Vector3(1846.577f, 2585.808f, 45.672f); //Prison station
            Vector3 zancudoPoliceStation = new Vector3(-2283.109f, 3372.162f, 31.683f); //Zancudo airforce base
            Vector3 lsCountyPoliceStation = new Vector3(387.063f, 789.904f, 187.693f); //Los santos county station
            Vector3 vPoliceStation = new Vector3(-1108.180f, -845.180f, 19.317f); //Vespucci police station

            if (!gettingClosetChunk)
            {
                gettingClosetChunk = true;
                Game.LogTrivial("-!!- SAHighwayCallouts - |SpawnChunkSystem| - Getting Closest Chunk!");

                policeStations.Add(pbPoliceStation); //Paleto Bay station
                policeStations.Add(bCountyPoliceStation); //Blaine County station
                policeStations.Add(prisonPoliceStation); //Prison
                policeStations.Add(zancudoPoliceStation); //Zancudo
                policeStations.Add(lsCountyPoliceStation); //Los Santos County station
                policeStations.Add(vPoliceStation); //Vespucci police station

                closestStation = policeStations.OrderBy(x => x.DistanceTo(playerPos)).FirstOrDefault();
                Game.LogTrivial("-!!- SAHighwayCallouts - |SpawnChunkSystem| - Closest Chunk Found!");
            }

            if (!gettingClosetSpawnpoint)
            {
                gettingClosetSpawnpoint = true;
                Game.LogTrivial("-!!- SAHighwayCallouts - |SpawnChunkSystem| - Sending to Closest Chunk!");

                if (pbPoliceStation == closestStation)
                {
                    cCounty = "PaletoCounty";
                    PbCounty(in cCallout);
                }

                if (bCountyPoliceStation == closestStation)
                {
                    cCounty = "BlaineCounty";
                    BCounty(in cCallout);
                }

                if (lsCountyPoliceStation == closestStation)
                {
                    cCounty = "LosSantosCounty";
                    LsCounty(in cCallout);
                }

                if (prisonPoliceStation == closestStation)
                {
                    cCounty = "BlaineCounty";
                    Prison(in cCallout);
                }

                if (zancudoPoliceStation == closestStation)
                {
                    cCounty = "PaletoCounty";
                    Zancudo(in cCallout);
                }

                if (vPoliceStation == closestStation)
                {
                    cCounty = "LosSantosCityCounty";
                    Vespucci(in cCallout);
                }
            }
        }

        //Paleto bay spawn chunk
        private static void PbCounty(in string cCallout)
        {
            if (cCallout == "VehiclePursuit") VehiclePursuitSpawnpoints.PbCountySpawns(out finalSpawnpoint, out finalHeading);
            if (cCallout == "GrandTheftAuto") GrandTheftAutoSpawnpoints.PbCountySpawns(out finalSpawnpoint, out finalHeading, out finalVicSpawnpoint, out finalVicHeading);
            if (cCallout == "IllegalCarMeet")
                IllegalCarMeetSpawnpoints.PbCountySpawns(out badSpawn1, out badHeading1, out badSpawn2, out badHeading2,
                    out badSpawn3, out badHeading3, out badSpawn4, out badHeading4, out badSpawn5, out badHeading5,
                    out badSpawn6, out badHeading6, out badSpawn7, out badHeading7, out badSpawn8, out badHeading8,
                    out badSpawn9, out badHeading9, out badSpawn10, out badHeading10);
        }

        //Blaine County spawn chunk
        private static void BCounty(in string cCallout)
        {
            if (cCallout == "VehiclePursuit") VehiclePursuitSpawnpoints.BCountySpawns(out finalSpawnpoint, out finalHeading);
            if (cCallout == "GrandTheftAuto") GrandTheftAutoSpawnpoints.BCountySpawns(out finalSpawnpoint, out finalHeading, out finalVicSpawnpoint, out finalVicHeading);
        }
        
        //Los Santos county spawn chunk
        private static void LsCounty(in string cCallout)
        {
            if (cCallout == "VehiclePursuit") VehiclePursuitSpawnpoints.LsCountySpawns(out finalSpawnpoint, out finalHeading);
            if (cCallout == "GrandTheftAuto") GrandTheftAutoSpawnpoints.LsCountySpawns(out finalSpawnpoint, out finalHeading, out finalVicSpawnpoint, out finalVicHeading);
        }

        private static void Prison(in string cCallout)
        {
            if (cCallout == "VehiclePursuit") VehiclePursuitSpawnpoints.PrisonSpawns(out finalSpawnpoint, out finalHeading);
            if (cCallout == "GrandTheftAuto") GrandTheftAutoSpawnpoints.PrisonSpawns(out finalSpawnpoint, out finalHeading, out finalVicSpawnpoint, out finalVicHeading);
        }

        private static void Zancudo(in string cCallout)
        {
            if (cCallout == "VehiclePursuit") VehiclePursuitSpawnpoints.ZancudoSpawns(out finalSpawnpoint, out finalHeading);
            if (cCallout == "GrandTheftAuto") GrandTheftAutoSpawnpoints.ZancudoSpawns(out finalSpawnpoint, out finalHeading, out finalVicSpawnpoint, out finalVicHeading);
        }

        private static void Vespucci(in string cCallout)
        {
            if (cCallout == "VehiclePursuit") VehiclePursuitSpawnpoints.ZancudoSpawns(out finalSpawnpoint, out finalHeading);
            if (cCallout == "GrandTheftAuto") GrandTheftAutoSpawnpoints.ZancudoSpawns(out finalSpawnpoint, out finalHeading, out finalVicSpawnpoint, out finalVicHeading);
        }
    }
}
