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
using SAHighwayCallouts.Functions.Logger;


namespace SAHighwayCallouts.Callouts
{
    [CalloutInfo("SemiTruckPursuit", CalloutProbability.Medium)]
    internal class SemiTruckPursuit : Callout
    {
        #region Variables

        private string callout = "SemiTruckPursuit";
        private string currentCounty = null;
        private Ped _suspect;
        private Ped _passenger;
        private Vehicle _susV;
        private Vehicle _susVTrailer;
        private Blip _susBlip;
        private float _heading;
        private Vector3 _spawnpoint;
        private LHandle _pursuit;

        private bool _onScene;
        private bool _beforeOnScene;
        private bool _pursuitStarted;
        private bool _suspectDead;

        private int _passengerChooser = new Random().Next(1, 4);
        #endregion

        public override bool OnBeforeCalloutDisplayed()
        {
            CalloutMessage = "~o~Semi Truck Pursuit";
            CalloutAdvisory = "~b~Dispatch:~w~ Stolen semi truck spotted, Respond ~r~Code 3~w~";
            SpawnChunks.ChunkGetter(in callout, out currentCounty);
            _spawnpoint = SpawnChunks.finalSpawnpoint;
            _heading = SpawnChunks.finalHeading;

            ShowCalloutAreaBlipBeforeAccepting(_spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, _spawnpoint);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS_01 OFFICERS_REPORT_01 CRIME_RESIST_ARREST_01 UNITS_RESPOND_CODE_03_01", _spawnpoint);
            CalloutPosition = _spawnpoint;
            LFunctions.BasicLogger(callout, "Callout displayed!");
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            LFunctions.BasicLogger(callout, "Callout accepted!");
            SAHC_Functions.SpawnSemiTruckAndTrailer(out _susV, out _susVTrailer, _spawnpoint, _heading);
            _susV.Trailer = _susVTrailer;
            _susV.IsStolen = true;
            if (_susV.FreeSeatsCount == 0) _passengerChooser = 3; //Forces there to be no passenger sense no seat available
            SAHC_Functions.SpawnNormalPed(out _suspect, _spawnpoint, _heading);
            SAHC_Functions.PedPersonaChooser(in _suspect);
            if (_passengerChooser == 1)
            {
                LFunctions.BasicLogger(callout, "Passenger was created along with diver!");
                SAHC_Functions.SpawnNormalPed(out _passenger, _spawnpoint, _heading);
                SAHC_Functions.PedPersonaChooser(in _passenger);
                _passenger.WarpIntoVehicle(_susV, 0);
            }
            _suspect.WarpIntoVehicle(_susV, -1);
            
            _susBlip = _suspect.AttachBlip();
            _susBlip.Color = Color.Red;
            _susBlip.EnableRoute(Color.Yellow);
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (Game.LocalPlayer.Character.DistanceTo(_suspect) <= 400f && !_beforeOnScene)
            {
                LFunctions.BasicLogger(callout, "Main process started!");
                _suspect.Tasks.CruiseWithVehicle(_susV, 70, VehicleDrivingFlags.Emergency);
                _beforeOnScene = true;
            }
            if (Game.LocalPlayer.Character.DistanceTo(_suspect) <= 180f && !_onScene)
            {
                _onScene = true;
                if (_susBlip) _susBlip.Delete();

                _pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(_pursuit, true);
                LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(_pursuit, _suspect);
                if (_passengerChooser == 1) LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(_pursuit, _passenger);
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                _pursuitStarted = true;

                if (Settings.PursuitBackup)
                {
                    UltimateBackup.API.Functions.callPursuitBackup();
                    UltimateBackup.API.Functions.callPursuitBackup();
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
                Game.DisplayNotification("~b~Dispatch:~w~ All Units, Semi Truck Pursuit Code 4");
                
                LFunctions.BasicLogger(callout, "Callout was force ended by player!");
                End();
            }
            
            base.Process();
        }

        public override void End()
        {
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", _spawnpoint);
            Game.DisplayNotification("~b~Dispatch:~w~ All Units, Semi Truck Pursuit Code 4");
            if (_suspect) _suspect.Dismiss();
            if (_susV) _susV.Dismiss();
            if (_susBlip) _susBlip.Delete();
            if (_pursuitStarted) LSPD_First_Response.Mod.API.Functions.ForceEndPursuit(_pursuit);
            if (_passenger) _passenger.Dismiss();
            if (_susVTrailer) _susVTrailer.Dismiss();

            LFunctions.BasicLogger(callout, "Cleaned up!");
            base.End();
        }
    }
}