using System;
using Rage;

namespace ForestryCallouts2.Backbone.Functions
{
    internal static class  CFunctions
    {
        //Callout Interface Functions
        public static void SendCalloutDetails(LSPD_First_Response.Mod.Callouts.Callout sender, string priority, string agency)
        {
            try
            {
                CalloutInterface.API.Functions.SendCalloutDetails(sender, priority, agency);
            }
            catch (Exception ex)
            {
                Game.LogTrivial("-!!- Forestry Callouts ERROR - |SendCalloutDetails| - There was en error sending callout details with Callout Interface please send this log to https://dsc.gg/ulss -!!-");
                Game.LogTrivial(ex.Message);
            }
        }
        
        public static void SendMessage(LSPD_First_Response.Mod.Callouts.Callout sender, string message)
        {
            try
            {
                CalloutInterface.API.Functions.SendMessage(sender, message);
            }
            catch (Exception ex)
            {
                Game.LogTrivial("-!!- Forestry Callouts ERROR - |SendMessage| - There was en error sending a MDT message with Callout Interface please send this log to https://dsc.gg/ulss -!!-");
                Game.LogTrivial(ex.Message);
            }
        }
        
        internal static void SpawnHikerPed(out Ped cPed, Vector3 Spawnpoint, float heading)
        {
            String[] pedModels = { "a_m_y_hiker_01", "a_f_y_hiker_01" };
            cPed = new Ped(pedModels[new Random().Next(pedModels.Length)], Spawnpoint, heading);
            cPed.IsPersistent = true;
            cPed.BlockPermanentEvents = true;
        }
        
        internal static void SetDrunk(Ped Bad, bool isDrunk)
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                Bad.Metadata.stpAlcoholDetected = isDrunk;
                var drunkAnimset = new AnimationSet("move_m@drunk@verydrunk");
                drunkAnimset.LoadAndWait();
                Bad.MovementAnimationSet = drunkAnimset;
                Rage.Native.NativeFunction.Natives.SET_PED_IS_DRUNK(Bad, isDrunk);
            });
        }
        
        internal static void GetDirection(in Ped ped, out string direction)
        {
            direction = "null";
            double badHeading = ped.Heading;
            double heading = Math.Round(badHeading, 1);
            //   LESS THAN       GREATER THAN
            if (heading < 22.5f && heading > 337.5f) direction = "NORTH"; //North
            if (heading < 67.5f && heading > 22.5f) direction = "NORTH WEST"; //North West
            if (heading < 112.5f && heading > 67.5f) direction = "WEST"; //West
            if (heading < 157.5f && heading > 112.5f) direction = "SOUTH WEST"; //South West
            if (heading < 202.5f && heading > 157.5f) direction = "SOUTH"; //South
            if (heading < 247.5f && heading > 202.5f) direction = "SOUTH EAST"; //South East
            if (heading < 292.5f && heading > 247.5f) direction = "EAST"; //East
            if (heading < 337.5f && heading > 292.5f) direction = "NORTH EAST"; //North
            if (direction == "null") direction = "~r~NOT KNOWN";
        }
        
        internal static void GetVehicleDirection(in Vehicle vehicle, out string direction)
        {
            direction = "null";
            double badHeading = vehicle.Heading;
            double heading = Math.Round(badHeading, 1);
            //   LESS THAN       GREATER THAN
            if (heading < 22.5f && heading > 337.5f) direction = "NORTH"; //North
            if (heading < 67.5f && heading > 22.5f) direction = "NORTH WEST"; //North West
            if (heading < 112.5f && heading > 67.5f) direction = "WEST"; //West
            if (heading < 157.5f && heading > 112.5f) direction = "SOUTH WEST"; //South West
            if (heading < 202.5f && heading > 157.5f) direction = "SOUTH"; //South
            if (heading < 247.5f && heading > 202.5f) direction = "SOUTH EAST"; //South East
            if (heading < 292.5f && heading > 247.5f) direction = "EAST"; //East
            if (heading < 337.5f && heading > 292.5f) direction = "NORTH EAST"; //North
            if (direction == "null") direction = "~r~NOT KNOWN";
        }
    }
}