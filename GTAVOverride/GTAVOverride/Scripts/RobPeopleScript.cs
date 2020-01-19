using System;
using System.Collections.Generic;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using GTAVOverride.Enums;
using GTAVOverride.Functions;
using GTAVOverride.Managers;

namespace GTAVOverride.Scripts
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class RobPeopleScript : Script
    {
        public RobPeopleScript()
        {
            Pause();

            PeopleManager.ThreatenedPeds = new List<Ped>();
            PeopleManager.RobbedPeds = new List<Ped>();

            Tick += RobPeopleScript_Tick;
        }

        private void RobPeopleScript_Tick(object sender, EventArgs e)
        {
            if (Game.IsLoading || Game.IsPaused) return;

            if (Helpers.CanPlayerRobPeople())
            {
                if (Game.Player.Character.IsAiming)
                {
                    Game.DisableControlThisFrame(Control.WeaponSpecial);
                    Game.DisableControlThisFrame(Control.WeaponSpecial2);
                    Game.DisableControlThisFrame(Control.VehicleHorn);
                    Game.DisableControlThisFrame(Control.Talk);
                
                    Entity entity = Game.Player.TargetedEntity;
                    if (entity != null && entity.Model.IsPed)
                    {
                        Ped ped = (Ped)entity;

                        if (ped != null)
                        {
                            if (CanPedBeRobbed(ped))
                            {
                                ThreathenPed(ped);
                            }

                            if (!Game.IsControlEnabled(Control.Talk) && Game.IsControlJustPressed(Control.Talk))
                            {
                                Helpers.PedSpeakThreathen(Game.Player.Character);
                            }
                        }
                    }
                    else
                    {
                        if (Function.Call<bool>(Hash.IS_HELP_MESSAGE_BEING_DISPLAYED))
                            Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
                    }
                }
            }
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
                !PedPositionOk(ped) ||
                !CanRobModel(ped.Model) ||
                !CanRobRelationship(ped.RelationshipGroup) ||
                ped.IsInCombatAgainst(Game.Player.Character) ||
                PeopleManager.ThreatenedPeds.Contains(ped) ||
                PeopleManager.RobbedPeds.Contains(ped))
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

            if (!Helpers.HasPedClearLineOfSighOfPed(ped, Game.Player.Character))
            {
                if (Game.IsControlJustPressed(Control.Talk))
                {
                    Interval = 0;
                    threathen = true;
                    Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
                }
                else Helpers.ShowHelpMessageThisFrame("Press ~INPUT_TALK~ to threathen.", false);
            }
            else threathen = true;

            if (threathen)
            {
                Helpers.PedSpeakThreathen(Game.Player.Character);
                RobPedSequence sequence = InstantiateScript<RobPedSequence>();
                sequence.SetTargetPed(ped);
                PeopleManager.AddThreatenedPed(ped);
            }
        }
    }
}
