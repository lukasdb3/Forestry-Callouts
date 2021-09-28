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
        #endregion

        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnChunks.ChunkGetter(in callout, out _spawnpoint, out _heading); 
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
            End();
            
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            
            
            base.Process();
        }

        public override void End()
        {
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", _spawnpoint);
            Game.DisplayNotification("~g~Dispatch:~w~ All Units, Luxury Vehicle Pursuit Code 4");
            if (_suspect) _suspect.Dismiss();
            if (_susV) _susV.Dismiss();
            if (_susBlip) _susBlip.Delete();
            
            Game.LogTrivial("-!!- SAHighwayCallouts - |LuxuryVehiclePursuit| - Cleaned up!");
            base.End();
        }
    }
}