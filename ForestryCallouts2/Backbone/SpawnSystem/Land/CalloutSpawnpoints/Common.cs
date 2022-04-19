using System;
using Rage;

namespace ForestryCallouts2.Backbone.SpawnSystem.CalloutSpawnpoints
{
    internal class Common
    {
        internal static void Chunk1(out Vector3 spawnpoint, out float heading)
        {
            spawnpoint = default;
            heading = 0f;
            int var = new Random().Next(1, 3);
            switch (var)
            {
                case 1:
                    spawnpoint = new Vector3();
                    heading = 0f;
                    break;
            }
        }
    }
}