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
using static DAGDialogueSystem.Type;
using LSPD_First_Response.Engine.Scripting.Entities;
using Type = System.Type;

#endregion

namespace ForestryCallouts2.Callouts.LandCallouts
{
    [CalloutInterface("Intoxicated Person", CalloutProbability.Medium, "Disturbance", "Code 2", "SASP")]

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
                                        DialogueFunctions.IterateDialogue(this, _root);
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
            
            // Make root node
            _root = new Node(Dialogue, prefixP+"Excuse me, may I speak to you for a minute?");
            
            // First npc response
            var _1 = _root.AddNode(Dialogue, prefixS+"Sure of course officer..");
            
            // Initial prompt node
            var _2 = _1.AddNode(Prompt, "[This is a Prompt Node]");
            var _3 = _2.AddNode(Option, "We have gotten reports of an intoxicated hiker.");
            var _4 = _2.AddNode(Option, "What are you doing out here?");
            var _5 = _2.AddNode(Option, "How much have you had to drink?");
            var _6 = _2.AddNode(Option, "You match the description of a intoxicated person!");
            var _7 = _2.AddNode(Option, "You smell like alcohol!");
            
            // Player chooses "We have gotten reports of an intoxicated hiker." in Prompt Node (_2)
            var _8 = _3.AddNode(Dialogue, "Your lying!");
            var _9 = _3.AddNode(Dialogue, "Well.. I don't think I am drunk?!?");
            var _10 = _3.AddNode(Dialogue, "Officer, I think you are mistaken, I am not drunk.");
            
            // Player chooses "What are you doing out here?" in Prompt Node (_2)
            var _11 = _4.AddNode(Dialogue, "Im going on a nice walk, would you like to join?");
            var _12 = _4.AddNode(Dialogue, "Im going on a beautiful hike!");
            var _13 = _4.AddNode(Dialogue, "Im trying to find the dinosaur that the zoo lost!");
            
            // Player chooses "How much have you had to drink" in Prompt Node (_2)
            var _14 = _5.AddNode(Dialogue, "Are you kidding me!?.. Not a single drop?");
            var _15 = _5.AddNode(Dialogue, "You know.. I really dislike the police force.");
            var _16 = _5.AddNode(Dialogue, "I've had a few apple juices the Busch Light ones!");
            var _17 = _5.AddNode(Dialogue, "Like a few bottles of wine?... That's nothing for me!");
            
            // Player chooses "You match the description of a intoxicated person!" in Prompt Node (_2)
            var _18 = _6.AddNode(Dialogue, "Your lying! I look nothing like someone who would be intoxicated!");
            var _19 = _6.AddNode(Dialogue, "No way! I think you should look in the mirror!");
            var _20 = _6.AddNode(Dialogue, "Yeah im wasted, what do you want?");
            
            // Player chooses "You smell like alcohol!" in Prompt Node (_2)
            var _21 = _7.AddNode(Dialogue, "You should smell yourself before you start judging!");
            var _22 = _7.AddNode(Dialogue, "Oh well I lost my smell. I wouldn't of gone out if I knew that!");
            var _23 = _7.AddNode(Dialogue, "Please, please dont arrest me!");
            
            // Prompt Node for Node (_8) (14) (_18) (_21)
            var _24 = _8.AddNode(Prompt, "[This is a Prompt Node]");

            var _25 = _24.AddNode("");
            
            
            
            
           
            
            
            
            
            
            
            
            // Player chooses "Put em up! You're under arrest for public intox!" in Prompt node (_15)
            Log.CallDebug(this, "Callout Dialogue Building Finished!");
        }
        
        public static void StartPursuit()
        {
            Log.Debug("IntoxicatedPerson", "Pursuit Starting");
        }
        
    }
}