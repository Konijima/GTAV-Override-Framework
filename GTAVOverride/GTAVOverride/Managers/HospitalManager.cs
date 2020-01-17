using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAVOverride.Managers
{
    public enum HospitalIds
    {
        MountZonah = 0,
        CentralLosSantos = 1,
        PillboxHill = 2,
        SandyShores = 3,
        PaletoBay = 4,
    }

    public static class HospitalManager
    {
        public static List<Blip> HospitalBlips = new List<Blip>();
        public static List<int> CustomHospitals = new List<int>();

        public static void CreateDefaultHospitalBlips()
        {
            Helpers.Log("Creating all Hospitals...");

            CreateHospitalBlip(new GTA.Math.Vector3(-474.2133f, -336.6607f, 0f)); // MountZonah
            CreateHospitalBlip(new GTA.Math.Vector3(344.8595f, -1415.773f, 0f)); // CentralLosSantos
            CreateHospitalBlip(new GTA.Math.Vector3(356.6836f, -586.7533f, 0f)); // PillboxHill
            CreateHospitalBlip(new GTA.Math.Vector3(1838.11f, 3680.505f, 0f)); // SandyShores
            CreateHospitalBlip(new GTA.Math.Vector3(-252.493f, 6322.25f, 0f)); // PaletoBay
        }

        public static Blip CreateHospitalBlip(Vector3 position)
        {
            Blip blip = World.CreateBlip(position);
            blip.Sprite = BlipSprite.Hospital;
            blip.Color = BlipColor.Red;
            blip.IsShortRange = true;

            HospitalBlips.Add(blip);

            return blip;
        }

        public static void DeleteHospitalBlips()
        {
            foreach (Blip blip in HospitalBlips)
            {
                blip.Delete();
            }
            Helpers.Log("Delete all Hospitals...");
        }

        public static void ShowHospitalBlips()
        {
            foreach (Blip blip in HospitalBlips)
            {
                blip.Alpha = 255;
            }
            Helpers.Log("Show all Hospital blips");
        }

        public static void HideHospitalBlips()
        {
            foreach (Blip blip in HospitalBlips)
            {
                blip.Alpha = 0;
            }
            Helpers.Log("Hide all Hospital blips");
        }

        public static void EnableHospital(int hospitalId)
        {
            Function.Call(Hash.DISABLE_HOSPITAL_RESTART, hospitalId, false);
        }

        public static void EnableHospital(HospitalIds hospitalId)
        {
            Function.Call(Hash.DISABLE_HOSPITAL_RESTART, (int)hospitalId, false);
        }

        public static void DisableHospital(int hospitalId)
        {
            Function.Call(Hash.DISABLE_HOSPITAL_RESTART, hospitalId, true);
        }

        public static void DisableHospital(HospitalIds hospitalId)
        {
            Function.Call(Hash.DISABLE_HOSPITAL_RESTART, (int)hospitalId, true);
        }

        public static int CreateHospital(Vector3 position, float heading, HospitalIds hospitalId)
        {
            int newHospitalId = Function.Call<int>(Hash.ADD_HOSPITAL_RESTART, position.X, position.Y, position.Z, heading, (int)hospitalId);

            CustomHospitals.Add(newHospitalId);

            return newHospitalId;
        }

        public static void EnableAllHospitals()
        {
            for (int H = 0; H < 8; H++)
            {
                EnableHospital(H);
            }
            Helpers.Log("Enabling all Hospital respawns...");
        }

        public static void DisableAllHospitals()
        {
            for (int H = 0; H < 8; H++)
            {
                DisableHospital(H);
            }
            Helpers.Log("Disabling all Hospital respawns...");
        }

        public static void EnableAllCustomHospitals()
        {
            foreach (int H in CustomHospitals)
            {
                EnableHospital(H);
            }
            Helpers.Log("Enabling all custom Hospital respawns...");
        }

        public static void DisableAllCustomHospitals()
        {
            foreach (int H in CustomHospitals)
            {
                DisableHospital(H);
            }
            Helpers.Log("Disabling all custom Hospital respawns...");
        }
    }
}