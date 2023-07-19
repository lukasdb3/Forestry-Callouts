#region Refrences
//System
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ForestryCallouts2.Backbone.IniConfiguration;
//Rage
using Rage;
#endregion

namespace ForestryCallouts2.Backbone.Functions
{
    internal static class AnimalControl
    {
        internal static bool AnimalControlActive = false;
        
        private static Ped _animal;
        private static List<Ped> _allPeds;
        private static Vehicle _acVehicle;
        private static Ped _acPed;
        private static Blip _acBlip;
        private static GameFiber _fiber;

        private static int _counter = 0;
        
        internal static void CallAnimalControl()
        {
            Logger.DebugLog("ANIMAL CONTROL", "Animal Control has been called");
            Logger.DebugLog("ANIMAL CONTROL", "Finding closest dead animal");
            _allPeds = CFunctions.GetValidPedsNearby(10);
            
            //return if animal is null
            if (!_allPeds.Any())
            {
                Game.DisplayNotification("~g~Could Not Find Dead Animal");
                Logger.DebugLog("ANIMAL CONTROL", "Failed to find dead Animal");
                return;
            }
            
            foreach (var ped in _allPeds.Where(ped => Game.LocalPlayer.Character.DistanceTo(ped) <= 10f && !ped.IsHuman && ped.IsDead))
            {
                Logger.DebugLog("ANIMAL CONTROL", ped.Model.Name);
                _animal = ped;
                break;
            }

            AnimalControlActive = true;
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
            var closeToAnimalPos = animalPos.Around2D(8f);
            var closeToFinalPos  = World.GetNextPositionOnStreet(closeToAnimalPos);

            Game.DisplayHelp("Hold ~y~Backspace~w~ To Spawn The ~g~Animal Control~w~ Unit Closer");
            //fiber
            _fiber = GameFiber.StartNew(delegate
            {
                while (true)
                {
                    GameFiber.Yield();
                    //If animal control is dead or player is dead destroy everything involved and return;
                    if (CFunctions.IsKeyAndModifierDown(IniSettings.EndCalloutKey, IniSettings.EndCalloutKeyModifier) || !_acPed.IsAlive || !Game.LocalPlayer.Character.IsAlive)
                    {
                        _acPed.Tasks.Clear();
                        DestroyAnimalControl();
                        return;
                    }
                    
                    //Resets counter if back key isn't down.
                    if (!Game.IsKeyDown(Keys.Back) && _counter != 0)
                    {
                        _counter = 0;
                    }

                    //While back key is down for 5 ish seconds spawn animal control closer.
                    while (Game.IsKeyDownRightNow(Keys.Back))
                    {
                        GameFiber.Yield();
                        _counter++;
                        if (_counter == 150) _acVehicle.Position = closeToFinalPos;   
                    }
                    
                    //Cool blip stuff
                    _acBlip.Scale = _acPed.IsOnFoot ? .70f : 1f;
                }
            });
            
            //Animal control drive to position
            if (_acPed) _acPed.Tasks.DriveToPosition(closeToFinalPos, 9f, VehicleDrivingFlags.AllowMedianCrossing).WaitForCompletion();
            
            //get ac to leave vehicle when on scene
            if (_acPed) _acPed.Tasks.LeaveVehicle(_acVehicle, LeaveVehicleFlags.None).WaitForCompletion();

            //ac walks to animal
            if (_acPed) _acPed.Tasks.FollowNavigationMeshToPosition(_animal.Position, _animal.Heading + 180f, 10f, -1).WaitForCompletion();

            //deletes the dead animal when animal control is right next to the animal.
            if (_animal)
            {
                _animal.Delete();
                _animal = null;
            }

            //Tells the animal control to get back into the truck
            if (_acPed) _acPed.Tasks.GoStraightToPosition(_acVehicle.Position.Around2D(2f, 4f), 10f, _acVehicle.Heading + 180f, 0f, -1).WaitForCompletion();
            if (_acPed) _acPed.Tasks.EnterVehicle(_acVehicle, -1).WaitForCompletion();

            //Tells the animal control to drive off
            if (_acPed) _acPed.Tasks.CruiseWithVehicle(10f, VehicleDrivingFlags.Normal);
            
            //Dismisses everything
            if (_acPed) DestroyAnimalControl();
        }

        internal static void DestroyAnimalControl()
        {
            if (_acPed) _acPed.Dismiss();
            if (_acVehicle) _acVehicle.Dismiss();
            if (_acBlip) _acBlip.Delete();
            _fiber.Abort();
        }
    }
}
