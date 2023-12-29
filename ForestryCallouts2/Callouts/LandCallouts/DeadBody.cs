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

namespace ForestryCallouts2.Callouts.LandCallouts;
[CalloutInterface("[FC] DeadBody", CalloutProbability.Medium, "Deceased Person Found", "Code 3", "SASP")]

internal class DeadBody : Callout
{
    #region Variables

    internal readonly string CurCall = "DeadBody";
    
    //victim
    private Ped _victim;
    private Vector3 _victimSpawn;
    private Blip _victimBlip;
    //reporter
    private Ped _reporter;
    private Vector3 _reporterSpawn;
    private float _reporterHeading;
    private Blip _reporterBlip;
    //timer variables
    private int _timer = 0;
    private bool _pauseTimer;
    //search area variables
    private Blip _areaBlip;
    private Vector3 _searchArea;
    private bool _reporterFound;
    private bool _maxNotfiSent;
    private bool _firstBlip;
    private int _notfiSentCount;
    //other
    private Random _rand = new();
    private static bool _onScene;
    #endregion

    public override bool OnBeforeCalloutDisplayed()
    {
        ChunkChooser.Main(in CurCall);
        _victimSpawn = ChunkChooser.DeadBodySpawn;
        _reporterSpawn = ChunkChooser.ReporterSpawn;
        _reporterHeading = ChunkChooser.ReporterHeading;
        
        ShowCalloutAreaBlipBeforeAccepting(_victimSpawn, 30f);
        CalloutMessage = ("~g~Dead Body Found");
        CalloutPosition = _victimSpawn; 
        AddMinimumDistanceCheck(IniSettings.MinCalloutDistance, CalloutPosition);
        CalloutAdvisory = ("~b~Dispatch:~w~ Possible dead body found. Respond Code 3");
        LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CITIZENS_REPORT_01 ASSISTANCE_REQUIRED_01 IN_OR_ON_POSITION UNITS_RESPOND_CODE_02_01", _victimSpawn);
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
        //Spawn victim
        CFunctions.SpawnCountryPed(out _victim, _victimSpawn, _rand.Next(1, 361));
        _victim.Health = 10;
        //Spawn reporter
        CFunctions.SpawnHikerPed(out _reporter, _reporterSpawn, _reporterHeading);
        _reporterBlip = _reporter.AttachBlip();
        _reporterBlip.Color = Color.Yellow;
        _reporterBlip.Scale = .7f;
        _reporterBlip.EnableRoute(Color.Yellow);
        return base.OnCalloutAccepted();
    }

    public override void Process()
    {
        //End Stuff
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
        if (_victim) _victim.Dismiss();
        if (_victimBlip) _victimBlip.Delete();
        if (_reporter) _reporter.Dismiss();
        if (_reporterBlip) _reporterBlip.Delete();
        if (!ChunkChooser.StoppingCurrentCall)
        {
            Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 GP_CODE4_01", _victimSpawn);
            if (IniSettings.EndNotfiMessages) Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "Status", "~g~Dead Body Code 4", "");
            CalloutInterfaceAPI.Functions.SendMessage(this, "Unit "+IniSettings.Callsign+" reporting Dead Body code 4");
        }
        Log.CallDebug(this, "Callout ended");
        base.End();
    }
}