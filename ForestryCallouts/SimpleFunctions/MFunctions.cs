using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using System.Drawing;
using System.Resources;
using ForestryCallouts.Callouts;
using LSPD_First_Response.Engine.Scripting.Entities;
using Rage.Native;

namespace ForestryCallouts.SimpleFunctions
{
    internal class MFunctions
    {
        static bool pedIsClsoeEnough;
        internal static void AskForHuntingLicense()
        {
            Game.LogTrivial("-!!- Forestry Callouts - |HuntingLicenseChecker| - Getting ped... -!!-");
             Ped[] peds = Game.LocalPlayer.Character.GetNearbyPeds(1);
             Ped ped = peds.First();

             if (ped.DistanceTo(Game.LocalPlayer.Character) > 8f)
             {
                 pedIsClsoeEnough = false;
             }
             if (ped.DistanceTo(Game.LocalPlayer.Character) <= 8f)
             {
                 pedIsClsoeEnough = true;
             }

             if (pedIsClsoeEnough)
             {
                 Persona personaData;
                 LHandle handle;
                 int year = System.DateTime.Today.Year;
                 personaData = LSPD_First_Response.Mod.API.Functions.GetPersonaForPed(ped);
                 Game.LogTrivial("-!!- Forestry Callouts - |HuntingLicenseChecker| - Found ped! -!!-");

                 int pedHasHuntingLicense = new Random().Next(1, 3);
                 if (pedHasHuntingLicense == 1)
                 {
                     Game.LogTrivial(
                         "-!!- Forestry Callouts - |HuntingLicenseChecker| - Ped has hunting license! -!!-");
                     
                     //If ped has hunting license this chooses if its expired or valid.
                     int licenseExpiredOrValid = new Random().Next(1, 4);
                     if (licenseExpiredOrValid == 1)
                     {
                         //License is valid
                         year = new Random().Next(DateTime.Today.Year, DateTime.Today.Year + 1);
                     }
                     if (licenseExpiredOrValid != 1)
                     {
                         //License is expired
                         year = new Random().Next(DateTime.Today.Year - 2, DateTime.Today.Year - 1);
                     }
                     //For Expiration date
                     int month = new Random().Next(1, 10);
                     int day = new Random().Next(10, 28);

                     //Combines the month, day, and year for a DD/MM/YY format
                     string expirationDate = "0" + month.ToString() + "/" + day.ToString() + "/" + year.ToString() + "";

                     //Final notification
                     if (licenseExpiredOrValid == 1)
                     {
                         //DisplayNoti if license is valid
                         Game.DisplayNotification("commonmenu", "mp_specitem_coke", "STATE ISSUED HUNTING LICENSE",
                             "~h~" + personaData.FullName.ToUpper() + "",
                             "~y~DOB: ~w~" + personaData.Birthday.Month + "/" + personaData.Birthday.Day + "/" +
                             personaData.Birthday.Year + " ~b~SEX: ~o~" + personaData.Gender + " ~g~EXPIRATION DATE:~w~ " +
                             expirationDate + "");
                     }
                     if (licenseExpiredOrValid != 1)
                     {
                         //DisplayNoti if license is expired
                         Game.DisplayNotification("commonmenu", "mp_specitem_coke", "STATE ISSUED HUNTING LICENSE",
                             "~h~" + personaData.FullName.ToUpper() + "",
                             "~y~DOB: ~w~" + personaData.Birthday.Month + "/" + personaData.Birthday.Day + "/" +
                             personaData.Birthday.Year + " ~b~SEX: ~o~" + personaData.Gender + " ~r~EXPIRATION DATE:~w~ " +
                             expirationDate + "");
                     }
                 }

                 if (pedHasHuntingLicense != 1)
                 {
                     Game.LogTrivial("-!!- Forestry Callouts - |HuntingLicenseChecker| - Ped does not have hunting license -!!-");
                     Game.DisplayNotification("~b~" +personaData.FullName+ "~w~ does ~r~not~w~ have a ~y~hunting license~w~");
                 }
             }

             if (!pedIsClsoeEnough)
             {
                 Game.DisplayNotification("No ~r~ped~w~ was found nearby to use");
             }

             Game.LogTrivial("-!!- Forestry Callouts - |HuntingLicenseChecker| - Finished -!!-");
        }
        
