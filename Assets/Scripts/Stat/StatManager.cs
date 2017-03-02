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

        }
        internal void Start()
        {
            heromanager.CreateHero(); // and hero is move
            //Singleton<Player>.getInstance().ExpGain(new BigInteger(new BigIntExpression(150, "m")));
            //elementalmanager.LearnNewElemental("magmaball");
            //elementalmanager.LearnNewElemental("fireball");
            //elementalmanager.LearnNewElemental("waterball");
            //elementalmanager.LearnNewElemental("rock");
            //elementalmanager.LearnNewElemental("electricball");
            //elementalmanager.LearnNewElemental("snowball");
            //elementalmanager.LearnNewElemental("earthball");
            //elementalmanager.LearnNewElemental("airball"); // for test
            //elementalmanager.LearnNewElemental("iceblock");
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
                        heromanager.GetHero().SetStat(heroStat);
                        heromanager.GetHero().SetDamageStat(heroStat);
                        break;
                    }
                case "DpcIncreaseXH":
                    {
                        heroStat.DpcIncreaseXH += bonus.value;
                        heromanager.GetHero().SetStat(heroStat);
                        heromanager.GetHero().SetDamageStat(heroStat);
                        break;
                    }
                case "AttackCount":
                    {
                        heroStat.AttackCount += bonus.value;
                        heromanager.GetHero().SetStat(heroStat);
                        break;
                    }
                case "AttackSpeedXH":
                    {
                        heroStat.AttackSpeedXH += bonus.value;
                        heromanager.GetHero().SetStat(heroStat);
                        break;
                    }
                case "CriticalRateXH":
                    {
                        heroStat.CriticalRateXH += bonus.value;
                        heromanager.GetHero().SetStat(heroStat);
                        break;
                    }
                case "CriticalDamageXH":
                    {
                        heroStat.CriticalDamageXH += bonus.value;
                        heromanager.GetHero().SetStat(heroStat);
                        break;
                    }
                case "MovespeedXH":
                    {
                        heroStat.MovespeedXH += bonus.value;
                        heromanager.GetHero().SetStat(heroStat);
                        break;
                    }
                case "Fever":
                    {
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
                        elementalmanager.SetStat(elementalStats[bonus.targetID], bonus.targetID);
                        break;
                    }
                case "DpsIncreaseXH":
                    {
                        if (bonus.targetID == "elementals")
                        {
                            foreach (ElementalStat stat in elementalStats.Values)
                                stat.DpsIncreaseXH += bonus.value;
                            elementalmanager.SetStatAll(elementalStats);
                        }
                        else
                        {
                            elementalStats[bonus.targetID].DpsIncreaseXH += bonus.value;
                            elementalmanager.SetStat(elementalStats[bonus.targetID], bonus.targetID);
                        }
                        break;
                    }
                case "CastSpeedXH":
                    {
                        elementalStats[bonus.targetID].CastSpeedXH += bonus.value;
                        elementalmanager.SetStat(elementalStats[bonus.targetID], bonus.targetID);
                        break;
                    }
                case "Fever":
                    {
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
            }
        }
        public void ExpGain(BigInteger e)
        {
            exp += e * playerStat.ExpIncreaseXH / 100;
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

}
