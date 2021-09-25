using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response;
//using StopThePed.API;
using LSPD_First_Response.Engine;
using Rage.Native;
using LSPD_First_Response.Engine.Scripting.Entities;
using RAGENativeUI.Elements;

namespace ForestryCallouts.SimpleFunctions
{
    class SPFunctions
    {
        internal static void AnimalAttatckSpawnChooser(out Vector3 vcSpawnpoint, out Vector3 AnimalSpawnpoint, out float animalHeading)
        {
            vcSpawnpoint = default;
            AnimalSpawnpoint = default;
            animalHeading = default;
            int AnimalAttackSpawnpoints = new Random().Next(1, 26);
            Game.LogTrivial("-!!- Forestry Callouts - |SpawnChooser| - AnimalAttackSpawnpoints, chose: Blondee Swag " + AnimalAttackSpawnpoints + " -!!-");
            switch (AnimalAttackSpawnpoints)
            {
                case 1:
                    vcSpawnpoint = new Vector3(-567.111f, 5447.534f, 61.143f);
                    AnimalSpawnpoint = new Vector3(-518.384f, 5429.047f, 65.459f);
                    animalHeading = 267.390f;
                    break;
                case 2:
                    vcSpawnpoint = new Vector3(-457.368f, 5484.195f, 81.604f);
                    AnimalSpawnpoint = new Vector3(-441.162f, 5532.486f, 71.767f);
                    animalHeading = 340.306f;
                    break;
                case 3:
                    vcSpawnpoint = new Vector3(-490.195f, 5648.356f, 59.390f);
                    AnimalSpawnpoint = new Vector3(-454.088f, 5670.512f, 69.579f);
                    animalHeading = 347.468f;
                    break;
                case 4:
                    vcSpawnpoint = new Vector3(-665.680f, 5688.250f, 30.047f);
                    AnimalSpawnpoint = new Vector3(-662.406f, 5720.319f, 22.795f);
                    animalHeading = 54.489f;
                    break;
                case 5:
                    vcSpawnpoint = new Vector3(-638.715f, 5779.375f, 25.035f);
                    AnimalSpawnpoint = new Vector3(-584.440f, 5820.559f, 32.129f);
                    animalHeading = 325.804f;
                    break;
                case 6:
                    vcSpawnpoint = new Vector3(-577.052f, 5880.760f, 28.331f);
                    AnimalSpawnpoint = new Vector3(-560.505f, 5928.332f, 28.360f);
                    animalHeading = 276.630f;
                    break;
                case 7:
                    vcSpawnpoint = new Vector3(-567.153f, 5992.463f, 30.510f);
                    AnimalSpawnpoint = new Vector3(-542.998f, 5968.120f, 35.629f);
                    animalHeading = 217.626f;
                    break;
                case 8:
                    vcSpawnpoint = new Vector3(-701.188f, 5331.767f, 70.059f);
                    AnimalSpawnpoint = new Vector3(-742.611f, 5303.106f, 75.051f);
                    animalHeading = 152.749f;
                    break;
                case 9:
                    vcSpawnpoint = new Vector3(-796.889f, 5290.490f, 85.556f);
                    AnimalSpawnpoint = new Vector3(-817.716f, 5250.218f, 88.147f);
                    animalHeading = 165.558f;
                    break;
                case 10:
                    vcSpawnpoint = new Vector3(-765.330f, 5214.119f, 107.005f);
                    AnimalSpawnpoint = new Vector3(-795.488f, 5193.372f, 115.347f);
                    animalHeading = 122.515f;
                    break;
                case 11:
                    vcSpawnpoint = new Vector3(-822.654f, 5172.811f, 112.741f);
                    AnimalSpawnpoint = new Vector3(-795.488f, 5193.372f, 115.347f);
                    animalHeading = 309.392f;
                    break;
                case 12:
                    vcSpawnpoint = new Vector3(-881.795f, 5198.420f, 113.908f);
                    AnimalSpawnpoint = new Vector3(-869.385f, 5217.189f, 105.023f);
                    animalHeading = 308.307f;
                    break;
                case 13:
                    vcSpawnpoint = new Vector3(-1070.794f, 5055.690f, 168.529f);
                    AnimalSpawnpoint = new Vector3(-1164.136f, 5028.375f, 156.731f);
                    animalHeading = 118.187f;
                    break;
                case 14:
                    vcSpawnpoint = new Vector3(-1289.283f, 4944.125f, 151.509f);
                    AnimalSpawnpoint = new Vector3(-1306.172f, 4849.485f, 144.496f);
                    animalHeading = 233.440f;
                    break;
                case 15:
                    vcSpawnpoint = new Vector3(-994.371f, 4692.742f, 250.073f);
                    AnimalSpawnpoint = new Vector3(-942.748f, 4729.925f, 279.312f);
                    animalHeading = 324.450f;
                    break;
                case 16:
                    vcSpawnpoint = new Vector3(-758.998f, 4801.037f, 230.666f);
                    AnimalSpawnpoint = new Vector3(-756.633f, 4736.614f, 229.726f);
                    animalHeading = 159.947f;
                    break;
                case 17:
                    vcSpawnpoint = new Vector3(-585.880f, 4754.321f, 211.736f);
                    AnimalSpawnpoint = new Vector3(-500.435f, 4730.775f, 241.073f);
                    animalHeading = 271.463f;
                    break;
                case 18:
                    vcSpawnpoint = new Vector3(-309.330f, 4719.706f, 233.401f);
                    AnimalSpawnpoint = new Vector3(-331.838f, 4803.100f, 219.276f);
                    animalHeading = 15.140f;
                    break;
                case 19:
                    vcSpawnpoint = new Vector3(-284.526f, 4777.419f, 205.651f);
                    AnimalSpawnpoint = new Vector3(-235.701f, 4708.486f, 194.913f);
                    animalHeading = 222.668f;
                    break;
                case 20:
                    vcSpawnpoint = new Vector3(-364.665f, 4966.926f, 201.266f);
                    AnimalSpawnpoint = new Vector3(-311.475f, 4975.517f, 239.294f);
                    animalHeading = 238.451f;
                    break;
                case 21:
                    vcSpawnpoint = new Vector3(-443.920f, 4905.924f, 175.390f);
                    AnimalSpawnpoint = new Vector3(-434.804f, 4960.584f, 165.843f);
                    animalHeading = 355.097f;
                    break;
                case 22:
                    vcSpawnpoint = new Vector3(-602.102f, 5116.040f, 120.919f);
                    AnimalSpawnpoint = new Vector3(-573.488f, 5151.018f, 105.035f);
                    animalHeading = 279.132f;
                    break;
                case 23:
                    vcSpawnpoint = new Vector3(-635.521f, 5210.356f, 82.484f);
                    AnimalSpawnpoint = new Vector3(-659.282f, 5282.166f, 72.254f);
                    animalHeading = 13.410f;
                    break;
                case 24:
                    vcSpawnpoint = new Vector3(-712.457f, 5475.591f, 43.592f);
                    AnimalSpawnpoint = new Vector3(-654.298f, 5501.904f, 48.703f);
                    animalHeading = 265.098f;
                    break;
                case 25:
                    vcSpawnpoint = new Vector3(-484.277f, 5593.648f, 68.015f);
                    AnimalSpawnpoint = new Vector3(-508.956f, 5590.003f, 67.115f);
                    animalHeading = 149.278f;
                    break;
            }
        }
        internal static void MissingHikkerSpawnChooser(out Vector3 pedSpawnpoint, out float pedHeading)
        {
            pedSpawnpoint = default;
            pedHeading = default;
            int MissingHikerSpawnpoints = new Random().Next(1, 26);
            Game.LogTrivial("-!!- Forestry Callouts - |MissingHikerSpawnpoints| - MissingHikerSpawnpoints, chose: Blondee Swag " + MissingHikerSpawnpoints + " -!!-");
            switch (MissingHikerSpawnpoints)
            {
                case 1:
                    pedSpawnpoint = new Vector3(-468.902f, 5627.561f, 61.830f);
                    pedHeading = 14.242f;
                    break;
                case 2:
                    pedSpawnpoint = new Vector3(-691.609f, 5024.531f, 161.335f);
                    pedHeading = 188.977f;
                    break;
                case 3:
                    pedSpawnpoint = new Vector3(-747.114f, 4799.032f, 228.743f);
                    pedHeading = 42.241f;
                    break;
                case 4:
                    pedSpawnpoint = new Vector3(-407.597f, 4689.428f, 258.755f);
                    pedHeading = 249.880f;
                    break;
                case 5:
                    pedSpawnpoint = new Vector3(-1066.095f, 4567.201f, 102.332f);
                    pedHeading = 153.474f;
                    break;
                case 6:
                    pedSpawnpoint = new Vector3(-1550.187f, 4443.572f, 11.483f);
                    pedHeading = 177.148f;
                    break;
                case 7:
                    pedSpawnpoint = new Vector3(-1635.124f, 4586.562f, 42.745f);
                    pedHeading = 210.257f;
                    break;
                case 8:
                    pedSpawnpoint = new Vector3(-1687.091f, 4578.473f, 40.371f);
                    pedHeading = 247.109f;
                    break;
                case 9:
                    pedSpawnpoint = new Vector3(-927.830f, 4818.380f, 308.795f);
                    pedHeading = 4.725f;
                    break;
                case 10:
                    pedSpawnpoint = new Vector3(-718.586f, 4825.982f, 212.680f);
                    pedHeading = 305.137f;
                    break;
                case 11:
                    pedSpawnpoint = new Vector3(-406.110f, 4711.855f, 261.385f);
                    pedHeading = 254.038f;
                    break;
                case 12:
                    pedSpawnpoint = new Vector3(-273.296f, 4773.916f, 208.815f);
                    pedHeading = 196.880f;
                    break;
                case 13:
                    pedSpawnpoint = new Vector3(-480.776f, 5035.054f, 149.912f);
                    pedHeading = 35.982f;
                    break;
                case 14:
                    pedSpawnpoint = new Vector3(-720.985f, 4559.380f, 81.592f);
                    pedHeading = 288.597f;
                    break;
                case 15:
                    pedSpawnpoint = new Vector3(-447.787f, 4538.074f, 97.061f);
                    pedHeading = 268.466f;
                    break;
                case 16:
                    pedSpawnpoint = new Vector3(-206.459f, 443.791f, 48.543f);
                    pedHeading = 24.434f;
                    break;
                case 17:
                    pedSpawnpoint = new Vector3(-111.268f, 4387.314f, 71.611f);
                    pedHeading = 159.115f;
                    break;
                case 18:
                    pedSpawnpoint = new Vector3(-1211.696f, 4632.935f, 134.984f);
                    pedHeading = 128.064f;
                    break;
                case 19:
                    pedSpawnpoint = new Vector3(-1598.069f, 4516.003f, 16.443f);
                    pedHeading = 125.930f;
                    break;
                case 20:
                    pedSpawnpoint = new Vector3(-1398.089f, 4557.721f, 64.041f);
                    pedHeading = 85.919f;
                    break;
                case 21:
                    pedSpawnpoint = new Vector3(-1494.520f, 4575.935f, 35.485f);
                    pedHeading = 36.259f;
                    break;
                case 22:
                    pedSpawnpoint = new Vector3(-1590.400f, 4199.873f, 80.974f);
                    pedHeading = 273.549f;
                    break;
                case 23:
                    pedSpawnpoint = new Vector3(-720.405f, 4159.882f, 161.797f);
                    pedHeading = 75.459f;
                    break;
                case 24:
                    pedSpawnpoint = new Vector3(-596.675f, 4152.889f, 182.835f);
                    pedHeading = 316.553f;
                    break;
                case 25:
                    pedSpawnpoint = new Vector3(-533.834f, 3957.935f, 93.913f);
                    pedHeading = 289.911f;
                    break;
            }
        }
        internal static void VehicleOnFireSpawnChooser(out Vector3 position, out float heading, out Vector3 pedPosition, out float pedHeading)
        {
            position = default;
            heading = default;
            pedPosition = default;
            pedHeading = default;
            int VehicleOnFireSpawnpoints = new Random().Next(1, 11);
            Game.LogTrivial("-!!- Forestry Callouts - |VehicleOnFireSpawnpoints| - VehicleOnFireSpawnpoints, chose: Blondee Swag " + VehicleOnFireSpawnpoints + " -!!-");
            switch (VehicleOnFireSpawnpoints)
            {
                case 1:
                    position = new Vector3(-1652.839f, 4216.450f, 83.196f);
                    heading = 49.770f;
                    pedPosition = new Vector3(-1540.745f, 4213.843f, 70.734f);
                    pedHeading = 282.515f;
                    break;
                case 2:
                    position = new Vector3(-1127.778f, 4292.969f, 87.711f);
                    heading = 269.257f;
                    pedPosition = new Vector3(-1040.344f, 4225.405f, 116.898f);
                    pedHeading = 183.641f;
                    break;
                case 3:
                    position = new Vector3(-862.546f, 4090.812f, 163.944f);
                    heading = 236.541f;
                    pedPosition = new Vector3(-665.513f, 4111.928f, 156.943f);
                    pedHeading = 5.697f;
                    break;
                case 4:
                    position = new Vector3(-508.968f, 4352.713f, 67.216f);
                    heading = 69.069f;
                    pedPosition = new Vector3(-390.041f, 4294.538f, 52.221f);
                    pedHeading = 247.853f;
                    break;
                case 5:
                    position = new Vector3(-1350.642f, 4247.955f, 7.497f);
                    heading = 164.894f;
                    pedPosition = new Vector3(-1484.356f, 4300.162f, 3.859f);
                    pedHeading = 83.548f;
                    break;
                case 6:
                    position = new Vector3(-1850.799f, 4508.628f, 21.269f);
                    heading = 88.756f;
                    pedPosition = new Vector3(-1863.379f, 4412.466f, 48.534f);
                    pedHeading = 240.594f;
                    break;
                case 7:
                    position = new Vector3(-678.627f, 5797.420f, 16.849f);
                    heading = 67.437f;
                    pedPosition = new Vector3(-592.053f, 5928.780f, 25.596f);
                    pedHeading = 337.168f;
                    break;
                case 8:
                    position = new Vector3(-434.483f, 5628.147f, 61.008f);
                    heading = 273.372f;
                    pedPosition = new Vector3(-439.142f, 5830.188f, 46.503f);
                    pedHeading = 346.193f;
                    break;
                case 9:
                    position = new Vector3(-469.735f, 5513.066f, 79.437f);
                    heading = 182.460f;
                    pedPosition = new Vector3(-569.349f, 5561.751f, 50.663f);
                    pedHeading = 339.527f;
                    break;
                case 10:
                    position = new Vector3(-874.269f, 5302.925f, 78.749f);
                    heading = 110.438f;
                    pedPosition = new Vector3(-807.691f, 5260.183f, 87.749f);
                    pedHeading = 266.295f;
                    break;

            }
        }
        internal static void LoggerPursuitSpawnChooser(out Vector3 position, out float heading)
        {
            position = default;
            heading = default;
            int LoggerPursuitSpawnpoints = new Random().Next(1, 21);
            int HeadingChoice = new Random().Next(1, 3);
            Game.LogTrivial("-!!- Forestry Callouts - |LoggerPursuitSpawnChooser| - LoggerPursuitSpawnChooser, chose: Blondee Swag " + LoggerPursuitSpawnpoints + " -!!-");
            switch (LoggerPursuitSpawnpoints)
            {
                case 1:
                    position = new Vector3(-1585.681f, 4781.532f, 50.652f);
                    heading = 182.838f;
                    break;

                case 2:
                    position = new Vector3(-1505.608f, 4469.358f, 17.325f);
                    heading = 276.752f;
                    break;
                case 3:
                    position = new Vector3(-1175.160f, 4468.653f, 22.084f);
                    heading = 44.582f;
                    break;

                case 4:
                    position = new Vector3(-781.132f, 4403.589f, 17.421f);
                    heading = 80.320f;
                    break;
                case 5:
                    position = new Vector3(-507.160f, 4364.442f, 67.010f);
                    heading = 78.212f;
                    break;

                case 6:
                    position = new Vector3(-1176.189f, 4362.688f, 6.936f);
                    heading = 86.330f;
                    break;
                case 7:
                    position = new Vector3(-1676.451f, 4454.439f, 1.816f);
                    heading = 256.970f;
                    break;

                case 8:
                    position = new Vector3(-1901.271f, 4483.411f, 27.860f);
                    if (HeadingChoice == 1)
                    {
                        heading = 80.617f;
                    }
                    if (HeadingChoice == 2)
                    {
                        heading = 263.150f;
                    }
                    break;
                case 9:
                    position = new Vector3(-1856.104f, 4406.780f, 49.260f);
                    heading = 238.347f;
                    break;

                case 10:
                    position = new Vector3(-1614.466f, 4200.498f, 82.865f);
                    if (HeadingChoice == 1)
                    {
                        heading = 264.880f;
                    }
                    if (HeadingChoice == 2)
                    {
                        heading = 85.905f;
                    }
                    break;
                case 11:
                    position = new Vector3(-1308.407f, 4205.630f, 60.535f);
                    heading = 343.135f;
                    break;
                case 12:
                    position = new Vector3(-859.083f, 4077.364f, 163.953f);
                    heading = 44.836f;
                    break;
                case 13:
                    position = new Vector3(-815.586f, 5418.194f, 33.557f);
                    heading = 290.353f;
                    break;
                case 14:
                    position = new Vector3(-569.487f, 5352.833f, 69.776f);
                    if (HeadingChoice == 1)
                    {
                        heading = 340.273f;
                    }
                    if (HeadingChoice == 2)
                    {
                        heading = 160.094f;
                    }
                    break;
                case 15:
                    position = new Vector3(-459.666f, 5497.891f, 80.571f);
                    heading = 13.234f;
                    break;
                case 16:
                    position = new Vector3(-449.538f, 5892.518f, 32.510f);
                    heading = 323.100f;
                    break;
                case 17:
                    position = new Vector3(-292.154f, 6049.609f, 31.018f);
                    heading = 345.993f;
                    break;
                case 18:
                    position = new Vector3(-561.420f, 5661.979f, 37.667f);
                    if (HeadingChoice == 1)
                    {
                        heading = 331.756f;
                    }
                    if (HeadingChoice == 2)
                    {
                        heading = 158.845f;
                    }
                    break;
                case 19:
                    position = new Vector3(-763.426f, 5324.632f, 73.990f);
                    heading = 85.275f;                                                                                                                  
                    break;
                case 20:
                    position = new Vector3(-715.050f, 5166.300f, 112.194f);
                    heading = 67.469f;
                    break;
            }
        }
        internal static void SuspiciousVehicleSpawnChooser(out Vector3 position, out float heading)
        {
            position = default;
            heading = default;
            int SuspiciousSpawnpoints = new Random().Next(1, 28);
            Game.LogTrivial("-!!- Forestry Callouts - |SpawnChooser| - SuspiciousVehicleSpawnpoints, chose: Blondee Swag " + SuspiciousSpawnpoints + " -!!-");
            switch (SuspiciousSpawnpoints)
            {
                case 1:
                    position = new Vector3(-681.706f, 5491.221f, 46.837f);
                    heading = 115.373f;
                    break;
                case 2:
                    position = new Vector3(-790.660f, 5482.447f, 26.118f);
                    heading = 22.069f;
                    break;
                case 3:
                    position = new Vector3(-834.857f, 5571.851f, 31.171f);
                    heading = 11.604f;
                    break;
                case 4:
                    position = new Vector3(-661.217f, 5719.463f, 22.181f);
                    heading = 146.285f;
                    break;
                case 5:
                    position = new Vector3(-706.653f, 5612.974f, 28.733f);
                    heading = 9.405f;
                    break;
                case 6:
                    position = new Vector3(-639.962f, 5779.543f, 24.306f);
                    heading = 335.285f;
                    break;
                case 7:
                    position = new Vector3(-565.855f, 5888.031f, 30.518f);
                    heading = 215.653f;
                    break;
                case 8:
                    position = new Vector3(-565.855f, 5888.031f, 30.518f);
                    heading = 215.653f;
                    break;
                case 9:
                    position = new Vector3(-333.329f, 5955.506f, 40.768f);
                    heading = 138.844f;
                    break;
                case 10:
                    position = new Vector3(-441.483f, 5765.502f, 57.266f);
                    heading = 34.406f;
                    break;
                case 11:
                    position = new Vector3(-492.627f, 5598.470f, 66.793f);
                    heading = 285.000f;
                    break;
                case 12:
                    position = new Vector3(-471.475f, 5512.052f, 79.451f);
                    heading = 181.145f;
                    break;
                case 13:
                    position = new Vector3(-605.771f, 5532.345f, 47.566f);
                    heading = 248.031f;
                    break;
                case 14:
                    position = new Vector3(-690.958f, 5340.576f, 68.106f);
                    heading = 329.748f;
                    break;
                case 15:
                    position = new Vector3(-735.405f, 5169.661f, 109.592f);
                    heading = 222.169f;
                    break;
                case 16:
                    position = new Vector3(-756.054f, 5077.228f, 140.402f);
                    heading = 339.248f;
                    break;
                case 17:
                    position = new Vector3(-915.858f, 5195.608f, 118.637f);
                    heading = 63.604f;
                    break;
                case 18:
                    position = new Vector3(-1053.939f, 5105.398f, 151.432f);
                    heading = 159.710f;
                    break;
                case 19:
                    position = new Vector3(-1351.749f, 4840.559f, 137.382f);
                    heading = 145.201f;
                    break;
                case 20:
                    position = new Vector3(-1621.123f, 4723.268f, 51.474f);
                    heading = 357.577f;
                    break;
                case 21:
                    position = new Vector3(-1161.131f, 4772.209f, 50.114f);
                    heading = 148.168f;
                    break;
                case 22:
                    position = new Vector3(-1627.163f, 4669.848f, 33.644f);
                    heading = 133.089f;
                    break;
                case 23:
                    position = new Vector3(-1855.040f, 4726.583f, 1.543f);
                    heading = 9.945f;
                    break;
                case 24:
                    position = new Vector3(-1824.979f, 4811.894f, 4.108f);
                    heading = 115.115f;
                    break;
                case 25:
                    position = new Vector3(-1584f, 4512.711f, 18.936f);
                    heading = 226.392f;
                    break;
                case 26:
                    position = new Vector3(-1311.167f, 4476.164f, 20.899f);
                    heading = 349.437f;
                    break;
                case 27:
                    position = new Vector3(-1192.818f, 4439.255f, 30.861f);
                    heading = 291.698f;
                    break;
            }
        }
        internal static void IllegalCampingSpawnChooser(out Vector3 position, out float Heading)
        {
            position = default;
            Heading = default;
            int IllegalCampingSpawnpoints = new Random().Next(1, 21);
            Game.LogTrivial("-!!- Forestry Callouts - |SpawnChooser| - IllegalCampingSpawnpoints, chose: Blondee Swag " + IllegalCampingSpawnpoints + " -!!-");
            switch (IllegalCampingSpawnpoints)
            {
                case 1:
                    position = new Vector3(-1827.575f, 4820.502f, 4.082f);
                    Heading = 357.761f;
                    break;
                case 2:
                    position = new Vector3(-1838.445f, 4710.840f, 2.050f);
                    Heading = 92.057f;
                    break;
                case 3:
                    position = new Vector3(-1641.969f, 4729.582f, 53.182f);
                    Heading = 159.502f;
                    break;
                case 4:
                    position = new Vector3(-1654.529f, 4543.524f, 40.622f);
                    Heading = 65.721f;
                    break;
                case 5:
                    position = new Vector3(-1695.464f, 4599.015f, 46.932f);
                    Heading = 64.883f;
                    break;
                case 6:
                    position = new Vector3(-1572.222f, 4506.617f, 20.333f);
                    Heading = 236.064f;
                    break;
                case 7:
                    position = new Vector3(-1226.491f, 4454.838f, 29.618f);
                    Heading = 195.899f;
                    break;
                case 8:
                    position = new Vector3(-944.107f, 4413.721f, 17.498f);
                    Heading = 257.919f;
                    break;
                case 9:
                    position = new Vector3(-888.614f, 4432.248f, 20.578f);
                    Heading = 242.006f;
                    break;
                case 10:
                    position = new Vector3(-1084.133f, 4350.601f, 14.081f);
                    Heading = 16.762f;
                    break;
                case 11:
                    position = new Vector3(-1460.234f, 4295.016f, 2.486f);
                    Heading = 87.731f;
                    break;
                case 12:
                    position = new Vector3(-1509.148f, 4318.007f, 4.712f);
                    Heading = 106.732f;
                    break;
                case 13:
                    position = new Vector3(-1924.853f, 4483.565f, 29.857f);
                    Heading = 209.834f;
                    break;
                case 14:
                    position = new Vector3(-1962.436f, 4442.237f, 35.326f);
                    Heading = 275.683f;
                    break;
                case 15:
                    position = new Vector3(-1851.352f, 4424.243f, 48.374f);
                    Heading = 290.289f;
                    break;
                case 16:
                    position = new Vector3(-1429.187f, 4232.035f, 46.683f);
                    Heading = 253.244f;
                    break;
                case 17:
                    position = new Vector3(-1215.841f, 4300.017f, 75.359f);
                    Heading = 271.983f;
                    break;
                case 18:
                    position = new Vector3(-930.765f, 5281.491f, 80.444f);
                    Heading = 311.667f;
                    break;
                case 19:
                    position = new Vector3(-607.948f, 5509.610f, 49.591f);
                    Heading = 305.001f;
                    break;
                case 20:
                    position = new Vector3(-791.533f, 5483.531f, 26.022f);
                    Heading = 32.763f;
                    break;
            }
        }

