using System;
using UnityEngine;
namespace SexyBackPlayScene
{
    internal class SexyBackHero
    {
        public double DPC = 1;
        public double DPS = 1;
        public double EXP = 0;

        public double CRIRATE = 0.15;
        public double CRIDMG = 2;

        public bool ISCRITICAL { get { return CRIRATE > UnityEngine.Random.Range(0.0f,1.0f); } }

        public GameObject slash;
        
        public SexyBackHero()
        {
            slash = GameObject.Find("slash") as GameObject;
        }

        public void AttackDPC(SexyBackMonster target)
        {
            if (ISCRITICAL)
            {
                // GameManager.SexyBackLog("Crit");
                double totaldamage = DPC * CRIDMG;
                target.Hit(totaldamage, true);
                GainEXP(totaldamage);
                // 크리티컬 글자 필요 
                slash.GetComponent<Slash>().Play();
            }
            else
            {
                //GameObject.Instantiate<GameObject>(Resources.Load("Prefabs/slash") as GameObject);
                target.Hit(DPC, true);
                GainEXP(DPC);
                slash.GetComponent<Slash>().Play();
            }
        }

        public void AttackDPS(float deltaTime, SexyBackMonster target)
        {
            target.Hit(deltaTime * DPS, false);
            GainEXP(deltaTime * DPS);
        }

        public void IncreaseDPC(double amount)
        {
            DPC += amount;
            UIUpdater.getInstance().noticeDamageChanged(DPC, DPS);
        }

        public void IncreaseDPS(double amount)
        {
            DPS += amount;
            UIUpdater.getInstance().noticeDamageChanged(DPC, DPS);
        }

        void GainEXP(double exp)
        {
            EXP += exp;
            UIUpdater.getInstance().noteiceExpChanged(EXP);
        }
    }
}