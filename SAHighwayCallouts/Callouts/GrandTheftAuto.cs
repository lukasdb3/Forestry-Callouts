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
    [CalloutInfo("GrandTheftAuto", CalloutProbability.Medium)]
    internal class GrandTheftAuto : Callout
    {
        #region Variables

        private string callout = "GrandTheftAuto";
        internal string CurrentCounty;
        private Vector3 _spawnpoint;
        private float _heading;
        private Vector3 _vicSpawnpoint;
        private float _vicHeading;
        private int _scenario = new Random().Next(1, 3);
        private int _scenario2Options = new Random().Next(1, 4);

        private Color _vicCarC;
        private string _vicCarColor;
        private string _vicCarModel;
        private string _vicCarPlateNum;
        private string _suspectsCurrentStreet;
        private float _suspectsCurrentSpeed;
        private double _suspectsDisplaySpeed;

        //AI Cop shit
        private Vector3 _copSpawnpoint;
        private Vector3 _finalCopSpawnpoint;
        private Ped _cop;
        private Vehicle _copCar;
        private Blip _copBlip;
        private Vector3 _driveToPosition;
        private Vector3 _finalDriveToPosition;

        //Victim and Suspect shit
        private Ped _victim;
        private Blip _vicBlip;
        private Ped _suspect;
        private Blip _susBlip;
        private Vehicle _victimCar;


        //BOOLSSSSSSSSSSSSSSSSSSSSSSSSS
        private bool _beforeOnScene;
        private bool _onScene;
        private bool _suspectLeft;
        private bool _suspectDead;
        private bool _victimTakenCareOf;
        private bool _victimWaitingTransport;
        private bool _infoModelNotiSent;
        private bool _suspectFound;
        private bool _pursuitStarted;
        private bool _notfiRanMax;
        private bool _firstNotfiOut;
        private bool _shootoutStarted;

        //Dialogue stuuf
        private bool _dialogueReady;
        private bool _dialgueOver;
        private int dPick = new Random().Next(1, 2);
        private int _counter;
        private string _maleFemale;
        private int doesVictimKnowLicensePlate = new Random().Next(1, 3);
        private int victimWantsTransport = new Random().Next(1, 3);


        //Scenario Crap
        private int _notfiRan;
        private LHandle _pursuit;
        private bool _pulloverPrompted;

        //Timer stuffs
        private int _timer;
        private bool _timerPaused;

        #endregion

        public override bool OnBeforeCalloutDisplayed()
        {
            CalloutMessage = "~o~Grand Theft Auto";
            CalloutAdvisory = "~b~Dispatch:~w~ Grand Theft Auto in progress, Respond ~r~Code 3";
            SpawnChunks.ChunkGetter(in callout, out CurrentCounty);
            _spawnpoint = SpawnChunks.finalSpawnpoint;
            _heading = SpawnChunks.finalHeading;
            _vicSpawnpoint = SpawnChunks.finalVicSpawnpoint;
            _vicHeading = SpawnChunks.finalVicHeading;
            ShowCalloutAreaBlipBeforeAccepting(_spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, _spawnpoint);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition(
                "ATTENTION_ALL_UNITS_01 WE_HAVE_01 CRIME_GRAND_THEFT_AUTO_01 UNITS_RESPOND_CODE_03_01", _spawnpoint);
            CalloutPosition = _spawnpoint;
            Game.LogTrivial("-!!- SAHighwayCallouts - |" + callout + "| - Callout displayed!");

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("-!!- SAHighwayCallouts - |" + callout + "| - Callout accepted!");
            SAHC_Functions.SpawnNormalPed(out _suspect, _spawnpoint, _heading);
            SAHC_Functions.PedPersonaChooser(in _suspect);

            SAHC_Functions.SpawnNormalPed(out _victim, _vicSpawnpoint, _vicHeading);
            SAHC_Functions.SpawnNormalCar(out _victimCar, _spawnpoint, _heading);
            SAHC_Functions.ColorPicker(out _vicCarC, out _vicCarColor);
            _victimCar.PrimaryColor = _vicCarC;
            _vicCarModel = _victimCar.Model.Name.ToUpper();
            _vicCarPlateNum = _victimCar.LicensePlate;
            LSPD_First_Response.Mod.API.Functions.SetVehicleOwnerName(_victimCar, LSPD_First_Response.Mod.API.Functions.GetPersonaForPed(_victim).FullName);

            if (_victim.IsMale) _maleFemale = "sir";
            if (!_victim.IsMale) _maleFemale = "mam";

            _suspect.WarpIntoVehicle(_victimCar, -1);

            _vicBlip = _victim.AttachBlip();
            _vicBlip.EnableRoute(Color.Yellow);
            _vicBlip.Color = Color.Orange;
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (!_suspectLeft && Game.LocalPlayer.Character.DistanceTo(_victim) <= 350f)
            {
                Game.LogTrivial("-!!- SAHighwayCallouts - |" + callout + "| - Main process started!");
                _suspectLeft = true;
                _suspect.Tasks.CruiseWithVehicle(_victimCar, 30f, VehicleDrivingFlags.Normal);
            }

            if (!_beforeOnScene && Game.LocalPlayer.Character.DistanceTo(_victim) <= 30f)
            {
                Game.DisplayNotification("Go talk to the ~o~Victim~w~ of the Grand Theft Auto");
                _beforeOnScene = true;
            }

            if (_beforeOnScene && !_onScene && Game.LocalPlayer.Character.DistanceTo(_victim) <= 5f)
            {
                Game.DisplayNotification("Press ~y~'" + Settings.DialogueKey + "'~w~ to talk to the ~o~Victim~w~");
                _onScene = true;
            }

            if (!_dialogueReady && Game.IsKeyDown(Settings.InputDialogueKey))
            {
                _victim.Heading = Game.LocalPlayer.Character.Heading + 180f;
                _victim.Tasks.StandStill(-1);
                _dialogueReady = true;
            }

            if (!_dialgueOver && _dialogueReady && Game.IsKeyDown(Settings.InputDialogueKey)) Dialogue();

            if (_dialgueOver && !_victimWaitingTransport && victimWantsTransport == 1)
            {
                _suspect.Tasks.ParkVehicle(_victimCar, _suspect.Position, _suspect.Heading);
                Game.DisplayNotification("Press ~y~'" + Settings.InteractionKey +
                                         "'~w~ to call for transport for the ~o~Victim~w~.");
                _victimWaitingTransport = true;
            }

            if (_dialgueOver && victimWantsTransport != 1)
            {
                if (_vicBlip) _vicBlip.Delete();
                if (_victim) _victim.Dismiss();
            }

            if (victimWantsTransport == 1 && !_victimTakenCareOf && Game.IsKeyDown(Settings.InputInteractionKey))
            {
                Game.DisplayHelp("Wait until ~y~transportation~w~ arrives for the ~o~Victim");
                _copSpawnpoint = _spawnpoint.Around2D(75f, 100f);
                _finalCopSpawnpoint = World.GetNextPositionOnStreet(_copSpawnpoint);

                SAHC_Functions.SpawnPoliceVehicle(out _copCar, _finalCopSpawnpoint, _heading, CurrentCounty);
                SAHC_Functions.SpawnPolcePed(out _cop, _finalCopSpawnpoint, _heading, CurrentCounty);
                _cop.WarpIntoVehicle(_copCar, -1);

                _copBlip = _cop.AttachBlip();
                _copBlip.IsRouteEnabled = false;
                _copBlip.Color = Color.MediumBlue;
                _copBlip.Scale = 0.7f;

                _driveToPosition = _spawnpoint.Around2D(5f, 7f);
                _finalDriveToPosition = World.GetNextPositionOnStreet(_driveToPosition);

                _cop.Tasks.DriveToPosition(_finalDriveToPosition, 10f, VehicleDrivingFlags.Normal).WaitForCompletion();
                _victim.Tasks
                    .GoStraightToPosition(_copCar.Position.Around2D(2f, 4f), 10f, _copCar.Heading + 180f, 0f, -1)
                    .WaitForCompletion();
                _suspect.Tasks.CruiseWithVehicle(_victimCar, 30f, VehicleDrivingFlags.Normal);
                _victim.Tasks.EnterVehicle(_copCar, 2).WaitForCompletion();

                _cop.Tasks.CruiseWithVehicle(_copCar, 10f, VehicleDrivingFlags.Normal);
                _victimTakenCareOf = true;
            }

            if (_victimTakenCareOf && Game.LocalPlayer.Character.DistanceTo(_suspect) > 50f)
            {
                if (_victim) _victim.Delete();
                if (_cop) _cop.Dismiss();
                if (_copBlip) _copBlip.Delete();
            }

            if (_victimTakenCareOf && !_infoModelNotiSent ||
                !_infoModelNotiSent && victimWantsTransport != 1 && _dialgueOver)
            {
                Game.DisplayHelp("Look for the ~o~Victims~w~ vehicle");
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition(
                    "ATTENTION_ALL_UNITS_01 SUSPECT_LAST_SEEN_01 IN_OR_ON_POSITION", _suspect.Position);
                if (doesVictimKnowLicensePlate == 1)
                {
                    _vicCarPlateNum = _victimCar.LicensePlate;
                }
                else _vicCarPlateNum = "~r~NOT KNOWN";

                Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "~b~VEHICLE DESCRIPTION",
                    "~p~MODEL:~w~ " + _vicCarModel + "",
                    "~b~PRIMARY COLOR:~w~ " + _vicCarColor.ToUpper() + "~n~ ~g~LICENSEPLATE:~w~ " + _vicCarPlateNum +
                    "");
                _infoModelNotiSent = true;
            }

            if (_infoModelNotiSent && !_suspectFound)
            {
                if (!_timerPaused)
                {
                    _timer++;
                }

                if (Game.LocalPlayer.Character.DistanceTo(_suspect) <= 35f)
                {
                    _suspectFound = true;
                    Game.LogTrivial("-!!- SAHighwayCallouts - |" + callout + "| - Suspect has been found, running scenario " + _scenario + "!");
                    if (_susBlip) _susBlip.Delete();
                }

                if (_timer == 500 && !_firstNotfiOut)
                {
                    _suspectsCurrentSpeed = _victimCar.Speed;
                    _suspectsDisplaySpeed = Math.Round(_suspectsCurrentSpeed, 0);
                    _suspectsCurrentStreet = World.GetStreetName(_suspect.Position);
                    Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "~b~SUSPECT LAST SEEN",
                        "~p~MODEL:~w~ " + _vicCarModel + "",
                        "~b~PRIMARY COLOR:~w~ " + _vicCarColor.ToUpper() + "~n~ ~g~LICENSE PLATE:~w~ " +
                        _vicCarPlateNum + "~n~ ~o~STREET:~w~ " + _suspectsCurrentStreet + "~n~ ~y~CURRENT SPEED:~w~ " +
                        _suspectsDisplaySpeed + "MPH");
                    LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition(
                        "ATTENTION_ALL_UNITS_01 SUSPECT_LAST_SEEN_01 IN_OR_ON_POSITION", _suspect.Position);
                    _firstNotfiOut = true;
                }

                if (_timer == 1250 && _notfiRan != 10)
                {
                    _suspectsCurrentSpeed = _victimCar.Speed;
                    _suspectsDisplaySpeed = Math.Round(_suspectsCurrentSpeed, 0);
                    _suspectsCurrentStreet = World.GetStreetName(_suspect.Position);
                    Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "~b~SUSPECT LAST SEEN",
                        "~p~MODEL:~w~ " + _vicCarModel + "",
                        "~b~PRIMARY COLOR:~w~ " + _vicCarColor.ToUpper() + "~n~ ~g~LICENSE PLATE:~w~ " +
                        _vicCarPlateNum + "~n~ ~o~STREET:~w~ " + _suspectsCurrentStreet + "~n~ ~y~CURRENT SPEED:~w~ " +
                        _suspectsDisplaySpeed + "MPH");
                    LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition(
                        "ATTENTION_ALL_UNITS_01 SUSPECT_LAST_SEEN_01 IN_OR_ON_POSITION", _suspect.Position);
                    _notfiRan++;
                    _timer = 0;
                }

                if (_notfiRan == 10 && !_notfiRanMax)
                {
                    _timerPaused = true;
                    //Blip the vehicle
                    Game.DisplayHelp("The ~r~suspect~w~ has been blipped");
                    _susBlip = _suspect.AttachBlip();
                    _susBlip.IsRouteEnabled = true;
                    _susBlip.RouteColor = Color.Yellow;
                    _susBlip.Color = Color.Red;
                    _notfiRanMax = true;
                }
            }

            //Scenario 1: Pursuit
            if (_suspectFound && _scenario == 1)
            {
                if (Game.LocalPlayer.Character.DistanceTo(_suspect) <= 25f && !_pursuitStarted)
                {
                    Game.LogTrivial("-!!- SAHighwayCallouts - |" + callout + "| - Pursuit starting!");
                    if (_susBlip.Exists()) _susBlip.Delete();
                    _pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                    LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(_pursuit, true);
                    LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(_pursuit, _suspect);
                    LSPD_First_Response.Mod.API.Functions.PlayScannerAudio(
                        "ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                    _pursuitStarted = true;

                    if (Settings.PursuitBackup)
                    {
                        UltimateBackup.API.Functions.callPursuitBackup();
                        UltimateBackup.API.Functions.callPursuitBackup();
                    }
                }
            }

            //Scenario 2: Pullover
            if (_suspectFound && _scenario == 2)
            {
                if (Game.LocalPlayer.Character.DistanceTo(_suspect) <= 25f && !_pulloverPrompted)
                {
                    Game.DisplayHelp("Pullover the ~r~Vehicle~w~");
                    _pulloverPrompted = true;
                    if (!_susBlip)
                    {
                        _susBlip = _suspect.AttachBlip();
                        _susBlip.IsRouteEnabled = false;
                        _susBlip.Color = Color.Red;
                        _susBlip.Scale = 0.7f;
                        LSPD_First_Response.Mod.API.Functions.SetPedCanBePulledOver(_suspect, true);
                    }
                }
                LSPD_First_Response.Mod.API.Events.OnPulloverOfficerApproachDriver += EventsOnOnPulloverOfficerApproachDriver;
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
                    _suspect.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Player, Relationship.Dislike);
                    _suspect.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Dislike);
                    _suspectDead = true;
                    Game.DisplayHelp("Press ~r~'"+Settings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                }
            }
            
            if (Game.IsKeyDown(Settings.InputEndCalloutKey))
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition(
                    "OFFICERS_REPORT_03 OP_CODE OP_4", _spawnpoint);
                Game.DisplayNotification("~b~Dispatch:~w~ All Units, Grand Theft Auto Code 4");
                if (LSPD_First_Response.Mod.API.Functions.IsPursuitStillRunning(_pursuit)) LSPD_First_Response.Mod.API.Functions.ForceEndPursuit(_pursuit);
                Game.LogTrivial("-!!- SAHighwayCallouts - |"+callout+"| - Callout was force ended by player -!!-");
                End();
            }
            base.Process();
        }

        private void EventsOnOnPulloverOfficerApproachDriver(LHandle handle)
        {
            var rPed = LSPD_First_Response.Mod.API.Functions.GetPulloverSuspect(handle);
            if (rPed == _suspect)
            {
                if (_scenario2Options == 1 && !_pursuitStarted)
                {
                    LSPD_First_Response.Mod.API.Functions.ForceEndCurrentPullover();
                    Game.LogTrivial("-!!- SAHighwayCallouts - |" + callout + "| - Running Sceario2 Approach Pursuit Option!");
                    Game.LogTrivial("-!!- SAHighwayCallouts - |" + callout + "| - Pursuit starting!");
                    if (_susBlip.Exists()) _susBlip.Delete();
                    _pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                    LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(_pursuit, true);
                    LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(_pursuit, _suspect);
                    LSPD_First_Response.Mod.API.Functions.PlayScannerAudio(
                        "ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                    _pursuitStarted = true;

                    if (Settings.PursuitBackup)
                    {
                        UltimateBackup.API.Functions.callPursuitBackup();
                        UltimateBackup.API.Functions.callPursuitBackup();
                    }
                }

                if (_scenario2Options == 2)
                {
                    Game.LogTrivial("-!!- SAHighwayCallouts - |" + callout + "| - Running Sceario2 Regular Traffic Stop Option!");
                }

                if (_scenario2Options == 3)
                {
                    if (!_shootoutStarted)
                    {
                        Game.LogTrivial("-!!- SAHighwayCallouts - |" + callout + "| - Running Sceario2 Shootout Traffic Stop Option!");
                        if (!_suspect.Inventory.HasLoadedWeapon) SAHC_Functions.NormalWeaponChooser(_suspect, -1, true);
                        _suspect.Tasks.LeaveVehicle(_victimCar, LeaveVehicleFlags.LeaveDoorOpen).WaitForCompletion();
                        _suspect.Tasks.AimWeaponAt(Game.LocalPlayer.Character, 1000);
                        _suspect.Tasks.FightAgainst(Game.LocalPlayer.Character);
                        _suspect.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Player, Relationship.Hate);
                        _suspect.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Hate);
                        _shootoutStarted = true;
                    }
                }
            }

            if (rPed != _suspect)
            {
                Game.DisplayHelp("You pulled over the wrong ~y~Vehicle~w~!");
                Game.LogTrivial("-!!- SAHighwayCallouts - |" + callout + "| - Wrong Vehicle pulled over try again!");
            }
        }

        public override void End()
        {
            if (_suspect) _suspect.Dismiss();
            if (_susBlip) _susBlip.Delete();
            if (_victim) _victim.Dismiss();
            if (_vicBlip) _vicBlip.Delete();
            if (_victimCar) _victimCar.Dismiss();
            if (_cop) _cop.Dismiss();
            if (_copCar) _copCar.Dismiss();
            if (_copBlip) _copBlip.Delete();
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
                        Game.DisplaySubtitle("~y~Player:~w~ Hello, what happened " + _maleFemale + "?");
                    }
                    if (_counter == 2)
                    {
                        Game.DisplaySubtitle("~o~Victim:~w~ The person was asking for help on the side of the road then got in my car and took off!");
                    }
                    if (_counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Okay, do you have any information I could use to locate your vehicle?");
                    }
                    if (_counter == 4)
                    {
                        Game.DisplaySubtitle("~o~Victim:~w~ Yes the color is ~b~"+_vicCarColor+"~w~ and the model is ~b~"+_vicCarModel+"~w~.");
                    }
                    if (_counter == 5)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Okay, do you know your license plate number by chance?");
                    }
                    if (_counter == 6)
                    {
                        if (doesVictimKnowLicensePlate == 1) Game.DisplaySubtitle("~g~Victim:~w~ Yes I do it's ~b~"+_vicCarPlateNum+"~w~.");
                        if (doesVictimKnowLicensePlate != 1) Game.DisplaySubtitle("~g~Victim:~w~ No, Im so sorry I dont!");
                        
                    }
                    if (_counter == 7)
                    {
                        if (doesVictimKnowLicensePlate == 1) Game.DisplaySubtitle("~y~Player:~w~ Okay thank you, would you like a officer to transport you?");
                        if (doesVictimKnowLicensePlate != 1) Game.DisplaySubtitle("~y~Player:~w~ That's okay. Would you like an officer to transport you somewhere?");
                    }

                    if (_counter == 8)
                    {
                        if (victimWantsTransport == 1) Game.DisplaySubtitle("~o~Victim:~w~ Yes I would please.");
                        if (victimWantsTransport != 1) Game.DisplaySubtitle("~o~Victim:~w~ No thank you I will be okay.");
                        _dialgueOver = true;
                    }

                    _counter++;
                    break;
            }
        }
    }
}