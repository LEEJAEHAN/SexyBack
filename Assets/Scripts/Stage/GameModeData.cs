namespace SexyBackPlayScene
{
    internal class GameModeData
    {
        public  string ID;
        public int GoalFloor;

        public GameModeData(string stageid, int goal, int initexp)
        {
            ID = stageid;
            GoalFloor = goal;
        }
    }
}