using GTA;
using GTAVOverride.Managers;
using GTAVOverride.Scripts;

namespace GTAVOverride.Configs
{
    public class ConfigSettings
    {
        private readonly string _section;
        private readonly ScriptSettings _settings;

        private bool _Debug_Mode = false;
        private bool _Dont_Generate_Log = false;
        private int _Kill_GTAV_Scripts_Ms_Delay = 0;
        private bool _Kill_GTAV_Scripts_Only = false;
        private bool _Dont_Kill_GTAV_Scripts = false;
        private bool _Info_Intro_Screen = true;
        private int _Info_Intro_Screen_Ms_Duration = 4000;
        private bool _Unlock_All_Doors = true;
        private bool _Hospital_Spawn_OnDeath = true;
        private int _Hospital_Fee = 500;
        private bool _PoliceStation_Spawn_OnArrest = true;
        private int _PoliceStation_Fee = 200;
        private bool _Show_Radar_OnKeyDown = true;
        private bool _Randomize_People_Money = true;
        private int _Randomize_Poor_People_MaxMoney = 30;
        private int _Randomize_Regular_People_MaxMoney = 100;
        private int _Randomize_Rich_People_MaxMoney = 300;

        public ConfigSettings(ScriptSettings settings)
        {
            _section = "SETTINGS";
            _settings = settings;

            Load();
            Save();
        }

        public void Save()
        {
            Debug_Mode = _Debug_Mode;
            Dont_Generate_Log = _Dont_Generate_Log;
            Kill_GTAV_Scripts_Ms_Delay = _Kill_GTAV_Scripts_Ms_Delay;
            Kill_GTAV_Scripts_Only = _Kill_GTAV_Scripts_Only;
            Dont_Kill_GTAV_Scripts = _Dont_Kill_GTAV_Scripts;
            Info_Intro_Screen = _Info_Intro_Screen;
            Info_Intro_Screen_Ms_Duration = _Info_Intro_Screen_Ms_Duration;
            Unlock_All_Doors = _Unlock_All_Doors;
            Hospital_Spawn_OnDeath = _Hospital_Spawn_OnDeath;
            Hospital_Fee = _Hospital_Fee;
            PoliceStation_Spawn_OnArrest = _PoliceStation_Spawn_OnArrest;
            PoliceStation_Fee = _PoliceStation_Fee;
            Show_Radar_OnKeyDown = _Show_Radar_OnKeyDown;
            Randomize_People_Money = _Randomize_People_Money;
            Randomize_Poor_People_MaxMoney = _Randomize_Poor_People_MaxMoney;
            Randomize_Regular_People_MaxMoney = _Randomize_Regular_People_MaxMoney;
            Randomize_Rich_People_MaxMoney = _Randomize_Rich_People_MaxMoney;

            _settings.Save();
        }

        public void Load()
        {
            _Debug_Mode = _settings.GetValue(_section, "Debug_Mode", _Debug_Mode);
            _Dont_Generate_Log = _settings.GetValue(_section, "Dont_Generate_Log", _Dont_Generate_Log);
            _Kill_GTAV_Scripts_Ms_Delay = _settings.GetValue(_section, "Kill_GTAV_Scripts_Ms_Delay", _Kill_GTAV_Scripts_Ms_Delay);
            _Kill_GTAV_Scripts_Only = _settings.GetValue(_section, "Kill_GTAV_Scripts_Only", _Kill_GTAV_Scripts_Only);
            _Dont_Kill_GTAV_Scripts = _settings.GetValue(_section, "Dont_Kill_GTAV_Scripts", _Dont_Kill_GTAV_Scripts);
            _Info_Intro_Screen = _settings.GetValue(_section, "Info_Intro_Screen", _Info_Intro_Screen);
            _Info_Intro_Screen_Ms_Duration = _settings.GetValue(_section, "Info_Intro_Screen_Ms_Duration", _Info_Intro_Screen_Ms_Duration);
            _Unlock_All_Doors = _settings.GetValue(_section, "Unlock_All_Doors", _Unlock_All_Doors);
            _Hospital_Spawn_OnDeath = _settings.GetValue(_section, "Hospital_Spawn_OnDeath", _Hospital_Spawn_OnDeath);
            _Hospital_Fee = _settings.GetValue(_section, "Hospital_Fee", _Hospital_Fee);
            _PoliceStation_Spawn_OnArrest = _settings.GetValue(_section, "PoliceStation_Spawn_OnArrest", _PoliceStation_Spawn_OnArrest);
            _PoliceStation_Fee = _settings.GetValue(_section, "PoliceStation_Fee", _PoliceStation_Fee);
            _Show_Radar_OnKeyDown = _settings.GetValue(_section, "Show_Radar_OnKeyDown", _Show_Radar_OnKeyDown);
            _Randomize_People_Money = _settings.GetValue(_section, "Randomize_People_Money", _Randomize_People_Money);
            _Randomize_Poor_People_MaxMoney = _settings.GetValue(_section, "Randomize_Poor_People_MaxMoney", _Randomize_Poor_People_MaxMoney);
            _Randomize_Regular_People_MaxMoney = _settings.GetValue(_section, "Randomize_Regular_People_MaxMoney", _Randomize_Regular_People_MaxMoney);
            _Randomize_Rich_People_MaxMoney = _settings.GetValue(_section, "Randomize_Rich_People_MaxMoney", _Randomize_Rich_People_MaxMoney);
        }