        internal static void IllegalFishingSpawnChooser(out Vector3 spawnpoint, out float susHeading)
        {
            spawnpoint = default;
            susHeading = default;
            int IllegalFishingSpawnpoints = new Random().Next(1, 18);
            Game.LogTrivial("-!!- Forestry Callouts - |SpawnChooser| - IllegalFishingSpawnChooser, chose: Blondee Swag " + IllegalFishingSpawnpoints + " -!!-");
            switch (IllegalFishingSpawnpoints)
            {
                case 1:
                    spawnpoint = new Vector3(-1706.727f, 4464.035f, 1.772f);
                    susHeading = 0.297f;
                    break;
                case 2:
                    spawnpoint = new Vector3(-1758.408f, 4487.292f, 1.153f);
                    susHeading = 1.036f;
                    break;
                case 3:
                    spawnpoint = new Vector3(-1669.564f, 4464.863f, 1.079f);
                    susHeading = 349.038f;
                    break;
                case 4:
                    spawnpoint = new Vector3(-1630.792f, 4433.671f, 1.314f);
                    susHeading = 293.175f;
                    break;
                case 5:
                    spawnpoint = new Vector3(-1606.591f, 4387.150f, 1.122f);
                    susHeading = 284.539f;
                    break;
                case 6:
                    spawnpoint = new Vector3(-1581.806f, 4354.148f, 1.067f);
                    susHeading = 305.990f;
                    break;
                case 7:
                    spawnpoint = new Vector3(-1532.227f, 4342.702f, 1.536f);
                    susHeading = 43.492f;
                    break;
                case 8:
                    spawnpoint = new Vector3(-1479.571f, 4330.337f, 2.031f);
                    susHeading = 344.180f;
                    break;
                case 9:
                    spawnpoint = new Vector3(-1424.716f, 4318.917f, 1.156f);
                    susHeading = 343.885f;
                    break;
                case 10:
                    spawnpoint = new Vector3(-1331.230f, 4336.470f, 7.651f);
                    susHeading = 19.533f;
                    break;
                case 11:
                    spawnpoint = new Vector3(-1287.176f, 4355.337f, 6.708f);
                    susHeading = 9.867f;
                    break;
                case 12:
                    spawnpoint = new Vector3(-1229.365f, 4375.995f, 5.957f);
                    susHeading = 13.902f;
                    break;
                case 13:
                    spawnpoint = new Vector3(-1170.375f, 4374.113f, 5.691f);
                    susHeading = 335.879f;
                    break;
                case 14:
                    spawnpoint = new Vector3(-1097.723f, 4391.721f, 11.083f);
                    susHeading = 345.729f;
                    break;
                case 15:
                    spawnpoint = new Vector3(-1065.355f, 4384.868f, 11.100f);
                    susHeading = 316.173f;
                    break;
                case 16:
                    spawnpoint = new Vector3(-1847.407f, 4708.089f, 1117.484f);
                    susHeading = 117.484f;
                    break;
                case 17:
                    spawnpoint = new Vector3(-1865.814f, 4815.784f, 1.814f);
                    susHeading = 29.358f;
                    break;
            }
        }
        internal static void RangerBackupSpawnChooser(out Vector3 copPosition, out float copHeading, out Vector3 susPosition, out float susHeading)
        {
            copPosition = default;
            copHeading = default;
            susPosition = default;
            susHeading = default;
            int RangerBackupSpawnChooser = new Random().Next(1, 11);
            Game.LogTrivial("-!!- Forestry Callouts - |SpawnChooser| - RangerBackupSpawnChooser, chose: Blondee Swag " + RangerBackupSpawnChooser + " -!!-");
            switch (RangerBackupSpawnChooser)
            {
                case 1:
                    copPosition = new Vector3(-1841.731f, 4502.646f, 21.558f);
                    copHeading = 69.219f;

                    susPosition = new Vector3(-1851.055f, 4504.458f, 22.065f);
                    susHeading = 88.110f;
                    break;
                case 2:
                    copPosition = new Vector3(-1074.663f, 4425.528f, 18.722f);
                    copHeading = 98.741f;

                    susPosition = new Vector3(-1083.506f, 4424.677f, 16.749f);
                    susHeading = 101.612f;
                    break;
                case 3:
                    copPosition = new Vector3(-1566.866f, 4583.466f, 18.813f);
                    copHeading = 168.562f;

                    susPosition = new Vector3(-1568.748f, 4573.490f, 18.244f);
                    susHeading = 178.709f;
                    break;
                case 4:
                    copPosition = new Vector3(-1585.964f, 4848.119f, 58.650f);
                    copHeading = 158.862f;

                    susPosition = new Vector3(-1589.531f, 4827.646f, 57.095f);
                    susHeading = 169.219f;
                    break;
                case 5:
                    copPosition = new Vector3(-722.989f, 5434.609f, 42.758f);
                    copHeading = 253.795f;

                    susPosition = new Vector3(-711.307f, 5430.563f, 44.415f);
                    susHeading = 245.220f;
                    break;
                case 6:
                    copPosition = new Vector3(-572.333f, 5444.141f, 60.563f);
                    copHeading = 231.679f;

                    susPosition = new Vector3(-563.386f, 5435.981f, 61.182f);
                    susHeading = 216.487f;
                    break;
                case 7:
                    copPosition = new Vector3(-524.693f, 5614.503f, 55.881f);
                    copHeading = 327.370f;

                    susPosition = new Vector3(-517.957f, 5624.265f, 55.969f);
                    susHeading = 316.810f;
                    break;
                case 8:
                    copPosition = new Vector3(-1691.650f, 4272.734f, 73.848f);
                    copHeading = 196.286f;

                    susPosition = new Vector3(-1688.245f, 4261.460f, 75.766f);
                    susHeading = 192.585f;
                    break;
                case 9:
                    copPosition = new Vector3(-1049.341f, 4197.003f, 118.839f);
                    copHeading = 177.553f;

                    susPosition = new Vector3(-1040.359f, 4185.345f, 119.378f);
                    susHeading = 185.489f;
                    break;
                case 10:
                    copPosition = new Vector3(-546.513f, 4358.779f, 64.064f);
                    copHeading = 93.483f;

                    susPosition = new Vector3(-560.283f, 4358.543f, 61f);
                    susHeading = 95.428f;
                    break;
            }
        }
        internal static void DeadAnimalBlockingRoadway(out Vector3 position)
        {
            position = default;
            int animalBlockingRoadway = new Random().Next(1, 16);
            Game.LogTrivial("-!!- Forestry Callouts - |SpawnChooser| - DeadAnimalBlockingRoadway, chose: Blondee Swag " + animalBlockingRoadway + " -!!-");
            switch (animalBlockingRoadway)
            {
                case 1:
                    position = new Vector3(-561.399f, 5439.361f, 61.131f);
                    break;
                case 2:
                    position = new Vector3(-561.399f, 5439.361f, 61.131f);
                    break;
                case 3:
                    position = new Vector3(-783.675f, 5431.739f, 35.565f);
                    break;
                case 4:
                    position = new Vector3(-655.420f, 5340.894f, 61.180f);
                    break;
                case 5:
                    position = new Vector3(-846.178f, 5316.432f, 77.302f);
                    break;
                case 6:
                    position = new Vector3(-862.280f, 5256.250f, 85.823f);
                    break;
                case 7:
                    position = new Vector3(-497.893f, 4931.103f, 146.854f);
                    break;
                case 8:
                    position = new Vector3(-1563.174f, 4863.088f, 61.039f);
                    break;
                case 9:
                    position = new Vector3(-1561.721f, 4589.228f, 19.208f);
                    break;
                case 10:
                    position = new Vector3(-1216.513f, 4475.584f, 29.550f);
                    break;
                case 11:
                    position = new Vector3(-1009.792f, 4355.187f, 11.675f);
                    break;
                case 12:
                    position = new Vector3(-1447.186f, 4302.891f, 1.981f);
                    break;
                case 13:
                    position = new Vector3(-1673.367f, 4450.542f, 2.097f);
                    break;
                case 14:
                    position = new Vector3(-776.203f, 4398.830f, 17.910f);
                    break;
                case 15:
                    position = new Vector3(-776.203f, 4398.830f, 17.910f);
                    break;
            }
        }

