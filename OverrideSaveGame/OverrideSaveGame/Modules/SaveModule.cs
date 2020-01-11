using System;
using GTA;
using GTA.Native;

namespace OverrideSaveGame.Modules
{
    public class SaveModule
    {
        private SaveSettings _saveSettings = new SaveSettings();
        private AutoSaveSettings _autoSaveSettings = new AutoSaveSettings();

        private int lastSave;
        private int lastAutoSave;
        private bool forcedStopThisFrame = false;

        public SaveModule()
        {
            _saveSettings.SaveEnabled = false;
            
            _autoSaveSettings.AutoSaveEnabled = false;
            _autoSaveSettings.AutoSaveDelay = 6000;

            HideSpinner();
        }

        public SaveSettings saveSettings
        {
            get
            {
                return _saveSettings;
            }
            set
            {
                _saveSettings = value;
            }
        }

        public AutoSaveSettings autoSaveSettings
        {
            get
            {
                return _autoSaveSettings;
            }
            set
            {
                _autoSaveSettings = value;
            }
        }

        public void Update()
        {
            if (Game.Player.IsDead)
            {
                if (!autoSaveSettings.CanAutoSaveWhileDead)
                {
                    forcedStopThisFrame = true;
                }
            }

            if (Game.Player.WantedLevel > 0)
            {
                if (!autoSaveSettings.CanAutoSaveWhileWanted)
                {
                    forcedStopThisFrame = true;
                }
            }

            if (forcedStopThisFrame)
            {
                forcedStopThisFrame = false;
                return;
            }

            if (autoSaveSettings.AutoSaveEnabled)
            {
                if (Game.GameTime > lastAutoSave)
                {
                    OnAutoSaveStarted(new EventArgs());
                }
            }
        }

        public void DisableSaveThisFrame()
        {
            forcedStopThisFrame = true;
        }

        public void ShowSpinner()
        {
            Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "FM_COR_AUTOD");
            Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, 3);
        }

        public void HideSpinner()
        {
            Function.Call(Hash.BUSYSPINNER_OFF);
        }

        public void TriggerSaveCompleted()
        {
            lastSave = Game.GameTime;

            HideSpinner();
        }

        public void TriggerAutoSaveCompleted()
        {
            lastAutoSave = Game.GameTime + autoSaveSettings.AutoSaveDelay;

            HideSpinner();
        }

        protected virtual void OnSaveStarted(EventArgs e)
        {
            EventHandler<EventArgs> handler = Save;
            if (handler != null && saveSettings.SaveEnabled)
            {
                ShowSpinner();
                handler(this, e);
            }
        }

        protected virtual void OnAutoSaveStarted(EventArgs e)
        {
            EventHandler<EventArgs> handler = AutoSave;
            if (handler != null && autoSaveSettings.AutoSaveEnabled)
            {
                ShowSpinner();
                handler(this, e);
            }
        }

        public event EventHandler<EventArgs> Save;
        public event EventHandler<EventArgs> AutoSave;
    }

    public class SaveSettings
    {
        public bool CanSaveWhileDead { get; set; }
        public bool CanSaveWhileWanted { get; set; }
        public bool SaveEnabled { get; set; }
    }

    public class AutoSaveSettings 
    {
        public bool CanAutoSaveWhileDead { get; set; }
        public bool CanAutoSaveWhileWanted { get; set; }
        public bool AutoSaveEnabled { get; set; }
        public int AutoSaveDelay { get; set; }
    }
}
