#region Refrences
//System
using System;
//Rage
using Rage;
#endregion

namespace ForestryCallouts2.Backbone.SpawnSystem.Water.CalloutSpawnpoints;

internal static class WaterCommon
{
    internal static void PaletoBayCoast(out Vector3 spawnpoint, out float heading)
            {
                Log.Debug("COMMON WATER SPAWNPOINT CHOOSER", "Choosing spawnpoint in PaletoBayCoast chunk");
                spawnpoint = default;
                heading = 0f;
                int var = new Random().Next(1, 2);
                Log.Debug("CASE", ""+var+"");
                switch (var)
                {
                    case 1:
                        spawnpoint = new Vector3(-346.7211f, 6782.1162f, 0.7159f);
                        heading = 31.7464f;
                        break;
                }
            }
}