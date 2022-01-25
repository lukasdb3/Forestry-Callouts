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
using ForestryCallouts.SimpleFunctions;

namespace ForestryCallouts.Callouts
{
    
    [CalloutInfo("IllegalHunting", CalloutProbability.Medium)]
    
    internal class IllegalHunting : Callout
    {
        private Ped suspect;
        private Vector3 spawnpoint;
        private Blip suspectBlip;
        private int scenario = 2;
        private string maleFemale;
        private Vector3 SearchArea;
        private bool suspectFound;
        private bool onScene;
        private bool suspectsDead;
        private bool searchNotfiSent;
        private bool askedForHuntersLicense;
        private bool dialogueReady;
        private int Counter;
        private bool DialogueOver;
        private bool notfiSent;
        private int ScenarioOneDialogueChooser = new Random().Next(1, 5);
        

        //timer variables
        private float timer;
        private bool timerPaused;
        
        private float timer2;
        private bool timerPaused2;
        

        public override bool OnBeforeCalloutDisplayed()
        {
            //Scenario 1 = Person hunting illegally with a illegal firearm like an automated rifle.
            if (scenario == 1)
            {
                CalloutMessage = "~g~Illegal Hunting Reported";
                CalloutAdvisory = "~b~Dispatch:~w~ Person hunting with illegal weapon reported, Respond Code 2";
            }
            
            //Scenario 2 = Person possibly hunting without a license, go to the scene and check the license of the hunter with STP.
            if (scenario == 2)
            {
                CalloutMessage = "~g~Possible Illegal Hunting Reported";
                CalloutAdvisory = "~b~Dispatch:~w~ Person possibly hunting without a license, Respond Code 2";
            }

            ForestryCallouts.SimpleFunctions.SPFunctions.IllegalHuntingSpawnChooser(out spawnpoint);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT_03 CRIME_DISTURBING_THE_PEACE_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_02_02", spawnpoint);
            CalloutPosition = spawnpoint;
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, spawnpoint);
            return base.OnBeforeCalloutDisplayed();
        }
        
        public override void OnCalloutDisplayed()
        {
            if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendCalloutDetails(this, "CODE 2", "SAPR");
            Game.LogTrivial("-!!- Forestry Callouts - |IllegalHunting| Callout displayed -!!-");

            base.OnCalloutDisplayed();
        }

        public override void OnCalloutNotAccepted()
        {
            if (!CIPluginChecker.IsCalloutInterfaceRunning) LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("OTHER_UNITS_TAKING_CALL");

            base.OnCalloutNotAccepted();
        }
        
        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("-!!- Forestry Callouts - |IllegalHunting| - Callout accepted -!!-");
            ForestryCallouts.SimpleFunctions.CFunctions.SpawnHunterPed(out suspect, spawnpoint, new Random().Next(1, 361));
            suspect.Tasks.Wander();

            var position = suspect.Position;
            SearchArea = position.Around2D(10f, 35f);
            suspectBlip = new Blip(SearchArea, 45f) { Color = Color.Yellow, Alpha = .5f };
            suspectBlip.EnableRoute(Color.Yellow);
            
            //Gives the suspect a weapon depending on the scenario
            if (scenario == 1)
            {
                ForestryCallouts.SimpleFunctions.CFunctions.RifleWeaponChooser(suspect, -1 , true);
            }

            if (scenario == 2)
            {
                suspect.Inventory.GiveNewWeapon("weapon_pumpshotgun", -1, true);
            }
            
