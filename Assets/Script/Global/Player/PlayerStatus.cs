using System;
using SexyBackRewardScene;
using System.Collections.Generic;


internal class PlayerStatus
{
    // 진행중인 맵
    public string mapID;
    public bool boost;

    //base stat
    BaseStat baseStat;
    //special stat ( from passive, equipment enchant, in game research.. etc )
    PlayerStat playerStat;
    HeroStat heroStat;
    Dictionary<string, ElementalStat> elementalStats;

    internal PlayerStat GetPlayerStat { get { return playerStat; } }
    internal HeroStat GetHeroStat { get { return heroStat; } }
    internal Dictionary<string, ElementalStat> GetElementalStats { get { return elementalStats; } }
    internal ElementalStat GetElementalStat(string id) { return elementalStats[id]; }

    public void Init()
    {
        if(baseStat == null)
        {
            baseStat = new BaseStat();
            playerStat = new PlayerStat();
            heroStat = new HeroStat();
            elementalStats = new Dictionary<string, ElementalStat>();
            foreach (string elementalid in Singleton<TableLoader>.getInstance().elementaltable.Keys)
                elementalStats.Add(elementalid, new ElementalStat());
        }
    }
    void Add(BaseStat equipstat)
    {
        baseStat.Int = equipstat.Int;
        baseStat.Str = equipstat.Str;
        baseStat.Spd = equipstat.Spd;
        baseStat.Luck = equipstat.Luck;
    }
    void Add(BonusStat bonus)
    {
        switch (bonus.targetID)
        {
            case "Hero":
                heroStat.Add(bonus);
                break;
            case "Player":
                playerStat.Add(bonus);
                break;
            default:
                if (elementalStats[bonus.targetID] == null)
                {
                    sexybacklog.Error("no Attribute");
                    return;
                }
                elementalStats[bonus.targetID].Add(bonus);
                break;
        }
    }

    void Remove(BonusStat bonus)
    {
        switch (bonus.targetID)
        {
            case "Hero":
                heroStat.Remove(bonus);
                break;
            case "Player":
                playerStat.Remove(bonus);
                break;
            default:
                if (elementalStats[bonus.targetID] == null)
                {
                    sexybacklog.Error("no Attribute");
                    return;
                }
                elementalStats[bonus.targetID].Remove(bonus);
                break;
        }
    }


}

