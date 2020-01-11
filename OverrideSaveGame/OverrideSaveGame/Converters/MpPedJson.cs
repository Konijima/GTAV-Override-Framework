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
    public class MpPedJson
    {
        [JsonIgnore]
        private Ped _ped;

        public int Hash;
        public float X;
        public float Y;
        public float Z;
        public float Heading;
        public int MaxHealth;
        public int Health;
        public int Armor;
        public int Accuracy;
        public bool CanRagdoll;
        public bool CanSufferCriticalHits;
        public bool CanWrithe;
        public bool HasGravity;
        public bool DropsEquippedWeaponOnDeath;
        public float FatalInjuryHealthThreshold;
        public float InjuryHealthThreshold;
        public int LodDistance;
        public int Money;
        public int Opacity;
        public int RelationshipGroupHash;
        public float Sweat;
        public List<WeaponHash> Weapons = new List<WeaponHash>();

        public MpPedJson(Ped ped)
        {
            if (!isValidModel(ped.Model))
            {
                throw new Exception("Can only use MpPedJson class with FreemodeMale01 and FreemodeFemale01 model.");
            }
            _ped = ped;
            Update();
        }

        [JsonIgnore]
        public Ped pedInstance
        {
            get
            {
                return _ped;
            }
        }

        [JsonIgnore]
        public bool isValid
        {
            get
            {
                if (_ped != null && _ped.Exists())
                {
                    return true;
                }
                return false;
            }
        }

        public string ToJson()
        {
            if (isValid)
            {
                Update();
                return JsonConvert.SerializeObject(this);
            }

            return null;
        }

        public static MpPedJson FromJson(string json, bool autoSpawn = false)
        {
            MpPedJson newPedJson = JsonConvert.DeserializeObject<MpPedJson>(json);

            if (autoSpawn) { newPedJson.Spawn(); }
                
            return newPedJson;
        }

        public static bool isValidModel(Model pedModel)
        {
            if (!pedModel.Hash.Equals(PedHash.FreemodeMale01) || !pedModel.Hash.Equals(PedHash.FreemodeFemale01))
            {
                return false;
            }
            return true;
        }

        public void Update()
        {
            if (_ped != null && _ped.Exists())
            {
                Hash = _ped.Model.Hash;
                X = _ped.Position.X;
                Y = _ped.Position.Y;
                Z = _ped.Position.Z;
                Heading = _ped.Heading;
                MaxHealth = _ped.MaxHealth;
                Health = _ped.Health;
                Armor = _ped.Armor;
                Accuracy = _ped.Accuracy;
                CanRagdoll = _ped.CanRagdoll;
                CanSufferCriticalHits = _ped.CanSufferCriticalHits;
                CanWrithe = _ped.CanWrithe;
                HasGravity = _ped.HasGravity;
                DropsEquippedWeaponOnDeath = _ped.DropsEquippedWeaponOnDeath;
                FatalInjuryHealthThreshold = _ped.FatalInjuryHealthThreshold;
                InjuryHealthThreshold = _ped.InjuryHealthThreshold;
                LodDistance = _ped.LodDistance;
                Money = _ped.Money;
                Opacity = _ped.Opacity;
                RelationshipGroupHash = _ped.RelationshipGroup.Hash;
                Sweat = _ped.Sweat;

                if (Weapons == null)
                {
                    Weapons = new List<WeaponHash>();
                }
                foreach (WeaponHash hash in (WeaponHash[])Enum.GetValues(typeof(WeaponHash)))
                {
                    if (_ped.Weapons.HasWeapon(hash))
                    {
                        if (!Weapons.Contains(hash))
                        {
                            Weapons.Add(hash);
                        }
                    }
                }
            }
        }

        public Ped Spawn()
        {
            if (_ped == null)
            {
                Model model = new Model(Hash);
                Vector3 position = new Vector3(X, Y, Z);
                RelationshipGroup relationshipGroup = new RelationshipGroup(RelationshipGroupHash);

                Function.Call(GTA.Native.Hash.CLEAR_AREA_OF_PEDS, X, Y, Z, 1f, 1);

                Ped ped = World.CreatePed(model, position, Heading);

                Function.Call(GTA.Native.Hash.SET_ENTITY_AS_MISSION_ENTITY, ped.Handle, false, false);
                Function.Call(GTA.Native.Hash.SET_PED_AS_NO_LONGER_NEEDED, ped.Handle);

                ped.MaxHealth = MaxHealth;
                ped.Health = Health;
                ped.Armor = Armor;
                ped.Accuracy = Accuracy;
                ped.CanRagdoll = CanRagdoll;
                ped.CanSufferCriticalHits = CanSufferCriticalHits;
                ped.CanWrithe = CanWrithe;
                ped.HasGravity = HasGravity;
                ped.DropsEquippedWeaponOnDeath = DropsEquippedWeaponOnDeath;
                ped.FatalInjuryHealthThreshold = FatalInjuryHealthThreshold;
                ped.InjuryHealthThreshold = InjuryHealthThreshold;
                ped.LodDistance = LodDistance;
                ped.Money = Money;
                ped.Opacity = Opacity;
                ped.Opacity = Opacity;
                ped.Sweat = Sweat;
                ped.RelationshipGroup = relationshipGroup;

                foreach(WeaponHash hash in Weapons)
                {
                    ped.Weapons.Give(hash, 0, true, false);
                }

                _ped = ped;

                SpawnedEventArgs e = new SpawnedEventArgs();
                e.ped = ped;
                OnSpawned(e);

                return ped;
            }

            return null;
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