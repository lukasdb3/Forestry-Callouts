using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using System.Drawing;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using ForestryCallouts2.Backbone;
using ForestryCallouts2.Backbone.Functions;
using ForestryCallouts2.Backbone.IniConfiguration;
using ForestryCallouts2.Backbone.SpawnSystem.Land;
using RAGENativeUI;
using RAGENativeUI.Elements;
using RAGENativeUI.PauseMenu;

namespace ForestryCallouts2.Callouts.LandCallouts
{
    [CalloutInfo("IntoxicatedPerson", CalloutProbability.Medium)]

    internal class IntoxicatedPerson : Callout
    {
        
        #region Variables
        internal string CurCall = "IntoxicatedPerson";

        private bool onScene;

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
        private int _scenario = 1; //new Random().Next(1, 3);
        
        //pursuit
        private LHandle _pursuit;
        private bool _pursuitStarted;
        
        //dialogue
        private bool _askedPedToTalk;
        private string _gender;
        private int _counter = 0;
        private bool _choiceChoosed;
        private int _rnd;
        private int _playersChoice;
        
        private MenuPool _pool = new();
        private UIMenu _choiceMenu = new("Forestry Callouts", "Pick a dialogue option");
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
            AddMinimumDistanceCheck(100f, _suspectSpawn);
            CalloutMessage = ("~g~Intoxicated Person Reported");
            CalloutPosition = _suspectSpawn; 
            CalloutAdvisory = ("~b~Dispatch:~w~ Intoxicated Person reported, Respond Code 2");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT_02 CRIME_DISTURBING_THE_PEACE_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_02_02", _suspectSpawn);
            return base.OnBeforeCalloutDisplayed();
        }

        public override void OnCalloutDisplayed()
        {
            //Send callout info to Callout Interface
            if (PluginChecker.CalloutInterface) CFunctions.SendCalloutDetails(this, "CODE 2", "SAPR");
            Logger.CallDebugLog(this, "Callout displayed");
            base.OnCalloutDisplayed();
        }
        
        public override void OnCalloutNotAccepted()
        {
            if (PluginChecker.CalloutInterface) LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("OTHER_UNITS_TAKING_CALL");

            base.OnCalloutNotAccepted();
        }

        public override bool OnCalloutAccepted()
        {
            Logger.CallDebugLog(this, "Callout accepted");
            //Send details message
            Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "~b~Dispatch", "~g~Callout Details", "Citizens report a intoxicated individual disturbing the peace, suspect is on foot hiking. Citizens also reporting suspect is stumbling around and has poor balance. Respond Code 2");
            //Spawn the suspect
            CFunctions.SpawnHikerPed(out _suspect, _suspectSpawn, _suspectHeading);
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
            _pool.Add(_choiceMenu);
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            //If the player is 200 or closer delete route and blip
            if (Game.LocalPlayer.Character.DistanceTo(_suspect) <= 200f && !onScene)
            {
                _suspect.Tasks.Wander();
                Logger.CallDebugLog(this, "Process started");
                onScene = true;
                if (_suspectBlip) _suspectBlip.Delete();
                _firstBlip = true;
            }

