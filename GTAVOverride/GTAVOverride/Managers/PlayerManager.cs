using System.IO;
using System;
using Newtonsoft.Json;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using GTAVOverride.Functions;

namespace GTAVOverride.Managers
{
    public class PlayerJson
    {
        public Weather GameWeather { get; set; }
        public DateTime GameDate { get; set; }
        public TimeSpan GameTime { get; set; }
        public int OnFootCameraMode { get; set; }
        public int InVehicleCameraMode { get; set; }
        public int Money { get; set; }
        public int Bank { get; set; }
        public int Hash { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Heading { get; set; }
        public float Health { get; set; }
        public float MaxHealth { get; set; }
        public float Armor { get; set; }
        public WeaponHash Weapon { get; set; }
        public VehicleJson CurrentVehicle { get; set; }

        public PlayerJson()
        {
            CurrentVehicle = null;
        }

        public string ToJson()
        {
            Player player = Game.Player;
            Ped ped = Game.Player.Character;

            GameWeather = World.Weather;
            GameDate = DateTimeManager.CurrentDate;
            GameTime = DateTimeManager.CurrentTime;

            OnFootCameraMode = Function.Call<int>(GTA.Native.Hash.GET_FOLLOW_PED_CAM_VIEW_MODE);
            InVehicleCameraMode = Function.Call<int>(GTA.Native.Hash.GET_FOLLOW_VEHICLE_CAM_VIEW_MODE);

            Money = player.Money;
            Bank = 0;
            Hash = ped.Model.Hash;
            X = ped.Position.X;
            Y = ped.Position.Y;
            Z = ped.Position.Z;
            Heading = ped.Heading;
            Health = ped.HealthFloat;
            MaxHealth = ped.MaxHealthFloat;
            Armor = ped.ArmorFloat;
            Weapon = ped.Weapons.Current.Hash;

            if (ped.CurrentVehicle != null) PlayerManager.CurrentPlayerVehicle = ped.CurrentVehicle;
            else if (ped.LastVehicle != null) PlayerManager.CurrentPlayerVehicle = ped.LastVehicle;

            if (PlayerManager.CurrentPlayerVehicle != null && !PlayerManager.CurrentPlayerVehicle.IsAlive) PlayerManager.CurrentPlayerVehicle = null;

            if (PlayerManager.CurrentPlayerVehicle != null && PlayerManager.CurrentPlayerVehicle.Exists())
            {
                CurrentVehicle = new VehicleJson();
                CurrentVehicle.SetVehicle(PlayerManager.CurrentPlayerVehicle);
            }
           
            return JsonConvert.SerializeObject(this);
        }

        public void Spawn()
        {
            Player player = Game.Player;
            Ped ped = Game.Player.Character;

            if (!Main.configData.Auto_Save_When_Wanted) Game.Player.WantedLevel = 0;

            World.Weather = GameWeather;
            DateTimeManager.SetDate(GameDate);
            DateTimeManager.SetTime(GameTime);

            Function.Call(GTA.Native.Hash.SET_FOLLOW_PED_CAM_VIEW_MODE, OnFootCameraMode);
            Function.Call(GTA.Native.Hash.SET_FOLLOW_VEHICLE_CAM_VIEW_MODE, InVehicleCameraMode);

            player.Money = Money;
            ped.Money = Money;
            ped.HealthFloat = Health;
            ped.MaxHealthFloat = MaxHealth;
            ped.ArmorFloat = Armor;

            InitPlayer setupPlayer = Script.InstantiateScript<InitPlayer>();
            setupPlayer.Start(this);

            Helpers.Log("Init player started!");
        }

        public static PlayerJson FromJson(string json)
        {
            PlayerJson playerJson = JsonConvert.DeserializeObject<PlayerJson>(json);

            return playerJson;
        }
    }

    public class VehicleJson
    {
        [JsonIgnore]
        private Vehicle _vehicle;

        public bool PlayerInside { get; set; }
        public int Hash { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Heading { get; set; }
        public float Health { get; set; }
        public float BodyHealth { get; set; }
        public float EngineHealth { get; set; }
        public float PetrolTankHealth { get; set; }
        public bool IsStolen { get; set; }
        public bool IsSirenActive { get; set; }
        public bool IsEngineRunning { get; set; }
        public bool IsInteriorLightOn { get; set; }
        public VehicleColor PrimaryColor { get; set; }
        public VehicleColor SecondaryColor { get; set; }
        public VehicleColor RimColor { get; set; }
        public VehicleColor DashboardColor { get; set; }
        public System.Drawing.Color TireSmokeColor { get; set; }
        public int ColorCombination { get; set; }
        public int Livery { get; set; }
        public string LicensePlate { get; set; }
        public string RadioStation { get; set; }
        public VehicleLockStatus LockStatus { get; set; }


        public void SetVehicle(Vehicle vehicle)
        {
            _vehicle = vehicle;

            PlayerInside = Game.Player.Character.IsInVehicle(_vehicle);

            Hash = _vehicle.Model.Hash;
            X = _vehicle.Position.X;
            Y = _vehicle.Position.Y;
            Z = _vehicle.Position.Z;
            Heading = _vehicle.Heading;
            Health = _vehicle.HealthFloat;
            BodyHealth = _vehicle.BodyHealth;
            EngineHealth = _vehicle.EngineHealth;
            PetrolTankHealth = _vehicle.PetrolTankHealth;
            IsStolen = _vehicle.IsStolen;
            IsSirenActive = _vehicle.IsSirenActive;
            IsEngineRunning = _vehicle.IsEngineRunning;
            IsInteriorLightOn = _vehicle.IsInteriorLightOn;
            LockStatus = _vehicle.LockStatus;

            PrimaryColor = _vehicle.Mods.PrimaryColor;
            SecondaryColor = _vehicle.Mods.SecondaryColor;
            RimColor = _vehicle.Mods.RimColor;
            DashboardColor = _vehicle.Mods.DashboardColor;
            TireSmokeColor = _vehicle.Mods.TireSmokeColor;
            ColorCombination = _vehicle.Mods.ColorCombination;
            Livery = _vehicle.Mods.Livery;
            LicensePlate = _vehicle.Mods.LicensePlate;
            RadioStation = Function.Call<string>(GTA.Native.Hash.GET_RADIO_STATION_NAME, Game.RadioStation);
        }

        public string ToJson()
        {
            Helpers.Log("Vehicle to json...");

            SetVehicle(_vehicle);

            return JsonConvert.SerializeObject(this);
        }

        public void Spawn()
        {
            
        }

        public static VehicleJson FromJson(string json)
        {
            Helpers.Log("Loading vehicle from json...");

            VehicleJson vehicleJson = JsonConvert.DeserializeObject<VehicleJson>(json);

            return vehicleJson;
        }
    }

    public class LoadoutJson
    {

    }

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
                Helpers.Log("Removing last personal vehicle...");
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

            Helpers.Log("Setting up blip...");
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
                Helpers.Log("Loading player " + playerFileName + " data at " + saveFilePath + playerFileName);
                return File.ReadAllText(Path.Combine(saveFilePath, playerFileName));
            }
            else
            {
                Helpers.Log("No player savegame found!");
            }
            return "";
        }

        private static void SavePlayerData(string json)
        {
            Helpers.Log("Saving player " + playerFileName + " data at " + saveFilePath + playerFileName);
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
            Helpers.Log("Player save path set to " + path + " with file name " + saveFile);

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
                Helpers.Log("Player save directory not found, creating it...");
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
            Helpers.Log("Player loaded!");
        }
    }
}
