using GTA;

namespace GTAVOverride.Configs
{
    public class ConfigScripts
    {
        private readonly string _section;
        private ScriptSettings _settings;

        private bool _Player_Death_Arrest_Script = true;
        private bool _Player_Persistence_Script = true;
        private bool _ClockTime_Script = true;
        private bool _TimeScale_Script = true;
        private bool _Economy_Script = true;
        private bool _Rob_People_Script = true;
        private bool _People_Money_Script = true;
        private bool _Atm_Script = true;
        private bool _Store_Script = true;
        private bool _Play_Police_Script = false;
        private bool _Waypoint_Tools_Script = false;

        public ConfigScripts(ScriptSettings settings)
        {
            _section = "SCRIPTS";
            _settings = settings;

            Load();
            Save();
        }

        public void Save()
        {
            Player_Death_Arrest_Script = _Player_Death_Arrest_Script;
            Player_Persistence_Script = _Player_Persistence_Script;
            ClockTime_Script = _ClockTime_Script;
            TimeScale_Script = _TimeScale_Script;
            Economy_Script = _Economy_Script;
            Rob_People_Script = _Rob_People_Script;
            People_Money_Script = _People_Money_Script;
            Atm_Script = _Atm_Script;
            Store_Script = _Store_Script;
            //Play_Police_Script = _Play_Police_Script;
            Waypoint_Tools_Script = _Waypoint_Tools_Script;
            
            _settings.Save();
        }

        public void Load()
        {
            _Player_Death_Arrest_Script = _settings.GetValue<bool>(_section, "Player_Death_Arrest_Script", _Player_Death_Arrest_Script);
            _Player_Persistence_Script = _settings.GetValue<bool>(_section, "Player_Persistence_Script", _Player_Persistence_Script);
            _ClockTime_Script = _settings.GetValue<bool>(_section, "ClockTime_Script", _ClockTime_Script);
            _TimeScale_Script = _settings.GetValue<bool>(_section, "TimeScale_Script", _TimeScale_Script);
            _Economy_Script = _settings.GetValue<bool>(_section, "Economy_Script", _Economy_Script);
            _Rob_People_Script = _settings.GetValue<bool>(_section, "Rob_People_Script", _Rob_People_Script);
            _People_Money_Script = _settings.GetValue<bool>(_section, "People_Money_Script", _People_Money_Script);
            _Atm_Script = _settings.GetValue<bool>(_section, "Atm_Script", _Atm_Script);
            _Store_Script = _settings.GetValue<bool>(_section, "Store_Script", _Store_Script);
            _Play_Police_Script = _settings.GetValue<bool>(_section, "Play_Police_Script", _Play_Police_Script);
            _Waypoint_Tools_Script = _settings.GetValue<bool>(_section, "Waypoint_Tools_Script", _Waypoint_Tools_Script);
        }

        public bool Player_Death_Arrest_Script
        {
            get
            {
                return _Player_Death_Arrest_Script;
            }
            set
            {
                _Player_Death_Arrest_Script = value;
                _settings.SetValue<bool>(_section, "Player_Death_Arrest_Script", _Player_Death_Arrest_Script);
            }
        }

        public bool Player_Persistence_Script
        {
            get
            {
                return _Player_Persistence_Script;
            }
            set
            {
                _Player_Persistence_Script = value;
                _settings.SetValue<bool>(_section, "Player_Persistence_Script", _Player_Persistence_Script);
            }
        }

        public bool ClockTime_Script
        {
            get
            {
                return _ClockTime_Script;
            }
            set
            {
                _ClockTime_Script = value;
                _settings.SetValue<bool>(_section, "ClockTime_Script", _ClockTime_Script);
            }
        }

        public bool TimeScale_Script
        {
            get
            {
                return _TimeScale_Script;
            }
            set
            {
                _TimeScale_Script = value;
                _settings.SetValue<bool>(_section, "TimeScale_Script", _TimeScale_Script);
            }
        }

        public bool Economy_Script
        {
            get
            {
                return _Economy_Script;
            }
            set
            {
                _Economy_Script = value;
                _settings.SetValue<bool>(_section, "Economy_Script", _Economy_Script);
            }
        }

        public bool Rob_People_Script
        {
            get
            {
                return _Rob_People_Script;
            }
            set
            {
                _Rob_People_Script = value;
                _settings.SetValue<bool>(_section, "Rob_People_Script", _Rob_People_Script);
            }
        }

        public bool People_Money_Script
        {
            get
            {
                return _People_Money_Script;
            }
            set
            {
                _People_Money_Script = value;
                _settings.SetValue<bool>(_section, "People_Money_Script", _People_Money_Script);
            }
        }

        public bool Atm_Script
        {
            get
            {
                return _Atm_Script;
            }
            set
            {
                _Atm_Script = value;
                _settings.SetValue<bool>(_section, "Atm_Script", _Atm_Script);
            }
        }

        public bool Store_Script
        {
            get
            {
                return _Store_Script;
            }
            set
            {
                _Store_Script = value;
                _settings.SetValue<bool>(_section, "Store_Script", _Store_Script);
            }
        }

        public bool Play_Police_Script
        {
            get
            {
                return _Play_Police_Script;
            }
            set
            {
                _Play_Police_Script = value;
                _settings.SetValue<bool>(_section, "Play_Police_Script", _Play_Police_Script);
            }
        }

        public bool Waypoint_Tools_Script
        {
            get
            {
                return _Waypoint_Tools_Script;
            }
            set
            {
                _Waypoint_Tools_Script = value;
                _settings.SetValue<bool>(_section, "Waypoint_Tools_Script", _Waypoint_Tools_Script);
            }
        }


    }
}
