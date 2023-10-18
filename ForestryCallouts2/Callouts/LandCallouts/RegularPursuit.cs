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
using ForestryCallouts2.Backbone.SpawnSystem;
using ForestryCallouts2.Backbone.SpawnSystem.Land;
//CalloutInterface
using CalloutInterfaceAPI;
using Functions = LSPD_First_Response.Mod.API.Functions;

#endregion

namespace ForestryCallouts2.Callouts.LandCallouts
{
    
    [CalloutInterface("FCPursuit", CalloutProbability.Medium, "Pursuit", "Code 3", "SASP")]
    
    internal class RegularPursuit : Callout
    {
        #region Variables

        internal readonly string CurCall = "RegularPursuit";
        
        //suspect variables
        private Ped _suspect;
        private Blip _suspectBlip;
        private Vector3 _suspectSpawn;
        private float _suspectHeading;
        private Vehicle _susVehicle;
        //cop
        private Ped _cop;
        private Vehicle _copCar;
        //passenger variables
        private readonly List<Ped> _passengerList = new();
        //callout variables
        private LHandle _pursuit;
        private bool _pursuitStarted;
        private Random _rand = new();
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

        public override void OnCalloutNotAccepted()
        {
            Functions.PlayScannerAudio("OTHER_UNITS_TAKING_CALL");
            base.OnCalloutNotAccepted();
        }
        public override bool OnCalloutAccepted()
        {
            Log.CallDebug(this, "Callout accepted");
            //Spawn Suspect and car
            CFunctions.SpawnCountryPed(out _suspect, _suspectSpawn, _suspectHeading);
            Vector3 vehicleSpawn = World.GetNextPositionOnStreet(_suspectSpawn);
            CFunctions.SpawnOffroadCar(out _susVehicle, vehicleSpawn, _suspectHeading);
            if (IniSettings.AICops)
            {
                _cop = new Ped("s_f_y_ranger_01", World.GetNextPositionOnStreet(_suspectSpawn.Around(15f, 20f)), 0f);
                CFunctions.SpawnRangerBackup(out _copCar, World.GetNextPositionOnStreet(_suspectSpawn.Around(10f, 15f)), _susVehicle.Heading);
                _cop.WarpIntoVehicle(_copCar, -1);
                _cop.Tasks.CruiseWithVehicle(-1);
            }
            //Spawn possible passenger
            CFunctions.GetVehiclePassengers(_susVehicle, _passengerList, _suspectSpawn);
            
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
                    if (IniSettings.AICops) Functions.AddCopToPursuit(_pursuit, _cop);
                    Functions.AddPedToPursuit(_pursuit, _suspect);
                    foreach (Ped passenger in _passengerList)
                    {
                        Log.CallDebug(this, "passenger added to pursuit");
                        Functions.AddPedToPursuit(_pursuit, passenger);
                    }
                    Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                    _pursuitStarted = true;
                }
            }
            
            //End Callout
            if (CFunctions.IsKeyAndModifierDown(IniSettings.EndCalloutKey, IniSettings.EndCalloutKeyModifier))
            {
                Log.CallDebug(this, "Callout was force ended by player");
                End();
            }
            if (Game.LocalPlayer.Character.IsDead)
            {
                Log.CallDebug(this, "Player died callout ending");
                End();
            }
        }

        public override void End()
        {
            if (_suspect) _suspect.Dismiss();
            if (_suspectBlip) _suspectBlip.Delete();
            if (_susVehicle) _susVehicle.Dismiss();
            if (_cop) _cop.Dismiss();
            if (_copCar) _copCar.Dismiss();
            foreach (Ped passenger in _passengerList)
            {
                if (passenger) passenger.Dismiss();
            }
            if (_pursuitStarted) if (Functions.IsPursuitStillRunning(_pursuit)) Functions.ForceEndPursuit(_pursuit);
            if (!ChunkChooser.StoppingCurrentCall)
            {
                Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 GP_CODE4_01", _suspectSpawn);
                if (IniSettings.EndNotfiMessages) Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "Status", "~g~Regular Pursuit Code 4", "");
                CalloutInterfaceAPI.Functions.SendMessage(this, "Unit "+IniSettings.Callsign+" reporting Regular Pursuit code 4");
            }
            Log.CallDebug(this, "Callout ended");
            base.End();
        }
    }
}