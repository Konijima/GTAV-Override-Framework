using System.Collections.Generic;
using GTA.Math;
using GTAVOverride.Classes;

namespace GTAVOverride.Managers
{
    public static class AmmunationManager
    {
        public static List<Ammunation> Ammunations = new List<Ammunation>();

        public static void CreateAmmunations()
        {
            Debug.Log("Creating Ammunations...");

            CreateAmmunation(new Vector3(21.72542f, -1107.191f, 29.79702f), 343.6222f, true, "Ammunation01"); // ammunation pillboxhill
            CreateAmmunation(new Vector3(1693.553f, 3759.291f, 34.70535f), 48.2788f, false, "Ammunation02"); // ammunation sandyshore
            CreateAmmunation(new Vector3(251.7146f, -49.34251f, 69.94105f), 251.1295f, false, "Ammunation03"); // ammunation Vinewood/hawick
            CreateAmmunation(new Vector3(842.8749f, -1032.97f, 28.19486f), 182.2369f, false, "Ammunation04"); // ammunation La mesa
        }

        public static Ammunation CreateAmmunation(Vector3 position, float heading, bool hasShootingRange, string doorId)
        {
            Ammunation ammunation = new Ammunation(position, heading, hasShootingRange, doorId);

            ammunation.CreateBlip();

            Ammunations.Add(ammunation);

            return ammunation;
        }

        public static void DeleteAmmunationBlips()
        {
            foreach (Ammunation ammunation in Ammunations)
            {
                ammunation.DeleteBlip();
            }
            Debug.Log("Deleted all Ammunation blips!");
        }

        public static void UpdateAllAmmunations()
        {
            foreach (Ammunation ammunation in Ammunations)
            {
                ammunation.Update();
            }
        }

        public static void ShowAmmunationBlips()
        {
            foreach (Ammunation ammunation in Ammunations)
            {
                ammunation.ShowBlip();
            }
            Debug.Log("Show all Ammunation blips!");
        }

        public static void HideAmmunationBlips()
        {
            foreach (Ammunation ammunation in Ammunations)
            {
                ammunation.HideBlip();
            }
            Debug.Log("Hide all Ammunation blips!");
        }

        public static void LockAllAmmunations()
        {
            foreach (Ammunation ammunation in Ammunations)
            {
                ammunation.Lock();
            }
            Debug.Log("Locked all Ammunations!");
        }

        public static void UnlockAllAmmunations()
        {
            foreach (Ammunation ammunation in Ammunations)
            {
                ammunation.Unlock();
            }
            Debug.Log("Unlocked all Ammunations!");
        }
    }
}
