#region Refrences
//System
using System;
using System.Collections.Generic;
using System.Drawing;
//Rage
using Rage;
//LSPDFR
using LSPD_First_Response.Mod.API;
using Functions = LSPD_First_Response.Mod.API.Functions;
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
//CalloutInterface
using CalloutInterfaceAPI;
//DAGDialogueSystem
using static DAGDialogueSystem.DirectedAcyclicGraph;
using DAGDialogueSystem;

#endregion

namespace ForestryCallouts2.Callouts.LandCallouts
{
    [CalloutInterface("Intoxicated Person", CalloutProbability.Medium, "Disturbance", "Code 2", "SASP")]

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
        
        //scenarios
        private int _scenario;
        private bool _sceanrioChosen;
        
        //pursuit
        private LHandle _pursuit;
        private bool _pursuitStarted;
        
        //dialogue
        private bool _askedPedToTalk;
        private Node _root;
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
        
        public override void OnCalloutNotAccepted()
        {
            Functions.PlayScannerAudio("OTHER_UNITS_TAKING_CALL");
            base.OnCalloutNotAccepted();
        }

        public override bool OnCalloutAccepted()
        {
            Log.CallDebug(this, "Callout accepted");
            // spawn the suspect
            CFunctions.SpawnHikerPed(out _suspect, _suspectSpawn, _rand.Next(1, 361));
            CFunctions.SetDrunk(_suspect, true);
            // sets a blip on the suspect and enables route
            _suspectBlip = _suspect.AttachBlip();
            _suspectBlip.EnableRoute(Color.Yellow);
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            // prevent crashes by not running anything in Process other than end methods
            if (!_pursuitStarted)
            {
                // if the player is 200 or closer delete route and blip
                if (Game.LocalPlayer.Character.DistanceTo(_suspect) <= 200f && !_onScene)
                {
                    _suspect.Tasks.Wander();
                    Log.CallDebug(this, "Process started");
                    _onScene = true;
                    if (_suspectBlip) _suspectBlip.Delete();
                    _firstBlip = true;
                }

                // if suspect isn't found initialize the search area
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
                        Log.CallDebug(this, "Search areas sent: " + _notfiSentCount + "");
                        _firstBlip = false;
                        Functions.PlayScannerAudioUsingPosition("SUSPECT_LAST_SEEN_01 IN_OR_ON_POSITION",
                            _suspect.Position);
                        _timer = 0;
                    }

                    // we delete the search area, and blip the suspect because the player is taking to long to find the suspect
                    if (_notfiSentCount == IniSettings.SearchAreaNotifications && !_maxNotfiSent)
                    {
                        // pause the timer so search blips dont keep coming in
                        Log.CallDebug(this, "Blipped suspect because player took to long to find them.");
                        _pauseTimer = true;
                        if (_suspectAreaBlip) _suspectAreaBlip.Delete();
                        _suspectBlip = _suspect.AttachBlip();
                        _suspectBlip.Color = Color.Red;
                        _suspectBlip.IsRouteEnabled = true;
                        _maxNotfiSent = true;
                    }
                }

                // player found the intoxicated ped
                if (!_suspectFound && Game.LocalPlayer.Character.DistanceTo(_suspect) <= 10f)
                {
                    Log.CallDebug(this, "Suspect found!");
                    _suspectBlip = _suspect.AttachBlip();
                    _suspectBlip.Color = Color.Red;
                    if (_suspectAreaBlip) _suspectAreaBlip.Delete();
                    _suspectFound = true;
                }

