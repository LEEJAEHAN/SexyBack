﻿using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
namespace SexyBackPlayScene
{
    internal class TableLoader
    {
        public List<MonsterData> monstertablelist = new List<MonsterData>();

        public Dictionary<string, HeroData> herotable = new Dictionary<string, HeroData>();
        public Dictionary<string, MonsterData> monstertable = new Dictionary<string, MonsterData>();
        public Dictionary<string, ElementalData> elementaltable = new Dictionary<string, ElementalData>();
        public Dictionary<string, LevelUpItemData> leveluptable = new Dictionary<string, LevelUpItemData>();

        public Dictionary<string, List<Bonus>> bonuses = new Dictionary<string, List<Bonus>>();
        public List<ResearchData> researchtable = new List<ResearchData>();


        public Dictionary<string, GameModeData> gamemodetable = new Dictionary<string, GameModeData>();

        internal void LoadAll()
        {
            LoadHeroData();
            LoadMonsterData();
            LoadElementData();
            LoadStageData();
            LoadLevelUpData();
            LoadResearchData();
        }

        private void LoadMonsterData()
        {
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
            herotable.Add("hero", new HeroData());
        }

        private void LoadLevelUpData()
        {
            LevelUpItemData heroAttack = new LevelUpItemData("L001", "hero", "일반공격", "SexyBackIcon_SWORD2");
            LevelUpItemData item1 = new LevelUpItemData("L002", "fireball", "파이어볼", "SexyBackIcon_FireElemental");
            LevelUpItemData item2 = new LevelUpItemData("L003", "waterball", "물폭탄", "SexyBackIcon_WaterElemental");
            LevelUpItemData item3 = new LevelUpItemData("L004", "rock", "짱돌", "SexyBackIcon_RockElemental");
            LevelUpItemData item4 = new LevelUpItemData("L005", "electricball", "지지직", "SexyBackIcon_ElectricElemental");
            LevelUpItemData item5 = new LevelUpItemData("L006", "snowball", "눈덩이", "SexyBackIcon_SnowElemental");
            LevelUpItemData item6 = new LevelUpItemData("L007", "earthball", "똥", "SexyBackIcon_EarthElemental");
            LevelUpItemData item7 = new LevelUpItemData("L008", "airball", "바람바람", "SexyBackIcon_AirElemental");
            LevelUpItemData item8 = new LevelUpItemData("L009", "iceblock", "각얼음", "SexyBackIcon_IceElemental");
            LevelUpItemData item9 = new LevelUpItemData("L010", "magmaball", "메테오", "SexyBackIcon_MagmaElemental");
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

        private void LoadStageData()
        {
            GameModeData data1 = new GameModeData("TestStage", 20, 0);
            gamemodetable.Add(data1.ID, data1);

        }

        private void LoadElementData()
        {
            TextAsset textasset = Resources.Load("Xml/ElementalData") as TextAsset;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(textasset.text);
            XmlNode rootNode = xmldoc.SelectSingleNode("Elementals");
            XmlNodeList nodes = rootNode.SelectNodes("Elemental");

            List<Bonus> group = new List<Bonus>();
            foreach (XmlNode node in nodes)
            {
                string id = node.Attributes["id"].Value;
                string name = node.Attributes["name"].Value;
                int attackIntervalK = int.Parse(node.Attributes["attackIntervalK"].Value);
                string basedps = node.Attributes["basedps"].Value;
                string baseexp = node.Attributes["baseexp"].Value;

                ElementalData elemental;
                if (basedps.Contains("."))
                {
                    double basedpsdouble = double.Parse(basedps);
                    elemental = new ElementalData(id, name, attackIntervalK, basedpsdouble, new BigInteger(baseexp));
                }
                else
                    elemental = new ElementalData(id, name, attackIntervalK, new BigInteger(basedps), new BigInteger(baseexp));
                elementaltable.Add(id, elemental);
            }
        }

        private void LoadBonus()
        {
            TextAsset textasset = Resources.Load("Xml/BonusData") as TextAsset;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(textasset.text);
            XmlNode rootNode = xmldoc.SelectSingleNode("BonusList");
            XmlNodeList nodes = rootNode.SelectNodes("Bonus");

            List<Bonus> group = new List<Bonus>();
            foreach (XmlNode node in nodes)
            {
                string groupid = node.Attributes["group"].Value;
                string target = node.Attributes["target"].Value;
                string attribute = node.Attributes["attribute"].Value;
                int value;
                if (int.TryParse(node.Attributes["value"].Value, out value) == false)
                    value = 0;
                string stringvalue = node.Attributes["stringvalue"].Value;

                Bonus bonus = new Bonus(target, attribute, value, stringvalue);
                if (!bonuses.ContainsKey(groupid))
                {
                    List<Bonus> grouplist = new List<Bonus>();
                    grouplist.Add(bonus);
                    bonuses.Add(groupid, grouplist);
                }
                else
                    bonuses[groupid].Add(bonus);
            }
        }

        private void TestParsing()
        {
            //foreach (List<Bonus> list in bonuses.Values)
            //{
            //    foreach (Bonus b in list)
            //    {
            //        sexybacklog.Console(b.strvalue);
            //    }
            //    sexybacklog.Console("nextGroup");
            //}
            //foreach (ResearchData data in researchtable)
            //{
            //    sexybacklog.Console(data.ID);
            //    sexybacklog.Console(data.IconName);
            //    sexybacklog.Console(data.bonuses[0].strvalue);
            //}

            //foreach (ElementalData a in elementaltable.Values)
            //    sexybacklog.Console(a.BaseDps);

        }

        private void LoadResearchData()
        {
            LoadBonus();

            TextAsset textasset = Resources.Load("Xml/ResearchData") as TextAsset;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(textasset.text);
            XmlNode rootNode = xmldoc.SelectSingleNode("Researches");
            XmlNodeList nodes = rootNode.SelectNodes("Research");

            List<Bonus> group = new List<Bonus>();
            foreach (XmlNode node in nodes)
            {
                string id = node.Attributes["id"].Value;
                string requireid = node.Attributes["requireid"].Value;
                int requirelevel = int.Parse(node.Attributes["requirelevel"].Value);

                XmlNode infonode = node.SelectSingleNode("Info");
                string icon = infonode.Attributes["icon"].Value;
                string name = infonode.Attributes["name"].Value;
                string description = infonode.Attributes["description"].Value;

                XmlNode pricenode = node.SelectSingleNode("Price");
                int pricevalue = int.Parse(pricenode.Attributes["value"].Value);
                string pricedigit = pricenode.Attributes["digit"].Value;
                BigIntExpression price = new BigIntExpression(pricevalue, pricedigit);

                XmlNode potnode = node.SelectSingleNode("PriceOverTime");
                int potvalue = int.Parse(potnode.Attributes["value"].Value);
                string potdigit = potnode.Attributes["digit"].Value;
                int time = int.Parse(potnode.Attributes["time"].Value);
                BigIntExpression pot = new BigIntExpression(potvalue, potdigit);

                XmlNode bonusnode = node.SelectSingleNode("BonusList");
                string groupid = bonusnode.Attributes["groupid"].Value;

                List<Bonus> bonuselist;
                if (!bonuses.ContainsKey(groupid))
                    bonuselist = new List<Bonus>();
                else
                    bonuselist = bonuses[groupid];

                ResearchData research = new ResearchData(id, requireid, requirelevel, bonuselist, price, pot, time, icon, name, description);
                researchtable.Add(research);
            }


        }
    }

}