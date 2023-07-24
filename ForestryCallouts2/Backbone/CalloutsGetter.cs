#region Refrences
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
    internal static class CalloutsGetter
    {
        private static readonly List<string> RandomCalloutCache = new();
        internal static readonly List<string> ForestryCalloutsCalls = new();
        internal static readonly List<string> ForestryCalloutsWaterCalls = new();
        private static int _callCount;

        internal static bool IsCalloutEnabledInIni(string assemName, string callout)
        {
            //Get all files in user plugins lspdfr directory
            string[] _allFiles = Directory.GetFiles("Plugins/LSPDFR");
            //For each file we check if its the designated assembly's ini file
            List<string> calloutEdits = new List<string>();
            foreach (var file in _allFiles)
            {
                if (file.Contains(assemName + ".ini"))
                {
                    //We make similar strings to the callout as some authors don't use the same name as the class
                    calloutEdits.Add(callout);
                    if (callout.Any(char.IsDigit)) calloutEdits.Add(callout.RemoveIntegers());
                    if (callout.Contains("And")) callout.Replace("And", String.Empty);
                    //Set the file to an ini file
                    var iniFile = new InitializationFile(file);
                    //Few common sections we should check if they exist
                    string[] sections = {"Settings", "Callouts", "Callout"};
                    //Check each section for existence and then read the boolean value for the callout
                    foreach (var section in sections)
                    {
                        if (iniFile.DoesSectionExist(section))
                        {
                            foreach (var call in calloutEdits)
                            {
                                if (iniFile.ReadBoolean(section, call)) return true;   
                            }
                        }   
                    }
                }
                else
                {
                    //assem does not have an ini file to go with it, therefore we return true as the callout has to be enabled.
                    return true;
                }
            }
            return false;
        }
        
        internal static void CacheCallouts()
        {
            foreach (Assembly assem in LSPD_First_Response.Mod.API.Functions.GetAllUserPlugins())
            {
                string assemName = assem.GetName().Name;
                if (assemName != "CalloutInterface")
                {
                    List<Type> assemCallouts = (from callout in assem.GetTypes() where callout.IsClass && callout.BaseType == typeof(Callout) select callout).ToList();
                    
                    if (assemCallouts.Count < 1)
                    {
                        //no callouts in assembly
                    }
                    else
                    {
                        foreach (Type callout in assemCallouts)
                        {
                            var calloutAttributes =
                                callout.GetCustomAttributes(typeof(CalloutInfoAttribute), true);

                            if (calloutAttributes.Any())
                            {
                                CalloutInfoAttribute calloutAttribute =
                                    (CalloutInfoAttribute) (from a in calloutAttributes select a).FirstOrDefault();

                                if (calloutAttribute != null && assemName != "ForestryCallouts2")
                                {
                                    if (IsCalloutEnabledInIni(assemName, calloutAttribute.Name))
                                    {
                                        RandomCalloutCache.Add(calloutAttribute.Name);
                                        _callCount++;
                                    }
                                    else
                                    {
                                        Game.Console.Print(assemName + " " + callout + " Is Disabled");
                                    }
                                        
                                }

                                if (calloutAttribute == null || assemName != "ForestryCallouts2") continue;
                                if (calloutAttribute.Name is "DeadBodyWater" or "BoatPursuit")
                                {
                                    ForestryCalloutsWaterCalls.Add(calloutAttribute.Name);
                                }
                                else ForestryCalloutsCalls.Add(calloutAttribute.Name);
                            }
                        }
                    }
                }
            }
            Game.Console.Print("Cached "+_callCount+" callouts!");
            Game.Console.Print("Caching callouts finished");
        }

        internal static void StartRandomCallout()
        {
            Random randomValue = new Random();

            try
            {
                string randomCallout = RandomCalloutCache[randomValue.Next(0, RandomCalloutCache.Count)];

                LSPD_First_Response.Mod.API.Functions.StartCallout(randomCallout);
                Logger.DebugLog("RANDOM CALLOUT STARTER", "Starting "+randomCallout+"");
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
                Game.Console.Print("ADDITIONAL INFORMATION:");
                Game.Console.Print("This error may have been caused by ForestryCallouts being the");
                Game.Console.Print("only source of callouts in your plugins folder. If you are only playing with");
                Game.Console.Print("Forestry Callouts please disable the distance checker in the ini file.");
                Game.Console.Print("=============== FORESTRY CALLOUTS WARNING ===============");
            }
        }
    }
}