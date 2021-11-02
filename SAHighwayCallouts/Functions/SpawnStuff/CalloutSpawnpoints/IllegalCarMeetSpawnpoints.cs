using System;
using System.Diagnostics;
using Rage;

namespace SAHighwayCallouts.Functions.SpawnStuff.CalloutSpawnpoints
{
    public class IllegalCarMeetSpawnpoints
    {
        internal static void PbCountySpawns(out Vector3 badSpawn1, out float badHeading1, out Vector3 badSpawn2, out float badHeading2, out Vector3 badSpawn3, out float badHeading3, out Vector3 badSpawn4,out float badHeading4, out Vector3 badSpawn5, out float badHeading5, out Vector3 badSpawn6, out float badHeading6, out Vector3 badSpawn7, out float badHeading7, out Vector3 badSpawn8, out float badHeading8, out Vector3 badSpawn9, out float badHeading9, out Vector3 badSpawn10, out float badHeading10)
        {
            #region Default
            badSpawn1 = default;
            badHeading1 = default;
            badSpawn2 = default;
            badHeading2 = default;
            badSpawn3 = default;
            badHeading3 = default;
            badSpawn4 = default;
            badHeading4 = default;
            badSpawn5 = default;
            badHeading5 = default;
            badSpawn6 = default;
            badHeading6 = default;
            badSpawn7 = default;
            badHeading7 = default;
            badSpawn8 = default;
            badHeading8 = default;
            badSpawn9 = default;
            badHeading9 = default;
            badSpawn10 = default;
            badHeading10 = default;
            #endregion

                int chose = new Random().Next(1, 2);
                Game.LogTrivial(
                    "-!!- SAHighwayCallouts - |GrandTheftAutoSpawnChooser| - Choosing Spawnpoint in Paleto Bay Chunk!");
                Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuitSpawnChooser| - Choosing Case: "+chose+" Out of 1 possible spawns!");
                switch (chose)
                {
                    case 1:
                        badSpawn1 = new Vector3();
                        badHeading1 = 0;
                        badSpawn2 = new Vector3();
                        badHeading2 = 0;
                        badSpawn3 = new Vector3();
                        badHeading3 = 0;
                        badSpawn4 = new Vector3();
                        badHeading4 = 0;
                        badSpawn5 = new Vector3();
                        badHeading5 = 0;
                        badSpawn6 = new Vector3();
                        badHeading6 = 0;
                        badSpawn7 = new Vector3();
                        badHeading7 = 0;
                        badSpawn8 = new Vector3();
                        badHeading8 = 0;
                        badSpawn9 = new Vector3();
                        badHeading9 = 0;
                        badSpawn10 = new Vector3();
                        badHeading10 = 0;
                        break;
                }

        }
    }
}