﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SexyBackPlayScene
{
    [Serializable]
    internal class StatManager : IDisposable // 누적배수를 가지고있다. BattleScene안에서만 동작한다.
    {
        ~StatManager()
        {
            sexybacklog.Console("StatManager 소멸");
        }
        public void Dispose()
        {
            heromanager = null;
            elementalmanager = null;
            playerStat = null;
            heroStat = null;
            elementalStats = null;
            Action_ExpChange = null;
            exp = null;
            lspend = null;
            rspend = null;
        }

        private BigInteger exp;
        public BigInteger EXP { get { return exp; } }

        // for test
        [NonSerialized]
        BigInteger lspend = new BigInteger(0);
        [NonSerialized]
        BigInteger rspend = new BigInteger(0);

        public delegate void ExpChange_Event(BigInteger exp);
        [field:NonSerialized]
        public event ExpChange_Event Action_ExpChange;

        PlayerStat playerStat;
        HeroStat heroStat;
        Dictionary<string, ElementalStat> elementalStats;

        internal PlayerStat GetPlayerStat { get { return playerStat; } }
        internal HeroStat GetHeroStat { get { return heroStat; } }
        internal Dictionary<string, ElementalStat> GetElementalStats { get { return elementalStats; } }



        internal ElementalStat GetElementalStat(string id) { return elementalStats[id]; }

        [NonSerialized]
        HeroManager heromanager = Singleton<HeroManager>.getInstance();
        [NonSerialized]
        ElementalManager elementalmanager = Singleton<ElementalManager>.getInstance();

        internal void Init()
        {
            heroStat = new HeroStat();
            playerStat = new PlayerStat();
            elementalStats = MakeElementalStats();
            exp = new BigInteger(0);
        }
        internal void SetInitStat(HeroStat HStat, PlayerStat PStat, Dictionary<string, ElementalStat> EStatList)
        {
            heroStat = HStat;
            playerStat = PStat;
            elementalStats = EStatList;

            // 스텟 적용해야하는놈들, 히어로, 엘리멘탈, 리서치 레벨업, 스텟매니져 
            Singleton<ResearchManager>.getInstance().SetStat(playerStat); //첨엔 없고, herocreate시 서먼된다.
            Singleton<LevelUpManager>.getInstance().SetStat(playerStat); //첨엔 없고, herocreate시 서먼된다.
            elementalmanager.SetStatAll(elementalStats, false); // 첨엔 없고, 서먼된다.
            heromanager.CurrentHero.SetStat(heroStat, false);
        }
        public static Dictionary<string, ElementalStat> MakeElementalStats()
        {
            Dictionary<string, ElementalStat> elementalStats = new Dictionary<string, ElementalStat>();
            foreach (string elementalid in Singleton<TableLoader>.getInstance().elementaltable.Keys)
                elementalStats.Add(elementalid, new ElementalStat());
            return elementalStats;
        }

        internal void Update()
        {
            sexybacklog.InGame("레벨업에쓴돈 " + lspend.To5String());
            sexybacklog.InGame("리서치에쓴돈 " + rspend.To5String());
        }


        internal void Buff(Bonus bonus, GridItemIcon icon, int duration)
        {

        }
        internal void Upgrade(Bonus bonus, GridItemIcon icon)
        {
            switch (bonus.targetID)
            {
                case "hero":
                    UpgradeHero(bonus);
                    break;
                case "player":
                    UpgradePlayer(bonus);
                    break;
                default:
                    UpgradeElemental(bonus);
                    break;
            }
            EffectController.getInstance.AddBuffEffect(icon);
        }


        private void UpgradeHero(Bonus bonus)
        {
            switch (bonus.attribute)
            {
                case "LearnSkill":
                    {
                        elementalmanager.LearnNewElemental(bonus.strvalue);
                        break;
                    }
                case "DpcX":
                    {
                        heroStat.DpcX *= bonus.value;
                        heromanager.GetHero().SetStat(heroStat, true);
                        break;
                    }
                case "DpcIncreaseXH":
                    {
                        heroStat.DpcIncreaseXH += bonus.value;
                        heromanager.GetHero().SetStat(heroStat, true);
                        break;
                    }
                case "AttackCount":
                    {
                        heroStat.AttackCount += bonus.value;
                        heromanager.GetHero().SetStat(heroStat, false);
                        break;
                    }
                case "AttackSpeedXH":
                    {
                        heroStat.AttackSpeedXH += bonus.value;
                        heromanager.GetHero().SetStat(heroStat, false);
                        break;
                    }
                case "CriticalRateXH":
                    {
                        heroStat.CriticalRateXH += bonus.value;
                        heromanager.GetHero().SetStat(heroStat, false);
                        break;
                    }
                case "CriticalDamageXH":
                    {
                        heroStat.CriticalDamageXH += bonus.value;
                        heromanager.GetHero().SetStat(heroStat, false);
                        break;
                    }
                case "MovespeedXH":
                    {
                        heroStat.MovespeedXH += bonus.value;
                        heromanager.GetHero().SetStat(heroStat, false);
                        break;
                    }
                case "Fever":
                    {
                        break;
                    }
                default:
                    {
                        sexybacklog.Error("noAttribute");
                        break;
                    }
            }
        }


        private void UpgradeElemental(Bonus bonus)
        {
            switch (bonus.attribute)
            {
                case "DpsX":
                    {
                        elementalStats[bonus.targetID].DpsX *= bonus.value;
                        elementalmanager.SetStat(elementalStats[bonus.targetID], bonus.targetID, true);
                        break;
                    }
                case "DpsIncreaseXH":
                    {
                        if (bonus.targetID == "elementals")
                        {
                            foreach (ElementalStat stat in elementalStats.Values)
                                stat.DpsIncreaseXH += bonus.value;
                            elementalmanager.SetStatAll(elementalStats, true);
                        }
                        else
                        {
                            elementalStats[bonus.targetID].DpsIncreaseXH += bonus.value;
                            elementalmanager.SetStat(elementalStats[bonus.targetID], bonus.targetID, true);
                        }
                        break;
                    }
                case "CastSpeedXH":
                    {
                        if (bonus.targetID == "elementals")
                        {
                            foreach (ElementalStat stat in elementalStats.Values)
                                stat.CastSpeedXH += bonus.value;
                            elementalmanager.SetStatAll(elementalStats, true);
                        }
                        else
                        {
                            elementalStats[bonus.targetID].CastSpeedXH += bonus.value;
                            elementalmanager.SetStat(elementalStats[bonus.targetID], bonus.targetID, true);
                        }
                        break;
                    }
                case "Fever":
                    {
                        break;
                    }
                default:
                    {
                        sexybacklog.Error("noAttribute");
                        break;
                    }
            }
        }
        private void UpgradePlayer(Bonus bonus)
        {
            switch (bonus.attribute)
            {
                case "ExpIncreaseXH":
                    {
                        playerStat.ExpIncreaseXH += bonus.value;
                        break;
                    }
                case "ResearchTimeX":
                    {
                        playerStat.ResearchTimeX *= bonus.value;
                        Singleton<ResearchManager>.getInstance().SetStat(playerStat);
                        break;
                    }
                case "ResearchTime":
                    {
                        playerStat.ResearchTime += bonus.value;
                        Singleton<ResearchManager>.getInstance().SetStat(playerStat);
                        break;
                    }
                case "ResearchThread":
                    {
                        playerStat.ResearchThread += bonus.value;
                        Singleton<ResearchManager>.getInstance().SetStat(playerStat);
                        break;
                    }
                case "LevelUpPriceXH":
                    {
                        playerStat.LevelUpPriceXH -= bonus.value;
                        Singleton<LevelUpManager>.getInstance().SetStat(playerStat);
                        break;
                    }
                case "ResearchPriceXH":
                    {
                        playerStat.ResearchPriceXH -= bonus.value;
                        Singleton<ResearchManager>.getInstance().SetStat(playerStat);
                        break;
                    }
                case "FinishResearch":
                    {
                        Singleton<ResearchManager>.getInstance().FinishFrontOne();
                        break;
                    }
                case "ExpPerFloor":
                    {
                        ExpGain(bonus.bigvalue, false);
                        break;
                    }
                default:
                    {
                        sexybacklog.Error("noAttribute");
                        break;
                    }
            }
        }
        public void ExpGain(BigInteger e, bool isApplyStat)
        {
            if (isApplyStat)
                exp += e * playerStat.ExpIncreaseXH / 100;
            else
                exp += e;
            Action_ExpChange(exp);
        }
        //public void ExpGain(BigInteger e)
        //{
        //    exp += e * playerStat.ExpIncreaseXH / 100;
        //    Action_ExpChange(exp);
        //}

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



        public static double Growth(double growthRate, int power)
        {
            //TODO : 더블에걸림.
            return Math.Pow(growthRate, power);
        }


        internal static double GetTotalDensityPerLevel(int level)
        {
            foreach (PriceData pData in Singleton<TableLoader>.getInstance().pricetable)
            {
                if (level >= pData.minLevel && level <= pData.maxLevel)
                {
                    sexybacklog.Console("currentlv Density " + pData.basePriceDensity);
                    return pData.basePriceDensity;
                }
            }

            sexybacklog.Error("no PriceInfo Level " +  level);
            return double.MaxValue;
        }

    }

}
