using GTA;
using System;
using System.Collections.Generic;
using System.Text;

namespace GTAVOverride.Managers
{
    public static class PeopleManager
    {
        public static List<Ped> ThreatenedPeds = new List<Ped>();
        public static List<Ped> RobbedPeds = new List<Ped>();

        public static int TotalPedsThreatened
        {
            get { return ThreatenedPeds.Count; }
        }

        public static void AddThreatenedPed(Ped ped)
        {
            if (ThreatenedPeds.Count > 20) ThreatenedPeds.RemoveAt(ThreatenedPeds.Count - 1);
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
