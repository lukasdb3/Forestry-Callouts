using System.Collections.Generic;
using System.Linq;
using Rage;

namespace ForestryCallouts2.Backbone.SpawnSystem.Water
{
    internal static class ChunkLoader
    {
        internal static List<Vector3> WaterChunkList = new List<Vector3>(); //All water chunks
        
        //Vector3 in the middle of each water chunk
        internal static Vector3 chunk1 = new Vector3();
        internal static Vector3 chunk2 = new Vector3();
        internal static Vector3 chunk3 = new Vector3();
        internal static Vector3 chunk4 = new Vector3();
        internal static Vector3 chunk5 = new Vector3();
        internal static void Main()
        {
            //Adds chunks to a list
            WaterChunkList.Add(chunk1);
            WaterChunkList.Add(chunk2);
            WaterChunkList.Add(chunk3);
            WaterChunkList.Add(chunk4);
            WaterChunkList.Add(chunk5);
        }
    }
}