using System;
using GTA.UI;
using GTAVOverride;
using OverridePersistence;

namespace OverrideBoilerplate
{
    public class Example : ModBase
    {
        public Example()
        {
            ModName = "Example Mod";
            OverrideMaxVersion = new Version("1.0.0.4");

            Started += Example_Started;
            Stopped += Example_Stopped;
        }

        protected override bool DependenciesValidator()
        {
            return (Persistence.Instance != null && Persistence.version <= new Version("1.0.0.4"));
        }

        protected override void Update()
        {
            Screen.ShowHelpTextThisFrame(ModName);
        }

        private void Example_Started(object sender, EventArgs e)
        {

        }

        private void Example_Stopped(object sender, EventArgs e)
        {

        }
    }
}
