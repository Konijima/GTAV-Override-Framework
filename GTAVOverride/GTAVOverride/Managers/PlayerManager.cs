using System.IO;
using GTA;
using GTA.UI;
using GTAVOverride.Data;
using GTAVOverride.Functions;

namespace GTAVOverride.Managers
{
    public static class PlayerManager
    {
        private static int lastSave = 0;
        private static int saveDelay = Main.configData.Auto_Save_Delay;


        private static bool loaded = false;
        private static string saveFilePath = "GTAVOverrideData";
        private static string playerFileName = "playersave.player0";

        public static PlayerJson payerJson;
        public static Vehicle CurrentPlayerVehicle = null;

        public static void ReplacePersonalVehicle(Vehicle vehicle)
        {
            RemoveVehicleBlips(CurrentPlayerVehicle);
            RemoveVehicleBlips(vehicle);

            if (CurrentPlayerVehicle != null)
            {
                Debug.Log("Removing last personal vehicle...");
                CurrentPlayerVehicle.IsPersistent = false;
                CurrentPlayerVehicle.MarkAsNoLongerNeeded();
            }

            CurrentPlayerVehicle = vehicle;
            CurrentPlayerVehicle.IsPersistent = true;
            SetPersonalVehicleBlip(CurrentPlayerVehicle);
        }

        public static void LeavePersonalVehicle()
        {
            if (CurrentPlayerVehicle != null) CurrentPlayerVehicle.AttachedBlip.Alpha = 255;
        }

        public static void RemoveVehicleBlips(Vehicle vehicle)
        {
            if (vehicle == null) return;

            if (vehicle.AttachedBlips.Length > 0)
            {
                foreach (Blip b in vehicle.AttachedBlips)
                {
                    b.Delete();
                }
            }
        }

        private static void SetPersonalVehicleBlip(Vehicle vehicle)
        {
            // Remove this vehicle blips
            RemoveVehicleBlips(vehicle);

            Debug.Log("Setting up blip...");
            Blip blip = vehicle.AddBlip();
            VehicleClass vehicleClass = CurrentPlayerVehicle.ClassType;

            blip.Sprite = BlipSprite.PersonalVehicleCar;

            if (vehicleClass == VehicleClass.Boats) blip.Sprite = BlipSprite.Boat;
            else if (vehicleClass == VehicleClass.Commercial) blip.Sprite = BlipSprite.GetawayCar;
            else if (vehicleClass == VehicleClass.Compacts) blip.Sprite = BlipSprite.GetawayCar;
            else if (vehicleClass == VehicleClass.Coupes) blip.Sprite = BlipSprite.GetawayCar;
            else if (vehicleClass == VehicleClass.Cycles) blip.Sprite = BlipSprite.GangBike;
            else if (vehicleClass == VehicleClass.Emergency) blip.Sprite = BlipSprite.GetawayCar;
            else if (vehicleClass == VehicleClass.Helicopters) blip.Sprite = BlipSprite.Helicopter;
            else if (vehicleClass == VehicleClass.Industrial) blip.Sprite = BlipSprite.Truck;
            else if (vehicleClass == VehicleClass.Military) blip.Sprite = BlipSprite.Tank;
            else if (vehicleClass == VehicleClass.Motorcycles) blip.Sprite = BlipSprite.GangBike;
            else if (vehicleClass == VehicleClass.Muscle) blip.Sprite = BlipSprite.Deluxo;
            else if (vehicleClass == VehicleClass.OffRoad) blip.Sprite = BlipSprite.GetawayCar;
            else if (vehicleClass == VehicleClass.Planes) blip.Sprite = BlipSprite.Plane;
            else if (vehicleClass == VehicleClass.Sedans) blip.Sprite = BlipSprite.GetawayCar;
            else if (vehicleClass == VehicleClass.Service) blip.Sprite = BlipSprite.Cab;
            else if (vehicleClass == VehicleClass.Sports) blip.Sprite = BlipSprite.SportsCar;
            else if (vehicleClass == VehicleClass.SportsClassics) blip.Sprite = BlipSprite.GetawayCar;
            else if (vehicleClass == VehicleClass.Super) blip.Sprite = BlipSprite.Stromberg;
            else if (vehicleClass == VehicleClass.SUVs) blip.Sprite = BlipSprite.GetawayCar;
            else if (vehicleClass == VehicleClass.Utility) blip.Sprite = BlipSprite.ArmoredTruck;
            else if (vehicleClass == VehicleClass.Vans) blip.Sprite = BlipSprite.ArmoredTruck;

            blip.Color = BlipColor.White;
            blip.Name = "Last Vehicle";
            blip.Scale = 1;

            if (Game.Player.Character.IsInVehicle(vehicle)) blip.Alpha = 0;
            else blip.Alpha = 255;
        }

