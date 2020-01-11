using System;
using GTA;

namespace OverrideBoilerplate.Events
{
    public class BoilerplateEventArgs : EventArgs
    {
        public Ped ped { get; set; }
    }
}
