using System;
using System.Collections.Generic;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using GTAVOverride.Classes;
using GTAVOverride.Managers;

namespace GTAVOverride.Scripts
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class AtmScript : Script
    {
        private ATM _nearbyATM;
        private bool _usingAtm = false;
        private bool _toggleHide = false;
        private int _updateNearestTimer = 0;

        public AtmScript()
        {
            Pause();

            Tick += AtmManager_Tick;
            Aborted += AtmManager_Aborted;
        }

        private void AtmManager_Aborted(object sender, EventArgs e)
        {
            if (_usingAtm)
            {
                Game.Player.Character.Task.ClearAllImmediately();
            }
        }

        private void UpdateNearbyATM()
        {
            if (_nearbyATM == null) return;

            if (Game.Player.Character.Position.DistanceTo(_nearbyATM.position) < 1f)
            {
                if (Game.IsControlJustPressed(Control.Talk))
                {
                    if (_usingAtm)
                    {
                        Game.Player.Character.Task.ClearAll();
                        Game.Player.Character.Task.ClearSecondary();
                        Wait(7000);
                        _usingAtm = false;
                    }
                    else
                    {
                        Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
                        Wait(200);
                        Game.Player.Character.Task.GoStraightTo(_nearbyATM.position, -1, _nearbyATM.heading, 600);
                        Wait(600);
                        Vector3 newPos = Game.Player.Character.Position - Game.Player.Character.ForwardVector * 0.15f;
                        Function.Call(Hash.TASK_START_SCENARIO_AT_POSITION, Game.Player.Character, "PROP_HUMAN_ATM", newPos.X, newPos.Y, newPos.Z, _nearbyATM.heading, 600000, false, false);
                        Wait(7000);
                        _usingAtm = true;
                    }
                }
                else
                {
                    if (_usingAtm)
                    {
                        Screen.ShowHelpTextThisFrame("Press ~INPUT_TALK~ to stop using the ATM.");
                    }
                    else
                    {
                        Screen.ShowHelpTextThisFrame("Press ~INPUT_TALK~ to use the ATM.");
                    }
                }
            }
            else
            {
                _nearbyATM = null;
            }
        }

        private void AtmManager_Tick(object sender, EventArgs e)
        {
            if (!_toggleHide && Game.Player.WantedLevel > 0)
            {
                _toggleHide = true;
                AtmManager.HideATMBlips();
                return;
            }
            else if (_toggleHide && Game.Player.WantedLevel == 0)
            {
                _toggleHide = false;
                AtmManager.ShowATMBlips();
            }

            if (_toggleHide ||
                Game.IsLoading ||
                !Game.Player.Character.IsAlive ||
                !Game.Player.Character.IsOnFoot) return;

            if (_nearbyATM != null)
            {
                UpdateNearbyATM();
            }
            else
            {
                if (Game.GameTime > _updateNearestTimer + 1000)
                {
                    _updateNearestTimer = Game.GameTime;
                    foreach (ATM atm in AtmManager.Atms)
                    {
                        if (Game.Player.Character.Position.DistanceTo(atm.position) < 1f)
                        {
                            _nearbyATM = atm;
                            break;
                        }
                    }
                }
            }
        }
    }
}
