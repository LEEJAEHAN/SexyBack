namespace SexyBackPlayScene
{
    delegate void LevelUp_EventHandler(ICanLevelUp sender);

    internal interface ICanLevelUp
    {
        string GetID { get; }
           int LEVEL { get; }
        BigInteger LevelUpPrice { get; }
        string LevelUpDamageText { get; }
        string LevelUpNextText { get; }
        void LevelUp(int level);

        // event sender
        event LevelUp_EventHandler Action_LevelUpInfoChange;
    }

}