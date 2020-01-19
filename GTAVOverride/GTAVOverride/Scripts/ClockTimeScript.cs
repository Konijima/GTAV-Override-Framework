using System;
using GTA;
using GTA.Math;
using GTA.UI;
using GTAVOverride.Enums;
using GTAVOverride.Managers;

namespace GTAVOverride.Scripts
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class ClockTimeScript : Script
    {
        private int _timer = 0;
        private float _clockScroll = 0;

        public ClockTimeScript()
        {
            Pause();

            DateTimeManager.SetTimerate(Main.configClock.Virtual_Timerate);

            Tick += ClockScripts_Tick;
        }

        private void ClockScripts_Tick(object sender, EventArgs e)
        {
            if (Game.IsPaused || Game.IsLoading) return;

            string time = DateTimeManager.GetTimeString();
            if (Game.Player.CanControlCharacter && Game.IsControlPressed(Control.CharacterWheel))
            {
                if (Main.configSettings.Show_Hud_OnKeyDown) Hud.IsRadarVisible = true;
                _clockScroll += 1 * Game.LastFrameTime;
                if (_clockScroll > 0.02f) _clockScroll = 0.02f;
            }
            else
            {
                _clockScroll -= 1 * Game.LastFrameTime;
                if (_clockScroll < -0.2f)
                {
                    _clockScroll = -0.2f;
                    if (Main.configSettings.Show_Hud_OnKeyDown) Hud.IsRadarVisible = false;
                }
            }
            if (Main.configSettings.Show_Hud_OnKeyDown && Hud.IsRadarVisible)
            {
                Helpers.DrawText(new Vector2(0.5f, _clockScroll), time, true, true, 1.068f, 0, 0, 0, 200, Font.ChaletComprimeCologne);
                Helpers.DrawText(new Vector2(0.5f, _clockScroll), time, true, true, 1.066f, 240, 240, 240, 255, Font.ChaletComprimeCologne);
            }
            else
            {
                Helpers.DrawText(new Vector2(0.5f, _clockScroll), time, true, true, 1.068f, 0, 0, 0, 200, Font.ChaletComprimeCologne);
                Helpers.DrawText(new Vector2(0.5f, _clockScroll), time, true, true, 1.066f, 240, 240, 240, 255, Font.ChaletComprimeCologne);
            }

            // Sync with machine
            if (DateTimeManager.CurrentClockMode == ClockMode.Sync)
            {
                DateTimeManager.Freeze(true);
                DateTimeManager.SetDate(DateTime.Now);
                DateTimeManager.SetTime(DateTime.Now.TimeOfDay);
            }
            // Vanilla game time
            else if (DateTimeManager.CurrentClockMode == ClockMode.Vanilla)
            {
                DateTimeManager.Freeze(false);
            }
            // Virtual game time
            else
            {
                DateTimeManager.Freeze(true);

                if (Game.GameTime > _timer)
                {
                    _timer = Game.GameTime + (int)Math.Round(1000 * DateTimeManager.CurrentTimerate);
                    DateTimeManager.AddSeconds(1);
                }
            }

            // Update Clock
            DateTimeManager.UpdateGameClock();
        }
    }
}
