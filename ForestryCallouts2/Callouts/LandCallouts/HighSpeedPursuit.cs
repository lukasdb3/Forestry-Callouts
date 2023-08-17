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
    
    [CalloutInterface("High Speed Pursuit", CalloutProbability.Medium, "Pursuit", "Code 3", "SASP")]
    
    internal class HighSpeedPursuit : Callout
    {
        #region Variables

        internal readonly string CurCall = "HighSpeedPursuit";
        
        //suspect variables
        private Ped _suspect;
        private Blip _suspectBlip;
        private Vector3 _suspectSpawn;
        private float _suspectHeading;
        private Vehicle _susVehicle;
        //passenger variables
        private GameFiber _fiber;
        private List<Ped> _passengerList = new();
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
            CalloutMessage = ("~g~High Speed Offroad Pursuit In Progress");
            CalloutPosition = _suspectSpawn;
            AddMinimumDistanceCheck(IniSettings.MinCalloutDistance, CalloutPosition);
            CalloutAdvisory = ("~b~Dispatch:~w~ We need backup for a high speed pursuit in progress. Respond code 3.");
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
            Logger.CallDebugLog(this, "Callout accepted");
            //Spawn Suspect and car
            CFunctions.SpawnCountryPed(out _suspect, _suspectSpawn, _suspectHeading);
            Vector3 vehicleSpawn = World.GetNextPositionOnStreet(_suspectSpawn);
            CFunctions.SpawnFastOffroadCar(out _susVehicle, vehicleSpawn, _suspectHeading);
            //Spawn possible passenger
            var pChoice = _rand.Next(1, 3);
            if (pChoice == 1)
            {
                if (_susVehicle.FreePassengerSeatsCount >= 1)
                {
                    var rnd = _rand.Next(1, _susVehicle.FreePassengerSeatsCount + 1);
                    Logger.CallDebugLog(this, "Free Passenger Seats Count " + _susVehicle.FreePassengerSeatsCount);
                    Logger.CallDebugLog(this, "Spawning "+ rnd + " criminal passengers");
                    _fiber = GameFiber.StartNew(delegate
                    {
                        var i = 0;
                        while (i != rnd)
                        {
                            if (i > 4) break;
                            GameFiber.Yield();
                            var cped = new Ped();
                            Logger.CallDebugLog(this, "Creating Passenger..");
                            CFunctions.SpawnCountryPed(out cped, _suspectSpawn, 0);
                            cped.WarpIntoVehicle(_susVehicle, i);
                            _passengerList.Add(cped);
                            i += 1;   
                        }
                        Logger.CallDebugLog(this, "There is " + _passengerList.Count.ToString() + " passengers");
                        Logger.CallDebugLog(this, "Aborting passenger fiber");
                        _fiber.Abort();
                    });
                }    
            }
            else
            {
                Logger.CallDebugLog(this, "No passengers spawning");
            }
            
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
                    foreach (Ped passenger in _passengerList)
                    {
                        Logger.CallDebugLog(this, "passenger added to pursuit");
                        Functions.AddPedToPursuit(_pursuit, passenger);
                    }
                    Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                    _pursuitStarted = true;
                }
            }
            
            //End Callout
            if (CFunctions.IsKeyAndModifierDown(IniSettings.EndCalloutKey, IniSettings.EndCalloutKeyModifier))
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
            foreach (Ped passenger in _passengerList)
            {
                if (passenger) passenger.Dismiss();
            }
            if (Functions.IsPursuitStillRunning(_pursuit) && _pursuitStarted) Functions.ForceEndPursuit(_pursuit);
            if (!ChunkChooser.StoppingCurrentCall)
            {
                Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 GP_CODE4_01", _suspectSpawn);
                if (IniSettings.EndNotfiMessages) Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "Status", "~g~High Speed Pursuit Code 4", "");
                CalloutInterfaceAPI.Functions.SendMessage(this, "Unit "+IniSettings.Callsign+" reporting High Speed Pursuit code 4");
            }
            Logger.CallDebugLog(this, "Callout ended");
            base.End();
        }
    }
}