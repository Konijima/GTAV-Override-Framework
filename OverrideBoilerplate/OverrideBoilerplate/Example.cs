using System;
using GTA.UI;
using GTAVOverride;

namespace OverrideBoilerplate
{
    public class Example : ModBase
    {
        public Example()
        {
            ModName = "Example Mod";

            Started += Example_Started;
            Stopped += Example_Stopped;
        }

        protected override bool DependenciesValidator()
        {
            return true;
        }

        protected override void Update()
        {
            // Screen.ShowHelpTextThisFrame(ModName);
        }

        private void Example_Started(object sender, EventArgs e)
        {

        }

        private void Example_Stopped(object sender, EventArgs e)
        {

        }
    }
}
