using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using RAGENativeUI.Elements;

namespace ForestryCallouts2.Backbone
{
    internal static class CalloutsGetter
    {
        private static List<string> _randomCalloutCache = new List<string>();
        private static int _callCount;
        internal static void CacheCallouts()
        {
            foreach (Assembly assem in LSPD_First_Response.Mod.API.Functions.GetAllUserPlugins())
            {
                string assemName = assem.GetName().Name;
                if (assemName != Assembly.GetExecutingAssembly().GetName().Name && assemName != "CalloutInterface")
                {
                    Logger.DebugLog("CALLOUT CACHE", "Assembly checking: "+assemName+"");
                    List<Type> assemCallouts = (from Callout in assem.GetTypes()
                        where Callout.IsClass && Callout.BaseType == typeof(Callout)
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
                                    _randomCalloutCache.Add(CalloutAttribute.Name);
                                    _callCount++;
                                }
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
                string randomCallout = _randomCalloutCache[randomValue.Next(0, _randomCalloutCache.Count)];

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