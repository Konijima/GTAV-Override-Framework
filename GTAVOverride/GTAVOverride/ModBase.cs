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
        public static Version OverrideMaxVersion;

        public string ModName;

        private bool _inited = false;

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
            Override.Instance.Started += Override_Started;
            Override.Instance.Stopped += Override_Stopped;
        }

        private void Start()
        {
            Override.Instance.Notify(NotificationIcon.MpFmContact, ModName, "Mod version " + version.ToString() + " has started!", "", true, false);
            OnStarted(new EventArgs());
        }

        private void Stop()
        {
            Override.Instance.Notify(NotificationIcon.Blocked, ModName, "Mod has stopped!", "", true, false);
            OnStopped(new EventArgs());
        }

        private void ModBase_Tick(object sender, EventArgs e)
        {
            if (Override.Instance != null && DependenciesValidator())
            {
                if (Override.version > OverrideMaxVersion)
                {
                    return;
                }

                if (!_inited)
                {
                    Init();
                }
                else
                {
                    Update();
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
