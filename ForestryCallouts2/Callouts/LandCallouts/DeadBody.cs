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
[CalloutInterface("Dead Body", CalloutProbability.Medium, "Deceased Person Found", "Code 2", "SASP")]

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
        CalloutAdvisory = ("~b~Dispatch:~w~ Dead body found by citizen. Respond code 2");
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
        Logger.CallDebugLog(this, "Callout accepted");
        //Spawn victim
        CFunctions.SpawnCountryPed(out _victim, _victimSpawn, _rand.Next(1, 361));
        _victimBlip = _victim.AttachBlip();
        _victimBlip.EnableRoute(Color.Yellow);
        _victim.Health = 10;
        //Spawn reporter
        CFunctions.SpawnHikerPed(out _reporter, _reporterSpawn, _reporterHeading);
        return base.OnCalloutAccepted();
    }

    public override void Process()
    {
        if (Game.LocalPlayer.Character.DistanceTo(_victim) <= 200f && !_onScene)
        {
            CalloutInterfaceAPI.Functions.SendMessage(this, "Unit "+IniSettings.Callsign+" proceed with caution.");
            Functions.PlayScannerAudioUsingPosition("GP_ATTENTION_UNIT "+Main.CallsignAudioString+" SUSPECT_LAST_SEEN_01 IN_OR_ON_POSITION", _victimSpawn);
            Logger.CallDebugLog(this, "Process started");
            _onScene = true;
            if (_victimBlip) _victimBlip.Delete();
            _firstBlip = true;
        }   
        
        if (!_pauseTimer)
        {
            _timer++;
        }

        if (!_reporterFound)
        {
            if (_firstBlip && _timer >= 1 || _timer >= 1250)
            {
                if (_areaBlip) _areaBlip.Delete();
                var position = _victim.Position;
                _searchArea = position.Around2D(10f, 20f);
                _areaBlip = new Blip(_searchArea, 35f) {Color = Color.Yellow, Alpha = .5f};
                _notfiSentCount++;
                Logger.CallDebugLog(this, "Search areas sent: " + _notfiSentCount + "");
                _firstBlip = false;
                _timer = 0;
            }

            //we delete the search area, and blip the victim because the player is taking to long to find the victim
            if (_notfiSentCount == IniSettings.SearchAreaNotifications && !_maxNotfiSent)
            {
                //Pause the timer so search blips dont keep coming in
                Logger.CallDebugLog(this, "Blipped victim because player took to long to find them.");
                _pauseTimer = true;
                if (_areaBlip) _areaBlip.Delete();
                _victimBlip = _victim.AttachBlip();
                _victimBlip.Color = Color.ForestGreen;
                _victimBlip.Scale = .7f;
                _victimBlip.IsRouteEnabled = true;
                _maxNotfiSent = true;
            }
        }
        
        if (Game.LocalPlayer.Character.DistanceTo(_reporter) <= 10f)
        {
            Logger.CallDebugLog(this, "Reporter found");
            _reporterFound = true;
            _reporterBlip = _reporter.AttachBlip();
            _reporterBlip.Scale = .7f;
            _reporterBlip.Color = Color.Green;
            _victimBlip = _victim.AttachBlip();
            _victimBlip.Scale = .7f;
            _victimBlip.Color = Color.Red;
        }
        
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
        if (_victim) _victim.Dismiss();
        if (_victimBlip) _victimBlip.Delete();
        if (_reporter) _reporter.Dismiss();
        if (_reporterBlip) _reporterBlip.Delete();
        if (!ChunkChooser.StoppingCurrentCall)
        {
            Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 GP_CODE4_01", _victimSpawn);
            Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "Status", "~g~Dead Body Code 4", "");
            CalloutInterfaceAPI.Functions.SendMessage(this, "Unit "+IniSettings.Callsign+" reporting Dead Body code 4");
        }
        Logger.CallDebugLog(this, "Callout ended");
        base.End();
    }
}