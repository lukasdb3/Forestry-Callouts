#region Refrences
//System
using System;
using System.Drawing;
//Rage
using Rage;
//LSPDFR
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
//RageNativeUI
using RAGENativeUI;
using RAGENativeUI.Elements;
//ForestryCallouts2
using ForestryCallouts2.Backbone;
using ForestryCallouts2.Backbone.Functions;
using ForestryCallouts2.Backbone.IniConfiguration;
using ForestryCallouts2.Backbone.SpawnSystem;
using ForestryCallouts2.Backbone.SpawnSystem.Land;
#endregion

namespace ForestryCallouts2.Callouts.LandCallouts
{
    [CalloutInfo("IntoxicatedPerson", CalloutProbability.Medium)]

    internal class IntoxicatedPerson : Callout
    {
        
        #region Variables
        internal readonly string CurCall = "IntoxicatedPerson";

        private bool _onScene;
        private Random _rand = new();

        //suspect variables
        private Ped _suspect;
        private Vector3 _suspectSpawn;
        private float _suspectHeading;
        private Blip _suspectBlip;
        
        //timer variables
        private int _timer = 0;
        private bool _pauseTimer;
        
        //search area variables
        private Blip _suspectAreaBlip;
        private Vector3 _searchArea;
        private bool _suspectFound;
        private bool _maxNotfiSent;
        
        private bool _firstBlip;
        private int _notfiSentCount;
        
        //scenarios & dialogue
        private int _scenario;
        
        //pursuit
        private LHandle _pursuit;
        private bool _pursuitStarted;
        
        //dialogue
        private bool _askedPedToTalk;
        private string _gender;
        private int _counter = 0;
        private bool _dialoguePhase1Complete = false;

        private MenuPool _pool = new();
        private UIMenu _choiceMenu = new("Forestry Callouts", "Pick A Dialogue Option");
        private UIMenuItem _choice1 = new("null");
        private UIMenuItem _choice2 = new("null");
        private UIMenuItem _choice3 = new("null");
        private UIMenuItem _choice4 = new("null");
        #endregion
        public override bool OnBeforeCalloutDisplayed()
        {
            //Code to get the spawnpoints for the call
            ChunkChooser.Main(in CurCall);
            _suspectSpawn = ChunkChooser.FinalSpawnpoint;
            _suspectHeading = ChunkChooser.FinalHeading;
            
            //Normal callout details
            ShowCalloutAreaBlipBeforeAccepting(_suspectSpawn, 30f);
            CalloutMessage = ("~g~Intoxicated Person");
            CalloutPosition = _suspectSpawn; 
            AddMinimumDistanceCheck(IniSettings.MinCalloutDistance, CalloutPosition);
            CalloutAdvisory = ("~b~Dispatch:~w~ Intoxicated Person reported, Respond Code 2");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT_02 CRIME_DISTURBING_THE_PEACE_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_02_02", _suspectSpawn);
            return base.OnBeforeCalloutDisplayed();
        }

        public override void OnCalloutDisplayed()
        {
            //Send callout info to Callout Interface
            if (PluginChecker.CalloutInterface) CFunctions.CISendCalloutDetails(this, "CODE 2", "SASP");
            Logger.CallDebugLog(this, "Callout displayed");
            base.OnCalloutDisplayed();
        }
        
        public override void OnCalloutNotAccepted()
        {
            if (PluginChecker.CalloutInterface) Functions.PlayScannerAudio("OTHER_UNITS_TAKING_CALL");

            base.OnCalloutNotAccepted();
        }

