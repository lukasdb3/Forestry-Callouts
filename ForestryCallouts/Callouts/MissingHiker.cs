using System;
using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using System.Drawing;
using ForestryCallouts.Ini;
using ForestryCallouts.SimpleFunctions;
using ForestryCallouts.SimpleFunctions.Logger;

namespace ForestryCallouts.Callouts
{
    [CalloutInfo("MissingHiker", CalloutProbability.Medium)]
    internal class MissingHiker : Callout
    {
        private float Heading;
        private Ped Suspect;
        private Vector3 PedSpawnpoint;
        private Blip SuspectAreaBlip;
        private bool FirstBlip;
        private Vector3 SearchArea;
        private bool OnScene;
        private bool RanMessage;
        private bool CalloutAccepted;
        private bool FoundHiker;
        private bool BeforeDialogueStart;
        private int ScenarioDrunkDialogueChooser = new Random().Next(1, 3);
        private int Counter;
        private bool DialogueOver;
        private string MaleFemale;
        private int WantedDrunkOrNothing = new Random().Next(1, 4);
        private bool PursuitStarted;
        private LHandle Pursuit;
        private WeaponDescriptor SuspectsGun;
        private int ScenarioWantedDialogueChooser = new Random().Next(1, 3);
        private int ScenarioNothingChangedDialogueChooser = new Random().Next(1, 3);

        //timer Shit
        private float timer;
        private bool IsTimerPaused;
        

        public override bool OnBeforeCalloutDisplayed()
        {
            CalloutMessage = ("~g~Missing Hiker Reported");
            CalloutAdvisory = ("~b~Dispatch:~w~ Search for the person, may be ~r~disorientated~w~. Respond Code 2");
            ForestryCallouts.SimpleFunctions.SPFunctions.MissingHikkerSpawnChooser(out PedSpawnpoint, out Heading);
            CalloutPosition = PedSpawnpoint;
            ShowCalloutAreaBlipBeforeAccepting(PedSpawnpoint, 30f);
            AddMinimumDistanceCheck(30f, PedSpawnpoint);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("WE_HAVE_01 ASSISTANCE_REQUIRED_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_02_02", PedSpawnpoint);
            return base.OnBeforeCalloutDisplayed();
        }
        
        public override void OnCalloutDisplayed()
        {
            if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendCalloutDetails(this, "CODE 3", "SAPR");
            LFunctions.Log(this, "Callout displayed!");

            base.OnCalloutDisplayed();
        }

