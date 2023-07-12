#region Refrences
//System
using System;
using System.Collections.Generic;
using System.Drawing;
//Rage
using Rage;
//LSPDFR
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
//ForestryCallouts2
using ForestryCallouts2.Backbone;
using ForestryCallouts2.Backbone.Functions;
using ForestryCallouts2.Backbone.IniConfiguration;
using ForestryCallouts2.Backbone.SpawnSystem.Land;
#endregion

namespace ForestryCallouts2.Callouts.LandCallouts
{
    
    [CalloutInfo("NAME", CalloutProbability.Medium)]
    
    internal class NAME : Callout
    {
        #region Variables

        internal readonly string CurCall = "NAME";
        
        #endregion

        public override bool OnBeforeCalloutDisplayed()
        {
            //Gets spawnpoints from closest chunk
            ChunkChooser.Main(in CurCall);
            _suspectSpawn = ChunkChooser.FinalSpawnpoint;
            _suspectHeading = ChunkChooser.FinalHeading;

            //Normal callout details
            ShowCalloutAreaBlipBeforeAccepting(_suspectSpawn, 30f);
            CalloutMessage = ("~g~Pursuit In Progress");
            CalloutPosition = _suspectSpawn;
            AddMinimumDistanceCheck(IniSettings.MinCalloutDistance, CalloutPosition);
            CalloutAdvisory = ("~b~Dispatch:~w~ We need backup for a pursuit in progress. Respond code 3.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_02 CRIME_SUSPECT_ON_THE_RUN_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_03_01", _suspectSpawn);
            return base.OnBeforeCalloutDisplayed();
        }

        public override void OnCalloutDisplayed()
        {
            //Send info to callout interface
            if (PluginChecker.CalloutInterface) CFunctions.CISendCalloutDetails(this, "CODE 3", "SASP");
            Logger.CallDebugLog(this, "Callout displayed");
            base.OnCalloutDisplayed();
        }

        public override void OnCalloutNotAccepted()
        {
            if (PluginChecker.CalloutInterface) Functions.PlayScannerAudio("OTHER_UNITS_TAKING_CALL");

            base.OnCalloutNotAccepted();
        }
        public override bool OnCalloutAccepted()
        {
            Logger.CallDebugLog(this, "Callout accepted");
            
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            
            //End Callout
            if (Game.IsKeyDown(IniSettings.EndCalloutKey)) //If player presses "End" it will forcefully clean the callout up
            {
                Logger.CallDebugLog(this, "Callout was force ended by player");
                End();
            }
            if (Game.LocalPlayer.Character.IsDead)
            {
                Logger.CallDebugLog(this, "Player died callout ending");
                End();
            }
        }

        public override void End()
        {
            
            if (!ChunkChooser.CalloutForceEnded)
            {
                Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", _suspectSpawn);
                Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "Status", "~g~Pursuit Code 4", "");
                if (PluginChecker.CalloutInterface) CFunctions.CISendMessage(this, "Pursuit Code 4");
            }
            Logger.CallDebugLog(this, "Callout ended");
            base.End();
        }
    }
}