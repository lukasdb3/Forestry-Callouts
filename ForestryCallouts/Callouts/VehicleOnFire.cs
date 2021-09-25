using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using System.Drawing;
using Rage.Native;
using ForestryCallouts.Ini;

namespace ForestryCallouts.Callouts
{
    [CalloutInfo("VehicleOnFire", CalloutProbability.Low)]
    internal class VehicleOnFire : Callout
    {
        private bool SuspectIsViolent;
        private Vector3 Spawnpoint;
        private float Heading;
        private Vehicle SusVehicle;
        private Blip SusVehicleBlip;
        private bool OnScene;
        private bool ToFar;
        private Vector3 FireSpawnpoint;
        private Vector3 FireSpawnpoint2;
        private bool FirePutOut;
        private Ped Suspect;
        private Vector3 SearchArea;
        private Blip SuspectAreaBlip;
        private bool FoundSuspect;
        private WeaponDescriptor SuspectsJerryCan;
        private WeaponDescriptor SuspectsGun;
        private int Scenario = new Random().Next(1, 2);
        private LHandle Pursuit;
        private bool PursuitStarted;
        private bool ScenarioOneRan;
        private Vector3 PedSpawnpoint;
        private float PedHeading;
        private bool TimerSet;
        private float timer;
        private bool FirstSearchArea;
        private bool FirstSearchAreaGiven;
        private bool SuspectSpawned;
        private bool StartTimer;



        public override bool OnBeforeCalloutDisplayed()
        {
            Game.LogTrivial("-!!- Forestry Callouts - |VehicleOnFire| - Callout Displayed");
            SimpleFunctions.CFunctions.SuspectViolChooser(out SuspectIsViolent);
            CalloutMessage = ("~g~Vehicle Arson Reported");
            CalloutAdvisory = ("~b~Dispatch:~w~ Reports of vehicle set on fire by a person, Respond ~r~Code 3~w~");
            ForestryCallouts.SimpleFunctions.SPFunctions.VehicleOnFireSpawnChooser(out Spawnpoint, out Heading, out PedSpawnpoint, out PedHeading);
            CalloutPosition = Spawnpoint;
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, Spawnpoint);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("WE_HAVE_01 ASSISTANCE_REQUIRED_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_03_02", Spawnpoint);

            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            ForestryCallouts.SimpleFunctions.CFunctions.SpawnOffroadCar(out SusVehicle, Spawnpoint, Heading);
            SusVehicleBlip = SusVehicle.AttachBlip();
            SusVehicleBlip.Color = Color.Red;
            SusVehicleBlip.EnableRoute(Color.Yellow);

