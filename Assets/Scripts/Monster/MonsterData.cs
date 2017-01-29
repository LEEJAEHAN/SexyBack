using UnityEngine;

namespace SexyBackPlayScene
{
    internal class MonsterData
    {
        public string ID;
        public string Name;
        public BigInteger MaxHP;
        public string SpritePath;
        internal Vector3 LocalPosition;

        public MonsterData(string id, string name, string spritepath, float x, float y, BigInteger maxhp)
        {
            ID = id;
            Name = name;
            MaxHP = maxhp;
            SpritePath = spritepath;
            LocalPosition = new Vector3(x, y, 0);   
        }
    }
}