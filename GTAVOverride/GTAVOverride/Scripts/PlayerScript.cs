using System;
using System.Collections.Generic;
using System.Text;
using GTA;
using GTA.UI;
using GTA.Native;

namespace GTAVOverride.Scripts
{

    [ScriptAttributes(NoDefaultInstance = true)]
    public class PlayerScript : Script
    {
        private bool dead;
        private bool deathTriggered;
        private bool arrested;
        private bool arrestTriggered;

        public PlayerScript()
        {
            Interval = 128;

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
                if (deathTriggered)
                {
                    deathTriggered = false;
                    Screen.StopEffects();
                }
                if (arrestTriggered)
                {
                    arrestTriggered = false;
                    Screen.StopEffects();
                }
            }

            if (dead && !deathTriggered)
            {
                TriggerDeath();
            }
            else if (arrested && !arrestTriggered)
            {
                TriggerArrest();
            }
        }

        private void TriggerDeath()
        {
            deathTriggered = true;
            Audio.PlaySoundFrontend("Bed", "WastedSounds");
            World.RenderingCamera.Shake(CameraShake.DeathFail, 1f);
            Screen.StartEffect(ScreenEffect.DeathFailMpDark, 0, false);
        }

        private void TriggerArrest()
        {
            arrestTriggered = true;
            Audio.PlaySoundFrontend("Bed", "WastedSounds");
            World.RenderingCamera.Shake(CameraShake.DeathFail, 1f);
            Screen.StartEffect(ScreenEffect.DontTazemeBro, 0, false);
        }
    }
}
