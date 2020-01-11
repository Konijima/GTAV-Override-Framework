using System;
using GTAVOverride;
using OverrideSaveGame;

namespace OverridePersistence
{
    public class Persistence : ModBase
    {
        public Persistence()
        {
            ModName = "Override Persistence";
            OverrideMaxVersion = new Version("1.0.0.4");

            Started += Persistence_Started;
            Stopped += Persistence_Stopped;
        }

        protected override bool DependenciesValidator()
        {
            return (SaveGame.Instance != null && SaveGame.version >= new Version("1.0.0.4"));
        }

        protected override void Update()
        {
            
        }

        private void Persistence_Started(object sender, EventArgs e)
        {

        }

        private void Persistence_Stopped(object sender, EventArgs e)
        {

        }
    }
}
