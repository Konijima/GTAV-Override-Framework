using System;
using System.Reflection;
using GTA;
using GTA.UI;
using GTAVOverride;
using OverrideSaveSystem;

namespace OverridePersistence
{
    public class Persistence : Script
    {
        public static readonly Version version = new Version(Assembly.GetExecutingAssembly().GetName().Version.ToString());
        public static readonly Version minimumGTAVOverrideVersion = version;
        public static readonly Version minimumOverrideSaveSystemVersion = version;

        public static Persistence instance;

        private bool dependenciesFound = false;

        public Persistence()
        {
            Interval = 200;

            instance = this;

            Tick += Persistence_Tick;
        }

        private void DependenciesValidator()
        {
            if (dependenciesFound) { return; }

            if (Override.instance == null || SaveSystem.instance == null)
            {
                dependenciesFound = false;
            }

            if (!dependenciesFound && Override.instance != null && SaveSystem.instance != null)
            {
                if (Override.version < minimumGTAVOverrideVersion || SaveSystem.version < minimumOverrideSaveSystemVersion)
                {
                    if (SaveSystem.version < minimumOverrideSaveSystemVersion) Notification.Show(NotificationIcon.Blocked, "Override Persistence", "", "Mod require OverrideSaveSystem version " + minimumOverrideSaveSystemVersion.ToString() + " or more!", true, false);
                    if (Override.version < minimumGTAVOverrideVersion) Notification.Show(NotificationIcon.Blocked, "Override Persistence", "", "Mod require GTAVOverride version " + minimumGTAVOverrideVersion.ToString() + " or more!", true, false);
                    Abort();
                }
                else
                {
                    dependenciesFound = true;
                    Init();
                }
            }
        }

        private void Init()
        {
            SaveSystem.instance.Started += SaveSystem_Started;
            SaveSystem.instance.Stopped += SaveSystem_Stopped;

            SaveSystem.instance.saveModule.Save += SaveModule_Save;
            SaveSystem.instance.saveModule.AutoSave += SaveModule_AutoSave;

            SaveSystem.instance.saveModule.SetCanSave(true);
            SaveSystem.instance.saveModule.SetCanSaveWhileDead(false);
            SaveSystem.instance.saveModule.SetCanSaveWhileWanted(false);
            SaveSystem.instance.saveModule.SetAutoSaveEnabled(true);
            SaveSystem.instance.saveModule.SetAutoSaveDelay(8000);
        }

        private void Start()
        {
            if (Override.instance.notifications)
            {
                Notification.Show(NotificationIcon.MpFmContact, "Override Persistence", "Mod version " + version.ToString() + " has started!", "", true, false);
            }
            OnStarted(new EventArgs());

            Load();
        }

        private void Stop()
        {
            if (Override.instance.notifications)
            {
                Notification.Show(NotificationIcon.Blocked, "Override Persistence", "Mod has stopped!", "", true, false);
            }
            OnStopped(new EventArgs());
        }

        protected void Save()
        {
            Wait(1000);
        }

        protected void Load()
        {
            Wait(1000);
            OnLoaded(new EventArgs());
        }

        private void Persistence_Tick(object sender, EventArgs e)
        {
            DependenciesValidator();

            if (dependenciesFound)
            {
                // dependencies are ready
                // update loop here
            }
        }

        private void SaveSystem_Started(object sender, EventArgs e)
        {
            Start();
        }

        private void SaveSystem_Stopped(object sender, EventArgs e)
        {
            Stop();
        }

        private void SaveModule_Save(object sender, EventArgs e)
        {
            Wait(1000);
            SaveSystem.instance.saveModule.TriggerSaveCompleted();
        }

        private void SaveModule_AutoSave(object sender, EventArgs e)
        {
            Save();
            SaveSystem.instance.saveModule.TriggerSaveCompleted();
        }

        protected virtual void OnStarted(EventArgs e)
        {
            EventHandler<EventArgs> handler = Started;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler<EventArgs> Started;

        protected virtual void OnStopped(EventArgs e)
        {
            EventHandler<EventArgs> handler = Stopped;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler<EventArgs> Stopped;

        protected virtual void OnLoaded(EventArgs e)
        {
            EventHandler<EventArgs> handler = Loaded;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler<EventArgs> Loaded;
    }
}
