#region Refrences
//System
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ForestryCallouts2.Backbone.IniConfiguration;
using ForestryCallouts2.Backbone.Menu;
using LSPD_First_Response.Engine.Scripting.Entities;
//Rage
using Rage;
#endregion

namespace ForestryCallouts2.Backbone.Functions;

internal static class StopPedFiber
{
    internal static GameFiber Fiber;
    private static Ped _cPed;
    private static Persona _persona;
    private static Random _rand;
    private static License _pedsLicense;

    internal static void Main()
    {
        Fiber = GameFiber.StartNew(delegate
        {
            while (true)
            {
                GameFiber.Yield();
                if (CFunctions.IsKeyAndModifierDown(IniSettings.StopPedKey, IniSettings.StopPedKeyModifier))
                {
                    _cPed = CFunctions.GetValidPedsNearby(10).FirstOrDefault();
                    if (_cPed == null) return;
                    if (!_cPed.IsAlive) return;

                    _persona = LSPD_First_Response.Mod.API.Functions.GetPersonaForPed(_cPed);
                    _cPed.IsPersistent = true;
                    _cPed.BlockPermanentEvents = true;
                    _cPed.Tasks.StandStill(-1);
                    _cPed.Heading = Game.LocalPlayer.Character.Heading + 180f;
                    GameFiber.Wait(700);
                    StopPedMenu.LicenseMenu.Visible = true;
                }
            }
        });   
    }

    internal static void OnAskForFishingLicence()
    {
        if (License.FishingDict.ContainsKey(_persona.FullName))
        {
            foreach (var t in License.FishingDict)
            {
                if (t.Key == _persona.FullName) _pedsLicense = t.Value;
            }
        }
        else
        {
            _pedsLicense = License.ChooseTypeOfLicense("Fishing");
            _pedsLicense = License.CreateLicence(_persona, _pedsLicense);
        }
        License.DisplayLicenceInfo(_pedsLicense);
        
    }

    internal static void OnAskForHuntingLicence()
    {
        if (License.HuntingDict.ContainsKey(_persona.FullName))
        {
            foreach (var t in License.FishingDict)
            {
                if (t.Key == _persona.FullName) _pedsLicense = t.Value;
            }
        }
        else
        {
            _pedsLicense = License.ChooseTypeOfLicense("Hunting");
            _pedsLicense = License.CreateLicence(_persona, _pedsLicense);
        }
        License.DisplayLicenceInfo(_pedsLicense);
    }

    internal static void Dismiss()
    {
        if (StopPedMenu.LicenseMenu.Visible) StopPedMenu.LicenseMenu.Visible = false;
        if (_cPed) _cPed.Dismiss();
    }
}