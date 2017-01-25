using System;
using System.Collections.Generic;
using UnityEngine;
namespace SexyBackPlayScene
{
    internal class Hero : Statable
    {
        private HeroData baseData;
        private GameObject avatar = ViewLoader.HeroPanel;
        private GameObject sprite = ViewLoader.hero_sprite;

        // 상태값
        public string targetID;

        // 게임플레이에 따라 변하는 데이터.
        private int level;
        private BigInteger baseDpc = new BigInteger(0);
        private BigInteger dpcX = new BigInteger(1); // 곱계수는 X를붙인다.
        int bounsAttackCount = 49; // 보너스 공격스택횟수 
        int attackspeedXH = 100; // speed 와 interval은 역수관계

        // 최종적으로 나가는 값은 모두 대문자이다. 중간과정은 앞에만대문자;
        // data property
        public int MAXATTACKCOUNT { get { return baseData.ATTACKCOUNT + bounsAttackCount; } }
        public string ID { get { return baseData.ID; } }
        public string NAME { get { return baseData.Name; } }
        public int LEVEL { get { return level; } }
        public BigInteger DPC { get { return baseDpc * dpcX; } }
        public string BASEDPC { get { return baseDpc.To5String(); } }
        public string NEXTDPC { get { return baseData.BaseDpcPool[level].ToSexyBackString(); } }
        public BigIntExpression NEXTEXPSTR { get { return baseData.BaseExpPool[level]; } }
        public double ATTACKINTERVAL { get { return baseData.ATTACKINTERVAL * 100 / attackspeedXH; } }   // 공속공식
        public double CRIRATE { get { return baseData.CRIRATE; } }
        public int CRIDAMAGE { get { return baseData.CRIDAMAGE; } }
        public int MOVESPEED { get { return baseData.MOVESPEED; } }
        public double ATTACKSPEED { get { return attackspeedXH / 100; } } // for view action, state

        // member
        public HeroStateMachine StateMachine;
        public HeroAttackManager AttackManager;

        // flag property
        private bool JudgeCritical { get { return CRIRATE > UnityEngine.Random.Range(0.0f, 1.0f); } }

        public Hero(HeroData data)
        {
            baseData = data;
            AddLevel(1);

            AttackManager = new HeroAttackManager(this);
            StateMachine = new HeroStateMachine(this);

            //
        }
        internal void AddLevel(int amount) // 레벨이 10이면 9까지더해야한다;
        {
            if (level + amount > baseData.MaxLevel)
                return;

            for (int i = level; i < level + amount; i++)
                baseDpc += new BigInteger(baseData.BaseDpcPool[i]);
            level += amount;
        }
        internal void Move(Vector3 step)
        {
            avatar.transform.position += step;
        }
        internal void Warp(Vector3 toPos)
        {
            avatar.transform.position = toPos;
        }

        internal void Update()
        {
            AttackManager.Update();
            StateMachine.Update();
        }

        internal bool Attack(float attackSpeed) // 데미지딜링과 sprite action ctrl,
        {
            if (!AttackManager.CanAttack|| targetID == null)
                return false;

            // Calculate damage
            BigInteger damage;
            bool isCritical = JudgeCritical;
            if (isCritical)
                damage = DPC * CRIDAMAGE / 100;
            else
                damage = DPC;

            // play sprite action
            sprite.GetComponent<Animator>().speed = attackSpeed;
            sprite.GetComponent<Animator>().SetTrigger("Attack");

            // do deal
            TapPoint atkPlan = AttackManager.NextAttackPlan();
            Singleton<MonsterManager>.getInstance().Hit(targetID, atkPlan.EffectPos, damage, isCritical);

            // make attack effect
            Vector3 targetpos = Singleton<MonsterManager>.getInstance().FindPosition(targetID);
            AttackManager.MoveMakePlayEffect(atkPlan, targetpos, isCritical);
            return true;
        }

        public void onTouch(TapPoint pos)
        {
            StateMachine.onTouch(pos);
        }

    }
}
