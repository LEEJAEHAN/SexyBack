using UnityEngine;

namespace SexyBackPlayScene
{
    internal class MonsterData
    {
        public readonly string ID;
        public readonly string Name;
        public readonly int MinFloor = 0;
        public readonly int MaxFloor = 100;
        public readonly float GrowthRate = 2f;
        //public BigInteger MaxHP;
        public readonly string SpritePath;
        public readonly Vector3 LocalPosition;
        public int baseHP = 20;

        public MonsterData(string id, string name, float x, float y)
        {
            ID = id;
            Name = name;
//            MaxHP = maxhp;
            SpritePath = "Sprites/Monster/" + id;
            LocalPosition = new Vector3(x, y, 0);   
        }
    }
}