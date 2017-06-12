using UnityEngine;

namespace SexyBackPlayScene
{
    internal class MonsterData
    {
        //public static double GrowthRate = 1.148698f; // / 100

        public readonly string ID;
        public readonly string Name;
        public readonly int MinFloor = 0;
        public readonly int MaxFloor = 10000;
        //public BigInteger MaxHP;
        public readonly string SpritePath;
        public readonly Vector3 LocalPosition;

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