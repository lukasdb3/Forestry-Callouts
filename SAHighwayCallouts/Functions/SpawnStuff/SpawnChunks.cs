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
        //Used by all callouts
        internal static Vector3 finalSpawnpoint; //Final spawnpoint that will be used in callout
        internal static float finalHeading; //Final heading that will be used in callout
        
        //Used by: GrandTheftAuto
        internal static Vector3 finalVicSpawnpoint; //Final victim spawnpoint that will be used in callout
        internal static float finalVicHeading; //Final heading for victim that will be used in the callout

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