        public bool Debug_Mode
        {
            get
            {
                return _Debug_Mode;
            }
            set
            {
                _Debug_Mode = value;
                _settings.SetValue<bool>(_section, "Debug_Mode", _Debug_Mode);
            }
        }

        public bool Dont_Generate_Log
        {
            get
            {
                return _Dont_Generate_Log;
            }
            set
            {
                _Dont_Generate_Log = value;
                _settings.SetValue<bool>(_section, "Dont_Generate_Log", _Dont_Generate_Log);
            }
        }

        public int Kill_GTAV_Scripts_Ms_Delay
        {
            get
            {
                return _Kill_GTAV_Scripts_Ms_Delay;
            }
            set
            {
                if (value <= 0) value = 0;
                _Kill_GTAV_Scripts_Ms_Delay = value;
                _settings.SetValue<int>(_section, "Kill_GTAV_Scripts_Ms_Delay", _Kill_GTAV_Scripts_Ms_Delay);
            }
        }

        public bool Kill_GTAV_Scripts_Only
        {
            get
            {
                return _Kill_GTAV_Scripts_Only;
            }
            set
            {
                _Kill_GTAV_Scripts_Only = value;
                _settings.SetValue<bool>(_section, "Kill_GTAV_Scripts_Only", _Kill_GTAV_Scripts_Only);
            }
        }

        public bool Dont_Kill_GTAV_Scripts
        {
            get
            {
                return _Dont_Kill_GTAV_Scripts;
            }
            set
            {
                _Dont_Kill_GTAV_Scripts = value;
                _settings.SetValue<bool>(_section, "Dont_Kill_GTAV_Scripts", _Dont_Kill_GTAV_Scripts);
            }
        }

        public bool Info_Intro_Screen
        {
            get
            {
                return _Info_Intro_Screen;
            }
            set
            {
                _Info_Intro_Screen = value;
                _settings.SetValue<bool>(_section, "Info_Intro_Screen", _Info_Intro_Screen);
            }
        }

        public int Info_Intro_Screen_Ms_Duration
        {
            get
            {
                return _Info_Intro_Screen_Ms_Duration;
            }
            set
            {
                _Info_Intro_Screen_Ms_Duration = value;
                _settings.SetValue<int>(_section, "Info_Intro_Screen_Ms_Duration", _Info_Intro_Screen_Ms_Duration);
            }
        }

        public bool Unlock_All_Doors
        {
            get
            {
                return _Unlock_All_Doors;
            }
            set
            {
                _Unlock_All_Doors = value;
                _settings.SetValue<bool>(_section, "Unlock_All_Doors", _Unlock_All_Doors);
            }
        }

        public bool Hospital_Spawn_OnDeath
        {
            get
            {
                return _Hospital_Spawn_OnDeath;
            }
            set
            {
                _Hospital_Spawn_OnDeath = value;
                _settings.SetValue<bool>(_section, "Hospital_Spawn_OnDeath", _Hospital_Spawn_OnDeath);
            }
        }

        public int Hospital_Fee
        {
            get
            {
                return _Hospital_Fee;
            }
            set
            {
                _Hospital_Fee = value;
                _settings.SetValue(_section, "Hospital_Fee", _Hospital_Fee);
            }
        }

        public bool PoliceStation_Spawn_OnArrest
        {
            get
            {
                return _PoliceStation_Spawn_OnArrest;
            }
            set
            {
                _PoliceStation_Spawn_OnArrest = value;
                _settings.SetValue<bool>(_section, "PoliceStation_Spawn_OnArrest", _PoliceStation_Spawn_OnArrest);
            }
        }

        public int PoliceStation_Fee
        {
            get
            {
                return _PoliceStation_Fee;
            }
            set
            {
                _PoliceStation_Fee = value;
                _settings.SetValue(_section, "PoliceStation_Fee", _PoliceStation_Fee);
            }
        }

        public bool Show_Radar_OnKeyDown
        {
            get
            {
                return _Show_Radar_OnKeyDown;
            }
            set
            {
                _Show_Radar_OnKeyDown = value;
                _settings.SetValue(_section, "Show_Radar_OnKeyDown", _Show_Radar_OnKeyDown);
            }
        }

        public bool Randomize_People_Money
        {
            get
            {
                return _Randomize_People_Money;
            }
            set
            {
                _Randomize_People_Money = value;
                _settings.SetValue<bool>(_section, "Randomize_People_Money", _Randomize_People_Money);
            }
        }

        public int Randomize_Poor_People_MaxMoney
        {
            get
            {
                return _Randomize_Poor_People_MaxMoney;
            }
            set
            {
                _Randomize_Poor_People_MaxMoney = value;
                _settings.SetValue<int>(_section, "Randomize_Poor_People_MaxMoney", _Randomize_Poor_People_MaxMoney);
            }
        }

        public int Randomize_Regular_People_MaxMoney
        {
            get
            {
                return _Randomize_Regular_People_MaxMoney;
            }
            set
            {
                _Randomize_Regular_People_MaxMoney = value;
                _settings.SetValue<int>(_section, "Randomize_Regular_People_MaxMoney", _Randomize_Regular_People_MaxMoney);
            }
        }

        public int Randomize_Rich_People_MaxMoney
        {
            get
            {
                return _Randomize_Rich_People_MaxMoney;
            }
            set
            {
                _Randomize_Rich_People_MaxMoney = value;
                _settings.SetValue<int>(_section, "Randomize_Rich_People_MaxMoney", _Randomize_Rich_People_MaxMoney);
            }
        }
    }
}
