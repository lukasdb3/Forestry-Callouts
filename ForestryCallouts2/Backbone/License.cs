#region Refrences
//System
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
//Rage
using Rage;
using Rage.Native;
//ForestryCallouts2
using ForestryCallouts2.Backbone.IniConfiguration;
using LSPD_First_Response.Mod.Callouts;

#endregion

namespace ForestryCallouts2.Backbone;

public class LicenseType
{
    internal string Type;
    internal int Chance;
}

public class License : LicenseType
{
    internal string HolderName;
    internal string HolderDob;
    internal string ExpDate;
}
public static class LicenseStuff
{
    private static Random _rnd = new Random();

    private static LicenseType SelectLicense(List<LicenseType> licenses)
    {
        // Calculate the sum.
        var poolSize = licenses.Sum(t => t.Chance);

        // Get a random integer from 0 to PoolSize.
        var randomNumber = _rnd.Next(0, poolSize) + 1;

        // Detect the item, which corresponds to current random number.
        var accumulatedProbability = 0;
        foreach (var t in licenses)
        {
            accumulatedProbability += t.Chance;
            if (randomNumber <= accumulatedProbability)
                return t;
        }
        return null; 
    }
    
    internal static LicenseType ChooseTypeOfLicense()
    {
        var licenseList = new List<LicenseType>() { };
        licenseList.Add(new LicenseType() {Type = "ResidentialFishingLicense", Chance = IniSettings.ResidentLicense});
        licenseList.Add(new LicenseType() {Type = "NonResidentialFishingLicense", Chance = IniSettings.NonResidentLicense});
        licenseList.Add(new LicenseType() {Type = "OneDayFishingLicense", Chance = IniSettings.OneDayLicense});
        licenseList.Add(new LicenseType() {Type = "TwoDayFishingLicense", Chance = IniSettings.TwoDayLicense});
        licenseList.Add(new LicenseType() {Type = "null", Chance = IniSettings.NoLicense});
        return SelectLicense(licenseList);
    }
}

