using System;
using System.Collections.Generic;
using System.Text;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;

namespace GTAVOverride.Managers
{
    public class Ammunation
    {
        public Vector3 position;
        public float heading;
        public Blip blip;

        public Ammunation(Vector3 position, float heading, bool hasShootingRange)
        {
            this.position = position;
            this.heading = heading;

            blip = World.CreateBlip(position);
            blip.Sprite = BlipSprite.AmmuNation;
            if (hasShootingRange)
            {
                blip.Sprite = BlipSprite.AmmuNationShootingRange;
            }
            blip.Color = BlipColor.White;
            blip.IsShortRange = true;
        }
    }

    public static class AmmunationManager
    {
        public static List<Ammunation> Ammunations = new List<Ammunation>();

        public static void CreateAmmunations()
        {
            Helpers.Log("Creating Ammunations...");
            CreateAmmunation(new Vector3(21.72542f, -1107.191f, 29.79702f), 343.6222f, true); // ammunation pillboxhill
            CreateAmmunation(new Vector3(1693.553f, 3759.291f, 34.70535f), 48.2788f, false); // ammunation sandyshore
            CreateAmmunation(new Vector3(251.7146f, -49.34251f, 69.94105f), 251.1295f, false); // ammunation Vinewood/hawick
            CreateAmmunation(new Vector3(842.8749f, -1032.97f, 28.19486f), 182.2369f, false); // ammunation La mesa
        }

        public static Ammunation CreateAmmunation(Vector3 position, float heading, bool hasShootingRange = false)
        {
            Ammunation ammunation = new Ammunation(position, heading, hasShootingRange);

            Ammunations.Add(ammunation);

            return ammunation;
        }

        public static void DeleteAmmunationBlips()
        {
            foreach (Ammunation ammunation in Ammunations)
            {
                ammunation.blip.Delete();
                Helpers.Log("Delete all Ammunation blips!");
            }
        }

        public static void ShowAmmunationBlips()
        {
            foreach (Ammunation ammunation in Ammunations)
            {
                ammunation.blip.Alpha = 255;
            }
            Helpers.Log("Show all Ammunation blips!");
        }

        public static void HideAmmunationBlips()
        {
            foreach (Ammunation ammunation in Ammunations)
            {
                ammunation.blip.Alpha = 0;
                Helpers.Log("Hide all Ammunation blips!");
            }
        }
    }
}
