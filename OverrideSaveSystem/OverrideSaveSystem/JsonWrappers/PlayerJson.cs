using System;
using System.Collections.Generic;
using System.Text;
using GTA;
using GTA.Math;
using GTA.Native;
using Newtonsoft.Json;
using OverrideSaveSystem.Events;

namespace OverrideSaveSystem.JsonWrappers
{
    public class PlayerJson
    {
        public PlayerJson()
        {
            Update();
        }

        public string ToJson()
        {
            Update();
            return JsonConvert.SerializeObject(this);
        }

        public static PlayerJson FromJson(string json, bool autoSpawn = false)
        {
            PlayerJson playerJson = JsonConvert.DeserializeObject<PlayerJson>(json);

            if (autoSpawn) { playerJson.Spawn(); }

            return playerJson;
        }

        public void Update()
        {
            
        }

        public Ped Spawn()
        {
            SpawnedEventArgs e = new SpawnedEventArgs();
            e.ped = Game.Player.Character;
            OnSpawned(e);

            return Game.Player.Character;
        }

        protected virtual void OnSpawned(SpawnedEventArgs e)
        {
            EventHandler<SpawnedEventArgs> handler = Spawned;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler<SpawnedEventArgs> Spawned;
    }
}
