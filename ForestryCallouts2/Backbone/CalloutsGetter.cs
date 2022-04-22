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
        internal static List<string> randomCalloutCache = new List<string>();

        static CalloutsGetter()
        {
            CacheCallouts();
        }

        internal static void CacheCallouts()
        { 
            Game.Console.Print("Caching players callouts..");
            foreach (Assembly assem in Functions.GetAllUserPlugins())
            {
                AssemblyName assemName = assem.GetName();
                if (assemName.ToString() != "ForestryCallouts2")
                {
                    List<Type> assemCallouts = (from Callout in assem.GetTypes()
                        where Callout.IsClass && Callout.BaseType == typeof(LSPD_First_Response.Mod.Callouts.Callout)
                        select Callout).ToList();

                    if (assemCallouts.Count < 1)
                    {
                        //no callouts in assembly
                    }
                    else
                    {
                        int addCount = 0;
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
                                    addCount++;
                                }
                            }
                        }
                        Game.Console.Print("Caching callouts finished");
                    }
                }
            }
        }

        internal static string StartRandomCallout()
        {
            Random randomValue = new Random();

            try
            {
                string randomCallout = randomCalloutCache[randomValue.Next(0, randomCalloutCache.Count)];

                Functions.StartCallout(randomCallout);
                return randomCallout;
            }

            catch
            {
                Logger.Log("[Callout Getter] - WARNING: There was an error trying to start a random callout");
                return null;
            }
        }
    }
}