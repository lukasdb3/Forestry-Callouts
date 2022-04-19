using System.Collections.Generic;
using System.Linq;
using Rage;

namespace ForestryCallouts2.Backbone.SpawnSystem.Land
{
    internal static class ChunkLoader
    {
        internal static List<Vector3> chunklist = new List<Vector3>(); //All chunks
        
        //Vector3 in the middle of each chunk
        internal static Vector3 chunk1 = new Vector3();
        internal static Vector3 chunk2 = new Vector3();
        internal static Vector3 chunk3 = new Vector3();
        internal static Vector3 chunk4 = new Vector3();
        internal static Vector3 chunk5 = new Vector3();
        internal static void Main()
        {
            //Adds chunks to a list
            chunklist.Add(chunk1);
            chunklist.Add(chunk2);
            chunklist.Add(chunk3);
            chunklist.Add(chunk4);
            chunklist.Add(chunk5);
        }
    }
}