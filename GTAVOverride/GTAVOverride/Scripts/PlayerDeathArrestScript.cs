using System;
using System.Collections.Generic;
using System.Text;
using GTA;
using GTA.UI;
using GTA.Native;

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
                    if (Main.configSettings.Hospital_Fee > 0)
                    {
                        Game.Player.Money -= Main.configSettings.Hospital_Fee;
                        if (Game.Player.Money < 0) Game.Player.Money = 0;
                    }
                    deathTriggered = false;
                    Screen.StopEffects();
                }
                if (arrestTriggered)
                {
                    if (Main.configSettings.PoliceStation_Fee > 0)
                    {
                        Game.Player.Money -= Main.configSettings.PoliceStation_Fee;
                        if (Game.Player.Money < 0) Game.Player.Money = 0;
                    }
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
