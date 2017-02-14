﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SexyBackPlayScene
{
    internal class Player // 누적배수를 가지고있다.
    {
        private BigInteger exp = new BigInteger(0);
        public BigInteger EXP { get { return exp; } }


        public delegate void ExpChange_Event(BigInteger exp);
        public event ExpChange_Event Action_ExpChange;


        HeroManager heromanager = Singleton<HeroManager>.getInstance();
        ElementalManager elementalmanager = Singleton<ElementalManager>.getInstance();


        HeroUpgradeStat heroStat;
        ResearchUpgradeStat researchStat;
        List<ElementalUpgradeStat> elementalStats = new List<ElementalUpgradeStat>();

        internal HeroUpgradeStat GetHeroStat { get { return heroStat; } }
        internal ResearchUpgradeStat GetResearchStat { get { return researchStat; } }
        internal ElementalUpgradeStat GetElementalStat(string id)
        {
            foreach (ElementalUpgradeStat stat in elementalStats)
                if (stat.ID == id)
                    return stat;
            return null;
        }

        public Player()
        {
            heroStat = new HeroUpgradeStat(new BigInteger(1), 100, 1000, 6);
            researchStat = new ResearchUpgradeStat(1, 0);
            foreach (string elementalid in Singleton<TableLoader>.getInstance().elementaltable.Keys)
                elementalStats.Add(new ElementalUpgradeStat(elementalid, new BigInteger(1), 100));
        }

        internal void Init(GameModeData args)
        {
            // 아직할것없음.
        }

        internal void Start()
        {
            ExpGain(100000);

            heromanager.CreateHero(); // and hero is move

            Upgrade(new Bonus("hero", "ResearchTimeX", 2, ""));

            Upgrade(new Bonus("hero", "ResearchTime", 3, ""));

            //elementalmanager.LearnNewElemental("magmaball");
            //elementalManager.SummonNewElemental("fireball");
            //elementalManager.SummonNewElemental("waterball");
            //elementalManager.SummonNewElemental("rock");
            //elementalManager.SummonNewElemental("electricball");
            //elementalManager.SummonNewElemental("snowball");
            //elementalManager.SummonNewElemental("earthball");
            //elementalManager.SummonNewElemental("airball"); // for test
            //elementalManager.SummonNewElemental("iceblock");
        }

        internal void Upgrade(Bonus bonus)
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
                        heromanager.GetHero(bonus.targetID).SetDamageX(heroStat.DpcX);
                        break;
                    }
                case "BounsAttackCount":
                    {
                        heroStat.BounsAttackCount += bonus.value;
                        heromanager.GetHero(bonus.targetID).SetStat(heroStat);
                        break;
                    }
                case "HeroAttackspeedXH":
                    {
                        heroStat.HeroAttackspeedXH += bonus.value;
                        heromanager.GetHero(bonus.targetID).SetStat(heroStat);
                        break;
                    }
                case "CriticalRate":
                    {
                        heroStat.CriticalRate += bonus.value;
                        heromanager.GetHero(bonus.targetID).SetStat(heroStat);
                        break;
                    }
                case "CriticalDamage":
                    {
                        heroStat.CriticalDamage += bonus.value;
                        heromanager.GetHero(bonus.targetID).SetStat(heroStat);
                        break;
                    }
                case "MovespeedXH":
                    {
                        heroStat.MovespeedXH += bonus.value;
                        heromanager.GetHero(bonus.targetID).SetStat(heroStat);
                        break;
                    }
                // 여기까지 hero
                // 여기서부턴 elemental
                case "DpsX": // dpsx all;
                    {
                        if (bonus.targetID == "All")
                            foreach (ElementalUpgradeStat stat in elementalStats)
                            {
                                stat.DpsX *= bonus.value;
                                elementalmanager.SetDamageX(stat.DpsX, stat.ID);
                            }
                        else
                        {
                            foreach (ElementalUpgradeStat stat in elementalStats)
                            {
                                if (stat.ID == bonus.targetID)
                                {
                                    stat.DpsX *= bonus.value;
                                    elementalmanager.SetDamageX(stat.DpsX, stat.ID);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case "ElementalAttackSpeedXH":
                    {
                        if (bonus.targetID == "All")
                            foreach (ElementalUpgradeStat stat in elementalStats)
                            {
                                stat.ElementalAttackSpeedXH += bonus.value;
                                elementalmanager.SetStat(stat, stat.ID);
                            }
                        else
                        {
                            foreach (ElementalUpgradeStat stat in elementalStats)
                            {
                                if (stat.ID == bonus.targetID)
                                {
                                    stat.ElementalAttackSpeedXH += bonus.value;
                                    elementalmanager.SetStat(stat, stat.ID);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case "ResearchTimeX":
                    {
                        researchStat.ReduceTimeX *= bonus.value;
                        Singleton<ResearchManager>.getInstance().ReduceTime(researchStat);
                        break;
                    }
                case "ResearchTime":
                    {
                        researchStat.ReduceTime += bonus.value;
                        Singleton<ResearchManager>.getInstance().ReduceTime(researchStat);
                        break;
                    }
                default:
                    {
                        sexybacklog.Error("업그레이드가능한 attribute가 없습니다.");
                        break;
                    }
            }
        }


        public void ExpGain(BigInteger e)
        {
            exp += e;
            Action_ExpChange(exp);
        }
        internal bool ExpUse(BigInteger e)
        {
            bool result;

            if (exp - e < 0)
                result = false;
            else
            {
                exp -= e;
                result = true;
            }

            Action_ExpChange(exp);
            return result;
        }

    }

    internal struct ResearchUpgradeStat
    {
        internal int ReduceTimeX; // 곱계수는 X를붙인다.
        internal int ReduceTime; // 보너스 공격스택횟수  6

        public ResearchUpgradeStat(int reducetimex, int reducetime) : this()
        {
            ReduceTimeX = reducetimex;
            ReduceTime = reducetime;
        }
    }

    internal struct HeroUpgradeStat
    {
        internal BigInteger DpcX; // 곱계수는 X를붙인다.
        internal int BounsAttackCount; // 보너스 공격스택횟수  6
        internal int HeroAttackspeedXH;
        internal double CriticalRate;
        internal int CriticalDamage;
        internal int MovespeedXH;

        public HeroUpgradeStat(BigInteger dpcx, int heroattackspeedxh, int movespeedxh, int bounsattackcount) : this()
        {
            DpcX = dpcx;
            HeroAttackspeedXH = heroattackspeedxh;
            MovespeedXH = movespeedxh;
            BounsAttackCount = bounsattackcount;
        }
    }
    internal class ElementalUpgradeStat // 누적배수
    {
        internal string ID;
        internal int ElementalAttackSpeedXH; //
        internal BigInteger DpsX;

        internal ElementalUpgradeStat(string id, BigInteger dpsx, int attackspeedxh)
        {
            ID = id;
            ElementalAttackSpeedXH = attackspeedxh;
            DpsX = dpsx;
        }
    }



}
