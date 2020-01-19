using System;
using GTA;
using GTA.UI;
using GTA.Math;
using GTAVOverride.Managers;

namespace GTAVOverride.Functions
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class IntroScreen : Script
    {
        private Font font;
        private bool drawn;
        private int timer;
        private int alpha;

        public IntroScreen()
        {
            font = Font.Pricedown;
            drawn = false;
            alpha = 255;
        }

        public void SetFont(Font font)
        {
            this.font = font;
        }

        public void Start()
        {
            drawn = true;
            timer = Game.GameTime + Main.configSettings.Info_Intro_Screen_Ms_Duration;
            Tick += IntroScreen_Tick;
        }

        private void DrawTime()
        {
            float yOffset = -0.023f;
            float scale = 3f;

            string time = DateTimeManager.GetTimeString();
            
            // scale an offset
            if (Main.configClock.Show_Seconds || !Main.configClock.Military_Time_Format)
            {
                yOffset = 0.02f;
                scale = 1.75f;
            }

            Helpers.DrawText(new Vector2(0.5f, 0.33f + yOffset), time, true, true, scale, 238, 201, 76, alpha, font);
        }

        private void DrawDate()
        {
            string date = World.CurrentDate.ToString("MMMM d, yyyy");

            Helpers.DrawText(new Vector2(0.5f, 0.46f), date, true, true, 1f, 255, 255, 255, alpha, font);
        }

        private void DrawLine()
        {
            Helpers.DrawRectangle(0.5f, 0.535f, 0.25f, 0.001f, 255, 255, 255, alpha);
        }

        private void DrawLocation()
        {
            string locationString = Helpers.GetAreaName(Game.Player.Character.Position);
            Helpers.DrawText(new Vector2(0.5f, 0.55f), locationString, true, true, 1.3f, 255, 255, 255, alpha, font);
        }

        private void IntroScreen_Tick(object sender, EventArgs e)
        {
            if (drawn)
            {
                Game.Player.CanControlCharacter = false;

                if (Game.GameTime > timer)
                {
                    alpha -= (int)(255 * Game.LastFrameTime);

                    if (alpha < 0) alpha = 0;
                }

                Helpers.DrawRectangle(0.5f, 0.5f, 1f, 1f, 0, 0, 0, alpha);

                DrawTime();
                DrawDate();
                DrawLine();
                DrawLocation();

                if (alpha <= 30)
                {
                    Game.Player.CanControlCharacter = true;
                    Abort();
                }
            }
        }
    }
}
