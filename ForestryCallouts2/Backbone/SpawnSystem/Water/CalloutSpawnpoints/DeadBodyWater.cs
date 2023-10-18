#region Refrences
//System
using System;
//Rage
using Rage;
#endregion

namespace ForestryCallouts2.Backbone.SpawnSystem.Water.CalloutSpawnpoints;

internal static class DeadBodyWater
{
    internal static void PaletoBayCoast(out Vector3 spawnpoint)
            {
                Log.Debug("DEAD BODY WATER SPAWNPOINT CHOOSER", "Choosing spawnpoint in PaletoBayCoast chunk");
                spawnpoint = default;
                int var = new Random().Next(1, 2);
                Log.Debug("CASE", ""+var+"");
                switch (var)
                {
                    case 1:
                        spawnpoint = new Vector3(-361.7248f, 6592.2275f, -0.5330f);
                        break;
                }
            }
}