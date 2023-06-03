#region Refrences
//System
using System.Linq;
using ForestryCallouts2.Backbone.IniConfiguration;
//Rage
using Rage;
//ForestryCallouts2
using ForestryCallouts2.Backbone.SpawnSystem.Land.CalloutSpawnpoints;
#endregion

namespace ForestryCallouts2.Backbone.SpawnSystem.Land
{
    internal class ChunkChooser
    {
        internal static  Vector3 ClosestChunk; //Closet land chunk to player
        internal static string Curcall;
        internal static bool CalloutForceEnded;
        
        #region Common
        internal static Vector3 FinalSpawnpoint;
        internal static float FinalHeading;
        #endregion

        internal static void Main(in string currentCallout)
        {
            Curcall = currentCallout;
            CalloutForceEnded = false;
            Vector3 playerPos = Game.LocalPlayer.Character.Position;

            //finds closest land chunk to the player
            ClosestChunk = ChunkLoader.chunklist.OrderBy(x => x.DistanceTo(playerPos)).FirstOrDefault();
            Logger.DebugLog("CHUNK CHOOSER","Closest land chunk: "+ClosestChunk+"");

            //Checks and makes sure the chunk is within the max distance range if not callout is ended.
            if (IniSettings.EnableDistanceChecker)
            {
                if (DistanceChecker.IsChunkToFar(ClosestChunk))
                {
                    CalloutForceEnded = true;
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
            if (ClosestChunk == ChunkLoader.PaletoBayForest) NPaletoBayForest(in Curcall);
            /*if (closestChunk == ChunkLoader.chunk2) Chunk2(in curcall);
            if (closestChunk == ChunkLoader.chunk3) Chunk3(in curcall);
            if (closestChunk == ChunkLoader.chunk4) Chunk4(in curcall);
            if (closestChunk == ChunkLoader.chunk5) Chunk5(in curcall); */ 
        }

        private static void NPaletoBayForest(in string currentCallout)
        {
            if (currentCallout is "IntoxicatedPerson" or "RegularPursuit" or "AnimalAttack" or "DirtBikePursuit" or "AtvPursuit") 
                Common.PaletoBayForest(out FinalSpawnpoint, out FinalHeading);
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