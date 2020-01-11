using System;
using GTAVOverride;
using OverrideSaveGame.Modules;

namespace OverrideSaveGame
{
    public class SaveGame : ModBase
    {
        public SaveModule saveModule;

        public SaveGame()
        {
            ModName = "SaveGame";
            OverrideMaxVersion = new Version("1.0.0.4");

            saveModule = new SaveModule();

            Started += SaveGame_Started;
            Stopped += SaveGame_Stopped;
        }

        protected override bool DependenciesValidator()
        {
            return true;
        }

        protected override void Update()
        {
            saveModule.Update();
        }

        private void SaveGame_Started(object sender, EventArgs e)
        {

        }

        private void SaveGame_Stopped(object sender, EventArgs e)
        {

        }
    }
}
