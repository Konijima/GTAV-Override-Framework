using System;
using GTA;
using GTA.Native;

namespace GTAVOverride.Scripts
{
    public enum ClockMode
    {
        Sync = 0,
        Virtual,
        Vanilla,
    }

    [ScriptAttributes(NoDefaultInstance = true)]
    public class ClockTimeScript : Script
    {

        private ClockMode _mode;
        private float _timerate = 1f;
        private int _timer = 0;

        private bool _inited = false;

        public ClockTimeScript()
        {
            Pause();

            Tick += ClockScripts_Tick;
        }

        public void Freeze(bool toggle)
        {
            Function.Call(Hash.PAUSE_CLOCK, toggle);
        }

        public void SetMode(ClockMode mode)
        {
            _mode = mode;
        }

        public void SetTimerate(float timerate)
        {
            _timerate = timerate;
        }

        private void ClockScripts_Tick(object sender, EventArgs e)
        {
            if (!_inited) Init();

            if (_inited)
            {
                if (_mode == ClockMode.Sync)
                {
                    date = DateTime.Now;
                    time = date.TimeOfDay;
                }
                else if (_mode == ClockMode.Vanilla) Freeze(false);
                else
                {
                    Freeze(true);
                    if (!Game.IsPaused && Game.GameTime > _timer)
                    {
                        _timer = Game.GameTime + (int)Math.Round(1000 * _timerate);
                        Function.Call(Hash.ADD_TO_CLOCK_TIME, 0, 0, 1);
                    }
                }
            }
        }

        private void Init()
        {
            _inited = true;
        }

        public DateTime date
        {
            get
            {
                return World.CurrentDate;
            }
            set
            {
                World.CurrentDate = value;
            }
        }

        public TimeSpan time
        {
            get
            {
                return World.CurrentTimeOfDay;
            }
            set
            {
                World.CurrentTimeOfDay = value;
            }
        }
    }
}
