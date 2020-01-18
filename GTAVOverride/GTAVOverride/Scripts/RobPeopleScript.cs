using System;
using System.Collections.Generic;
using System.Text;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using System.Drawing;
using GTAVOverride.Managers;

namespace GTAVOverride.Scripts
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class RobPedSequenceScript : Script
    {
        public static readonly Random Random = new Random();

        private float intimidationSpeed;
        private float maxIntimidationSpeed;
        private int tolerence;
        private Ped targetPed;
        private int state;

        private int reward = 0;
        private bool canceled = false;
        private bool droppedLoot = false;
        private float totalIntimidation = 0;
        private float maxAngle = 120f;

        public RobPedSequenceScript()
        {
            intimidationSpeed = 0;
            maxIntimidationSpeed = 15;
            tolerence = Random.Next(200, 400);
            targetPed = null;
            state = -1;
        }

        public void SetTargetPed(Ped ped)
        {
            if (ped != null)
            {
                targetPed = ped;
                targetPed.BlockPermanentEvents = true;
                reward = targetPed.Money;
                tolerence += reward;
                state++;

                Tick += RobPedSequenceScript_Tick;
            }
        }

        private void SpawnPedMoney(Ped targetPed)
        {
            if (targetPed.Money <= 0) return;

            if (Main.configSettings.Quick_Rob_People_No_Pickup)
            {
                Game.Player.Money += targetPed.Money;
                targetPed.Money = 0;
                return;
            }

            string modelName = "prop_ld_wallet_01";
            if (targetPed.Gender == Gender.Female) modelName = "prop_amb_handbag_01";

            Model wallet = new Model(modelName);
            wallet.Request();
            while (!wallet.IsInCdImage)
            {
                Wait(1);
                wallet.Request();
            }

            Prop pickup = World.CreateAmbientPickup(PickupType.MoneyPurse, targetPed.Position + targetPed.ForwardVector * 0.5f, wallet, targetPed.Money);
            wallet.MarkAsNoLongerNeeded();
        }

        private void RobPedSequenceScript_Tick(object sender, EventArgs e)
        {
            if (targetPed != null)
            {
                maxAngle = 120f;

                switch (state)
                {
                    case -1:
                        break;

                    case 0:
                        while (targetPed.IsRagdoll) Wait(1);

                        targetPed.AlwaysKeepTask = true;
                        targetPed.BlockPermanentEvents = true;

                        targetPed.Task.ClearAll();

                        state++;
                        break;

                    case 1:
                        while (
                            !canceled &&
                            !droppedLoot &&
                            targetPed.IsAlive &&
                            !Game.Player.Character.IsShooting && 
                            !Game.Player.Character.IsRagdoll &&
                            Function.Call<bool>(Hash.IS_PED_FACING_PED, Game.Player.Character, targetPed, maxAngle) &&
                            Game.Player.Character.Position.DistanceTo(targetPed.Position) < 20f)
                        {
                            Wait(1);

                            // Update Max Angle
                            if (Game.Player.Character.IsInVehicle()) maxAngle = 240f;
                            else maxAngle = 90f;

                            if (Game.Player.Character.IsAiming)
                            {
                                // if directly targeting lets intimidate faster
                                if (Game.Player.IsTargeting(targetPed))
                                {
                                    intimidationSpeed = 0.8f;

                                    if (Function.Call<bool>(Hash.IS_AMBIENT_SPEECH_PLAYING, Game.Player.Character))
                                        intimidationSpeed = 1.8f;
                                }
                                else
                                {
                                    intimidationSpeed = 0.1f;

                                    if (Function.Call<bool>(Hash.IS_AMBIENT_SPEECH_PLAYING, Game.Player.Character))
                                        intimidationSpeed = 0.6f;
                                }
                            }
                            else
                            {
                                intimidationSpeed -= 5f * Game.LastFrameTime;
                            }
                            if (intimidationSpeed > maxIntimidationSpeed) intimidationSpeed = maxIntimidationSpeed;
                            
                            if (!Function.Call<bool>(Hash.IS_AMBIENT_SPEECH_PLAYING, targetPed)) Function.Call(Hash._PLAY_AMBIENT_SPEECH1, targetPed, "GENERIC_FRIGHTENED_MED", "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL");

                            if (Main.configSettings.Debug_Mode)
                            {
                                if (targetPed.IsVisible)
                                {
                                    Helpers.DrawText3D(targetPed.Position, new Vector3(0, 0, -1.45f), intimidationSpeed.ToString());
                                    Helpers.DrawText3D(targetPed.Position, new Vector3(0, 0, -1.3f), totalIntimidation.ToString("0") + "/" + tolerence.ToString("0"));
                                }
                            }

                            if (targetPed.IsInVehicle()) targetPed.Task.LeaveVehicle(LeaveVehicleFlags.LeaveDoorOpen);
                            else
                            {
                                // Update Handup
                                if (!Function.Call<bool>(Hash.GET_IS_TASK_ACTIVE, targetPed, 0))
                                    Function.Call(Hash.TASK_HANDS_UP, targetPed, 2000, Game.Player.Character, -1, true);
                                else
                                    Function.Call(Hash.UPDATE_TASK_HANDS_UP_DURATION, targetPed, 2000);
                            }

                            if (!targetPed.IsRagdoll)
                            {
                                totalIntimidation += intimidationSpeed;
                            }

                            if (totalIntimidation > tolerence)
                            {
                                SpawnPedMoney(targetPed);
                                targetPed.Money = 0;
                                droppedLoot = true;
                            }
                            else if (totalIntimidation < 0f)
                            {
                                targetPed.Task.ClearAll();
                                targetPed.Task.FleeFrom(Game.Player.Character, -1);
                                canceled = true;
                            }
                        }
                        state++;
                        break;

                    case 2:

                        if (!droppedLoot)
                        {
                            RobPeopleScript.CancelThreatenedPed(targetPed);

                            Function.Call(Hash._PLAY_AMBIENT_SPEECH1, targetPed.Handle, "GENERIC_FUCK_YOU", "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL");

                            targetPed.Task.FleeFrom(Game.Player.Character);
                        }
                        else if (droppedLoot)
                        {
                            RobPeopleScript.AddRobbedPed(targetPed);

                            Function.Call(Hash._PLAY_AMBIENT_SPEECH1, targetPed.Handle, "GENERIC_SHOCKED_HIGH", "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL");

                            if (targetPed.Gender == Gender.Male)
                            {
                                targetPed.Task.FleeFrom(Game.Player.Character);
                            }
                            else
                            {
                                targetPed.Task.ReactAndFlee(Game.Player.Character);
                            }

                            if (Function.Call<bool>(Hash.IS_AMBIENT_SPEECH_PLAYING, Game.Player.Character))
                            {
                                while (Function.Call<bool>(Hash.IS_AMBIENT_SPEECH_PLAYING, Game.Player.Character)) Wait(0);
                                Wait(300);
                            }

                            if (reward == 0)
                            {
                                Function.Call(Hash._PLAY_AMBIENT_SPEECH1, Game.Player.Character, "GENERIC_FUCK_YOU", "SPEECH_PARAMS_FORCE_SHOUTED_CLEAR");
                            }
                            else
                            {
                                Function.Call(Hash._PLAY_AMBIENT_SPEECH1, Game.Player.Character, "GENERIC_THANKS", "SPEECH_PARAMS_FORCE_SHOUTED_CLEAR");
                            }
                        }

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
    public class RobPeopleScript : Script
    {
        private static List<Ped> threatenedPed;
        private static List<Ped> robbedPed;

        private static Ped[] surroundingPeds;
        private static int surroundPedsTimer;

        public static void AddThreatenedPed(Ped ped)
        {
            if (threatenedPed.Count > 20) threatenedPed.RemoveAt(threatenedPed.Count - 1);
            threatenedPed.Add(ped);
        }

        public static void CancelThreatenedPed(Ped ped)
        {
            threatenedPed.Remove(ped);
        }

        public static void AddRobbedPed(Ped ped)
        {
            if (robbedPed.Count > 20) robbedPed.RemoveAt(robbedPed.Count - 1);
            robbedPed.Add(ped);
            threatenedPed.Remove(ped);
        }

        public RobPeopleScript()
        {
            Pause();

            threatenedPed = new List<Ped>();
            robbedPed = new List<Ped>();

            Tick += RobPedScript_Tick;
        }

        private void RobPedScript_Tick(object sender, EventArgs e)
        {
            if (Game.IsLoading || Game.IsPaused) return;

            if (Game.GameTime > surroundPedsTimer + 1000)
            {
                surroundingPeds = World.GetNearbyPeds(Game.Player.Character, 20f);
                surroundPedsTimer = Game.GameTime;

                if (Main.configSettings.Randomize_People_Money)
                {
                    int maxMoney = Main.configSettings.Randomize_People_MaxMoney;
                    if (maxMoney < 1) maxMoney = 1;
                    if (maxMoney > 999999) maxMoney = 999999;
                    foreach (Ped ped in surroundingPeds)
                    {
                        if (ped.IsAlive && !threatenedPed.Contains(ped) && !robbedPed.Contains(ped))
                            ped.Money = Helpers.Random.Next(0, maxMoney);
                    }
                }
            }

            if (Main.configSettings.Debug_Mode)
            {
                foreach (Ped ped in surroundingPeds)
                {
                    if (ped != null && ped.IsAlive && ped.IsHuman && (ped.IsOnBike || ped.IsOnFoot) && CanRobModel(ped.Model))
                    {
                        Helpers.DrawText3D(ped.Position, new Vector3(0, 0, -1.1f), "$" + ped.Money.ToString());
                    }
                }
                
                Helpers.DrawText(new Vector2(0.015f, 0.1f), "ThreathenPeds List Count: " + threatenedPed.Count.ToString());
                Helpers.DrawText(new Vector2(0.015f, 0.12f), "RobbedPeds List Count: " + robbedPed.Count.ToString());
            }

            if (CanPlayerRobPeds())
            {
                Game.DisableControlThisFrame(Control.Talk);

                Entity entity = Game.Player.TargetedEntity;
                if (entity != null && entity.Model.IsPed)
                {
                    Ped ped = (Ped)entity;

                    if (CanPedBeRobbed(ped))
                    {
                        ThreathenPed(ped);
                    }

                    if (ped != null)
                    {
                        if (Game.IsControlJustPressed(Control.Talk) && !Function.Call<bool>(Hash.IS_AMBIENT_SPEECH_PLAYING, ped))
                        {
                            Function.Call(Hash._PLAY_AMBIENT_SPEECH1, Game.Player.Character, "STAY_DOWN", "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL");
                        }
                    }
                }
            }
        }

        public bool CanPlayerRobPeds()
        {
            Ped playerPed = Game.Player.Character;

            if (!playerPed.IsAlive ||
                !playerPed.IsAiming ||
                playerPed.IsRagdoll ||
                Game.Player.Character.IsInMeleeCombat ||
                !CanRobWithWeapon(playerPed.Weapons.Current) ||
                (!playerPed.IsOnFoot && !playerPed.IsOnBike))
                return false;
            return true;
        }

        public bool CanRobWithWeapon(Weapon weapon)
        {
            if (weapon.Group != WeaponGroup.Unarmed &&
                weapon.Group != WeaponGroup.Thrown &&
                weapon.Group != WeaponGroup.PetrolCan &&
                weapon.Group != WeaponGroup.NightVision &&
                weapon.Group != WeaponGroup.Melee &&
                weapon.Group != WeaponGroup.FireExtinguisher &&
                weapon.Group != WeaponGroup.DigiScanner &&
                weapon.Group != WeaponGroup.Parachute)
                return true;
            return false;
        }

        public bool PedIsOk(Ped ped)
        {
            if (ped == null ||
                !ped.IsAlive ||
                !ped.IsHuman ||
                ped.IsPersistent ||
                ped.IsOnFire ||
                ped.IsInWater ||
                ped.IsInMeleeCombat ||
                (!ped.IsOnFoot && !ped.IsOnBike) ||
                (ped.IsOnBike && ped.CurrentVehicle.Speed > 3f))
                return false;
            return true;
        }

        public bool PedPositionOk(Ped ped)
        {
            if (ped.Position.DistanceTo(Game.Player.Character.Position) > 20f)
                return false;
            return true;
        }

        public bool CanPedBeRobbed(Ped ped)
        {
            if (!PedIsOk(ped) ||
                !CanRobModel(ped.Model) ||
                !CanRobRelationship(ped.RelationshipGroup) ||
                !PedPositionOk(ped) ||
                ped.IsInCombatAgainst(Game.Player.Character) ||
                threatenedPed.Contains(ped) ||
                robbedPed.Contains(ped))
                return false;
            return true;
        }

        public bool CanRobModel(Model model)
        {
            if (model == PedHash.Cop01SFY ||
                model == PedHash.Cop01SMY ||
                model == PedHash.ShopHighSFM ||
                model == PedHash.ShopKeep01 ||
                model == PedHash.ShopLowSFY ||
                model == PedHash.ShopMidSFY)
                return false;
            return true;
        }

        public bool CanRobRelationship(RelationshipGroup group)
        {
            Relationship relation = group.GetRelationshipBetweenGroups(Game.Player.Character.RelationshipGroup.Hash);
            if (relation == Relationship.Companion)
                return false;
            return true;
        }

        public void ThreathenPed(Ped ped)
        {
            bool threathen = false;

            if (!Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT, ped, Game.Player.Character))
            {
                Game.DisableControlThisFrame(Control.Talk);
                if (Game.IsControlJustPressed(Control.Talk))
                {
                    threathen = true;
                    Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
                }
                else Screen.ShowHelpTextThisFrame("Press ~INPUT_TALK~ to threathen.");
            }
            else threathen = true;

            if (threathen)
            {
                Function.Call(Hash._PLAY_AMBIENT_SPEECH1, Game.Player.Character, "STAY_DOWN", "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL");
                RobPedSequenceScript sequence = InstantiateScript<RobPedSequenceScript>();
                sequence.SetTargetPed(ped);
                AddThreatenedPed(ped);
            }
        }
    }
}