            //If suspect isn't found initialize the search area
            if (!_suspectFound && onScene)
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
                _suspectBlip = _suspect.AttachBlip();
                _suspectBlip.Color = Color.Red;
                if (_suspectAreaBlip) _suspectAreaBlip.Delete();
                _suspectFound = true;
            }

            if (_suspectFound)
            {
                //Dialogue scenario
                if (_scenario == 1)
                {
                    if (Game.LocalPlayer.Character.DistanceTo(_suspect) <= 5f && !_askedPedToTalk &&
                        Game.LocalPlayer.Character.IsOnFoot)
                    {
                        Game.DisplayHelp("Press ~r~( ~g~" + IniSettings.DialogueKey.GetInstructionalId() + "~r~ )~w~ to start dialogue with the suspect.");
                    }

                    //Goes to Dialogue when player presses key again.
                    if (Game.IsKeyDown(IniSettings.DialogueKey))
                    {
                        if (!_askedPedToTalk) Game.DisplaySubtitle("~b~You:~w~ Can I talk to you, " + _gender + ".");
                        _suspect.Tasks.Clear();
                        _suspect.Tasks.StandStill(-1);
                        _suspect.Heading = Game.LocalPlayer.Character.Heading + 180f;
                        DialoguePhase1();
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


            //End Callout Ifs
            if (Game.IsKeyDown(IniSettings.EndCalloutKey)) //If player presses "End" it will forcefully clean the callout up
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 OP_CODE OP_4", _suspectSpawn);
                Game.DisplayNotification("~g~Dispatch:~w~ All Units, Intoxicated Person Code 4");
                if (PluginChecker.CalloutInterface)
                {
                    CFunctions.SendMessage(this, "Intoxicated Person Code 4");
                }
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
            Logger.CallDebugLog(this, "Callout ended");
            base.End();
        }

        private void DialoguePhase1()
        {

            if (_counter == 1)
            {
                _askedPedToTalk = true;
                var random = new Random().Next(1, 5);
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
                _choice3.Text = "We received a report of an intoxicated individual.";
                _choice4.Text = "It's not safe out here especially when your intoxicated.";
                _choiceMenu.RefreshIndex();
                _choiceMenu.Visible = true;

                //If 1 is chosen
                _choice1.Activated += (menu, item) =>
                {
                    GameFiber.Wait(1500);
                    _playersChoice = 1;
                    _choiceMenu.Visible = false;
                    _rnd = new Random().Next(1, 5);
                    switch (_rnd)
                    {
                        case 1:
                            Game.DisplaySubtitle("~r~Suspect:~w~ Leave me alone im trying to go on a peaceful walk.");
                            break;
                        case 2:
                            Game.DisplaySubtitle("~r~Suspect:~w~ Im taking a nap in my head. Alcohol seems to help a lot and the natural beauty around me.");
                            break;
                        case 3:
                            Game.DisplaySubtitle("~r~Suspect:~w~ Going on a walk officer.. ranger.. what ever you are, what are you doing?");
                            break;
                        case 4:
                            Game.DisplaySubtitle("~r~Suspect:~w~ How does a beer sound officer?");
                            break;
                    }
                    DialoguePhase2();
                    _choiceChoosed = true;
                };
                //If 2 is chosen
                _choice2.Activated += (menu, item) =>
                {
                    GameFiber.Wait(1500);
                    _playersChoice = 2;
                    _choiceMenu.Visible = false;
                    _rnd = new Random().Next(1, 5);
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
                    _choiceChoosed = true;
                };
                //If 3 is chosen
                _choice3.Activated += (menu, item) =>
                {
                    GameFiber.Wait(1500);
                    _playersChoice = 3;
                    _choiceMenu.Visible = false;
                    _rnd = new Random().Next(1, 5);
                    switch (_rnd)
                    {
                        case 1:
                            Game.DisplaySubtitle("~r~Suspect:~w~ Yeah your right I shouldn't be out here.");
                            break;
                        case 2:
                            Game.DisplaySubtitle("~r~Suspect:~w~ You think im intoxicated? I had a few drinks.");
                            break;
                        case 3:
                            Game.DisplaySubtitle("~r~Suspect:~w~ I just wanted to go on a nice walk officer I hope you understand.");
                            break;
                        case 4:
                            Game.DisplaySubtitle("~r~Suspect:~w~ Well I heard nature sounds will sober you up so thought I would give it a shot.");
                            break;
                    }
                    _choiceChoosed = true;
                };
                //If 4 is chosen
                _choice4.Activated += (menu, item) =>
                {
                    GameFiber.Wait(1500);
                    _playersChoice = 4;
                    _choiceMenu.Visible = false;
                    _rnd = new Random().Next(1, 5);
                    switch (_rnd)
                    {
                        case 1:
                            Game.DisplaySubtitle("~r~Suspect:~w~ A report of an intoxicated person? That's not me..");
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
                    _choiceChoosed = true;
                };
                if (_choiceChoosed)
                {
                    DialoguePhase2();
                }
            }
            _counter++;
        }

        private void DialoguePhase2()
        {
            Logger.DebugLog("IntoxicatedHiker", "Dialogue Phase 2 started");
            GameFiber.Wait(1000);
            _choice1.Text = "What are you doing out here?";
            _choice2.Text = "How much have you had to drink?";
            _choice3.Text = "We received a report of an intoxicated individual.";
            _choice4.Text = "It's not safe out here especially when your intoxicated.";
            _choiceMenu.RefreshIndex();
            _choiceMenu.Visible = true;
        }
    }
}