using System;
using System.Collections.Generic;
using Rage;
using ForestryCallouts2.Backbone;
using ForestryCallouts2.Backbone.IniConfiguration;
using ForestryCallouts2.Backbone.Menu;

namespace ForestryCallouts2.AmbientEvents
{
    public class AmbientEvent
    {
        internal string StringName { get; }
        internal bool Active { get; private set; }
        private bool ProcessStarted { get; set; }
        
        protected AmbientEvent()
        {
            try 
            {
                StringName = this.ToString().Replace("ForestryCallouts2.AmbientEvents.Events.", string.Empty);
                ActiveEntities = new List<Entity>();
                ActiveBlips = new List<Blip>();
                ProcessFiber = new GameFiber(delegate
                {
                    while (Main.IsAnyEventRunning)
                    {
                        GameFiber.Yield();
                        Process();
                    }
                });
            }
            catch (Exception e)
            {
                Logger.ErrorLog("AmbientEvent", "There was an error starting an event. Please send this log to https://discord.gg/ulss.", e);
                End();
            }
        }

        internal List<Entity> ActiveEntities { get; set; }
        internal List<Blip> ActiveBlips { get; set; }
        internal GameFiber ProcessFiber { get; }

        protected internal virtual void Start()
        {
            Main.IsAnyEventRunning = true;
            Active = true;
            Main.currentEvent = this;
            Logger.InfoLog("AmbientEvent - "+StringName+"", "Started");
            ProcessFiber.Start();
        }

        protected virtual void Process()
        {
            if (!ProcessStarted) Logger.InfoLog("AmbientEvent - "+StringName+"", "Process Initialized");
            ProcessStarted = true;
            
            //End Stuff
            if (Game.IsKeyDown(IniSettings.EndCalloutKey)) End();
        }

        protected internal virtual void End()
        {
            Logger.InfoLog("AmbientEvent - "+StringName+"", "Ending");
            Main.IsAnyEventRunning = false;
            Active = false;
            Main.currentEvent = null;
            Main.Timer();
        }
    }
}