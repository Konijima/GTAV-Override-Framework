using System;
using System.Collections.Generic;
using System.Text;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;

namespace GTAVOverride
{
    public class WaypointScript : Script
    {
        public WaypointScript()
        {
            Interval = 1000;

            Tick += WaypointTool_Tick;
        }

        private void WaypointTool_Tick(object sender, EventArgs e)
        {
            if (World.WaypointBlip != null && World.WaypointBlip.Exists() && !Game.IsPaused)
            {
                Screen.ShowSubtitle("Waypoint: X: " + World.WaypointPosition.X + " | Y: " + World.WaypointPosition.Y);
            }
            else
            {
                if (Game.IsControlPressed(Control.InteractionMenu))
                {
                    Screen.ShowSubtitle("Player: X: " + Game.Player.Character.Position.X + " | Y: " + Game.Player.Character.Position.Y + " | Z: " + Game.Player.Character.Position.Z + " | H: " + Game.Player.Character.Heading, 5000);
                }
            }
        }
    }
}
