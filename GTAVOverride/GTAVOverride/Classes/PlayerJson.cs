using System;
using Newtonsoft.Json;
using GTA;
using GTA.Native;
using GTAVOverride.Functions;
using GTAVOverride.Managers;

namespace GTAVOverride.Data
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
        public LoadoutJson Weapons { get; set; }
        public VehicleJson CurrentVehicle { get; set; }

        public PlayerJson()
        {
            Weapons = null;
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

            // loadout
            Weapon = ped.Weapons.Current.Hash;
            Weapons = new LoadoutJson();
            Weapons.SetWeapons(ped);

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

            Debug.Log("Init player started!");
        }

        public static PlayerJson FromJson(string json)
        {
            PlayerJson playerJson = JsonConvert.DeserializeObject<PlayerJson>(json);

            return playerJson;
        }
    }
}
