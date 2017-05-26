using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

using SexyBackPlayScene;
using SexyBackMenuScene;

internal class TableLoader
{
    public HeroData herotable;
    public Dictionary<string, MonsterData> monstertable;
    public Dictionary<string, ElementalData> elementaltable;
    public Dictionary<string, MapData> mapTable;
    public Dictionary<string, LevelUpData> leveluptable;
    public Dictionary<string, ResearchData> researchtable;
    public Dictionary<string, ConsumableData> consumable;
    public List<PriceData> pricetable;
    //public List<ConsumableData> talenttable;

    public Dictionary<string, EquipmentData> equipmenttable;
    public Dictionary<string, EquipmentSkillData> equipskilltable;

    bool FinishLoad = false;

    ~TableLoader()
    {
        sexybacklog.Console("TableLoader 소멸");
    }

    internal bool Init()
    {
        if (FinishLoad == false)
        {
            sexybacklog.Console("게임 데이터 테이블을 로드합니다.");
            try
            {
                LoadPriceData();
                LoadHeroData();
                LoadMonsterData();
                LoadElementData();
                LoadLevelUpData();
                LoadResearchData();
                LoadMapData();
                //LoadTalentData();
                LoadConsumableData();
                LoadEquipmentData();
                LoadEquipmentSkillData();
            }
            catch ( Exception e)
            {
                sexybacklog.Error("게임 데이터 테이블 로드에 문제가 있었습니다. " + e.Message);
                return false;
            }
        }
        FinishLoad = true;
        return true;
    }

    public XmlDocument OpenXml(string path)
    {
        TextAsset textasset = Resources.Load(path) as TextAsset;
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(textasset.text);
        return xmldoc;
    }


    private void LoadMapData()
    {
        mapTable = new Dictionary<string, MapData>();

        MapData data1 = new MapData();
        data1.ID = "Map01";
        data1.Name = "10층돌파";
        data1.RequireClearMap = null;
        data1.LimitTime = 3600;
        data1.RewardData = new MapRewardData(67, 67, 1, 1);
        data1.MaxFloor = 10;
        data1.LevelPerFloor = 5;
        data1.MonsterPerStage = 1;
        data1.ChestPerMonster = 1;
        data1.BossHPX = 10;
        data1.ChestPerBossMonster = 3;
        data1.BaseMonsterHP = 5;    // 1000;
        mapTable.Add(data1.ID, data1);

        MapData data2 = new MapData();
        data2.ID = "Map02";
        data2.Name = "20층돌파";
        data2.RequireClearMap = "Map01";
        data2.LimitTime = 7200;
        data2.RewardData = new MapRewardData(167, 67, 3, 1);
        data2.MaxFloor = 20;
        data2.LevelPerFloor = 5;
        data2.MonsterPerStage = 1;
        data2.ChestPerMonster = 1;
        data2.BossHPX = 10;
        data2.ChestPerBossMonster = 3;
        data2.BaseMonsterHP = 5;
        mapTable.Add(data2.ID, data2);
    }
    private void LoadEquipmentData()
    {
        // loadEquip table
        equipmenttable = new Dictionary<string, EquipmentData>();
        XmlDocument xmldoc = OpenXml("Xml/EquipmentData");
        XmlNodeList nodes = xmldoc.SelectSingleNode("Equipments").SelectNodes("Equipment");
        foreach (XmlNode node in nodes)
        {
            if (node.Attributes["name"] == null)
                continue;

            EquipmentData newOne = new EquipmentData();
            newOne.ID = node.Attributes["id"].Value;
            newOne.iconID = node.Attributes["icon"].Value;
            newOne.baseName = node.Attributes["name"].Value;
            newOne.droplevel = int.Parse(node.Attributes["droplevel"].Value);
            if (node.Attributes["dropmap"] != null)
                newOne.dropMapID = node.Attributes["dropmap"].Value;
            if (node.Attributes["skill"] != null)
                newOne.skillID= node.Attributes["skill"].Value;
            newOne.grade = int.Parse(node.Attributes["grade"].Value);
            newOne.type = (Equipment.Type)Enum.Parse(typeof(Equipment.Type), node.Attributes["type"].Value);
            newOne.baseStat.Str = int.Parse(node.Attributes["str"].Value);
            newOne.baseStat.Int = int.Parse(node.Attributes["int"].Value);
            newOne.baseStat.Spd = int.Parse(node.Attributes["spd"].Value);
            newOne.baseStat.Luck = int.Parse(node.Attributes["luck"].Value);

            equipmenttable.Add(newOne.ID, newOne);
        }
    }
    public void LoadEquipmentSkillData()
    {
        equipskilltable = new Dictionary<string, EquipmentSkillData>();
        XmlDocument xmldoc = OpenXml("Xml/EquipmentSkillData");
        XmlNodeList nodes = xmldoc.SelectSingleNode("EquipmentSkills").SelectNodes("Skill");

        foreach (XmlNode node in nodes)
        {
            if (node.Attributes["name"] == null)
                continue;
            EquipmentSkillData newOne = new EquipmentSkillData();
            newOne.ID = node.Attributes["id"].Value;
            newOne.baseSkillName = node.Attributes["name"].Value;
            newOne.belong= bool.Parse(node.Attributes["belong"].Value);
            newOne.dropLevel = int.Parse(node.Attributes["droplevel"].Value);
            XmlNodeList statNodes = node.SelectNodes("Bonus");
            foreach(XmlNode statNode in statNodes)
            {
                BonusStat temp = new BonusStat();
                temp.targetID = statNode.Attributes["target"].Value;
                temp.attribute = (Attribute)Enum.Parse(typeof(Attribute), statNode.Attributes["attribute"].Value);
                temp.value = int.Parse(statNode.Attributes["value"].Value);
                newOne.baseSkillStat.Add(temp);
            }
            equipskilltable.Add(newOne.ID, newOne);
        }
    }

