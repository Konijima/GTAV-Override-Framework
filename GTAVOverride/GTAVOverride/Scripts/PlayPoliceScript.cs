using System;
using GTA;
using GTA.UI;
using GTA.Native;

namespace GTAVOverride.Scripts
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class PlayPoliceScript : Script
    {
        public PlayPoliceScript()
        {
            Pause();

            Tick += PlayPoliceScript_Tick;
            Aborted += PlayPoliceScript_Aborted;
        }

        private void PlayPoliceScript_Tick(object sender, EventArgs e)
        {

        }

        private void PlayPoliceScript_Aborted(object sender, EventArgs e)
        {
            
        }
    }
}
