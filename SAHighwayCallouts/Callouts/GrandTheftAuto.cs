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

namespace SAHighwayCallouts.Callouts
{
    [CalloutInfo("GrandTheftAuto", CalloutProbability.Medium)]
    internal class GrandTheftAuto : Callout
    {
        #region Variables

        private string callout = "GrandTheftAuto";
        internal string currentCounty;
        private Vector3 _spawnpoint;
        private Vector3 _victimSpawnpoint;
        private Vector3 _finalVictimSpawnpoint;
        private float _heading;

        private Ped _victim;
        private Blip _vicBlip;
        private Ped _suspect;
        private Ped _susBlip;
        private Vehicle _victimCar;

        private bool _beforeOnScene;
        private bool _onScene;

        private bool _dialogueReady;
        private bool _dialgueOver;
        private int dPick = new Random().Next(1, 4);
        private int _counter;
        private string maleFemale;
        
        

        #endregion

        public override bool OnBeforeCalloutDisplayed()
        {
            CalloutMessage = "~o~Grand Theft Auto";
            CalloutAdvisory = "~b~Dispatch:~w~ Grand Theft Auto in progress, Respond ~r~Code 3";
            SpawnChunks.ChunkGetter(in callout, out currentCounty);
            _spawnpoint = SpawnChunks.finalSpawnpoint;
            _heading = SpawnChunks.finalHeading;
            ShowCalloutAreaBlipBeforeAccepting(_spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, _spawnpoint);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS_01 WE_HAVE_01 CRIME_GRAND_THEFT_AUTO_01 UNITS_RESPOND_CODE_03_01", _spawnpoint);
            CalloutPosition = _spawnpoint;
            Game.LogTrivial("-!!- SAHighwayCallouts - |"+callout+"| - Callout displayed!");
            
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("-!!- SAHighwayCallouts - |"+callout+"| - Callout accepted!");
            SAHC_Functions.SpawnNormalPed(out _suspect, _spawnpoint, _heading);
            SAHC_Functions.PedPersonaChooser(in _suspect);

            _victimSpawnpoint = _spawnpoint.Around2D(5f, 7f);
            _finalVictimSpawnpoint = World.GetNextPositionOnStreet(_victimSpawnpoint);
            
            SAHC_Functions.SpawnNormalPed(out _victim, _finalVictimSpawnpoint, _heading);
            SAHC_Functions.SpawnNormalCar(out _victimCar, _spawnpoint, _heading);

            if (_victim.IsMale) maleFemale = "sir";
            if (!_victim.IsMale) maleFemale = "mam";
            
            _suspect.WarpIntoVehicle(_victimCar, -1);

            _vicBlip = _victim.AttachBlip();
            _vicBlip.EnableRoute(Color.Yellow);
            _vicBlip.Color = Color.OrangeRed;
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (_beforeOnScene && Game.LocalPlayer.Character.DistanceTo(_victim) <= 30f)
            {
                Game.LogTrivial("-!!- SAHighwayCallouts - |"+callout+"| - Main process started!");
                Game.DisplayNotification("Go talk to the ~o~Victim~w~ of the Grand Theft Auto");
                _dialogueReady = true;
                _beforeOnScene = true;
            }

            if (_dialogueReady && !_onScene && Game.LocalPlayer.Character.DistanceTo(_victim) <= 5f)
            {
                Game.DisplayNotification("Press ~y~'" + Settings.DialogueKey + "' to talk to the ~o~Victim~w~");
                _victim.Heading = Game.LocalPlayer.Character.Heading + 180f;
                _victim.Tasks.StandStill(-1);
                _onScene = true;
            }
            
            if (Game.IsKeyDown(Settings.InputDialogueKey)) Dialogue();

                base.Process();
        }

        public override void End()
        {
            if (_suspect) _suspect.Dismiss();
            if (_susBlip) _susBlip.Delete();
            if (_victim) _victim.Dismiss();
            if (_vicBlip) _vicBlip.Delete();
            if (_victimCar) _victimCar.Dismiss();
            Game.LogTrivial("-!!- SAHighwayCallouts - |"+callout+"| - Cleaned up!");
            base.End();
        }
        
        private void Dialogue()
        {
            switch (dPick)
            {
                case 1:
                    if (_counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello, how are you doing today " + maleFemale + ".");
                    }
                    if (_counter == 2)
                    {
                        Game.DisplaySubtitle("~g~Suspect:~w~ Im doing great, just doing some camping.");
                    }
                    if (_counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ it's against the law to camp out here. ");
                    }
                    if (_counter == 4)
                    {
                        Game.DisplaySubtitle("~g~Suspect:~w~ Are you sure? I dont think it is.");
                    }
                    if (_counter == 5)
                    {
                        Game.DisplaySubtitle("No further dialogue take appropriate acction");
                        _dialgueOver = true;
                    }

                    _counter++;
                    break;

                case 2:
                    if (_counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello, how are you doing today " + maleFemale + ".");
                    }
                    if (_counter == 2)
                    {
                        Game.DisplaySubtitle("~g~Suspect:~w~ I'm doing fine, Whats your issue.");
                    }
                    if (_counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ It's against the law to be camping at this location.");
                    }
                    if (_counter == 4)
                    {
                        Game.DisplaySubtitle("~g~Suspect:~w~ Ya right, no it's not.");
                    }
                    if (_counter == 5)
                    {
                        Game.DisplaySubtitle("No further dialogue take appropriate acction");
                        _dialgueOver = true;
                    }

                    _counter++;
                    break;
            }
        }
    }
}