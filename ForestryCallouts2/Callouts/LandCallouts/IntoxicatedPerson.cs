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
//ForestryCallouts2
using ForestryCallouts2.Backbone;
using ForestryCallouts2.Backbone.Functions;
using ForestryCallouts2.Backbone.IniConfiguration;
using ForestryCallouts2.Backbone.SpawnSystem;
//CalloutInterface
using CalloutInterfaceAPI;
//DAGDialogueSystem
using static DAGDialogueSystem.DirectedAcyclicGraph;
using DAGDialogueSystem;
using static DAGDialogueSystem.Type;
#endregion

namespace ForestryCallouts2.Callouts.LandCallouts
{
    [CalloutInterface("[FC] IntoxicatedPerson", CalloutProbability.Medium, "Disturbance", "Code 2", "SASP")]

    public class IntoxicatedPerson : Callout
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
            _suspectBlip = CFunctions.CreateBlip(_suspect, true, Color.Yellow, Color.Yellow,1);
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
                        _suspectBlip = CFunctions.CreateBlip(_suspect, true, Color.Yellow, Color.Yellow, .75f);
                    }
                }

                // player found the intoxicated ped
                if (!_suspectFound && Game.LocalPlayer.Character.DistanceTo(_suspect) <= 10f)
                {
                    Log.CallDebug(this, "Suspect found!");
                    _suspectBlip = CFunctions.CreateBlip(_suspect, false, Color.Yellow, Color.Yellow,.75f);
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
                                        DialogueFunctions.IterateDialogue(this, _suspect ,_root, "~y~Suspect", "~b~You");
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
                                StartPursuit();
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
            DialogueFunctions.Finish();
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

            // Make root node
            _root = new Node(PlayerDialogue, "Excuse me, may I speak to you for a minute?");

            // First npc response
            var _1 = _root.AddNode(NpcDialogue, "Sure of course officer..");

            // Initial prompt node
            var _2 = _1.AddNode(Prompt, "[This is a Prompt Node]");
            var _3 = _2.AddNode(Option, "We have gotten reports of an intoxicated hiker.");
            var _4 = _2.AddNode(Option, "What are you doing out here?");
            var _5 = _2.AddNode(Option, "How much have you had to drink?");
            var _6 = _2.AddNode(Option, "You match the description of a intoxicated person!");
            var _7 = _2.AddNode(Option, "You smell like alcohol!");

                // Player chooses "We have gotten reports of an intoxicated hiker." in Prompt Node (_2)
                var _8 = _3.AddNode(NpcDialogue, "Your lying!"); 
            
                // Player chooses "What are you doing out here?" in Prompt Node (_2)
                var _13 = _4.AddNode(NpcDialogue, "Im trying to find the dinosaur that the zoo lost!");

                // Player chooses "How much have you had to drink" in Prompt Node (_2)
                var _14 = _5.AddNode(NpcDialogue, "Are you kidding me!?.. Not a single drop?"); 

                // Player chooses "You match the description of a intoxicated person!" in Prompt Node (_2)
                var _18 = _6.AddNode(NpcDialogue, "Your lying! I look nothing like someone who would be intoxicated!"); 

                // Player chooses "You smell like alcohol!" in Prompt Node (_2)
                var _21 = _7.AddNode(NpcDialogue, "You should smell yourself before you start judging!"); 

                    // Prompt Node for Node (_8) (_13) (_14) (_18) (_21). NPC bad responses
                    var _24 = _8.AddNode(Prompt, "[This is a Prompt Node]");
                    var _25 = _24.AddNode(Option, "Do you mind taking a breathalyzer?");
                    var _26 = _24.AddNode(Option, "Do you mind if I frisk you?");
                    var _27 = _24.AddNode(Option, "Be honest with me!");
                    var _28 = _24.AddNode(Option, "Im going to have to arrest you for public intoxication.");
                    _24.LinkNodesAsParents(new []{_13, _14, _18, _21});

                        // Player chooses "Do you mind taking a breathalyzer?" in Prompt Node (_24)
                        var _29 = _25.AddNode(NpcDialogue, "Yes, I will take a breathalyzer test.");
                        var _30 = _25.AddNode(NpcDialogue, "No! I refuse! Do not test me!");
                        var _31 = _25.AddActionNodeWithDialogue(StartPursuit, "NO RUN AWAY!!");

                            // Prompt Node for Node (_29)                   
                            var _32 = _29.AddNode(Prompt, "[This is a Prompt Node]");
                            var _33 = _32.AddNode(Option, "Great! Thank you for your cooperation!");
                            var _34 = _32.AddNode(Option, "I appreciate your cooperation.");
                            var _35 = _32.AddNode(Option, "Blow hard!");

                            // Prompt Node for Node (_30)
                            var _36 = _30.AddNode(Prompt, "[This is a Prompt Node]");
                            var _37 = _36.AddNode(Option, "If you resist we will have to go to the station and do blood work.");
                            var _38 = _36.AddNode(Option, "Well to bad, I believe you are intoxicated. Therefore you are under arrest.");
                            var _39 = _36.AddNode(Option, "Okay then, can I have you complete a field test?");

                                // Player chooses "If you resist we will have to go to the station and do blood work." in Prompt Node (_36)
                                var _40 = _37.AddNode(NpcDialogue, "No way! I hate drawing blood. You can do the breathalyzer test.");
                                var _41 = _37.AddNode(NpcDialogue, "Can I complete a field sobriety test?");
                                var _42 = _37.AddNode(NpcDialogue, "No thank you! I will do breathalyzer test.");

                                    var _43 = _41.AddNode(Prompt, "[This is a Prompt Node]");
                                    var _44 = _43.AddNode(Option, "No, you cant.");
                                    var _45 = _43.AddNode(Option, "Sure, why not.");
                                    
                                        // Player chooses "No, you cant." in Prompt Node (_43)
                                        var _46 = _44.AddNode(NpcDialogue, "Fine! Just arrest me!");
                                        var _47 = _44.AddNode(NpcDialogue, "If you arrest me I will sue you!");
                                        var _48 = _38.AddActionNodeWithDialogue(StartPursuit, "TRY AND CATCH ME!");
                                
                                // Player chooses "Well to bad, I believe you are intoxicated. Therefore you are under arrest." in Prompt Node (_36)
                                var _49 = _38.AddNode(NpcDialogue, "No please don't arrest me, you can do a breathalyzer test.");
                                var _50 = _38.AddNode(NpcDialogue, "I have nothing to live for anyways.");
                                var _51 = _38.AddActionNodeWithDialogue(StartPursuit, "DO THAT AND I WILL SUE YOU!!");
                                
                                // Player chooses "Okay then, can I have you complete a field test?" in Prompt Node (_36)
                                var _52 = _39.AddNode(NpcDialogue, "No, I will not do that either!");
                                var _53 = _39.AddNode(NpcDialogue, "Okay look.. I am drunk. I've had a rough day.");
                                var _54 = _39.AddActionNodeWithDialogue(StartPursuit, "MAYBE IF YOU CAN CATCH ME");

                        // Player chooses "Do you mind if I frisk you?" in Prompt Node (_24)
                        var _55 = _26.AddNode(NpcDialogue, "I suppose.. not sure why you would want to.");
                        var _56 = _26.AddNode(NpcDialogue, "Sure.");
                        var _57 = _26.AddNode(NpcDialogue, "No, do not touch me!");
                        var _58 = _26.AddActionNodeWithDialogue(StartPursuit, "NO DONT TOUCH ME!");

                            //Prompt Node for Node (_57)
                            var _59 = _57.AddNode(Prompt, "[This is a Prompt Node]");
                            var _60 = _59.AddNode(Option, "Im going to have to take you into the station.");
                            var _60 = _59.AddNode(Option, "Im going to have to arrest you for public intoxication.");

                                //Player chooses "Im going to have to take you into the station." in Prompt Node (_59)
                                var _61 = _60.AddNode(NpcDialogue, "Fine.");
                                var _62 = _60.AddActionNodeWithDialogue(StartPursuit, "YEAH RIGHT!");

                                //Player chooses "Im going to have to arrest you for public intoxication." in Prompt Node (_59)
                                var _63 = _60.AddNode(NpcDialogue, "Cops are such assholes!");
                                var _64 = _60.AddActionNodeWithDialogue(StartPursuit, "No way asshole!");

                        // Player chooses "Be honest with me!" in Prompt Node (_24)
                        var _65 = _27.AddNode(NpcDialogue, "I am being honest I am not drunk!");
                        var _66 = _27.AddNode(NpcDialogue, "Your a shitty cop!");
                        var _67 = _27.AddNode(NpcDialogue, "OKAY! I had only a few drinks..");

                        // Player chooses "Im going to have to arrest you for public intoxication." in Prompt Node (_24)
                        var _68 = _28.AddNode(NpcDialogue, "Fine! Free food and shelter is fine with me!");
                        var _69 = _28.AddActionNodeWithDialogue(StartPursuit, "TRY AND CATCH ME PIG!");
                        Log.CallDebug(this, "Callout Dialogue Building Finished!");
        }
        
        public static void StartPursuit()
        {
            Log.Debug("IntoxicatedPerson", "Pursuit Starting");
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