    private void LoadMonsterData()
    {
        monstertable = new Dictionary<string, MonsterData>();
        monstertable.Add("m01", new MonsterData("m01", "슬라임", 0, 0f));
        monstertable.Add("m02", new MonsterData("m02", "불멍멍이", 0, 0f));
        monstertable.Add("m03", new MonsterData("m03", "맹금류", 0, 0f));
        monstertable.Add("m04", new MonsterData("m04", "나라쿠", 0, 0f));
        monstertable.Add("m05", new MonsterData("m05", "흔한골렘", 0, 0f));
        monstertable.Add("m06", new MonsterData("m06", "미녀마녀", 0, 0f));
        monstertable.Add("m07", new MonsterData("m07", "산적1", 0, 0f));
        monstertable.Add("m08", new MonsterData("m08", "괴인1", 0, 0f));
        monstertable.Add("m09", new MonsterData("m09", "괴인2", 0, 0f));
        monstertable.Add("m10", new MonsterData("m10", "21세기산적", 0, 0f));
    }

    private void LoadHeroData()
    {
        herotable = new HeroData();
    }

    private void LoadLevelUpData()
    {
        leveluptable = new Dictionary<string, LevelUpData>();
        LevelUpData heroAttack = new LevelUpData("L001", "hero", "일반공격", "Icon_11");
        LevelUpData item1 = new LevelUpData("L002", "fireball", "파이어볼", "Icon_01");
        LevelUpData item2 = new LevelUpData("L003", "iceblock", "각얼음", "Icon_05");
        LevelUpData item3 = new LevelUpData("L004", "rock", "짱돌", "Icon_07");
        LevelUpData item4 = new LevelUpData("L005", "electricball", "지지직", "Icon_04");
        LevelUpData item5 = new LevelUpData("L006", "waterball", "물폭탄", "Icon_09");
        LevelUpData item6 = new LevelUpData("L007", "earthball", "똥", "Icon_03");
        LevelUpData item7 = new LevelUpData("L008", "airball", "바람바람", "Icon_02");
        LevelUpData item8 = new LevelUpData("L009", "snowball", "눈덩이", "Icon_08");
        LevelUpData item9 = new LevelUpData("L010", "magmaball", "메테오", "Icon_06");
        leveluptable.Add(heroAttack.OwnerID, heroAttack);
        leveluptable.Add(item1.OwnerID, item1);
        leveluptable.Add(item2.OwnerID, item2);
        leveluptable.Add(item3.OwnerID, item3);
        leveluptable.Add(item4.OwnerID, item4);
        leveluptable.Add(item5.OwnerID, item5);
        leveluptable.Add(item6.OwnerID, item6);
        leveluptable.Add(item7.OwnerID, item7);
        leveluptable.Add(item8.OwnerID, item8);
        leveluptable.Add(item9.OwnerID, item9);
    }

