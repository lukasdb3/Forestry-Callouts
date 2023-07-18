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

namespace ForestryCallouts2.Backbone.Functions;

internal static class GrabPed
{
    internal static GameFiber Fiber;
    private static List<Ped> _cPeds;
    private static Ped _cPed;
    private static bool _pedAttached;
    internal static void Main()
    {
        Fiber = GameFiber.StartNew(delegate
        {
            while (true)
            {
                GameFiber.Yield();
                if (Game.IsKeyDown(Keys.T) && Game.LocalPlayer.Character.IsOnFoot && !_pedAttached)
                {
                    _cPeds = CFunctions.GetValidPedsNearby(10);
                    if (_cPeds == null) return;
                    
                    _cPed = _cPeds.FirstOrDefault();
                    if (_cPed.DistanceTo(Game.LocalPlayer.Character) > 2f) return;
                    Logger.DebugLog("GrabPed", "Model Grabbing is " + _cPed.Model.Name);
                
                    _cPed.AttachTo(Game.LocalPlayer.Character, 57005, Vector3.RelativeFront, new Rotator(0,0,90));
                    _cPed.IsPersistent = true;
                    _cPed.BlockPermanentEvents = true;
                    _cPed.Tasks.StandStill(-1);
                    _pedAttached = true;
                    GameFiber.Sleep(1000);
                }
                if (Game.IsKeyDown(Keys.T) && _pedAttached)
                {
                    _cPed.Detach();
                    _cPed.Tasks.Clear();
                    _cPed.Position = _cPed.Position;
                    _cPed.Dismiss();
                    _pedAttached = false;
                }
            }
        });
    }
    
}