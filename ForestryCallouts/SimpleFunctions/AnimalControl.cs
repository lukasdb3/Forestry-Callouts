using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using System.Drawing;
using ForestryCallouts.Callouts;
using ForestryCallouts.Ini;

namespace ForestryCallouts.SimpleFunctions
{
    internal class AnimalControl
    {
        internal static Vehicle acVehicle;
        internal static Ped acPed;
        internal static Blip acBlip;
        internal static void CallAnimalControl(in Ped animal)
        { 
            

            //yeah cool stuff man
            Game.LogTrivial("-!!- Forestry Callouts - |AnimalControl| - Animal Control Called -!!-");
            Game.DisplayNotification("~g~Dispatch:~w~ Animal Control is in route to your location");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("OFFICERS_REPORT_03 ASSISTANCE_REQUIRED_02");

            //spawnpoint position
            var startPosition = Game.LocalPlayer.Character.Position.Around(150f, 200f);
            var finalStartPosition = World.GetNextPositionOnStreet(startPosition);
            var animalPos = animal.Position;
            var aroundAnimalPos = animalPos.Around2D(150f);
            var finalPosition = World.GetNextPositionOnStreet(finalStartPosition);

            //spawn animal control vehicle
            acVehicle = new Vehicle(IniSettings.AnimalControlModel, finalPosition);
            acVehicle.IsPersistent = true;

            acVehicle.PrimaryColor = Color.White;

            //spawn animal control ped
            acPed = new Ped("s_f_y_ranger_01", finalPosition, 0f);
            acPed.BlockPermanentEvents = true;
            acPed.IsPersistent = true;

            //set blip to animal control ped
            acBlip = acPed.AttachBlip();
            acBlip.Color = Color.Lime;

            //warp ac ped into ac vehicle
            acPed.WarpIntoVehicle(acVehicle, -1);

            //Get drive to postion for animal control
            var closeToAnimalPos = animalPos.Around2D(5f);
            var closeToFinalPos  = World.GetNextPositionOnStreet(closeToAnimalPos);

            //Animal control drive to position
            acPed.Tasks.DriveToPosition(closeToFinalPos, 10f, VehicleDrivingFlags.Normal).WaitForCompletion();

            //get ac to leave vehicle when on scene
            acPed.Tasks.LeaveVehicle(acVehicle, LeaveVehicleFlags.None).WaitForCompletion();
            Game.LogTrivial("-!!- Forestry Callouts - |AnimalControl| - Animal Control leaving vehicle -!!-");

            //ac walks to animal
            acPed.Tasks.FollowNavigationMeshToPosition(animal.Position, animal.Heading + 180f, 10f, -1).WaitForCompletion(); 
            Game.DisplaySubtitle("~g~Animal Control:~w~ Thank you, I will take care of the animal from here.");
            
            //deletes the dead animal when animal control is right next to the animal.
            if (animal.Exists())
            {
                animal.Delete();
            }

            //Tells the animal control to get back into the truck
            acPed.Tasks.GoStraightToPosition(acVehicle.Position.Around2D(2f, 4f), 10f, acVehicle.Heading + 180f, 0f, -1).WaitForCompletion();
            acPed.Tasks.EnterVehicle(acVehicle, -1).WaitForCompletion();

            //Tells the animal control to drive off
            acPed.Tasks.CruiseWithVehicle(10f, VehicleDrivingFlags.Normal);
            
            //Dismisses everything
            destroyAnimalControl();
        }

        internal static void destroyAnimalControl()
        {
            if (acPed.Exists())
            {
                acPed.Dismiss();
            }
            if (acVehicle.Exists())
            {
                acVehicle.Dismiss();
            }
            if (acBlip.Exists())
            {
                acBlip.Delete();
            }
        }
    }
}
