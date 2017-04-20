using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SexyBackPlayScene // instance 던젼안에서의 스텟
{
    [Serializable]
    internal class InstanceStat : IDisposable // 던젼안에서의 스텟을 가지고있다. BattleScene안에서만 동작한다.
    {
        ~InstanceStat()
        {
            sexybacklog.Console("InstanceStat 소멸");
        }

        BigInteger exp;
        public BigInteger EXP { get { return exp; } }

        // for test
        [NonSerialized]
        BigInteger lspend = new BigInteger(0);
        [NonSerialized]
        BigInteger rspend = new BigInteger(0);

        public delegate void ExpChange_Event(BigInteger exp);
        [field: NonSerialized]
        public event ExpChange_Event Action_ExpChange;

        // from PlayerStatus
        PlayerStat IPlayerStat;
        HeroStat IHeroSTat;
        Dictionary<string, ElementalStat> IElementalStats;
        internal PlayerStat GetIPlayerStat { get { return IPlayerStat; } }
        internal HeroStat GetIHeroStat { get { return IHeroSTat; } }
        internal Dictionary<string, ElementalStat> GetElementalStats { get { return IElementalStats; } }
        internal ElementalStat GetIElementalStat(string id) { return IElementalStats[id]; }

        public void Dispose()
        {
            IPlayerStat = null;
            IHeroSTat = null;
            IElementalStats = null;
            Action_ExpChange = null;
            exp = null;
            lspend = null;
            rspend = null;
        }

        internal void Start(PlayerStatus stat) //new game
        {
            if(stat.GetPlayerStat == null)
            {
                sexybacklog.Console("플레이씬에서 새 게임을 시작합니다. 더미스텟으로 시작합니다.");
                stat.Init();
            }
            IHeroSTat = stat.GetHeroStat;
            IPlayerStat = stat.GetPlayerStat;
            IElementalStats = stat.GetElementalStats;
            exp = stat.GetPlayerStat.InitExp;
        }

        internal void Load(InstanceStat sData) // load game
        {
            IHeroSTat = sData.GetIHeroStat;
            IPlayerStat = sData.GetIPlayerStat;
            IElementalStats = sData.GetElementalStats;
            exp = sData.EXP;
        }

        internal void ApplyStat() // 스텟 적용해야하는놈들, 히어로, 엘리멘탈, 리서치, 레벨업, 인스턴스스텟(exp)
        {
            //for test
            Singleton<ResearchManager>.getInstance().SetStat(IPlayerStat); //첨엔 없고, herocreate시 서먼된다.
            Singleton<LevelUpManager>.getInstance().SetStat(IPlayerStat); //첨엔 없고, herocreate시 서먼된다.
            Singleton<ElementalManager>.getInstance().SetLevelAndStat(IElementalStats); // 첨엔 없고, 서먼된다.
            Singleton<HeroManager>.getInstance().SetLevelAndStat(IHeroSTat);
            ExpGain(exp, false);
        }

        internal void Update()
        {
            sexybacklog.InGame("레벨업에쓴돈 " + lspend.To5String());
            sexybacklog.InGame("리서치에쓴돈 " + rspend.To5String());
        }
        internal void Buff(Bonus bonus, GridItemIcon icon, int duration)
        {

        }
        internal void ApplyBonusWithIcon(Bonus bonus, GridItemIcon icon)
        {
            ApplyBonus(bonus, true);
            EffectController.getInstance.AddBuffEffect(icon);
        }
        internal void DisableBonusWithIcon(Bonus bonus, GridItemIcon icon)
        {
            ApplyBonus(bonus, false);
            EffectController.getInstance.AddBuffEffect(icon);
        }

        internal void Upgrade(BaseStat basestat)
        {
            //              case "DpsIncreaseXH": // TODO : case all elemental 현재 안쓰고있음.
            //        {
            //    foreach (ElementalStat stat in IElementalStats.Values)
            //        stat.DpsIncreaseXH += bonus.value;
            //    elementalmanager.SetStatAll(IElementalStats, true, false);
            //    break;
            //}
            //    case "CastSpeedXH": // case all elemental
            //        {
            //    foreach (ElementalStat stat in IElementalStats.Values)
            //        stat.CastSpeedXH += bonus.value;
            //    elementalmanager.SetStatAll(IElementalStats, true, false);
            //    break;
            //}
        }
        internal void ApplyBonus(Bonus bonus, bool signPositive)
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
        private void ApplyHeroBonus(Bonus bonus, bool signPositive)
        {
            if (signPositive)
                IHeroSTat.Add(bonus);
            else
                IHeroSTat.Remove(bonus);

            HeroManager hManager = Singleton<HeroManager>.getInstance();
            switch (bonus.attribute) // 이벤트 처리
            {
                case "Level":
                    hManager.SetLevelAndStat(IHeroSTat);
                    break;
                case "ActiveElement":
                    Singleton<ElementalManager>.getInstance().LearnNewElemental(bonus.strvalue);
                    break;
                case "Enchant":
                case "DpcX":
                case "DpcIncreaseXH":
                    hManager.SetStat(IHeroSTat, true, false);
                    break;
                default:
                    hManager.SetStat(IHeroSTat, false, false);
                    break;
            }
        }

        private void ApplyElementalBonus(Bonus bonus, bool signPositive) // 각 element에게만 해당하는것. 전체는 player
        {
            string targetID = bonus.targetID;
            ElementalStat IElementalStat = IElementalStats[bonus.targetID];

            if (signPositive)
                IElementalStat.Add(bonus);
            else
                IElementalStat.Remove(bonus);

            ElementalManager eManager = Singleton<ElementalManager>.getInstance();
            switch (bonus.attribute)
            {
                case "Level":
                    eManager.SetLevelAndStat(IElementalStat, targetID);
                    break;
                case "ActiveSkill":
                    eManager.ActiveSkill(IElementalStat, targetID);
                    break;
                case "DpsX":
                case "DpsIncreaseXH":
                case "CastSpeedXH":
                    eManager.SetStat(IElementalStat, targetID, true, false);
                    break;
                default:
                    eManager.SetStat(IElementalStat, targetID, false, false);
                    break;
            }
        }
        private void ApplyPlayerBonus(Bonus bonus, bool signPositive)
        {
            if (signPositive)
                IPlayerStat.Add(bonus);
            else
                IPlayerStat.Remove(bonus);

            switch (bonus.attribute)
            {
                case "FinishResearch":
                    Singleton<ResearchManager>.getInstance().FinishFrontOne();
                    break;
                case "ExpPerFloor":
                    ExpGain(bonus.bigvalue, false);
                    break;
                case "ResearchTimeX":
                case "ResearchTime":
                case "ResearchThread":
                case "ResearchPriceXH":
                    Singleton<ResearchManager>.getInstance().SetStat(IPlayerStat);
                    break;
                case "LevelUpPriceXH":
                    Singleton<LevelUpManager>.getInstance().SetStat(IPlayerStat);
                    break;
            }
        }
        public void ExpGain(BigInteger e, bool isApplyStat)
        {
            if (isApplyStat)
                exp += e * IPlayerStat.ExpIncreaseXH / 100;
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