            FireSpawnpoint = SusVehicle.GetOffsetPositionFront(1.5f);
            FireSpawnpoint2 = SusVehicle.Position;
            SusVehicle.FuelTankHealth = 20f;
            Game.LogTrivial("-!!- Forestry Callouts - |VehicleOnFire| - Callout accepted! -!!-");
            return base.OnCalloutAccepted();
        }
        public override void Process()
        {
            if (Game.LocalPlayer.Character.DistanceTo(SusVehicle) <= 250f && !OnScene)
            {
                ForestryCallouts.SimpleFunctions.CFunctions.FireControl(FireSpawnpoint, 2, false);
                ForestryCallouts.SimpleFunctions.CFunctions.FireControl(FireSpawnpoint2, 2, false);
                Game.LogTrivial("-!!- Forestry Callouts - |VehicleOnFire| - Fires Spawned! -!!-");
                OnScene = true;
                TimerSet = true;
                FirstSearchArea = true;
            }
            if (Game.LocalPlayer.Character.DistanceTo(SusVehicle) <= 25f && !StartTimer)
            {
                StartTimer = true;
                Game.LogTrivial("-!!- Forestry Callouts - |VehicleOnFire| - Timer started! -!!-");
            }
            if (!FoundSuspect && TimerSet && StartTimer)
            {
                timer++;
            }
            if (!SuspectSpawned && timer >= 1350f)
            {
                SusVehicleBlip.IsRouteEnabled = false;
                ForestryCallouts.SimpleFunctions.CFunctions.SpawnCountryPed(out Suspect, PedSpawnpoint, PedHeading);
                ForestryCallouts.SimpleFunctions.CFunctions.PedPersonaChooser(in Suspect, false, true);
                SuspectsJerryCan = Suspect.Inventory.GiveNewWeapon("weapon_petrolcan", -1, true);
                Suspect.Inventory.EquippedWeapon = SuspectsJerryCan;
                Suspect.Tasks.Wander();
                SuspectSpawned = true;
                Game.LogTrivial("-!!- Forestry Callouts - |VehicleOnFire| - Suspect Spawned -!!-");
            }
            if (TimerSet)
            {
                if (timer >= 1400f && FirstSearchArea)
                {
                    LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("WE_HAVE_01 SUSPECT_LAST_SEEN_01 IN_OR_ON_POSITION", Spawnpoint);
                    TimerSet = true;
                    FirstSearchArea = false;
                    FirePutOut = true;
                }
                if (FirstSearchAreaGiven && !FoundSuspect)
                {
                    Game.DisplayHelp("Search for the ~r~Suspect~w~ in the ~y~Yellow~w~ circle");
                }
                if (timer >= 1400f && !FoundSuspect && FirstSearchArea || timer >= 1350f && !FoundSuspect && !FirstSearchArea)
                {
                    if (SuspectAreaBlip.Exists())
                    {
                        SuspectAreaBlip.Delete();
                    }
                    var position = Suspect.Position;
                    SearchArea = position.Around2D(10f, 35f);
                    SuspectAreaBlip = new Blip(SearchArea, 45f) { Color = Color.Yellow, Alpha = .5f };
                    timer = 0f;
                    if (FirstSearchAreaGiven)
                    {
                        Game.LogTrivial("-!!- Forestry Callouts - |VehicleOnFire| - Suspect's location has been updated! -!!-");
                        Game.DisplayNotification("~b~Dispatch:~w~ Suspect's location updated");
                        LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("WE_HAVE_01 SUSPECT_LAST_SEEN_01 IN_OR_ON_POSITION", Spawnpoint);
                    }
                    if (!FirstSearchAreaGiven)
                    {
                        Game.LogTrivial("-!!- Forestry Callouts - |VehicleOnFire| - Suspect's location has been pinned! -!!-");
                        FirstSearchAreaGiven = true;
                    }
                }
            }
                if (FirePutOut && !FoundSuspect && Game.LocalPlayer.Character.DistanceTo(Suspect.Position) <= 10f)
                {
                if (SuspectAreaBlip.Exists())
                {
                    SuspectAreaBlip.Delete();
                }
                FoundSuspect = true;
                }
                if (FoundSuspect)
                {
                    if (!SuspectIsViolent && Scenario == 1 && !PursuitStarted)
                    {
                        Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                        LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                        LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                        Game.DisplaySubtitle("~r~Suspect:~w~ OH SHITT RUN!", 2500);
                        LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                        PursuitStarted = true;
                    }
                    if (SuspectIsViolent && Scenario == 1 && !ScenarioOneRan)
                    {
                        int PursuitOrShoot = new Random().Next(1, 3);
                        if (PursuitOrShoot == 1 && !PursuitStarted)
                        {
                            Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                            LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                            Game.DisplaySubtitle("~r~Suspect:~w~ OH SHITT RUN!", 2500);
                            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                            PursuitStarted = true;
                        }
                        if (PursuitOrShoot == 2)
                        {
                            SuspectsGun = Suspect.Inventory.GiveNewWeapon("weapon_combatpistol", -1, true);
                            Suspect.Inventory.EquippedWeapon = SuspectsGun;
                            Game.DisplaySubtitle("~r~Suspect:~w~ OH SHIT THE COPS!");
                            Suspect.Tasks.AimWeaponAt(Game.LocalPlayer.Character, -1).WaitForCompletion(400);
                            Suspect.Tasks.FireWeaponAt(Game.LocalPlayer.Character, -1, FiringPattern.DelayFireByOneSecond);
                        }
                        ScenarioOneRan = true;
                    }
                    if (Scenario == 2)
                    {
                        SuspectAreaBlip = Suspect.AttachBlip();
                    }
                }

                if (FirePutOut && Suspect.IsDead || FirePutOut && !Suspect.Exists() || PursuitStarted && !LSPD_First_Response.Mod.API.Functions.IsPursuitStillRunning(Pursuit))
                {
                    if (SuspectAreaBlip.Exists())
                    {
                        SuspectAreaBlip.Delete();
                    }
                if (Ini.IniSettings.EnableEndCalloutHelpMessages)
                {
                    Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                }
            }
                if (Game.IsKeyDown(IniSettings.InputEndCalloutKey)) //If player presses "End" it will forcefully clean the callout up
                {
                    LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", Spawnpoint);
                    Game.DisplayNotification("~g~Dispatch:~w~ All Units, Vehicle On Fire Code 4");
                    Game.LogTrivial("-!!- Forestry Callouts - |VehicleOnFire| - Callout was force ended by player -!!-");
                    End();
                }
                if (Game.LocalPlayer.Character.IsDead)
                {
                    Game.LogTrivial("-!!- Forestry Callouts - |VehicleOnFire| - Callout was ended due to players death -!!-");
                    End();
                }

                base.Process();
        }
        
        public override void End()
        {
            if (SusVehicle.Exists())
            {
                SusVehicle.Dismiss();
            }
            if (SusVehicleBlip.Exists())
            {
                SusVehicleBlip.Delete();
            }
            if (Suspect.Exists()) 
            {
                Suspect.Dismiss();
            }
            if (SuspectAreaBlip.Exists())
            {
                SuspectAreaBlip.Delete();
            }
            NativeFunction.Natives.STOP_FIRE_IN_RANGE(FireSpawnpoint, 15f);
            NativeFunction.Natives.STOP_FIRE_IN_RANGE(FireSpawnpoint2, 15f);
            Game.LogTrivial("-!!- Forestry Callouts - |VehicleOnFire| - Cleaned Up -!!-");

            base.End();
        }
    }
}
