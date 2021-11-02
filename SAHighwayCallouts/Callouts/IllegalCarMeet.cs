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
using Rage.Native;

namespace SAHighwayCallouts.Callouts
{
    [CalloutInfo("IllegalCarMeet", CalloutProbability.Medium)]
    internal class IllegalCarMeet : Callout
    {
        #region Variables
        private string callout = "IllegalCarMeet";
        internal string CurrentCounty;
        private Vector3 _badSpawn1;
        private float _badHeading1;
        private Vector3 _badSpawn2;
        private float _badHeading2;
        private Vector3 _badSpawn3;
        private float _badHeading3;
        private Vector3 _badSpawn4;
        private float _badHeading4;
        private Vector3 _badSpawn5;
        private float _badHeading5;
        private Vector3 _badSpawn6;
        private float _badHeading6;
        private Vector3 _badSpawn7;
        private float _badHeading7;
        private Vector3 _badSpawn8;
        private float _badHeading8;
        private Vector3 _badSpawn9;
        private float _badHeading9;
        private Vector3 _badSpawn10;
        private float _badHeading10;

        private Ped _bad1;
        private Ped _bad2;
        private Ped _bad3;
        private Ped _bad4;
        private Ped _bad5;
        private Ped _bad6;
        private Ped _bad7;
        private Ped _bad8;
        private Ped _bad9;
        private Ped _bad10;

        private Vehicle _badVehicle1;
        private Vehicle _badVehicle2;
        private Vehicle _badVehicle3;
        private Vehicle _badVehicle4;
        private Vehicle _badVehicle5;
        private Vehicle _badVehicle6;
        private Vehicle _badVehicle7;
        private Vehicle _badVehicle8;
        private Vehicle _badVehicle9;
        private Vehicle _badVehicle10;

        private LHandle _pursuit;

        #endregion

        public override void OnCalloutDisplayed()
        {
            CalloutMessage = "~o~Illegal Car Meet";
            CalloutAdvisory = "~b~Dispatch:~w~ Illegal car meet reported, Respond ~r~Code 2";
            SpawnChunks.ChunkGetter(in callout, out CurrentCounty);
            _badSpawn1 = SpawnChunks.badSpawn1;
            _badHeading1 = SpawnChunks.badHeading1;
            _badSpawn2 = SpawnChunks.badSpawn2;
            _badHeading2 = SpawnChunks.badHeading2;
            _badSpawn3 = SpawnChunks.badSpawn3;
            _badHeading3 = SpawnChunks.badHeading3;
            _badSpawn4 = SpawnChunks.badSpawn4;
            _badHeading4 = SpawnChunks.badHeading4;
            _badSpawn5 = SpawnChunks.badSpawn5;
            _badHeading5 = SpawnChunks.badHeading5;
            _badSpawn6 = SpawnChunks.badSpawn6;
            _badHeading6 = SpawnChunks.badHeading6;
            _badSpawn7 = SpawnChunks.badSpawn7;
            _badHeading7 = SpawnChunks.badHeading7;
            _badSpawn8 = SpawnChunks.badSpawn8;
            _badHeading8 = SpawnChunks.badHeading8;
            _badSpawn9 = SpawnChunks.badSpawn9;
            _badHeading9 = SpawnChunks.badHeading9;
            _badSpawn10 = SpawnChunks.badSpawn10;
            _badHeading10 = SpawnChunks.badHeading10;
            ShowCalloutAreaBlipBeforeAccepting(_badSpawn1, 30f);
            AddMinimumDistanceCheck(30f, _badSpawn1);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS_01 WE_HAVE_01 CRIME_DISTURBING_THE_PEACE_01 UNITS_RESPOND_CODE_02_01", _badSpawn1);
            CalloutPosition = _badSpawn1;
            Game.LogTrivial("-!!- SAHighwayCallouts - |" + callout + "| - Callout displayed!");
            
            base.OnCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            
            
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            
            //End Script stufs
            if (Game.LocalPlayer.Character.IsDead)
            {
                End();
            }

            if (Game.IsKeyDown(Settings.InputEndCalloutKey))
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", _badSpawn1);
                Game.DisplayNotification("~b~Dispatch:~w~ All Units, Illegal Car Meet 4");
                if (LSPD_First_Response.Mod.API.Functions.IsPursuitStillRunning(_pursuit)) LSPD_First_Response.Mod.API.Functions.ForceEndPursuit(_pursuit);
                Game.LogTrivial("-!!- SAHighwayCallouts - |"+callout+"| - Callout was force ended by player -!!-");
                End();
            }
            base.Process();
        }

        public override void End()
        {
            if (_bad1) _bad1.Dismiss();
            if (_bad2) _bad2.Dismiss();
            if (_bad3) _bad3.Dismiss();
            if (_bad4) _bad4.Dismiss();
            if (_bad5) _bad5.Dismiss();
            if (_bad6) _bad6.Dismiss();
            if (_bad7) _bad7.Dismiss();
            if (_bad8) _bad8.Dismiss();
            if (_bad9) _bad9.Dismiss();
            if (_bad10) _bad10.Dismiss();
            if (_badVehicle1) _badVehicle1.Dismiss();
            if (_badVehicle2) _badVehicle2.Dismiss();
            if (_badVehicle3) _badVehicle3.Dismiss();
            if (_badVehicle4) _badVehicle4.Dismiss();
            if (_badVehicle5) _badVehicle5.Dismiss();
            if (_badVehicle6) _badVehicle6.Dismiss();
            if (_badVehicle7) _badVehicle7.Dismiss();
            if (_badVehicle8) _badVehicle8.Dismiss();
            if (_badVehicle9) _badVehicle9.Dismiss();
            if (_badVehicle10) _badVehicle10.Dismiss();
            Game.LogTrivial("-!!- SAHighwayCallouts - |" + callout + "| - Cleaned up!");
            base.End();
        }
    }
}