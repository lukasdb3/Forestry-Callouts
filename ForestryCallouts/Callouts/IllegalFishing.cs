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
    [CalloutInfo("IllegalFishing", CalloutProbability.Medium)]
    
    internal class IllegalFishing : Callout
    {
        private Ped suspect;
        private Blip susBlip;
        private Vector3 spawnpoint;
        private float susHeading;
        private bool onScene;
        private bool dialogueReady;
        private bool DialogueOver;
        private bool suspectsDead;
        private int ScenarioOneDialogueChooser = new Random().Next(1, 5);
        private int Counter;
        private string maleFemale;
        private bool notfiSent;
        private bool askedForFishingLicense;

        //timer variables
        private float timer;
        private bool timerPaused;
        
        private float timer2;

        public override bool OnBeforeCalloutDisplayed()
        {
            Game.LogTrivial("-!!- Forestry Callouts - |IllegalFishing| - Callout Displayed -!!-");
            CalloutMessage = "~g~Illegal Fishing Reported";
            CalloutAdvisory = "~b~Dispatch:~w~ Person possibly fishing without a license, Respond Code 2";
            
            ForestryCallouts.SimpleFunctions.SPFunctions.IllegalFishingSpawnChooser(out spawnpoint, out susHeading);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT_03 CRIME_DISTURBING_THE_PEACE_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_02_02", spawnpoint);
            CalloutPosition = spawnpoint;
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, spawnpoint);
            
            return base.OnBeforeCalloutDisplayed();
        }
        
        public override void OnCalloutDisplayed()
        {
            if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendCalloutDetails(this, "CODE 2", "SAPR");
            Game.LogTrivial("-!!- Forestry Callouts - |IllegalFishing| Callout displayed -!!-");

            base.OnCalloutDisplayed();
        }

        public override void OnCalloutNotAccepted()
        {
            if (!CIPluginChecker.IsCalloutInterfaceRunning) LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("OTHER_UNITS_TAKING_CALL");

            base.OnCalloutNotAccepted();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("-!!- Forestry Callouts - |IllegalFishing| - Callout Accepted -!!-");
            CFunctions.SpawnCountryPed(out suspect, spawnpoint, susHeading);
            suspect.Tasks.PlayAnimation("amb@world_human_stand_fishing@base", "base", 5, AnimationFlags.Loop);
            if (suspect.IsMale)
            {
                maleFemale = "sir";
            }
            else
            {
                maleFemale = "mam";
            }
            
            susBlip = suspect.AttachBlip();
            susBlip.EnableRoute(Color.Yellow);
            susBlip.Color = Color.Red;
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (Game.LocalPlayer.Character.DistanceTo(suspect) <= 10f && !onScene)
            {
                if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendMessage(this, "Officer is on scene.");
                susBlip.IsRouteEnabled = false;
                Game.DisplayHelp("Press ~r~'" + IniSettings.InteractionKey + "'~w~ to stop the ~r~Suspect~w~.", false);
                onScene = true;
            }

            if (Game.IsKeyDown(IniSettings.InputInteractionKey) && !dialogueReady)
            {
                suspect.Tasks.Clear();
                suspect.Tasks.StandStill(-1);
                suspect.Heading = Game.LocalPlayer.Character.Heading + 180f;
                Game.DisplayHelp("Press ~r~'" + IniSettings.DialogueKey + "'~w~ to start dialogue with the ~r~Suspect~w~.", false);
                dialogueReady = true;
            }

            if (dialogueReady && onScene && Game.IsKeyDown(IniSettings.InputDialogueKey) && !DialogueOver)
            {
                ScenarioOneDialogue();
            }
            
            if (DialogueOver && !notfiSent)
            {
                Game.DisplayHelp("Press ~r~'" + IniSettings.InteractionKey + "'~w~ to get the ~r~Suspects~w~ fishing license.", false);
                notfiSent = true;
            }
            if (notfiSent && Game.IsKeyDown(IniSettings.InputInteractionKey) && !askedForFishingLicense)
            {
                askedForFishingLicense = true;
                SimpleFunctions.MFunctions.AskForFishingLicense();
            }
            if (askedForFishingLicense)
            {
                if (IniSettings.EnableEndCalloutHelpMessages)
                {
                    Game.DisplayHelp("Press ~r~'" + IniSettings.EndCalloutKey + "'~w~ at anytime to end the callout", false);
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
                    MFunctions.SendMessage(this, "Illegal Fishing code 4");
                }
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition(
                    "OFFICERS_REPORT_03 OP_CODE OP_4", spawnpoint);
                Game.DisplayNotification("~g~Dispatch:~w~ All Units, Illegal Fishing Code 4");
                Game.LogTrivial("-!!- Forestry Callouts - |IllegalFishing| - Callout was force ended by player -!!-");
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
                        Game.DisplaySubtitle("~g~Suspect:~w~ Ohh. Im just trying to catch some fish.");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Okay, could I see your license?");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~g~Suspect:~w~ Yeah of course.");
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
                        Game.DisplaySubtitle("~g~Suspect:~w~ Horrible im not getting any bites! ");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Ahh that sucks, could I see your fishing license please?");
                    }

                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~g~Suspect:~w~ Yeah of course, here you go");
                        DialogueOver = true;
                    }
                    Counter++;
                    break;
                
                case 3:
                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello, what are you fishing for today, " + maleFemale + ".");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~g~Suspect:~w~ Im trying to catch some bass.");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Ahh any luck? Can I also see your license please?");
                    }

                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~g~Suspect:~w~ No luck yet, and of course here you go.");
                        DialogueOver = true;
                    }
                    Counter++;
                    break;
                
                case 4:
                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello, have you had any luck out here " + maleFemale + "?");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~g~Suspect:~w~ Yeah, I caught a couple but threw them back.");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Oh awesome! Can I see your fishing license please?");
                    }

                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~g~Suspect:~w~ Yep here you go.");
                        DialogueOver = true;
                    }
                    Counter++;
                    break;
            }
        }

        public override void End()
        {
            if (suspect)
            {
                suspect.Dismiss();
            }

            if (susBlip)
            {
                susBlip.Delete();
            }
            
            base.End();
        }
    }
}