using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using System.Drawing;
using System;

namespace ForestryCallouts2.Callouts.LandCallouts
{
    
    [CalloutInfo("IntoxicatedPerson", CalloutProbability.Medium)]
    
    internal class IntoxicatedPerson : Callout
    {
        public override bool OnBeforeCalloutDisplayed()
        {
            
            
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            
            
            return base.OnCalloutAccepted();
        }
    }
}