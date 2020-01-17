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

            GameDate = World.CurrentDate;
            GameTime = World.CurrentTimeOfDay;

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

            World.CurrentDate = GameDate;
            World.CurrentTimeOfDay = GameTime;

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
        private static bool loaded = false;
        private static string saveFilePath = "GTAVOverrideData";
        private static string playerFileName = "playersave.player0";

        public static PlayerJson payerJson;
        public static Vehicle CurrentPlayerVehicle = null;

        public static void ReplacePersonalVehicle(Vehicle vehicle)
        {
            if (vehicle == null || !vehicle.Exists()) return;

            if (CurrentPlayerVehicle != vehicle)
            {
                Helpers.Log("Replacing personal vehicle...");
                if (CurrentPlayerVehicle != null && CurrentPlayerVehicle.IsAlive && CurrentPlayerVehicle.AttachedBlip != null)
                {
                    Helpers.Log("Removing last personal vehicle blip...");
                    CurrentPlayerVehicle.AttachedBlip.Alpha = 0;
                }

                if (CurrentPlayerVehicle != null)
                {
                    Helpers.Log("Removing last personal vehicle...");
                    CurrentPlayerVehicle.IsPersistent = false;

                    if (CurrentPlayerVehicle.AttachedBlip != null) CurrentPlayerVehicle.AttachedBlip.Alpha = 0;
                }

                Helpers.Log("Setting new car as personal vehicle...");
                CurrentPlayerVehicle = vehicle;

                if (CurrentPlayerVehicle.AttachedBlip == null)
                {
                    Helpers.Log("New car blip not found we create it...");
                    Blip blip = vehicle.AddBlip();
                    if (CurrentPlayerVehicle.Model.IsBike) blip.Sprite = BlipSprite.GangBike;
                    else if (CurrentPlayerVehicle.Model.IsBicycle) blip.Sprite = BlipSprite.PersonalVehicleBike;
                    else if (CurrentPlayerVehicle.Model.IsQuadBike) blip.Sprite = BlipSprite.QuadBike;
                    else if (CurrentPlayerVehicle.Model.IsCar) blip.Sprite = BlipSprite.PersonalVehicleCar;
                    else if (CurrentPlayerVehicle.Model.IsHelicopter) blip.Sprite = BlipSprite.Helicopter;
                    else if (CurrentPlayerVehicle.Model.IsPlane) blip.Sprite = BlipSprite.Plane;
                    else if (CurrentPlayerVehicle.Model.IsBoat) blip.Sprite = BlipSprite.Boat;
                    else if (CurrentPlayerVehicle.Model.IsBlimp) blip.Sprite = BlipSprite.Blimp;
                    else if (CurrentPlayerVehicle.Model.IsCargobob) blip.Sprite = BlipSprite.Cargobob;
                    else blip.Sprite = BlipSprite.PersonalVehicleCar;

                    blip.Color = BlipColor.White;
                    blip.Name = "Last Vehicle";
                    blip.Scale = 1;
                }
            }

            if (Game.Player.Character.IsInVehicle(CurrentPlayerVehicle)) vehicle.AttachedBlip.Alpha = 0;
            else vehicle.AttachedBlip.Alpha = 255;
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

        public static bool CanSave()
        {
            if (Main.configData.Auto_Save_Enabled && !Game.IsLoading && !Game.IsPaused)
            {
                Ped ped = Game.Player.Character;
                if (ped.IsDead ||
                    Game.IsMissionActive ||
                    Game.IsCutsceneActive ||
                    ped.IsInAir ||
                    ped.IsInFlyingVehicle ||
                    ped.IsInHeli ||
                    ped.IsInParachuteFreeFall ||
                    ped.IsInPlane ||
                    ped.IsInWater ||
                    ped.IsFalling)
                    return false;
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
            loaded = true;
            Helpers.Log("Player loaded!");
        }
    }
}
