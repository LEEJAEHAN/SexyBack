namespace SexyBackPlayScene
{
    // 레벨업이 가능한 클래스, 레벨업ui에 표시되기위해 필요한정보도 필수적으로가져야한다.
    public abstract class CanLevelUp 
    {
        public abstract string ID { get; }
        public abstract string Name { get; } 

        public int LevelCount;

        public abstract string Item_Text { get; } // 아이템버튼 우하단 텍스트
        public abstract string Info_Description { get; } // 정보창 내용
        public abstract BigInteger PriceToNextLv { get; } // 정보창 내용

        public LevelUpItem levelupItem;
    }
}