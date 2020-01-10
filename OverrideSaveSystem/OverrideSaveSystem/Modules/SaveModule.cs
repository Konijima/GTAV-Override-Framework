using System;
using GTA;
using GTA.Native;

namespace OverrideSaveSystem.Modules
{
    public class SaveModule
    {
        private SaveSettings saveSettings = new SaveSettings();

        private int lastSave;
        private int lastAutoSave;
        private bool forcedStopThisFrame = false;

        public SaveModule()
        {
            saveSettings.canSave = false;
            saveSettings.autoSaveEnabled = false;
            saveSettings.autoSaveDelay = 6000;

            HideSpinner();
        }

        public SaveSettings settings
        {
            get
            {
                return saveSettings;
            }
            set
            {
                saveSettings = value;
            }
        }

        public void Update()
        {
            if (Game.Player.IsDead)
            {
                if (!saveSettings.canSaveWhileDead)
                {
                    forcedStopThisFrame = true;
                }
            }

            if (Game.Player.WantedLevel > 0)
            {
                if (!saveSettings.canSaveWhileWanted)
                {
                    forcedStopThisFrame = true;
                }
            }

            if (forcedStopThisFrame)
            {
                forcedStopThisFrame = false;
                return;
            }

            if (saveSettings.canSave && saveSettings.autoSaveEnabled)
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

        public void SetCanSave(bool canSave)
        {
            saveSettings.canSave = canSave;

            // reset next autosave delay
            if (saveSettings.autoSaveEnabled)
            {
                lastAutoSave = Game.GameTime + saveSettings.autoSaveDelay;
            }
        }

        public void SetCanSaveWhileDead(bool canSaveWhileDead)
        {
            saveSettings.canSaveWhileDead = canSaveWhileDead;

            // reset next autosave delay
            if (saveSettings.autoSaveEnabled)
            {
                lastAutoSave = Game.GameTime + saveSettings.autoSaveDelay;
            }
        }

        public void SetCanSaveWhileWanted(bool canSaveWhileWanted)
        {
            saveSettings.canSaveWhileWanted = canSaveWhileWanted;

            // reset next autosave delay
            if (saveSettings.autoSaveEnabled)
            {
                lastAutoSave = Game.GameTime + saveSettings.autoSaveDelay;
            }
        }

        public void SetAutoSaveEnabled(bool autoSaveEnabled)
        {
            saveSettings.autoSaveEnabled = autoSaveEnabled;

            // reset next autosave delay
            if (saveSettings.autoSaveEnabled)
            {
                lastAutoSave = Game.GameTime + saveSettings.autoSaveDelay;
            }
        }

        public void SetAutoSaveDelay(int autoSaveDelay)
        {
            if (autoSaveDelay < 3000) { autoSaveDelay = 3000; }
            saveSettings.autoSaveDelay = autoSaveDelay;

            // reset next autosave delay
            if (saveSettings.autoSaveEnabled)
            {
                lastAutoSave = Game.GameTime + saveSettings.autoSaveDelay;
            }
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
            lastAutoSave = Game.GameTime + saveSettings.autoSaveDelay;
            lastSave = Game.GameTime;

            HideSpinner();
        }

        protected virtual void OnSaveStarted(EventArgs e)
        {
            EventHandler<EventArgs> handler = Save;
            if (handler != null && saveSettings.canSave)
            {
                ShowSpinner();
                handler(this, e);
            }
        }

        protected virtual void OnAutoSaveStarted(EventArgs e)
        {
            EventHandler<EventArgs> handler = AutoSave;
            if (handler != null && saveSettings.canSave && saveSettings.autoSaveEnabled)
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
        public bool canSave { get; set; }
        public bool canSaveWhileDead { get; set; }
        public bool canSaveWhileWanted { get; set; }
        public bool autoSaveEnabled { get; set; }
        public int autoSaveDelay { get; set; }
    }
}
