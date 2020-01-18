using System;
using System.Collections.Generic;
using System.Text;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using GTAVOverride.Managers;

namespace GTAVOverride.Scripts
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class PlayerPersistenceScript : Script
    {
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

        private void PersistenceScript_Tick(object sender, EventArgs e)
        {
            if (!_inited && !Game.IsLoading)
            {
                _inited = true;

                PlayerManager.Load();

                lastMoney = Game.Player.Money;
            }

            if (_inited && PlayerManager.HasLoadedGame())
            {
                if (Game.Player.Character.IsDead && !deathTrigger) deathTrigger = true;
                if (Game.Player.WantedLevel > 0 && !wantedTrigger) wantedTrigger = true;
                
                if (Main.configSettings.Debug_Mode)
                {
                    if (PlayerManager.CanSave(false)) Helpers.DrawText(new Vector2(0.015f, 0.08f), "Can auto-save");
                }

                if (Game.Player.Character.IsInVehicle() && !vehicleTrigger)
                {
                    PlayerManager.ReplacePersonalVehicle(Game.Player.Character.CurrentVehicle);
                    vehicleTrigger = true;
                }

                if (vehicleTrigger && !Game.Player.Character.IsInVehicle())
                {
                    PlayerManager.LeavePersonalVehicle();
                    vehicleTrigger = false;
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
                            PlayerManager.Save();
                            Helpers.DebugSubtitle("Autosaving after respawn...", 1000);
                            ShowSpinner("Autosaving");
                        }
                    }

                    // auto-save with delay
                    if (PlayerManager.CanSave() && !deathTrigger)
                    {
                        PlayerManager.Save();
                        Helpers.DebugSubtitle("Autosaving...", 1000);
                        ShowSpinner("Autosaving");
                    }
                }
            }
        }
    }
}
