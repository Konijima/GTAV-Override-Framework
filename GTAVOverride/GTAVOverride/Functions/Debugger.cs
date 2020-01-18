using System;
using GTA;
using GTAVOverride.Managers;

namespace GTAVOverride.Functions
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class Debugger : Script
    {
        public void Create()
        {
            if (!Main.configSettings.Debug_Mode) return;

            Debug.Log("Creating Debugger...");

            Tick += Debugger_Tick;
        }

        private void Debugger_Tick(object sender, EventArgs e)
        {

            DateTimeManager.Debug();

        }

        public void Destroy()
        {
            Debug.Log("Destroying Debugger...");

            Tick -= Debugger_Tick;
            Abort();
        }
    }
}