        private static string LoadPlayerData()
        {
            if (HasSaveGame())
            {
                Debug.Log("Loading player " + playerFileName + " data at " + saveFilePath + playerFileName);
                return File.ReadAllText(Path.Combine(saveFilePath, playerFileName));
            }
            else
            {
                Debug.Log("No player savegame found!");
            }
            return "";
        }

        private static void SavePlayerData(string json)
        {
            Debug.Log("Saving player " + playerFileName + " data at " + saveFilePath + playerFileName);
            File.WriteAllText(Path.Combine(saveFilePath, playerFileName), json);
        }

        public static bool HasSaveGame()
        {
            return File.Exists(Path.Combine(saveFilePath, playerFileName));
        }

        public static bool HasLoadedGame()
        {
            return loaded;
        }

        public static bool CanSave(bool checkLastSaveTime = true)
        {
            if (Main.configData.Auto_Save_Enabled && !Game.IsLoading && !Game.IsPaused)
            {
                Ped ped = Game.Player.Character;

                if (checkLastSaveTime && Game.GameTime < lastSave + saveDelay) return false;

                if (!HasLoadedGame() ||
                    ped.IsDead ||
                    Game.IsMissionActive ||
                    Game.IsCutsceneActive ||
                    ped.IsInAir ||
                    ped.IsInParachuteFreeFall ||
                    ped.IsInWater ||
                    ped.IsFalling ||
                    Game.IsLoading ||
                    Game.IsPaused ||
                    Game.IsMissionActive ||
                    !ped.IsAlive ||
                    ped.IsRagdoll ||
                    ped.IsSwimming ||
                    ped.IsSwimmingUnderWater ||
                    ped.IsInMeleeCombat ||
                    ped.IsOnFire ||
                    ped.Speed > 15f)
                    return false;

                Vehicle current = Game.Player.Character.CurrentVehicle;
                if (current != null && current.ClassType == VehicleClass.Planes && (current.HeightAboveGround > 5f || current.Speed > 2f))
                    return false;

                if (current != null && current.ClassType == VehicleClass.Helicopters && (current.HeightAboveGround > 5f || current.Speed > 2f))
                    return false;

                if (!Main.configData.Auto_Save_In_Vehicle && Game.Player.Character.IsSittingInVehicle()) return false;

                return true;
            }

            return false;
        }

        public static void SetSavePath(string path, string saveFile)
        {
            Debug.Log("Player save path set to " + path + " with file name " + saveFile);

            saveFilePath = path;
            playerFileName = "playersave." + saveFile;
        }

        public static void Save()
        {
            if (!CanSave()) return;

            lastSave = Game.GameTime;

            payerJson = new PlayerJson();

            if (!Directory.Exists(saveFilePath))
            {
                Debug.Log("Player save directory not found, creating it...");
                Directory.CreateDirectory(saveFilePath);
            }

            if (CurrentPlayerVehicle != null)
            {
                payerJson.CurrentVehicle = new VehicleJson();
                payerJson.CurrentVehicle.SetVehicle(CurrentPlayerVehicle);
            }

            SavePlayerData(payerJson.ToJson());
        }

        public static void Load()
        {
            if (HasSaveGame())
            {
                string json = LoadPlayerData();
                if (json != "")
                {
                    PlayerJson playerJson = PlayerJson.FromJson(json);
                    playerJson.Spawn();
                }
            }
            else
            {
                Screen.FadeIn(500);

                if (Main.configSettings.Info_Intro_Screen)
                {
                    IntroScreen intro = Script.InstantiateScript<IntroScreen>();
                    intro.Start();
                }
            }
            loaded = true;
            lastSave = Game.GameTime;
            Debug.Log("Player loaded!");
        }
    }
}
