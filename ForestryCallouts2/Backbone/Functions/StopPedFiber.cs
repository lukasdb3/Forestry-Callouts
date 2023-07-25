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
    internal static Ped _cPed;
    internal static Persona _persona;
    private static Random _rand;
    
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
                    GameFiber.Wait(1000);
                    StopPedMenu.LicenseMenu.Visible = true;
                }
            }
        });   
    }

    internal static void OnAskForFishingLicence()
    {
        var pedsLicense = LicenseStuff.ChooseTypeOfLicense();
        //Ped does not have license
        if (pedsLicense.Type == "null") Game.DisplayNotification("~g~" + _persona.FullName + " ~w~does ~r~not~w~ have a fishing license");
    }

    internal static void OnAskForHuntingLicence()
    {
        Game.DisplayNotification("Not implemented yet.");
    }

    internal static void Dismiss()
    {
        if (StopPedMenu.LicenseMenu.Visible) StopPedMenu.LicenseMenu.Visible = false;
        if (_cPed) _cPed.Dismiss();
    }
}