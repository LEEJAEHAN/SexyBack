using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
namespace SexyBackPlayScene
{
    internal class TableLoader : IDisposable
    {
        public HeroData herotable;
        public Dictionary<string, MonsterData> monstertable;
        public Dictionary<string, ElementalData> elementaltable;
        public Dictionary<string, GameModeData> gamemodetable;

        public Dictionary<string, LevelUpData> leveluptable;
        public Dictionary<string, ResearchData> researchtable;
        public List<TalentData> talenttable;

        bool FinishLoad = false;

        ~TableLoader()
        {
            sexybacklog.Console("TableLoader 소멸");
        }

        internal void Init()
        {
            if (FinishLoad == false)
            {
                sexybacklog.Console("Xml데이터를 로드합니다.");
                LoadHeroData();
                LoadMonsterData();
                LoadElementData();
                LoadStageData();
                LoadLevelUpData();
                LoadResearchData();
                LoadTalentData();
            }
            FinishLoad = true;
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

        public void Dispose()
        {
            herotable = null;
            monstertable = null;
            elementaltable = null;
            gamemodetable = null;
            leveluptable = null;
            researchtable = null;
            talenttable = null;
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
            LevelUpData item2 = new LevelUpData("L003", "waterball", "물폭탄", "Icon_09");
            LevelUpData item3 = new LevelUpData("L004", "rock", "짱돌", "Icon_07");
            LevelUpData item4 = new LevelUpData("L005", "electricball", "지지직", "Icon_04");
            LevelUpData item5 = new LevelUpData("L006", "snowball", "눈덩이", "Icon_08");
            LevelUpData item6 = new LevelUpData("L007", "earthball", "똥", "Icon_03");
            LevelUpData item7 = new LevelUpData("L008", "airball", "바람바람", "Icon_02");
            LevelUpData item8 = new LevelUpData("L009", "iceblock", "각얼음", "Icon_05");
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

        private void LoadStageData()
        {
            gamemodetable = new Dictionary<string, GameModeData>();
            GameModeData data1 = new GameModeData("TestStage", 100, 0);
            gamemodetable.Add(data1.ID, data1);
        }

        private void LoadElementData()
        {
            elementaltable = new Dictionary<string, ElementalData>();
            TextAsset textasset = Resources.Load("Xml/ElementalData") as TextAsset;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(textasset.text);
            XmlNode rootNode = xmldoc.SelectSingleNode("Elementals");
            XmlNodeList nodes = rootNode.SelectNodes("Elemental");

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

        private Dictionary<string, List<Bonus>> LoadBonus()
        {
            Dictionary<string, List<Bonus>> bonuses = new Dictionary<string, List<Bonus>>();
            TextAsset textasset = Resources.Load("Xml/BonusData") as TextAsset;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(textasset.text);
            XmlNode rootNode = xmldoc.SelectSingleNode("BonusList");
            XmlNodeList nodes = rootNode.SelectNodes("Bonus");

            foreach (XmlNode node in nodes)
            {
                string groupid = node.Attributes["group"].Value;
                string target = node.Attributes["target"].Value;
                string attribute = node.Attributes["attribute"].Value;
                int value = 0;
                string stringvalue = "";
                if (node.Attributes["value"] != null)
                {
                    if (int.TryParse(node.Attributes["value"].Value, out value) == false)
                        value = 0;
                }
                else
                    stringvalue = node.Attributes["stringvalue"].Value;

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
            return bonuses;
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
            Dictionary<string, List<Bonus>> bonuses = LoadBonus();
            researchtable = new Dictionary<string, ResearchData>();

            TextAsset textasset = Resources.Load("Xml/ResearchData") as TextAsset;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(textasset.text);
            XmlNode rootNode = xmldoc.SelectSingleNode("Researches");
            XmlNodeList nodes = rootNode.SelectNodes("Research");

            foreach (XmlNode node in nodes)
            {
                string id = node.Attributes["id"].Value;
                string requireid = node.Attributes["requireid"].Value;
                int requirelevel = int.Parse(node.Attributes["requirelevel"].Value);

                XmlNode infonode = node.SelectSingleNode("Info");
                string icon = infonode.Attributes["icon"].Value;
                string subicon = null;
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

                XmlNode bonusnode = node.SelectSingleNode("BonusList");
                string groupid = bonusnode.Attributes["groupid"].Value;

                List<Bonus> bonuselist;
                if (!bonuses.ContainsKey(groupid))
                    bonuselist = new List<Bonus>();
                else
                    bonuselist = bonuses[groupid];

                GridItemIcon iconinfo = new GridItemIcon(icon, subicon);
                ResearchData research = new ResearchData(id, requireid, requirelevel, iconinfo, name, description, level, baselevel, baseprice,
                    rate, basetime, bonuselist);
                researchtable.Add(id, research);
            }


        }

        private void LoadTalentData()
        {
            talenttable = new List<TalentData>();

            TextAsset textasset = Resources.Load("Xml/TalentData") as TextAsset;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(textasset.text);
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
                Bonus bonus = new Bonus(target, attribute, value, null);

                GridItemIcon iconinfo = new GridItemIcon(icon, subicon);
                TalentData talentdata = new TalentData(id, iconinfo, description, bonus, type, rate, abs);
                talenttable.Add(talentdata);
            }

            //// test
            //Bonus bonus = new Bonus("hero", "AttackSpeedXH", 5, null);
            //Bonus bonus2 = new Bonus("fireball", "CastSpeedXH", 10, null);
            //Bonus bonus3 = new Bonus("player", "ExpPerFloor", 400, null);

            //talenttable.Add(new TalentData("T01", new GridItemIcon("Icon_10", "A.S"), "공격속도가 $s% 증가합니다.", bonus, TalentType.Attack, 1, true));
            //talenttable.Add(new TalentData("T02", new GridItemIcon("Icon_01", "C.S"), "화염구의 시전속도가 $s% 증가합니다.", bonus2, TalentType.Element, 1, false));
            //talenttable.Add(new TalentData("T03", new GridItemIcon("Icon_18", null), "$s의 경험치를 획득합니다.", bonus3, TalentType.Util, 1, false));
        }

    }
}