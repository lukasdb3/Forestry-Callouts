#region Refrences
//System
using System.Linq;
//Rage
using Rage;
//ForestryCallouts2
using ForestryCallouts2.Backbone.IniConfiguration;
using ForestryCallouts2.Backbone.SpawnSystem.Land;
using ForestryCallouts2.Backbone.SpawnSystem.Land.CalloutSpawnpoints;
using ForestryCallouts2.Callouts.LandCallouts;

#endregion

namespace ForestryCallouts2.Backbone.SpawnSystem
{
    internal static class ChunkChooser
    {
        private static  Vector3 _closestChunk; //Closet land chunk to player
        private static string _curcall;
        internal static bool StoppingCurrentCall;
        
        #region Common
        internal static Vector3 FinalSpawnpoint;
        internal static float FinalHeading;
        #endregion

        internal static void Main(in string currentCallout)
        {
            _curcall = currentCallout;
            StoppingCurrentCall = false;
            Vector3 playerPos = Game.LocalPlayer.Character.Position;

            //finds closest land chunk to the player
            _closestChunk = ChunkLoader.chunklist.OrderBy(x => x.DistanceTo(playerPos)).FirstOrDefault();
            Logger.DebugLog("CHUNK CHOOSER","Closest land chunk: "+_closestChunk+"");

            //Checks and makes sure the chunk is within the max distance range if not callout is ended.
            if (IniSettings.EnableDistanceChecker)
            {
                if (DistanceChecker.IsChunkToFar(_closestChunk))
                {
                    StoppingCurrentCall = true;
                    LSPD_First_Response.Mod.API.Functions.StopCurrentCallout();
                    Logger.DebugLog("DISTANCE CHECKER", "Stopping current callout due to it being out of the max distance range");
                    Logger.DebugLog("DISTANCE CHECKER", "Selecting new callout to start");
                    CalloutsGetter.StartRandomCallout();
                }
                else
                {
                    Logger.DebugLog("DISTANCE CHECKER", "Player is in good range of the chunk");
                    CalloutSpawnSorter();
                }   
            }
            else CalloutSpawnSorter();
        }

        internal static void CalloutSpawnSorter()
        {
            //North paleto bay forest
            if (_closestChunk == ChunkLoader.PaletoBayForest) NPaletoBayForest(in _curcall);
            /*if (closestChunk == ChunkLoader.chunk2) Chunk2(in curcall);
            if (closestChunk == ChunkLoader.chunk3) Chunk3(in curcall);
            if (closestChunk == ChunkLoader.chunk4) Chunk4(in curcall);
            if (closestChunk == ChunkLoader.chunk5) Chunk5(in curcall); */ 
        }

        private static void NPaletoBayForest(in string currentCallout)
        {
            if (currentCallout is "IntoxicatedPerson" or "RegularPursuit" or "AnimalAttack" or "DirtBikePursuit" or "AtvPursuit" or "HighSpeedPursuit") 
                Common.PaletoBayForest(out FinalSpawnpoint, out FinalHeading);
            if (currentCallout is "LoggerTruckPursuit")
                LoggerTruckSpawns.PaletoBayForest(out FinalSpawnpoint, out FinalHeading);
            if (currentCallout is "DeadAnimalOnRoadway")
                DeadAnimalSpawnpoints.PaletoBayForest(out FinalSpawnpoint, out FinalHeading);
        }
        
        /*private static void Chunk2(in string currentCallout)
        {
            if (currentCallout == "IntoxicatedPerson") Common.nPaletoBayForest(out finalSpawnpoint, out finalHeading);     
        }
        
        private static void Chunk3(in string currentCallout)
        {
            if (currentCallout == "IntoxicatedPerson") Common.nPaletoBayForest(out finalSpawnpoint, out finalHeading);
        }
        
        private static void Chunk4(in string currentCallout)
        {
            if (currentCallout == "IntoxicatedPerson") Common.nPaletoBayForest(out finalSpawnpoint, out finalHeading);
        }
        
        private static void Chunk5(in string currentCallout)
        {
            if (currentCallout == "IntoxicatedPerson") Common.nPaletoBayForest(out finalSpawnpoint, out finalHeading); 
        }*/
    }
}