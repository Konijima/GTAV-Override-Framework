using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using GTA;
using GTA.Native;

namespace GTAVOverride.Data
{
    public class WeaponJson
    {
        public WeaponHash hash;
        public int ammo;
    }

    public class LoadoutJson
    {
        [JsonIgnore]
        private Ped _ped;

        public List<WeaponJson> ownedWeapons = new List<WeaponJson>();

        public void SetWeapons(Ped ped)
        {
            _ped = ped;

            Weapon current = Game.Player.Character.Weapons.Current;
            foreach (WeaponHash weapon in (WeaponHash[])Enum.GetValues(typeof(WeaponHash)))
            {
                if (_ped.Weapons.HasWeapon(weapon))
                {
                    _ped.Weapons.Select(weapon, false);
                    //int ammoType = Helpers.GetAmmoTypeFromPedWeapon(_ped, weapon);
                    int ammo = _ped.Weapons.Current.Ammo;
                    WeaponJson weaponJson = new WeaponJson
                    {
                        hash = weapon,
                        ammo = ammo,
                    };
                    ownedWeapons.Add(weaponJson);
                }
            }
            _ped.Weapons.Select(current, false);
        }

        public string ToJson()
        {
            Debug.Log("Weapon loadout to json...");

            SetWeapons(_ped);

            return JsonConvert.SerializeObject(this);
        }

        public static WeaponJson FromJson(string json)
        {
            Debug.Log("Loading weapons loadout from json...");

            WeaponJson weaponJson = JsonConvert.DeserializeObject<WeaponJson>(json);

            return weaponJson;
        }
    }
}
