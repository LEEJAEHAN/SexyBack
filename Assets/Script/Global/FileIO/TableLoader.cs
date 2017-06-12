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
    public List<MapData> mapTable;
    public Dictionary<string, LevelUpData> leveluptable;
    public Dictionary<string, ResearchData> researchtable;
    public Dictionary<string, ConsumableData> consumable;
    public Dictionary<string, TalentData> talenttable;

    public List<PriceData> pricetable;
    public Dictionary<int, double> powertable;
    //public List<ConsumableData> talenttable;

    public Dictionary<string, EquipmentData> equipmenttable;
    public Dictionary<string, EquipmentSkillData> equipskilltable;

    public bool FinishLoad = false;

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
                LoadTalentData();
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
        mapTable = new List<MapData>();
        XmlDocument xmldoc = OpenXml("Xml/MapData");
        XmlNode rootNode = xmldoc.SelectSingleNode("Maps");
        XmlNodeList nodes = rootNode.SelectNodes("Map");

        foreach (XmlNode node in nodes)
        {
            MapData newOne = new MapData();
            newOne.ID = node.Attributes["id"].Value;
            newOne.RequireMap = node.Attributes["requireclearmap"] == null ? null : node.Attributes["requireclearmap"].Value;
            newOne.Name = node.Attributes["name"].Value;
            newOne.Difficulty = int.Parse(node.Attributes["difficulty"].Value);
            newOne.LimitTime = int.Parse(node.Attributes["limittime"].Value);
            newOne.MaxFloor = int.Parse(node.Attributes["maxfloor"].Value);

            {
                XmlNode rNode = node.SelectSingleNode("MapReward");
                newOne.MapReward.Level = int.Parse(rNode.Attributes["rewardlevel"].Value);
                newOne.MapReward.PrevLevel = int.Parse(rNode.Attributes["prevrewardlevel"].Value);
                newOne.MapReward.ItemCount = int.Parse(rNode.Attributes["itemcount"].Value);
            }
            {
                XmlNode mNode = node.SelectSingleNode("MapMonster");
                newOne.MapMonster.LevelPerFloor = int.Parse(mNode.Attributes["levelperfloor"].Value);
                newOne.MapMonster.MonsterPerStage = int.Parse(mNode.Attributes["monsterperstage"].Value);
                newOne.MapMonster.MonsterHP = int.Parse(mNode.Attributes["monsterhp"].Value);
                newOne.MapMonster.BossHp = int.Parse(mNode.Attributes["bosshp"].Value);
                newOne.MapMonster.ChestPerMonster = int.Parse(mNode.Attributes["chestpermonster"].Value);
                newOne.MapMonster.ChestPerBoss = int.Parse(mNode.Attributes["chestperboss"].Value);
                newOne.MapMonster.BossTerm= int.Parse(mNode.Attributes["bossterm"].Value);

            }
            mapTable.Add(newOne);
        }



        //MapData data1 = new MapData();
        //data1.ID = "Map01";
        //data1.Name = "50층돌파";
        //data1.RequireMap = null;
        //data1.LimitTime = 3600;
        //data1.RewardData = new MapRewardData(50, 50, 1, 8);
        //data1.MaxFloor = 50;        // 10
        //data1.LevelPerFloor = 1;    // 5
        //data1.MonsterPerStage = 1;
        //data1.ChestPerMonster = 1;
        //data1.BossHp = 2000;
        //data1.ChestPerBoss = 1;
        //data1.MonsterHP = 200;    // 1000;
        //mapTable.Add(data1);

        //MapData data2 = new MapData();
        //data2.ID = "Map02";
        //data2.Name = "100층돌파";
        //data2.RequireMap = "Map01";
        //data2.LimitTime = 7200;
        //data2.RewardData = new MapRewardData(100, 100, 2, 8);
        //data2.MaxFloor = 20;
        //data2.LevelPerFloor = 5;
        //data2.MonsterPerStage = 1;
        //data2.ChestPerMonster = 1;
        //data2.BossHp = 10;
        //data2.ChestPerBoss = 3;
        //data2.MonsterHP = 5;
        //mapTable.Add(data2);

        //MapData data3= new MapData();
        //data3.ID = "Map03";
        //data3.Name = "테스트맵";
        //data3.RequireClearMap = "Map01";
        //data3.LimitTime = 7200;
        //data3.RewardData = new MapRewardData(167, 67, 3, 3);
        //data3.MaxFloor = 1;
        //data3.LevelPerFloor = 5;
        //data3.MonsterPerStage = 1;
        //data3.ChestPerMonster = 1;
        //data3.BossMonsterHP = 10;
        //data3.ChestPerBossMonster = 3;
        //data3.MonsterHP = 5;
        //mapTable.Add(data3);
    }


    private void LoadTalentData()
    {
        talenttable = new Dictionary<string, TalentData>();

        for (int i = 1; i <= 8; i++)
        {
            TalentData t1 = new TalentData(i);
            talenttable.Add(t1.ID, t1);
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
        LevelUpData heroAttack = new LevelUpData("hero", "검술", "Icon_11","L01");
        LevelUpData item1 = new LevelUpData("fireball", "불", "Icon_01", "L02");
        LevelUpData item2 = new LevelUpData("iceblock", "얼음", "Icon_05", "L03");
        LevelUpData item3 = new LevelUpData("rock", "바위", "Icon_07", "L04");
        LevelUpData item4 = new LevelUpData("electricball", "전기", "Icon_04", "L05");
        LevelUpData item5 = new LevelUpData("waterball", "물", "Icon_09", "L06");
        LevelUpData item6 = new LevelUpData("earthball", "대지", "Icon_03", "L07");
        LevelUpData item7 = new LevelUpData("airball", "바람", "Icon_02", "L08");
        LevelUpData item8 = new LevelUpData("snowball", "눈", "Icon_08", "L09");
        LevelUpData item9 = new LevelUpData("magmaball", "용암", "Icon_06", "L10");
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
            elemental.BaseDmgDensity = double.Parse(node.Attributes["basedmgdensity"].Value);
            elemental.BaseDamage= int.Parse(node.Attributes["basedamage"].Value);
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
            if (node.Attributes["id"] == null)
                continue;

            string id = node.Attributes["id"].Value;
            string requireid = node.Attributes["requireid"].Value;
            int showlevel = int.Parse(node.Attributes["showlevel"].Value);

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
            BonusStat bonus = LoadBonus(bonusnode);

            NestedIcon iconinfo = new NestedIcon(icon, icontext, subicon);
            ResearchData research = new ResearchData(id, requireid, showlevel, iconinfo, name, description, level, baselevel, baseprice,
                rate, basetime, bonus);
            researchtable.Add(id, research);
        }

    }

    private void LoadEquipmentData()
    {
        LoadPowerData();
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
            newOne.belong = bool.Parse(node.Attributes["belong"].Value);
            newOne.dropStart = int.Parse(node.Attributes["dropstart"].Value);
            newOne.dropEnd = int.Parse(node.Attributes["dropend"].Value);
            if (node.Attributes["skill"] != null)
                newOne.skillID = node.Attributes["skill"].Value;
            newOne.grade = int.Parse(node.Attributes["grade"].Value);
            newOne.type = (Equipment.Type)Enum.Parse(typeof(Equipment.Type), node.Attributes["type"].Value);

            XmlNode statnodes = node.SelectSingleNode("Stats");
            int count = statnodes.Attributes.Count;
            for(int i = 0; i < count; i++)
            {
                if (newOne.ID == "E10")
                {
                    sexybacklog.Console("지금!");
                }
                newOne.stats.Add(new BonusStat(statnodes.Attributes[i].Value, Attribute.BaseDensityAdd, 1f / count));
            }

            equipmenttable.Add(newOne.ID, newOne);
        }
    }

    private void LoadPowerData()
    {
        powertable = new Dictionary<int, double>();

        XmlDocument xmldoc = OpenXml("Xml/PowerData");
        XmlNode rootNode = xmldoc.SelectSingleNode("Powers");
        XmlNodeList nodes = rootNode.SelectNodes("Power");

        foreach (XmlNode node in nodes)
            powertable.Add(int.Parse(node.Attributes["level"].Value), double.Parse(node.Attributes["basedmg"].Value));
    }

    private void LoadEquipmentSkillData()
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
            newOne.belong = bool.Parse(node.Attributes["belong"].Value);
            newOne.dropStart = int.Parse(node.Attributes["dropstart"].Value);
            newOne.dropEnd = int.Parse(node.Attributes["dropend"].Value);
            XmlNodeList statNodes = node.SelectNodes("Bonus");

            foreach (XmlNode statNode in statNodes)
            {
                BonusStat temp = LoadBonus(statNode);
                newOne.baseSkillStat.Add(temp);
            }
            equipskilltable.Add(newOne.ID, newOne);
        }
    }

    private BonusStat LoadBonus(XmlNode statNode)
    {
        BonusStat bonus = new BonusStat();
        bonus.targetID = statNode.Attributes["target"].Value;
        bonus.attribute = (Attribute)Enum.Parse(typeof(Attribute), statNode.Attributes["attribute"].Value);
        if (statNode.Attributes["value"] != null)
            bonus.value = int.Parse(statNode.Attributes["value"].Value);
        else
            bonus.strvalue = statNode.Attributes["stringvalue"].Value;
        return bonus;
    }

    private void LoadConsumableData()
    {
        consumable = new Dictionary<string, ConsumableData>();
        XmlDocument xmldoc = OpenXml("Xml/ConsumableData");
        XmlNode rootNode = xmldoc.SelectSingleNode("Consumables");
        XmlNodeList nodes = rootNode.SelectNodes("Consumable");

        foreach (XmlNode node in nodes)
        {
            ConsumableData data = new ConsumableData();

            data.id = node.Attributes["id"].Value;
            data.name = node.Attributes["name"] != null ? node.Attributes["name"].Value : null;
            data.description = node.Attributes["description"] != null ? node.Attributes["description"].Value : null;
            data.order = node.Attributes["order"] != null ? int.Parse(node.Attributes["order"].Value) : 0;
            {
                XmlNode iconNode = node.SelectSingleNode("NestedIcon");
                data.icon = new NestedIcon();
                data.icon.IconName = iconNode.Attributes["icon"].Value;
                data.icon.SubIconName = iconNode.Attributes["subicon"] != null ? iconNode.Attributes["subicon"].Value : null;
                data.icon.IconText = iconNode.Attributes["icontext"] != null ? iconNode.Attributes["icontext"].Value : null;
            }
            {
                XmlNode usageNode = node.SelectSingleNode("Usage");
                data.type = (Consumable.Type)Enum.Parse(typeof(Consumable.Type), usageNode.Attributes["type"].Value);
                data.value = usageNode.Attributes["value"] != null ? int.Parse(usageNode.Attributes["value"].Value) : 0;
                data.strValue = usageNode.Attributes["strvalue"] != null ? usageNode.Attributes["strvalue"].Value : null;
                data.CoolTime = usageNode.Attributes["cooltime"] != null ? double.Parse(usageNode.Attributes["cooltime"].Value) : 0;
            }
            {
                XmlNode dropNode = node.SelectSingleNode("DropInfo");
                data.stackPerChest = dropNode.Attributes["stackperchest"] != null ? int.Parse(dropNode.Attributes["stackperchest"].Value) : 0;
                data.dropLevel = dropNode.Attributes["droplevel"] != null ? int.Parse(dropNode.Attributes["droplevel"].Value) : 0;
                data.AbsRate = dropNode.Attributes["absrate"] != null ? int.Parse(dropNode.Attributes["absrate"].Value) : 0;
                data.Density = dropNode.Attributes["density"] != null ? int.Parse(dropNode.Attributes["density"].Value) : 0;
            }
            consumable.Add(data.id, data);
        }
    }

}