        public override void OnCalloutNotAccepted()
        {
            if (!CIPluginChecker.IsCalloutInterfaceRunning) LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("OTHER_UNITS_TAKING_CALL");

            base.OnCalloutNotAccepted();
        }
        public override bool OnCalloutAccepted()
        {
            LFunctions.Log(this, "Callout accepted!");
            ForestryCallouts.SimpleFunctions.CFunctions.SpawnHikerPed(out Suspect, PedSpawnpoint, Heading);
            if (WantedDrunkOrNothing == 1)
            {
                ForestryCallouts.SimpleFunctions.CFunctions.SetWanted(Suspect, true);
                LFunctions.Log(this, "Hiker is wanted!");
            }
            if (WantedDrunkOrNothing == 2)
            {
                ForestryCallouts.SimpleFunctions.CFunctions.SetDrunk(Suspect, true);
                LFunctions.Log(this, "Hiker is drunk!");
            }
            if (WantedDrunkOrNothing == 3)
            {
                LFunctions.Log(this, "Hiker is wasn't changed!");
            }
            
            if (Suspect.IsMale)
            {
                MaleFemale = "sir";
            }
            if (Suspect.IsFemale)
            {
                MaleFemale = "mam";
            }
            IsTimerPaused = false;
            FirstBlip = true;
            LFunctions.Log(this, "Timer started!");
            CalloutAccepted = true;
            timer = 0f;
            return base.OnCalloutAccepted();
        }
        public override void Process()
        {
            if (!FoundHiker)
            {
                if (!IsTimerPaused && CalloutAccepted)
                {
                    timer++;
                }
                if (FirstBlip && timer >= 1f || !FirstBlip && timer >= 1250f)
                {
                    if (SuspectAreaBlip.Exists())
                    {
                        SuspectAreaBlip.Delete();
                    }
                    var position = Suspect.Position;
                    SearchArea = position.Around2D(10f, 50f);
                    SuspectAreaBlip = new Blip(SearchArea, 65f) { Color = Color.Yellow, Alpha = .5f };
                    SuspectAreaBlip.EnableRoute(Color.Yellow);
                    if (FirstBlip)
                    {
                        IsTimerPaused = true;
                        LFunctions.Log(this, "Timer paused!");
                        FirstBlip = false;
                    }
                    if (!FirstBlip)
                    {
                        timer = 0f;
                    }
                }
                if (Game.LocalPlayer.Character.DistanceTo(PedSpawnpoint) <= 175f && !OnScene)
                {
                    Suspect.Tasks.Wander();
                    Game.DisplayNotification("Look for the ~r~Missing Hiker~w~ in the ~y~Yellow~w~ Circle");
                    IsTimerPaused = false;
                    SuspectAreaBlip.DisableRoute();
                    OnScene = true;
                    LFunctions.Log(this, "Timer unpaused!");
                }
                if (Game.LocalPlayer.Character.DistanceTo(Suspect) <= 10f && !FoundHiker)
                {
                    if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendMessage(this, "Officer is on scene.");
                    LFunctions.Log(this, "Hiker is found!");
                    FoundHiker = true;
                    IsTimerPaused = true;
                    if (SuspectAreaBlip.Exists())
                    {
                        SuspectAreaBlip.Delete();
                    }
                    SuspectAreaBlip = Suspect.AttachBlip();
                    SuspectAreaBlip.Color = Color.OrangeRed;
                    Game.DisplaySubtitle("Go talk to the ~r~Missing Hiker~w~");
                }
            }
            if (FoundHiker)
            {
                if (Game.LocalPlayer.Character.DistanceTo(Suspect) <= 5f && !BeforeDialogueStart)
                {
                    Suspect.Tasks.StandStill(-1);
                    Suspect.Heading = Game.LocalPlayer.Character.Heading + 180f;
                    Game.DisplayHelp("Press ~r~'"+IniSettings.DialogueKey+"'~w~ to talk to the suspect");
                    BeforeDialogueStart = true;
                }
                if (BeforeDialogueStart && Game.IsKeyDown(IniSettings.InputDialogueKey) && !DialogueOver && WantedDrunkOrNothing == 1) //if ped is wanted
                {
                    ScenarioWantedDialogue();
                }
                if (BeforeDialogueStart && Game.IsKeyDown(IniSettings.InputDialogueKey) && !DialogueOver && WantedDrunkOrNothing == 2) //if ped is drunk
                {
                    ScenarioDrunkDialogue();
                }
                if (BeforeDialogueStart && Game.IsKeyDown(IniSettings.InputDialogueKey) && !DialogueOver && WantedDrunkOrNothing == 3) //if ped is didnt change
                {
                    ScenarioNothingChangedDialogue();
                }
            }
            //End Shit
            if (Suspect.IsDead || PursuitStarted && !LSPD_First_Response.Mod.API.Functions.IsPursuitStillRunning(Pursuit))
            {
                if (Ini.IniSettings.EnableEndCalloutHelpMessages)
                {
                    Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                }
            }
            if (Game.IsKeyDown(IniSettings.InputEndCalloutKey)) //If player presses "End" it will forcefully clean the callout up
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", PedSpawnpoint);
                Game.DisplayNotification("~g~Dispatch:~w~ All Units, Missing Hiker Code 4");
                if (CIPluginChecker.IsCalloutInterfaceRunning)
                {
                    MFunctions.SendMessage(this, "Missing Hiker code 4");
                }
                LFunctions.Log(this, "Callout was force ended by player!");
                End();
            }
            if (Game.LocalPlayer.Character.IsDead)
            {
                LFunctions.Log(this, "Player died callout ending!");
                End();
            }
            base.Process();
        }

