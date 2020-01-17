using GTA;
using GTAVOverride.Scripts;

namespace GTAVOverride.Configs
{
    public class ConfigSettings
    {
        private readonly string _section;
        private ScriptSettings _settings;

        private bool _Debug_Mode = false;
        private int _Kill_GTAV_Scripts_Ms_Delay = 0;
        private bool _Kill_GTAV_Scripts_Only = false;
        private bool _Dont_Kill_GTAV_Scripts = false;
        private ClockMode _Clock_Mode = ClockMode.Virtual;
        private bool _Unlock_All_Doors = true;
        private bool _Hospital_Spawn_OnDeath = true;
        private bool _PoliceStation_Spawn_OnArrest = true;
        private bool _Quick_Rob_People_No_Pickup = false;
        private bool _Randomize_People_Money = true;
        private int _Randomize_People_MaxMoney = 100;

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
            Kill_GTAV_Scripts_Ms_Delay = _Kill_GTAV_Scripts_Ms_Delay;
            Kill_GTAV_Scripts_Only = _Kill_GTAV_Scripts_Only;
            Dont_Kill_GTAV_Scripts = _Dont_Kill_GTAV_Scripts;
            Clock_Mode = _Clock_Mode;
            Unlock_All_Doors = _Unlock_All_Doors;
            Hospital_Spawn_OnDeath = _Hospital_Spawn_OnDeath;
            PoliceStation_Spawn_OnArrest = _PoliceStation_Spawn_OnArrest;
            Quick_Rob_People_No_Pickup = _Quick_Rob_People_No_Pickup;
            Randomize_People_Money = _Randomize_People_Money;
            Randomize_People_MaxMoney = _Randomize_People_MaxMoney;

            _settings.Save();
        }

        public void Load()
        {
            _Debug_Mode = _settings.GetValue<bool>(_section, "Debug_Mode", _Debug_Mode);
            _Kill_GTAV_Scripts_Ms_Delay = _settings.GetValue<int>(_section, "Kill_GTAV_Scripts_Ms_Delay", _Kill_GTAV_Scripts_Ms_Delay);
            _Kill_GTAV_Scripts_Only = _settings.GetValue<bool>(_section, "Kill_GTAV_Scripts_Only", _Kill_GTAV_Scripts_Only);
            _Dont_Kill_GTAV_Scripts = _settings.GetValue<bool>(_section, "Dont_Kill_GTAV_Scripts", _Dont_Kill_GTAV_Scripts);
            _Clock_Mode = _settings.GetValue<ClockMode>(_section, "Clock_Mode", _Clock_Mode);
            _Unlock_All_Doors = _settings.GetValue<bool>(_section, "Unlock_All_Doors", _Unlock_All_Doors);
            _Hospital_Spawn_OnDeath = _settings.GetValue<bool>(_section, "Hospital_Spawn_OnDeath", _Hospital_Spawn_OnDeath);
            _PoliceStation_Spawn_OnArrest = _settings.GetValue<bool>(_section, "PoliceStation_Spawn_OnArrest", _PoliceStation_Spawn_OnArrest);
            _Quick_Rob_People_No_Pickup = _settings.GetValue<bool>(_section, "Quick_Rob_People_No_Pickup", _Quick_Rob_People_No_Pickup);
            _Randomize_People_Money = _settings.GetValue<bool>(_section, "Randomize_People_Money", _Randomize_People_Money);
            _Randomize_People_MaxMoney = _settings.GetValue<int>(_section, "Randomize_People_MaxMoney", _Randomize_People_MaxMoney);
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

        public ClockMode Clock_Mode
        {
            get
            {
                return _Clock_Mode;
            }
            set
            {
                _Clock_Mode = value;
                _settings.SetValue<ClockMode>(_section, "Clock_Mode", _Clock_Mode);
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

        public bool Quick_Rob_People_No_Pickup
        {
            get
            {
                return _Quick_Rob_People_No_Pickup;
            }
            set
            {
                _Quick_Rob_People_No_Pickup = value;
                _settings.SetValue<bool>(_section, "Quick_Rob_People_No_Pickup", _Quick_Rob_People_No_Pickup);
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

        public int Randomize_People_MaxMoney
        {
            get
            {
                return _Randomize_People_MaxMoney;
            }
            set
            {
                _Randomize_People_MaxMoney = value;
                _settings.SetValue<int>(_section, "Randomize_People_MaxMoney", _Randomize_People_MaxMoney);
            }
        }
    }
}
