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


namespace SAHighwayCallouts.Callouts.PursuitCallouts
{
    [CalloutInfo("VehiclePursuit", CalloutProbability.Medium)]
    internal class VehiclePursuit : Callout
    {
        #region Variables

        private string callout = "VehiclePursuit";
        private string currentCounty = null;
        private Ped _suspect;
        private Ped _cop;
        private Ped _passenger;
        private Vehicle _susV;
        private Vehicle _copV;
        private Vehicle _susVTrailer;
        private Blip _susBlip;
        private float _heading;
        private Vector3 _spawnpoint;
        private Vector3 _copSpawmpoint;
        private Vector3 _finalCopSpawnpoint;
        private LHandle _pursuit;
        private int PursuitChooser = new Random().Next(1, 3);

        private bool _onScene;
        private bool _beforeOnScene;
        private bool _pursuitStarted;
        private bool _suspectDead;

        private int _passengerChooser = new Random().Next(1, 4);
        #endregion

        public override bool OnBeforeCalloutDisplayed()
        {
            if (PursuitChooser == 1)
            {
                //Luxury Vehicle Pursuit
                CalloutMessage = "~o~Luxury Vehicle Pursuit";
                CalloutAdvisory = "~b~Dispatch:~w~ Luxury stolen vehicle spotted, Respond ~r~Code 3~w~";
            }

            if (PursuitChooser == 2)
            {
                //Normal pursuit
                CalloutMessage = "~o~Pursuit In Progress";
                CalloutAdvisory = "~b~Dispatch:~w~ Vehicle pursuit in progress, Respond ~r~Code 3~w~";
            }

            if (PursuitChooser == 3)
            {
                //Semi truck pursuit
                CalloutMessage = "~o~Semi Truck Pursuit";
                CalloutAdvisory = "~b~Dispatch:~w~ Semi truck fleeing from Police, Respond ~r~Code 3~w~";
            }
            Game.LogTrivial("-!!- SAHighwayCallouts - |VehiclePursuit| - Pursuit chooser has chosen "+callout+"!");
            SpawnChunks.ChunkGetter(in callout, out currentCounty);
            _spawnpoint = SpawnChunks.finalSpawnpoint;
            _heading = SpawnChunks.finalHeading;

            ShowCalloutAreaBlipBeforeAccepting(_spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, _spawnpoint);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS_01 OFFICERS_REPORT_01 CRIME_RESIST_ARREST_01 UNITS_RESPOND_CODE_03_01", _spawnpoint);
            CalloutPosition = _spawnpoint;
            Game.LogTrivial("-!!- SAHighwayCallouts - |"+callout+"| - Callout displayed!");
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("-!!- SAHighwayCallouts - |"+callout+"| - Callout accepted!");
            if (PursuitChooser == 1) SAHC_Functions.LuxVehicleSpawn(out _susV, _spawnpoint, _heading);
            if (PursuitChooser == 2) SAHC_Functions.SpawnNormalCar(out _susV, _spawnpoint, _heading);
            if (PursuitChooser == 3)
            {
                SAHC_Functions.SpawnSemiTruckAndTrailer(out _susV, out _susVTrailer, _spawnpoint, _heading);
                _susV.Trailer = _susVTrailer;
            }
            if (_susV.FreeSeatsCount == 0) _passengerChooser = 3; //Forces there to be no passenger sense no seat available
            SAHC_Functions.SpawnNormalPed(out _suspect, _spawnpoint, _heading);
            SAHC_Functions.PedPersonaChooser(in _suspect);
            if (_passengerChooser == 1)
            {
                Game.LogTrivial("-!!- SAHighwayCallouts - |"+callout+"| - Passenger was created along with driver");
                SAHC_Functions.SpawnNormalPed(out _passenger, _spawnpoint, _heading);
                SAHC_Functions.PedPersonaChooser(in _passenger);
                _passenger.WarpIntoVehicle(_susV, 0);
            }
            _suspect.WarpIntoVehicle(_susV, -1);
            
            _susBlip = _suspect.AttachBlip();
            _susBlip.Color = Color.Red;
            _susBlip.EnableRoute(Color.Yellow);

            if (Settings.PursuitBackup)
            {
                _copSpawmpoint = _spawnpoint.Around2D(5f, 10f);
                _finalCopSpawnpoint = World.GetNextPositionOnStreet(_copSpawmpoint);
                
                SAHC_Functions.SpawnPolicePed(in currentCounty, out _cop, _finalCopSpawnpoint, _heading);
                SAHC_Functions.SpawnPoliceCar(in currentCounty, out _copV, _finalCopSpawnpoint, _heading);
                _cop.WarpIntoVehicle(_copV, -1);
            }
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            Game.LogTrivial("-!!- SAHighwayCallouts - |"+callout+"| - Main process started");
            if (!Settings.PursuitBackup)
            {
                Game.LogTrivial("-!!- SAHighwayCallouts - |"+callout+"| - Running callout without AI backup");
                if (Game.LocalPlayer.Character.DistanceTo(_suspect) <= 120f && !_beforeOnScene)
                {
                    _beforeOnScene = true;
                    _suspect.Tasks.CruiseWithVehicle(70, VehicleDrivingFlags.Emergency);
                }
                if (Game.LocalPlayer.Character.DistanceTo(_suspect) <= 55f && !_onScene)
                {
                    _onScene = true;
                    if (_susBlip) _susBlip.Delete();

                    _pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                    LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(_pursuit, true);
                    LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(_pursuit, _suspect);
                    if (_passengerChooser == 1) LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(_pursuit, _passenger);
                    LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                    _pursuitStarted = true;
                }   
            }

            if (Settings.PursuitBackup)
            {
                Game.LogTrivial("-!!- SAHighwayCallouts - |"+callout+"| - Running callout with AI backup");
                if (Game.LocalPlayer.Character.DistanceTo(_suspect) <= 175f && !_beforeOnScene)
                {
                    _suspect.Tasks.CruiseWithVehicle(65, VehicleDrivingFlags.Emergency);
                    _cop.Tasks.CruiseWithVehicle(70, VehicleDrivingFlags.Emergency);
                    _beforeOnScene = true;
                    
                    _pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                    LSPD_First_Response.Mod.API.Functions.AddCopToPursuit(_pursuit, _cop);
                    LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(_pursuit, _suspect);
                    if (_passengerChooser == 1) LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(_pursuit, _passenger);
                    LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                    _pursuitStarted = true;
                }

                if (Game.LocalPlayer.Character.DistanceTo(_suspect) <= 70f && !_onScene)
                {
                    _susBlip.Delete();
                    _onScene = true;
                    LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(_pursuit, true);
                }
            }

            //End Script stufs
            if (Game.LocalPlayer.Character.IsDead)
            {
                End();
            }

            if (_passengerChooser != 1 && _suspect.IsDead && !_suspectDead)
            {
                if (Settings.EnableEndCalloutHelpMessages)
                {
                    _suspectDead = true;
                    Game.DisplayHelp("Press ~r~'"+Settings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                }
            }
            
            if (_passengerChooser == 1 && _suspect.IsDead && !_suspectDead && _passenger.IsDead)
            {
                if (Settings.EnableEndCalloutHelpMessages)
                {
                    _suspectDead = true;
                    Game.DisplayHelp("Press ~r~'"+Settings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                }
            }

            if (Game.IsKeyDown(Settings.InputEndCalloutKey))
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition(
                    "OFFICERS_REPORT_03 OP_CODE OP_4", _spawnpoint);
                Game.DisplayNotification("~g~Dispatch:~w~ All Units, "+CalloutMessage+" Code 4");
                
                Game.LogTrivial("-!!- SAHighwayCallouts - |"+callout+"| - Callout was force ended by player -!!-");
                End();
            }
            
            base.Process();
        }

        public override void End()
        {
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", _spawnpoint);
            Game.DisplayNotification("~g~Dispatch:~w~ All Units, "+CalloutMessage+" Code 4");
            if (_suspect) _suspect.Dismiss();
            if (_susV) _susV.Dismiss();
            if (_susBlip) _susBlip.Delete();
            if (_pursuitStarted) LSPD_First_Response.Mod.API.Functions.ForceEndPursuit(_pursuit);
            if (_passenger) _passenger.Dismiss();
            if (_cop) _cop.Dismiss();
            if (_copV) _copV.Dismiss();
            if (_susVTrailer) _susVTrailer.Dismiss();

                Game.LogTrivial("-!!- SAHighwayCallouts - |"+callout+"| - Cleaned up!");
            base.End();
        }
    }
}