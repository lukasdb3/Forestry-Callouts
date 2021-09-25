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

namespace ForestryCallouts.Callouts
{

    [CalloutInfo("IntoxicatedHiker", CalloutProbability.Medium)]
    internal class IntoxicatedHiker : Callout
    {
        private Ped Suspect;
        private Blip SuspectBlip;
        private Vector3 Spawnpoint;
        private int Counter;
        private string MaleFemale;
        private bool OnScene;
        private int rnd = new Random().Next(1,5);
        private LHandle Pursuit;
        private bool CloseToCall;

        public override bool OnBeforeCalloutDisplayed()
        {
            SimpleFunctions.SPFunctions.SpawnChooser(out Spawnpoint);
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, Spawnpoint);
            CalloutMessage = ("~g~Intoxicated Hiker Reported");
            CalloutAdvisory = ("~b~Dispatch:~w~ Person could be ~r~violent~w~, use caution. Respond Code 2");
            CalloutPosition = Spawnpoint; 
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT_02 CROME_DISTURBING_THE_PEACE_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_02_02", Spawnpoint);

            Game.LogTrivial("-!!- Forestry Callouts - |IntoxicatedHiker| Callout displayed -!!-");
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            SimpleFunctions.CFunctions.SpawnHikerPed(out Suspect, Spawnpoint, 0);
            SuspectBlip = Suspect.AttachBlip(); 
            SuspectBlip.Color = Color.Red; 
            SuspectBlip.EnableRoute(Color.Yellow); 

            if (Suspect.IsMale) //Shit for dialouge
                MaleFemale = "sir";
            else
                MaleFemale = "mam"; 

            Counter = 0;

            Game.LogTrivial("-!!- Forestry Callouts - |IntoxicatedHiker| Callout accepted! -!!-");
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (!CloseToCall && Game.LocalPlayer.Character.DistanceTo(Suspect) <= 200f)
            {
                CloseToCall = true;
                SimpleFunctions.CFunctions.SetDrunk(Suspect, true);
                Suspect.Tasks.Wander();
                SimpleFunctions.CFunctions.PedPersonaChooser(Suspect, false, true);
            }
            if (!OnScene && Game.LocalPlayer.Character.DistanceTo(Suspect) <= 10f)
            {
            Game.LogTrivial("-!!- Forestry Callout - |IntoxicatedHiker| - Dialugoe, Case: " + rnd + " -!!-");
            Game.LogTrivial("-!!- Forestry Callouts - Starting main process -!!-");
            Game.DisplayHelp("Press ~r~'"+IniSettings.DialogueKey+"'~w~ to talk to the suspect", false);
            Suspect.Tasks.StandStill(-1);
            Suspect.Heading = Game.LocalPlayer.Character.Heading + 180f;
            OnScene = true;  
            }
            if (OnScene && Game.IsKeyDown(IniSettings.InputDialogueKey)) 
            {
            StopThePed.API.Functions.setPedAlcoholOverLimit(Suspect, true);
            Dialogue();  
            }

            if (Game.LocalPlayer.IsDead)
            {
                End();
            }
            if (Suspect.IsCuffed || Suspect.IsDead)
            {
                if (Ini.IniSettings.EnableEndCalloutHelpMessages)
                {
                    Game.DisplayHelp("Press ~r~'"+IniSettings.EndCalloutKey+"'~w~ at anytime to end the callout", false);
                }
            }
            if (Game.IsKeyDown(IniSettings.InputEndCalloutKey))
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", Spawnpoint);
                Game.DisplayNotification("~g~Dispatch:~w~ All Units, Intoxicated Hiker Code 4");
                Game.LogTrivial("-!!- Forestry Callouts - |IntoxicatedHiker| - Callout was force ended by player -!!-");
                End();
            }
            base.Process();
        }
        public override void End()
        {
            base.End();

            if (Suspect.Exists())
            {
                Suspect.Dismiss();
            }
            if (SuspectBlip.Exists())
            {
                SuspectBlip.Delete();
            }
          
                Game.LogTrivial("-!!- Forestry Callouts - |IntoxicatedHiker| - cleaned up -!!-");
        }
        private void Dialogue()
        {
            var step = 1;

            switch (rnd)
            {
                case 1: 

                            if (Counter == 1)
                        {
                            Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + ", what are you doing out here?");
                        }
                            if (Counter == 2)
                        {
                            Game.DisplaySubtitle("~r~Suspect:~w~ None of your business leave me alone, pig!");
                        }
                            if (Counter == 3)
                        {
                            Game.DisplaySubtitle("~y~Player:~w~ There is reports of a intoxicated person in this area, have you been drinking?");
                        }
                            if (Counter == 4)
                        {
                            Game.DisplaySubtitle("~r~Suspect:~w~ FUCK RUN!");
                            Suspect.Tasks.Wander();
                            Pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                            LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(Pursuit, Suspect);
                            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                        }
                            if (Counter == 5)
                        {
                            Game.DisplaySubtitle("No further dialogue pursue the suspect");
                        }

                        Counter++;

                    break;

                case 2:

                        if (Counter == 1)
                        {
                            Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + ", what are you doing out here?");
                        }
                        if (Counter == 2)
                        {
                            Game.DisplaySubtitle("~r~Suspect:~w~ Whatever I want to do.. I dont listen TO YOU IDIOTS!");
                        }
                        if (Counter == 3)
                        {
                            Game.DisplaySubtitle("~y~Player:~w~ There is reports of a intoxicated person around this location, have you been drinking?");
                        }
                        if (Counter == 4)
                        {
                            Game.DisplaySubtitle("~r~Suspect:~w~ FUCK YOU YOU PIG DIE!");
                            Suspect.Tasks.Wander();
                            Suspect.Inventory.GiveNewWeapon("WEAPON_KNIFE", -1, true);
                            Suspect.Tasks.FightAgainst(Game.LocalPlayer.Character);
                        }
                        if (Counter == 5)
                        {
                            Game.DisplaySubtitle("No further dialogue take apprioate action");
                        }

                    Counter++;


                    break;

                case 3:

                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + ", what are you doing out here?");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Ahhhh just uhm.. enjoying the view..");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ There has been reports of a intoxicated person around this area");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Pshhh. That's no good coppers, you better go get them!");
                        Suspect.Tasks.Wander();
                    }
                    if (Counter == 5)
                    {
                        Game.DisplaySubtitle("No further dialogue take apprioate action");
                    }

                    Counter++;

                    break;

                case 4:

                    if (Counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player:~w~ Hello " + MaleFemale + ", what are you doing out here?");
                    }
                    if (Counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Ahhhh just uhm.. enjoying the view..");
                    }
                    if (Counter == 3)
                    {
                        Game.DisplaySubtitle("~r~Player:~w~ There is reports of a intoxicated person around this location");
                    }
                    if (Counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ I promise I wasn't drinking officer! Please dont give me a citation");
                    }
                    if (Counter == 5)
                    {
                        Game.DisplaySubtitle("No further dialogue take apprioate action");
                    }

                    Counter++;

                    break;
            }
            return;
        }
    }
}
