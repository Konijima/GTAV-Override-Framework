using GTA;
using GTA.UI;
using GTA.Native;

namespace GTAVOverride.Scripts
{
    [ScriptAttributes(NoDefaultInstance = true)]
    class TimeScaleScript : Script
    {
        public static readonly float maximumTimescale = 1f;
        public static readonly float minimumTimescale = 0.01f;

        private float _target;
        private float _forced;

        public TimeScaleScript()
        {
            Pause();

            _target = 1f;
            current = 1f;

            Tick += TimescaleScript_Tick;
        }

        private void TimescaleScript_Tick(object sender, System.EventArgs e)
        {
            bool dead = Game.Player.Character.IsDead;
            bool arrested = Function.Call<bool>(Hash.IS_PLAYER_BEING_ARRESTED, Game.Player.Handle, false);

            if (arrested)
            {
                SetTimescaleThisFrame(0.4f);
            }

            if (dead)
            {
                SetTimescaleThisFrame(0.4f);
            }

            if (_forced > -1)
            {
                current = _forced;
                _forced = -1;
                return;
            }

            if (current < _target)
            {
                current += 0.01f;
            }
            else if (current > _target)
            {
                current -= 0.01f;
            }
        }

        public float current
        {
            get { return Game.TimeScale; }
            set
            {
                if (value > maximumTimescale) value = maximumTimescale;
                if (value < minimumTimescale) value = minimumTimescale;
                Game.TimeScale = value;
            }
        }

        public float target
        {
            get { return _target; }
            set
            {
                if (value > maximumTimescale) value = maximumTimescale;
                if (value < minimumTimescale) value = minimumTimescale;
                _target = value;
            }
        }

        public void SetTimescaleThisFrame(float timescale)
        {
            _forced = timescale;
        }
    }
}
