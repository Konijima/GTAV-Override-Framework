using System;
using GTA;
using GTA.Math;
using GTA.Native;
using GTAVOverride.Managers;
using GTAVOverride.Scripts;

namespace GTAVOverride.Functions
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class RobPedSequence : Script
    {
        public static readonly Random Random = new Random();

        private float intimidationSpeed;
        private readonly float maxIntimidationSpeed;
        private int tolerence;
        private Ped targetPed;
        private int state;

        private int reward = 0;
        private bool canceled = false;
        private bool droppedLoot = false;
        private float totalIntimidation = 0;
        private float maxAngle = 120f;

        public RobPedSequence()
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

                Tick += RobPedSequence_Tick;
            }
        }

        private void SpawnPedMoney(Ped targetPed)
        {
            if (targetPed.Money <= 0) return;

            if (Main.configRobPeople.Quick_Rob_People_No_Pickup)
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

            World.CreateAmbientPickup(PickupType.MoneyPurse, targetPed.Position + targetPed.ForwardVector * 0.5f, wallet, targetPed.Money);
            wallet.MarkAsNoLongerNeeded();
        }

        private void RobPedSequence_Tick(object sender, EventArgs e)
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

                        Helpers.PedSpeakScared(targetPed);

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
                            Game.Player.Character.Position.DistanceTo(targetPed.Position) < Main.configRobPeople.Distance_Of_Reach)
                        {
                            Wait(1);

                            Helpers.PedSpeakShocked(targetPed);

                            // Update Max Angle
                            if (Game.Player.Character.IsInVehicle()) maxAngle = 240f;
                            else maxAngle = 90f;

                            if (Game.Player.Character.IsAiming)
                            {
                                Game.DisableControlThisFrame(Control.WeaponSpecial);
                                Game.DisableControlThisFrame(Control.WeaponSpecial2);
                                Game.DisableControlThisFrame(Control.VehicleHorn);
                                Game.DisableControlThisFrame(Control.Talk);

                                // if directly targeting lets intimidate faster
                                if (Game.Player.IsTargeting(targetPed))
                                {
                                    intimidationSpeed = 0.8f;

                                    if (Helpers.IsPedSpeaking(Game.Player.Character))
                                        intimidationSpeed = 1.8f;
                                }
                                else
                                {
                                    intimidationSpeed = 0.1f;

                                    if (Helpers.IsPedSpeaking(Game.Player.Character))
                                        intimidationSpeed = 0.6f;
                                }
                            }
                            else
                            {
                                intimidationSpeed -= 5f * Game.LastFrameTime;
                            }
                            if (intimidationSpeed > maxIntimidationSpeed) intimidationSpeed = maxIntimidationSpeed;

                            if (Main.configSettings.Debug_Mode)
                            {
                                Helpers.DrawText3D(targetPed.Position, new Vector3(0, 0, -1.6f), Helpers.GetPedSocialClass(targetPed).ToString());
                                Helpers.DrawText3D(targetPed.Position, new Vector3(0, 0, -1.45f), intimidationSpeed.ToString());
                                Helpers.DrawText3D(targetPed.Position, new Vector3(0, 0, -1.3f), totalIntimidation.ToString("0") + "/" + tolerence.ToString("0"));
                            }

                            if (targetPed.IsInVehicle())
                            {
                                targetPed.Task.LeaveVehicle(LeaveVehicleFlags.LeaveDoorOpen);
                            }
                            else
                            {
                                // Update Handup
                                if (!Helpers.HasPedHandsUp(targetPed))
                                    Helpers.StartPedHandsUp(targetPed, Game.Player.Character, 2000);
                                else
                                    Helpers.UpdatePedHandsUp(targetPed, 2000);
                            }

                            if (!targetPed.IsRagdoll && !targetPed.IsGettingUp)
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
                            while (Helpers.IsPedSpeaking(targetPed)) Wait(0);
                            Helpers.PedSpeakScared(targetPed);

                            targetPed.Task.FleeFrom(Game.Player.Character);
                            PeopleManager.CancelThreatenedPed(targetPed);
                        }
                        else if (droppedLoot)
                        {
                            PeopleManager.AddRobbedPed(targetPed);

                            while (Helpers.IsPedSpeaking(targetPed)) Wait(1);
                            Helpers.PedSpeakScared(targetPed);

                            if (targetPed.Gender == Gender.Male) targetPed.Task.FleeFrom(Game.Player.Character);
                            else targetPed.Task.ReactAndFlee(Game.Player.Character);

                            if (!Helpers.IsPedSpeaking(Game.Player.Character))
                            {
                                Wait(333);
                                if (reward == 0) Helpers.PedSpeakThreathenFailed(Game.Player.Character);
                                else Helpers.PedSpeakThreathenSuccess(Game.Player.Character);
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
}
