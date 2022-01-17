using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using System.Drawing;
using SAHighwayCallouts.Functions.SpawnStuff;
using SAHighwayCallouts.Functions.SpawnStuff.CalloutSpawnpoints;
using SAHighwayCallouts.Functions;
using SAHighwayCallouts.Ini;
using UltimateBackup.API;
using Rage.Native;
using SAHighwayCallouts.Functions.Logger;

namespace SAHighwayCallouts.Callouts
{
    [CalloutInfo("AbandonVehicle", CalloutProbability.Medium)]
    
    internal class AbandonVehicle : Callout
    {
        #region variables

        private string callout = "AbandonVehicle";
        private string cCounty;
        
        //Stuff for callout
        private Vector3 spawnpoint;
        private float heading;
        private Vehicle vehicle;
        private Blip vehBlip;
        private Vector3 SearchArea;
        private Color vehColor;
        private string VehNameColor;

        private bool vehLocationSent;
        private bool vehicleFound;

        private string vehStreet;
        private string vehModel;
        private string vehDirection;
        
        private int notfiRan;
        private bool notfiRanMax;

        private bool displayEndHelpSent;
        
        //timer stuff
        private int timer;
        private bool timerPaused;
        #endregion

        public override bool OnBeforeCalloutDisplayed()
        {
            CalloutMessage = "~o~Abandon Vehicle";
            CalloutAdvisory = "~b~Dispatch:~w~ Car left on the side of the highway, respond Code 2";
            SpawnChunks.ChunkGetter(in callout, out cCounty);
            spawnpoint = SpawnChunks.finalSpawnpoint;
            heading = SpawnChunks.finalHeading;
            
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 30f);
            AddMinimumDistanceCheck(100f, spawnpoint);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition(
                "ATTENTION_ALL_UNITS_01 WE_HAVE_01 UNITS_RESPOND_CODE_02_01", spawnpoint); //Change this to something better.
            CalloutPosition = spawnpoint;
            LFunctions.BasicLogger(callout, "Callout displayed!");
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            LFunctions.BasicLogger(callout, "Callout accepted!");
            SAHC_Functions.SpawnNormalCar(out vehicle, spawnpoint, heading);
            SAHC_Functions.ColorPicker(out vehColor, out VehNameColor);
            
            vehicle.PrimaryColor = vehColor;
            vehStreet = World.GetStreetName(vehicle.Position);
            vehModel = vehicle.Model.Name.ToUpper();
            
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            //Timer Process
            if (!timerPaused)
            {
                timer++;
            }

            if (timer == 1 && !vehLocationSent)
            {
                LFunctions.BasicLogger(callout, "First search notification sent");
                Game.DisplayNotification("Look for the ~r~Abandoned Vehicle~w~.");
                SAHC_Functions.GetVehicleDirection(in vehicle, out vehDirection);
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition(
                    "ATTENTION_ALL_UNITS_01 SUSPECT_LAST_SEEN_01 IN_OR_ON_POSITION", vehicle.Position);
                Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "~b~VEHICLE LOCATION",                                                               
                    "~p~MODEL:~w~ " + vehModel + "",                                                                                                             
                    "~b~PRIMARY COLOR:~w~ " + VehNameColor.ToUpper() + "~n~ ~o~STREET:~w~ " + vehStreet.ToUpper() + "~n~ ~b~DIRECTION:~w~ " + vehDirection + "");
                vehLocationSent = true;
            }

            if (timer == 1250 && vehLocationSent && notfiRan != 15)
            {
                LFunctions.BasicLogger(callout, "Search notification sent ("+notfiRan+") times");
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition(                   
                    "ATTENTION_ALL_UNITS_01 SUSPECT_LAST_SEEN_01 IN_OR_ON_POSITION", vehicle.Position);
                Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "~b~VEHICLE LOCATION",                                                               
                    "~p~MODEL:~w~ " + vehModel + "",                                                                                                             
                    "~b~PRIMARY COLOR:~w~ " + VehNameColor.ToUpper() + "~n~ ~o~STREET:~w~ " + vehStreet.ToUpper() + "~n~ ~b~DIRECTION:~w~ " + vehDirection + "");
                notfiRan++;
                timer = 0;
            }

            if (timer == 1250 && notfiRan == 15 && !notfiRanMax)
            {
                LFunctions.BasicLogger(callout, "Search notification sent ("+notfiRan+") times, added search blip!");
                var position = vehicle.Position;
                SearchArea = position.Around2D(10f, 35f);
                vehBlip = new Blip(SearchArea, 65f) { Color = Color.Yellow, Alpha = .5f };

                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition(                   
                    "ATTENTION_ALL_UNITS_01 SUSPECT_LAST_SEEN_01 IN_OR_ON_POSITION", vehicle.Position);
                Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "~b~VEHICLE LOCATION",                 
                    "~p~MODEL:~w~ " + vehModel + "",                                                               
                    "~b~PRIMARY COLOR:~w~ " + VehNameColor.ToUpper() + "~n~ ~o~STREET:~w~ " + vehStreet.ToUpper() + "~n~ ~b~DIRECTION:~w~ " + vehDirection + "");
                notfiRanMax = true;
                timer = 0;
            }

            if (timer == 1250 && notfiRanMax && !vehicleFound)
            {
                LFunctions.BasicLogger(callout, "Search notification sent ("+notfiRan+") times, above average!"); 
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition(                                                                              
                    "ATTENTION_ALL_UNITS_01 SUSPECT_LAST_SEEN_01 IN_OR_ON_POSITION", vehicle.Position);                                                           
                Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "~b~VEHICLE LOCATION",                                                                
                    "~p~MODEL:~w~ " + vehModel + "",                                                                                                              
                    "~b~PRIMARY COLOR:~w~ " + VehNameColor.ToUpper() + "~n~ ~o~STREET:~w~ " + vehStreet.ToUpper() + "~n~ ~b~DIRECTION:~w~ " + vehDirection + "");
                timer = 0;
            }

            if (!vehicleFound && Game.LocalPlayer.Character.DistanceTo(vehicle) < 15f)
            {
                vehicleFound = true;
                if (Settings.HelpBlips)
                {
                    if (vehBlip) vehBlip.Delete();
                    vehBlip = vehicle.AttachBlip();
                }
                Game.DisplayHelp("Take appropriate action for the vehicle.");
                timer = 0;
            }
            
            if (timer == 2000 && vehicleFound && !displayEndHelpSent)                                                                                        
            {
                if (Settings.EnableEndCalloutHelpMessages)
                {
                    Game.DisplayHelp("Press ~r~'"+Settings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                }
                displayEndHelpSent = true;
            }
            
            if (Game.IsKeyDown(Settings.InputEndCalloutKey))
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition(
                    "OFFICERS_REPORT_03 OP_CODE OP_4", spawnpoint);
                Game.DisplayNotification("~b~Dispatch:~w~ All Units, Abandon Vehicle Code 4");
                LFunctions.BasicLogger(callout, "Callout was force ended by player.");
                End();
            }
            base.Process();
        }

        public override void End()
        {
            if (vehicle) vehicle.Dismiss();
            if (vehBlip) vehBlip.Delete();
            LFunctions.BasicLogger(callout, "Cleaned up!");
            base.End();
        }
    }
}