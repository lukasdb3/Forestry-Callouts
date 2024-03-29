﻿#region Refrences
//System
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using ForestryCallouts2.Backbone.Functions;
using ForestryCallouts2.Backbone.IniConfiguration;
//Rage
using Rage;
//LSPDFR
using LSPD_First_Response.Mod.Callouts;
#endregion

namespace ForestryCallouts2.Backbone
{
    internal static class CalloutCache
    {
        private static readonly List<string> RandomCalloutCache = new();
        private static int _callCount;
        
        internal static void CacheCallouts()
        {
            var startTime = DateTime.Now;
            Game.Console.Print("----- Forestry Callouts: Callouts Caching Process Started -----");
            foreach (var assem in LSPD_First_Response.Mod.API.Functions.GetAllUserPlugins())
            {
                var assemName = assem.GetName().Name;
                Game.Console.Print("Checking assembly: "+assemName);
                // check if current assembly is plugins we don't want to check, if they are break iteration
                if (assemName is "CalloutInterface" or "ForestryCallouts2" or "GrammarPolice") continue;
                // get callouts in assembly and sub classes of callout
                var baseType = typeof(Callout);
                var assemCallouts = (from callout in assem.GetTypes() where callout.IsClass && baseType.IsAssignableFrom(callout)
                    select callout).ToList();
                Game.Console.Print("Callout count from ("+assemName+"): "+assemCallouts.Count);
                // if assemCallouts les than 1 break iteration
                if (assemCallouts.Count < 1) continue;

                // callouts were found so we go through them
                foreach (var callout in assemCallouts)
                {
                    var calloutAttributes =
                        callout.GetCustomAttributes(typeof(CalloutInfoAttribute), true);

                    // getting callout attribute stuff
                    if (!calloutAttributes.Any()) continue;
                    var calloutAttribute = (CalloutInfoAttribute) (from a in calloutAttributes select a).FirstOrDefault();
                    
                    if (calloutAttribute == null) continue;

                    var calloutName = calloutAttribute.Name;
                    
                    // special cases
                    // SuperCallouts has [SC] in every callout name. this removes that.
                    if (assemName == "SuperCallouts")
                        calloutName = calloutAttribute.Name.Replace("[SC] ", string.Empty);
                    
                    Game.Console.Print(calloutName);
                    
                    // checks if callout is enabled in the designated ini file if it is add it to callouts that can be started
                    if (CalloutEnabled(assemName, calloutName))
                    {
                        RandomCalloutCache.Add(calloutAttribute.Name);
                        _callCount++;
                    }
                }
            }
            RandomCalloutCache.Add("Pursuit");
            Game.Console.Print("Cached "+_callCount +" additional callouts!");
            var endTime = DateTime.Now;
            var runTime = endTime - startTime;
            Game.Console.Print("----- Forestry Callouts: Callouts Caching Process Finished ("+runTime.Milliseconds+"ms : "+runTime.Seconds+"s) -----");
            Game.Console.Print("");
        }

        internal static void StartRandomCallout()
        {
            var randomValue = new Random();

            try
            {
                var randomCallout = RandomCalloutCache[randomValue.Next(0, RandomCalloutCache.Count)];

                LSPD_First_Response.Mod.API.Functions.StartCallout(randomCallout);
                Log.Debug("RANDOM CALLOUT STARTER", "Starting "+randomCallout+"");
            }

            catch (Exception e)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~g~FORESTRY CALLOUTS WARNING",
                    "~g~FAILED TO START RANDOM CALLOUT",
                    "Please check the rage log for more information!");
                Game.Console.Print("=============== FORESTRY CALLOUTS WARNING ===============");
                Game.Console.Print("There was an error in selecting a random callout");
                Game.Console.Print("ERROR: "+e+"");
                Game.Console.Print("Please send this log to https://dsc.gg/ulss)");
                Game.Console.Print("=============== FORESTRY CALLOUTS WARNING ===============");
            }
        }
        
        private static bool CalloutEnabled(string assemName, string callout)
        {
            InitializationFile iniFile = null;
            
            try
            {
                foreach (var f in Directory.GetFiles(@"Plugins\lspdfr").Where(f => f == @"Plugins\lspdfr\" + assemName + ".ini"))
                {
                    iniFile = new InitializationFile(f);
                    break;
                }
            }
            catch (Exception e)
            {
                return true;
            }
            
            // if iniFile null return true as this indicated there is no ini file.
            if (iniFile == null) return true;
            
            // ini file is found so get similar strings.
            var calloutEdits = GetSimilarStrings(callout);
                
            // few common sections we should check if they exist.
            string[] sections = { "Settings", "Callouts", "Callout" };
                
            // check each section for existence and then read the boolean value for the callout
            foreach (var section in sections)
            {
                try
                {
                    if (!iniFile.DoesSectionExist(section)) continue;
                    if (calloutEdits.Any(c => iniFile.ReadBoolean(section, c))) return true;
                }
                catch (Exception e)
                {
                    Game.Console.Print("Error processing ("+callout+") callout! (don't worry about this)");
                }
            }
            return false;
        }

        /// <summary>
        /// Gets similar and inputted string and returns list.
        /// </summary>
        /// <param name="s"> string that you want modified </param>
        /// <returns> list of strings </returns>
        private static List<string> GetSimilarStrings(string s)
        {
            var list = new List<string>();
            // adds the regular callout name
            list.Add(s);
            // add the callout name without numbers
            if (s.Any(char.IsDigit)) list.Add(s.RemoveIntegers());
            // removes and if and is in there.
            if (s.Contains("And")) list.Add(s.Replace("And", string.Empty));
            // removes whitespace
            list.Add(s.Replace(" ", string.Empty));
            return list;
        }
    }
}