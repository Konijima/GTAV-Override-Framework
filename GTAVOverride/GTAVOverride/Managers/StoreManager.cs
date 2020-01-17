using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAVOverride.Managers
{
    public class Store
    {
        public Vector3 position;
        public float heading;
        public Blip blip;

        public Store(Vector3 position, float heading)
        {
            this.position = position;
            this.heading = heading;

            blip = World.CreateBlip(position);
            blip.Sprite = BlipSprite.Store;
            blip.Color = BlipColor.White;
            blip.IsShortRange = true;

            Function.Call(Hash.BEGIN_TEXT_COMMAND_SET_BLIP_NAME, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Store");
            Function.Call(Hash.END_TEXT_COMMAND_SET_BLIP_NAME, blip.Handle);
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

    public static class StoreManager
    {
        public static List<Store> Stores = new List<Store>();

        public static void CreateStores()
        {
            Helpers.Log("Creating all Stores...");

            CreateStore(new Vector3(27.52871f, -1345.263f, 29.49702f), 91.18019f);
            CreateStore(new Vector3(1162.928f, -323.5625f, 69.20512f), 287.8695f);
            CreateStore(new Vector3(1162.928f, -323.5625f, 69.20512f), 287.8695f);
            CreateStore(new Vector3(-48.50315f, -1756.276f, 29.42099f), 233.0288f);
            CreateStore(new Vector3(-708.8316f, -913.6761f, 19.21561f), 268.9477f);
            CreateStore(new Vector3(375.5313f, 326.9882f, 103.5664f), 74.97479f);
        }

        public static Store CreateStore(Vector3 position, float heading)
        {
            Store store = new Store(position, heading);

            Stores.Add(store);

            return store;
        }

        public static void DeleteStoreBlips()
        {
            foreach (Store store in Stores)
            {
                store.blip.Delete();
            }
            Helpers.Log("Deleting all Stores blips...");
        }

        public static void ShowStoreBlips()
        {
            foreach (Store store in Stores)
            {
                store.Show();
            }
            Helpers.Log("Show all Stores blips...");
        }

        public static void HideStoreBlips(int alpha = 0)
        {
            foreach (Store store in Stores)
            {
                store.Hide(alpha);
            }
            Helpers.Log("Hide all Stores blips...");
        }
    }
}