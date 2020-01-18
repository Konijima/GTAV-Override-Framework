using System;
using System.Collections.Generic;
using System.Text;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using GTAVOverride.Managers;

namespace GTAVOverride.Functions
{
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

            string hours = DateTimeManager.CurrentTime.Hours.ToString();
            string minutes = DateTimeManager.CurrentTime.Minutes.ToString();
            string seconds = DateTimeManager.CurrentTime.Seconds.ToString();

            // 12 hours clock
            if (!Main.configClock.Military_Time_Format)
            {
                if (DateTimeManager.CurrentTime.Hours > 12) hours = (DateTimeManager.CurrentTime.Hours - 12).ToString();
            }

            // add zeroes
            if (DateTimeManager.CurrentTime.Minutes < 10) minutes = "0" + minutes;
            if (DateTimeManager.CurrentTime.Seconds < 10) seconds = "0" + seconds;

            string time = hours + ":" + minutes;

            // add seconds to time string
            if (Main.configClock.Show_Seconds) time += ":" + seconds;

            // add AM PM
            if (!Main.configClock.Military_Time_Format) time += " " + DateTimeManager.CurrentDate.ToString("tt");
            
            // scale an offset
            if (Main.configClock.Show_Seconds || !Main.configClock.Military_Time_Format)
            {
                yOffset = 0.02f;
                scale = 1.75f;
            }

            Function.Call(Hash.SET_TEXT_SCALE, scale, scale);
            Function.Call(Hash.SET_TEXT_FONT, font);
            Function.Call(Hash.SET_TEXT_CENTRE, 1);
            Function.Call(Hash.SET_TEXT_PROPORTIONAL, 1);
            Function.Call(Hash.SET_TEXT_COLOUR, 238, 201, 76, alpha);
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, time);
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, 0.5f, 0.33f + yOffset);
        }

        private void DrawDate()
        {
            string date = World.CurrentDate.ToString("MMMM d, yyyy");

            Function.Call(Hash.SET_TEXT_SCALE, 1f, 1f);
            Function.Call(Hash.SET_TEXT_FONT, font);
            Function.Call(Hash.SET_TEXT_CENTRE, 1);
            Function.Call(Hash.SET_TEXT_PROPORTIONAL, 1);
            Function.Call(Hash.SET_TEXT_COLOUR, 255, 255, 255, alpha);
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, date);
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, 0.5f, 0.46f);
        }

        private void DrawLine()
        {
            Function.Call(Hash.DRAW_RECT, 0.5f, 0.535f, 0.25f, 0.001f, 255, 255, 255, alpha);
        }

        private void DrawLocation()
        {
            Vector3 position = Game.Player.Character.Position;
            int locationHash = Function.Call<int>(Hash.GET_HASH_OF_MAP_AREA_AT_COORDS, position.X, position.Y, position.Z);
            string locationString = "Los Santos";
            if (locationHash == 2072609373) locationString = "Blaine County";

            Function.Call(Hash.SET_TEXT_SCALE, 1.3f, 1.3f);
            Function.Call(Hash.SET_TEXT_FONT, font);
            Function.Call(Hash.SET_TEXT_CENTRE, 1);
            Function.Call(Hash.SET_TEXT_PROPORTIONAL, 1);
            Function.Call(Hash.SET_TEXT_COLOUR, 255, 255, 255, alpha);
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, locationString);
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, 0.5f, 0.55f);
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

                Function.Call(Hash.DRAW_RECT, 0.5f, 0.5f, 1f, 1f, 0, 0, 0, alpha);

                DrawTime();
                DrawDate();
                DrawLine();
                DrawLocation();

                if (alpha <= 0)
                {
                    Game.Player.CanControlCharacter = true;
                    Abort();
                }
            }
        }
    }
}