        public override bool OnCalloutAccepted()
        {
            Logger.CallDebugLog(this, "Callout accepted");
            Logger.CallDebugLog(this, "Scenario: " + _scenario);
            //Spawn the suspect
            CFunctions.SpawnHikerPed(out _suspect, _suspectSpawn, _rand.Next(1, 361));
            CFunctions.SetDrunk(_suspect, true);
            //Sets a blip on the suspects head and enables route
            _suspectBlip = _suspect.AttachBlip();
            _suspectBlip.EnableRoute(Color.Yellow);
            //Gets the gender of suspect for dialogue
            if (_suspect.IsMale) { _gender = "Sir"; }
            else { _gender = "Mam"; }
            //StartUI
            _choiceMenu.AddItems(_choice1, _choice2, _choice3, _choice4);
            _choiceMenu.MouseControlsEnabled = false;
            _choiceMenu.Width = .3312f;
            _choiceMenu.RemoveBanner();
            _choiceMenu.SubtitleBackgroundColor = Color.DarkGreen;
            _pool.Add(_choiceMenu);
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            //Prevent crashes by not running anything in Process other than end methods
            if (!_pursuitStarted)
            {
                //If the player is 200 or closer delete route and blip
                if (Game.LocalPlayer.Character.DistanceTo(_suspect) <= 200f && !_onScene)
                {
                    _suspect.Tasks.Wander();
                    Logger.CallDebugLog(this, "Process started");
                    _onScene = true;
                    if (_suspectBlip) _suspectBlip.Delete();
                    _firstBlip = true;
                }

                //If suspect isn't found initialize the search area
                if (!_suspectFound && _onScene)
                {
                    if (!_pauseTimer) _timer++;

                    if (_firstBlip && _timer >= 1 || _timer >= 1250)
                    {
                        if (_suspectAreaBlip) _suspectAreaBlip.Delete();
                        var position = _suspect.Position;
                        _searchArea = position.Around2D(10f, 50f);
                        _suspectAreaBlip = new Blip(_searchArea, 65f) {Color = Color.Yellow, Alpha = .5f};
                        _notfiSentCount++;
                        Logger.CallDebugLog(this, "Search areas sent: " + _notfiSentCount + "");
                        _firstBlip = false;
                        Functions.PlayScannerAudioUsingPosition("SUSPECT_LAST_SEEN_01 IN_OR_ON_POSITION",
                            _suspect.Position);
                        _timer = 0;
                    }

                    //we delete the search area, and blip the suspect because the player is taking to long to find the suspect
                    if (_notfiSentCount == IniSettings.SearchAreaNotifications && !_maxNotfiSent)
                    {
                        //Pause the timer so search blips dont keep coming in
                        Logger.CallDebugLog(this, "Blipped suspect because player took to long to find them.");
                        _pauseTimer = true;
                        if (_suspectAreaBlip) _suspectAreaBlip.Delete();
                        _suspectBlip = _suspect.AttachBlip();
                        _suspectBlip.Color = Color.Red;
                        _suspectBlip.IsRouteEnabled = true;
                        _maxNotfiSent = true;
                    }
                }

                //player found the intoxicated ped
                if (!_suspectFound && Game.LocalPlayer.Character.DistanceTo(_suspect) <= 10f)
                {
                    Logger.CallDebugLog(this, "Suspect found!");
                    _suspectBlip = _suspect.AttachBlip();
                    _suspectBlip.Color = Color.Red;
                    if (_suspectAreaBlip) _suspectAreaBlip.Delete();
                    _suspectFound = true;
                }

                if (_suspectFound)
                {
                    //Dialogue scenario
                    _scenario = _rand.Next(1, 3);
                    if (_scenario == 1)
                    {
                        if (Game.LocalPlayer.Character.DistanceTo(_suspect) <= 5f && !_askedPedToTalk && Game.LocalPlayer.Character.IsOnFoot)
                        {
                            Game.DisplayHelp("Press ~r~(~y~" + IniSettings.DialogueKey.GetInstructionalKey() + "~r~)~w~ to start dialogue with the suspect.");
                        }

                        //Goes to Dialogue when player presses key again.
                        if (CFunctions.IsKeyAndModifierDown(IniSettings.DialogueKey, IniSettings.DialogueKeyModifier) && Game.LocalPlayer.Character.IsOnFoot)
                        {
                            if (!_askedPedToTalk)
                            {
                                Game.DisplaySubtitle("~b~You:~w~ Can I talk to you, " + _gender + ".");
                                CFunctions.CISendMessage(this, "Suspect as been found");
                                CFunctions.CISendMessage(this, "Talking with suspect");
                            }
                            _suspect.Tasks.Clear();
                            _suspect.Tasks.StandStill(-1);
                            _suspect.Heading = Game.LocalPlayer.Character.Heading + 180f;
                            MainDialogue();
                        }

                        //Dialogue Menu Processor
                        _pool.ProcessMenus();
                    }

                    //Drunk Ped starts foot pursuit
                    if (_scenario == 2)
                    {
                        if (!_pursuitStarted)
                        {
                            Game.DisplaySubtitle("~r~Suspect:~w~ NOO IM NOT DRUNK LEAVE ME ALONE!");
                            _suspect.Tasks.Wander();
                            _pursuit = Functions.CreatePursuit();
                            Functions.SetPursuitIsActiveForPlayer(_pursuit, true);
                            Functions.AddPedToPursuit(_pursuit, _suspect);
                            Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                            if (_suspectBlip) _suspectBlip.Delete();
                            _pursuitStarted = true;
                        }
                    }
                }
            }


            //End Callout
            if (CFunctions.IsKeyAndModifierDown(IniSettings.EndCalloutKey, IniSettings.EndCalloutKeyModifier))
            {
                Logger.CallDebugLog(this, "Callout was force ended by player");
                End();
            }
            if (Game.LocalPlayer.Character.IsDead)
            {
                Logger.CallDebugLog(this, "Player died callout ending");
                End();
            }
            base.Process();
        }