    private void LoadPriceData()
    {
        pricetable = new List<PriceData>();

        XmlDocument xmldoc = OpenXml("Xml/PriceData");
        XmlNode rootNode = xmldoc.SelectSingleNode("Prices");
        XmlNodeList nodes = rootNode.SelectNodes("Price");

        foreach (XmlNode node in nodes)
        {
            PriceData temp = new PriceData();
            temp.minLevel = int.Parse(node.Attributes["fromlevel"].Value);
            temp.maxLevel = int.Parse(node.Attributes["tolevel"].Value);
            temp.basePriceDensity = double.Parse(node.Attributes["base"].Value);
            pricetable.Add(temp);
        }
    }

    private void LoadElementData()
    {
        elementaltable = new Dictionary<string, ElementalData>();
        XmlDocument xmldoc = OpenXml("Xml/ElementalData");
        XmlNode rootNode = xmldoc.SelectSingleNode("Elementals");
        XmlNodeList nodes = rootNode.SelectNodes("Elemental");

        foreach (XmlNode node in nodes)
        {
            ElementalData elemental = new ElementalData();
            elemental.ID = node.Attributes["id"].Value;
            elemental.Name = node.Attributes["name"].Value;
            elemental.SkillName = node.Attributes["skillname"].Value;
            elemental.BaseCastIntervalXK = int.Parse(node.Attributes["basecastintervalxk"].Value);
            elemental.BaseDmg = double.Parse(node.Attributes["basedensity"].Value);
            elemental.BasePrice = int.Parse(node.Attributes["baseprice"].Value);
            elemental.BaseLevel = int.Parse(node.Attributes["baselevel"].Value);
            elemental.PrefabName = node.Attributes["prefab"].Value;
            elemental.SkillPrefabName = node.Attributes["skillprefab"].Value;
            elemental.BaseSkillRateXK = int.Parse(node.Attributes["baseskillratexk"].Value);
            elemental.BaseSkillDamageXH = int.Parse(node.Attributes["baseskilldamagexh"].Value); ;
            elementaltable.Add(elemental.ID, elemental);
        }
    }

    private void LoadResearchData()
    {
        researchtable = new Dictionary<string, ResearchData>();
        XmlDocument xmldoc = OpenXml("Xml/ResearchData");
        XmlNode rootNode = xmldoc.SelectSingleNode("Researches");
        XmlNodeList nodes = rootNode.SelectNodes("Research");

        foreach (XmlNode node in nodes)
        {
            string id = node.Attributes["id"].Value;
            string requireid = node.Attributes["requireid"].Value;
            int requirelevel = int.Parse(node.Attributes["requirelevel"].Value);

            XmlNode infonode = node.SelectSingleNode("Info");
            string icon = infonode.Attributes["icon"].Value;
            string icontext = null;
            string subicon = null;
            if (infonode.Attributes["icontext"] != null)
                icontext = infonode.Attributes["icontext"].Value;
            if (infonode.Attributes["subicon"] != null)
                subicon = infonode.Attributes["subicon"].Value;
            string name = infonode.Attributes["name"].Value;
            string description = infonode.Attributes["description"].Value;

            XmlNode pricenode = node.SelectSingleNode("Price");
            int level = int.Parse(pricenode.Attributes["level"].Value);
            int baselevel = int.Parse(pricenode.Attributes["baselevel"].Value);
            int baseprice = int.Parse(pricenode.Attributes["baseprice"].Value);

            XmlNode potnode = node.SelectSingleNode("PriceOverTime");
            int rate = int.Parse(potnode.Attributes["rate"].Value);
            int basetime = int.Parse(potnode.Attributes["basetime"].Value);

            XmlNode bonusnode = node.SelectSingleNode("Bonus");
            string target = bonusnode.Attributes["target"].Value;
            Attribute attribute = (Attribute)Enum.Parse(typeof(Attribute),bonusnode.Attributes["attribute"].Value);
            int value = 0;
            string stringvalue = null;
            if (bonusnode.Attributes["value"] != null)
                value = int.Parse(bonusnode.Attributes["value"].Value);
            else
                stringvalue = bonusnode.Attributes["stringvalue"].Value;

            BonusStat bonus = new BonusStat(target, attribute, value, stringvalue, null);
            NestedIcon iconinfo = new NestedIcon(icon, icontext, subicon);
            ResearchData research = new ResearchData(id, requireid, requirelevel, iconinfo, name, description, level, baselevel, baseprice,
                rate, basetime, bonus);
            researchtable.Add(id, research);
        }

    }

