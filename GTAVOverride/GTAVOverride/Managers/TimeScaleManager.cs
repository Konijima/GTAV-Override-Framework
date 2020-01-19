using GTA;
using System;
using System.Collections.Generic;
using System.Text;

namespace GTAVOverride.Managers
{
    public static class TimeScaleManager
    {
        public static readonly float Maximum = 1f;
        public static readonly float Minimum = 0.01f;

        private static float _current = 1f;
        private static float _target = 1f;
        private static float _forced = -1f;
        private static float _speed = 1f;

        public static float Current
        {
            get
            {
                return _current;
            }
            set
            {
                if (value < Minimum) value = Minimum;
                if (value > Maximum) value = Maximum;
                _current = value;
                Game.TimeScale = _current;
            }
        }

        public static float Target
        {
            get
            {
                return _target;
            }
            set
            {
                if (value < Minimum) value = Minimum;
                if (value > Maximum) value = Maximum;
                _target = value;
            }
        }

        public static float Forced
        {
            get
            {
                return _forced;
            }
            set
            {
                if (value < -1) value = -1;
                if (value > Maximum) value = Maximum;
                _forced = value;
            }
        }

        public static float Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                if (value < 1) value = 1;
                if (value > 3) value = 3;
                _speed = value;
            }
        }

        public static void DebugThisFrame()
        {
            Debug.DrawText(new GTA.Math.Vector2(0.015f, 0.3f), "Timescale: " + Current.ToString());
        }
    }
}
