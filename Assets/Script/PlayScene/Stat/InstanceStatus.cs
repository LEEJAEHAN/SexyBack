using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Xml;

namespace SexyBackPlayScene 
{
    // instance 던젼안에서의 스텟을 관리한다. BattleScene안에서만 동작한다.
    [Serializable]
    internal class InstanceStatus : IDisposable
    {
        ~InstanceStatus()
        {
            sexybacklog.Console("InstanceStat 소멸");
        }
        // map and 진행 info
        Map MapInfo;
        //public string MapID = null;
        public bool IsBonused = false;
        public double CurrentGameTime;
        [NonSerialized]
        public static int LimitGameTime;

        bool RefreshStat = true;
        // exp
        BigInteger exp;
        public BigInteger EXP { get { return exp; } }
        public delegate void ExpChange_Event(BigInteger exp);
        public Action<int> Action_Buff;

        [field: NonSerialized]
        public event ExpChange_Event Action_ExpChange;
        [NonSerialized]
        public int BuffCoef = 1;

        TopWindow topView;

        //public void Load()
        //{
        //    XmlDocument doc = SaveSystem.LoadXml(SaveSystem.SaveDataPath);
        //    globalStat = new GlobalStat(doc.SelectSingleNode("PlayerStatus/Stats/GlobalStat"));
        //    baseStat = new BaseStat(doc.SelectSingleNode("PlayerStatus/Stats/BaseStat"));
        //    utilStat = new UtilStat(doc.SelectSingleNode("PlayerStatus/Stats/UtilStat"));
        //    heroStat = new HeroStat(doc.SelectSingleNode("PlayerStatus/Stats/HeroStat"));

        //    elementalStats = new Dictionary<string, ElementalStat>();
        //    foreach (string elementalid in Singleton<TableLoader>.getInstance().elementaltable.Keys)
        //        elementalStats.Add(elementalid, new ElementalStat());
        //    foreach (XmlNode node in doc.SelectSingleNode("PlayerStatus/Stats/ElementalStats").ChildNodes)
        //        elementalStats[node.Attributes["id"].Value].LoadStat(node);
        //}
        internal void Load(XmlDocument doc)
        {
            XmlNode mainNode = doc.SelectSingleNode("InstanceStatus");
            ExpGain(new BigInteger(mainNode.Attributes["Exp"].Value), false);
            string mapid = mainNode.Attributes["MapID"].Value;
            MapInfo = Singleton<MapManager>.getInstance().Maps[mapid];
            IsBonused = bool.Parse(mainNode.Attributes["IsBonused"].Value);
            LimitGameTime = MapInfo.baseData.LimitTime;
            CurrentGameTime = double.Parse(mainNode.Attributes["CurrentGameTime"].Value);
        }

        public Map InstanceMap
        {
            get
            {
                if (MapInfo == null)
                {
                    sexybacklog.Console("플레이씬에서 새 게임을 시작합니다. Map01을 시작합니다.");
                    MapInfo = Singleton<MapManager>.getInstance().Maps["Map01"];
                }
                return MapInfo;
            }
            set
            {
                MapInfo = value;
            }
        }

        // for test
        [NonSerialized]
        BigInteger lspend = new BigInteger(0);
        [NonSerialized]
        BigInteger rspend = new BigInteger(0);

