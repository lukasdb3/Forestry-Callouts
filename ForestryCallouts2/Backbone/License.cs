#region Refrences
//System
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ForestryCallouts2.Backbone.Functions;
//Rage
using Rage;
using Rage.Native;
//ForestryCallouts2
using ForestryCallouts2.Backbone.IniConfiguration;
using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Mod.Callouts;

#endregion

namespace ForestryCallouts2.Backbone;

public class License
{
    internal string Type { get; set; }
    private int Chance { get; set; }
    internal string HolderName { get; private set; }
    internal string DateOfBirth { get; private set; }
    internal string HolderGender { get; private set; }

    internal DateTime ExpDate { get; private set; }

    private static Random _rnd = new();

    internal static Dictionary<string, License> FishingDict = new();
    internal static Dictionary<string, License> HuntingDict = new();

    internal static License ChooseTypeOfLicense(string sender)
    {
        var licenseList = new List<License>() { };
        if (sender == "Fishing")
        {
            licenseList.Add(new License() { Type = "ResidentialFishingLicense", Chance = IniSettings.ResidentLicense });
            licenseList.Add(new License() { Type = "NonResidentialFishingLicense", Chance = IniSettings.NonResidentLicense });
            licenseList.Add(new License() { Type = "OneDayFishingLicense", Chance = IniSettings.OneDayLicense });
            licenseList.Add(new License() { Type = "TwoDayFishingLicense", Chance = IniSettings.TwoDayLicense });
        }
        else
        {
            licenseList.Add(new License() { Type = "ResidentialHuntingLicense", Chance = IniSettings.ResidentLicense });
            licenseList.Add(new License() { Type = "NonResidentialHuntingLicense", Chance = IniSettings.NonResidentLicense });
            licenseList.Add(new License() { Type = "OneDayHuntingLicense", Chance = IniSettings.OneDayLicense });
            licenseList.Add(new License() { Type = "TwoDayHuntingLicense", Chance = IniSettings.TwoDayLicense });
        }
        licenseList.Add(new License() { Type = "null", Chance = IniSettings.NoLicense });
        return SelectLicense(licenseList);
    }

