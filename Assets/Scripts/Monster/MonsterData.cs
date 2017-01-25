namespace SexyBackPlayScene
{
    internal class MonsterData
    {
        public string ID;
        public string Name;
        public BigInteger MaxHP;
        public string SpritePath;

        public MonsterData(string id, string name, string spritepath, BigInteger maxhp)
        {
            ID = id;
            Name = name;
            MaxHP = maxhp;
            SpritePath = spritepath;
        }
    }
}