using ForestryCallouts2.Backbone.IniConfiguration;
using Rage;

namespace ForestryCallouts2.Backbone.SpawnSystem
{
    internal static class DistanceChecker
    {
        private static float _distance;
        //Checks the distance between the closest chunk and the player.
        internal static bool IsChunkToFar(in Vector3 closestChunk)
        {
           _distance = Game.LocalPlayer.Character.Position.DistanceTo(closestChunk);
            
            //returns true if the player is out farther than the max distance
            if (_distance > IniSettings.FinalDistance)
            {
                return true;
            }
            //returns false if the player is within the max distance range
            return false;
        }
    }
}