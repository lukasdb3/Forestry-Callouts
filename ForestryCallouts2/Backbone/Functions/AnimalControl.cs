#region Refrences
//System
using System.Drawing;
using System.Windows.Forms;
using ForestryCallouts2.Backbone.IniConfiguration;
//Rage
using Rage;
#endregion

namespace ForestryCallouts2.Backbone.Functions
{
    internal static class AnimalControl
    {
        private static Ped _animal;
        private static Ped[] _allPeds;
        private static Vehicle _acVehicle;
        private static Ped _acPed;
        private static Blip _acBlip;
        private static GameFiber _fiber;

        private static bool _acOnScene;
        
        internal static void CallAnimalControl()
        {
            Logger.DebugLog("ANIMAL CONTROL", "Animal Control has been called");
            Logger.DebugLog("ANIMAL CONTROL", "Finding closest dead animal");
            _allPeds = World.GetAllPeds();
            
            foreach (var ped in _allPeds)
            {
                if (Game.LocalPlayer.Character.DistanceTo(ped) <= 10f && !ped.IsHuman && ped.IsDead)
                {
                    Logger.DebugLog("GetClosestPed", ped.Model.Name);
                    _animal = ped;
                }
            }
            
            //return if animal is null
            if (_animal == null)
            {
                Game.DisplayNotification("~g~Could Not Find Dead Animal");
                Logger.DebugLog("ANIMAL CONTROL", "Failed to find dead Animal");
                return;
            }

            //yeah cool stuff man
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("OFFICERS_REPORT_03 ASSISTANCE_REQUIRED_02");
            Game.DisplayNotification("~b~Dispatch:~w~ Animal Control in route to your location.");

            //spawnpoint position
            var startPosition = Game.LocalPlayer.Character.Position.Around(150f, 200f);
            var finalStartPosition = World.GetNextPositionOnStreet(startPosition);
            var animalPos = _animal.Position;
            var finalPosition = World.GetNextPositionOnStreet(finalStartPosition);

            //spawn animal control vehicle
            CFunctions.SpawnAnimalControl(out _acVehicle, finalPosition, 0f);
            _acVehicle.IsPersistent = true;
            _acVehicle.PrimaryColor = Color.White;

            //spawn animal control ped
            _acPed = new Ped("s_f_y_ranger_01", finalPosition, 0f);
            _acPed.BlockPermanentEvents = true;
            _acPed.IsPersistent = true;

            //set blip to animal control ped
            _acBlip = _acPed.AttachBlip();
            _acBlip.Color = Color.ForestGreen;

            //warp ac ped into ac vehicle
            _acPed.WarpIntoVehicle(_acVehicle, -1);

            //Get drive to postion for animal control
            var closeToAnimalPos = animalPos.Around2D(5f);
            var closeToFinalPos  = World.GetNextPositionOnStreet(closeToAnimalPos);

            Game.DisplayHelp("Press ~y~Backspace~w~ To Spawn The ~g~Animal Control~w~ Closer.");
            //fiber
            _fiber = GameFiber.StartNew(delegate
            {
                while (true)
                {
                    GameFiber.Yield();
                    if (Game.IsKeyDown(IniSettings.EndCalloutKey) || !_acPed.IsAlive || !Game.LocalPlayer.Character.IsAlive)
                    {
                        DestroyAnimalControl();
                        return;
                    }

                    if (Game.IsKeyDown(Keys.Back))
                    {
                        _acVehicle.Position = closeToFinalPos;   
                    }
                }
            });
            
            //Animal control drive to position
            _acPed.Tasks.DriveToPosition(closeToFinalPos, 9f, VehicleDrivingFlags.Normal).WaitForCompletion();
            
            //get ac to leave vehicle when on scene
            _acPed.Tasks.LeaveVehicle(_acVehicle, LeaveVehicleFlags.None).WaitForCompletion();
            Logger.DebugLog("ANIMAL CONTROL", "Animal Control officer leaving vehicle");

            //ac walks to animal
            _acPed.Tasks.FollowNavigationMeshToPosition(_animal.Position, _animal.Heading + 180f, 10f, -1).WaitForCompletion(); 
            Game.DisplaySubtitle("~g~Animal Control:~w~ Thank you, I will take care of the animal from here.");
            
            //deletes the dead animal when animal control is right next to the animal.
            if (_animal.Exists())
            {
                _animal.Delete();
                _animal = null;
            }

            //Tells the animal control to get back into the truck
            _acPed.Tasks.GoStraightToPosition(_acVehicle.Position.Around2D(2f, 4f), 10f, _acVehicle.Heading + 180f, 0f, -1).WaitForCompletion();
            _acPed.Tasks.EnterVehicle(_acVehicle, -1).WaitForCompletion();

            //Tells the animal control to drive off
            _acPed.Tasks.CruiseWithVehicle(10f, VehicleDrivingFlags.Normal);
            
            //Dismisses everything
            DestroyAnimalControl();
        }

        private static void DestroyAnimalControl()
        {
            if (_acPed) _acPed.Dismiss();
            if (_acVehicle) _acVehicle.Dismiss();
            if (_acBlip) _acBlip.Delete();
            _fiber.Abort();
        }
    }
}
