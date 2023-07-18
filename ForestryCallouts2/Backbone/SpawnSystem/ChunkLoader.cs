#region Refrences
//System
using System.Collections.Generic;
using ForestryCallouts2.Backbone.IniConfiguration;
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
        internal static Vector3 AltruistCampArea = new Vector3(-1019.696f, 4949.0752f, 198.2460f);
        internal static Vector3 RatonCanyon = new Vector3(-1365.854f, 4375.6421f, 40.04f);
        internal static Vector3 RatonCanyonUpper = new Vector3(-713.2326f, 4425.2168f, 16.2168f);
        internal static Vector3 chunk5 = new Vector3(-532.092f, 5322.865f, 91.310f);

        internal static Vector3 PaletoBayCoast = new Vector3(-369.7004f, 6527.3716f, 1f);
        internal static void Land()
        {
            //Adds chunks to a list
            chunklist.Add(PaletoBayForest);
            chunklist.Add(AltruistCampArea);
            chunklist.Add(RatonCanyon);
            chunklist.Add(RatonCanyonUpper);
            chunklist.Add(chunk5);
        }

        internal static void Water()
        {
            chunklist.Add(PaletoBayCoast);
        }
    }
}