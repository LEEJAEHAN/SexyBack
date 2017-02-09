using System;
using System.Collections.Generic;
using UnityEngine;
namespace SexyBackPlayScene
{

    internal class Hero : IStateOwner, ICanLevelUp
    {
        readonly string ID;
        public readonly string Name;
        public string GetID { get { return ID; } }


        // manager
        public HeroStateMachine StateMachine;
        public Animator Animator;
        public HeroAttackManager AttackManager;

        // init member
        private GameObject avatar;

        // 게임플레이에 따라 변하는 데이터.
        // 상태값
        public string targetID;
        private int level = 0;
        private BigInteger DpcX = new BigInteger(1); // 곱계수는 X를붙인다.
        private int bounsAttackCount = 6; // 보너스 공격스택횟수  6
        private int attackspeedXH = 100; // speed 와 interval은 역수관계
        private int movespeedXH = 1000;

        // 능력치 property;
        // 최종적으로 나가는 값은 모두 대문자이다. 중간과정은 앞에만대문자;
        public BigInteger DPC;
        public int MAXATTACKCOUNT { get { return AttackCount + bounsAttackCount; } }
        public double ATTACKINTERVAL { get { return AttackInterval * 100 / attackspeedXH; } }   // 공속공식
        public double CRIRATE { get { return CriRate; } }
        public int CRIDAMAGE { get { return CriDamage; } }
        public float MOVESPEED { get { return MoveSpeed * movespeedXH / 100; } }
        public double ATTACKSPEED { get { return attackspeedXH / 100; } } // for view action, state

        // member from data
        private readonly BigInteger BaseDpc;
        public readonly BigInteger BaseExp;
        public readonly int AttackCount;
        public readonly double AttackInterval;
        public readonly double CriRate;
        public readonly int CriDamage;
        public readonly float MoveSpeed;
        public readonly double GrowthRate;

        //stateOwner
        public string CurrentState { get { return StateMachine.currStateID; } }

        // function property
        private bool JudgeCritical { get { return CRIRATE > UnityEngine.Random.Range(0.0f, 1.0f); } }

        // ICanLevelUp
        public int LEVEL { get { return level; } }
        public event LevelUp_EventHandler Action_LevelUp = delegate { };
        public BigInteger LevelUpPrice { get { return BigInteger.PowerByGrowth(BaseExp, level, GrowthRate); } }
        public string LevelUpDescription
        {
            get
            {
                string text = "Damage : " + DPC.To5String() + "/tap\n";
                text += "Next : +" + (DpcX * BaseDpc).To5String() + "/tap\n";
                return text;
            }
        }

        public delegate void HeroChange_EventHandler(Hero hero);
        public event HeroChange_EventHandler Action_DamageChange;

        public delegate void DistanceChange_Event(float distance);
        public event DistanceChange_Event Action_DistanceChange;


        public Hero(HeroData data)
        {
            ID = data.ID;
            Name = data.Name;
            BaseDpc = data.BaseDpc;
            BaseExp = data.BaseExp;
            AttackCount = data.AttackCount;
            AttackInterval = data.AttackInterval;
            CriRate = data.CriRate;
            CriDamage = data.CriDamage;
            MoveSpeed = data.MoveSpeed;
            GrowthRate = data.GrowthRate;

            avatar = ViewLoader.HeroPanel; // 동적으로 생성하지않는다.
            Animator = ViewLoader.hero_sprite.GetComponent<Animator>();
            AttackManager = new HeroAttackManager(this);
            StateMachine = new HeroStateMachine(this);
        }

        public void LevelUp(int amount)
        {
            level += amount;
            CalDPC();

            Action_LevelUp(this);
            Action_DamageChange(this);
        }

        void CalDPC()
        {
            //DPC = BigInteger.PowerByGrowth(BaseDpc * DpcX, level, GrowthRate);
            DPC = level * BaseDpc * DpcX;
        }

        internal void Move(Vector3 step)
        {
            avatar.transform.position += step;
        }
        internal void Warp(Vector3 toPos)
        {
            avatar.transform.position = toPos;
        }
        internal void FakeMove(float amount)
        {
            Action_DistanceChange(amount);
        }

        internal void Update()
        {
            AttackManager.Update();
            StateMachine.Update();
        }

        internal bool Attack(float attackSpeed) // 데미지딜링과 sprite action ctrl,
        {
            if (!AttackManager.CanAttack || targetID == null)
                return false;
            Monster target = Singleton<MonsterManager>.getInstance().GetMonster(targetID);
            if (target == null)
                return false;

            // Calculate damage
            BigInteger damage;
            bool isCritical = JudgeCritical;
            if (isCritical)
                damage = DPC * CRIDAMAGE / 100;
            else
                damage = DPC;

            // play sprite action
            //Animator.
            Animator.speed = attackSpeed;
            Animator.SetTrigger("Attack");

            // do deal
            TapPoint atkPlan = AttackManager.NextAttackPlan();
            target.Hit(atkPlan.EffectPos, damage, isCritical);

            // make attack effect
            Vector3 targetpos = target.CenterPosition;
            AttackManager.MoveMakePlayEffect(atkPlan, targetpos, isCritical);
            return true;
        }
        public void onTargetStateChange(string monsterid, string stateID)
        {
            if (stateID == "Ready")
            {
                targetID = monsterid;
                return;
            }
            else if (stateID == "Death")
            {
                ChangeState("Move");
            }
            targetID = null;
        }

        public void ChangeState(string stateid)
        {
            StateMachine.ChangeState(stateid);
        }


        internal bool Upgrade(Bonus bonus)
        {
            bool result = false;
            switch (bonus.attribute)
            {
                case "LearnSkill":
                    {
                        result =  Singleton<ElementalManager>.getInstance().SummonNewElemental(bonus.strvalue);
                        break;
                    }
                default:
                    {
                        sexybacklog.Error("업그레이드가능한 attribute가 없습니다.");
                        result = false;
                        break;
                    }
            }
            if (result)
                Action_DamageChange(this);
            return result;
        }



    }
}
