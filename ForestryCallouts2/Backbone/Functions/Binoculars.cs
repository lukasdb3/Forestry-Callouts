#region Refrences
//Rage
using System.Windows.Forms;
using Rage;
using Rage.Native;
//ForestryCallouts2
using ForestryCallouts2.Backbone.IniConfiguration;

#endregion

namespace ForestryCallouts2.Backbone.Functions
{
    internal class Binoculars
    {
        private static Texture _binoTexture = Game.CreateTextureFromFile(@"plugins\lspdfr\ForestryCallouts2\Binoculars.png");
        internal static bool IsRendering = false;
        internal static bool BinoKeyEnabled = true;
        private static Object _binoculars;
        private static Camera _binoCamera;

        internal static void Enable()
        {
            GameFiber.StartNew(delegate
            {
                BinoKeyEnabled = false;
                
                // Create binoculars and attach to players right hand and play binocular animation
                _binoculars = new Rage.Object("prop_binoc_01", Game.LocalPlayer.Character.Position);
                _binoculars.AttachTo(Game.LocalPlayer.Character, Game.LocalPlayer.Character.GetBoneIndex(PedBoneId.RightPhHand), Vector3.Zero, Rotator.Zero);
                Game.LocalPlayer.Character.Tasks.PlayAnimation("amb@world_human_binoculars@male@base", "base", 10f, AnimationFlags.UpperBodyOnly | AnimationFlags.Loop);

                GameFiber.Sleep(800);

                // Create new camera and position and attach at front of binoculars
                _binoCamera = new Camera(true);
                _binoCamera.FOV = 30;
                _binoCamera.AttachToEntity(_binoculars, new Vector3(0.0f, -0.1f, 0.0f), true);
                _binoCamera.Rotation = Game.LocalPlayer.Character.Rotation;
                // Start texture rendering
                Game.RawFrameRender += RawFrameRender;
                IsRendering = true;

                while (true)
                {
                    GameFiber.Yield();
                    NativeFunction.Natives.DISABLE_CONTROL_ACTION(0, 14, true);
                    NativeFunction.Natives.DISABLE_CONTROL_ACTION(0, 15, true);
                    NativeFunction.Natives.DISABLE_CONTROL_ACTION(0, 13, true);
                    NativeFunction.Natives.DISABLE_CONTROL_ACTION(0, 12, true);
                    NativeFunction.Natives.DISABLE_CONTROL_ACTION(0, 16, true);
                    NativeFunction.Natives.DISABLE_CONTROL_ACTION(0, 17, true);
                    NativeFunction.Natives.DISABLE_CONTROL_ACTION(0, 27, true);

                    //Moving left and right
                    float mouseSense = (_binoCamera.FOV / 70) * (float)IniSettings.BinocularsSensitivity;

                    float upDown = NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(0, (int)GameControl.LookUpDown) * mouseSense;
                    float leftRight = NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(0, (int)GameControl.LookLeftRight) * mouseSense;

                    _binoCamera.Rotation = new Rotator(_binoCamera.Rotation.Pitch - upDown, _binoCamera.Rotation.Roll, _binoCamera.Rotation.Yaw - leftRight);
                    
                    //Zooming in and out
                    if (CFunctions.IsKeyAndModifierDown(IniSettings.BinocularsZoom, IniSettings.BinocularsZoomModifier))
                    {
                        _binoCamera.FOV -= 10;
                        if (_binoCamera.FOV <= 15)
                        {
                            _binoCamera.FOV = 30;
                        }
                    }
                    
                    if (CFunctions.IsKeyAndModifierDown(IniSettings.BinocularsKey, IniSettings.BinocularsKeyModifier))
                    {
                        _binoCamera.Delete();
                        _binoculars.Delete();
                        Game.RawFrameRender -= RawFrameRender;
                        Game.LocalPlayer.Character.Tasks.Clear();
                        NativeFunction.Natives.ENABLE_CONTROL_ACTION(0, 14, true);
                        NativeFunction.Natives.ENABLE_CONTROL_ACTION(0, 15, true);
                        NativeFunction.Natives.ENABLE_CONTROL_ACTION(0, 13, true);
                        NativeFunction.Natives.ENABLE_CONTROL_ACTION(0, 12, true);
                        NativeFunction.Natives.ENABLE_CONTROL_ACTION(0, 16, true);
                        NativeFunction.Natives.ENABLE_CONTROL_ACTION(0, 17, true);
                        NativeFunction.Natives.ENABLE_CONTROL_ACTION(0, 27, true);
                        IsRendering = false;
                        GameFiber.Wait(2000);
                        BinoKeyEnabled = true;
                        break;
                    }
                }
            });
        }

        private static void RawFrameRender(object sender, GraphicsEventArgs eventArgs)
        {
            eventArgs.Graphics.DrawTexture(_binoTexture, 0f, 0f, Game.Resolution.Width, Game.Resolution.Height);
        }
    }
}