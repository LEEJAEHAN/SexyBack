namespace SexyBackPlayScene
{
    delegate void LevelUp_EventHandler(ICanLevelUp sender);

    internal interface ICanLevelUp
    {
        string GetID { get; }
        string LevelUpDescription { get; }
        int LEVEL { get; }
        BigInteger LevelUpPrice { get; }
        void LevelUp(int level);

        // event sender
        event LevelUp_EventHandler Action_LevelUp;

    }

}