using GTA;
using GTA.UI;
using GTA.Native;
using GTAVOverride.Managers;

namespace GTAVOverride.Scripts
{
    [ScriptAttributes(NoDefaultInstance = true)]
    class TimeScaleScript : Script
    {
        public TimeScaleScript()
        {
            Pause();

            TimeScaleManager.Target = 1f;
            TimeScaleManager.Current = 1f;

            Tick += TimescaleScript_Tick;
        }

        private void TimescaleScript_Tick(object sender, System.EventArgs e)
        {
            // Forced TimeScale for a single frame
            if (TimeScaleManager.Forced > -1)
            {
                TimeScaleManager.Current = TimeScaleManager.Forced;
                TimeScaleManager.Forced = -1; // we reset to -1 wich disable it
                return;
            }

            if (TimeScaleManager.Current < TimeScaleManager.Target)
            {
                TimeScaleManager.Current += 1f * TimeScaleManager.Speed * Game.LastFrameTime;
            }
            else if (TimeScaleManager.Current > TimeScaleManager.Target)
            {
                TimeScaleManager.Current -= 1f * TimeScaleManager.Speed * Game.LastFrameTime;
            }
        }
    }
}
