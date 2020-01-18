using System;
using System.IO;
using System.Collections.Generic;
using GTA;
using GTA.UI;
using GTA.Math;
using GTAVOverride.Functions;

namespace GTAVOverride
{
    public static class Debug
    {
        public static readonly string LogDirectoryPath = "scripts/GTAVOverrideData/logs";
        public static readonly string LogFilename = "GTAVOverride";

        private static DateTime _logStartTime;
        private static List<string> _logEnties;
        private static Debugger _debugger;

        public static void InitLog()
        {
            if (Main.configSettings.Dont_Generate_Log) return;

            if (_logEnties == null)
            {
                _logEnties = new List<string>();

                _logStartTime = DateTime.Now;

                Log("========================================================", false);
                Log("Log start at " + _logStartTime.ToLongTimeString(), false);
                Log("========================================================", false);
            }
        }

        public static void Log(string text, bool autoDate = true)
        {
            if (Main.configSettings.Dont_Generate_Log) return;

            if (_logEnties != null) _logEnties.Add(((autoDate) ? DateTime.Now.ToLongTimeString() + ": " : "") + text);
        }

        public static void EndLog()
        {
            if (Main.configSettings.Dont_Generate_Log) return;

            if (_logEnties != null)
            {
                Log("========================================================", false);
                Log("Log end at " + DateTime.Now.ToLongTimeString(), false);

                if (!Directory.Exists(LogDirectoryPath)) Directory.CreateDirectory(LogDirectoryPath);

                File.WriteAllLines(Path.Combine(LogDirectoryPath, LogFilename) + "-" + _logStartTime.Hour + "-" + _logStartTime.Minute + "-" + _logStartTime.Second + ".log", _logEnties);
            }
        }

        public static void CreateDebugger()
        {
            if (!Main.configSettings.Debug_Mode) return;

            if (_debugger == null)
            {
                _debugger = Script.InstantiateScript<Debugger>();
                _debugger.Create();
            }
        }

        public static void DestroyDebugger()
        {
            if (!Main.configSettings.Debug_Mode) return;

            if (_debugger != null)
            {
                _debugger.Destroy();
                _debugger = null;
            }
        }

        public static void DrawText(Vector2 position, string text)
        {
            if (!Main.configSettings.Debug_Mode) return;

            Helpers.DrawText(position, text, false, true, 0.35f, 255, 255, 255, 255, Font.Monospace);
        }

        public static void DrawText3D(Vector3 coords, Vector3 offset, string text)
        {
            if (!Main.configSettings.Debug_Mode) return;

            Helpers.DrawText3D(coords, offset, text, true, false, 0.35f, 255, 255, 255, 255, Font.Monospace);
        }

        public static void Subtitle(string text, int delay = 1000)
        {
            if (Main.configSettings.Debug_Mode) Screen.ShowSubtitle(text, delay);
        }

        public static void SetPlayerDebugKit()
        {
            Game.MaxWantedLevel = 5;
            Game.Player.WantedLevel = 0;
            Game.Player.IsInvincible = false;
            Game.Player.Character.IsInvincible = Game.Player.IsInvincible;
            Game.Player.Character.Weapons.Give(WeaponHash.Bat, 1, true, true);
            Game.Player.Character.Weapons.Give(WeaponHash.Crowbar, 1, true, true);
            Game.Player.Character.Weapons.Give(WeaponHash.Knife, 1, true, true);
            Game.Player.Character.Weapons.Give(WeaponHash.Grenade, 500, true, true);
            Game.Player.Character.Weapons.Give(WeaponHash.Pistol, 500, true, true);
            Game.Player.Character.Weapons.Give(WeaponHash.Railgun, 500, true, true);
            Game.Player.Character.Weapons.Give(WeaponHash.Minigun, 500, true, true);
            Game.Player.Character.Weapons.Give(WeaponHash.SniperRifle, 500, true, true);
            Game.Player.Character.Weapons.Give(WeaponHash.PipeBomb, 500, true, true);
            Game.Player.Character.Weapons.Give(WeaponHash.PetrolCan, 500, true, true);
            Game.Player.Character.Weapons.Give(WeaponHash.Parachute, 500, true, true);
            Game.Player.Character.Weapons.Give(WeaponHash.NightVision, 500, true, true);
            Game.Player.Character.Weapons.Give(WeaponHash.Molotov, 500, true, true);
            Game.Player.Character.Weapons.Give(WeaponHash.MicroSMG, 500, true, true);
            Game.Player.Character.Weapons.Give(WeaponHash.Hatchet, 500, true, true);
            Game.Player.Character.Weapons.Give(WeaponHash.GrenadeLauncher, 500, true, true);
            Game.Player.Character.Weapons.Give(WeaponHash.DoubleBarrelShotgun, 500, true, true);
            Game.Player.Character.Weapons.Give(WeaponHash.AssaultRifle, 500, true, true);
        }
    }
}
