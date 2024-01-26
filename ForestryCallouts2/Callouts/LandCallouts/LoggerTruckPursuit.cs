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
    
    [CalloutInterface("[FC] LoggerTruckPursuit", CalloutProbability.Medium, "Pursuit", "Code 3", "SASP")]
    
    internal class LoggerTruckPursuit : FcCallout
    {
        #region Variables

        internal override string CurrentCall { get; set; } = "LoggerTruckPursuit";
        internal override string CurrentCallFriendlyName { get; set; } = "Logger Truck Pursuit";
        protected override Vector3 Spawnpoint { get; set; }
        
        //suspect variables
        private Ped _suspect;
        private Blip _suspectBlip;
        private float _suspectHeading;
        private Vehicle _susVehicle;
        private Vehicle _susVehTrailer;
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
            _suspectHeading = ChunkChooser.FinalHeading;

            //Normal callout details
            CalloutMessage = ("~g~Logger Truck Pursuit In Progress");
            CalloutAdvisory = ("~b~Dispatch:~w~ We need backup for a truck pursuit in progress. Respond code 3.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_02 CRIME_SUSPECT_ON_THE_RUN_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_03_01", Spawnpoint);
            return base.OnBeforeCalloutDisplayed();
        }
        
        public override bool OnCalloutAccepted()
        {
            Log.CallDebug(this, "Callout accepted");
            //Spawn Suspect and car
            CFunctions.SpawnCountryPed(out _suspect, Spawnpoint, _suspectHeading);
            Vector3 vehicleSpawn = World.GetNextPositionOnStreet(Spawnpoint);
            CFunctions.SpawnSemiTrucks(out _susVehicle, vehicleSpawn, _suspectHeading);
            _susVehicle.TopSpeed = 25f;
            _susVehTrailer = new Vehicle("trailerlogs", _susVehicle.GetOffsetPositionFront(10f), _suspectHeading);
            _susVehTrailer.IsPersistent = true;
            _susVehicle.Trailer = _susVehTrailer;
            if (IniSettings.AICops)
            {
                _cop = new Ped("s_f_y_ranger_01", World.GetNextPositionOnStreet(Spawnpoint.Around(15f, 20f)), 0f);
                CFunctions.SpawnRangerBackup(out _copCar, World.GetNextPositionOnStreet(Spawnpoint.Around(10f, 15f)), _susVehicle.Heading);
                _cop.WarpIntoVehicle(_copCar, -1);
                _cop.Tasks.CruiseWithVehicle(-1);
            }
            //Spawn possible passenger
            CFunctions.GetVehiclePassengers(_susVehicle, _passengerList, Spawnpoint);
            
            //Warp suspect into vehicle and set a blip
            _suspect.WarpIntoVehicle(_susVehicle, -1);
            _suspectBlip = CFunctions.CreateBlip(_suspect, true, Color.Yellow, Color.Yellow, 1f);
            _suspect.Tasks.CruiseWithVehicle(_susVehicle, -1, VehicleDrivingFlags.Emergency);
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
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
            base.Process();
        }

        public override void End()
        {
            if (_suspect) _suspect.Dismiss();
            if (_suspectBlip) _suspectBlip.Delete();
            if (_susVehicle) _susVehicle.Dismiss();
            if (_susVehTrailer) _susVehTrailer.Dismiss();
            if (_cop) _cop.Dismiss();
            if (_copCar) _copCar.Dismiss();
            foreach (Ped passenger in _passengerList)
            {
                if (passenger) passenger.Dismiss();
            }
            if (_pursuitStarted) if (Functions.IsPursuitStillRunning(_pursuit)) Functions.ForceEndPursuit(_pursuit);
            base.End();
        }
    }
}