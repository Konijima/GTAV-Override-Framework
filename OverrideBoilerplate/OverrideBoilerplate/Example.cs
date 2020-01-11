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
            ModName = "OverrideBoilerplate";
            OverrideMaxVersion = new Version("1.0.0.4"); // if omited will default to this current mod assembly version.

            Started += Example_Started;
            Stopped += Example_Stopped;
        }

        protected override bool DependenciesValidator()
        {
            return (Persistence.Instance != null && Persistence.version <= new Version("1.0.0.4")); // you can add as many dependency
        }

        protected override void Update()
        {
            Screen.ShowHelpTextThisFrame("Remove " + ModName + ".dll and reload scripts (F5)."); // main loop
        }

        private void Example_Started(object sender, EventArgs e)
        {
            // what happen when we start

        }

        private void Example_Stopped(object sender, EventArgs e)
        {
            // what happen when we stop, duh
        }
    }
}
