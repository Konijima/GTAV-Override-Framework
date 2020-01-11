using System;
using System.Collections.Generic;
using System.Text;
using GTA;
using GTA.Math;
using GTA.Native;
using Newtonsoft.Json;
using OverrideSaveGame.Events;

namespace OverrideSaveGame.Converters
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

        public static PlayerJson FromJson(string json, bool autoTeleport = false)
        {
            PlayerJson playerJson = JsonConvert.DeserializeObject<PlayerJson>(json);

            if (autoTeleport) { playerJson.Teleport(); }

            return playerJson;
        }

        public void Update()
        {
            
        }

        public Ped Teleport()
        {
            TeleportedEventArgs e = new TeleportedEventArgs();
            e.player = Game.Player;
            OnTeleported(e);

            return Game.Player.Character;
        }

        protected virtual void OnTeleported(TeleportedEventArgs e)
        {
            Teleported?.Invoke(this, e);
        }
        public event EventHandler<TeleportedEventArgs> Teleported;
    }
}
