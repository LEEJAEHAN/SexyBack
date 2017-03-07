using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class StatManager // 누적배수를 가지고있다.
    {
        private BigInteger exp = new BigInteger(0);
        public BigInteger EXP { get { return exp; } }

        // for test
        BigInteger lspend = new BigInteger(0);
        BigInteger rspend = new BigInteger(0);

        public delegate void ExpChange_Event(BigInteger exp);
        public event ExpChange_Event Action_ExpChange;

        PlayerStat playerStat;
        HeroStat heroStat;
        Dictionary<string, ElementalStat> elementalStats = new Dictionary<string, ElementalStat>();

        internal PlayerStat GetPlayerStat { get { return playerStat; } }
        internal HeroStat GetHeroStat { get { return heroStat; } }
        internal ElementalStat GetElementalStat(string id) { return elementalStats[id]; }

        HeroManager heromanager = Singleton<HeroManager>.getInstance();
        ElementalManager elementalmanager = Singleton<ElementalManager>.getInstance();

        public StatManager()
        {
            heroStat = new HeroStat();
            playerStat = new PlayerStat();
            foreach (string elementalid in Singleton<TableLoader>.getInstance().elementaltable.Keys)
                elementalStats.Add(elementalid, new ElementalStat());
        }

        internal void Init(GameModeData args)
        {
            // 아직할것없음.
        }

        internal void Update()
        {
            sexybacklog.InGame("레벨업에쓴돈 " + lspend.To5String());
            sexybacklog.InGame("리서치에쓴돈 " + rspend.To5String());
        }
        internal void Start()
        {
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
                        Singleton<ResearchManager>.getInstance().SetThread(playerStat);
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
                        ExpGain(bonus.bigvalue);
                        break;
                    }
                default:
                    {
                        sexybacklog.Error("noAttribute");
                        break;
                    }
            }
        }
        public void ExpGain(BigInteger e)
        {
            exp += e * playerStat.ExpIncreaseXH / 100;
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

                if(islevelup)
                    lspend += e;
                else
                    rspend += e;

            }

            Action_ExpChange(exp);

            return result;
        }

    }

}
