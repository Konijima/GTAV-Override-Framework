using Newtonsoft.Json;
using GTA;
using GTA.Native;

namespace GTAVOverride.Data
{
    public class VehicleJson
    {
        [JsonIgnore]
        private Vehicle _vehicle;

        public bool PlayerInside { get; set; }
        public int Hash { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Heading { get; set; }
        public float Health { get; set; }
        public float BodyHealth { get; set; }
        public float EngineHealth { get; set; }
        public float PetrolTankHealth { get; set; }
        public bool IsStolen { get; set; }
        public bool IsSirenActive { get; set; }
        public bool IsEngineRunning { get; set; }
        public bool IsInteriorLightOn { get; set; }
        public bool AreLightsOn { get; set; }
        public VehicleLockStatus LockStatus { get; set; }
        public VehicleColor PrimaryColor { get; set; }
        public VehicleColor SecondaryColor { get; set; }
        public VehicleColor RimColor { get; set; }
        public VehicleColor DashboardColor { get; set; }
        public System.Drawing.Color TireSmokeColor { get; set; }
        public int ColorCombination { get; set; }
        public int Livery { get; set; }
        public string LicensePlate { get; set; }
        public string RadioStation { get; set; }

        public void SetVehicle(Vehicle vehicle)
        {
            _vehicle = vehicle;

            PlayerInside = Game.Player.Character.IsInVehicle(_vehicle);

            Hash = _vehicle.Model.Hash;
            X = _vehicle.Position.X;
            Y = _vehicle.Position.Y;
            Z = _vehicle.Position.Z;
            Heading = _vehicle.Heading;
            Health = _vehicle.HealthFloat;
            BodyHealth = _vehicle.BodyHealth;
            EngineHealth = _vehicle.EngineHealth;
            PetrolTankHealth = _vehicle.PetrolTankHealth;
            IsStolen = _vehicle.IsStolen;
            IsSirenActive = _vehicle.IsSirenActive;
            IsEngineRunning = _vehicle.IsEngineRunning;
            IsInteriorLightOn = _vehicle.IsInteriorLightOn;
            AreLightsOn = _vehicle.AreLightsOn;
            LockStatus = _vehicle.LockStatus;

            PrimaryColor = _vehicle.Mods.PrimaryColor;
            SecondaryColor = _vehicle.Mods.SecondaryColor;
            RimColor = _vehicle.Mods.RimColor;
            DashboardColor = _vehicle.Mods.DashboardColor;
            TireSmokeColor = _vehicle.Mods.TireSmokeColor;
            ColorCombination = _vehicle.Mods.ColorCombination;
            Livery = _vehicle.Mods.Livery;
            LicensePlate = _vehicle.Mods.LicensePlate;
            RadioStation = Function.Call<string>(GTA.Native.Hash.GET_RADIO_STATION_NAME, Game.RadioStation);
        }

        public string ToJson()
        {
            Debug.Log("Vehicle to json...");

            SetVehicle(_vehicle);

            return JsonConvert.SerializeObject(this);
        }

        public static VehicleJson FromJson(string json)
        {
            Debug.Log("Loading vehicle from json...");

            VehicleJson vehicleJson = JsonConvert.DeserializeObject<VehicleJson>(json);

            return vehicleJson;
        }
    }
}
