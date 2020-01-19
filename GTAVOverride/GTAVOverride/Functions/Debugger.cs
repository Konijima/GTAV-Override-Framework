using System;
using GTA;
using GTAVOverride.Managers;

namespace GTAVOverride.Functions
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class Debugger : Script
    {
        private int currentFps = 0;
        private int fpsUpdateTimer = 0;

        public void Create()
        {
            if (!Main.configSettings.Debug_Mode) return;

            Debug.Log("Creating Debugger...");

            Tick += Debugger_Tick;
        }

        private void Debugger_Tick(object sender, EventArgs e)
        {

            DateTimeManager.Debug();

            if (Game.GameTime > fpsUpdateTimer)
            {
                currentFps = Convert.ToInt32(Game.FPS);
                fpsUpdateTimer = Game.GameTime + 100;
            }

            Debug.DrawText(new GTA.Math.Vector2(0.49f, 0.03f), "FPS: " + currentFps.ToString("0"));
        }

        public void Destroy()
        {
            Debug.Log("Destroying Debugger...");

            Tick -= Debugger_Tick;
            Abort();
        }
    }
}