        internal static void DangerousPersonSpawnChooser(out Vector3 position)
        {
            position = default;
            int DangerousPersonSpawnpoints = new Random().Next(1, 14);
            Game.LogTrivial("-!!- Forestry Callouts - |SpawnChooser| - DangerousPersonSpawnpoints, chose: Blondee Swag " + DangerousPersonSpawnpoints + " -!!-");
            switch (DangerousPersonSpawnpoints)
            {
                case 1:
                    position = new Vector3(-588.725f, 5527.590f, 50.442f);
                    break;
                case 2:
                    position = new Vector3(-788.498f, 5298.563f, 82.435f);
                    break;
                case 3:
                    position = new Vector3(-644.998f, 5116.664f, 127.334f);
                    break;
                case 4:
                    position = new Vector3(-756.908f, 5105.381f, 140.833f);
                    break;
                case 5:
                    position = new Vector3(-1577.316f, 4836.143f, 59.553f);
                    break;
                case 6:
                    position = new Vector3(-1560.454f, 4572.264f, 18.605f);
                    break;
                case 7:
                    position = new Vector3(-1212.745f, 4474.019f, 29.579f);
                    break;
                case 8:
                    position = new Vector3(-781.969f, 4395.291f, 18.061f);
                    break;
                case 9:
                    position = new Vector3(-515.283f, 4357.992f, 67.507f);
                    break;
                case 10:
                    position = new Vector3(-1011.701f, 4356.670f, 11.925f);
                    break;
                case 11:
                    position = new Vector3(-1554.501f, 4329.499f, 4.061f);
                    break;
                case 12:
                    position = new Vector3(-1924.018f, 4453.202f, 36.513f);
                    break;
                case 13:
                    position = new Vector3(-1698.926f, 4296.459f, 70.298f);
                    break;
            }
        }

