using System.Linq;
using Rage;
using ForestryCallouts2.Backbone.SpawnSystem.Land;
using ForestryCallouts2.Backbone.SpawnSystem.Land.CalloutSpawnpoints;

namespace ForestryCallouts2.Backbone.SpawnSystem.Land
{
    internal class ChunkChooser
    {
        internal static  Vector3 closestChunk = new Vector3(); //Closet land chunk to player
        internal static string curcall;
        
        #region Common
        internal static Vector3 finalSpawnpoint;
        internal static float finalHeading;
        #endregion

        internal static void Main(in string currentCallout)
        {
            curcall = currentCallout;
            //Main
            Vector3 playerPos = Game.LocalPlayer.Character.Position;

            //finds closest land chunk to the player
            closestChunk = ChunkLoader.chunklist.OrderBy(x => x.DistanceTo(playerPos)).FirstOrDefault();
            Logger.DebugLog("CHUNK CHOOSER","Closest land chunk: "+closestChunk+"");

            //Checks and makes sure the chunk is within the max distance range if not callout is ended.
            DistanceChecker.Main(closestChunk);
        }

        internal static void CalloutSpawnSorter()
        {
            //North paleto bay forest
            if (closestChunk == ChunkLoader.nPaletoBayForest) NPaletoBayForest(in curcall);
            /*if (closestChunk == ChunkLoader.chunk2) Chunk2(in curcall);
            if (closestChunk == ChunkLoader.chunk3) Chunk3(in curcall);
            if (closestChunk == ChunkLoader.chunk4) Chunk4(in curcall);
            if (closestChunk == ChunkLoader.chunk5) Chunk5(in curcall); */ 
        }

        private static void NPaletoBayForest(in string currentCallout)
        {
            //if (currentCallout == "IntoxicatedPerson")
            if (currentCallout == "IntoxicatedPerson") Land.CalloutSpawnpoints.Common.nPaletoBayForest(out finalSpawnpoint, out finalHeading);
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