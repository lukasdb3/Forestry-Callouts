using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using System.Drawing;
using System;
using ForestryCallouts2.Backbone;
using ForestryCallouts2.Backbone.Functions;
using ForestryCallouts2.Backbone.IniConfiguration;
using ForestryCallouts2.Backbone.SpawnSystem.Land;

namespace ForestryCallouts2.Callouts.LandCallouts
{
    [CalloutInfo("IntoxicatedPerson", CalloutProbability.Medium)]

    internal class IntoxicatedPerson : Callout
    {
        
        #region Variables
        internal string CurCall = "IntoxicatedPerson";

        private bool onScene;

        //suspect variables
        private Ped suspect;
        private Vector3 suspectSpawn;
        private float suspectHeading;
        private Blip suspectBlip;
        //pursuit handle
        
        
        //timer variables
        private int timer = 0;
        private bool pauseTimer;
        
        //search area variables
        private Blip suspectAreaBlip;
        private Vector3 searchArea;
        private bool suspectFound;
        private bool maxNotfiSent;
        
        private bool firstBlip;
        private int notfiSentCount;

        #endregion
        public override bool OnBeforeCalloutDisplayed()
        {
            //Code to get the spawnpoints for the call
            ChunkChooser.Main(in CurCall);
            suspectSpawn = ChunkChooser.finalSpawnpoint;
            suspectHeading = ChunkChooser.finalHeading;
            
            //Normal callout details
            ShowCalloutAreaBlipBeforeAccepting(suspectSpawn, 30f);
            AddMinimumDistanceCheck(100f, suspectSpawn);
            CalloutMessage = ("~g~Intoxicated Person Reported");
            CalloutPosition = suspectSpawn; 
            CalloutAdvisory = ("~b~Dispatch:~w~ Intoxicated Person reported, Respond Code 2");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT_02 CRIME_DISTURBING_THE_PEACE_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_02_02", suspectSpawn);
            return base.OnBeforeCalloutDisplayed();
        }

        public override void OnCalloutDisplayed()
        {
            //Send callout info to Callout Interface
            if (PluginChecker.CalloutInterface) CFunctions.SendCalloutDetails(this, "CODE 2", "SAPR");
            Logger.CallDebugLog(this, "Callout displayed");
            base.OnCalloutDisplayed();
        }
        
        public override void OnCalloutNotAccepted()
        {
            if (PluginChecker.CalloutInterface) LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("OTHER_UNITS_TAKING_CALL");

            base.OnCalloutNotAccepted();
        }

        public override bool OnCalloutAccepted()
        {
            Logger.CallDebugLog(this, "Callout accepted");
            //Spawn the suspect
            CFunctions.SpawnHikerPed(out suspect, suspectSpawn, suspectHeading);
            CFunctions.SetDrunk(suspect, true);
            //Sets a blip on the suspects head and enables route
            suspectBlip = suspect.AttachBlip();
            suspectBlip.EnableRoute(Color.Yellow);
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            //If the player is 200 or closer delete route and blip
            if (Game.LocalPlayer.Character.DistanceTo(suspect) <= 200f && !onScene)
            {
                suspect.Tasks.Wander();
                Logger.CallDebugLog(this, "Process started");
                onScene = true;
                if (suspectBlip) suspectBlip.Delete();
                firstBlip = true;
            }
            
            //If suspect isn't found initialize the search area
            if (!suspectFound && onScene)
            {
                if (!pauseTimer) timer++;
                
                if (firstBlip && timer >= 1 || timer >= 1250)
                {
                    if (suspectAreaBlip) suspectAreaBlip.Delete();
                    var position = suspect.Position;    
                    searchArea = position.Around2D(10f, 50f);
                    suspectAreaBlip = new Blip(searchArea, 65f) { Color = Color.Yellow, Alpha = .5f };
                    notfiSentCount++;
                    Logger.CallDebugLog(this, "Search areas sent: "+notfiSentCount+"");
                    firstBlip = false;
                    timer = 0;
                }
                
                //we delete the search area, and blip the suspect because the player is taking to long to find the suspect
                if (notfiSentCount == IniSettings.SearchAreaNotifis && !maxNotfiSent)
                {
                    //Pause the timer so search blips dont keep coming in
                    pauseTimer = true;
                    if (suspectAreaBlip) suspectAreaBlip.Delete();
                    suspectBlip = suspect.AttachBlip();
                    suspectBlip.Color = Color.Red;
                    suspectBlip.IsRouteEnabled = true; 
                    maxNotfiSent = true;
                }
            }
            
            //player found the intoxicated ped
            if (!suspectFound && Game.LocalPlayer.Character.DistanceTo(suspect) <= 10f)
            {
                suspectFound = true;
            }

            if (suspectFound)
            {
                Game.DisplaySubtitle("NOICE");
            }
                
                
            //End Shit
            if (Game.IsKeyDown(IniSettings.InputEndCalloutKey)) //If player presses "End" it will forcefully clean the callout up
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", suspectSpawn);
                Game.DisplayNotification("~g~Dispatch:~w~ All Units, Intoxicated Person Code 4");
                if (PluginChecker.CalloutInterface)
                {
                    CFunctions.SendMessage(this, "Intoxicated Person Code 4");
                }
                Logger.CallDebugLog(this, "Callout was force ended by player");
                End();
            }
            if (Game.LocalPlayer.Character.IsDead)
            {
                Logger.CallDebugLog(this, "Player died callout ending");
                End();
            }
            
            base.Process();
        }

        public override void End()
        {
            if (suspect) suspect.Dismiss();
            if (suspectBlip) suspectBlip.Delete();
            if (suspectAreaBlip) suspectAreaBlip.Delete();
            Logger.CallDebugLog(this, "Callout ended");
            base.End();
        }
    }
}