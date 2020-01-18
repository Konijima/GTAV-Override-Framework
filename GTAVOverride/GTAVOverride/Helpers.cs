using System;
using System.Drawing;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;

namespace GTAVOverride
{
    public static class Helpers
    {
        public static Random Random = new Random();

        public static void DrawText(Vector2 position, string text, bool centre = false, bool proportional = true, float scale = 0.35f, int r = 255, int g = 255, int b = 255, int opacity = 255, Font font = Font.ChaletLondon)
        {
            Function.Call(Hash.SET_TEXT_SCALE, scale, scale);
            Function.Call(Hash.SET_TEXT_FONT, font);
            Function.Call(Hash.SET_TEXT_CENTRE, centre);
            Function.Call(Hash.SET_TEXT_PROPORTIONAL, proportional);
            Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, opacity);
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, position.X, position.Y);
        }

        public static void DrawText3D(Vector3 coords, Vector3 offset, string text, bool centre = true, bool proportional = false, float scale = 0.35f, int r = 255, int g = 255, int b = 255, int opacity = 255, Font font = Font.ChaletLondon)
        {
            Vector3 position = coords - offset;
            PointF point = Screen.WorldToScreen(position, true);

            if (point.X == 0 || point.Y == 0) return;

            Function.Call(Hash.SET_TEXT_SCALE, scale, scale);
            Function.Call(Hash.SET_TEXT_FONT, font);
            Function.Call(Hash.SET_TEXT_CENTRE, centre);
            Function.Call(Hash.SET_TEXT_PROPORTIONAL, proportional);
            Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, opacity);
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, point.X / Screen.Width, point.Y / Screen.Height);
        }

        public static void DrawRectangle()
        {

        }

        public static void ClearAllBlips()
        {
            Debug.Log("Clearing all blips...");

            Blip[] blips = World.GetAllBlips();
            foreach (Blip blip in blips) blip.Delete();
        }
    }
}
