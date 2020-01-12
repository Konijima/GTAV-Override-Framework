using System;
using System.Collections.Generic;
using System.Text;
using GTA;
using GTA.UI;
using GTA.Native;
using GTAVOverride.Managers;
using GTAVOverride.Scripts;

namespace GTAVOverride
{
    public class Main : Script
    {
        private bool _init;

        private List<Script> scripts = new List<Script>();

        public Main()
        {
            Screen.ShowHelpTextThisFrame("");

            if (!Settings.GetValue<bool>("KILLSCRIPT", "KILLONLY", false))
            {
                if (Settings.GetValue<bool>("SCRIPTS", "PLAYERSCRIPT", true))
                {
                    scripts.Add(Script.InstantiateScript<PlayerScript>());
                }
                if (Settings.GetValue<bool>("SCRIPTS", "CLOCKSCRIPT", true))
                {
                    scripts.Add(Script.InstantiateScript<ClockScript>());
                }
                if (Settings.GetValue<bool>("SCRIPTS", "TIMESCALESCRIPT", true))
                {
                    scripts.Add(Script.InstantiateScript<TimescaleScript>());
                }
                if (Settings.GetValue<bool>("SCRIPTS", "ECONOMYSCRIPT", true))
                {
                    scripts.Add(Script.InstantiateScript<EconomyScript>());
                }
                if (Settings.GetValue<bool>("SCRIPTS", "WAYPOINTSCRIPT", true))
                {
                    scripts.Add(Script.InstantiateScript<WaypointScript>());
                }

                Tick += Main_Tick;
                Aborted += Main_Aborted;
            }
        }
        
        private void Main_Tick(object sender, EventArgs e)
        {
            if (!Game.IsPaused && !Game.IsLoading)
            {
                if (!_init)
                {
                    Init();
                }

                if (Game.Player.Character.IsShooting)
                {
                    Entity entity = Game.Player.TargetedEntity;
                    if (entity != null)
                    {
                        entity.IsPositionFrozen = false;
                        Settings.SetValue("ENTITYHASH | X | Y | Z | HEADING", entity.Model.Hash.ToString(), entity.Position.X + "f, " + entity.Position.Y + "f, " + entity.Position.Z + "f, " + entity.Heading + "f");
                        Settings.Save();
                    }
                }

                foreach(Script script in scripts)
                {
                    if (script.IsPaused)
                    {
                        script.Resume();
                    }
                }
            }
            else
            {
                foreach (Script script in scripts)
                {
                    if (script.IsExecuting)
                    {
                        script.Pause();
                    }
                }
            }
        }

        private void Init()
        {
            _init = true;

            Game.MaxWantedLevel = 0;
            Game.Player.IsInvincible = true;
            Game.Player.Character.Weapons.Give(WeaponHash.Bat, 1, true, false);
            Game.Player.Character.Weapons.Give(WeaponHash.Pistol, 500, true, true);

            foreach (Script script in scripts)
            {
                // ClockScript Init
                if (script.GetType() == typeof(ClockScript))
                {
                    ((ClockScript)script).SetMode(ClockMode.Virtual);
                }
            }

            // DOORS
            DoorManager.CreateDoors();
            if (Settings.GetValue<bool>("DOORS", "ALL_UNLOCKED", true))
            {
                DoorManager.UnlockAll();
            }
            if (Settings.GetValue<bool>("DOORS", "ALL_LOCKED", true))
            {
                DoorManager.LockAll();
            }

            AmmunationManager.CreateAmmunations();
            if (!Settings.GetValue<bool>("BLIPS", "AMMUNATIONS", true))
            {
                AmmunationManager.HideAmmunationBlips();
            }

            AtmManager.CreateATMs();
            if (!Settings.GetValue<bool>("BLIPS", "ATMS", true))
            {
                AtmManager.HideATMBlips();
            }

            StoreManager.CreateStores();
            if (!Settings.GetValue<bool>("BLIPS", "STORES", true))
            {
                StoreManager.HideStoreBlips();
            }

            HospitalManager.CreateDefaultHospitalBlips();
            if (!Settings.GetValue<bool>("BLIPS", "HOSPITALS", true))
            {
                HospitalManager.HideHospitalBlips();
            }

            PoliceStationManager.CreateDefaultPoliceStationBlips();
            if (!Settings.GetValue<bool>("BLIPS", "POLICESTATIONS", true))
            {
                PoliceStationManager.HidePoliceStationBlips();
            }

            HospitalManager.EnableAllHospitals();
            if (!Settings.GetValue<bool>("SETTINGS", "HOSPITAL_SPAWNS", true))
            {
                HospitalManager.DisableAllHospitals();
            }

            PoliceStationManager.EnableAllPoliceStations();
            if (!Settings.GetValue<bool>("SETTINGS", "POLICESTATION_SPAWNS", true))
            {
                PoliceStationManager.DisableAllPoliceStations();
            }

            // firestation: 207.2706f, -1649.573f, 29.8032f
            // CreateFleeca(new Vector3(150.12f, -1040.139f, 29.37409f), 149.2541f);
        }

        private void Main_Aborted(object sender, EventArgs e)
        {
            // delete all blips
            Blip[] blips = World.GetAllBlips();
            foreach(Blip blip in blips)
            {
                blip.Delete();
            }
        }
    }
}