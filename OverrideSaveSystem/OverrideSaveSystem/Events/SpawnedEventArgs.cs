using System;
using System.Collections.Generic;
using System.Text;
using GTA;

namespace OverrideSaveSystem.Events
{
    public class SpawnedEventArgs : EventArgs
    {
        public Ped ped { get; set; }
    }
}
