namespace SexyBackPlayScene
{
    // 레벨업이 가능한 클래스, 레벨업ui에 표시되기위해 필요한정보도 필수적으로가져야한다.
    public abstract class CanLevelUp 
    {
        public int level;
        public abstract string ItemViewID { get; }
        public abstract string Name { get; }
        public abstract string DamageStatusText { get; } 

        public abstract void LevelUp(int lv);
        public abstract string GetDamageString();
    }
}