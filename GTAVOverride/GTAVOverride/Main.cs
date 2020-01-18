using System;
using System.Collections.Generic;
using GTA;
using GTA.UI;
using GTA.Native;
using GTAVOverride.Managers;
using GTAVOverride.Scripts;
using GTAVOverride.Configs;

namespace GTAVOverride
{
    public class Main : Script
    {
        public static ConfigData configData;
        public static ConfigSettings configSettings;
        public static ConfigScripts configScripts;
        public static ConfigBlips configBlips;
        public static ConfigClock configClock;

        private KillScript _killScript;
        private List<Script> _scripts;

        public Main()
        {
            Helpers.StartLog();
            Helpers.Log("Mod is initializing...");

            configData = new ConfigData(Settings);
            configSettings = new ConfigSettings(Settings);
            configScripts = new ConfigScripts(Settings);
            configBlips = new ConfigBlips(Settings);
            configClock = new ConfigClock(Settings);

            Helpers.Log("Mod is preparing killscript...");
            _killScript = new KillScript();

            Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);

            _scripts = new List<Script>();
            if (configScripts.Atm_Script) _scripts.Add(InstantiateScript<AtmScript>());
            if (configScripts.ClockTime_Script) _scripts.Add(InstantiateScript<ClockTimeScript>());
            if (configScripts.Economy_Script) _scripts.Add(InstantiateScript<EconomyScript>());
            if (configScripts.Player_Death_Arrest_Script) _scripts.Add(InstantiateScript<PlayerDeathArrestScript>());
            if (configScripts.Player_Persistence_Script) _scripts.Add(InstantiateScript<PlayerPersistenceScript>());
            if (configScripts.Rob_People_Script && !configScripts.Play_Police_Script) _scripts.Add(InstantiateScript<RobPeopleScript>());
            if (configScripts.Store_Script) _scripts.Add(InstantiateScript<StoreScript>());
            if (configScripts.TimeScale_Script) _scripts.Add(InstantiateScript<TimeScaleScript>());
            if (configScripts.Play_Police_Script) _scripts.Add(InstantiateScript<PlayPoliceScript>());
            if (configScripts.Waypoint_Tools_Script) _scripts.Add(InstantiateScript<WaypointToolsScript>());

            Helpers.Log("Mod is starting update tick...");
            Tick += Main_Tick;
            Aborted += Main_Aborted;
        }

        private void Main_Tick(object sender, EventArgs e)
        {
            if (!Game.IsLoading && !_killScript.isStarted && !_killScript.isCompleted)
            {
                if (Screen.IsFadedIn)
                {
                    Screen.FadeOut(1);
                    Wait(1);
                }

                if (!configSettings.Dont_Kill_GTAV_Scripts)
                {
                    Helpers.Log("Waiting " + configSettings.Kill_GTAV_Scripts_Ms_Delay + "ms to start killscript...");
                    Wait(configSettings.Kill_GTAV_Scripts_Ms_Delay);

                    _killScript.Run();

                    if (configSettings.Kill_GTAV_Scripts_Only)
                    {
                        Helpers.Log("Kill Scripts only is enabled, not continuing mod initialization...");
                        Abort();
                    }
                }
                else _killScript.DontRun();

                Initialize();
            }
            else if (Game.IsLoading && _killScript.isCompleted)
            {
                Helpers.Log("Mod stopped due to loading screen re-openned!");
                Abort();
                return;
            }
        }

        private void Main_Aborted(object sender, EventArgs e)
        {
            Helpers.Log("Mod is aborting...");

            if (!configSettings.Dont_Kill_GTAV_Scripts)
                ClearAllBlips();
            else
            {
                AtmManager.DeleteATMBlips();
                StoreManager.DeleteStoreBlips();
                HospitalManager.DeleteHospitalBlips();
                PoliceStationManager.DeletePoliceStationBlips();
                AmmunationManager.DeleteAmmunationBlips();
            }

            Helpers.EndLog();
        }

        private void Initialize()
        {
            InitializeManagers();
            StartScripts();
            DebugInit();

            Helpers.Log("Mod has initialized!");
        }

        private void InitializeManagers()
        {
            Helpers.Log("Initializing managers...");


            DateTimeManager.SetMode(configClock.Clock_Mode);

            AmmunationManager.CreateAmmunations();
            if (!configBlips.Show_Ammunations) AmmunationManager.HideAmmunationBlips();

            AtmManager.CreateATMs();
            if (!configBlips.Show_Atm) AtmManager.HideATMBlips();

            DoorManager.CreateDoors();
            if (configSettings.Unlock_All_Doors) DoorManager.UnlockAll();

            HospitalManager.CreateDefaultHospitalBlips();
            if (!configBlips.Show_Hospitals) HospitalManager.HideHospitalBlips();
            if (configSettings.Hospital_Spawn_OnDeath) HospitalManager.EnableAllHospitals();
            else HospitalManager.DisableAllHospitals();

            PoliceStationManager.CreateDefaultPoliceStationBlips();
            if (!configBlips.Show_PoliceStations) PoliceStationManager.HidePoliceStationBlips();
            if (configSettings.PoliceStation_Spawn_OnArrest) PoliceStationManager.EnableAllPoliceStations();
            else PoliceStationManager.DisableAllPoliceStations();

            StoreManager.CreateStores();
            if (!configBlips.Show_Stores) StoreManager.HideStoreBlips();
        }

        private void DebugInit()
        {
            if (configSettings.Debug_Mode)
            {
                Game.MaxWantedLevel = 5;
                Game.Player.WantedLevel = 0;
                Game.Player.IsInvincible = false;
                Game.Player.Character.IsInvincible = Game.Player.IsInvincible;
                Game.Player.Character.Weapons.Give(WeaponHash.Bat, 1, true, false);
                Game.Player.Character.Weapons.Give(WeaponHash.Grenade, 500, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.Pistol, 500, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.AssaultRifle, 500, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.DoubleBarrelShotgun, 500, true, true);

                Helpers.Log("Debug mode activated!");
            }
            Game.Player.Character.Weapons.Give(WeaponHash.Pistol, 500, true, true);
        }

        private void StartScripts()
        {
            Helpers.Log("Starting scripts...");
            int startedCount = 0;
            foreach (Script script in _scripts)
            {
                if (script.IsRunning)
                {
                    script.Resume();
                    startedCount++;
                }
            }
            Helpers.Log("Started (" + startedCount + ") mod script(s).");
        }

        private void StopScripts()
        {
            Helpers.Log("Stopping scripts...");
            int stoppedCount = 0;
            foreach (Script script in _scripts)
            {
                if (script.IsRunning)
                {
                    script.Pause();
                    stoppedCount++;
                }
            }
            Helpers.Log("Stopped (" + stoppedCount + ") mod script(s).");
        }

        private void ClearAllBlips()
        {
            Helpers.Log("Clearing all blips...");

            Blip[] blips = World.GetAllBlips();
            foreach (Blip blip in blips)
            {
                blip.Delete();
            }
        }
    }
}