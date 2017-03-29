namespace SexyBackPlayScene
{
    internal class MapData
    {
        public string ID;
        public string Name;
        public int LimitTime;
        public int MaxFloor;

        public bool isDoubled;

        public MapData(string stageid, string name, int maxfloor, int limitTime)
        {
            ID = stageid;
            this.Name = name;
            LimitTime = limitTime;
            MaxFloor = maxfloor;
        }
    }
}