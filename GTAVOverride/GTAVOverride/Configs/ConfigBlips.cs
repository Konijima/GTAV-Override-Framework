using GTA;

namespace GTAVOverride.Configs
{
    public class ConfigBlips
    {
        private readonly string _section;
        private ScriptSettings _settings;

        private bool _Show_Ammunations = true;
        private bool _Show_Atm = true;
        private bool _Show_Stores = true;
        private bool _Show_Hospitals = true;
        private bool _Show_PoliceStations = true;

        public ConfigBlips(ScriptSettings settings)
        {
            _section = "BLIPS";
            _settings = settings;

            Load();
            Save();
        }

        public void Save()
        {
            Show_Ammunations = _Show_Ammunations;
            Show_Atm = _Show_Atm;
            Show_Stores = _Show_Stores;
            Show_Hospitals = _Show_Hospitals;
            Show_PoliceStations = _Show_PoliceStations;

            _settings.Save();
        }

        public void Load()
        {
            _Show_Ammunations = _settings.GetValue<bool>(_section, "Show_Ammunations", _Show_Ammunations);
            _Show_Atm = _settings.GetValue<bool>(_section, "Show_Atm", _Show_Atm);
            _Show_Stores = _settings.GetValue<bool>(_section, "Show_Stores", _Show_Stores);
            _Show_Hospitals = _settings.GetValue<bool>(_section, "Show_Hospitals", _Show_Hospitals);
            _Show_PoliceStations = _settings.GetValue<bool>(_section, "Show_PoliceStations", _Show_PoliceStations);
        }

        public bool Show_Ammunations
        {
            get
            {
                return _Show_Ammunations;
            }
            set
            {
                _Show_Ammunations = value;
                _settings.SetValue<bool>(_section, "Show_Ammunations", _Show_Ammunations);
                _settings.Save();
            }
        }

        public bool Show_Atm
        {
            get
            {
                return _Show_Atm;
            }
            set
            {
                _Show_Atm = value;
                _settings.SetValue<bool>(_section, "Show_Atm", _Show_Atm);
                _settings.Save();
            }
        }

        public bool Show_Stores
        {
            get
            {
                return _Show_Stores;
            }
            set
            {
                _Show_Stores = value;
                _settings.SetValue<bool>(_section, "Show_Stores", _Show_Stores);
                _settings.Save();
            }
        }

        public bool Show_Hospitals
        {
            get
            {
                return _Show_Hospitals;
            }
            set
            {
                _Show_Hospitals = value;
                _settings.SetValue<bool>(_section, "Show_Hospitals", _Show_Hospitals);
                _settings.Save();
            }
        }

        public bool Show_PoliceStations
        {
            get
            {
                return _Show_PoliceStations;
            }
            set
            {
                _Show_PoliceStations = value;
                _settings.SetValue<bool>(_section, "Show_PoliceStations", _Show_PoliceStations);
                _settings.Save();
            }
        }
    }
}
