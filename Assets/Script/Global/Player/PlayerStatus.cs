using System;
using SexyBackRewardScene;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Xml;
[Serializable]
internal class PlayerStatus
{
    // 언제 다른 매니져로옮겨갈지 모른다.
    public int Jewel = 0;
    public int Reputation = 0;
    public List<string> ClearedMapID;
    public bool PremiumUser = false;

    // 글로벌 스텟. 게임 시작 전과 끝난 후에 영향을미친다, 그때그때 참조하므로 리스너가 따로없다.
    GlobalStat globalStat;


    // 글로벌이면서, 인스턴스 게임 진행도중에도 영향을 미치는 스텟들. ( 변경 후 playscene에 notice가 필요하다. )
    BaseStat baseStat;      //
    UtilStat utilStat;      // 인스턴스 업그레이드의 자원획득, 가격, 시간 관련
    HeroStat heroStat;      // 인스턴스 히어로에 관한 모든것.
    Dictionary<string, ElementalStat> elementalStats; // 인스턴스 element에 관한 모든 것.

    internal GlobalStat GetGlobalStat { get { return globalStat; } }
    internal BaseStat GetBaseStat { get { return baseStat; } }
    internal UtilStat GetUtilStat { get { return utilStat; } }
    internal HeroStat GetHeroStat { get { return heroStat; } }
    internal Dictionary<string, ElementalStat> GetElementalStats { get { return elementalStats; } }


    internal ElementalStat GetElementalStat(string id) { return elementalStats[id]; }

    [field: NonSerialized]
    public event Action<BaseStat> Action_BaseStatChange = delegate { };
    [field: NonSerialized]
    public event Action<UtilStat> Action_UtilStatChange = delegate { };
    [field: NonSerialized]
    public event Action<HeroStat> Action_HeroStatChange = delegate { };
    [field: NonSerialized]
    public event Action<ElementalStat, string> Action_ElementalStatChange = delegate { };

    internal void Init()
    {
        sexybacklog.Console("PlayerStatus 로드 및 초기화");
        if (baseStat != null)
            return;

        if (SaveSystem.GlobalDataExist)
        {
            sexybacklog.Console("PlayerStatus 파일로부터 로드.");
            //NewData(); for test
            Load();
        }
        else
        {
            sexybacklog.Console("PlayerStatus 시작초기 데이터로 생성.");
            NewData();
        }
    }

    internal bool CheckFirstClear(string mapID)
    {
        if (ClearedMapID.Contains(mapID))
            return false;
        else
        {
            ClearedMapID.Add(mapID);
            return true;
        }
    }

    private void Load()
    {
        XmlDocument doc = SaveSystem.LoadXml(SaveSystem.SaveDataPath);
        globalStat = new GlobalStat(doc.SelectSingleNode("PlayerStatus/Stats/GlobalStat"));
        baseStat = new BaseStat(doc.SelectSingleNode("PlayerStatus/Stats/BaseStat"));
        utilStat = new UtilStat(doc.SelectSingleNode("PlayerStatus/Stats/UtilStat"));
        heroStat = new HeroStat(doc.SelectSingleNode("PlayerStatus/Stats/HeroStat"));

        elementalStats = new Dictionary<string, ElementalStat>();
        foreach (string elementalid in Singleton<TableLoader>.getInstance().elementaltable.Keys)
            elementalStats.Add(elementalid, new ElementalStat());
        foreach (XmlNode node in doc.SelectSingleNode("PlayerStatus/Stats/ElementalStats").ChildNodes)
            elementalStats[node.Attributes["id"].Value].LoadStat(node);
        ClearedMapID = new List<string>();
        foreach (XmlNode node in doc.SelectSingleNode("PlayerStatus/Progress").ChildNodes)
            ClearedMapID.Add(node.Attributes["id"].Value);
    }

    public void NewData()
    {
        sexybacklog.Console("플레이씬에서 새 게임을 시작합니다. 더미스텟으로 시작합니다.");
        globalStat = new GlobalStat();
        baseStat = new BaseStat();
        utilStat = new UtilStat();
        heroStat = new HeroStat();
        elementalStats = new Dictionary<string, ElementalStat>();
        foreach (string elementalid in Singleton<TableLoader>.getInstance().elementaltable.Keys)
            elementalStats.Add(elementalid, new ElementalStat());
        ClearedMapID = new List<string>();
    }

    internal void ReCheckStat()
    {
        NewData();
        Singleton<EquipmentManager>.getInstance().ReCheckStat();
        sexybacklog.Console("장비와 특성으로부터 스텟을 새로 계산합니다.");
    }

    internal void ApplySpecialStat(BonusStat bonus, bool signPositive)
    {
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

