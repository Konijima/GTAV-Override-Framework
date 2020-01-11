using System;
using System.Reflection;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using GTAVOverride;
using OverrideSaveSystem;
using OverrideBoilerplate.Events;

namespace OverrideBoilerplate
{
    public class Boilerplate : Script
    {
        public static readonly string ModName = "Override Boilerplate";

        // current assembly version
        public static readonly Version version = new Version(Assembly.GetExecutingAssembly().GetName().Version.ToString());
        
        // for now it's set so it match the current mod version
        // so change the assembly version to match the GTAVOverride.dll version
        public static readonly Version minimumGTAVOverrideVersion = version; // set the minimum version of the dependency
        public static readonly Version minimumOverrideSaveSystemVersion = version; // set the minimum version of the dependency

        // static instance of itself
        public static Boilerplate instance;

        private bool dependenciesFound = false;

        public Boilerplate()
        {
            Interval = 200; // or whatever you need

            instance = this;

            Tick += Boilerplate_Tick;
        }

        // Since we use the static instances, we cannot make sure it load in the right order.
        // This way, we wait until static variable isnt null and then we Init
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
                    // depend on OverrideSaveSystem
                    if (SaveSystem.version < minimumOverrideSaveSystemVersion) Notification.Show(NotificationIcon.Blocked, ModName, "", "Mod require OverrideSaveSystem version " + minimumOverrideSaveSystemVersion.ToString() + "!", true, false);

                    // depend on GTAVOverride
                    if (Override.version < minimumGTAVOverrideVersion) Notification.Show(NotificationIcon.Blocked, ModName, "", "Mod require GTAVOverride version " + minimumGTAVOverrideVersion.ToString() + "!", true, false);
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
            // since we depend on OverrideSaveSystem, we start and stop with it
            SaveSystem.instance.Started += SaveSystem_Started;
            SaveSystem.instance.Stopped += SaveSystem_Stopped;

            /* 
             *  If you only depend on GTAVOverride
             * 
                Override.instance.Started += Override_Started;
                Override.instance.Stopped += Override_Stopped;
             */
        }

        private void Start()
        {
            if (Override.instance.notifications)
            {
                Notification.Show(NotificationIcon.MpFmContact, ModName, "Mod version " + version.ToString() + " has started!", "", true, false);
            }
            OnStarted(new EventArgs()); // trigger OnStarted event
        }

        private void Stop()
        {
            if (Override.instance.notifications)
            {
                Notification.Show(NotificationIcon.Blocked, ModName, "Mod has stopped!", "", true, false);
            }
            OnStopped(new EventArgs()); // trigger OnStopped event
        }

        private void Boilerplate_Tick(object sender, EventArgs e)
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


        // Boilerplate OnExemple event with custom event args
        protected virtual void OnExemple(BoilerplateEventArgs e)
        {
            EventHandler<BoilerplateEventArgs> handler = Exemple;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler<BoilerplateEventArgs> Exemple;

        // Boilerplate OnStarted event
        protected virtual void OnStarted(EventArgs e)
        {
            EventHandler<EventArgs> handler = Started;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler<EventArgs> Started;

        // Boilerplate OnStopped event
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
