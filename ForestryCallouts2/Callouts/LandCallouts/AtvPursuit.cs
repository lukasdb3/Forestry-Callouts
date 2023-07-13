#region Refrences
//System
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
using ForestryCallouts2.Backbone.SpawnSystem;
using ForestryCallouts2.Backbone.SpawnSystem.Land;
#endregion

namespace ForestryCallouts2.Callouts.LandCallouts
{
    [CalloutInfo("AtvPursuit", CalloutProbability.Medium)]
     
    internal class AtvPursuit : Callout
    {
        #region Variables

        internal readonly string CurCall = "AtvPursuit";
        
        //suspect variables
        private Ped _suspect;
        private Blip _suspectBlip;
        private Vector3 _suspectSpawn;
        private float _suspectHeading;
        private Vehicle _susVehicle;
        //callout variables
        private LHandle _pursuit;
        private bool _pursuitStarted;
        #endregion
        
         public override bool OnBeforeCalloutDisplayed()
        {
            //Gets spawnpoints from closest chunk
            ChunkChooser.Main(in CurCall);
            _suspectSpawn = ChunkChooser.FinalSpawnpoint;
            _suspectHeading = ChunkChooser.FinalHeading;

            //Normal callout details
            ShowCalloutAreaBlipBeforeAccepting(_suspectSpawn, 30f);
            CalloutMessage = ("~g~ATV Pursuit In Progress");
            CalloutPosition = _suspectSpawn; 
            AddMinimumDistanceCheck(IniSettings.MinCalloutDistance, CalloutPosition);
            CalloutAdvisory = ("~b~Dispatch:~w~ We need backup for a ATV pursuit in progress. Respond code 3.");
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
            //Spawn Suspect and car
            CFunctions.SpawnCountryPed(out _suspect, _suspectSpawn, _suspectHeading);
            Vector3 vehicleSpawn = World.GetNextPositionOnStreet(_suspectSpawn);
            CFunctions.SpawnAtv(out _susVehicle, vehicleSpawn, _suspectHeading);
            //Warp suspect into vehicle and set a blip
            _suspect.WarpIntoVehicle(_susVehicle, -1);
            _suspectBlip = _suspect.AttachBlip();
            _suspectBlip.EnableRoute(Color.Yellow);
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            //Prevent crashes by not running anything in Process other than end methods
            if (!_pursuitStarted)
            {
                //When player gets close disable route and start the pursuit
                if (Game.LocalPlayer.Character.DistanceTo(_suspect) <= 300f && !_pursuitStarted)
                {
                    if (_suspectBlip) _suspectBlip.Delete();
                    _suspect.Tasks.CruiseWithVehicle(_susVehicle, 15f ,VehicleDrivingFlags.Emergency);
                    _pursuit = Functions.CreatePursuit();
                    Functions.SetPursuitIsActiveForPlayer(_pursuit, true);
                    Functions.AddPedToPursuit(_pursuit, _suspect);
                    Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                    _pursuitStarted = true;
                }
            }
            
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
            if (_suspect) _suspect.Dismiss();
            if (_suspectBlip) _suspectBlip.Delete();
            if (_susVehicle) _susVehicle.Dismiss();
            if (Functions.IsPursuitStillRunning(_pursuit)) Functions.ForceEndPursuit(_pursuit);
            if (!ChunkChooser.StoppingCurrentCall)
            {
                Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", _suspectSpawn);
                Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "Status", "~g~ATV Pursuit Code 4", "");
                if (PluginChecker.CalloutInterface) CFunctions.CISendMessage(this, "ATV Pursuit Code 4");
            }
            Logger.CallDebugLog(this, "Callout ended");
            base.End();
        }
    }
}