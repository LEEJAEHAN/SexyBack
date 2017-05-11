using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SexyBackPlayScene // instance 던젼안에서의 스텟
{
    [Serializable]
    internal class InstanceStatus : IDisposable // 던젼안에서의 스텟을 가지고있다. BattleScene안에서만 동작한다.
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
            ExpGain(Singleton<PlayerStatus>.getInstance().GetUtilStat.InitExp, false);
            MapID = GetMapID;   // 외부에서 setting 되어있으면 setting된값으로 아니면 더미값을출력
            // isBonused // 외부에서 이미 setting; 안되있으면 false;
            LimitGameTime = Singleton<TableLoader>.getInstance().mapTable[MapID].LimitTime;
            expIncreaseXH = 0;
            CurrentGameTime = 0;
        }
        internal void Load(InstanceStatus sData) // load game
        {
            ExpGain(sData.EXP, false);
            MapID = sData.MapID;
            IsBonused = sData.IsBonused;
            LimitGameTime = Singleton<TableLoader>.getInstance().mapTable[MapID].LimitTime;
            //expincrease // 이건stat; 적용될때 적용됨.
            CurrentGameTime = sData.CurrentGameTime;
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
        public void onUtilStatChange(UtilStat newStat, string eventType)
        {
            switch (eventType)
            {
                case "ExpIncreaseXH":
                    expIncreaseXH = newStat.ExpIncreaseXH;
                    break;
            }
        }
        internal void ApplyBonusWithIcon(BonusStat bonus, GridItemIcon icon)
        {
            Singleton<PlayerStatus>.getInstance().ApplySpecialStat(bonus, true);
            EffectController.getInstance.AddBuffEffect(icon);
        }
        internal void DisableBonusWithIcon(BonusStat bonus, GridItemIcon icon)
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
