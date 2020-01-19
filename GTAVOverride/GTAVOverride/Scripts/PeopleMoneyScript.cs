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

        public PeopleMoneyScript()
        {
            Interval = 3333;
            Tick += PeopleMoneyScript_Tick;
        }

        private void PeopleMoneyScript_Tick(object sender, System.EventArgs e)
        {
            if (Game.IsLoading || Game.IsPaused) return;

            surroundingPeds = World.GetNearbyPeds(Game.Player.Character, 20f);

            // only if we allow randomization setting
            if (Main.configSettings.Randomize_People_Money)
            {
                foreach (Ped ped in surroundingPeds)
                {
                    int maxMoney = 0;
                    SocialClasses classe = Helpers.GetPedSocialClass(ped);

                    if (classe == SocialClasses.Poor) maxMoney = Main.configSettings.Randomize_Poor_People_MaxMoney;
                    if (classe == SocialClasses.Regular) maxMoney = Main.configSettings.Randomize_Regular_People_MaxMoney;
                    if (classe == SocialClasses.Rich) maxMoney = Main.configSettings.Randomize_Rich_People_MaxMoney;

                    if (maxMoney < 0) maxMoney = 0;
                    if (maxMoney > 9999999) maxMoney = 9999999;

                    if (Main.configScripts.Rob_People_Script && ped.IsAlive && !PeopleManager.ThreatenedPeds.Contains(ped) && !PeopleManager.RobbedPeds.Contains(ped))
                    {
                        ped.Money = Helpers.Random.Next(0, maxMoney);
                    }
                }
            }

            // show people money even if randomize is disabled
            if (Main.configSettings.Debug_Mode)
            {
                foreach (Ped ped in surroundingPeds)
                {
                    if (ped != null && ped.IsAlive && ped.IsHuman && (ped.IsOnBike || ped.IsOnFoot))
                    {
                        Helpers.DrawText3D(ped.Position, new Vector3(0, 0, -1.1f), "$" + ped.Money.ToString());
                    }
                }
            }
        }
    }
}
