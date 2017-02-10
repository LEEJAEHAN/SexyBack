using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SexyBackPlayScene
{
    internal class Player // 누적배수를 가지고있다.
    {
        HeroManager heromanager = Singleton<HeroManager>.getInstance();
        ElementalManager elementalmanager = Singleton<ElementalManager>.getInstance();

        HeroUpgradeStat heroStat;
        List<ElementalUpgradeStat> elementalStats = new List<ElementalUpgradeStat>();

        internal HeroUpgradeStat GetHeroStat { get { return heroStat; } }
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
            foreach (string elementalid in Singleton<TableLoader>.getInstance().elementaltable.Keys)
                elementalStats.Add(new ElementalUpgradeStat(elementalid, new BigInteger(1), 100));
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
                                if(stat.ID == bonus.targetID)
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
                default:
                    {
                        sexybacklog.Error("업그레이드가능한 attribute가 없습니다.");
                        break;
                    }
            }
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
