using Rage;

namespace ForestryCallouts2.Backbone.SpawnSystem
{
    internal class DistanceChecker
    {
        internal static float distance;
        //Checks the distance between the player and the closest chunk, if its to far current callout will be ended before even displayed.
        internal static void Main(in Vector3 closestChunk)
        {
           distance = closestChunk.DistanceTo(Game.LocalPlayer.Character.Position);
            
           //the distance between the chunk and the player is to far so we end the current callout.
            if (distance >= IniSettings.MaxDistance)
            {
                if (LSPD_First_Response.Mod.API.Functions.IsCalloutRunning())
                {
                    LSPD_First_Response.Mod.API.Functions.StopCurrentCallout();
                    Logger.Log("Current callout was ended due to closest chunk being out of the MaxDistance range");
                } 
            }
            //the distance between the chunk and the player is within the IniSettings.MaxDistance range.
            else
            {
                Logger.Log("Player is within Max Distance range of the closest chunk");
            }
        }
    }
}