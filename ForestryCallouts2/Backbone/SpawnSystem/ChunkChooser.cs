#region Refrences
//System
using System.Linq;
//Rage
using Rage;
//ForestryCallouts2
using ForestryCallouts2.Backbone.IniConfiguration;
using ForestryCallouts2.Backbone.SpawnSystem.Land;
using ForestryCallouts2.Backbone.SpawnSystem.Land.CalloutSpawnpoints;
using ForestryCallouts2.Backbone.SpawnSystem.Water.CalloutSpawnpoints;
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
        #region AnimalOnRoadwayCall
        internal static Vector3 SafeOffroadPos;
        #endregion
        #region DeadBody
        internal static Vector3 DeadBodySpawn;
        internal static Vector3 ReporterSpawn;
        internal static float ReporterHeading;
        #endregion
        #region IllegalCampFire
        internal static Vector3 SuspectSpawn;
        internal static float SuspectHeading;
        internal static Vector3 FireSpawn;
        #endregion

        internal static void Main(in string currentCallout)
        {
            _curcall = currentCallout;
            StoppingCurrentCall = false;
            Vector3 playerPos = Game.LocalPlayer.Character.Position;

            //finds closest chunk to the player
            _closestChunk = ChunkLoader.chunklist.OrderBy(x => x.DistanceTo(playerPos)).FirstOrDefault();
            Logger.DebugLog("CHUNK CHOOSER","WaterCallouts = " + IniSettings.WaterCallouts);
            Logger.DebugLog("CHUNK CHOOSER","Closest chunk: "+_closestChunk+"");

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
            else
            {
                CalloutSpawnSorter();
            }
        }

        internal static void CalloutSpawnSorter()
        {
            if (!IniSettings.WaterCallouts)
            {
                if (_closestChunk == ChunkLoader.PaletoBayForest) PaletoBayForest(in _curcall);
                if (_closestChunk == ChunkLoader.AltruistCampArea) AltruistCampArea(in _curcall);
                if (_closestChunk == ChunkLoader.RatonCanyon || _closestChunk == ChunkLoader.RatonCanyonUpper || _closestChunk == ChunkLoader.RatonCanyonLower) RatonCanyon(in _curcall);
                /*if (closestChunk == ChunkLoader.chunk4) Chunk4(in curcall);
                if (closestChunk == ChunkLoader.chunk5) Chunk5(in curcall); */
            }
            else
            {
                if (_closestChunk == ChunkLoader.PaletoBayCoast) PaletoBayCoast(in _curcall);
            }
        }

        private static void PaletoBayForest(in string currentCallout)
        {
            switch (currentCallout)
            {
                case "IntoxicatedPerson" or "RegularPursuit" or "AnimalAttack" or "DirtBikePursuit" or "AtvPursuit" or "HighSpeedPursuit" or "DangerousPerson":
                    Common.PaletoBayForest(out FinalSpawnpoint, out FinalHeading);
                    break;
                case "LoggerTruckPursuit":
                    LoggerTruckSpawns.PaletoBayForest(out FinalSpawnpoint, out FinalHeading);
                    break;
                case "DeadAnimalOnRoadway":
                    DeadAnimalSpawnpoints.PaletoBayForest(out FinalSpawnpoint, out FinalHeading);
                    break;
                case "AnimalOnRoadway":
                    AnimalOnRoadwaySpawnpoints.PaletoBayForest(out FinalSpawnpoint, out FinalHeading, out SafeOffroadPos);
                    break;
                case "DeadBody":
                    DeadBodySpawnpoints.PaletoBayForest(out DeadBodySpawn, out ReporterSpawn, out ReporterHeading);
                    break;
                case "IllegalCampFire":
                    IllegalCampFireSpawnpoints.PaletoBayForest(out SuspectSpawn, out SuspectHeading, out FireSpawn);
                    break;

            }
        }
        
        private static void AltruistCampArea(in string currentCallout)
        {
            switch (currentCallout)
            {
                case "IntoxicatedPerson" or "RegularPursuit" or "AnimalAttack" or "DirtBikePursuit" or "AtvPursuit" or "HighSpeedPursuit" or "DangerousPerson":
                    Common.AltruistCampArea(out FinalSpawnpoint, out FinalHeading);
                    break;
                case "LoggerTruckPursuit":
                    LoggerTruckSpawns.AltruistCampArea(out FinalSpawnpoint, out FinalHeading);
                    break;
                case "DeadAnimalOnRoadway":
                    DeadAnimalSpawnpoints.AltruistCampArea(out FinalSpawnpoint, out FinalHeading);
                    break;
                case "AnimalOnRoadway":
                    AnimalOnRoadwaySpawnpoints.AltruistCampArea(out FinalSpawnpoint, out FinalHeading, out SafeOffroadPos);
                    break;
                case "DeadBody":
                    DeadBodySpawnpoints.AltruistCampArea(out DeadBodySpawn, out ReporterSpawn, out ReporterHeading);
                    break;
                case "IllegalCampFire":
                    IllegalCampFireSpawnpoints.AltruistCampArea(out SuspectSpawn, out SuspectHeading, out FireSpawn);
                    break;
            }
        }
        
        private static void RatonCanyon(in string currentCallout)
        {
            switch (currentCallout)
            {
                case "IntoxicatedPerson" or "RegularPursuit" or "AnimalAttack" or "DirtBikePursuit" or "AtvPursuit" or "HighSpeedPursuit" or "DangerousPerson":
                    Common.RatonCanyon(out FinalSpawnpoint, out FinalHeading);
                    break;
                case "LoggerTruckPursuit":
                    LoggerTruckSpawns.RatonCanyon(out FinalSpawnpoint, out FinalHeading);
                    break;
                case "DeadAnimalOnRoadway":
                    DeadAnimalSpawnpoints.RatonCanyon(out FinalSpawnpoint, out FinalHeading);
                    break;
                case "AnimalOnRoadway":
                    AnimalOnRoadwaySpawnpoints.RatonCanyon(out FinalSpawnpoint, out FinalHeading, out SafeOffroadPos);
                    break;
                case "DeadBody":
                    DeadBodySpawnpoints.RatonCanyon(out DeadBodySpawn, out ReporterSpawn, out ReporterHeading);
                    break;
                case "IllegalCampFire":
                    IllegalCampFireSpawnpoints.RatonCanyon(out SuspectSpawn, out SuspectHeading, out FireSpawn);
                    break;
            }
        }
        
        /*private static void Chunk4(in string currentCallout)
        {
            if (currentCallout == "IntoxicatedPerson") Common.nPaletoBayForest(out finalSpawnpoint, out finalHeading);
        }
        
        private static void Chunk5(in string currentCallout)
        {
            if (currentCallout == "IntoxicatedPerson") Common.nPaletoBayForest(out finalSpawnpoint, out finalHeading); 
        }*/

        private static void PaletoBayCoast(in string currentCallout)
        {
            switch (currentCallout)
            {
                case "DeadBodyWater":
                    DeadBodyWater.PaletoBayCoast(out FinalSpawnpoint);
                    break;
                case "BoatPursuit":
                    WaterCommon.PaletoBayCoast(out FinalSpawnpoint, out FinalHeading);
                    break;
            }
        }
    }
}