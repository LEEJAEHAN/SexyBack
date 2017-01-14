using System;
using System.Collections.Generic;
using UnityEngine;
namespace SexyBackPlayScene
{
    internal class SexyBackHero : CanLevelUp
    {
        public BigInteger DPC = 1;
        public BigInteger EXP = 0;

        public double CRIRATE = 0.15;
        public int CRIDAMAGE = 200;

        public bool ISCRITICAL { get { return CRIRATE > UnityEngine.Random.Range(0.0f,1.0f); } }

        public GameObject slash;
        public GameObject avatar;
        public override string ItemViewID { get { return "heroattack"; } }

        public override string Name
        {
            get
            {
                return "Hero";
            }
        }

        public override string DamageStatusText { get { return "Damage : " + DPC.ToSexyBackString() + "/tap\n" + "Next : +" + DPC.ToSexyBackString() + "/tap"; } }

        public SexyBackHero()
        {
            slash = ViewLoader.slash;
            avatar = ViewLoader.hero;
        }

        internal void Update()
        {
        }

        public void AttackDPC()
        {
            SexyBackMonster target = Singleton<MonsterManager>.getInstance().GetMonster();

            if (ISCRITICAL)
            {
                BigInteger totaldamage = DPC * CRIDAMAGE / 100;
                target.OnHit(totaldamage);
                // 크리티컬 글자 필요 
                slash.GetComponent<Slash>().Play();
                avatar.GetComponent<Animator>().SetTrigger("Attack");
            }
            else
            {
                target.OnHit(DPC);
                slash.GetComponent<Slash>().Play();
                avatar.GetComponent<Animator>().SetTrigger("Attack");                
            }
        }

        internal void GainExp(BigInteger damage)
        {
            EXP += damage;
            Singleton<HeroManager>.getInstance().noteiceExpChanged();
        }

        public void IncreaseDPC(BigInteger amount)
        {
            DPC += amount;
            Singleton<HeroManager>.getInstance().noticeDamageChanged();
        }

        internal BigInteger GetTotalDPC()
        {
            return DPC;
        }

        public override void LevelUp(int lv)
        {
            level = lv;
            // 데미지업 미구현
            Singleton<HeroManager>.getInstance().noticeDamageChanged();
        }

        public override string GetDamageString()
        {
            return DPC.ToSexyBackString();
        }
    }
}