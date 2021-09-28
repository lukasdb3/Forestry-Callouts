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
        internal static void ChunkGetter(in string cCallout, out Vector3 finalSpawnpoint, out float finalHeading)
        { 
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

            if (!gettingClosetChunk)
            {
                gettingClosetChunk = true;
                Game.LogTrivial("-!!- SAHighwayCallouts - |" + cCallout + "| - getting closet chunk!");
                
                policeStations.Add(pbPoliceStation); //Paleto Bay station
                policeStations.Add(bCountyPoliceStation); //Blaine County station
                policeStations.Add(prisonPoliceStation); //Prison
                policeStations.Add(zancudoPoliceStation); //Zancudo
                policeStations.Add(lsCountyPoliceStation); //Los Santos County station

                closestStation = policeStations.OrderBy(x => x.DistanceTo(playerPos)).FirstOrDefault();
            }

            if (!gettingClosetSpawnpoint)
            {
                gettingClosetSpawnpoint = true;
                Game.LogTrivial("-!!- SAHighwayCallouts - |" + cCallout + "| - prepping to find closet spawnpoint!");
                
                if(pbPoliceStation == closestStation) PbCounty(in cCallout, out finalSpawnpoint, out finalHeading);
                if (bCountyPoliceStation == closestStation) BCounty(in cCallout, out finalSpawnpoint, out finalHeading); 
                if (lsCountyPoliceStation == closestStation) LsCounty(in cCallout, out finalSpawnpoint, out finalHeading);
                if (prisonPoliceStation == closestStation) Prison(in cCallout, out finalSpawnpoint, out finalHeading);
                if (zancudoPoliceStation == closestStation) Zancudo(in cCallout, out finalSpawnpoint, out finalHeading);
            }
            
            finalSpawnpoint = default;
            finalHeading = default;
        }
        //Paleto bay spawn chunk
        private static void PbCounty(in string cCallout, out Vector3 finalSpawnpoint, out float finalHeading)
        {
            finalSpawnpoint = default;
            finalHeading = default;
            if (cCallout == "LuxuryVehiclePursuit") VehiclePursuitSpawns.PbCountySpawns(out finalSpawnpoint, out finalHeading);
        }

        //Blaine County spawn chunk
        private static void BCounty(in string cCallout, out Vector3 finalSpawnpoint, out float finalHeading)
        {
            finalSpawnpoint = default;
            finalHeading = default;
            if (cCallout == "LuxuryVehiclePursuit") VehiclePursuitSpawns.BCountySpawns(out finalSpawnpoint, out finalHeading);
        }
        
        //Los Santos county spawn chunk
        private static void LsCounty(in string cCallout, out Vector3 finalSpawnpoint, out float finalHeading)
        {
            finalSpawnpoint = default;
            finalHeading = default;
            if (cCallout == "LuxuryVehiclePursuit") VehiclePursuitSpawns.LsCountySpawns(out finalSpawnpoint, out finalHeading);
        }

        private static void Prison(in string cCallout, out Vector3 finalSpawnpoint, out float finalHeading)
        {
            finalSpawnpoint = default;
            finalHeading = default;
            if (cCallout == "LuxuryVehiclePursuit") VehiclePursuitSpawns.PrisonSpawns(out finalSpawnpoint, out finalHeading);
        }

        private static void Zancudo(in string cCallout, out Vector3 finalSpawnpoint, out float finalHeading)
        {
            finalSpawnpoint = default;
            finalHeading = default;
            if (cCallout == "LuxuryVehiclePursuit") VehiclePursuitSpawns.ZancudoSpawns(out finalSpawnpoint, out finalHeading);
        }
    }
}
