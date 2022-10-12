using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;

namespace ForestryCallouts2.Backbone
{
    internal static class CalloutsGetter
    {
        private static List<string> randomCalloutCache = new List<string>();
        private static int callCount;

        internal static void CacheCallouts()
        { 
            Game.Console.Print("Caching players callouts..");
            Logger.DebugLog("CALLOUT CACHER", "Assembly using: "+System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString()+"");
            foreach (Assembly assem in LSPD_First_Response.Mod.API.Functions.GetAllUserPlugins())
            {
                AssemblyName assemName = assem.GetName();
                if (assemName.ToString() != System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString())
                {
                    Logger.DebugLog("CALLOUT CACHER", "Assembly checking: "+assemName+"");
                    List<Type> assemCallouts = (from Callout in assem.GetTypes()
                        where Callout.IsClass && Callout.BaseType == typeof(LSPD_First_Response.Mod.Callouts.Callout)
                        select Callout).ToList();

                    if (assemCallouts.Count < 1)
                    {
                        //no callouts in assembly
                    }
                    else
                    {
                        foreach (Type callout in assemCallouts)
                        {
                            object[] CalloutAttributes =
                                callout.GetCustomAttributes(typeof(CalloutInfoAttribute), true);

                            if (CalloutAttributes.Count() > 0)
                            {
                                CalloutInfoAttribute CalloutAttribute =
                                    (CalloutInfoAttribute) (from a in CalloutAttributes select a).FirstOrDefault();

                                if (CalloutAttribute != null)
                                {
                                    randomCalloutCache.Add(CalloutAttribute.Name);
                                    callCount++;
                                }
                            }
                        }
                    }
                }
            }
            Game.Console.Print("Cached "+callCount+" callouts!");
            Game.Console.Print("Caching callouts finished");
        }

        internal static void StartRandomCallout()
        {
            Random randomValue = new Random();

            try
            {
                string randomCallout = randomCalloutCache[randomValue.Next(0, randomCalloutCache.Count)];

                LSPD_First_Response.Mod.API.Functions.StartCallout(randomCallout);
                Logger.DebugLog("RANDOM CALLOUT STARTER", "Starting "+randomCallout+"");
            }

            catch (Exception e)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~g~Forestry Callouts 2 Warning",
                    "~g~Failed to start random callout",
                    "Please check log for more details");
                Game.Console.Print("=============== FORESTRY CALLOUTS WARNING ===============");
                Game.Console.Print("There was an error in selecting a random callout");
                Game.Console.Print("ERROR: "+e+"");
                Game.Console.Print("Please send this log to https://dsc.gg/ulss)");
                Game.Console.Print("=============== FORESTRY CALLOUTS WARNING ===============");
            }
        }
    }
}