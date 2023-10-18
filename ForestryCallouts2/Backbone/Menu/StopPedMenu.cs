#region Refrences
//System
using System.Drawing;
//Rage
using Rage;
//RageNativeUI
using RAGENativeUI;
using RAGENativeUI.Elements;
//ForestryCallouts2
using ForestryCallouts2.Backbone.Functions;
using ForestryCallouts2.Backbone.IniConfiguration;
#endregion
namespace ForestryCallouts2.Backbone.Menu;

public static class StopPedMenu
{
    // Version
    private static readonly string Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        
    // Menus
    internal static UIMenu LicenseMenu = new("","");


    // Dismiss Ped
    private static UIMenuItem _dismissPed;
    private static UIMenuItem _fishingLicence = new("", "");
    private static UIMenuItem _huntingLicence = new("", "");
    
    // Licenses
    
    
    internal static void Initialize()
    {
        LicenseMenu = new UIMenu("Forestry Callouts", "~b~License Menu ~g~| ~y~v"+Version);
        LicenseMenu.SetBannerType(Color.ForestGreen);
        LicenseMenu.MouseControlsEnabled = false;

        _dismissPed = new UIMenuItem("Dismiss Ped", "");
        _fishingLicence = new UIMenuItem("Fishing License", "");
        _huntingLicence = new UIMenuItem("Hunting License", "");
        
        LicenseMenu.AddItems(_dismissPed, _fishingLicence, _huntingLicence);
        LicenseMenu.RefreshIndex();
        LicenseMenu.OnItemSelect += OnLicenseMenuItemSelect;
        Main.Pool.Add(LicenseMenu);
    }

    private static void OnLicenseMenuItemSelect(UIMenu sender, UIMenuItem selecteditem, int index)
    {
        Log.Debug("LICENSE MENU", "Item " + selecteditem.Text + " was selected!");
        if (selecteditem == _dismissPed) StopPedFiber.Dismiss();
        if (selecteditem == _fishingLicence) StopPedFiber.OnAskForFishingLicence();
        if (selecteditem == _huntingLicence) StopPedFiber.OnAskForHuntingLicence();
    }
    
    
}