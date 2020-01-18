using System;
using GTA;
using GTAVOverride.Enums;
using GTAVOverride.Managers;

namespace GTAVOverride.Scripts
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class ClockTimeScript : Script
    {
        private int _timer = 0;

        public ClockTimeScript()
        {
            Pause();

            DateTimeManager.SetTimerate(Main.configClock.Virtual_Timerate);

            Tick += ClockScripts_Tick;
        }

        private void ClockScripts_Tick(object sender, EventArgs e)
        {
            if (Game.IsPaused || Game.IsLoading) return;

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