    private static License SelectLicense(List<License> licenses)
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
            {
                Logger.DebugLog("Select License", "Selected " + t.Type);
                return t;
            }
        }
        return null;
    }

    internal static License CreateLicence(in Persona persona, in License license)
    {
        //Create a new license for the ped
        license.HolderName = persona.FullName;
        if (license.Type != "null")
        {
            license.DateOfBirth = persona.Birthday.ToShortDateString();
            license.HolderGender = persona.Gender.ToString();
            license.ExpDate = GetExpirationDate(license);
        }

        //Add ped as key and license as val to dict
        if (license.Type.Contains("Fishing")) FishingDict.Add(license.HolderName, license);
        else HuntingDict.Add(license.HolderName, license);
        return license;
    }

    private static DateTime GetExpirationDate(in License license)
    {
        var rawLicenseStatus = GetLicenseStatus();
        var type = license.Type;
        var sysDate = DateTime.Today;
        switch (type)
        {
            case "ResidentialFishingLicense" or "NonResidentialFishingLicense" when rawLicenseStatus.Status == "Expired":
                var minDate = new DateTime(sysDate.Year - 1, sysDate.Month, sysDate.Day);
                var maxDate = new DateTime(sysDate.Year, sysDate.Month, sysDate.Day - 1);
                return GetRandomDateBetween(minDate, maxDate);
            case "ResidentialFishingLicense" or "NonResidentialFishingLicense":
                minDate = sysDate;
                maxDate = new DateTime(sysDate.Year + 1, sysDate.Month, sysDate.Day);
                return GetRandomDateBetween(minDate, maxDate);
            case "OneDayFishingLicense" when rawLicenseStatus.Status == "Expired":
                return new DateTime(sysDate.Year, sysDate.Month, sysDate.Day - 1);
            case "OneDayFishingLicense":
                return sysDate;
            case "TwoDayFishingLicense" when rawLicenseStatus.Status == "Expired":
                minDate = new DateTime(sysDate.Year, sysDate.Month, sysDate.Day -7);
                maxDate = new DateTime(sysDate.Year, sysDate.Month, sysDate.Day -1);
                return GetRandomDateBetween(minDate, maxDate);
            case "TwoDayFishingLicense":
                minDate = sysDate;
                maxDate = new DateTime(sysDate.Year, sysDate.Month+1, sysDate.Day);
                return GetRandomDateBetween(minDate, maxDate);
            case "ResidentialHuntingLicense" or "NonResidentialHuntingLicense" when rawLicenseStatus.Status == "Expired":
                minDate = new DateTime(sysDate.Year - 1, sysDate.Month, sysDate.Day);
                maxDate = new DateTime(sysDate.Year, sysDate.Month, sysDate.Day - 1);
                return GetRandomDateBetween(minDate, maxDate);
            case "ResidentialHuntingLicense" or "NonResidentialHuntingLicense":
                minDate = sysDate;
                maxDate = new DateTime(sysDate.Year + 1, sysDate.Month, sysDate.Day);
                return GetRandomDateBetween(minDate, maxDate);
            case "OneDayHuntingLicense" when rawLicenseStatus.Status == "Expired":
                return new DateTime(sysDate.Year, sysDate.Month, sysDate.Day - 1);
            case "OneDayHuntingLicense":
                return sysDate;
            case "TwoDayHuntingLicense" when rawLicenseStatus.Status == "Expired":
                minDate = new DateTime(sysDate.Year, sysDate.Month, sysDate.Day -7);
                maxDate = new DateTime(sysDate.Year, sysDate.Month, sysDate.Day -1);
                return GetRandomDateBetween(minDate, maxDate);
            case "TwoDayHuntingLicense":
                minDate = sysDate;
                maxDate = new DateTime(sysDate.Year, sysDate.Month+1, sysDate.Day);
                return GetRandomDateBetween(minDate, maxDate);
        }
        return sysDate;
    }

    private class LicenseStatus
    {
        internal string Status;
        internal int Chance;
    }
    private static LicenseStatus GetLicenseStatus()
    {
        List<LicenseStatus> statuses = new List<LicenseStatus>();
        statuses.Add(new LicenseStatus() {Status = "Expired", Chance = IniSettings.Expired});
        statuses.Add(new LicenseStatus() {Status = "Valid", Chance = IniSettings.Valid});
        // Calculate the sum.
        var poolSize = statuses.Sum(t => t.Chance);

        // Get a random integer from 0 to PoolSize.
        var randomNumber = _rnd.Next(0, poolSize) + 1;

        // Detect the item, which corresponds to current random number.
        var accumulatedProbability = 0;
        foreach (var t in statuses)
        {
            accumulatedProbability += t.Chance;
            if (randomNumber <= accumulatedProbability)
            {
                Logger.DebugLog("Select License Status", "Selected " + t.Status);
                return t;
            }
        }
        return null;
    }

    private static DateTime GetRandomDateBetween(DateTime startDate, DateTime endDate)
    {
        // Calculate the total number of days between the two dates
        var totalDays = (int)(endDate - startDate).TotalDays;

        // Generate a random number of days between 0 and the total difference in days
        var randomDays = _rnd.Next(totalDays + 1);

        // Add the random number of days to the start date to get an intermediate date
        var intermediateDate = startDate.AddDays(randomDays);

        // Generate random year, month, and day components
        var randomYear = _rnd.Next(intermediateDate.Year, endDate.Year + 1);
        var randomMonth = randomYear == intermediateDate.Year ? _rnd.Next(intermediateDate.Month, 13) : _rnd.Next(1, 13);
        var randomDay = randomYear == intermediateDate.Year && randomMonth == intermediateDate.Month ? _rnd.Next(intermediateDate.Day, DateTime.DaysInMonth(randomYear, randomMonth) + 1) : _rnd.Next(1, DateTime.DaysInMonth(randomYear, randomMonth) + 1);

        // Create the final random date
        return new DateTime(randomYear, randomMonth, randomDay);
    }

    internal static void DisplayLicenceInfo(License license)
    {
        var sender = "";
        sender = license.Type.Contains("Fishing") ? "fishing" : "hunting";
        if (license.Type != "null")
        {
            Game.DisplayNotification("commonmenu", "mp_specitem_coke", GetNiceTypeString(license),
                "~h~" + license.HolderName.ToUpper() + "",
                "~y~DOB: ~w~" + license.DateOfBirth + " ~b~SEX: ~o~" + license.HolderGender.ToUpper() +
                " ~g~EXPIRATION DATE:~w~ " +
                license.ExpDate.ToShortDateString() + "");
        }
        else
        {
            Game.DisplayNotification(license.HolderName + " does not have a "+sender+" license.");
        }
    }
    
    private static string GetNiceTypeString(License license)
    {
        return license.Type switch
        {
            "ResidentialFishingLicense" => "RESIDENT FISHING LICENCE",
            "NonResidentialFishingLicense" => "NON RESIDENT FISHING LICENSE",
            "OneDayFishingLicense" => "ONE DAY FISHING LICENCE",
            "TwoDayFishingLicense" => "TWO DAY FISHING LICENSE",
            "ResidentialHuntingLicense" => "RESIDENT HUNTING LICENCE",
            "NonResidentialHuntingLicense" => "NON RESIDENT HUNTING LICENCE",
            "OneDayHuntingLicense" => "ONE DAY HUNTING LICENCE",
            "TwoDayHuntingLicense" => "TWO DAY HUNTING LICENSE",
            _ => null
        };
    }
}

