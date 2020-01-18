using System.Collections.Generic;
using GTA.Math;
using GTAVOverride.Classes;

namespace GTAVOverride.Managers
{
    public static class AtmManager
    {
        public static List<ATM> Atms = new List<ATM>();

        public static void CreateATMs()
        {
            Debug.Log("Creating ATMs...");

            CreateATM(new Vector3(-1109.797f, -1690.808f, 4.375014f), 122.9616f);
            CreateATM(new Vector3(-821.6062f, -1081.885f, 11.13243f), 29.3056f);
            CreateATM(new Vector3(-537.8409f, -854.5145f, 29.28953f), 182.9156f);
            CreateATM(new Vector3(-1315.744f, -834.6907f, 16.96173f), 311.8347f);
            CreateATM(new Vector3(-1314.786f, -835.9669f, 16.96015f), 310.3952f);
            CreateATM(new Vector3(-1570.069f, -546.6727f, 34.95547f), 215.2224f);
            CreateATM(new Vector3(-1571.018f, -547.3666f, 34.95734f), 218.438f);
            CreateATM(new Vector3(-866.6416f, -187.8008f, 37.84286f), 123.5083f);
            CreateATM(new Vector3(-867.6165f, -186.1373f, 37.8433f), 120.8281f);
            CreateATM(new Vector3(-721.1284f, -415.5296f, 34.98175f), 268.9118f);
            CreateATM(new Vector3(-254.3758f, -692.4947f, 33.63751f), 159.0533f);
            CreateATM(new Vector3(24.37422f, -946.0142f, 29.35756f), 339.1346f);
            CreateATM(new Vector3(130.1186f, -1292.669f, 29.26953f), 300.0509f);
            CreateATM(new Vector3(129.7023f, -1291.954f, 29.26953f), 303.6002f);
            CreateATM(new Vector3(129.2096f, -1291.14f, 29.26953f), 298.3779f);
            CreateATM(new Vector3(288.8256f, -1282.364f, 29.64128f), 271.0125f);
            CreateATM(new Vector3(289.0061f, -1256.769f, 29.44076f), 268.2305f);
            CreateATM(new Vector3(1077.768f, -776.4548f, 58.23997f), 186.3605f);
            CreateATM(new Vector3(527.2687f, -160.7156f, 57.08937f), 272.5496f);
            CreateATM(new Vector3(-867.5897f, -186.1757f, 37.84291f), 117.088f);
            CreateATM(new Vector3(-866.6556f, -187.7766f, 37.84278f), 118.6207f);
            CreateATM(new Vector3(-1205.024f, -326.2916f, 37.83985f), 117.6146f);
            CreateATM(new Vector3(-1205.703f, -324.7474f, 37.85942f), 117.3035f);
            CreateATM(new Vector3(-1570.167f, -546.7214f, 34.95663f), 216.3378f);
            CreateATM(new Vector3(-1571.056f, -547.3947f, 34.95724f), 213.4567f);
            CreateATM(new Vector3(-57.64693f, -92.66162f, 57.77995f), 295.8091f);
            CreateATM(new Vector3(527.3583f, -160.6381f, 57.0933f), 271.9661f);
            CreateATM(new Vector3(-165.1658f, 234.8314f, 94.92194f), 90.9362f);
            CreateATM(new Vector3(-165.1503f, 232.7887f, 94.92194f), 94.44688f);
            CreateATM(new Vector3(-2072.445f, -317.3048f, 13.31597f), 269.9654f);
            CreateATM(new Vector3(-3241.082f, 997.5428f, 12.55044f), 40.48888f);
            CreateATM(new Vector3(-1091.462f, 2708.637f, 18.95291f), 44.16092f);
            CreateATM(new Vector3(1172.492f, 2702.492f, 38.17477f), 359.9989f);
            CreateATM(new Vector3(1171.537f, 2702.492f, 38.17542f), 359.9672f);
            CreateATM(new Vector3(1822.637f, 3683.131f, 34.27678f), 207.7707f);
            CreateATM(new Vector3(1686.753f, 4815.806f, 42.00874f), 272.3396f);
            CreateATM(new Vector3(1701.209f, 6426.569f, 32.76408f), 65.12852f);
            CreateATM(new Vector3(-95.54314f, 6457.19f, 31.46093f), 46.20586f);
            CreateATM(new Vector3(-97.23336f, 6455.469f, 31.46682f), 49.50279f);
            CreateATM(new Vector3(-386.7451f, 6046.102f, 31.50172f), 315.2239f);
            CreateATM(new Vector3(-1091.42f, 2708.629f, 18.95568f), 46.68598f);
            CreateATM(new Vector3(5.132f, -919.7711f, 29.55953f), 250.4304f);
            CreateATM(new Vector3(-660.703f, -853.971f, 24.484f), 180.1663f);
            CreateATM(new Vector3(-2293.827f, 354.817f, 174.602f), 115.5668f);
            CreateATM(new Vector3(-2294.637f, 356.553f, 174.602f), 113.6837f);
            CreateATM(new Vector3(-2295.377f, 358.241f, 174.648f), 110.682f);
            CreateATM(new Vector3(-1409.782f, -100.41f, 52.387f), 109.271f);
            CreateATM(new Vector3(-1410.279f, -98.649f, 52.436f), 112.71f);
            CreateATM(new Vector3(33.17087f, -1348.251f, 29.49702f), 184.6213f);
            CreateATM(new Vector3(147.7443f, -1035.649f, 29.34299f), 151.4444f);
            CreateATM(new Vector3(146.026f, -1035.167f, 29.34481f), 154.4736f);
            CreateATM(new Vector3(295.7826f, -896.064f, 29.21946f), 243.8558f);
            CreateATM(new Vector3(296.4495f, -894.1503f, 29.23078f), 249.1935f);
            CreateATM(new Vector3(1153.734f, -326.7081f, 69.20514f), 100.984f);
            CreateATM(new Vector3(1166.953f, -456.0949f, 66.80185f), 338.2458f);
            CreateATM(new Vector3(1138.329f, -468.9302f, 66.73131f), 74.24528f);
            CreateATM(new Vector3(-56.93884f, -1752.217f, 29.42102f), 40.88365f);
            CreateATM(new Vector3(-717.6683f, -915.7895f, 19.21559f), 72.52481f);
            CreateATM(new Vector3(-712.9374f, -819.0009f, 23.72953f), 353.1915f);
            CreateATM(new Vector3(-710.1151f, -818.892f, 23.72953f), 347.1123f);
            CreateATM(new Vector3(285.5089f, 143.3617f, 104.1719f), 158.422f);
            CreateATM(new Vector3(356.9287f, 173.5581f, 103.0684f), 328.4685f);
            CreateATM(new Vector3(380.757f, 323.3838f, 103.5664f), 156.8707f);
            CreateATM(new Vector3(-28.03569f, -724.5967f, 44.22889f), 348.704f);
            CreateATM(new Vector3(-30.27417f, -723.7722f, 44.22886f), 340.4604f);
            CreateATM(new Vector3(-203.8204f, -861.3698f, 30.26763f), 24.54926f);
            CreateATM(new Vector3(-303.2257f, -829.3121f, 31.41977f), 344.6674f);
            CreateATM(new Vector3(-301.6573f, -829.5886f, 31.41977f), 344.6674f);
        }

        public static ATM CreateATM(Vector3 position, float heading)
        {
            ATM atm = new ATM(position, heading);

            atm.CreateBlip();

            Atms.Add(atm);

            return atm;
        }

        public static void DeleteATMBlips()
        {
            foreach (ATM atm in Atms)
            {
                atm.DeleteBlip();
            }
            Debug.Log("Delete all ATM blips!");
        }

        public static void ShowATMBlips()
        {
            foreach (ATM atm in Atms)
            {
                atm.blip.Alpha = 255;
            }
            Debug.Log("Show all ATM blips!");
        }

        public static void HideATMBlips()
        {
            foreach (ATM atm in Atms)
            {
                atm.blip.Alpha = 0;
            }
            Debug.Log("Hide all ATM blips!");
        }
    }
}
