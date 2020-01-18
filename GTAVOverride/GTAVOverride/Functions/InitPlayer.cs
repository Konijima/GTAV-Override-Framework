﻿using System;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using GTAVOverride.Data;
using GTAVOverride.Managers;

namespace GTAVOverride.Functions
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class InitPlayer : Script
    {
        private PlayerJson playerJson;
        private VehicleJson vehicleJson;
        private Vehicle vehicle;

        public void SpawnVehicle()
        {
            Model model = new Model(vehicleJson.Hash);
            while (!model.IsLoaded)
            {
                model.Request();
                Wait(1);
            }
            Debug.Log("Vehicle model has loaded!");

            vehicle = World.CreateVehicle(model, new Vector3(vehicleJson.X, vehicleJson.Y, vehicleJson.Z), vehicleJson.Heading);
            vehicle.HealthFloat = vehicleJson.Health;
            vehicle.BodyHealth = vehicleJson.BodyHealth;
            vehicle.EngineHealth = vehicleJson.EngineHealth;
            vehicle.PetrolTankHealth = vehicleJson.PetrolTankHealth;
            vehicle.IsStolen = vehicleJson.IsStolen;
            vehicle.IsSirenActive = vehicleJson.IsSirenActive;
            vehicle.IsEngineRunning = true;
            vehicle.IsInteriorLightOn = vehicleJson.IsInteriorLightOn;
            vehicle.LockStatus = vehicleJson.LockStatus;

            vehicle.Mods.PrimaryColor = vehicleJson.PrimaryColor;
            vehicle.Mods.SecondaryColor = vehicleJson.SecondaryColor;
            vehicle.Mods.ColorCombination = vehicleJson.ColorCombination;
            vehicle.Mods.Livery = vehicleJson.Livery;
            vehicle.Mods.LicensePlate = vehicleJson.LicensePlate;
            Function.Call(Hash.SET_VEH_RADIO_STATION, vehicle, vehicleJson.RadioStation);
            Function.Call(Hash.SET_RADIO_TO_STATION_NAME, vehicleJson.RadioStation);
            vehicle.Mods.RimColor = vehicleJson.RimColor;
            vehicle.Mods.DashboardColor = vehicleJson.DashboardColor;
            vehicle.Mods.TireSmokeColor = vehicleJson.TireSmokeColor;

            Debug.Log("Vehicle spawned!");

            while (!model.IsCollisionLoaded)
            {
                model.RequestCollision();
                Wait(1);
            }
            Debug.Log("Vehicle collision has loaded!");

            model.MarkAsNoLongerNeeded();
        }

        public void SetPlayerOnFoot()
        {
            Function.Call(Hash.START_PLAYER_TELEPORT, Game.Player, playerJson.X, playerJson.Y, playerJson.Z, playerJson.Heading, false, false, false);
            while (!Function.Call<bool>(Hash.IS_PLAYER_TELEPORT_ACTIVE)) Wait(1);

            Wait(1000);
        }

        public void SetPlayerInVehicle()
        {
            if (vehicle != null)
            {
                Game.Player.Character.Task.ClearAllImmediately();
                Game.Player.Character.Task.WarpIntoVehicle(vehicle, VehicleSeat.Driver);
                Wait(1000);
            }
        }

        public void Start(PlayerJson playerJson)
        {
            this.playerJson = playerJson;
            this.vehicleJson = playerJson.CurrentVehicle;

            Tick += InitPlayer_Tick;

            Debug.Log("Starting init player...");
        }

        private void InitPlayer_Tick(object sender, EventArgs e)
        {
            // step 1
            if (Game.Player.Character.Model.Hash != playerJson.Hash)
            {
                Debug.Log("Player model need to be changed!");

                Model model = new Model(playerJson.Hash);
                model.Request();
                while (!model.IsLoaded) Wait(1);
                Debug.Log("Player model has loaded!");

                Game.Player.ChangeModel(model);
                Debug.Log("Player model has been changed!");
            }
            else Debug.Log("Player model did not need to change!");

            // current player car right now
            Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;

            // player data has no vehicle
            if (vehicleJson == null)
            {
                // player is not in a vehicle right now
                if (currentVehicle == null)
                {

                }
                // player is in a vehicle right now
                else
                {
                    currentVehicle.IsPersistent = true;
                    currentVehicle.Delete();
                }

                SetPlayerOnFoot();

            }
            // player data has a vehicle
            else
            {
                // player is not in a vehicle right now
                if (currentVehicle == null)
                {
                    SpawnVehicle();

                    if (vehicleJson.PlayerInside) SetPlayerInVehicle();
                    else SetPlayerOnFoot();
                }
                // player is in a vehicle right now
                else
                {
                    vehicle = Game.Player.Character.CurrentVehicle;
                }

                PlayerManager.ReplacePersonalVehicle(vehicle);
            }

            // end
            Debug.Log("Player init completed!");

            OnCompleted(new EventArgs());

            GameplayCamera.RelativeHeading = 0f;
            GameplayCamera.RelativePitch = 0f;

            Screen.FadeIn(200);

            if (Main.configSettings.Info_Intro_Screen)
            {
                IntroScreen intro = Script.InstantiateScript<IntroScreen>();
                intro.Start();
            }

            Abort();
        }

        protected virtual void OnCompleted(EventArgs e)
        {
            Completed?.Invoke(this, e);
        }
        public event EventHandler<EventArgs> Completed;
    }
}