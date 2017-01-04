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

        
        public SexyBackHero()
        {

        }

        public void AttackDPC(SexyBackMonster target)
        {
            if (ISCRITICAL)
            {
                // CriticalACtion Play;
                double totaldamage = DPC * CRIDMG;
                target.Hit(totaldamage, true);
                Gain(totaldamage);
                GameManager.SexyBackLog("Crit");
                // CriticalGAmeOBject 풀 
                //GameObject.Find("hitparticle").GetComponent<ParticleSystem>().Play();
                GameObject.Find("slash").GetComponent<Slash>().Play();

            }
            else
            {
                //AttackAction Play;
                GameManager.SexyBackLog("NOrmal");
                target.Hit(DPC, true);
                Gain(DPC);
                //EffectMaker.GetInstance().PlayAttackEffect();
                //GameObject.Find("hitparticle").GetComponent<ParticleSystem>().Play();
                GameObject.Find("slash").GetComponent<Slash>().Play();
                //GameObject.Instantiate<GameObject>(Resources.Load("Prefabs/slash") as GameObject);
            }
        }

        public void AttackDPS(float deltaTime, SexyBackMonster target)
        {
            target.Hit(deltaTime * DPS, false);
            Gain(deltaTime * DPS);
        }

        public void IncreaseDPC(double amount)
        {
            DPC += amount;
            UIUpdater.getInstance().noticeDamageChanged(DPC, DPS);
        }

        void Gain(double exp)
        {
            EXP += exp;

            UIUpdater.getInstance().noteiceExpChanged(EXP);
        }
    }
}