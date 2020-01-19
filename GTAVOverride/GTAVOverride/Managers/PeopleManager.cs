using GTA;
using GTAVOverride.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GTAVOverride.Managers
{
    public static class PeopleManager
    {
        public static List<Ped> RandomizedPeds = new List<Ped>();
        public static List<Ped> ThreatenedPeds = new List<Ped>();
        public static List<Ped> RobbedPeds = new List<Ped>();

        public static int TotalPedsThreatened
        {
            get { return ThreatenedPeds.Count; }
        }

        public static bool RandomizePedMoney(Ped ped)
        {
            if (RandomizedPeds.Contains(ped) || ThreatenedPeds.Contains(ped) || RobbedPeds.Contains(ped)) return false;

            int maxMoney = 0;
            int minMoney = 0;
            SocialClasses classe = Helpers.GetPedSocialClass(ped);

            if (classe == SocialClasses.Poor) maxMoney = Main.configSettings.Randomize_Poor_People_MaxMoney;
            if (classe == SocialClasses.Regular)
            {
                maxMoney = Main.configSettings.Randomize_Regular_People_MaxMoney;
                minMoney = Main.configSettings.Randomize_Regular_People_MaxMoney / 5;
            }
            if (classe == SocialClasses.Rich)
            {
                maxMoney = Main.configSettings.Randomize_Rich_People_MaxMoney;
                minMoney = Main.configSettings.Randomize_Rich_People_MaxMoney / 4;
            }

            if (maxMoney < 0) maxMoney = 0;
            if (maxMoney > 9999999) maxMoney = 9999999;

            if (ped.IsAlive && !ThreatenedPeds.Contains(ped) && !RobbedPeds.Contains(ped))
            {
                ped.Money = Helpers.Random.Next(minMoney, maxMoney);
                ped.DropsEquippedWeaponOnDeath = true;
            }

            if (RandomizedPeds.Count > 20) RandomizedPeds.RemoveAt(0);
            RandomizedPeds.Add(ped);

            return true;
        }

        public static void AddThreatenedPed(Ped ped)
        {
            if (ThreatenedPeds.Count > 20) ThreatenedPeds.RemoveAt(0);
            ThreatenedPeds.Add(ped);
        }

        public static void CancelThreatenedPed(Ped ped)
        {
            ThreatenedPeds.Remove(ped);
        }

        public static void AddRobbedPed(Ped ped)
        {
            if (RobbedPeds.Count > 20) RobbedPeds.RemoveAt(RobbedPeds.Count - 1);
            RobbedPeds.Add(ped);
            ThreatenedPeds.Remove(ped);
        }
    }
}
