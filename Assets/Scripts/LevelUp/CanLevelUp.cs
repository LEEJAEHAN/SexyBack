namespace SexyBackPlayScene
{
    public abstract class CanLevelUp
    {
        public int level;
        public abstract string ItemViewID { get; }

        public abstract void LevelUp(int lv);
        public abstract string GetDamageString();
    }
}