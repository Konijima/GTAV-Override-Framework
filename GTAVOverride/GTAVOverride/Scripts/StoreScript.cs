using System;
using System.Collections.Generic;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using GTAVOverride.Managers;

namespace GTAVOverride.Scripts
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class StoreScript : Script
    {
        private Store _nearbyStore;
        private int _updateNearestTimer = 0;

        public StoreScript()
        {
            Tick += StoreScript_Tick;
        }

        private void StoreScript_Tick(object sender, EventArgs e)
        {
            Pause();

            if (_nearbyStore != null)
            {
                if (Game.Player.Character.Position.DistanceTo(_nearbyStore.position) > 8f)
                {
                    _nearbyStore.Show();
                    _nearbyStore = null;
                }
            }
            else
            {
                if (Game.GameTime > _updateNearestTimer + 1000)
                {
                    _updateNearestTimer = Game.GameTime;
                    foreach (Store store in StoreManager.Stores)
                    {
                        if (Game.Player.Character.Position.DistanceTo(store.position) < 8f)
                        {
                            _nearbyStore = store;
                            _nearbyStore.Hide(50);
                            break;
                        }
                    }
                }
            }
        }
    }
}
