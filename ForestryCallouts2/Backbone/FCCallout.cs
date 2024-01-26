using LSPD_First_Response.Mod.Callouts;
using ForestryCallouts2.Backbone.Functions;
using ForestryCallouts2.Backbone.IniConfiguration;
using ForestryCallouts2.Backbone.SpawnSystem;
using Rage;
using LSPDFRFunctions = LSPD_First_Response.Mod.API.Functions;

namespace ForestryCallouts2.Backbone;

public abstract class FcCallout : Callout
{
    internal abstract string CurrentCall { get; set; }
    internal abstract string CurrentCallFriendlyName { get; set; }
    protected abstract Vector3 Spawnpoint { get; set; }

    public override bool OnBeforeCalloutDisplayed()
    {
        ChunkChooser.Main(CurrentCall);
        Spawnpoint = ChunkChooser.FinalSpawnpoint;
        CalloutPosition = Spawnpoint;
        AddMinimumDistanceCheck(IniSettings.MinCalloutDistance, CalloutPosition);
        ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 30f);
        return base.OnBeforeCalloutDisplayed();
    }

    public override void OnCalloutNotAccepted()
    {
        LSPDFRFunctions.PlayScannerAudio("OTHER_UNITS_TAKING_CALL");
        base.OnCalloutNotAccepted();
    }

    public override bool OnCalloutAccepted()
    {
        Log.CallDebug(this, "Callout accepted!");
        return base.OnCalloutAccepted();
    }

    public override void Process()
    {
        if (CFunctions.IsKeyAndModifierDown(IniSettings.EndCalloutKey, IniSettings.EndCalloutKeyModifier))
        {
            Log.CallDebug(this, "Callout was force ended by player!");
            End();
        }
        if (Game.LocalPlayer.Character.IsDead)
        {
            Log.CallDebug(this, "Player died callout ending!");
            End();
        }
        base.Process();
    }

    public override void End()
    {
        if (!ChunkChooser.StoppingCurrentCall)
        {
            LSPDFRFunctions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 GP_CODE4_01", Spawnpoint);
            if (IniSettings.EndNotfiMessages) Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "Status", "~g~"+CurrentCallFriendlyName+" Code 4", "");
            CalloutInterfaceAPI.Functions.SendMessage(this, "Unit "+IniSettings.Callsign+" reporting "+CurrentCallFriendlyName+" code 4");
        }
        Log.CallDebug(this, "Callout ended");
        base.End();
    }
}