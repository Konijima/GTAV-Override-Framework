using System;
using GTA;
using GTA.UI;
using GTA.Native;
using GTAVOverride.Managers;

namespace GTAVOverride.Scripts
{

    [ScriptAttributes(NoDefaultInstance = true)]
    public class PlayerDeathArrestScript : Script
    {
        private bool dead;
        private bool deathTriggered;
        private bool arrested;
        private bool arrestTriggered;

        public PlayerDeathArrestScript()
        {
            Pause();

            Interval = 333;
            dead = false;
            deathTriggered = false;
            arrested = false;
            arrestTriggered = false;

            Screen.StopEffects();

            Function.Call(Hash.SET_FADE_OUT_AFTER_DEATH, true);
            Function.Call(Hash.SET_FADE_OUT_AFTER_ARREST, true);
            Function.Call(Hash.SET_FADE_IN_AFTER_DEATH_ARREST, true);

            Tick += PlayerScript_Tick;
        }

        private void PlayerScript_Tick(object sender, EventArgs e)
        {
            dead = Game.Player.Character.IsDead;
            arrested = Function.Call<bool>(Hash.IS_PLAYER_BEING_ARRESTED, Game.Player.Handle, false);

            if (!dead && !arrested)
            {
                // player is back from death
                if (deathTriggered)
                {
                    TimeScaleManager.Target = 1f;
                    if (Main.configSettings.Hospital_Fee > 0)
                    {
                        Game.Player.Money -= Main.configSettings.Hospital_Fee;
                        if (Game.Player.Money < 0) Game.Player.Money = 0;
                    }
                    deathTriggered = false;
                    Screen.StopEffects();
                }

                // player is back from arrest
                if (arrestTriggered)
                {
                    TimeScaleManager.Target = 1f;
                    if (Main.configSettings.PoliceStation_Fee > 0)
                    {
                        Game.Player.Money -= Main.configSettings.PoliceStation_Fee;
                        if (Game.Player.Money < 0) Game.Player.Money = 0;
                    }
                    arrestTriggered = false;
                    Screen.StopEffects();
                }
            }

            // player just died
            if (dead && !deathTriggered)
            {
                TriggerDeath();
            }
            // player just got arrested
            else if (arrested && !arrestTriggered)
            {
                TriggerArrest();
            }
        }

        private void TriggerDeath()
        {
            deathTriggered = true;
            TimeScaleManager.Target = 0.3f;
            Audio.PlaySoundFrontend("Bed", "WastedSounds");
            World.RenderingCamera.Shake(CameraShake.DeathFail, 1f);
            Screen.StartEffect(ScreenEffect.DeathFailMpDark, 0, false);
        }

        private void TriggerArrest()
        {
            arrestTriggered = true;
            TimeScaleManager.Target = 0.3f;
            Audio.PlaySoundFrontend("Bed", "WastedSounds");
            World.RenderingCamera.Shake(CameraShake.DeathFail, 1f);
            Screen.StartEffect(ScreenEffect.DontTazemeBro, 0, false);
        }
    }
}
