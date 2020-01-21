using GTA;
using GTA.Math;
using GTAVOverride.Enums;
using GTAVOverride.Managers;

namespace GTAVOverride.Scripts
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class PeopleMoneyScript : Script
    {
        private static Ped[] surroundingPeds;
        private static int fetchSurroundingPedsTimer;

        public PeopleMoneyScript()
        {
            Tick += PeopleMoneyScript_Tick;
        }

        private void PeopleMoneyScript_Tick(object sender, System.EventArgs e)
        {
            if (Game.IsLoading || Game.IsPaused) return;

            if (Game.GameTime > fetchSurroundingPedsTimer)
            {
                surroundingPeds = World.GetNearbyPeds(Game.Player.Character, 30f);

                // only if we allow randomization setting
                if (Main.configSettings.Randomize_People_Money)
                {
                    foreach (Ped ped in surroundingPeds)
                    {
                        PeopleManager.RandomizePedMoney(ped);
                    }
                }

                fetchSurroundingPedsTimer = Game.GameTime + 1500;
            }

            if (Main.configSettings.Debug_Mode)
            {
                foreach (Ped ped in surroundingPeds)
                {
                    if (ped.IsAlive && ped.IsOnFoot)
                        Debug.DrawText3D(ped.Position, new Vector3(0, 0, -1.1f), "$" + ped.Money.ToString());
                }
            }
        }
    }
}
