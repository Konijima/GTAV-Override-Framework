using GTA;
using GTAVOverride.Enums;

namespace GTAVOverride.Configs
{
    public class ConfigClock
    {
        private readonly string _section;
        private ScriptSettings _settings;

        private float _Virtual_Timerate = 0.5f;
        private bool _Show_Seconds = false;
        private bool _Military_Time_Format = true;
        private ClockMode _Clock_Mode = ClockMode.Virtual;

        public ConfigClock(ScriptSettings settings)
        {
            _section = "CLOCK";
            _settings = settings;

            Load();
            Save();
        }

        public void Save()
        {
            Virtual_Timerate = _Virtual_Timerate;
            Show_Seconds = _Show_Seconds;
            Military_Time_Format = _Military_Time_Format;
            Clock_Mode = _Clock_Mode;

            _settings.Save();
        }

        public void Load()
        {
            _Virtual_Timerate = _settings.GetValue(_section, "Virtual_Timerate", _Virtual_Timerate);
            _Show_Seconds = _settings.GetValue(_section, "Show_Seconds", _Show_Seconds);
            _Military_Time_Format = _settings.GetValue(_section, "Military_Time_Format", _Military_Time_Format);
            _Clock_Mode = _settings.GetValue(_section, "Clock_Mode", _Clock_Mode);
        }

        public float Virtual_Timerate
        {
            get
            {
                return _Virtual_Timerate;
            }
            set
            {
                _Virtual_Timerate = value;
                _settings.SetValue(_section, "Virtual_Timerate", _Virtual_Timerate);
                _settings.Save();
            }
        }

        public bool Show_Seconds
        {
            get
            {
                return _Show_Seconds;
            }
            set
            {
                _Show_Seconds = value;
                _settings.SetValue(_section, "Show_Seconds", _Show_Seconds);
                _settings.Save();
            }
        }

        public bool Military_Time_Format
        {
            get
            {
                return _Military_Time_Format;
            }
            set
            {
                _Military_Time_Format = value;
                _settings.SetValue(_section, "Military_Time_Format", _Military_Time_Format);
                _settings.Save();
            }
        }

        public ClockMode Clock_Mode
        {
            get
            {
                return _Clock_Mode;
            }
            set
            {
                _Clock_Mode = value;
                _settings.SetValue(_section, "Clock_Mode", _Clock_Mode);
            }
        }
    }
}
