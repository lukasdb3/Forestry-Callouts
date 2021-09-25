using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using ForestryCallouts.Ini;

namespace ForestryCallouts.Callouts
{
    
    [CalloutInfo("DangerousPerson", CalloutProbability.Medium)]
    
    internal class DangerousPerson : Callout
    {
        //variables
        private int scenario = new Random().Next(1, 5);
        
        private Ped suspect;
        private Blip susBlip;
        private Vector3 spawnpoint;

        private WeaponDescriptor suspectsWeapon;
        private bool beforeOnScene;
        private bool onScene;
        private bool suspectFound;
        private bool suspectsDead;
        
        
        //searchArea shits
        private Vector3 SearchArea;

        //timer variables
        private float timer;
        private bool timerPaused;

        public override bool OnBeforeCalloutDisplayed()
        {
            SimpleFunctions.SPFunctions.DangerousPersonSpawnChooser(out spawnpoint);
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, spawnpoint);

            CalloutMessage = ("~g~Dangerous Person Reported");
            switch (scenario)
            {
                case 1:
                    CalloutAdvisory = ("~b~Dispatch:~w~ Person with a ~r~automated rifle~w~ reported, Respond ~r~Code 3~w~");
                    LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_SHOTS_FIRED_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_03_03", spawnpoint);
                    break;
                case 2:
                    CalloutAdvisory = ("~b~Dispatch:~w~ Person with a ~r~pistol~w~ reported, Respond ~r~Code 3~w~");
                    LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_SHOTS_FIRED_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_03_03", spawnpoint);
                    break;
                case 3:
                    LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("WE_HAVE ASSISTANCE_REQUIRED_02 IN_OR_ON_POSITION UNITS_RESPOND_CODE_03_03", spawnpoint);
                    CalloutAdvisory = ("~b~Dispatch:~w~ Person with a ~r~melee weapon~w~ reported, Respond ~r~Code 3~w~");
                    break;
                case 4:
                    LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("WE_HAVE ASSISTANCE_REQUIRED_02 IN_OR_ON_POSITION UNITS_RESPOND_CODE_03_03", spawnpoint);
                    CalloutAdvisory = ("~b~Dispatch:~w~ Person with a ~r~shotgun~w~ reported, Respond ~r~Code 3~w~");
                    break;
                    
            }
            CalloutPosition = spawnpoint;

            Game.LogTrivial("-!!- Forestry Callouts - |DangerousPerson| Callout displayed -!!-");
            
            return base.OnBeforeCalloutDisplayed();
        }
        
        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("-!!- Forestry Callouts - |DangerousPerson| - Callout Accepted");
            SimpleFunctions.CFunctions.SpawnCountryPed(out suspect, spawnpoint, new Random().Next(0, 361));
            suspect.Tasks.Wander();
            
            var position = suspect.Position;
            SearchArea = position.Around2D(10f, 35f);
            susBlip = new Blip(SearchArea, 45f) { Color = Color.Yellow, Alpha = .5f };
            susBlip.EnableRoute(Color.Yellow);

            Game.LocalPlayer.Character.RelationshipGroup = "PLAYER";
            suspect.RelationshipGroup = "SUSPECT";
            
