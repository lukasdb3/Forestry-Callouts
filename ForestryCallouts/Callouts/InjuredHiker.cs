using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using System.Drawing;
using ForestryCallouts.Ini;
using ForestryCallouts.SimpleFunctions;

namespace ForestryCallouts.Callouts
{
    [CalloutInfo("InjuredHiker", CalloutProbability.Medium)]
    internal class InjuredHiker : Callout
    {
        private Vector3 Spawnpoint;
        private int randomPeds = new Random().Next(1, 3);
        private Ped Victim;
        private Ped Witness;
        private Blip WitnessBlip;
        private string MaleFemale;
        private string MaleFemaleVictim;
        private bool OnScene;
        private int rnd = new Random().Next(1, 4);
        private int Counter;
        private string HisHer;

        public override bool OnBeforeCalloutDisplayed()
        {
            SimpleFunctions.SPFunctions.SpawnChooser(out Spawnpoint);
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, Spawnpoint);

            CalloutMessage = ("~g~Injured Hiker Reported");
            if (rnd == 1)
            {
                CalloutAdvisory = ("~b~Dispatch:~w~ Victim is unconsious, may have hit their head on a rock. Respond ~r~Code 3~w~");
            }
            if (rnd == 2)
            {
                CalloutAdvisory = ("~b~Dispatch:~w~ Victim is unconsious, may be having a heart attack. Respond ~r~Code 3~w~");
            }
            if (rnd == 3)
            {
                CalloutAdvisory = ("~b~Dispatch:~w~ Victim is unconsious, may be dehydrated. Respond ~r~Code 3~w~");
            }

            CalloutPosition = Spawnpoint;
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("WE_HAVE ASSISTANCE_REQUIRED_02 IN_OR_ON_POSITION UNITS_RESPOND_CODE_03_02", Spawnpoint);

            Game.LogTrivial("-!!- Forestry Callouts - |InjuredHiker| - Callout displayed -!!-");
            


            return base.OnBeforeCalloutDisplayed();
        }
        
        public override void OnCalloutDisplayed()
        {
            if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendCalloutDetails(this, "CODE 3", "SAPR");
            Game.LogTrivial("-!!- Forestry Callouts - |InjuredHiker| Callout displayed -!!-");

            base.OnCalloutDisplayed();
        }

        public override void OnCalloutNotAccepted()
        {
            if (!CIPluginChecker.IsCalloutInterfaceRunning) LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("OTHER_UNITS_TAKING_CALL");

            base.OnCalloutNotAccepted();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("-!!- Forestry Callouts - |InjuredHiker| - Witness/Victim, Case " + randomPeds + " -!!-");
            switch (randomPeds)
            {
                case 1:
                    Victim = new Ped("a_f_y_hiker_01", Spawnpoint, 1f);
                    Victim.Kill();
                    Witness = new Ped("a_m_y_hiker_01", Victim.GetOffsetPositionRight(3f), Victim.Heading + 180f);
                    break;

                case 2:
                    Victim = new Ped("a_m_y_hiker_01", Spawnpoint, 1f);
                    Victim.Kill();
                    Witness = new Ped("a_f_y_hiker_01", Victim.GetOffsetPositionRight(3f), Victim.Heading + 180f);
                    break;
            }            
            Victim.IsPersistent = true;
            Victim.BlockPermanentEvents = true;
            Witness.IsPersistent = true;
            Witness.BlockPermanentEvents = true;

            WitnessBlip = Witness.AttachBlip();
            WitnessBlip.Color = Color.OrangeRed;
            WitnessBlip.EnableRoute(Color.Yellow);

            if (Witness.IsMale)
                MaleFemale = "sir";
            else
                MaleFemale = "mam";

            if (Victim.IsMale)
                MaleFemaleVictim = "guy";
            else
                 MaleFemaleVictim = "lady";

            if (Victim.IsMale)
            {
                HisHer = "his";
            }
            else
            {
                HisHer = "her";
            }

            Game.LogTrivial("-!!- Forestry Callouts - |InjuredHiker| - Callout accepted -!!-");

            return base.OnCalloutAccepted();
        }
        public override void Process() //Possibly add more dialogue maybe different outcomes of the victim being revived.
        {
            if (!OnScene && Game.LocalPlayer.Character.DistanceTo(Witness) <= 10f)
            {
                if (CIPluginChecker.IsCalloutInterfaceRunning) MFunctions.SendMessage(this, "Officer is on scene.");
                Game.LogTrivial("-!!- Forestry Callouts - |InjuredHiker| - Dialugoe, Case: " + rnd + " -!!-");
                Game.LogTrivial("-!!- Forestry Callouts - Starting main process -!!-");
                Game.DisplayHelp("Press ~r~'"+IniSettings.DialogueKey+"'~w~ to talk to the witness", false);
                OnScene = true;
            }
            if (OnScene && Game.IsKeyDown(IniSettings.InputDialogueKey))
            {
                Witness.Heading = Game.LocalPlayer.Character.Heading + 180f;
                Dialogue();
            }

            if (Game.IsKeyDown(IniSettings.InputEndCalloutKey))
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", Spawnpoint);
                Game.DisplayNotification("~g~Dispatch:~w~ All Units, Injured Hiker Code 4");
                Game.LogTrivial("-!!- Forestry Callouts - |InjuredHiker| - Callout was force ended by player -!!-");
                if (CIPluginChecker.IsCalloutInterfaceRunning)
                {
                    MFunctions.SendMessage(this, "Injured Hiker code 4");
                }
                End();
            }

