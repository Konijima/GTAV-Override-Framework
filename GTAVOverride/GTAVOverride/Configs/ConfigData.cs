using GTA;

namespace GTAVOverride.Configs
{
    public class ConfigData
    {
        private readonly string _section;
        private ScriptSettings _settings;

        private string _Save_Directory_Path = "scripts/GTAVOverrideData/";
        private string _Save_Player_Filename = "player0";
        private bool _Auto_Save_Enabled = true;
        private int _Auto_Save_Delay = 15000;
        private bool _Auto_Save_Show_Spinner_text = true;
        private bool _Auto_Save_After_DeathArrest = true;
        private bool _Auto_Save_When_Wanted = false;
        private bool _Auto_Save_In_Vehicle = true;
        private bool _Auto_Save_On_Transaction = true;

        public ConfigData(ScriptSettings settings)
        {
            _section = "DATA";
            _settings = settings;

            Load();
            Save();
        }

        public void Save()
        {
            Save_Directory_Path = _Save_Directory_Path;
            Save_Player_Filename = _Save_Player_Filename;
            Auto_Save_Enabled = _Auto_Save_Enabled;
            Auto_Save_Delay = _Auto_Save_Delay;
            Auto_Save_Show_Spinner_text = _Auto_Save_Show_Spinner_text;
            Auto_Save_After_DeathArrest = _Auto_Save_After_DeathArrest;
            Auto_Save_When_Wanted = _Auto_Save_When_Wanted;
            Auto_Save_In_Vehicle = _Auto_Save_In_Vehicle;
            Auto_Save_On_Transaction = _Auto_Save_On_Transaction;

            _settings.Save();
        }

        public void Load()
        {
            _Save_Directory_Path = _settings.GetValue(_section, "Save_Directory_Path", _Save_Directory_Path);
            _Save_Player_Filename = _settings.GetValue(_section, "Save_Player_Filename", _Save_Player_Filename);
            _Auto_Save_Enabled = _settings.GetValue(_section, "Auto_Save_Enabled", _Auto_Save_Enabled);
            _Auto_Save_Delay = _settings.GetValue(_section, "Auto_Save_Delay", _Auto_Save_Delay);
            _Auto_Save_Show_Spinner_text = _settings.GetValue(_section, "Auto_Save_Show_Spinner_text", _Auto_Save_Show_Spinner_text);
            _Auto_Save_After_DeathArrest = _settings.GetValue(_section, "Auto_Save_After_DeathArrest", _Auto_Save_After_DeathArrest);
            _Auto_Save_When_Wanted = _settings.GetValue(_section, "Auto_Save_When_Wanted", _Auto_Save_When_Wanted);
            _Auto_Save_In_Vehicle = _settings.GetValue(_section, "Auto_Save_In_Vehicle", _Auto_Save_In_Vehicle);
            _Auto_Save_On_Transaction = _settings.GetValue(_section, "Auto_Save_On_Transaction", _Auto_Save_On_Transaction);
        }

        public string Save_Directory_Path
        {
            get
            {
                return _Save_Directory_Path;
            }
            set
            {
                _Save_Directory_Path = value;
                _settings.SetValue(_section, "Save_Directory_Path", _Save_Directory_Path);
            }
        }

        public string Save_Player_Filename
        {
            get
            {
                return _Save_Player_Filename;
            }
            set
            {
                _Save_Player_Filename = value;
                _settings.SetValue(_section, "Save_Player_Filename", _Save_Player_Filename);
            }
        }

        public bool Auto_Save_Enabled
        {
            get
            {
                return _Auto_Save_Enabled;
            }
            set
            {
                _Auto_Save_Enabled = value;
                _settings.SetValue(_section, "Auto_Save_Enabled", _Auto_Save_Enabled);
            }
        }

        public int Auto_Save_Delay
        {
            get
            {
                return _Auto_Save_Delay;
            }
            set
            {
                _Auto_Save_Delay = value;
                _settings.SetValue(_section, "Auto_Save_Delay", _Auto_Save_Delay);
            }
        }

        public bool Auto_Save_Show_Spinner_text
        {
            get
            {
                return _Auto_Save_Show_Spinner_text;
            }
            set
            {
                _Auto_Save_Show_Spinner_text = value;
                _settings.SetValue(_section, "Auto_Save_Show_Spinner_text", _Auto_Save_Show_Spinner_text);
            }
        }

        public bool Auto_Save_After_DeathArrest
        {
            get
            {
                return _Auto_Save_After_DeathArrest;
            }
            set
            {
                _Auto_Save_After_DeathArrest = value;
                _settings.SetValue(_section, "Auto_Save_After_DeathArrest", _Auto_Save_After_DeathArrest);
            }
        }

        public bool Auto_Save_When_Wanted
        {
            get
            {
                return _Auto_Save_When_Wanted;
            }
            set
            {
                _Auto_Save_When_Wanted = value;
                _settings.SetValue(_section, "Auto_Save_When_Wanted", _Auto_Save_When_Wanted);
            }
        }

        public bool Auto_Save_In_Vehicle
        {
            get
            {
                return _Auto_Save_In_Vehicle;
            }
            set
            {
                _Auto_Save_In_Vehicle = value;
                _settings.SetValue(_section, "Auto_Save_In_Vehicle", _Auto_Save_In_Vehicle);
            }
        }

        public bool Auto_Save_On_Transaction
        {
            get
            {
                return _Auto_Save_On_Transaction;
            }
            set
            {
                _Auto_Save_On_Transaction = value;
                _settings.SetValue(_section, "Auto_Save_On_Transaction", _Auto_Save_On_Transaction);
            }
        }
    }
}
