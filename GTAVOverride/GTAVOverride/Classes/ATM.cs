using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAVOverride.Classes
{
    public class ATM
    {
        public Vector3 position;
        public float heading;
        public Blip blip;

        private int holding;
        private bool broken;
        private int nextRespawn;

        public ATM(Vector3 position, float heading)
        {
            this.position = position;
            this.heading = heading;

            broken = false;
            nextRespawn = 0;

            RandomizeHolding();
        }

        public void Update()
        {
            if (broken && Game.GameTime > nextRespawn)
            {
                broken = false;
                RandomizeHolding();
            }
        }

        public void RandomizeHolding()
        {
            holding = Helpers.Random.Next(200, 2000);
        }

        public void CreateBlip()
        {
            blip = World.CreateBlip(position);
            blip.Sprite = BlipSprite.CashPickup;
            blip.Color = BlipColor.Green;
            blip.IsShortRange = true;
            blip.Scale = 0.6f;
            blip.Name = "ATM";
        }

        public void DeleteBlip()
        {
            if (blip != null) blip.Delete();
        }

        public void SpawnMoney()
        {
            int amount = 1;
            if (holding > 500) amount = 2;
            else if (holding > 1000) amount = 3;
            else if (holding > 1500) amount = 4;
            else if (holding > 2000) amount = 5;
            else if (holding > 2500) amount = 8;

            int value = holding / amount;

            Function.Call(Hash.CREATE_MONEY_PICKUPS, position.X, position.Y, position.Z, value, amount, 0);
        }

        public void Break()
        {
            if (!broken)
            {
                broken = true;
                nextRespawn = Game.GameTime + 10000;
                SpawnMoney();
            }
        }
    }
}
