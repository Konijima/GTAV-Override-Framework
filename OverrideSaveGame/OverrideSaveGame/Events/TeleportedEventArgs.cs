using System;
using System.Collections.Generic;
using System.Text;
using GTA;

namespace OverrideSaveGame.Events
{
    public class TeleportedEventArgs : EventArgs
    {
        public Player player { get; set; }
    }
}