            switch (scenario)
            {
                case 1:
                    ForestryCallouts.SimpleFunctions.CFunctions.RifleWeaponChooser(suspect, -1 , true);
                    break;
                
                case 2:
                    ForestryCallouts.SimpleFunctions.CFunctions.PistolWeaponChooser(suspect, -1, true);
                    break;
                
                case 3:
                    ForestryCallouts.SimpleFunctions.CFunctions.MeleeWeaponChooser(suspect, -1, true);
                    break;
                case 4:
                    ForestryCallouts.SimpleFunctions.CFunctions.ShotgunWeaponChooser(suspect, -1, true);
                    break;
            }
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (Game.LocalPlayer.Character.DistanceTo(spawnpoint) <= 250f && !suspectFound)
            {
                susBlip.IsRouteEnabled = false;
                Game.DisplayHelp("Search for the ~r~Suspect~w~ in the ~y~Yellow~w~ circle");

                if (!timerPaused)
                {
                    timer++;
                }
            
                if (timer == 50)
                {
                    timerPaused = true;
                    if (scenario == 1)
                    {
                        suspect.Tasks.FireWeaponAt(suspect, 1500, FiringPattern.FullAutomatic).WaitForCompletion();
                    }

                    if (scenario == 2 || scenario == 4)
                    {
                        suspect.Tasks.FireWeaponAt(suspect, 1500, FiringPattern.DelayFireByOneSecond)
                            .WaitForCompletion();
                    }

                    suspect.Tasks.ReloadWeapon().WaitForCompletion();
                    suspect.Tasks.Wander();
                    timerPaused = false;
                }
                
                if (timer == 250)
                {
                    timerPaused = true;
                    if (scenario == 1)
                    {
                        suspect.Tasks.FireWeaponAt(suspect, 1500, FiringPattern.FullAutomatic).WaitForCompletion();
                    }

                    if (scenario == 2 || scenario == 4)
                    {
                        suspect.Tasks.FireWeaponAt(suspect, 1500, FiringPattern.DelayFireByOneSecond)
                            .WaitForCompletion();
                    }

                    suspect.Tasks.ReloadWeapon().WaitForCompletion();
                    suspect.Tasks.Wander();
                    timerPaused = false;
                }
                
                if (timer == 650)
                {
                    timerPaused = true;
                    if (scenario == 1)
                    {
                        suspect.Tasks.FireWeaponAt(suspect, 1500, FiringPattern.FullAutomatic).WaitForCompletion();
                    }

                    if (scenario == 2 || scenario == 4)
                    {
                        suspect.Tasks.FireWeaponAt(suspect, 1500, FiringPattern.DelayFireByOneSecond)
                            .WaitForCompletion();
                    }

                    suspect.Tasks.ReloadWeapon().WaitForCompletion();
                    suspect.Tasks.Wander();
                    timerPaused = false;
                }

                if (timer == 1000)
                    {
                        if (susBlip.Exists())
                        {
                            susBlip.Delete();
                        }

                        var position = suspect.Position;
                        SearchArea = position.Around2D(10f, 35f);
                        susBlip = new Blip(SearchArea, 45f) {Color = Color.Yellow, Alpha = .5f};
                        Game.DisplayNotification("~b~Dispatch:~w~ Suspect's location updated");
                        LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition(
                            "WE_HAVE_01 SUSPECT_LAST_SEEN_01 IN_OR_ON_POSITION", spawnpoint);
                        Game.LogTrivial("-!!- Forestry Callouts - |DangerousPerson| - Suspect's location has been updated! -!!-");
                        Game.DisplayNotification("~b~Dispatch:~w~ Suspect's location updated");
                        LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("WE_HAVE_01 SUSPECT_LAST_SEEN_01 IN_OR_ON_POSITION", spawnpoint);
                        timer = 0;
                    }
                }

                if (Game.LocalPlayer.Character.DistanceTo(suspect.Position) <= 11f && !suspectFound)
                {
                    suspectFound = true;
                    Game.LogTrivial("-!!- Forestry Callouts - |DangerousPerson| - Suspect found!");
                    if (susBlip.Exists())
                    {
                        susBlip.Delete();
                    }

                    susBlip = suspect.AttachBlip();
                    susBlip.Color = Color.Red;
                    susBlip.IsRouteEnabled = false;

                    suspect.RelationshipGroup.SetRelationshipWith("PLAYER", Relationship.Hate);
                    Game.LocalPlayer.Character.RelationshipGroup.SetRelationshipWith("SUSPECT", Relationship.Hate);
                }

                if (Game.LocalPlayer.Character.DistanceTo(suspect.Position) <= 10f && !onScene && suspectFound)
                {
                    onScene = true;

                    switch (scenario)
                    {
                        case 1:
                            suspect.Tasks.FireWeaponAt(Game.LocalPlayer.Character, -1,
                                FiringPattern.FullAutomatic);
                            break;

                        case 2:
                            suspect.Tasks.FireWeaponAt(Game.LocalPlayer.Character, -1,
                                FiringPattern.DelayFireByOneSecond);
                            break;

                        case 3:
                            suspect.Tasks.FightAgainst(Game.LocalPlayer.Character);
                            break;
                           
                        case 4:
                            suspect.Tasks.FireWeaponAt(Game.LocalPlayer.Character, -1,
                                FiringPattern.BurstFirePumpShotgun);
                            break;
                    }
                }

                //End Script stufs
                if (Game.LocalPlayer.Character.IsDead)
                {
                    End();
                }

                if (suspect.IsDead && !suspectsDead)
                {
                    suspectsDead = true;
                    if (Ini.IniSettings.EnableEndCalloutHelpMessages)
                    {
                        Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                    }

                    if (susBlip.Exists())
                    {
                        susBlip.Delete();
                    }
                }

                if (Game.IsKeyDown(IniSettings.InputEndCalloutKey))
                {
                    LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition(
                        "OFFICERS_REPORT_03 OP_CODE OP_4", spawnpoint);
                    Game.DisplayNotification("~g~Dispatch:~w~ All Units, Dangerous Person Code 4");
                    Game.LogTrivial(
                        "-!!- Forestry Callouts - |DangerousPerson| - Callout was force ended by player -!!-");
                    End();
                }

                base.Process();
            }

        public override void End()
        {
            if (suspect.Exists())
            {
                suspect.Dismiss();
            }

            if (susBlip.Exists())
            {
                susBlip.Delete();
            }
            base.End();
        }
    }
}