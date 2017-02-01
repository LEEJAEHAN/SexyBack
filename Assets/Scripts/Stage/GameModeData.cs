namespace SexyBackPlayScene
{
    internal class GameModeData
    {
        public  string ID;
        public int GoalFloor;
        public BigInteger InitExp;

        public GameModeData(string stageid, int goal, int initexp)
        {
            ID = stageid;
            GoalFloor = goal;
            InitExp = new BigInteger(initexp);
        }
    }
}