            //Dialgue shit for later
            if (suspect.IsMale)
            {
                maleFemale = "sir";
            }
            else
            {
                maleFemale = "mam";
            }
            return base.OnCalloutAccepted();
        }
        public override void Process()
        {
            //Shit for both scenarios
            if (Game.LocalPlayer.Character.DistanceTo(suspect) <= 250f)
            {
                suspectBlip.IsRouteEnabled = false;
                if (!searchNotfiSent)
                {
                    if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendMessage(this, "Search area sent out to responding officers");
                    Game.DisplayHelp("Search for the ~r~Suspect~w~ in the ~y~Yellow Circle~w~");
                    searchNotfiSent = true;
                }

                if (!timerPaused)
                {
                    timer++;
                }

                if (scenario == 1 && timer == 500)
                {
                    timerPaused = true;
                    suspect.Tasks.FireWeaponAt(suspect, 1500, FiringPattern.FullAutomatic).WaitForCompletion();

                    suspect.Tasks.ReloadWeapon().WaitForCompletion();
                    suspect.Tasks.Wander();
                    timerPaused = false;
                }
                
                if (scenario == 1 && timer == 800)
                {
                    timerPaused = true;
                    suspect.Tasks.FireWeaponAt(suspect, 1500, FiringPattern.FullAutomatic).WaitForCompletion();

                    suspect.Tasks.ReloadWeapon().WaitForCompletion();
                    suspect.Tasks.Wander();
                    timerPaused = false;
                }

                if (timer == 1000 && !suspectFound)
                {
                    if (suspectBlip.Exists())
                    {
                        suspectBlip.Delete();
                    }
                    var position = suspect.Position;
                    SearchArea = position.Around2D(10f, 35f);
                    suspectBlip = new Blip(SearchArea, 45f) { Color = Color.Yellow, Alpha = .5f };
                    Game.LogTrivial("-!!- Forestry Callouts - |IllegalHunting| - Suspect's location has been updated! -!!-");
                    Game.DisplayNotification("~b~Dispatch:~w~ Suspect's location updated");
                    LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("WE_HAVE_01 SUSPECT_LAST_SEEN_01 IN_OR_ON_POSITION", spawnpoint);
                    timer = 0f;
                }

                if (Game.LocalPlayer.Character.DistanceTo(suspect) <= 11f && !suspectFound)
                {
                    if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendMessage(this, "Officer is on scene. Suspect found.");
                    Game.LogTrivial("-!!- Forestry Callouts - |IllegalHunting| - Suspect found! -!!-");
                    suspectFound = true;
                    timer = 0f;
                    timerPaused = true;
                    
                    if (suspectBlip.Exists())
                    {
                        suspectBlip.Delete();
                    }

                    suspectBlip = suspect.AttachBlip();
                    suspectBlip.Color = Color.Red;
                    suspectBlip.IsRouteEnabled = false;
                }
            }
            //Scenario 1 shit
            if (scenario == 1)
            {
                //add shit
            }
           
           //Scenario 2 shit
           if (scenario == 2)
           {
               if (Game.LocalPlayer.Character.DistanceTo(suspect) <= 10f && !onScene)
               {
                   Game.DisplayHelp("Press ~r~'" + IniSettings.InteractionKey + "'~w~ to stop the ~r~hunter~w~.", false);
                   onScene = true;
               }

               if (Game.IsKeyDown(IniSettings.InputInteractionKey) && !dialogueReady)
               {
                   suspect.Tasks.StandStill(-1);
                   suspect.Heading = Game.LocalPlayer.Character.Heading + 180f;
                   Game.DisplayHelp("Press ~r~'" + IniSettings.DialogueKey + "'~w~ to start dialogue with the ~r~hunter~w~.", false);
                   dialogueReady = true;
               }

               if (dialogueReady && onScene && Game.IsKeyDown(IniSettings.InputDialogueKey) && !DialogueOver)
               {
                   ScenarioOneDialogue();
               }

               if (DialogueOver && !notfiSent)
               {
                   Game.DisplayHelp("Press ~r~'" + IniSettings.InteractionKey + "'~w~ to get the ~r~hunters~w~ hunting license.", false);
                   notfiSent = true;
               }
               if (notfiSent && Game.IsKeyDown(IniSettings.InputInteractionKey) && !askedForHuntersLicense)
               {
                   askedForHuntersLicense = true;
                   SimpleFunctions.MFunctions.AskForHuntingLicense();
               }
               if (askedForHuntersLicense)
               {
                   if (IniSettings.EnableEndCalloutHelpMessages)
                   {
                       Game.DisplayHelp("Press ~r~'" + IniSettings.EndCalloutKey + "'~w~ at anytime to end the callout", false);
                   }
               }
           }
            
           //End Script stufs
           if (Game.LocalPlayer.Character.IsDead)
           {
               End();
           }

           if (suspect.IsDead && !suspectsDead)
           {
               if (Ini.IniSettings.EnableEndCalloutHelpMessages)
               {
                   suspectsDead = true;
                   Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
               }
           }

           if (Game.IsKeyDown(IniSettings.InputEndCalloutKey))
           {
               if (CIPluginChecker.IsCalloutInterfaceRunning)
               {
                   MFunctions.SendMessage(this, "Animal attack code 4");
               }
               LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition(
                   "OFFICERS_REPORT_03 OP_CODE OP_4", spawnpoint);
               if (CIPluginChecker.IsCalloutInterfaceRunning)
               {
                   MFunctions.SendMessage(this, "Illegal Hunting code 4");
               }
               Game.DisplayNotification("~g~Dispatch:~w~ All Units, Illegal Hunting Code 4");
               Game.LogTrivial("-!!- Forestry Callouts - |IllegalHunting| - Callout was force ended by player -!!-");
               End();
           }
            base.Process();
        }
        
         private void ScenarioOneDialogue()
        {
            switch (ScenarioOneDialogueChooser)
            {
                case 1:
                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello, what are you doing out here " + maleFemale + "?");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~g~Hunter:~w~ Ohh. Im just trying to hunt some deer.");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Okay, could I see your license?");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~g~Hunter:~w~ Yeah of course.");
                        DialogueOver = true;
                    }
                    Counter++;
                    break;

                case 2:
                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello, how are you doing today " + maleFemale + ".");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~g~Hunter:~w~ I'm doing good, Whats the issue. Im just doing some hunting.");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ I need to see your hunting license to make sure your allowed to hunt.");
                    }

                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~g~Hunter:~w~ Ah okay, here you go.");
                        DialogueOver = true;
                    }
                    Counter++;
                    break;
                
                case 3:
                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello, what you hunting for today " + maleFemale + "?");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~g~Hunter:~w~ Im going for deer, haven't got any yet.");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Nice, can I see your hunting license before you continue?");
                    }

                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~g~Hunter:~w~ Yeah not a problem here you go.");
                        DialogueOver = true;
                    }
                    Counter++;
                    break;
                
                case 4:
                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello, have you got any deer today " + maleFemale + "?");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~g~Hunter:~w~ Not yet I im still trying to get one though.");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Ah bummer, can I see your hunting license please?");
                    }

                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~g~Hunter:~w~ Yeah not a issue here you go.");
                        DialogueOver = true;
                    }
                    Counter++;
                    break;
            }
        }
        public override void End()
        {
            if (suspect.Exists())
            {
                suspect.Dismiss();
            }

            if (suspectBlip.Exists())
            {
                suspectBlip.Delete();
            }
            
            base.End();
        }
    }
}