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
        public string MapID = null;
        public bool IsBonused = false;
        public double CurrentGameTime;
        [NonSerialized]
        public static int LimitGameTime;

        // exp
        BigInteger exp;
        public BigInteger EXP { get { return exp; } }
        public delegate void ExpChange_Event(BigInteger exp);
        [field: NonSerialized]
        public event ExpChange_Event Action_ExpChange;
        [NonSerialized]
        public int expIncreaseXH;

        public string GetMapID
        {
            get
            {
                if (MapID == null)
                {
                    sexybacklog.Console("플레이씬에서 새 게임을 시작합니다. 더미맵을 출력합니다.");
                    MapID = "Map01";
                }
                return MapID;
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
            Singleton<PlayerStatus>.getInstance().Action_UtilStatChange -= onUtilStatChange;
        }
        internal void Init()
        {
            exp = new BigInteger();
            Singleton<PlayerStatus>.getInstance().Action_UtilStatChange += this.onUtilStatChange;
        }
        internal void Start() //new game
        {
            SetStat(Singleton<PlayerStatus>.getInstance().GetUtilStat);
            MapID = GetMapID;   // 외부에서 setting 되어있으면 setting된값으로 아니면 더미값을출력
            // isBonused // 외부에서 이미 setting; 안되있으면 false;
            CurrentGameTime = 0;
            ExpGain(Singleton<PlayerStatus>.getInstance().GetUtilStat.InitExp, false);
            LimitGameTime = Singleton<TableLoader>.getInstance().mapTable[MapID].LimitTime;
        }
        internal void Load(XmlDocument doc)
        {
            SetStat(Singleton<PlayerStatus>.getInstance().GetUtilStat);

            XmlNode mainNode = doc.SelectSingleNode("InstanceStatus");
            ExpGain(new BigInteger(mainNode.Attributes["Exp"].Value), false);
            MapID = mainNode.Attributes["MapID"].Value;
            IsBonused = bool.Parse(mainNode.Attributes["IsBonused"].Value);
            LimitGameTime = Singleton<TableLoader>.getInstance().mapTable[MapID].LimitTime;
            CurrentGameTime = double.Parse(mainNode.Attributes["CurrentGameTime"].Value);
        }

        public void Update()
        {
            CurrentGameTime += Time.deltaTime;
            sexybacklog.InGame("총시간 = " + (int)CurrentGameTime + " 초");
            sexybacklog.InGame("빨리감기 +" + Singleton<GameInput>.getInstance().fowardtimefordebug + " 초");
            sexybacklog.InGame("레벨업에쓴돈 " + lspend.To5String());
            sexybacklog.InGame("리서치에쓴돈 " + rspend.To5String());
        }
        internal void SetStat(UtilStat uStat)
        {
            expIncreaseXH = uStat.ExpIncreaseXH;
        }
        public void onUtilStatChange(UtilStat newStat)
        {
            SetStat(newStat);
        }
        internal void Upgrade(BonusStat bonus, GridItemIcon icon)
        {
            // case skill, active, stat 분리
            switch (bonus.attribute)
            {
                case "Active":
                    Singleton<ElementalManager>.getInstance().LearnNewElemental(bonus.strvalue);
                    break;
                case "ActiveSkill":
                    Singleton<ElementalManager>.getInstance().ActiveSkill(bonus.strvalue);
                    break;
                case "Enchant":
                    Singleton<HeroManager>.getInstance().Enchant(bonus.strvalue);
                    break;
                case "FinishResearch":
                    Singleton<ResearchManager>.getInstance().FinishFrontOne();
                    break;
                default:
                    Singleton<PlayerStatus>.getInstance().ApplySpecialStat(bonus, true);
                    break;
            }
            EffectController.getInstance.AddBuffEffect(icon);
        }
        internal void DownGrade(BonusStat bonus, GridItemIcon icon)
        {
            Singleton<PlayerStatus>.getInstance().ApplySpecialStat(bonus, false);
            EffectController.getInstance.AddBuffEffect(icon);
        }

        internal void Buff(BonusStat bonus, GridItemIcon icon, int duration)
        {
        }

        public void ExpGain(BigInteger e, bool isApplyStat)
        {
            if (isApplyStat)
                exp += e * expIncreaseXH / 100;
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
