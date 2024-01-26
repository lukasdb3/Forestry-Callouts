#region Refrences
//System
using System;
using System.Drawing;
using System.Windows.Forms;
using CalloutInterfaceAPI;
//Rage
using Rage;
//LSPDFR
using LSPD_First_Response.Mod.Callouts;
//ForestryCallouts2
using ForestryCallouts2.Backbone;
using ForestryCallouts2.Backbone.Functions;
using ForestryCallouts2.Backbone.IniConfiguration;
using ForestryCallouts2.Backbone.SpawnSystem;
using ForestryCallouts2.Backbone.SpawnSystem.Land;
using Functions = LSPD_First_Response.Mod.API.Functions;

#endregion

namespace ForestryCallouts2.Callouts
{
    [CalloutInterface("[FC] CalloutTemplate", CalloutProbability.Medium, "Template", "Code #", "SASP")]
    internal class Template : FcCallout
    {
        #region Variables

        internal override string CurrentCall { get; set; } = "Template";
        internal override string CurrentCallFriendlyName { get; set; } = "Template";
        protected override Vector3 Spawnpoint { get; set; }
        
        // put variables here

        #endregion

        public override bool OnBeforeCalloutDisplayed()
        {

           return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Log.CallDebug(this, "Callout accepted");

            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();
        }

        public override void End()
        {
            base.End();
        }
    }
}