            if (Game.LocalPlayer.IsDead)
            {
                End();
            }
                

            base.Process();
        }
        public override void End()
        {
            if (Victim.Exists())
            {
                Victim.Dismiss();
            }
            if (Witness.Exists())
            {
                Witness.Dismiss();
            }
            if (WitnessBlip.Exists())
            {
                WitnessBlip.Delete();
            }
            Game.LogTrivial("-!!- Forestry Callout - |InjuredHiker| - Cleaned Up -!!-");


            base.End();
        }
        private void Dialogue()
        {
            var step = 1;

            switch (rnd)
            {
                case 1:

                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + ", do you know what happend?");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~g~Witness:~w~ Yeah I do the " + MaleFemaleVictim + " tripped and hit " + HisHer + " head on a rock.");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Okay, did the " + MaleFemaleVictim + " become unconsicous right away?");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~g~Witness:~w~ Yes, the " + MaleFemaleVictim + " became unconsious right away.");
                    }
                    if (Counter == 5)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Okay thank you " + MaleFemale + " for the information. Please stand by to fill out a witness report.");
                    }
                    if (Counter == 6)
                    {
                        Game.DisplaySubtitle("~g~Witness:~w~ Okay will do.");
                    }
                    if (Counter == 7)
                    {
                        Game.DisplaySubtitle("No further dialogue, attend to the injured hiker");
                        if (Ini.IniSettings.EnableEndCalloutHelpMessages)
                        {
                            Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                        }
                    }

                    Counter++;

                    break;

                case 2:

                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + ", do you know what happened?");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~g~Witness:~w~ Yes I do, the " + MaleFemaleVictim + " said " + HisHer + " chest was painful near " + HisHer + " heart.");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Okay thank you for the information. Please stand by to fill out a witness report.");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~g~Witness:~w~ Okay will do officer.");
                    }
                    if (Counter == 5)
                    {
                        Game.DisplaySubtitle("No further dialogue, attend to the injured hiker");
                        if (Ini.IniSettings.EnableEndCalloutHelpMessages)
                        {
                            Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                        }
                    }

                    Counter++;

                    break;

                case 3:

                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + ", were you with the injured hiker?");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~g~Witness:~w~ Yes I was, " + MaleFemaleVictim + " said " + MaleFemaleVictim + " was really thirsty and wasn't feeling the best before she passed out.");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Okay thank you for the information. Please stand by to fill out a witness report.");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~g~Witness:~w~ Okay will do officer.");
                    }
                    if (Counter == 5)
                    {
                        Game.DisplaySubtitle("No further dialogue, attend to the injured hiker");
                        if (Ini.IniSettings.EnableEndCalloutHelpMessages)
                        {
                            Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                        }
                    }

                    Counter++;

                    break;
            }
            return;
        }
    }
}
