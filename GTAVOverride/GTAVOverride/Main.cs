using System;
using System.Collections.Generic;
using GTA;
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

        private bool _initialized;
        private KillScript _killScript;
        private List<Script> _scripts;

        public Main()
        {
            configData = new ConfigData(Settings);
            configSettings = new ConfigSettings(Settings);
            configScripts = new ConfigScripts(Settings);
            configBlips = new ConfigBlips(Settings);

            _initialized = false;
            _scripts = new List<Script>();

            if (configScripts.Atm_Script) _scripts.Add(InstantiateScript<AtmScript>());
            if (configScripts.ClockTime_Script) _scripts.Add(InstantiateScript<ClockTimeScript>());
            if (configScripts.Economy_Script) _scripts.Add(InstantiateScript<EconomyScript>());
            if (configScripts.Player_Death_Arrest_Script) _scripts.Add(InstantiateScript<PlayerDeathArrestScript>());
            if (configScripts.Player_Persistence_Script) _scripts.Add(InstantiateScript<PlayerPersistenceScript>());
            if (configScripts.Rob_People_Script) _scripts.Add(InstantiateScript<RobPeopleScript>());
            if (configScripts.Store_Script) _scripts.Add(InstantiateScript<StoreScript>());
            if (configScripts.TimeScale_Script) _scripts.Add(InstantiateScript<TimeScaleScript>());
            if (configScripts.Waypoint_Tools_Script) _scripts.Add(InstantiateScript<WaypointToolsScript>());

            Tick += Main_Tick;
            Aborted += Main_Aborted;
        }

        private void Main_Tick(object sender, EventArgs e)
        {
            if (!Game.IsLoading && _killScript == null)
            {
                GTA.UI.Screen.FadeOut(1);
                _killScript = InstantiateScript<KillScript>();

                if (configSettings.Kill_GTAV_Scripts_Only)
                {
                    Helpers.DebugSubtitle("Killing GTAV Scripts Only!", 1000);
                    Wait(2000);
                    Abort();
                    return;
                }
                else Helpers.DebugSubtitle("Killing GTAV Scripts...", 1000);
            }

            if (Game.IsLoading)
            {
                if (_initialized) Abort();
                return;
            }

            if (_killScript != null && _initialized == false)
            {
                InitializeManagers();
                Wait(1000);
                StartScripts();
                Wait(1000);
                DebugInit();
                _initialized = true;
            }
        }

        private void Main_Aborted(object sender, EventArgs e)
        {
            StopScripts();
            ClearAllBlips();
        }

        private void DebugInit()
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
        }

        private void StartScripts()
        {
            int startedCount = 0;
            foreach (Script script in _scripts)
            {
                if (script.IsRunning)
                {
                    script.Resume();
                    startedCount++;
                }
            }
            Helpers.DebugSubtitle("Started " + startedCount + " script(s).", 1000);
        }

        private void StopScripts()
        {
            int stoppedCount = 0;
            foreach (Script script in _scripts)
            {
                if (script.IsRunning)
                {
                    script.Pause();
                    stoppedCount++;
                }
            }
            Helpers.DebugSubtitle("Stopped " + stoppedCount + " script(s).", 1000);
        }

        private void InitializeManagers()
        {
            foreach (Script script in _scripts)
            {
                // ClockScript Init
                if (script.GetType() == typeof(ClockTimeScript))
                {
                    ((ClockTimeScript)script).SetMode(configSettings.Clock_Mode);
                }
            }

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

        private void ClearAllBlips()
        {
            PlayerManager.Save();
            Wait(1000);

            Blip[] blips = World.GetAllBlips();
            foreach (Blip blip in blips)
            {
                blip.Delete();
            }
        }
    }
}