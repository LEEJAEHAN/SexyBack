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
    public List<TalentData> talenttable;

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
                LoadTalentData();
                LoadConsumableData();
                LoadEquipmentData();
            }
            catch ( Exception e)
            {
                sexybacklog.Error("게임 데이터 테이블 로드에 문제가 있었습니다. " + e.Message);
                return false;
            }
            return true;
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


    private void LoadMapData()
    {
        mapTable = new Dictionary<string, MapData>();

        MapData data1 = new MapData("Map01", "10층돌파", 10, 3600, null, new MapRewardData(67, 67, 1, 1));
        MapData data2 = new MapData("Map02", "20층돌파", 20, 7200, "Map01", new MapRewardData(167, 67, 3, 1));

        mapTable.Add(data1.ID, data1);
        mapTable.Add(data2.ID, data2);
    }
    private void LoadEquipmentData()
    {
        // loadEquip table
        equipmenttable = new Dictionary<string, EquipmentData>();

        EquipmentData e01 = new EquipmentData("E01", "Icon_11", "롱소드", Equipment.Type.Weapon, 67, 0, new BaseStat(30, 0, 0, 0));
        EquipmentData e02 = new EquipmentData("E02", "Icon_10", "두번째아이템", Equipment.Type.Ring, 67, 1, new BaseStat(0, 10, 10, 0));
        EquipmentData e03 = new EquipmentData("E03", "Icon_09", "세번째아이템", Equipment.Type.Staff, 67, 2, new BaseStat(0, 0, 0, 10));

        equipmenttable.Add(e01.ID, e01);
        equipmenttable.Add(e02.ID, e02);
        equipmenttable.Add(e03.ID, e03);

        // load EquipSkill table;
        equipskilltable = new Dictionary<string, EquipmentSkillData>();

        List<BonusStat> skillstats = new List<BonusStat>();
        skillstats.Add(new BonusStat("hero", "DpcX", 10, null, "히어로 데미지 증가 $d배"));
        skillstats.Add(new BonusStat("fireball", "DpsX", 10, null, "화염구의 데미지 증가 $d배"));

        EquipmentSkillData se01 = new EquipmentSkillData("SE01", "무기스킬", 67, Equipment.Type.Weapon, skillstats);
        EquipmentSkillData se02 = new EquipmentSkillData("SE02", "링스킬", 67, Equipment.Type.Ring, skillstats);
        EquipmentSkillData se03 = new EquipmentSkillData("SE03", "스탭스킬", 67, Equipment.Type.Staff, skillstats);
        equipskilltable.Add(se01.ID, se01);
        equipskilltable.Add(se02.ID, se02);
        equipskilltable.Add(se03.ID, se03);
    }

    private void LoadConsumableData()
    {
        consumable = new Dictionary<string, ConsumableData>();

        //TODO : 채워넣어야함
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
            string attribute = bonusnode.Attributes["attribute"].Value;
            int value = 0;
            string stringvalue = null;
            if (bonusnode.Attributes["value"] != null)
                value = int.Parse(bonusnode.Attributes["value"].Value);
            else
                stringvalue = bonusnode.Attributes["stringvalue"].Value;

            BonusStat bonus = new BonusStat(target, attribute, value, stringvalue, null);
            GridItemIcon iconinfo = new GridItemIcon(icon, icontext, subicon);
            ResearchData research = new ResearchData(id, requireid, requirelevel, iconinfo, name, description, level, baselevel, baseprice,
                rate, basetime, bonus);
            researchtable.Add(id, research);
        }

    }

    private void LoadTalentData()
    {
        talenttable = new List<TalentData>();

        XmlDocument xmldoc = OpenXml("Xml/TalentData");
        XmlNode rootNode = xmldoc.SelectSingleNode("Talents");
        XmlNodeList nodes = rootNode.SelectNodes("Talent");

        foreach (XmlNode node in nodes)
        {
            string id = node.Attributes["id"].Value;
            string requireid = node.Attributes["requireid"].Value;
            TalentType type = (TalentType)Enum.Parse(typeof(TalentType), node.Attributes["type"].Value);
            int maxlevelper10;
            if (node.Attributes["maxlevelper10"] != null)
                maxlevelper10 = int.Parse(node.Attributes["maxlevelper10"].Value);

            XmlNode infonode = node.SelectSingleNode("Info");
            string icon = infonode.Attributes["icon"].Value;
            string subicon = null;
            if (infonode.Attributes["subicon"] != null)
                subicon = infonode.Attributes["subicon"].Value;
            string name = infonode.Attributes["name"].Value;
            string description = infonode.Attributes["description"].Value;

            XmlNode ratenode = node.SelectSingleNode("Rate");
            int rate;
            bool abs;
            if (ratenode.Attributes["absrate"] != null)
            {
                abs = true;
                rate = int.Parse(ratenode.Attributes["absrate"].Value);
            }
            else
            {
                abs = false;
                rate = int.Parse(ratenode.Attributes["rate"].Value);
            }

            XmlNode bonusnode = node.SelectSingleNode("Bonus");
            string target = bonusnode.Attributes["target"].Value;
            string attribute = bonusnode.Attributes["attribute"].Value;
            int value = 0;
            if (bonusnode.Attributes["value"] != null)
                value = int.Parse(bonusnode.Attributes["value"].Value);
            BonusStat bonus = new BonusStat(target, attribute, value, null, null);

            GridItemIcon iconinfo = new GridItemIcon(icon, "",  subicon);
            TalentData talentdata = new TalentData(id, iconinfo, description, bonus, type, rate, abs);
            talenttable.Add(talentdata);
        }

    }

}