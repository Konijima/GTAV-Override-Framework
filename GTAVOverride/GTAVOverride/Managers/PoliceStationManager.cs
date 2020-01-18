using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using GTAVOverride.Enums;

namespace GTAVOverride.Managers
{
    public static class PoliceStationManager
    {
        public static List<Blip> PoliceStationBlips = new List<Blip>();
        public static List<int> CustomPoliceStations = new List<int>();

        public static void CreateDefaultPoliceStationBlips()
        {
            Debug.Log("Creating all Police Stations...");
            
            CreatePoliceStationBlip(new Vector3(-1092.9016113281f, -808.55224609375f, 0f)); // Vespucci Police Station
            CreatePoliceStationBlip(new Vector3(363.5834f, -1587.378f, 0f)); // Davis Police Station
            CreatePoliceStationBlip(new Vector3(-561.56872558594f, -131.63426208496f, 0f)); // Rockford Hills Police Station
            CreatePoliceStationBlip(new Vector3(1852.588f, 3688.816f, 0f)); // Sandy Shores Police Station
            CreatePoliceStationBlip(new Vector3(-442.02770996094f, 6018.0600585938f, 0f)); // Paleto Bay Police Station
            CreatePoliceStationBlip(new Vector3(639.10504150391f, 1.50139772892f, 0f)); // Vinewood Police Station
            CreatePoliceStationBlip(new Vector3(477.90216064453f, -978.20941162109f, 0f)); // Mission Row Police Station
            CreatePoliceStationBlip(new Vector3(829.6835f, -1289.895f, 0f)); // La Mesa Police Station
        }

        public static Blip CreatePoliceStationBlip(Vector3 position)
        {
            Blip blip = World.CreateBlip(position);
            blip.Sprite = BlipSprite.PoliceStation;
            blip.Color = BlipColor.Blue;
            blip.IsShortRange = true;

            PoliceStationBlips.Add(blip);

            return blip;
        }

        public static void DeletePoliceStationBlips()
        {
            foreach (Blip blip in PoliceStationBlips)
            {
                blip.Delete();
            }
        }

        public static void ShowPoliceStationBlips()
        {
            foreach (Blip blip in PoliceStationBlips)
            {
                blip.Alpha = 255;
            }
        }

        public static void HidePoliceStationBlips()
        {
            foreach (Blip blip in PoliceStationBlips)
            {
                blip.Alpha = 0;
            }
        }

        public static void EnablePoliceStation(int policeStationId)
        {
            Function.Call(Hash.DISABLE_POLICE_RESTART, policeStationId, false);
        }

        public static void EnablePoliceStation(PoliceStationIds policeStationId)
        {
            Function.Call(Hash.DISABLE_POLICE_RESTART, (int)policeStationId, false);
        }

        public static void DisablePoliceStation(int policeStationId)
        {
            Function.Call(Hash.DISABLE_POLICE_RESTART, policeStationId, true);
        }

        public static void DisablePoliceStation(PoliceStationIds policeStationId)
        {
            Function.Call(Hash.DISABLE_POLICE_RESTART, (int)policeStationId, true);
        }

        public static int CreatePoliceStation(Vector3 position, float heading, PoliceStationIds policeStationId)
        {
            int newPoliceStationId = Function.Call<int>(Hash.ADD_POLICE_RESTART, position.X, position.Y, position.Z, heading, (int)policeStationId);

            CustomPoliceStations.Add(newPoliceStationId);

            return newPoliceStationId;
        }

        public static void EnableAllPoliceStations()
        {
            for (int H = 0; H < 7; H++)
            {
                EnablePoliceStation(H);
            }
            Debug.Log("Enabling all Police Station spawns...");
        }

        public static void DisableAllPoliceStations()
        {
            for (int H = 0; H < 7; H++)
            {
                DisablePoliceStation(H);
            }
            Debug.Log("Disabling all Police Station spawns...");
        }

        public static void EnableAllCustomPoliceStations()
        {
            foreach (int H in CustomPoliceStations)
            {
                EnablePoliceStation(H);
            }
            Debug.Log("Enabling all custom Police Station spawns...");
        }

        public static void DisableAllCustomPoliceStations()
        {
            foreach (int H in CustomPoliceStations)
            {
                DisablePoliceStation(H);
            }
            Debug.Log("Disabling all custom Police Station spawns...");
        }
    }
}