        internal static void IllegalHuntingSpawnChooser(out Vector3 position)
        {
            position = default;
            int illegalHuntingSpawnpoints = new Random().Next(1, 12);
            Game.LogTrivial("-!!- Forestry Callouts - |SpawnChooser| - illegalHuntingSpawnpoints, chose: Blondee Swag " + illegalHuntingSpawnpoints + " -!!-");
            switch (illegalHuntingSpawnpoints)
            {
                case 1:
                    position = new Vector3(-574.049f, 5160.293f, 103.346f);
                    break;
                case 2:
                    position = new Vector3(-780.482f, 5116.171f, 142.502f);
                    break;
                case 3:
                    position = new Vector3(-616.705f, 4881.109f, 192.504f);
                    break;
                case 4:
                    position = new Vector3(-455.129f, 4808.993f, 228.981f);
                    break;
                case 5:
                    position = new Vector3(-733.516f, 4763.263f, 228.174f);
                    break;
                case 6:
                    position = new Vector3(-854.822f, 4828.019f, 297.377f);
                    break;
                case 7:
                    position = new Vector3(-1122.641f, 4752.852f, 233.039f);
                    break;
                case 8:
                    position = new Vector3(-1288.889f, 4953.238f, 151.580f);
                    break;
                case 9:
                    position = new Vector3(-1664.681f, 4604.626f, 47.783f);
                    break;
                case 10:
                    position = new Vector3(-1664.681f, 4604.626f, 47.783f);
                    break;
                case 11:
                    position = new Vector3(-1556.599f, 4455.087f, 15.152f);
                    break;
            }
        }
        internal static void WreckedVehicleSpawnChooser(out Vector3 position, out float heading, out Vector3 pedPosition, out float pedHeading, out int SpawnpointChoosed, out Vector3 pedWanderSpawnpoint)
        {
            position = default;
            heading = default;
            pedPosition = default;
            pedHeading = default;
            SpawnpointChoosed = default;
            pedWanderSpawnpoint = default;
            int WreckedVehicleSpawnpoints = new Random().Next(1, 10);
            Game.LogTrivial("-!!- Forestry Callouts - |SpawnChooser| - WreckedVehicleSpawnpoints, chose: Blondee Swag " + WreckedVehicleSpawnpoints + " -!!-");
            switch (WreckedVehicleSpawnpoints)
            {
                case 1:
                    position = new Vector3(-689.543f, 5284.960f, 67.272f);
                    SpawnpointChoosed = 1;
                    position = new Vector3(-616.851f, 5449.33f, 55.135f);
                    heading = 273.255f;
                    pedPosition = new Vector3(-626.422f, 5455.646f, 54.562f);
                    pedHeading = 237.498f;
                    pedWanderSpawnpoint = new Vector3(-562.659f, 5431.691f, 61.913f);
                    break;
                case 2:
                    position = new Vector3(-709.997f, 5271.391f, 74.614f);
                    heading = 58.479f;
                    pedPosition = new Vector3(-700.366f, 5281.304f, 74.503f);
                    pedHeading = 131.888f;
                    pedWanderSpawnpoint = new Vector3(-783.398f, 5298.836f, 87.711f);
                    SpawnpointChoosed = 2;
                    break;
                case 3:
                    position = new Vector3(-723.511f, 5157.687f, 108.597f);
                    heading = 204.547f;
                    pedPosition = new Vector3(-708.275f, 5156.136f, 114.381f);
                    pedHeading = 355.614f;
                    pedWanderSpawnpoint = new Vector3(-680.068f, 5095.631f, 136.661f);
                    SpawnpointChoosed = 3;
                    break;
                case 4:
                    position = new Vector3(-805.961f, 5507.206f, 25.419f);
                    heading = 15.499f;
                    pedPosition = new Vector3(-805.853f, 5518.024f, 26.148f);
                    pedHeading = 180.90f;
                    SpawnpointChoosed = 4;
                    pedWanderSpawnpoint = new Vector3(-757.038f, 5455.279f, 32.856f);
                    break;
                case 5:
                    position = new Vector3(-1529.536f, 4681.030f, 40.111f);
                    heading = 187.347f;
                    pedPosition = new Vector3(-1533.654f, 4693.855f, 42.398f);
                    pedHeading = 204.379f;
                    pedWanderSpawnpoint = new Vector3(-1601.618f, 4641.055f, 48.483f);
                    SpawnpointChoosed = 5;
                    break;
                case 6:
                    position = new Vector3(-1551.366f, 4469.699f, 18.855f);
                    heading = 214.611f;
                    pedPosition = new Vector3(-1557.326f, 4484.822f, 19.977f);
                    pedHeading = 202.898f;
                    pedWanderSpawnpoint = new Vector3(-1543.667f, 4429.137f, 7.374f);
                    SpawnpointChoosed = 6;
                    break;
                case 7:
                    position = new Vector3(-958.134f, 4404.256f, 15.697f);
                    heading = 253.052f;
                    pedPosition = new Vector3(-958.678f, 4426.126f, 18.344f);
                    pedHeading = 179.904f;
                    pedWanderSpawnpoint = new Vector3(-881.006f, 4391.915f, 19.924f);
                    SpawnpointChoosed = 7;
                    break;
                case 8:
                    position = new Vector3(-914.305f, 5196.630f, 118.228f);
                    heading = 251.380f;
                    pedPosition = new Vector3(-927.464f, 5196.531f, 120.266f);
                    pedHeading = 273.195f;
                    pedWanderSpawnpoint = new Vector3(-973.618f, 5183.897f, 127.041f);
                    SpawnpointChoosed = 8;
                    break;
                case 9:
                    position = new Vector3(-594.250f, 4991.316f, 143.991f);
                    heading = 188.335f;
                    pedPosition = new Vector3(-591.458f, 5001.073f, 143.798f);
                    pedHeading = 165.538f;
                    pedWanderSpawnpoint = new Vector3(-587.880f, 4939.109f, 167.105f);
                    SpawnpointChoosed = 9;
                    break;

            }
        }
        internal static void SpawnChooser(out Vector3 position)
        {
            position = default;
            int ChiliadSpawnpoints = new Random().Next(1, 35);
            Game.LogTrivial("-!!- Forestry Callouts - |SpawnChooser| - ChiliadSpawnpoints, chose: Blondee Swag " + ChiliadSpawnpoints + " -!!-");
            switch (ChiliadSpawnpoints)
            {
                case 1:
                    position = new Vector3(-122.679f, 4607.656f, 124.203f); //(1VEC) x:-122.679 y:4607.656 z:124.203 angle:45.802
                    break;
                case 2:
                    position = new Vector3(-260.872f, 4226.933f, 44.528f); //(2VEC) x:-260.872 y:4226.933 z:44.528 angle:81.552
                    break;
                case 3:
                    position = new Vector3(-614.792f, 4373.041f, 44.106f); //(3VEC) x:-614.792 y:4373.041 z:44.106 angle:83.597
                    break;
                case 4:
                    position = new Vector3(-869.872f, 4401.644f, 20.844f); //(4VEC) x: -869.872 y: 4401.644 z: 20.844 angle: 121.932
                    break;
                case 5:
                    position = new Vector3(1032.517f, 4421.939f, 25.974f); //(5VEC) x:-1032.517 y:4421.939 z:25.974 angle:75.123
                    break;
                case 6:
                    position = new Vector3(-1156.605f, 4372.284f, 9.320f); //(6VEC) x:-1156.605 y:4372.284 z:9.320 angle:114.204
                    break;
                case 7:
                    position = new Vector3(-1405.774f, 4301.625f, 5.131f); //(7VEC) x:-1405.774 y:4301.625 z:5.131 angle:88.268
                    break;
                case 8:
                    position = new Vector3(-1771.288f, 4468.729f, 8.424f); //(8VEC) x:-1771.288 y:4468.729 z:8.424 angle:69.424
                    break;
                case 9:
                    position = new Vector3(-1919.316f, 4435.130f, 40.594f); //(9VEC) x:-1919.316 y:4435.130 z:40.594 angle:250.638
                    break;
                case 10:
                    position = new Vector3(-1607.754f, 4203.725f, 83.058f); //(10VEC) x:-1607.754 y:4203.725 z:83.058 angle:264.425
                    break;
                case 11:
                    position = new Vector3(-1367.612f, 4130.260f, 63.193f); //(11VEC) x:-1367.612 y:4130.260 z:63.193 angle:224.831
                    break;
                case 12:
                    position = new Vector3(-1175.217f, 4285.244f, 81.918f); //(12VEC) x:-1175.217 y:4285.244 z:81.918 angle:261.104
                    break;
                case 13:
                    position = new Vector3(-898.079f, 4097.720f, 161.825f); //(13VEC) x:-898.079 y:4097.720 z:161.825 angle:248.808
                    break;
                case 14:
                    position = new Vector3(-658.460f, 4012.099f, 128.086f); //(14VEC) x: -658.460 y: 4012.099 z: 128.086 angle: 258.036
                    break;
                case 15:
                    position = new Vector3(-315.705f, 4002.803f, 44.139f); //(15VEC) x:-315.705 y:4002.803 z:44.139 angle:219.217
                    break;
                case 16:
                    position = new Vector3(-1252.085f, 4494.537f, 22.052f); //(16VEC) x:-1252.085 y:4494.537 z:22.052 angle:68.979
                    break;
                case 17:
                    position = new Vector3(-1564.927f, 4502.697f, 21.237f); //(17VEC) x:-1564.927 y:4502.697 z:21.237 angle:16.771
                    break;
                case 18:
                    position = new Vector3(-1564.927f, 4502.697f, 21.237f); //(18VEC) x:-1564.927 y:4502.697 z:21.237 angle:16.771
                    break;
                case 19:
                    position = new Vector3(-1522.240f, 4681.234f, 38.304f); //(19VEC) x: -1522.240 y: 4681.234 z: 38.304 angle: 29.699
                    break;
                case 20:
                    position = new Vector3(-1513.894f, 4693.983f, 38.649f); //(20VEC) x:-1513.894 y:4693.983 z:38.649 angle:278.189
                    break;
                case 21:
                    position = new Vector3(-1614.461f, 4757.090f, 52.206f); //(21VEC) x:-1614.461 y:4757.090 z:52.206 angle:303.248
                    break;
                case 22:
                    position = new Vector3(-1289.438f, 4948.656f, 151.822f); //(22VEC) x:-1289.438 y:4948.656 z:151.822 angle:337.793
                    break;
                case 23:
                    position = new Vector3(-996.596f, 5162.230f, 127.702f); //(23VEC) x:-996.596 y:5162.230 z:127.702 angle:341.493
                    break;
                case 24:
                    position = new Vector3(-806.930f, 5320.060f, 76.513f); //(24VEC) x:-806.930 y:5320.060 z:76.513 angle:85.950
                    break;
                case 25:
                    position = new Vector3(-759.102f, 5323.337f, 74.225f); //(25VEC) x:-759.102 y:5323.337 z:74.225 angle:261.468
                    break;
                case 26:
                    position = new Vector3(-563.697f, 5371.572f, 70.214f); //(26VEC) x:-563.697 y:5371.572 z:70.214 angle:164.039
                    break;
                case 27:
                    position = new Vector3(-554.472f, 5391.967f, 68.072f); //(27VEC) x:-554.472 y:5391.967 z:68.072 angle:343.362
                    break;
                case 28:
                    position = new Vector3(-464.993f, 5525.166f, 79.189f); //(28VEC) x:-464.993 y:5525.166 z:79.189 angle:6.966
                    break;
                case 29:
                    position = new Vector3(-571.057f, 5553.720f, 51.764f); //(29VEC) x:-571.057 y:5553.720 z:51.764 angle:348.610
                    break;
                case 30:
                    position = new Vector3(-671.713f, 5687.986f, 29.274f); //(30VEC) x:-671.713 y:5687.986 z:29.274 angle:224.671
                    break;
                case 31:
                    position = new Vector3(-699.147f, 5689.984f, 27.247f); //(31VEC) x:-699.147 y:5689.984 z:27.247 angle:110.564
                    break;
                case 32:
                    position = new Vector3(-752.448f, 5709.807f, 20.693f); //(32VEC) x:-752.448 y:5709.807 z:20.693 angle:342.375
                    break;
                case 33:
                    position = new Vector3(-659.295f, 6004.836f, 10.469f); //(33VEC) x:-659.295 y:6004.836 z:10.469 angle:334.249
                    break;
                case 34:
                    position = new Vector3(-638.119f, 6051.195f, 8.414f); //(34VEC) x:-638.119 y:6051.195 z:8.414 angle:150.555
                    break;
            }
        }
    }
}
