﻿using System;
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

        // 변수
        private int level = 0;
        BigInteger DpcX = new BigInteger(1); // 곱계수는 X를붙인다.
        public BigInteger DPC = new BigInteger();
        public int MAXATTACKCOUNT;
        public double ATTACKINTERVAL;
        public double CRIRATE;
        public int CRIDAMAGE;
        public double MOVESPEED;
        public double ATTACKSPEED; // for view action, state

        // basestat
        readonly BigInteger BaseDpc;
        readonly BigInteger BaseExp;
        readonly int AttackCount;
        readonly double AttackInterval;
        readonly double CriRate;
        readonly int CriDamage;
        readonly float MoveSpeed;
        readonly double GrowthRate;

        //stateOwner
        public string CurrentState { get { return StateMachine.currStateID; } }

        // function property
        private bool JudgeCritical { get { return CRIRATE > UnityEngine.Random.Range(0.0f, 1.0f); } }

        // ICanLevelUp
        public int LEVEL { get { return level; } }
        public event LevelUp_EventHandler Action_LevelUpInfoChange = delegate { };
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
        public event HeroChange_EventHandler Action_DamageChange = delegate { };

        public delegate void DistanceChange_Event(double distance);
        public event DistanceChange_Event Action_DistanceChange = delegate { };


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

        internal void SetDamageX(BigInteger dpcx)
        {
            DpcX = dpcx;
            CalDPC();
            Action_LevelUpInfoChange(this);
            Action_DamageChange(this);
        }

        internal void SetStat(HeroUpgradeStat herostat)
        {
            MAXATTACKCOUNT = AttackCount + herostat.BounsAttackCount;
            ATTACKINTERVAL = AttackInterval * 100 / herostat.HeroAttackspeedXH;
            MOVESPEED = MoveSpeed * herostat.MovespeedXH / 100;
            ATTACKSPEED = herostat.HeroAttackspeedXH / 100; 
            CRIRATE = CriRate + herostat.CriticalRate;
            CRIDAMAGE = CriDamage + herostat.CriticalDamage;
            //send event
        }

        public void LevelUp(int amount)
        {
            level += amount;
            CalDPC();
            Action_LevelUpInfoChange(this);
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

    }
}
