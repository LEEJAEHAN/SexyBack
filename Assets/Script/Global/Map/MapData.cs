using System.Collections.Generic;

public class MapData
{
    public string ID;
    public string Name;
    public string RequireMap;
    public int LimitTime;
    public int MaxFloor;
    //public int Difficulty;  // 시간효율 레벨, 가장쉬움1, 중간3, 하드9 순이며, 명성보상의 계수가된다.
    public MapRewardData MapReward;
    public MapMonsterData MapMonster;

    public MapData()
    {
        MapReward = new MapRewardData();
        MapMonster = new MapMonsterData();
    }
}
public class MapRewardData
{
    //public int Level;      // 보상의 질 레벨, 10층당 50레벨, 아이템과 리서치보상에서 참고한다.
    //public int PrevLevel;  // F랭크시 보상의 질 레벨.
    public int ItemCount;
    public List<string> FixCandidates;

    public MapRewardData()
    {
        FixCandidates = new List<string>();
    }
}
public class MapMonsterData
{
    public int LevelPerFloor;// float GrowthRate;

    public int[] HP = new int[2];
    public int[] Chest = new int[2];

    //public int MonsterPerStage;
    public int BossTerm;
}