        public override void End()
        {
            if (_suspect) _suspect.Dismiss();
            if (_suspectBlip) _suspectBlip.Delete();
            if (_suspectAreaBlip) _suspectAreaBlip.Delete();
            if (!ChunkChooser.StoppingCurrentCall)
            {
                Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", _suspectSpawn);
                Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "Status", "~g~Intoxicated Person Code 4", "");
                if (PluginChecker.CalloutInterface) CFunctions.CISendMessage(this, "Intoxicated Person Code 4");
            }
            Logger.CallDebugLog(this, "Callout ended");
            base.End();
        }

        private void MainDialogue()
        {
            if (_counter == 1)
            {
                Logger.CallDebugLog(this, "Main Dialogue started");
                _askedPedToTalk = true;
                var random = _rand.Next(1, 5);
                Logger.CallDebugLog(this, "Suspect Dialogue Choice: " + random);
                switch (random)
                {
                    case 1:
                        Game.DisplaySubtitle("~r~Suspect:~w~ What do you want.");
                        break;
                    case 2:
                        Game.DisplaySubtitle("~r~Suspect:~w~ Oh, of course officer.");
                        break;
                    case 3:
                        Game.DisplaySubtitle("~r~Suspect:~w~ Im on my daily walk but sure.");
                        break;
                    case 4:
                        Game.DisplaySubtitle("~r~Suspect:~w~ Yes of course officer.");
                        break;
                }
            }

            if (_counter == 2)
            {
                _choice1.Text = "What are you doing out here?";
                _choice2.Text = "How much have you had to drink?";
                _choice3.Text = "It's not safe out here especially when your intoxicated.";
                _choice4.Text = "Do you know public intoxication is a misdemeanor charge?";
                _choiceMenu.RefreshIndex();
                _choiceMenu.Visible = true;
                Logger.CallDebugLog(this, "First Menu Visible");

                //If 1 is chosen
                _choice1.Activated += (menu, item) =>
                {
                    GameFiber.Wait(750);
                    _choiceMenu.Visible = false;
                    Game.DisplaySubtitle("~b~You: ~w~" + _choice1.Text);
                    GameFiber.Wait(3500);
                    var _rnd = _rand.Next(1, 5);
                    if (!_dialoguePhase1Complete)
                    {
                        switch (_rnd)
                        {
                            case 1:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Leave me alone im trying to go on a peaceful walk.");
                                break;
                            case 2:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Im taking a nap in my head. Alcohol seems to help a lot and the natural beauty around me.");
                                break;
                            case 3:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Going on a walk officer.. ranger.. what ever you are.");
                                break;
                            case 4:
                                Game.DisplaySubtitle("~r~Suspect:~w~ How does a beer sound officer?");
                                break;
                        }   
                    }
                    if (_dialoguePhase1Complete)
                    {
                        switch (_rnd)
                        {
                            case 1:
                                Game.DisplaySubtitle("~r~Suspect:~w~ NO YOUR NOT!");
                                _suspect.Tasks.Wander();
                                _pursuit = Functions.CreatePursuit();
                                Functions.SetPursuitIsActiveForPlayer(_pursuit, true);
                                Functions.AddPedToPursuit(_pursuit, _suspect);
                                Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                                if (_suspectBlip) _suspectBlip.Delete();
                                _pursuitStarted = true;
                                return;
                            case 2:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Please officer at least let me be released tomorrow.");
                                Game.DisplayHelp("~g~Dialogue Finished");
                                return;
                            case 3:
                                Game.DisplaySubtitle("~r~Suspect:~w~ I accept my consequences officer.");
                                Game.DisplayHelp("~g~Dialogue Finished");
                                return;
                            case 4:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Please don't do this officer.");
                                Game.DisplayHelp("~g~Dialogue Finished");
                                return;
                        }
                    }

                    _choice1.Enabled = false;
                    RefreshMenu();
                };
                //If 2 is chosen
                _choice2.Activated += (menu, item) =>
                {
                    GameFiber.Wait(750);
                    _choiceMenu.Visible = false;
                    Game.DisplaySubtitle("~b~You: ~w~" + _choice2.Text);
                    GameFiber.Wait(3500);
                    var _rnd = _rand.Next(1, 5);
                    if (!_dialoguePhase1Complete)
                    {
                        switch (_rnd)
                        {
                            case 1:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Ive had a few drinks, not enough to get me drunk though.");
                                break;
                            case 2:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Ive had zero drinks officer zero!");
                                break;
                            case 3:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Drinks? What are drinks? Like pepsi?");
                                break;
                            case 4:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Im gonna be honest im wasted. Went on a hike to sober up.");
                                break;
                        }   
                    }
                    if (_dialoguePhase1Complete)
                    {
                        switch (_rnd)
                        {
                            case 1:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Officer please I've done nothing wrong.");
                                Game.DisplayHelp("~g~Dialogue Finished");
                                return;
                            case 2:
                                Game.DisplaySubtitle("~r~Suspect:~w~ I accept my consequences officer.");
                                Game.DisplayHelp("~g~Dialogue Finished");
                                return;
                            case 3:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Let me go officer please I beg you.");
                                Game.DisplayHelp("~g~Dialogue Finished");
                                return;
                            case 4:
                                Game.DisplaySubtitle("~r~Suspect:~w~ I've never been to jail I don't want to go!");
                                Game.DisplayHelp("~g~Dialogue Finished");
                                return;
                        }
                    }
                    _choice2.Enabled = false;
                    RefreshMenu();
                };
                //If 3 is chosen
                _choice3.Activated += (menu, item) =>
                {
                    GameFiber.Wait(750);
                    _choiceMenu.Visible = false;
                    Game.DisplaySubtitle("~b~You: ~w~" + _choice3.Text);
                    GameFiber.Wait(3500);
                    var _rnd = _rand.Next(1, 5);
                    if (!_dialoguePhase1Complete)
                    {
                        switch (_rnd)
                        {
                            case 1:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Yeah I have had a lot of drinks.");
                                break;
                            case 2:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Yeah, I've had quite a few drinks I'll admit.");
                                break;
                            case 3:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Doesn't nature beauty sober people up? I must be an alien.");
                                break;
                            case 4:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Officer, you have the wrong person I swear.");
                                break;
                        }   
                    }
                    if (_dialoguePhase1Complete)
                    {
                        switch (_rnd)
                        {
                            case 1:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Officer you know.. there isn't any fun in that.");
                                Game.DisplayHelp("~g~Dialogue Finished");
                                return;
                            case 2:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Please don't arrest me.");
                                Game.DisplayHelp("~g~Dialogue Finished");
                                return;
                            case 3:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Please officer let me go on a warning.");
                                Game.DisplayHelp("~g~Dialogue Finished");
                                return;
                            case 4:
                                Game.DisplaySubtitle("~r~Suspect:~w~ I'LL RUN INTOXICATED TOO!");
                                _suspect.Tasks.Wander();
                                _pursuit = Functions.CreatePursuit();
                                Functions.SetPursuitIsActiveForPlayer(_pursuit, true);
                                Functions.AddPedToPursuit(_pursuit, _suspect);
                                Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                                if (_suspectBlip) _suspectBlip.Delete();
                                _pursuitStarted = true;
                                return;
                        }
                    }
                    _choice3.Enabled = false;
                    RefreshMenu();
                };
                //If 4 is chosen, choice 4 is the trigger for new dialogue questions
                _choice4.Activated += (menu, item) =>
                {
                    GameFiber.Wait(750);
                    _choiceMenu.Visible = false;
                    Game.DisplaySubtitle("~b~You: ~w~" + _choice4.Text);
                    GameFiber.Wait(3500);
                    var _rnd = _rand.Next(1, 5);
                    if (!_dialoguePhase1Complete)
                    {
                        switch (_rnd)
                        {
                            case 1:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Does that mean your going to arrest me?");
                                break;
                            case 2:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Please officer give me one more chance I'll go straight home!");
                                break;
                            case 3:
                                Game.DisplaySubtitle("~r~Suspect:~w~ I just wanted to go on a nice walk officer I hope you understand.");
                                break;
                            case 4:
                                Game.DisplaySubtitle("~r~Suspect:~w~ How could you be charged for a crime in mother nature officer?");
                                break;
                        }   
                    }
                    if (_dialoguePhase1Complete)
                    {
                        switch (_rnd)
                        {
                            case 1:
                                Game.DisplaySubtitle("~r~Suspect:~w~: I accept my consequences officer.");
                                Game.DisplayHelp("~g~Dialogue Finished");
                                return;
                            case 2:
                                Game.DisplaySubtitle("~r~Suspect:~w~: The inmates will kill me officer!");
                                Game.DisplayHelp("~g~Dialogue Finished");
                                return;
                            case 3:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Well I guess it was a nice walk while it lasted.");
                                Game.DisplayHelp("~g~Dialogue Finished");
                                return;
                            case 4:
                                Game.DisplaySubtitle("~r~Suspect:~w~ Im gonna miss the free world.");
                                Game.DisplayHelp("~g~Dialogue Finished");
                                return;
                        }
                    }

                    //Change questions and reenable the choices.
                    _choice1.Enabled = true;
                    _choice2.Enabled = true;
                    _choice3.Enabled = true;

                    _choice1.Text = "Im going to have to arrest you.";
                    _choice2.Text = "I can't just let you go free sorry.";
                    _choice3.Text = "You could've went on a nice walk not intoxicated.";
                    _choice4.Text = "Mother nature doesn't free you from charges.";

                    _dialoguePhase1Complete = true;
                    RefreshMenu();
                };
            }
            _counter++;
        }

        private void RefreshMenu()
        {
            Logger.CallDebugLog(this, "Refreshed Dialogue Menu");
            GameFiber.Wait(3500);
            _choiceMenu.RefreshIndex();
            _choiceMenu.Visible = true;
        }
    }
}