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

        private readonly KillScript _killScript = new KillScript();
        private readonly List<Script> _scripts = new List<Script>();

        public Main()
        {
            configData = new ConfigData(Settings);
            configSettings = new ConfigSettings(Settings);
            configScripts = new ConfigScripts(Settings);
            configBlips = new ConfigBlips(Settings);
            configClock = new ConfigClock(Settings);

            Debug.InitLog();
            Debug.Log("Mod is initializing...");

            if (configScripts.Atm_Script) _scripts.Add(InstantiateScript<AtmScript>());
            if (configScripts.ClockTime_Script) _scripts.Add(InstantiateScript<ClockTimeScript>());
            if (configScripts.Economy_Script) _scripts.Add(InstantiateScript<EconomyScript>());
            if (configScripts.People_Money_Script) _scripts.Add(InstantiateScript<PeopleMoneyScript>());
            if (configScripts.Player_Death_Arrest_Script) _scripts.Add(InstantiateScript<PlayerDeathArrestScript>());
            if (configScripts.Player_Persistence_Script) _scripts.Add(InstantiateScript<PlayerPersistenceScript>());
            if (configScripts.Rob_People_Script && !configScripts.Play_Police_Script) _scripts.Add(InstantiateScript<RobPeopleScript>());
            if (configScripts.Store_Script) _scripts.Add(InstantiateScript<StoreScript>());
            if (configScripts.TimeScale_Script) _scripts.Add(InstantiateScript<TimeScaleScript>());
            if (configScripts.Play_Police_Script) _scripts.Add(InstantiateScript<PlayPoliceScript>());
            if (configScripts.Waypoint_Tools_Script) _scripts.Add(InstantiateScript<WaypointToolsScript>());

            Debug.Log("Mod is starting update tick...");
            Tick += Main_Tick;
            Aborted += Main_Aborted;
        }

        private void Main_Tick(object sender, EventArgs e)
        {
            if (!Game.IsLoading && !_killScript.Started && !_killScript.Completed)
            {
                if (Screen.IsFadedIn)
                {
                    Screen.FadeOut(1);
                    Wait(1);
                }

                if (!configSettings.Dont_Kill_GTAV_Scripts)
                {
                    Debug.Log("Waiting " + configSettings.Kill_GTAV_Scripts_Ms_Delay + "ms to start killscript...");
                    Wait(configSettings.Kill_GTAV_Scripts_Ms_Delay);

                    _killScript.Run();

                    if (configSettings.Kill_GTAV_Scripts_Only)
                    {
                        Debug.Log("Kill Scripts only is enabled, not continuing mod initialization...");
                        Abort();
                    }
                }
                else _killScript.DontRun();

                Initialize();
            }
            else if (Game.IsLoading && _killScript.Completed)
            {
                Debug.Log("Mod stopped due to loading screen re-openned!");
                Abort();
                return;
            }
        }

        private void Main_Aborted(object sender, EventArgs e)
        {
            Debug.Log("Mod is aborting...");

            if (!configSettings.Dont_Kill_GTAV_Scripts)
                Helpers.ClearAllBlips();
            else
            {
                AtmManager.DeleteATMBlips();
                StoreManager.DeleteStoreBlips();
                HospitalManager.DeleteHospitalBlips();
                PoliceStationManager.DeletePoliceStationBlips();
                AmmunationManager.DeleteAmmunationBlips();
            }

            Debug.EndLog();
            Debug.DestroyDebugger();
        }

        private void Initialize()
        {
            InitializeManagers();
            StartScripts();
            DebugInit();

            Debug.Log("Mod has initialized!");
        }

        private void InitializeManagers()
        {
            Debug.Log("Initializing managers...");

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
            Debug.CreateDebugger();

            if (configSettings.Debug_Mode)
            {
                Debug.SetPlayerDebugKit();

                Debug.Log("Debug mode activated!");
            }
        }

        private void StartScripts()
        {
            Debug.Log("Starting scripts...");
            int startedCount = 0;
            foreach (Script script in _scripts)
            {
                if (script.IsRunning)
                {
                    script.Resume();
                    startedCount++;
                }
            }
            Debug.Log("Started (" + startedCount + ") mod script(s).");
        }

        private void StopScripts()
        {
            Debug.Log("Stopping scripts...");
            int stoppedCount = 0;
            foreach (Script script in _scripts)
            {
                if (script.IsRunning)
                {
                    script.Pause();
                    stoppedCount++;
                }
            }
            Debug.Log("Stopped (" + stoppedCount + ") mod script(s).");
        }
    }
}