    private void LoadConsumableData()
    {
        consumable = new Dictionary<string, ConsumableData>();
        ConsumableData cData1 = new ConsumableData(1);
        consumable.Add(cData1.id, cData1);
        ConsumableData cData2 = new ConsumableData(2);
        consumable.Add(cData2.id, cData2);
        ConsumableData cData3 = new ConsumableData(3);
        consumable.Add(cData3.id, cData3);
        ConsumableData cData4 = new ConsumableData(4);
        consumable.Add(cData4.id, cData4);
        ConsumableData cData5 = new ConsumableData(5);
        consumable.Add(cData5.id, cData5);
        ConsumableData cData6 = new ConsumableData(6);
        consumable.Add(cData6.id, cData6);
        ConsumableData cData7 = new ConsumableData(7);
        consumable.Add(cData7.id, cData7);
        ConsumableData cData8 = new ConsumableData(8);
        consumable.Add(cData8.id, cData8);
        ConsumableData cData9 = new ConsumableData(9);
        consumable.Add(cData9.id, cData9);
        ConsumableData cData10 = new ConsumableData(10);
        consumable.Add(cData10.id, cData10);

        //consumable.Add(new ConsumableData(8));

        //XmlDocument xmldoc = OpenXml("Xml/TalentData");
        //XmlNode rootNode = xmldoc.SelectSingleNode("Talents");
        //XmlNodeList nodes = rootNode.SelectNodes("Talent");

        //foreach (XmlNode node in nodes)
        //{
        //    string id = node.Attributes["id"].Value;
        //    string requireid = node.Attributes["requireid"].Value;
        //    ConsumableType type = (ConsumableType)Enum.Parse(typeof(ConsumableType), node.Attributes["type"].Value);
        //    int maxlevelper10;
        //    if (node.Attributes["maxlevelper10"] != null)
        //        maxlevelper10 = int.Parse(node.Attributes["maxlevelper10"].Value);

        //    XmlNode infonode = node.SelectSingleNode("Info");
        //    string icon = infonode.Attributes["icon"].Value;
        //    string subicon = null;
        //    if (infonode.Attributes["subicon"] != null)
        //        subicon = infonode.Attributes["subicon"].Value;
        //    string name = infonode.Attributes["name"].Value;
        //    string description = infonode.Attributes["description"].Value;

        //    XmlNode ratenode = node.SelectSingleNode("Rate");
        //    int rate;
        //    bool abs;
        //    if (ratenode.Attributes["absrate"] != null)
        //    {
        //        abs = true;
        //        rate = int.Parse(ratenode.Attributes["absrate"].Value);
        //    }
        //    else
        //    {
        //        abs = false;
        //        rate = int.Parse(ratenode.Attributes["rate"].Value);
        //    }

        //    XmlNode bonusnode = node.SelectSingleNode("Bonus");
        //    string target = bonusnode.Attributes["target"].Value;
        //    Attribute attribute = (Attribute)Enum.Parse(typeof(Attribute),bonusnode.Attributes["attribute"].Value);
        //    int value = 0;
        //    if (bonusnode.Attributes["value"] != null)
        //        value = int.Parse(bonusnode.Attributes["value"].Value);
        //    BonusStat bonus = new BonusStat(target, attribute, value, null, null);

        //    GridItemIcon iconinfo = new GridItemIcon(icon, "",  subicon);
        //    ConsumableData talentdata = new ConsumableData(id, iconinfo, description, bonus, type, rate, abs);
        //    talenttable.Add(talentdata);
    }

}