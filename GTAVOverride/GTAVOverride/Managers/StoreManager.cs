using System.Collections.Generic;
using GTA.Math;
using GTAVOverride.Classes;

namespace GTAVOverride.Managers
{
    public static class StoreManager
    {
        public static List<Store> Stores = new List<Store>();

        public static void CreateStores()
        {
            Debug.Log("Creating all Stores...");

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

            store.CreateBlip();

            Stores.Add(store);

            return store;
        }

        public static void DeleteStoreBlips()
        {
            foreach (Store store in Stores)
            {
                store.DeleteBlip();
            }
            Debug.Log("Deleting all Stores blips...");
        }

        public static void ShowStoreBlips()
        {
            foreach (Store store in Stores)
            {
                store.Show();
            }
            Debug.Log("Show all Stores blips...");
        }

        public static void HideStoreBlips(int alpha = 0)
        {
            foreach (Store store in Stores)
            {
                store.Hide(alpha);
            }
            Debug.Log("Hide all Stores blips...");
        }
    }
}