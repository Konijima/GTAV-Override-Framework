using System;
using System.Reflection;
using GTA;
using GTA.UI;

namespace GTAVOverride
{
    public abstract class ModBase : Script
    {
        private static ModBase instance;
        public static ModBase Instance
        {
            get
            {
                return instance;
            }
        }

        public static Version version { get; } = new Version(Assembly.GetExecutingAssembly().GetName().Version.ToString());
        public static Version OverrideMaxVersion = version;

        public string ModName = "No name";
        public bool UpdateWhilePaused = true;

        private bool _inited = false;
        private bool _stopped = false;

        public ModBase()
        {
            Interval = 1;
            instance = this;

            Tick += ModBase_Tick;
        }

        protected abstract void Update();

        protected abstract bool DependenciesValidator();

        private void Init()
        {
            _inited = true;
            if (Override.Instance.isStarted)
            {
                Start();
            }
            else
            {
                Override.Instance.Started += Override_Started;
            }

            Override.Instance.Stopped += Override_Stopped;
        }

        private void Start()
        {
            _inited = false;
            _stopped = false;

            OnStarted(new EventArgs());

            if (Override.Instance.SETTING_NOTIFY_STARTED)
            {
                Override.Instance.Notify(NotificationIcon.MpFmContact, ModName, "Mod version " + version.ToString() + " has started!", "", false, false);
            }
        }

        private void Stop()
        {
            _inited = false;
            _stopped = true;

            OnStopped(new EventArgs());

            if (Override.Instance.SETTING_NOTIFY_STOPPED)
            {
                Override.Instance.Notify(NotificationIcon.Blocked, ModName, "Mod has stopped!", "", false, false);
            }
        }

        private void ModBase_Tick(object sender, EventArgs e)
        {
            if (_stopped) { return; }

            if (_inited)
            {
                if (Game.IsPaused && !UpdateWhilePaused)
                {
                    return;
                }

                Update();
                return;
            }

            else if (Override.Instance != null && DependenciesValidator())
            {
                if (Override.version > OverrideMaxVersion)
                {
                    throw new Exception("Invalid mod dependency version!");
                }

                if (!_inited)
                {
                    Init();
                }
            }
        }

        private void Override_Started(object sender, EventArgs e)
        {
            Start();
        }

        private void Override_Stopped(object sender, EventArgs e)
        {
            Stop();
        }

        protected virtual void OnStarted(EventArgs e)
        {
            Started?.Invoke(this, e);
        }
        public event EventHandler<EventArgs> Started;

        protected virtual void OnStopped(EventArgs e)
        {
            Stopped?.Invoke(this, e);
        }
        public event EventHandler<EventArgs> Stopped;
    }
}
