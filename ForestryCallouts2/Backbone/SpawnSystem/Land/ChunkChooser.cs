using System.Linq;
using Rage;
using ForestryCallouts2.Backbone.SpawnSystem.Land;
using ForestryCallouts2.Backbone.SpawnSystem.CalloutSpawnpoints;

namespace ForestryCallouts2.Backbone.SpawnSystem.Land
{
    internal class ChunkChooser
    {
        internal static  Vector3 closestChunk = new Vector3(); //Closet land chunk to player
        
        #region Common
        internal static Vector3 finalSpawnpoint;
        internal static float finalHeading;
        #endregion

        internal static void Main(in string currentCallout)
        {
            //Main
            Vector3 playerPos = Game.LocalPlayer.Character.Position;

            //finds closest land chunk to the player
            closestChunk = ChunkLoader.chunklist.OrderBy(x => x.DistanceTo(playerPos)).FirstOrDefault();
            Logger.Log("Closest land chunk: "+closestChunk+"");

            //Checks and makes sure the chunk is within the max distance range if not callout is ended.
            DistanceChecker.Main(closestChunk);

            if (closestChunk == ChunkLoader.chunk1) Chunk1(currentCallout);
            if (closestChunk == ChunkLoader.chunk2) Chunk2(currentCallout);
            if (closestChunk == ChunkLoader.chunk3) Chunk3(currentCallout);
            if (closestChunk == ChunkLoader.chunk4) Chunk4(currentCallout);
            if (closestChunk == ChunkLoader.chunk5) Chunk5(currentCallout);
        }
        
        private static void Chunk1(in string currentCallout)
        {
            if (currentCallout == "IntoxicatedPerson") Common.Chunk1(out finalSpawnpoint, out finalHeading);
        }
        
        private static void Chunk2(in string currentCallout)
        {
            if (currentCallout == "IntoxicatedPerson") Common.Chunk1(out finalSpawnpoint, out finalHeading);     
        }
        
        private static void Chunk3(in string currentCallout)
        {
            if (currentCallout == "IntoxicatedPerson") Common.Chunk1(out finalSpawnpoint, out finalHeading);
        }
        
        private static void Chunk4(in string currentCallout)
        {
            if (currentCallout == "IntoxicatedPerson") Common.Chunk1(out finalSpawnpoint, out finalHeading);
        }
        
        private static void Chunk5(in string currentCallout)
        {
            if (currentCallout == "IntoxicatedPerson") Common.Chunk1(out finalSpawnpoint, out finalHeading); 
        }
    }
}