        internal static void AskForFishingLicense()
        {
            Game.LogTrivial("-!!- Forestry Callouts - |FishingLicenseChecker| - Getting ped... -!!-");
             Ped[] peds = Game.LocalPlayer.Character.GetNearbyPeds(1);
             Ped ped = peds.First();

             if (ped.DistanceTo(Game.LocalPlayer.Character) > 8f)
             {
                 pedIsClsoeEnough = false;
             }
             if (ped.DistanceTo(Game.LocalPlayer.Character) <= 8f)
             {
                 pedIsClsoeEnough = true;
             }

             if (pedIsClsoeEnough)
             {
                 Persona personaData;
                 LHandle handle;
                 int year = System.DateTime.Today.Year;
                 personaData = LSPD_First_Response.Mod.API.Functions.GetPersonaForPed(ped);
                 Game.LogTrivial("-!!- Forestry Callouts - |FishingLicenseChecker| - Found ped! -!!-");
                 
                 //Chooses if ped even has a license.
                 int pedHasFishingLicense = new Random().Next(1, 3);
                 if (pedHasFishingLicense == 1)
                 {
                     Game.LogTrivial("-!!- Forestry Callouts - |FishingLicenseChecker| - Ped has fishing license! -!!-");
                     
                     //If ped has fishing license this chooses if its expired or valid.
                     int licenseExpiredOrValid = new Random().Next(1, 4);
                     if (licenseExpiredOrValid == 1)
                     {
                         //License is valid
                         year = new Random().Next(DateTime.Today.Year, DateTime.Today.Year + 1);
                     }
                     if (licenseExpiredOrValid != 1)
                     {
                         //License is expired
                         year = new Random().Next(DateTime.Today.Year - 2, DateTime.Today.Year - 1);
                     }
                     //For Expiration date
                     int month = new Random().Next(1, 10);
                     int day = new Random().Next(10, 28);

                     //Combines the month, day, and year for a DD/MM/YY format
                     string expirationDate = "0" + month.ToString() + "/" + day.ToString() + "/" + year.ToString() + "";

                     //Final notification
                     if (licenseExpiredOrValid == 1)
                     {
                         //DisplayNoti if license is valid
                         Game.DisplayNotification("commonmenu", "mp_specitem_coke", "STATE ISSUED FISHING LICENSE",
                             "~h~" + personaData.FullName.ToUpper() + "",
                             "~y~DOB: ~w~" + personaData.Birthday.Month + "/" + personaData.Birthday.Day + "/" +
                             personaData.Birthday.Year + " ~b~SEX: ~o~" + personaData.Gender + " ~g~EXPIRATION DATE:~w~ " +
                             expirationDate + "");
                     }
                     if (licenseExpiredOrValid != 1)
                     {
                         //DisplayNoti if license is expired
                         Game.DisplayNotification("commonmenu", "mp_specitem_coke", "STATE ISSUED FISHING LICENSE",
                             "~h~" + personaData.FullName.ToUpper() + "",
                             "~y~DOB: ~w~" + personaData.Birthday.Month + "/" + personaData.Birthday.Day + "/" +
                             personaData.Birthday.Year + " ~b~SEX: ~o~" + personaData.Gender + " ~r~EXPIRATION DATE:~w~ " +
                             expirationDate + "");
                     }
                 }

                 if (pedHasFishingLicense != 1)
                 {
                     Game.LogTrivial("-!!- Forestry Callouts - |FishingLicenseChecker| - Ped does not have fishing license -!!-");
                     Game.DisplayNotification("~b~" +personaData.FullName+ "~w~ does ~r~not~w~ have a ~y~fishing license~w~");
                 }
             }

             if (!pedIsClsoeEnough)
             {
                 Game.DisplayNotification("No ~r~ped~w~ was found nearby to use");
             }

             Game.LogTrivial("-!!- Forestry Callouts - |FishingLicenseChecker| - Finished -!!-");
        }
    }
}