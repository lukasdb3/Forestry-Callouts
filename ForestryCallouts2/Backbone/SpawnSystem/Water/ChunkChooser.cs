using System.Linq;
using Rage;
using ForestryCallouts2.Backbone.SpawnSystem.Water;
using ForestryCallouts2.Backbone.SpawnSystem.CalloutSpawnpoints;

namespace ForestryCallouts2.Backbone.SpawnSystem.Water
{
    internal class ChunkChooser
    {
        internal static Vector3 closestChunk = new Vector3(); //Closet water chunk to player
        
        #region Common
        internal static Vector3 finalSpawnpoint;
        internal static float finalHeading;
        #endregion

        internal static void Main(in string currentCallout)
        {
            //Main
            Vector3 playerPos = Game.LocalPlayer.Character.Position;

            //finds closest water chunk to the player
            closestChunk = ChunkLoader.WaterChunkList.OrderBy(x => x.DistanceTo(playerPos)).FirstOrDefault();
            Logger.Log("Closest water chunk: "+closestChunk+"");

            if (closestChunk == SpawnSystem.Water.ChunkLoader.chunk1) Chunk1(currentCallout);
            if (closestChunk == SpawnSystem.Water.ChunkLoader.chunk2) Chunk2(currentCallout);
            if (closestChunk == SpawnSystem.Water.ChunkLoader.chunk3) Chunk3(currentCallout);
            if (closestChunk == SpawnSystem.Water.ChunkLoader.chunk4) Chunk4(currentCallout);
            if (closestChunk == SpawnSystem.Water.ChunkLoader.chunk5) Chunk5(currentCallout);
        }
        
        private static void Chunk1(in string currentCallout)
        {
            if (currentCallout == "") ;
        }
        
        private static void Chunk2(in string currentCallout)
        {
            if (currentCallout == "") ;
        }
        
        private static void Chunk3(in string currentCallout)
        {
            if (currentCallout == "") ;
        }
        
        private static void Chunk4(in string currentCallout)
        {
            if (currentCallout == "") ;
        }
        
        private static void Chunk5(in string currentCallout)
        {
            if (currentCallout == "") ;
        }
    }
}