using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAVOverride.Classes
{
    public class Store
    {
        public Vector3 position;
        public float heading;
        public float range;
        public Blip blip;

        public Store(Vector3 position, float heading, float range = 8f)
        {
            this.position = position;
            this.heading = heading;
            this.range = range;
        }

        public void CreateBlip()
        {
            blip = World.CreateBlip(position);
            blip.Sprite = BlipSprite.Store;
            blip.Color = BlipColor.White;
            blip.IsShortRange = true;
            blip.Name = "Store";
        }

        public void DeleteBlip()
        {
            if (blip != null) blip.Delete();
        }

        public void Show()
        {
            blip.Alpha = 255;
        }

        public void Hide(int alpha = 0)
        {
            blip.Alpha = alpha;
        }
    }
}
