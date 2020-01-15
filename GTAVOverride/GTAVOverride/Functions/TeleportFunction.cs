using System;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;

namespace GTAVOverride.Functions
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class TeleportFunction : Script
    {
        private bool _ready;

        private Ped targetPed;
        private Vector3 targetPosition;
        private float heading;
        private bool safe;
        private bool fade;
        private int fadeSpeed;

        public TeleportFunction()
        {
            Tick += TeleportFunction_Tick;
        }

        public void Prepare(Ped targetPed, Vector3 targetPosition, float heading, bool safe, bool fade = true, int fadeSpeed = 1000)
        {
            this.targetPed = targetPed;
            this.targetPosition = targetPosition;
            this.heading = heading;
            this.safe = safe;
            this.fade = fade;
            this.fadeSpeed = fadeSpeed;

            this._ready = true;
        }

        private void TeleportFunction_Tick(object sender, EventArgs e)
        {
            if (_ready)
            {
                if (fade)
                {
                    Screen.FadeOut(fadeSpeed);
                    while (Screen.IsFadingOut) Wait(1);
                }

                Wait(500);

                Function.Call(Hash.START_PLAYER_TELEPORT, Game.Player, targetPosition.X, targetPosition.Y, targetPosition.Z + 0.01f, heading, true, true, false);
                while (Function.Call<bool>(Hash._HAS_PLAYER_TELEPORT_FINISHED, Game.Player)) Wait(1);

                Wait(500);

                if (fade)
                {
                    Screen.FadeIn(fadeSpeed);
                    while (Screen.IsFadingIn) Wait(1);
                }

                Abort();
            }
        }
    }
}
