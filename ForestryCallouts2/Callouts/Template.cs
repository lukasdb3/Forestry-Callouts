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
    internal class Template : Callout
    {
        #region Variables

        internal readonly string CurCall = "Template";

        Vector3 spawnpoint = new Vector3();
        // put variables here

        #endregion

        public override bool OnBeforeCalloutDisplayed()
        {

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

            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            //End Callout
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
            if (!ChunkChooser.StoppingCurrentCall)
            {
                Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 GP_CODE4_01", spawnpoint);
                if (IniSettings.EndNotfiMessages) Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "Status", "~g~{TEMPLATE} Code 4", "");
                CalloutInterfaceAPI.Functions.SendMessage(this, "Unit "+IniSettings.Callsign+" reporting {TEMPLATE} code 4");
            }
            Log.CallDebug(this, "Callout ended");
            base.End();
        }
    }
}