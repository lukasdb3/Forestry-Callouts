using ForestryCallouts2.Backbone.IniConfiguration;
using Rage;

namespace ForestryCallouts2.Backbone.SpawnSystem
{
    internal class DistanceChecker
    {
        private static float _distance;
        //Checks the distance between the player and the closest chunk, if its to far current callout will be ended before even displayed.
        internal static void Main(in Vector3 closestChunk)
        {
           _distance = closestChunk.DistanceTo(Game.LocalPlayer.Character.Position);
            
           //the distance between the chunk and the player is to far so we end the current callout.
            if (_distance > IniSettings.FinalDistance)
            {
                if (LSPD_First_Response.Mod.API.Functions.IsCalloutRunning())
                {
                    LSPD_First_Response.Mod.API.Functions.StopCurrentCallout();
                    Logger.DebugLog("DISTANCE CHECKER", "Stopping current callout due to it being out of MaxDistance range");
                    Logger.DebugLog("DISTANCE CHECKER", "Selecting new callout to start");
                    CalloutsGetter.StartRandomCallout();
                } 
            }
            //the distance between the chunk and the player is within the IniSettings.MaxDistance range.
            else
            {
                Logger.DebugLog("DISTANCE CHECKER", "Player is in good range of the callout");
                if (!IniSettings.WaterCalls) Land.ChunkChooser.CalloutSpawnSorter();
            }
        }
    }
}