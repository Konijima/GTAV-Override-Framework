using System;
using System.Collections.Generic;
using System.Text;
using GTA;
using GTA.UI;
using GTA.Native;

namespace GTAVOverride.Scripts
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class RobPedSequenceScript : Script
    {
        private float intimidation;
        private int tolerence;
        private Ped targetPed;
        private int state;

        public RobPedSequenceScript()
        {
            Random random = new Random();

            intimidation = (float)random.NextDouble() + 0.25f;
            tolerence = random.Next(50, 600);
            targetPed = null;
            state = -1;
        }

        public void SetTargetPed(Ped ped)
        {
            if (ped != null)
            {
                targetPed = ped;
                targetPed.BlockPermanentEvents = true;
                state++;

                Tick += RobPedSequenceScript_Tick;
            }
        }

        public Prop SpawnMoney(Ped targetPed)
        {
            string modelName = "prop_ld_wallet_01";
            if (targetPed.Gender == Gender.Male) modelName = "prop_ld_wallet_01";
            else modelName = "prop_amb_handbag_01";

            Model wallet = new Model(modelName);
            wallet.Request();
            while (!wallet.IsInCdImage)
            {
                Wait(1);
                wallet.Request();
            }

            Prop pickup = World.CreateAmbientPickup(PickupType.MoneyPurse, targetPed.Position + targetPed.ForwardVector * 0.5f, wallet, targetPed.Money * 2 + 10);
            wallet.MarkAsNoLongerNeeded();

            return pickup;
        }

        private void RobPedSequenceScript_Tick(object sender, EventArgs e)
        {
            if (targetPed != null)
            {
                switch(state)
                {
                    case -1:
                        
                        break;

                    case 0:
                        targetPed.Task.ClearAll();
                        targetPed.Task.TurnTo(Game.Player.Character, 1000);

                        Wait(1000);

                        state++;
                        break;

                    case 1:

                        targetPed.AlwaysKeepTask = true;
                        Function.Call(Hash.TASK_HANDS_UP, targetPed.Handle, 2000, true, 0);

                        Wait(1000);

                        bool droppedLoot = false;
                        int totalIntimidation = 0;
                        float maxAngle = 90f;

                        while (
                            !droppedLoot &&
                            targetPed.IsAlive &&
                            !Game.Player.Character.IsShooting && 
                            Game.Player.Character.IsAiming &&
                            !Game.Player.Character.IsRagdoll &&
                            Function.Call<bool>(Hash.IS_PED_FACING_PED, Game.Player.Character.Handle, targetPed.Handle, maxAngle) &&
                            Game.Player.Character.Position.DistanceTo(targetPed.Position) < 20f)
                        {
                            if (Game.Player.IsTargeting(targetPed))
                            {
                                totalIntimidation += 5;
                            }
                            else
                            {
                                totalIntimidation += 1;
                            }

                            if (Game.Player.Character.IsInVehicle())
                            {
                                maxAngle = 200f;
                            }
                            else
                            {
                                maxAngle = 90f;
                            }

                            if (!Function.Call<bool>(Hash.IS_PED_FACING_PED, targetPed.Handle, Game.Player.Character.Handle, 55f))
                            {
                                targetPed.Task.TurnTo(Game.Player.Character, 1000);
                                Wait(1000);
                            }

                            Function.Call(Hash.UPDATE_TASK_HANDS_UP_DURATION, targetPed.Handle, 1000);

                            if (!Function.Call<bool>(Hash.GET_IS_TASK_ACTIVE, targetPed.Handle, 0))
                            {
                                Function.Call(Hash.TASK_HANDS_UP, targetPed.Handle, 1000, true, 0);
                            }

                            if (!Function.Call<bool>(Hash.IS_AMBIENT_SPEECH_PLAYING, targetPed.Handle))
                            {
                                Function.Call(Hash._PLAY_AMBIENT_SPEECH1, targetPed.Handle, "GENERIC_FRIGHTENED_HIGH", "SPEECH_PARAMS_FORCE");
                            }

                            if (totalIntimidation > tolerence)
                            {
                                Function.Call(Hash._PLAY_AMBIENT_SPEECH1, Game.Player.Character, "STAY_DOWN", "SPEECH_PARAMS_FORCE");

                                SpawnMoney(targetPed);

                                Wait(200);

                                droppedLoot = true;
                            }

                            Wait(1);
                        }

                        state++;
                        break;

                    case 2:
                        targetPed.BlockPermanentEvents = false;
                        Function.Call(Hash._PLAY_AMBIENT_SPEECH1, targetPed.Handle, "GENERIC_SHOCKED_HIGH", "Speech_Params_Force_Shouted_Clear");
                        targetPed.Task.ReactAndFlee(Game.Player.Character);
                        Abort();
                        break;
                    default:
                        Abort();
                        break;
                }
            }
        }
    }

    [ScriptAttributes(NoDefaultInstance = true)]
    public class RobPedScript : Script
    {
        private List<Ped> threathPed;

        public RobPedScript()
        {
            threathPed = new List<Ped>();

            Tick += RobPedScript_Tick;
        }

        private void RobPedScript_Tick(object sender, EventArgs e)
        {
            if (CanRobPed())
            {
                Entity entity = Game.Player.TargetedEntity;
                if (entity != null && entity.Model.IsPed)
                {
                    Ped ped = (Ped)entity;
                    if (CanPedBeRob(ped))
                    {
                        ThreathenPed(ped);
                    }
                }
            }
        }

        public bool CanRobPed()
        {
            Ped playerPed = Game.Player.Character;
            if (playerPed.IsAlive && 
                playerPed.IsAiming)
            {
                return true;
            }
            return false;
        }

        public bool CanPedBeRob(Ped ped)
        {
            if (ped != null && 
                ped.IsAlive && 
                ped.IsOnFoot && 
                !ped.IsInjured && 
                !threathPed.Contains(ped))
            {
                if (ped.Model == PedHash.Cop01SFY ||
                    ped.Model == PedHash.Cop01SMY ||
                    ped.Model == PedHash.Trevor ||
                    ped.Model == PedHash.ShopHighSFM ||
                    ped.Model == PedHash.ShopKeep01 ||
                    ped.Model == PedHash.ShopLowSFY ||
                    ped.Model == PedHash.ShopMaskSMY ||
                    ped.Model == PedHash.ShopMidSFY)
                {
                    return false;
                }

                if (!ped.IsInMeleeCombat && 
                    !ped.IsInCombatAgainst(Game.Player.Character) && 
                    (ped.GetRelationshipWithPed(Game.Player.Character) == Relationship.Pedestrians || 
                    ped.GetRelationshipWithPed(Game.Player.Character) == Relationship.Neutral ||
                    ped.GetRelationshipWithPed(Game.Player.Character) == Relationship.Like ||
                    ped.GetRelationshipWithPed(Game.Player.Character) == Relationship.Dislike) &&
                    ped.Position.DistanceTo(Game.Player.Character.Position) < 20f)
                {
                    return true;
                }
            }
            return false;
        }

        public void ThreathenPed(Ped ped)
        {
            if (ped != null && 
                !threathPed.Contains(ped))
            {
                threathPed.Add(ped);

                Function.Call(Hash._PLAY_AMBIENT_SPEECH1, Game.Player.Character, "STAY_DOWN", "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL");

                RobPedSequenceScript sequence = InstantiateScript<RobPedSequenceScript>();
                sequence.SetTargetPed(ped);
            }
        }
    }
}
