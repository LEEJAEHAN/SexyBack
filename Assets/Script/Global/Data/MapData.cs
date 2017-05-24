
public class MapData
{
    public string ID;
    public string Name;
    public string RequireClearMap;
    public int LimitTime;
    public MapRewardData RewardData;
    public int MaxFloor;
    public float GrowthRate;
    public int MonsterPerStage;
    public int BaseMonsterHP;
    public int BossHPX;
    public int ChestPerMonster;
    public int ChestPerBossMonster;

    public MapData()
    {
    }
}
public class MapRewardData
{
    public int RewardLevel;      // 보상의 질 레벨, 10층당 50레벨, 아이템과 리서치보상에서 참고한다.
    public int PrevRewardLevel;  // F랭크시 보상의 질 레벨.
    public int ReputationLevel;  // 시간효율 레벨, 가장쉬움1, 중간3, 하드9 순이며, 명성보상의 계수가된다.
    public int ItemCount;

    public MapRewardData(int rewardlevel, int prevlevel, int requtationlevel, int itemcount)
    {
        RewardLevel = rewardlevel;
        PrevRewardLevel = prevlevel;
        ReputationLevel = requtationlevel;
        ItemCount = itemcount;
    }

}