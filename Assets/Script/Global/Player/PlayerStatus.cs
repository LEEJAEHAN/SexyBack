using System;
using SexyBackRewardScene;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Xml;
[Serializable]
internal class PlayerStatus
{
    public string Name;

    // 글로벌 스텟. 게임 시작 전과 끝난 후에 영향을미친다, 그때그때 참조하므로 리스너가 따로없다.
    GlobalStat globalStat;
    
    // 글로벌이면서, 인스턴스 게임 진행도중에도 영향을 미치는 스텟들. ( 변경 후 playscene에 notice가 필요하다. )
    BaseStat baseStat;
    UtilStat utilStat;      // 인스턴스 업그레이드의 자원획득, 가격, 시간 관련
    HeroStat heroStat;      // 인스턴스 히어로에 관한 모든것.
    Dictionary<string, ElementalStat> elementalStats; // 인스턴스 element에 관한 모든 것.

    internal GlobalStat GetGlobalStat { get { return globalStat; } }
    internal BaseStat GetBaseStat { get { return baseStat; } }
    internal UtilStat GetUtilStat { get { return utilStat; } }
    internal HeroStat GetHeroStat { get { return heroStat; } }
    internal Dictionary<string, ElementalStat> GetElementalStats { get { return elementalStats; } }

    internal ElementalStat GetElementalStat(string id) { return elementalStats[id]; }

    //[field: NonSerialized]
    public event Action<BaseStat> Action_BaseStatChange = delegate { };
    [field: NonSerialized]
    public event Action<UtilStat> Action_UtilStatChange = delegate { };
    [field: NonSerialized]
    public event Action<HeroStat> Action_HeroStatChange = delegate { };
    [field: NonSerialized]
    public event Action<ElementalStat, string> Action_ElementalStatChange = delegate { };


    static int LevelUnit = 100;
    static int GrowthPerLevelUnit = 8;

    public static double CalGlobalGrowth(double level)
    {
        return Math.Pow(GrowthPerLevelUnit, level / LevelUnit);
        //return Math.Pow(GrowthPerLevelUnit, Math.Min(((double)level / LevelUnit), 10f));
    }

    internal void ReCheckStat()
    {
        sexybacklog.Console("장비와 특성으로부터 스텟을 새로 계산합니다.");
        Singleton<EquipmentManager>.getInstance().ReCheckStat();
        Singleton<TalentManager>.getInstance().ReCheckStat();
    }

    public void Init()
    {
        sexybacklog.Console("스텟 초기화");
        globalStat = new GlobalStat();
        baseStat = new BaseStat();
        utilStat = new UtilStat();
        heroStat = new HeroStat();
        elementalStats = new Dictionary<string, ElementalStat>();
        foreach (string elementalid in Singleton<TableLoader>.getInstance().elementaltable.Keys)
            elementalStats.Add(elementalid, new ElementalStat());
    }


    internal void ApplySpecialStat(BonusStat bonus, bool signPositive)
    {
        if (bonus == null)
            return;

        switch (bonus.targetID)
        {
            case "global":
                globalStat.ApplyBonus(bonus, signPositive);
                // no event handle
                break;
            case "base":
                baseStat.ApplyBonus(bonus, signPositive);
                Action_BaseStatChange(baseStat);
                break;
            case "hero":
                heroStat.ApplyBonus(bonus, signPositive);
                Action_HeroStatChange(heroStat);
                break;
            case "util":
                utilStat.ApplyBonus(bonus, signPositive);
                Action_UtilStatChange(utilStat);
                break;
            case "elementals":
                foreach( ElementalStat e in elementalStats.Values)
                {
                    e.ApplyBonus(bonus, signPositive);
                    Action_ElementalStatChange(e, bonus.targetID);
                }
                break;
            default:
                ElementalStat elementalStat = elementalStats[bonus.targetID];
                elementalStat.ApplyBonus(bonus, signPositive);
                Action_ElementalStatChange(elementalStat, bonus.targetID);
                break;
        }
    }

    public void ApplyBaseStat(BaseStat stat, bool signPositive)
    {
        if (signPositive)
        {
            baseStat.Int += stat.Int;
            baseStat.Str += stat.Str;
            baseStat.Spd += stat.Spd;
            baseStat.Luck += stat.Luck;
        }
        else
        {
            baseStat.Int -= stat.Int;
            baseStat.Str -= stat.Str;
            baseStat.Spd -= stat.Spd;
            baseStat.Luck -= stat.Luck;
        }

        sexybacklog.Console(baseStat.ToString());
        Action_BaseStatChange(baseStat);
    }


}

