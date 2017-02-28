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
        public string targetID;

        // manager
        public HeroStateMachine StateMachine;
        public Animator Animator;
        public HeroAttackManager AttackManager;

        // init member
        private GameObject avatar;

        // 데미지변수
        int level = 0;
        BigInteger dpcX = new BigInteger();
        int dpcIncreaseXH;

        // 기타변수
        public BigInteger DPC = new BigInteger();
        public int MAXATTACKCOUNT;
        public double ATTACKINTERVAL;
        public double CRIRATE;
        public int CRIDAMAGEXH;
        public double MOVESPEED;
        public double ATTACKSPEED; // for view action, state

        // basestat
        readonly BigInteger BaseDpc;
        readonly BigInteger BaseExp;
        readonly double AttackInterval;
        readonly float MoveSpeed;
        readonly double GrowthRate;

        //stateOwner
        public string CurrentState { get { return StateMachine.currStateID; } }

        // function property
        private bool JudgeCritical { get { return CRIRATE > UnityEngine.Random.Range(0.0f, 1.0f); } }

        // ICanLevelUp
        public int LEVEL { get { return level; } }
        public BigInteger LevelUpPrice { get { return BigInteger.PowerByGrowth(BaseExp, level, GrowthRate); } }
        public string LevelUpDamageText { get { return DPC.To5String() + " /Tap"; } }
        public string LevelUpNextText { get { return (BaseDpc * dpcX * dpcIncreaseXH / 100).To5String() + " /Tap"; } }
        public event LevelUp_EventHandler Action_LevelUpInfoChange = delegate { };

        public delegate void HeroChange_EventHandler(Hero hero);
        public event HeroChange_EventHandler Action_DamageChange = delegate { };

        public delegate void DistanceChange_Event(double distance);
        public event DistanceChange_Event Action_DistanceChange = delegate { };

        public Hero(HeroData data, HeroStat stat)
        {
            ID = data.ID;
            Name = data.Name;
            BaseDpc = data.BaseDpc;
            BaseExp = data.BaseExp;
            GrowthRate = data.GrowthRate;
            AttackInterval = data.AttackInterval;
            MoveSpeed = data.MoveSpeed;

            SetStat(stat);

            avatar = ViewLoader.HeroPanel; // 동적으로 생성하지않는다.
            Animator = ViewLoader.hero_sprite.GetComponent<Animator>();
            AttackManager = new HeroAttackManager(this);
            StateMachine = new HeroStateMachine(this);
        }
        void CalDpc()
        {
            DPC = level * BaseDpc * dpcX * dpcIncreaseXH / 100;
            Action_LevelUpInfoChange(this);
            Action_DamageChange(this);
        }
        public void LevelUp(int amount)
        {
            level += amount;
            CalDpc();
        }
        internal void SetDamageStat(HeroStat herostat)
        {
            dpcX = herostat.DpcX;
            dpcIncreaseXH = herostat.DpcIncreaseXH;
            CalDpc();
        }
        internal void SetStat(HeroStat herostat)
        {
            dpcX = herostat.DpcX;
            dpcIncreaseXH = herostat.DpcIncreaseXH;
            ATTACKINTERVAL = AttackInterval * 100 / herostat.AttackSpeedXH;
            MOVESPEED = MoveSpeed * herostat.MovespeedXH / 100;
            MAXATTACKCOUNT = herostat.AttackCount;
            ATTACKSPEED = (double)herostat.AttackSpeedXH / 100;
            CRIRATE = (double)herostat.CriticalRateXH / 100;
            CRIDAMAGEXH = herostat.CriticalDamageXH;
            //send event
        }

        internal void Move(Vector3 step)
        {
            avatar.transform.position += step;
        }
        internal void Warp(Vector3 toPos)
        {
            avatar.transform.position = toPos;
        }
        internal void FakeMove(double amount)
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
                damage = DPC * CRIDAMAGEXH / 100;
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
                targetID = monsterid;
            else
                targetID = null;
        }

        public void ChangeState(string stateid)
        {
            StateMachine.ChangeState(stateid);
        }

    }
}
