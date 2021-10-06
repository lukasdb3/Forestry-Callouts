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


namespace SAHighwayCallouts.Callouts
{
    [CalloutInfo("LuxuryVehiclePursuit", CalloutProbability.Medium)]
    internal class LuxuryVehiclePursuit : Callout
    {
        #region Variables

        private string callout = "LuxuryVehiclePursuit";
        private Ped _suspect;
        private Vehicle _susV;
        private Blip _susBlip;
        private float _heading;
        private Vector3 _spawnpoint;
        private LHandle _pursuit;

        private bool _onScene;
        private bool _pursuitStarted;
        private bool _suspectDead;
        #endregion

        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnChunks.ChunkGetter(in callout);
            _spawnpoint = SpawnChunks.finalSpawnpoint;
            _heading = SpawnChunks.finalHeading;

            ShowCalloutAreaBlipBeforeAccepting(_spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, _spawnpoint);

            CalloutMessage = "~o~Luxury Vehicle Pursuit";
            CalloutAdvisory = "~b~Dispatch:~w~ High end stolen vehicle spotted, Respond ~r~Code 3~w~";
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS_01 OFFICERS_REPORT_01 CRIME_RESIST_ARREST_01 UNITS_RESPOND_CODE_03_01", _spawnpoint);
            CalloutPosition = _spawnpoint;
            Game.LogTrivial("-!!- SAHighwayCallouts - |LuxuryVehiclePursuit| - Callout displayed!");
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("-!!- SAHighwayCallouts - |LuxuryVehiclePursuit| - Callout accepted!");
            SAHC_Functions.LuxVehicleSpawn(out _susV, _spawnpoint, _heading);
            SAHC_Functions.SpawnNormalPed(out _suspect, _spawnpoint, _heading);
            SAHC_Functions.PedPersonaChooser(in _suspect);
            _suspect.WarpIntoVehicle(_susV, -1);
            _susBlip = _suspect.AttachBlip();
            _susBlip.Color = Color.Red;
            _susBlip.EnableRoute(Color.Yellow);
            
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (Game.LocalPlayer.Character.DistanceTo(_suspect) <= 55f && !_onScene)
            {
                _onScene = true;
                _susBlip.IsRouteEnabled = false;
                
                _pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(_pursuit, true);
                LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(_pursuit, _suspect);
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                _pursuitStarted = true;
            }
            
            //End Script stufs
            if (Game.LocalPlayer.Character.IsDead)
            {
                End();
            }

            if (_suspect.IsDead && !_suspectDead)
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
                Game.DisplayNotification("~g~Dispatch:~w~ All Units, Luxury Vehicle Pursuit Code 4");
                Game.LogTrivial("-!!- SAHighwayCallouts - |LuxuryVehiclePursuit| - Callout was force ended by player -!!-");
                End();
            }
            
            base.Process();
        }

        public override void End()
        {
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", _spawnpoint);
            Game.DisplayNotification("~g~Dispatch:~w~ All Units, Luxury Vehicle Pursuit Code 4");
            if (_suspect) _suspect.Dismiss();
            if (_susV) _susV.Dismiss();
            if (_susBlip) _susBlip.Delete();
            if (_pursuitStarted) LSPD_First_Response.Mod.API.Functions.ForceEndPursuit(_pursuit);

                Game.LogTrivial("-!!- SAHighwayCallouts - |LuxuryVehiclePursuit| - Cleaned up!");
            base.End();
        }
    }
}