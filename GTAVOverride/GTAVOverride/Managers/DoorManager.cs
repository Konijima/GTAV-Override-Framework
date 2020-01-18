using System.Collections.Generic;
using GTA.Math;
using GTAVOverride.Classes;

namespace GTAVOverride.Managers
{
    public static class DoorManager
    {
        public static List<Door> doors = new List<Door>();

        public static void CreateDoors()
        {
            Debug.Log("Creating Doors...");

            doors.Add(new Door("Prologue", "v_ilev_cd_door", 1438783233, new Vector3(5307.52f, -5204.539f, 83.6686f)));
            doors.Add(new Door("Prologue", "v_ilev_cd_door", 1017370378, new Vector3(5310.12f, -5204.54f, 83.67f)));
            doors.Add(new Door("Prologue", "v_ilev_cd_door2", -551608542, new Vector3(5316.65f, -5205.74f, 83.67f)));
            doors.Add(new Door("Prologue", "v_ilev_cd_door3", -551608542, new Vector3(5305.46f, -5177.75f, 83.67f)));
            // doors.Add(new Door("Prologue", "v_ilev_cd_entrydoor", 348593192, new Vector3(5318.18f, -5185.113f, 83.66864f)));

            // Ammunation PillboxHill
            doors.Add(new Door("Ammunation01", "v_ilev_gc_door04", 97297972, new Vector3(16.12787f, -1114.606f, 29.94694f)));
            doors.Add(new Door("Ammunation01", "v_ilev_gc_door03", -8873588, new Vector3(18.572f, -1115.495f, 29.94694f)));

            // Ammunation SandyShores
            doors.Add(new Door("Ammunation02", "v_ilev_gc_door04", 97297972, new Vector3(1698.17626953125f, 3751.505615234375f, 34.8553f)));
            doors.Add(new Door("Ammunation02", "v_ilev_gc_door03", -8873588, new Vector3(1699.9371337890625f, 3753.420166015625f, 34.8553f)));

            // Ammunation Vinewood/hawick
            doors.Add(new Door("Ammunation03", "v_ilev_gc_door04", 97297972, new Vector3(244.72740173339844f, -44.0791015625f, 70.91f)));
            doors.Add(new Door("Ammunation03", "v_ilev_gc_door03", -8873588, new Vector3(243.83790588378906f, -46.5232f, 70.91f)));

            // Ammunation La mesa
            doors.Add(new Door("Ammunation04", "v_ilev_gc_door04", 97297972, new Vector3(845.3624267578125f, -1024.5390625f, 28.3448f)));
            doors.Add(new Door("Ammunation04", "v_ilev_gc_door03", -8873588, new Vector3(842.7683715820312f, -1024.5390625f, 23.3448f)));

            // https://pastebin.com/gywnbzsH

            //Game.Player.Character.PositionNoOffset = new Vector3(845.362426757812f, -1024.5390625f, 28.3448f);
        }

        public static List<Door> GetDoorsById(string id)
        {
            List<Door> _doors = new List<Door>();
            foreach (Door door in doors)
            {
                if (door.id == id)
                {
                    _doors.Add(door);
                }
            }
            return _doors;
        }

        public static void OpenAll()
        {
            foreach (Door door in doors)
            {
                door.Open();
            }
            Debug.Log("Opening all Doors...");
        }

        public static void CloseAll()
        {
            foreach (Door door in doors)
            {
                door.Close();
            }
            Debug.Log("Closing all Doors...");
        }

        public static void UnlockAll()
        {
            foreach (Door door in doors)
            {
                door.Unlock();
            }
            Debug.Log("Unlocking all Doors...");
        }

        public static void LockAll()
        {
            foreach (Door door in doors)
            {
                door.Lock();
            }
            Debug.Log("Locking all Doors...");
        }
    }
}
