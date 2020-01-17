using System;
using System.Collections.Generic;
using System.Text;
using GTA;
using GTA.UI;
using GTA.Native;
using GTAVOverride.Managers;
using GTA.Math;

namespace GTAVOverride.Scripts
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class PlayerPersistenceScript : Script
    {
        private int lastSave = 0;
        private int saveDelay = 5000;

        private bool vehicleTrigger;
        private bool deathTrigger;
        private bool wantedTrigger;
        private int lastMoney;

        private bool _inited = false;

        public PlayerPersistenceScript()
        {
            Pause();

            if (Function.Call<bool>(Hash.BUSYSPINNER_IS_DISPLAYING)) Function.Call(Hash.BUSYSPINNER_OFF);

            PlayerManager.SetSavePath(Main.configData.Save_Directory_Path, Main.configData.Save_Player_Filename);

            saveDelay = Main.configData.Auto_Save_Delay;
            if (saveDelay < 1000) saveDelay = 1000;

            Tick += PersistenceScript_Tick;
            Aborted += PlayerPersistenceScript_Aborted;
        }

        private void PlayerPersistenceScript_Aborted(object sender, EventArgs e)
        {
            if (Game.Player.Character.IsInVehicle())
            {
                Game.Player.Character.Task.WarpOutOfVehicle(Game.Player.Character.CurrentVehicle);
            }
            if (PlayerManager.CurrentPlayerVehicle != null)
            {
                PlayerManager.CurrentPlayerVehicle.IsPersistent = true;
                PlayerManager.CurrentPlayerVehicle.Delete();
            }
        }

        private void ShowSpinner(string text = "", int delay = 2000)
        {
            Function.Call(Hash.BUSYSPINNER_OFF);
            Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "STRING");

            if (Main.configData.Auto_Save_Show_Spinner_text) Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);

            Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, LoadingSpinnerType.SocialClubSaving);

            Wait(delay);

            Function.Call(Hash.BUSYSPINNER_OFF);
        }

        private bool CanSave()
        {
            Player player = Game.Player;
            Ped ped = Game.Player.Character;

            if (PlayerManager.HasLoadedGame() &&
                Main.configData.Auto_Save_Enabled &&
                Game.GameTime > lastSave + saveDelay &&
                !Game.IsLoading &&
                !Game.IsPaused &&
                !Game.IsMissionActive &&
                ped.IsAlive &&
                !ped.IsRagdoll &&
                !ped.IsSwimming &&
                !ped.IsSwimmingUnderWater &&
                !ped.IsInMeleeCombat &&
                !ped.IsOnFire &&
                ped.Speed < 15f)
            {
                if (!Main.configData.Auto_Save_In_Vehicle && Game.Player.Character.IsSittingInVehicle()) return false;

                return true;
            }
            return false;
        }

        private void PersistenceScript_Tick(object sender, EventArgs e)
        {
            if (!_inited && !Game.IsLoading)
            {
                _inited = true;

                PlayerManager.Load();

                lastMoney = Game.Player.Money;
                lastSave = Game.GameTime;
            }

            if (_inited && PlayerManager.HasLoadedGame())
            {
                if (Game.Player.Character.IsDead && !deathTrigger) deathTrigger = true;
                if (Game.Player.WantedLevel > 0 && !wantedTrigger) wantedTrigger = true;
                
                if (Game.Player.Character.IsInVehicle() && !vehicleTrigger)
                {
                    PlayerManager.ReplacePersonalVehicle(Game.Player.Character.LastVehicle);
                    vehicleTrigger = true;
                }

                if (vehicleTrigger && !Game.Player.Character.IsInVehicle())
                {
                    PlayerManager.ReplacePersonalVehicle(Game.Player.Character.LastVehicle);
                    vehicleTrigger = false;
                }
                else
                {
                    PlayerManager.ReplacePersonalVehicle(Game.Player.Character.CurrentVehicle);
                }

                if (Main.configData.Auto_Save_Enabled)
                {
                    if (!Main.configData.Auto_Save_When_Wanted)
                    {
                        if (Game.Player.WantedLevel > 0)
                        {
                            return;
                        }
                        else
                        {
                            if (wantedTrigger)
                            {
                                wantedTrigger = false;
                                lastSave = Game.GameTime;
                                PlayerManager.Save();
                                Helpers.DebugSubtitle("Autosaving after loosing cops...", 1000);
                                ShowSpinner("Autosaving");
                            }
                        }
                    }

                    // auto-save after transaction
                    if (Main.configData.Auto_Save_On_Transaction)
                    {
                        if (Game.Player.Money != lastMoney)
                        {
                            lastMoney = Game.Player.Money;
                            lastSave = Game.GameTime;
                            PlayerManager.Save();
                            Helpers.DebugSubtitle("Autosaving transaction...", 1000);
                            ShowSpinner("Autosaving");
                        }
                    }

                    // auto-save after death/arrest
                    if (Main.configData.Auto_Save_After_DeathArrest)
                    {
                        if (!Game.Player.Character.IsDead && deathTrigger && Screen.IsFadedIn)
                        {
                            deathTrigger = false;
                            lastSave = Game.GameTime;
                            PlayerManager.Save();
                            Helpers.DebugSubtitle("Autosaving after respawn...", 1000);
                            ShowSpinner("Autosaving");
                        }
                    }

                    // auto-save with delay
                    if (CanSave() && !deathTrigger)
                    {
                        lastSave = Game.GameTime;
                        PlayerManager.Save();
                        Helpers.DebugSubtitle("Autosaving...", 1000);
                        ShowSpinner("Autosaving");
                    }
                }
            }
        }
    }
}
