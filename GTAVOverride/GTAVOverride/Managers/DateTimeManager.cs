using System;
using GTA;
using GTA.Native;

namespace GTAVOverride.Managers
{
    public enum ClockMode
    {
        Sync = 0,
        Virtual,
        Vanilla,
    }

    public static class DateTimeManager
    {
        private static bool _dirty = false;
        private static ClockMode clockMode = ClockMode.Vanilla;
        private static DateTime date = DateTime.Now;
        private static float timerate = 0.5f;
        private static bool frozen = false;

        public static ClockMode CurrentClockMode
        {
            get
            {
                return clockMode;
            }
        }

        public static DateTime CurrentDate
        {
            get
            {
                if (clockMode != ClockMode.Vanilla)
                    return date;
                else return World.CurrentDate;
            }
        }

        public static TimeSpan CurrentTime
        {
            get
            {
                if (clockMode != ClockMode.Vanilla)
                    return date.TimeOfDay;
                else return World.CurrentTimeOfDay;
            }
        }

        public static float CurrentTimerate
        {
            get
            {
                return timerate;
            }
        }

        public static bool Frozen
        {
            get
            {
                return frozen;
            }
        }

        public static void Freeze(bool toggle)
        {
            if (frozen != toggle) _dirty = true;
            frozen = toggle;
        }

        public static void SetMode(ClockMode mode)
        {
            if (clockMode != mode) _dirty = true;
            clockMode = mode;
        }

        public static void SetTimerate(float rate)
        {
            if (timerate != rate) _dirty = true;
            timerate = rate;
        }

        public static void SetDate(DateTime d)
        {
            date = new DateTime(date.Year, date.Month, date.Day);
            _dirty = true;
        }

        public static void SetTime(TimeSpan t)
        {
            date = new DateTime(date.Year, date.Month, date.Day, t.Hours, t.Minutes, t.Seconds);
            _dirty = true;
        }

        public static void AddSeconds(int seconds)
        {
            date = date.AddSeconds(seconds);
            _dirty = true;
        }

        public static void UpdateGameClock()
        {
            if (Main.configSettings.Debug_Mode)
            {
                Helpers.DrawText(new GTA.Math.Vector2(0.015f, 0.01f), "Clock: " + CurrentDate.ToString());
                Helpers.DrawText(new GTA.Math.Vector2(0.015f, 0.03f), "Mode : " + clockMode.ToString());
                if (clockMode == ClockMode.Virtual) Helpers.DrawText(new GTA.Math.Vector2(0.015f, 0.05f), "Timerate : " + timerate.ToString());
            }

            if (_dirty)
            {
                Function.Call(Hash.PAUSE_CLOCK, frozen);
                Function.Call(Hash.SET_CLOCK_DATE, date.Day, date.Month, date.Year);
                Function.Call(Hash.SET_CLOCK_TIME, date.Hour, date.Minute, date.Second);
                _dirty = false;
            }
        }
    }
}
