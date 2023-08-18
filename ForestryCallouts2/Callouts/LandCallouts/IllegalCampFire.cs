/*#region Refrences
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

namespace ForestryCallouts2.Callouts.LandCallouts;
[CalloutInterface("Illegal Camp Fire", CalloutProbability.Medium, "Burning Fire", "Code 2", "SASP")]
internal class IllegalCampFire : Callout
{
    #region Variables

    private Ped _suspect;
    private float _suspectHeading;
    private Blip _suspectBlip;
    internal readonly string CurCall = "IllegalCampFire";
    #endregion

    public override bool OnBeforeCalloutDisplayed()
    {
        return base.OnBeforeCalloutDisplayed();
    }

    public override void OnCalloutNotAccepted()
    {
        base.OnCalloutNotAccepted();
    }

    public override bool OnCalloutAccepted()
    {
        return base.OnCalloutAccepted();
    }

    public override void Process()
    {
        if (CFunctions.IsKeyAndModifierDown(IniSettings.EndCalloutKey, IniSettings.EndCalloutKeyModifier))
        {
            Logger.CallDebugLog(this, "Callout was force ended by player");
            End();
        }
        if (Game.LocalPlayer.Character.IsDead)
        {
            Logger.CallDebugLog(this, "Player died callout ending");
            End();
        }
        base.Process();
    }

    public override void End()
    {
        base.End();
    }
}*/