using System;
using System.Collections.Generic;
using UnityEngine;
namespace SexyBackPlayScene
{
    internal class Hero : CanLevelUp
    {
        public BigInteger DPC = 1;
        public BigInteger EXP = 0;

        public double CRIRATE = 0.15;
        public int CRIDAMAGE = 200;
        public bool ISCRITICAL { get { return CRIRATE > UnityEngine.Random.Range(0.0f,1.0f); } }

        private GameObject slash;
        private GameObject avatar;

        public string targetID;


        // 수정해야함. 임시
        public override string ID { get { return "Hero01"; } }
        public override string Name { get { return "Hero"; } }
        public override string Item_Text { get { return DPC.ToSexyBackString(); } } // 아이템버튼 우하단 텍스트
        public override string Info_Description { get { return "Damage : " + DPC.ToSexyBackString() + "/tap\n" + "Next : +" + DPC.ToSexyBackString() + "/tap"; } }
        public override BigInteger PriceToNextLv { get { return new BigInteger(10); } }
        // 임시

        public Hero(HeroData data)
        {
            slash = ViewLoader.slash;
            avatar = ViewLoader.hero;
        }

        internal void Update()
        {
        }

        public void Attack()
        {
            if (ISCRITICAL)
            {
                BigInteger totaldamage = DPC * CRIDAMAGE / 100;
                Singleton<MonsterManager>.getInstance().onHitByHero(targetID, totaldamage);
                // 크리티컬 글자 필요 
                slash.GetComponent<Slash>().Play();
                avatar.GetComponent<Animator>().SetTrigger("Attack");
            }
            else
            {
                Singleton<MonsterManager>.getInstance().onHitByHero(targetID, DPC);
                slash.GetComponent<Slash>().Play();
                avatar.GetComponent<Animator>().SetTrigger("Attack");                
            }
        }

        internal void GainExp(BigInteger damage)
        {
            EXP += damage;
        }

        public void IncreaseDPC(BigInteger amount)
        {
            DPC += amount;
        }

        internal BigInteger GetTotalDPC()
        {
            return DPC;
        }
    }
}