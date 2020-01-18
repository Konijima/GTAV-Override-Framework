using System;
using GTA;
using GTAVOverride.Managers;
using GTAVOverride.Classes;

namespace GTAVOverride.Scripts
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class StoreScript : Script
    {
        private Store _inRangeStore;

        public StoreScript()
        {
            Interval = 1000;

            Tick += StoreScript_Tick;
        }

        private void StoreScript_Tick(object sender, EventArgs e)
        {
            Pause();

            // player is in store range
            if (_inRangeStore != null)
            {
                // is player leaving store range
                if (Game.Player.Character.Position.DistanceTo(_inRangeStore.position) > _inRangeStore.range)
                {
                    _inRangeStore.Show();
                    _inRangeStore = null;
                }
            }
            // player is not in range of any store
            else
            {
                foreach (Store store in StoreManager.Stores)
                {
                    // is player in range of this store
                    if (Game.Player.Character.Position.DistanceTo(store.position) < store.range)
                    {
                        _inRangeStore = store;
                        _inRangeStore.Hide(50);
                        break;
                    }
                }
            }
        }
    }
}
