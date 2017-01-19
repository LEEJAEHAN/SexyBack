namespace SexyBackPlayScene
{
    // 레벨업이 가능한 클래스, 레벨업ui에 표시되기위해 필요한정보도 필수적으로가져야한다.
    public interface CanLevelUp
    {
        string ID { get; }
        void LevelUp();
        bool LevelUp_isOK { get; }
    }
}