//System
using System;
using System.Collections.Generic;
using System.Drawing;
//Rage
using Rage;
//LSPDFR
using LSPD_First_Response.Mod.API;
//ForestryCallouts2
using ForestryCallouts2.AmbientEvents.Events;
using ForestryCallouts2.Backbone;
using ForestryCallouts2.Backbone.IniConfiguration;
using ForestryCallouts2.Backbone.Menu;
using RAGENativeUI.Elements;


namespace ForestryCallouts2.AmbientEvents
{
    internal static class Main
    {
        private static readonly Random Rand = new();
        internal static readonly List<string> EventNamesList = new();
        internal static bool IsAnyEventRunning = false;
        internal static AmbientEvent currentEvent = null;
        private static bool _pauseTimer;
        internal static GameFiber TimerFiber;

        internal static void RegisterEvents()
        {
            // hard coded disabling ambient as its wip and currently crashing :(
            IniSettings.AmbientEventsEnabled = false;
            
            if (IniSettings.AmbientEventsEnabled)
            {
                RegisterEvent("DeadAnimal");
                Timer();
            }
            else
            {
                Logger.InfoLog("AmbientEvents","Ambient Events are disabled");
            }
        }

        private static void RegisterEvent(string aEvent)
        {
            EventNamesList.Add(aEvent);
            Game.LogTrivial("Registering event "+aEvent);
            Create.EventsMenu.AddItem(new UIMenuItem(@aEvent));
            Create.EventsMenu.RefreshIndex();
        }

        internal static AmbientEvent GetAmbientEvent(string eventName)
        {
            AmbientEvent @event = null;
            switch (eventName)
            {
                case "DeadAnimal":
                    @event = new DeadAnimal();
                    break;
            }
            return @event;
        }

        internal static void Timer()
        {
            if (_pauseTimer) _pauseTimer = false;
            TimerFiber = GameFiber.StartNew(delegate
            {
                Logger.InfoLog("Ambient Events", "Events can now be created");
                while (!IsAnyEventRunning && IniSettings.AmbientEventsEnabled && !_pauseTimer)
                {
                    GameFiber.Wait(Rand.Next(IniSettings.MinimumWaitTime * 1000, IniSettings.MaximumWaitTime * 1000));
                    if (!IsAnyEventRunning && Functions.GetActivePursuit() == null && Functions.GetCurrentPullover() == null && !Functions.IsCalloutRunning())
                    {
                        string eventStarting =  EventNamesList[Rand.Next(0, EventNamesList.Count)];
                        StartEvent(eventStarting);
                    }
                }
            });
        }

        internal static void StartEvent(string @event)
        {
            AmbientEvent eevent = null;
            eevent = GetAmbientEvent(@event);
            if (eevent.Active) eevent.End();
            if (Functions.GetCurrentPullover() != null) Functions.ForceEndCurrentPullover();
            if (Functions.IsCalloutRunning()) Functions.StopCurrentCallout();
            if (!IsAnyEventRunning)
            {
                Logger.InfoLog("Ambient Events", "Starting " + eevent.StringName + " event");
                eevent.Start();
            }
        }

        internal static void EndEvent(string eventName)
        {
            foreach (var ae in EventNamesList)
            {
                var eevent = GetAmbientEvent(ae);
                if (eevent.StringName == eventName)
                {
                    Logger.InfoLog("Ambient Events", "Force Ending "+eevent.StringName+" event");
                    eevent.End();
                }
            }
        }

        internal static void CleanUp()
        {
            Logger.InfoLog("Ambient Events", "Shutting down thread and cleaning up");
            _pauseTimer = true;
            foreach (var ae in EventNamesList)
            {
                var @event = GetAmbientEvent(ae);
                if (@event.Active) @event.End();
            }
            EventNamesList.Clear();
            Create.EventsMenu.Clear();
            Logger.InfoLog("Ambient Events", "Shutdown Successful");
        }
    }
}