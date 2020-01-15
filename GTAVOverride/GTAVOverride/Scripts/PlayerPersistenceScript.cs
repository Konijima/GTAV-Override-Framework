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

        private bool deathTrigger;
        private int lastMoney;

        private bool _inited = false;

        public PlayerPersistenceScript()
        {
            Pause();

            PlayerManager.SetSavePath(Main.configData.Save_Directory_Path, Main.configData.Save_Player_Filename);

            saveDelay = Main.configData.Auto_Save_Delay;
            if (saveDelay < 1000) saveDelay = 1000;

            Tick += PersistenceScript_Tick;
        }

        private void ShowSpinner(int delay = 2200)
        {
            Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "STRING");
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
                ped.Speed < 10f)
            {
                if (!Main.configData.Auto_Save_When_Wanted && Game.Player.WantedLevel > 0) return false;
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

                Wait(500);

                lastMoney = Game.Player.Money;

                lastSave = Game.GameTime;

                Helpers.DebugSubtitle("Game has loaded!");

                if (Screen.IsFadedOut)
                {
                    Screen.FadeIn(1000);
                    while (Screen.IsFadingIn) Wait(1);
                }
            }

            if (_inited && PlayerManager.HasLoadedGame())
            {
                if (Game.Player.Character.IsDead && !deathTrigger) deathTrigger = true;

                if (Main.configData.Auto_Save_Enabled)
                {
                    if (Main.configData.Auto_Save_On_Transaction)
                    {
                        if (Game.Player.Money != lastMoney)
                        {
                            lastMoney = Game.Player.Money;
                            lastSave = Game.GameTime;
                            PlayerManager.Save();
                            Helpers.DebugSubtitle("Saving transaction...", 3000);
                            ShowSpinner();
                        }
                    }

                    if (Main.configData.Auto_Save_After_DeathArrest)
                    {
                        if (!Game.Player.Character.IsDead && deathTrigger && Screen.IsFadedIn)
                        {
                            deathTrigger = false;
                            lastSave = Game.GameTime;
                            PlayerManager.Save();
                            Helpers.DebugSubtitle("Saving respawned...", 3000);
                            ShowSpinner();
                        }
                    }

                    if (CanSave() && !deathTrigger)
                    {
                        lastSave = Game.GameTime;
                        PlayerManager.Save();
                        Helpers.DebugSubtitle("Auto-saving...");
                        ShowSpinner();
                    }
                }
            }
        }
    }
}
