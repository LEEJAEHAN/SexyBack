namespace SexyBackPlayScene
{
    internal class MonsterData
    {
        public string ID;
        public BigInteger MaxHP;
        public string SpritePath;

        public MonsterData(string id, string spritepath, BigInteger maxhp)
        {
            ID = id;
            MaxHP = maxhp;
            SpritePath = spritepath;
        }

    }
}