using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAVOverride.Classes
{
    public class Door
    {
        public string id;
        public string name;
        public int hash;
        public Vector3 position;

        public Door(string id, string name, int hash, Vector3 position)
        {
            this.id = id;
            this.name = name;
            this.hash = hash;
            this.position = position;
        }

        public void Open()
        {

        }

        public void Close()
        {

        }

        public void Unlock()
        {
            Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Game.GenerateHash(name), position.X, position.Y, position.Z, false, 1f, 0);
        }

        public void Lock()
        {
            Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Game.GenerateHash(name), position.X, position.Y, position.Z, true, 0f, 0);
        }
    }
}
