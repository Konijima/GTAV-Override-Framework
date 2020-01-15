using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using GTA;
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

            return JsonConvert.SerializeObject(this);
        }

        public void SetPlayer()
        {
            Player player = Game.Player;
            Ped ped = Game.Player.Character;

            World.CurrentDate = GameDate;
            World.CurrentTimeOfDay = GameTime;

            Function.Call(GTA.Native.Hash.SET_FOLLOW_PED_CAM_VIEW_MODE, OnFootCameraMode);
            Function.Call(GTA.Native.Hash.SET_FOLLOW_VEHICLE_CAM_VIEW_MODE, InVehicleCameraMode);

            player.Money = Money;
            ped.Money = Money;
            ped.HealthFloat = Health;
            ped.MaxHealthFloat = MaxHealth;
            ped.ArmorFloat = Armor;

            Helpers.TeleportPed(ped, new Vector3(X, Y, Z), Heading, true);
        }

        public static PlayerJson FromJson(string json)
        {
            PlayerJson playerJson = JsonConvert.DeserializeObject<PlayerJson>(json);

            return playerJson;
        }
    }

    public static class PlayerManager
    {
        private static bool loaded = false;
        private static string saveFilePath = "GTAVOverrideData";
        private static string playerFileName = "playersave.player0";


        private static string LoadPlayerData()
        {
            if (HasSaveGame())
            {
                return File.ReadAllText(Path.Combine(saveFilePath, playerFileName));
            }

            return "";
        }

        private static void SavePlayerData(string json)
        {
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
            if (Main.configData.Auto_Save_Enabled)
            {
                return true;
            }

            return false;
        }

        public static void SetSavePath(string path, string saveFile)
        {
            saveFilePath = path;
            playerFileName = "playersave." + saveFile;
        }

        public static void Save()
        {
            if (!CanSave()) return;

            PlayerJson payerJson = new PlayerJson();

            if (!Directory.Exists(saveFilePath))
            {
                Directory.CreateDirectory(saveFilePath);
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
                    playerJson.SetPlayer();
                }
            }
            loaded = true;
        }
    }
}
