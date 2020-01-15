using System;
using System.Collections.Generic;
using System.Text;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using GTAVOverride.Functions;
using System.Drawing;

namespace GTAVOverride
{
    public static class Helpers
    {

        public static Random Random = new Random();

        public static void DrawText(Vector2 position, string text)
        {
            Function.Call(Hash.SET_TEXT_SCALE, 0.35f, 0.35f);
            Function.Call(Hash.SET_TEXT_FONT, 0);
            Function.Call(Hash.SET_TEXT_CENTRE, 0);
            Function.Call(Hash.SET_TEXT_PROPORTIONAL, 1);
            Function.Call(Hash.SET_TEXT_COLOUR, 255, 255, 255, 255);
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, position.X, position.Y);
        }

        public static void DrawText3D(Vector3 coords, Vector3 offset, string text, int opacity = 255)
        {
            Vector3 position = coords - offset;
            PointF point = Screen.WorldToScreen(position, true);

            if (point.X == 0 || point.Y == 0) return;

            Function.Call(Hash.SET_TEXT_SCALE, 0.35f, 0.35f);
            Function.Call(Hash.SET_TEXT_FONT, 0);
            Function.Call(Hash.SET_TEXT_CENTRE, 1);
            Function.Call(Hash.SET_TEXT_PROPORTIONAL, 0);
            Function.Call(Hash.SET_TEXT_COLOUR, 255, 255, 255, 255);
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, point.X / Screen.Width, point.Y / Screen.Height);
        }

        public static void DebugSubtitle(string text, int delay = 1000)
        {
            if (Main.configSettings.Debug_Mode)
            {
                GTA.UI.Screen.ShowSubtitle(text, delay);
            }
        }

        public static void TeleportPed(Ped ped, Vector3 position, float heading, bool safe = false, bool fade = true, int fadeSpeed = 1000)
        {
            TeleportFunction teleport = Script.InstantiateScript<TeleportFunction>();
            teleport.Prepare(ped, position, heading, safe, fade, fadeSpeed);
        }
    }
}