                if (_suspectFound)
                {
                    if (!_sceanrioChosen)
                    {
                        _scenario = _rand.Next(1, 5);
                        Log.CallDebug(this, "Scenario # = " + _scenario);
                        _sceanrioChosen = true;
                    }

                    switch (_scenario)
                    {
                        case < 4:
                        {
                            if (Game.LocalPlayer.Character.DistanceTo(_suspect) < 5f && Game.LocalPlayer.Character.IsOnFoot)
                            {
                                if (CFunctions.IsKeyAndModifierDown(IniSettings.DialogueKey, IniSettings.DialogueKeyModifier) && !_askedPedToTalk)
                                {
                                    // builds dialogue for scenarios 1 - 3.
                                    BuildDialogue();
                                    _askedPedToTalk = true;
                                    _suspect.Tasks.StandStill(-1);
                                    _suspect.Heading = Game.LocalPlayer.Character.Heading + 180;
                                    GameFiber.StartNew(delegate
                                    {
                                        GameFiber.Yield();
                                        DialogueFunctions.IterateDialogue(_root);
                                    });
                                }
                                
                            }
                            break;
                        }
                        // drunk Ped starts foot pursuit
                        case 4:
                        {
                            if (!_pursuitStarted)
                            {
                                Game.DisplaySubtitle("~r~Suspect:~w~ LEAVE ME ALONE!");
                                _suspect.Tasks.Wander();
                                _pursuit = Functions.CreatePursuit();
                                Functions.SetPursuitIsActiveForPlayer(_pursuit, true);
                                Functions.AddPedToPursuit(_pursuit, _suspect);
                                Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 CRIME_SUSPECT_ON_THE_RUN_01");
                                if (_suspectBlip) _suspectBlip.Delete();
                                _pursuitStarted = true;
                            }
                            break;
                        }
                    }
                }
            }


            // end callout
            if (CFunctions.IsKeyAndModifierDown(IniSettings.EndCalloutKey, IniSettings.EndCalloutKeyModifier))
            {
                Log.CallDebug(this, "Callout was force ended by player");
                End();
            }
            if (Game.LocalPlayer.Character.IsDead)
            {
                Log.CallDebug(this, "Player died callout ending");
                End();
            }
            base.Process();
        }

        public override void End()
        {
            DialogueFunctions.Clean();
            if (_suspect) _suspect.Dismiss();
            if (_suspectBlip) _suspectBlip.Delete();
            if (_suspectAreaBlip) _suspectAreaBlip.Delete();
            if (_scenario == 4) if (_pursuitStarted) if (Functions.IsPursuitStillRunning(_pursuit)) Functions.ForceEndPursuit(_pursuit);
            if (!ChunkChooser.StoppingCurrentCall)
            {
                Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 GP_CODE4_01", _suspectSpawn);
                if (IniSettings.EndNotfiMessages) Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "Status", "~g~Intoxicated Person Code 4", "");
                CalloutInterfaceAPI.Functions.SendMessage(this, "Unit "+IniSettings.Callsign+" reporting Intoxicated Person code 4");
            }
            Log.CallDebug(this, "Callout ended");
            base.End();
        }

        private void BuildDialogue()
        {
            Log.CallDebug(this, "Callout Dialogue Building..");
            const string prefixS = "~r~Suspect:~w~ ";
            const string prefixP = "~y~You:~w~ ";
            _root = new Node(1, prefixP+"Excuse me, may I speak to you for a minute?");
            var _1 = _root.AddNode(1, prefixS+"Sure of course officer..");
        
            var _2 = _1.AddNode(2, "[This is a Prompt Node]");
            var _3 = _2.AddNode(3, "We have gotten reports of an intoxicated hiker.");
            var _4 = _2.AddNode(3, "You match the description of an intoxicated individual.");
            var _5 = _2.AddNode(3, "I know you have been drinking, how drunk are you?");
            var _6 = _2.AddNode(3, "Your going to get me drunk off your breath!");
                    
            //Ai Speaking Options
            //N
            var _7 = _3.AddNode(1, prefixS+"I can’t lie, I have had a few drinks but im good to walk.");
            var _8 = _3.AddNode(1, prefixS+"That’s not me officer, I swear!");
            //G
            var _9 = _4.AddNode(1, prefixS+"I think you are in the wrong area officer.. I'm not drunk!");
            var _10 = _4.AddNode(1, prefixS+"Get outta here! Your kidding! I'm walking my imaginary dog!");
            //B
            var _11 = _5.AddNode(1, prefixS+"Drinking is fun! I can tell your not a fun person..");
            var _12 = _5.AddNode(1, prefixS+"F*ck you, pig. Leave me alone!");
            //S
            var _13 = _6.AddNode(1, prefixS+"That’s why you want to talk to me?");
            var _14 = _6.AddNode(1, prefixS+"You should smell your own breath before you start judging.");
            
            //N G Prompt Node
            var _15 = _7.AddNode(2, "[This is a Prompt Node]");
            _8.ConnectTo(_15);
            _9.ConnectTo(_15);
            _10.ConnectTo(_15);
            var _16 = _15.AddNode(3, "Do you mind if I breathalyzer test you?");
            var _17 = _15.AddNode(3, "You are being detained, put your hands behind your back.");
            var _18 = _15.AddNode(3, "You're under arrest for public intoxication.");
            var _19 = _15.AddNode(3, "Put em up! You're under arrest for public intox!");
            
            //B S Prompt Node
            var _20 = _11.AddNode(2, "[This is a Prompt Node]");
            _12.ConnectTo(_20);
            _13.ConnectTo(_20);
            _14.ConnectTo(_20);
            var _21 = _20.AddNode(3, "NOT IMPLEMENTED");
            var _22 = _20.AddNode(3, "NOT IMPLEMENTED");
            var _23 = _20.AddNode(3, "NOT IMPLEMENTED");
            var _24 = _20.AddNode(3, "NOT IMPLEMENTED");
                        
            Log.CallDebug(this, "Callout Dialogue Building Finished!");
        }
    }
}