        private void ScenarioWantedDialogue()
        {

            switch (ScenarioWantedDialogueChooser)
            {
                case 1:
                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + " you match the description of a missing hiker.");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Ummm. I dont know what your talking about...");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Do you mind if we talk for a bit so I can get more information.");
                    }
                    if (Counter == 4)
                    {
                        int PursuitOrShoot = new Random().Next(1, 3);
                        if (PursuitOrShoot == 1)
                        {
                            Game.DisplaySubtitle("~r~Suspect:~w~ Officer no uhhh... I can't go back!.. RUNNNN");
                            Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                            LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                            PursuitStarted = true;
                            DialogueOver = true;
                        }
                        if (PursuitOrShoot == 2)
                        {
                            Game.DisplaySubtitle("~r~Suspect:~w~ Officer no uhhh... I can't go back!.. DIEEE!");
                            SuspectsGun = Suspect.Inventory.GiveNewWeapon("weapon_combatpistol", -1, true);
                            Suspect.Inventory.EquippedWeapon = SuspectsGun;
                            Suspect.Tasks.AimWeaponAt(Game.LocalPlayer.Character, -1).WaitForCompletion(400);
                            Suspect.Tasks.FireWeaponAt(Game.LocalPlayer.Character, -1, FiringPattern.DelayFireByOneSecond);
                            DialogueOver = true;
                        }
                    }
                    Counter++;
                    break;

                case 2:
                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + " your clothes match the description of a missing hiker.");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Why does that concern you? It doesn't matter if im missing or not leave me alone.");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Can I please talk to you for a second to gather more information.");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Yeah, I supose but don't take long.");
                    }
                    if (Counter == 5)
                    {
                        Game.DisplaySubtitle("No further dialogue take appropriate action");
                        DialogueOver = true;
                    }
                    Counter++;
                    break;
            }

        }

        private void ScenarioDrunkDialogue()
        {

            switch (ScenarioDrunkDialogueChooser)
            {
                case 1:
                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + " there is reports of a missing hiker, and you match the description.");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ No thats not me officer leave me alone!");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Have you had anything to drink, you look like you may be intoxicated.");
                    }
                    if (Counter == 4)
                    {
                        int PursuitOrShoot = new Random().Next(1, 3);
                        if (PursuitOrShoot == 1)
                        {
                            Game.DisplaySubtitle("~r~Suspect:~w~ Officer please... Of Course not.. RUNNNN");
                            Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                            LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                            PursuitStarted = true;
                            DialogueOver = true;
                        }
                        if (PursuitOrShoot == 2)
                        {
                            SuspectsGun = Suspect.Inventory.GiveNewWeapon("weapon_combatpistol", -1, true);
                            Suspect.Inventory.EquippedWeapon = SuspectsGun;
                            Suspect.Tasks.AimWeaponAt(Game.LocalPlayer.Character, -1).WaitForCompletion(400);
                            Suspect.Tasks.FireWeaponAt(Game.LocalPlayer.Character, -1, FiringPattern.DelayFireByOneSecond);
                            DialogueOver = true;
                        }
                    }
                    Counter++;
                    break;

                case 2:
                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + " there is reports of a missing hiker, and you match the description.");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Why does that concern you? It doesn't matter if im missing or not leave me alone.");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Are you under the influence of any drugs or alcohol?");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ No.. Uh.. It's apple juice that im under the influence of.");
                    }
                    if (Counter == 5)
                    {
                        Game.DisplaySubtitle("No further dialogue take appropriate action");
                        DialogueOver = true;
                    }
                    Counter++;
                    break;
            }

        }

        private void ScenarioNothingChangedDialogue()
        {

            switch (ScenarioNothingChangedDialogueChooser)
            {
                case 1:
                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + " there is reports of a missing hiker, and you match the description.");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Yes, that is probably me officer. I am very tired and feel like I might pass out so I am taking my time to get home!");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Okay, would you like me to call medical services to get you some help?");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Yes, that would be great officer, thank you for coming.");
                    }
                    if (Counter == 5)
                    {
                        Game.DisplaySubtitle("No further dialogue take appropriate action");
                        DialogueOver = true;
                    }
                    Counter++;
                    break;

                case 2:
                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + " there is reports of a missing hiker, and you match the description.");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Why does that concern you? It doesn't matter if im missing or not leave me alone.");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ I am worried for your safety, can you talk to me for a second?");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ No! I dont want to talk to anyone leave me alone!");
                    }
                    if (Counter == 5)
                    {
                        int WanderOrPursuit = new Random().Next(1, 3);
                        if (WanderOrPursuit == 1)
                        {
                            Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                            LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                            PursuitStarted = true;
                            DialogueOver = true;
                        }
                        if (WanderOrPursuit == 2)
                        {
                            Game.DisplaySubtitle("No further dialogue take appropriate action");
                            Suspect.Tasks.Wander();
                            DialogueOver = true;
                        }
                    }
                    Counter++;
                    break;
            }

        }

        public override void End()
        {
            if (Suspect.Exists())
            {
                Suspect.Dismiss();
            }
            if (SuspectAreaBlip.Exists())
            {
                SuspectAreaBlip.Delete();
            }
            LFunctions.Log(this, "Cleaned up!");
            base.End();
        }
    }
}
