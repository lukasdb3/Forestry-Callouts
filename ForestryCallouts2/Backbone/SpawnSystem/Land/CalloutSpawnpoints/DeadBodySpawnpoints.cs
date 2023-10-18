#region Refrences
//System
using System;
//Rage
using Rage;
#endregion


namespace ForestryCallouts2.Backbone.SpawnSystem.Land.CalloutSpawnpoints
{
    internal static class DeadBodySpawnpoints
    {                                                                                                                                                       
        internal static void PaletoBayForest(out Vector3 vSpawn, out Vector3 rSpawn, out float heading)
        {
            Log.Debug("DEAD BODY SPAWNPOINT CHOOSER", "Choosing spawnpoint in PaletoBayForest chunk");
            vSpawn = default;
            rSpawn = default;
            heading = 0f;
            var var = new Random().Next(1, 3);
            Log.Debug("CASE", ""+var+"");
            switch (var)
            {
                case 1:
                    vSpawn = new Vector3(-441.3585f, 5533.9849f, 71.8802f);
                    rSpawn = new Vector3(-461.7544f, 5525.8145f, 79.5952f);
                    heading = 10.9544f;
                    break;
            }
        }

        internal static void AltruistCampArea(out Vector3 vSpawn, out Vector3 rSpawn, out float heading)
        {
            Log.Debug("DEAD BODY SPAWNPOINT CHOOSER", "Choosing spawnpoint in AltruistCampArea chunk");
            vSpawn = default;
            rSpawn = default;
            heading = 0f;
            var var = new Random().Next(1, 2);
            Log.Debug("CASE", ""+var+"");
            switch (var)
            {
                case 1:
                    vSpawn = new Vector3();
                    rSpawn = new Vector3();
                    heading = 0f;
                    break;
            }
        }
        
        internal static void RatonCanyon(out Vector3 vSpawn, out Vector3 rSpawn, out float heading)
        {
            Log.Debug("DEAD BODY SPAWNPOINT CHOOSER", "Choosing spawnpoint in RatonCanyon chunk");
            vSpawn = default;
            rSpawn = default;
            heading = 0f;
            var var = new Random().Next(1, 13);
            Log.Debug("CASE", ""+var+"");
            switch (var)
            {
                case 1:
                    vSpawn = new Vector3();
                    rSpawn = new Vector3();
                    heading = 0f;
                    break;
            }
        }
    }
}