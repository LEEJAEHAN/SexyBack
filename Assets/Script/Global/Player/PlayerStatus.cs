﻿using System;
using SexyBackRewardScene;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Xml;
[Serializable]
internal class PlayerStatus
{
    // 진행중인 맵
    public int Jewel = 0;
    public int SkillPoint = 0;

    //base stat
    //special stat ( from passive, equipment enchant, in game research.. etc )
    BaseStat baseStat;
    UtilStat utilStat;
    HeroStat heroStat;
    Dictionary<string, ElementalStat> elementalStats;

    internal BaseStat GetBaseStat {  get { return baseStat;  } }
    internal UtilStat GetUtilStat { get { return utilStat; } }
    internal HeroStat GetHeroStat { get { return heroStat; } }
    internal Dictionary<string, ElementalStat> GetElementalStats { get { return elementalStats; } }
    internal ElementalStat GetElementalStat(string id) { return elementalStats[id]; }

    [field: NonSerialized]
    public event Action<BaseStat> Action_BaseStatChange;
    [field: NonSerialized]
    public event Action<UtilStat, string> Action_UtilStatChange;
    [field: NonSerialized]
    public event Action<HeroStat, string> Action_HeroStatChange;
    [field: NonSerialized]
    public event Action<ElementalStat, string, string> Action_ElementalStatChange;

    internal void Init()
    {
        sexybacklog.Console("PlayerStatus 로드 및 초기화");
        if (baseStat != null)
            return;

        if (SaveSystem.GlobalDataExist)
        {
            sexybacklog.Console("PlayerStatus 파일로부터 로드.");
            XmlDocument doc = SaveSystem.LoadXml(SaveSystem.SaveDataPath);
            XmlNode rootNode = doc.SelectSingleNode("PlayerStatus");
            XmlNode EquipmentNodes = rootNode.SelectSingleNode("Equipments");

            //for test
            Load();
            //NewData();
        }
        else
        {
            sexybacklog.Console("PlayerStatus 시작초기 데이터로 생성.");
            NewData();
        }
    }

    private void Load()
    {
        XmlDocument doc = SaveSystem.LoadXml(SaveSystem.SaveDataPath);
        XmlNode BaseStatNode = doc.SelectSingleNode("PlayerStatus/Stats/BaseStat");
        baseStat = new BaseStat(doc.SelectSingleNode("PlayerStatus/Stats/BaseStat"));
        utilStat = new UtilStat(doc.SelectSingleNode("PlayerStatus/Stats/UtilStat"));
        heroStat = new HeroStat(doc.SelectSingleNode("PlayerStatus/Stats/HeroStat"));

        elementalStats = new Dictionary<string, ElementalStat>();
        foreach (string elementalid in Singleton<TableLoader>.getInstance().elementaltable.Keys)
            elementalStats.Add(elementalid, new ElementalStat());
        foreach ( XmlNode node in doc.SelectSingleNode("PlayerStatus/Stats/ElementalStats").ChildNodes)
            elementalStats[node.Attributes["id"].Value].LoadStat(node);
    }

    public void NewData()
    {
        sexybacklog.Console("플레이씬에서 새 게임을 시작합니다. 더미스텟으로 시작합니다.");
        baseStat = new BaseStat();
        utilStat = new UtilStat();
        heroStat = new HeroStat();
        elementalStats = new Dictionary<string, ElementalStat>();
        foreach (string elementalid in Singleton<TableLoader>.getInstance().elementaltable.Keys)
            elementalStats.Add(elementalid, new ElementalStat());
    }

    internal void ReCheckStat()
    {
        NewData();
        sexybacklog.Console("장비와 특성으로부터 스텟을 새로 계산합니다.");
    }

    void ApplyBaseStat(BaseStat stat, bool signPositive)
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

        Action_BaseStatChange(baseStat);
    }

    internal void ApplySpecialStat(BonusStat bonus, bool signPositive)
    {
        switch (bonus.targetID)
        {
            case "hero":
                ApplyHeroBonus(bonus, signPositive);
                break;
            case "player":
                ApplyPlayerBonus(bonus, signPositive);
                break;
            default:
                ApplyElementalBonus(bonus, signPositive);
                break;
        }
    }

    private void ApplyHeroBonus(BonusStat bonus, bool signPositive)
    {
        if (signPositive)
            heroStat.Add(bonus);
        else
            heroStat.Remove(bonus);

        Action_HeroStatChange(heroStat, bonus.attribute);
    }

    private void ApplyElementalBonus(BonusStat bonus, bool signPositive) // 각 element에게만 해당하는것. 전체는 player
    {
        string targetID = bonus.targetID;
        ElementalStat elementalStat = elementalStats[bonus.targetID];

        if (signPositive)
            elementalStat.Add(bonus);
        else
            elementalStat.Remove(bonus);

        Action_ElementalStatChange(elementalStat, bonus.targetID, bonus.attribute);
    }

    private void ApplyPlayerBonus(BonusStat bonus, bool signPositive)
    {
        if (signPositive)
            utilStat.Add(bonus);
        else
            utilStat.Remove(bonus);
        
        Action_UtilStatChange(utilStat, bonus.attribute);
    }

    //internal void Upgrade(BaseStat basestat)
    //{
    //    //              case "DpsIncreaseXH": // TODO : case all elemental 현재 안쓰고있음.
    //    //        {
    //    //    foreach (ElementalStat stat in IElementalStats.Values)
    //    //        stat.DpsIncreaseXH += bonus.value;
    //    //    elementalmanager.SetStatAll(IElementalStats, true, false);
    //    //    break;
    //    //}
    //    //    case "CastSpeedXH": // case all elemental
    //    //        {
    //    //    foreach (ElementalStat stat in IElementalStats.Values)
    //    //        stat.CastSpeedXH += bonus.value;
    //    //    elementalmanager.SetStatAll(IElementalStats, true, false);
    //    //    break;
    //    //}
    //}



}

