using System;
using System.Reflection;
using GTA;
using GTA.UI;
using GTAVOverride;
using OverrideSaveSystem.Modules;

namespace OverrideSaveSystem
{
    public class SaveSystem : Script
    {
        // OverrideSaveSystem Version
        public static readonly Version version = new Version(Assembly.GetExecutingAssembly().GetName().Version.ToString());

        // Minimum GTAVOverrideVersion Version
        public static readonly Version minimumGTAVOverrideVersion = version;

        // Static SaveSystem instance
        public static SaveSystem instance;

        // SaveModule instance
        public SaveModule saveModule;

        // once true it means dependencies were validated
        private bool dependenciesFound = false;

        public SaveSystem()
        {
            Interval = 200;

            instance = this;

            Tick += SaveSystem_Tick;

            saveModule = new SaveModule();
        }

        private void DependenciesValidator()
        {
            if (dependenciesFound) { return; }

            if (Override.instance == null)
            {
                dependenciesFound = false;
            }

            if (!dependenciesFound && Override.instance != null)
            {
                if (Override.version < minimumGTAVOverrideVersion)
                {
                    Notification.Show(NotificationIcon.Blocked, "Override SaveSystem", "", "Mod require GTAVOverride version " + minimumGTAVOverrideVersion.ToString() + " or more!", true, false);
                    Abort();
                }
                else
                {
                    dependenciesFound = true;
                    Init();
                }
            }
        }

        // Init when dependencies are validated
        private void Init()
        {
            Override.instance.Launched += Override_Launched;
            Override.instance.Stopped += Override_Stopped;
        }

        // Start when Override has started
        private void Start()
        {
            if (Override.instance.notifications)
            {
                Notification.Show(NotificationIcon.MpFmContact, "Override SaveSystem", "Mod version " + version.ToString() + " has started!", "", true, false);
            }
            OnStarted(new EventArgs());
        }

        // Stop when Override has stopped
        private void Stop()
        {
            if (Override.instance.notifications)
            {
                Notification.Show(NotificationIcon.Blocked, "Override SaveSystem", "Mod has stopped!", "", true, false);
            }
            OnStopped(new EventArgs());
        }

        private void SaveSystem_Tick(object sender, EventArgs e)
        {
            DependenciesValidator();

            if (dependenciesFound)
            {
                saveModule.Update();
            }
        }

        private void Override_Launched(object sender, EventArgs e)
        {
            Start();
        }

        private void Override_Stopped(object sender, EventArgs e)
        {
            Stop();
        }

        // SaveSystem OnStarted event
        protected virtual void OnStarted(EventArgs e)
        {
            EventHandler<EventArgs> handler = Started;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler<EventArgs> Started;

        // SaveSystem OnStopped event
        protected virtual void OnStopped(EventArgs e)
        {
            EventHandler<EventArgs> handler = Stopped;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler<EventArgs> Stopped;
    }
}