        public void Dispose()
        {
            Action_ExpChange = null;
            exp = null;
            lspend = null;
            rspend = null;
            Singleton<PlayerStatus>.getInstance().Action_BaseStatChange -= onBaseStatchange;
            Singleton<PlayerStatus>.getInstance().Action_UtilStatChange -= onUtilStatChange;
        }
        internal void Init()
        {
            exp = new BigInteger();
            topView = GameObject.Find("Top_Window").GetComponent<TopWindow>();
            Singleton<PlayerStatus>.getInstance().Action_BaseStatChange += onBaseStatchange;
            Singleton<PlayerStatus>.getInstance().Action_UtilStatChange += onUtilStatChange;
        }
        internal void Start() //new game
        {
            MapInfo = InstanceMap; // 외부에서 setting 되어있으면 setting된값으로 아니면 더미값을출력
            // isBonused // 외부에서 이미 setting; 안되있으면 false;
            CurrentGameTime = 0;

            int initexp = (int)Math.Pow(2, Singleton<PlayerStatus>.getInstance().GetGlobalStat.InitExpCoef) * 100;
            ExpGain(new BigInteger(initexp), false);
            LimitGameTime = MapInfo.baseData.LimitTime;
        }

        public void Update()
        {
            if (RefreshStat)
            {
                SetStat();
                RefreshStat = false;
            }

            CurrentGameTime += Time.deltaTime;
            topView.PrintSlot1String(TimeSpan.FromSeconds((int)CurrentGameTime).ToString());

            sexybacklog.InGame("총시간 = " + (int)CurrentGameTime + " 초");
            sexybacklog.InGame("빨리감기 +" + Singleton<GameInput>.getInstance().fowardtimefordebug + " 초");
            sexybacklog.InGame("레벨업에쓴돈 " + lspend.To5String());
            sexybacklog.InGame("리서치에쓴돈 " + rspend.To5String());
        }

        private void SetStat()
        {
            //BaseStat baseStat = Singleton<PlayerStatus>.getInstance().GetBaseStat;
            //ExpXH = (200 + baseStat.Luck) / 2;

            Action_Buff(BuffCoef);
        }

        public void onUtilStatChange(UtilStat utilStat)
        {
            //utilStat.ExpIncreaseXH
            RefreshStat = true;
        }
        public void onBaseStatchange(BaseStat baseStat)
        {
            RefreshStat = true;
        }

        internal void Upgrade(BonusStat bonus, NestedIcon icon)
        {
            // case skill, active, stat 분리
            if(bonus.targetID == "ingame")
            {
                switch (bonus.attribute)
                {
                    case Attribute.Active:
                        Singleton<ElementalManager>.getInstance().LearnNewElemental(bonus.strvalue);
                        break;
                    case Attribute.ActiveSkill:
                        Singleton<ElementalManager>.getInstance().ActiveSkill(bonus.strvalue);
                        break;
                    case Attribute.Enchant:
                        Singleton<HeroManager>.getInstance().Enchant(bonus.strvalue);
                        break;
                }
            }
            else
                Singleton<PlayerStatus>.getInstance().ApplySpecialStat(bonus, true);

            EffectController.getInstance.AddBuffEffect(icon);
        }

        internal void BuffExp(bool on, int xtimes)
        {
            if (on)
                BuffCoef = xtimes;
            else
                BuffCoef = 1;

            RefreshStat = true;
        }

        public void ExpGain(BigInteger e, bool isApplyStat)
        {
            if (isApplyStat)
                exp += e * BuffCoef; // * expXh / 100
            else
                exp += e;
            Action_ExpChange(exp);
        }

        internal bool ExpUse(BigInteger e, bool islevelup)
        {
            bool result;

            if (exp - e < 0)
                result = false;
            else
            {
                exp -= e;
                result = true;

                if (islevelup)
                    lspend += e;
                else
                    rspend += e;

            }

            Action_ExpChange(exp);

            return result;
        }


        public static double CalGrowthPower(double growthRate, int power)
        {
            //TODO : 더블에걸림.
            return Math.Pow(growthRate, power);
        }

        internal static double GetTotalDensityPerLevel(int level)
        {
            foreach (PriceData pData in Singleton<TableLoader>.getInstance().pricetable)
            {
                if (level >= pData.minLevel && level <= pData.maxLevel)
                    return pData.basePriceDensity;
            }

            sexybacklog.Error("no PriceInfo Level " + level);
            return double.MaxValue;
        }

    }

}
