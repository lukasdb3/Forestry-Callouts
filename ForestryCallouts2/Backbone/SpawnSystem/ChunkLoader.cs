﻿#region Refrences
//System
using System.Collections.Generic;
using Rage;
//Rage

#endregion

namespace ForestryCallouts2.Backbone.SpawnSystem
{
    internal static class ChunkLoader
    {
        internal static List<Vector3> chunklist = new List<Vector3>(); //All chunks
        
        //Vector3 in the middle of each chunk
        internal static Vector3 PaletoBayForest = new Vector3(-532.092f, 5322.865f, 91.310f);
        internal static Vector3 chunk2 = new Vector3(-532.092f, 5322.865f, 91.310f);
        internal static Vector3 chunk3 = new Vector3(-532.092f, 5322.865f, 91.310f);
        internal static Vector3 chunk4 = new Vector3(-532.092f, 5322.865f, 91.310f);
        internal static Vector3 chunk5 = new Vector3(-532.092f, 5322.865f, 91.310f);
        internal static void Main()
        {
            //Adds chunks to a list
            chunklist.Add(PaletoBayForest);
            chunklist.Add(chunk2);
            chunklist.Add(chunk3);
            chunklist.Add(chunk4);
            chunklist.Add(chunk5);
        }
    }
}