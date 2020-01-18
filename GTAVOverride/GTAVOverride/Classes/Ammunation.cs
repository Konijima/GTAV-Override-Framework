using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTAVOverride.Managers;

namespace GTAVOverride.Classes
{
    public class Ammunation
    {
        public Vector3 position;
        public float heading;
        public bool hasShootingRange;
        public string doorId;

        private Blip blip;
        private TimeSpan openingHour;
        private TimeSpan closingHour;

        public Ammunation(Vector3 position, float heading, bool hasShootingRange, string doorId)
        {
            this.position = position;
            this.heading = heading;
            this.hasShootingRange = hasShootingRange;
            this.doorId = doorId;

            this.openingHour = new TimeSpan(7, 0, 0);
            this.closingHour = new TimeSpan(21, 0, 0);
        }

        public bool IsOpen
        {
            get
            {
                return (DateTimeManager.CurrentTime >= openingHour && DateTimeManager.CurrentTime < closingHour);
            }
        }

        public void Update()
        {
            if (IsOpen) Unlock();
            else Lock();
        }

        public void CreateBlip()
        {
            blip = World.CreateBlip(position);
            blip.Sprite = BlipSprite.AmmuNation;
            if (hasShootingRange) blip.Sprite = BlipSprite.AmmuNationShootingRange;
            blip.Color = BlipColor.White;
            blip.IsShortRange = true;
        }

        public void DeleteBlip()
        {
            if (blip != null) blip.Delete();
        }

        public void ShowBlip()
        {
            if (blip != null) blip.Alpha = 255;
        }

        public void HideBlip(int alpha = 0)
        {
            if (blip != null) blip.Alpha = alpha;
        }

        public void Lock()
        {
            foreach(Door door in DoorManager.GetDoorsById(this.doorId))
            {
                door.Lock();
            }
        }

        public void Unlock()
        {
            foreach (Door door in DoorManager.GetDoorsById(this.doorId))
            {
                door.Unlock();
            }
        }
    }
}
