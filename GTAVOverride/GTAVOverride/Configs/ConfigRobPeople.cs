using GTA;

namespace GTAVOverride.Configs
{
    public class ConfigRobPeople
    {
        private readonly string _section;
        private readonly ScriptSettings _settings;

        private float _Distance_Of_Reach = 20f;
        private int _Restrict_When_Stars_Bigger_Then = 1;
        private bool _Quick_Rob_People_No_Pickup = false;
        private bool _Help_Message_Beep_Sound = false;

        public ConfigRobPeople(ScriptSettings settings)
        {
            _section = "ROB_PEOPLE_SCRIPT";
            _settings = settings;

            Load();
            Save();
        }

        public void Save()
        {
            Distance_Of_Reach = _Distance_Of_Reach;
            Restrict_When_Stars_Bigger_Then = _Restrict_When_Stars_Bigger_Then;
            Quick_Rob_People_No_Pickup = _Quick_Rob_People_No_Pickup;
            Help_Message_Beep_Sound = _Help_Message_Beep_Sound;

            _settings.Save();
        }

        public void Load()
        {
            _Distance_Of_Reach = _settings.GetValue(_section, "Distance_Of_Reach", _Distance_Of_Reach);
            _Restrict_When_Stars_Bigger_Then = _settings.GetValue(_section, "Restrict_When_Stars_Bigger_Then", _Restrict_When_Stars_Bigger_Then);
            _Quick_Rob_People_No_Pickup = _settings.GetValue(_section, "Quick_Rob_People_No_Pickup", _Quick_Rob_People_No_Pickup);
            _Help_Message_Beep_Sound = _settings.GetValue(_section, "Help_Message_Beep_Sound", _Help_Message_Beep_Sound);
        }

        public float Distance_Of_Reach
        {
            get
            {
                return _Distance_Of_Reach;
            }
            set
            {
                _Distance_Of_Reach = value;
                _settings.SetValue(_section, "Distance_Of_Reach", _Distance_Of_Reach);
            }
        }

        public int Restrict_When_Stars_Bigger_Then
        {
            get
            {
                return _Restrict_When_Stars_Bigger_Then;
            }
            set
            {
                _Restrict_When_Stars_Bigger_Then = value;
                _settings.SetValue(_section, "Restrict_When_Stars_Bigger_Then", _Restrict_When_Stars_Bigger_Then);
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
                _settings.SetValue(_section, "Quick_Rob_People_No_Pickup", _Quick_Rob_People_No_Pickup);
            }
        }

        public bool Help_Message_Beep_Sound
        {
            get
            {
                return _Help_Message_Beep_Sound;
            }
            set
            {
                _Help_Message_Beep_Sound = value;
                _settings.SetValue(_section, "Help_Message_Beep_Sound", _Help_Message_Beep_Sound);
            }
        }
    }
}
