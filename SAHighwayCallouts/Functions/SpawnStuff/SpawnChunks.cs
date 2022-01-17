using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LSPD_First_Response.Mod.API;
using Rage;
using SAHighwayCallouts.Functions.SpawnStuff.CalloutSpawnpoints;
using SAHighwayCallouts.Functions.Logger;

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
            Vector3 rhPoliceStation = new Vector3(0f, 0f, 0f); //Rockford Hills police station
            Vector3 lmPoliceStation = new Vector3(0f, 0f, 0f); //LaMesa police station
            Vector3 dPoliceStation = new Vector3(0f, 0f, 0f); //Davis police station
            Vector3 mrPoliceStation = new Vector3(0f, 0f, 0f); //Mission Row police station

            if (!gettingClosetChunk)
            {
                gettingClosetChunk = true;
                LFunctions.BasicLogger("SpawnChunkSystem","Looking for closest chunk..");

                policeStations.Add(pbPoliceStation); //Paleto Bay station
                policeStations.Add(bCountyPoliceStation); //Blaine County station
                policeStations.Add(prisonPoliceStation); //Prison
                policeStations.Add(zancudoPoliceStation); //Zancudo
                policeStations.Add(lsCountyPoliceStation); //Los Santos County station
                policeStations.Add(vPoliceStation); //Vespucci police station
                policeStations.Add(rhPoliceStation); //Rockford Hills Police station
                policeStations.Add(lmPoliceStation); //LaMesa Police Station
                policeStations.Add(dPoliceStation); //Davis police station
                policeStations.Add(mrPoliceStation); //MissionRow Police station

                closestStation = policeStations.OrderBy(x => x.DistanceTo(playerPos)).FirstOrDefault();
                LFunctions.BasicLogger("SpawnChunkSystem","Closest chunk found!");
            }

            if (!gettingClosetSpawnpoint)
            {
                gettingClosetSpawnpoint = true;

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
                
                if (rhPoliceStation == closestStation)
                {
                    cCounty = "RockfordHills";
                    RockfordHills(in cCallout);
                }

                if (lmPoliceStation == closestStation)
                {
                    cCounty = "LaMesa";
                    LaMesa(in cCallout);
                }

                if (dPoliceStation == closestStation)
                {
                    cCounty = "Davis";
                    Davis(in cCallout);
                }

                if (mrPoliceStation == closestStation)
                {
                    cCounty = "MissionRow";
                    MissionRow(in cCallout);
                }
            }
        }

        //Paleto bay spawn chunk
        private static void PbCounty(in string cCallout)
        {
            if (cCallout == "SemiTruckPursuit") VehiclePursuitSpawnpoints.PbCountySpawns(out finalSpawnpoint, out finalHeading);
            if (cCallout == "LuxuryVehiclePursuit") VehiclePursuitSpawnpoints.PbCountySpawns(out finalSpawnpoint, out finalHeading);
            if (cCallout == "VehiclePursuit") VehiclePursuitSpawnpoints.PbCountySpawns(out finalSpawnpoint, out finalHeading);
            if (cCallout == "GrandTheftAuto") GrandTheftAutoSpawnpoints.PbCountySpawns(out finalSpawnpoint, out finalHeading, out finalVicSpawnpoint, out finalVicHeading);
            if (cCallout == "AbandonVehicle") VehiclePursuitSpawnpoints.PbCountySpawns(out finalSpawnpoint, out finalHeading);
        }

        //Blaine County spawn chunk
        private static void BCounty(in string cCallout)
        {
            if (cCallout == "SemiTruckPursuit") VehiclePursuitSpawnpoints.BCountySpawns(out finalSpawnpoint, out finalHeading);
            if (cCallout == "LuxuryVehiclePursuit") VehiclePursuitSpawnpoints.BCountySpawns(out finalSpawnpoint, out finalHeading);
            if (cCallout == "VehiclePursuit") VehiclePursuitSpawnpoints.BCountySpawns(out finalSpawnpoint, out finalHeading);
            if (cCallout == "GrandTheftAuto") GrandTheftAutoSpawnpoints.BCountySpawns(out finalSpawnpoint, out finalHeading, out finalVicSpawnpoint, out finalVicHeading);
            if (cCallout == "AbandonVehicle") VehiclePursuitSpawnpoints.BCountySpawns(out finalSpawnpoint, out finalHeading);
        }
        
        //Los Santos county spawn chunk
        private static void LsCounty(in string cCallout)
        {
            if (cCallout == "SemiTruckPursuit") VehiclePursuitSpawnpoints.LsCountySpawns(out finalSpawnpoint, out finalHeading);
            if (cCallout == "LuxuryVehiclePursuit") VehiclePursuitSpawnpoints.LsCountySpawns(out finalSpawnpoint, out finalHeading);
            if (cCallout == "VehiclePursuit") VehiclePursuitSpawnpoints.LsCountySpawns(out finalSpawnpoint, out finalHeading);
            if (cCallout == "GrandTheftAuto") GrandTheftAutoSpawnpoints.LsCountySpawns(out finalSpawnpoint, out finalHeading, out finalVicSpawnpoint, out finalVicHeading);
            if (cCallout == "AbandonVehicle") VehiclePursuitSpawnpoints.LsCountySpawns(out finalSpawnpoint, out finalHeading);
        }

        private static void Prison(in string cCallout)
        {
            if (cCallout == "SemiTruckPursuit") VehiclePursuitSpawnpoints.PrisonSpawns(out finalSpawnpoint, out finalHeading);
            if (cCallout == "LuxuryVehiclePursuit") VehiclePursuitSpawnpoints.PrisonSpawns(out finalSpawnpoint, out finalHeading);
            if (cCallout == "VehiclePursuit") VehiclePursuitSpawnpoints.PrisonSpawns(out finalSpawnpoint, out finalHeading);
            if (cCallout == "GrandTheftAuto") GrandTheftAutoSpawnpoints.PrisonSpawns(out finalSpawnpoint, out finalHeading, out finalVicSpawnpoint, out finalVicHeading);
            if (cCallout == "AbandonVehicle") VehiclePursuitSpawnpoints.PrisonSpawns(out finalSpawnpoint, out finalHeading);
        }

        private static void Zancudo(in string cCallout)
        {
            if (cCallout == "SemiTruckPursuit") VehiclePursuitSpawnpoints.ZancudoSpawns(out finalSpawnpoint, out finalHeading);
            if (cCallout == "LuxuryVehiclePursuit") VehiclePursuitSpawnpoints.ZancudoSpawns(out finalSpawnpoint, out finalHeading);
            if (cCallout == "VehiclePursuit") VehiclePursuitSpawnpoints.ZancudoSpawns(out finalSpawnpoint, out finalHeading);
            if (cCallout == "GrandTheftAuto") GrandTheftAutoSpawnpoints.ZancudoSpawns(out finalSpawnpoint, out finalHeading, out finalVicSpawnpoint, out finalVicHeading);
            if (cCallout == "AbandonVehicle") VehiclePursuitSpawnpoints.ZancudoSpawns(out finalSpawnpoint, out finalHeading);
        }

        private static void Vespucci(in string cCallout)
        {
            if (cCallout == "SemiTruckPursuit") VehiclePursuitSpawnpoints.Vespucci(out finalSpawnpoint, out finalHeading);
            if (cCallout == "LuxuryVehiclePursuit") VehiclePursuitSpawnpoints.Vespucci(out finalSpawnpoint, out finalHeading);
            if (cCallout == "VehiclePursuit") VehiclePursuitSpawnpoints.Vespucci(out finalSpawnpoint, out finalHeading);
            if (cCallout == "GrandTheftAuto") GrandTheftAutoSpawnpoints.Vespucci(out finalSpawnpoint, out finalHeading, out finalVicSpawnpoint, out finalVicHeading);
            if (cCallout == "AbandonVehicle") VehiclePursuitSpawnpoints.Vespucci(out finalSpawnpoint, out finalHeading);
        }
        
        private static void RockfordHills(in string cCallout)
        {
            if (cCallout == "SemiTruckPursuit") VehiclePursuitSpawnpoints.RockfordHills(out finalSpawnpoint, out finalHeading);
            if (cCallout == "LuxuryVehiclePursuit") VehiclePursuitSpawnpoints.RockfordHills(out finalSpawnpoint, out finalHeading);
            if (cCallout == "VehiclePursuit") VehiclePursuitSpawnpoints.RockfordHills(out finalSpawnpoint, out finalHeading);
            if (cCallout == "GrandTheftAuto") GrandTheftAutoSpawnpoints.RockfordHills(out finalSpawnpoint, out finalHeading, out finalVicSpawnpoint, out finalVicHeading);
            if (cCallout == "AbandonVehicle") VehiclePursuitSpawnpoints.RockfordHills(out finalSpawnpoint, out finalHeading);
        }
        
        private static void LaMesa(in string cCallout)
        {
            if (cCallout == "SemiTruckPursuit") VehiclePursuitSpawnpoints.LaMesa(out finalSpawnpoint, out finalHeading);
            if (cCallout == "LuxuryVehiclePursuit") VehiclePursuitSpawnpoints.LaMesa(out finalSpawnpoint, out finalHeading);
            if (cCallout == "VehiclePursuit") VehiclePursuitSpawnpoints.LaMesa(out finalSpawnpoint, out finalHeading);
            if (cCallout == "GrandTheftAuto") GrandTheftAutoSpawnpoints.LaMesa(out finalSpawnpoint, out finalHeading, out finalVicSpawnpoint, out finalVicHeading);
            if (cCallout == "AbandonVehicle") VehiclePursuitSpawnpoints.LaMesa(out finalSpawnpoint, out finalHeading);
        }
        
        private static void Davis(in string cCallout)
        {
            if (cCallout == "SemiTruckPursuit") VehiclePursuitSpawnpoints.Davis(out finalSpawnpoint, out finalHeading);
            if (cCallout == "LuxuryVehiclePursuit") VehiclePursuitSpawnpoints.Davis(out finalSpawnpoint, out finalHeading);
            if (cCallout == "VehiclePursuit") VehiclePursuitSpawnpoints.Davis(out finalSpawnpoint, out finalHeading);
            if (cCallout == "GrandTheftAuto") GrandTheftAutoSpawnpoints.Davis(out finalSpawnpoint, out finalHeading, out finalVicSpawnpoint, out finalVicHeading);
            if (cCallout == "AbandonVehicle") VehiclePursuitSpawnpoints.Davis(out finalSpawnpoint, out finalHeading);
        }
        
        private static void MissionRow(in string cCallout)
        {
            if (cCallout == "SemiTruckPursuit") VehiclePursuitSpawnpoints.MissionRow(out finalSpawnpoint, out finalHeading);
            if (cCallout == "LuxuryVehiclePursuit") VehiclePursuitSpawnpoints.MissionRow(out finalSpawnpoint, out finalHeading);
            if (cCallout == "VehiclePursuit") VehiclePursuitSpawnpoints.MissionRow(out finalSpawnpoint, out finalHeading);
            if (cCallout == "GrandTheftAuto") GrandTheftAutoSpawnpoints.MissionRow(out finalSpawnpoint, out finalHeading, out finalVicSpawnpoint, out finalVicHeading);
            if (cCallout == "AbandonVehicle") VehiclePursuitSpawnpoints.MissionRow(out finalSpawnpoint, out finalHeading);
        }
        
        
        
        
    }
}
