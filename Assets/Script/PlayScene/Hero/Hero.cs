﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

namespace SexyBackPlayScene
{
    [Serializable]
    internal class Hero : IStateOwner, IDisposable, ISerializable
    {
        public string GetID { get { return baseData.ID; } }
        public readonly HeroData baseData;
        public HeroStateMachine StateMachine;
        public Animator Animator;
        public HeroAttackManager AttackManager;
        private GameObject avatar;

        // 변수
        public int AttackCount; // 누를시 바로 소모된다.
        public string TargetID;
        // public int buffduration

        // 최종 스텟
        public int LEVEL;
        public double BaseDmg;
        BigInteger DpcX = new BigInteger();
        public int DpcXH;
        public int MaxAttackCount;
        public int AttackSpeedXH;
        public double AttackInterval;   // 밑에것과 역수관계
        public double AttackSpeed; // for view action, state
        public double MoveSpeed;
        public int CriRateXK;
        public int CriDamageXH;
        int BuffCoef = 1;

        // 계산되는값.
        public BigInteger PRICE = new BigInteger();
        public BigInteger DPC = new BigInteger();
        public BigInteger DPCTick = new BigInteger();

        public string CurrentState { get { return StateMachine.currStateID; } }
        private bool JudgeCritical { get { return CriRateXK > UnityEngine.Random.Range(0, 1000); } }

        public delegate void HeroChange_Event(Hero hero);
        public event HeroChange_Event Action_Change = delegate { };

        public delegate void DistanceChange_Event(double distance);
        public event DistanceChange_Event Action_DistanceChange = delegate { };

        bool RefreshStat = true;

        public Hero(HeroData data)
        {
            baseData = data;
            BaseDmg = data.BaseDmg;

            avatar = GameObject.Find("HeroPanel"); // 동적으로 생성하지 않는다.
            Animator = GameObject.Find("hero_sprite").GetComponent<Animator>();
            AttackManager = new HeroAttackManager(this);
            StateMachine = new HeroStateMachine(this);
        }
        internal void onStatChange()
        {
            RefreshStat = true;
        }
        internal void Enchant(string elementID)
        {
            BaseDmg += Singleton<TableLoader>.getInstance().elementaltable[elementID].BaseDmg;
            RefreshStat = true;
        }
        internal void LevelUp(int value)
        {
            LEVEL += value;
            RefreshStat = true;
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

        internal void Refresh()
        {
            CalStat();
            CalDpc();
            CalPrice();
            Action_Change(this);    // send event for view update
        }

        public void CalStat()
        {
            HeroStat herostat = Singleton<PlayerStatus>.getInstance().GetHeroStat;
            BaseStat basestat = Singleton<PlayerStatus>.getInstance().GetBaseStat;

            DpcX = herostat.DpcX;
            DpcXH = (100 + herostat.DpcIncreaseXH) * (100 + basestat.Str) / 100; // 곱하기 힘
            AttackSpeedXH = (100 + herostat.AttackSpeedXH) * (200 + basestat.Spd) / 200;
            CriRateXK = baseData.BaseSkillRateXK * (100 + herostat.CriticalRateXH) / 100;
            CriDamageXH = baseData.BaseSkillDamageXH * (100 + herostat.CriticalDamageXH) / 100;

            AttackSpeed = (double)AttackSpeedXH / 100;
            AttackInterval = baseData.AttackInterval * 100 / AttackSpeedXH;
            MoveSpeed = baseData.MoveSpeed * (100 + herostat.MovespeedXH) / 100;
            MaxAttackCount = herostat.AttackCapacity;
        }

        void CalDpc()
        {
            double growth = InstanceStatus.CalGrowthPower(HeroData.GrowthRate, baseData.BaseLevel); // 
            double doubleC = 5 * BaseDmg * growth * LEVEL * DpcXH * BuffCoef/ 100;
            BigInteger Coefficient = BigInteger.FromDouble(doubleC);
            DPC = DpcX * Coefficient;
            if (LEVEL > 0)
                DPCTick = DPC / LEVEL;
        }

        internal void Buff(bool on, int xtimes)
        {
            if (on)
                BuffCoef = xtimes;
            else
                BuffCoef = 1;
            CalDpc();
            Action_Change(this);
        }

        private void CalPrice()
        {
            double BasePriceDensity = InstanceStatus.GetTotalDensityPerLevel(baseData.BaseLevel + LEVEL);
            // cal price
            double growth = InstanceStatus.CalGrowthPower(ElementalData.GrowthRate, baseData.BaseLevel + LEVEL);
            double doubleC = baseData.BasePrice * BasePriceDensity * growth;
            PRICE = BigInteger.FromDouble(doubleC);
        }

        internal void Update()
        {
            if(RefreshStat)
            {
                Refresh();
                RefreshStat = false;
            }
            AttackManager.Update();
            StateMachine.Update();
        }

        internal bool Attack(float attackSpeed) // 데미지딜링과 sprite action ctrl,
        {
            if (!AttackManager.CanAttack || TargetID == null)
                return false;
            Monster target = Singleton<MonsterManager>.getInstance().GetMonster();
            if (target == null)
                return false;

            // Calculate damage
            BigInteger damage;
            bool isCritical = JudgeCritical;
            if (isCritical)
                damage = DPC * CriDamageXH / 100;
            else
                damage = DPC;

            // play sprite action
            //Animator.
            Animator.speed = attackSpeed;
            Animator.SetTrigger("Attack");

            // do deal
            TapPoint atkPlan = AttackManager.NextAttackPlan();

            target.Hit(atkPlan.PosInEffectCam, damage, isCritical);

            // make attack effect
            Vector3 targetpos = target.CenterPosition;
            AttackManager.MakeSlashEffect(atkPlan, targetpos, isCritical);
            return true;
        }
        public void onTargetStateChange(string monsterid, string stateID)
        {
            if (stateID == "Ready")
                TargetID = monsterid;
            else
                TargetID = null;
        }
        public void ChangeState(string stateid)
        {
            StateMachine.ChangeState(stateid);
        }

        //// Init and Dispose
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("AttackCount", AttackCount);
        }
        public Hero(SerializationInfo info, StreamingContext context)
        {
            AttackCount = (int)info.GetValue("AttackCount", typeof(int));
        }
        public void Dispose()
        {
            // manager
            StateMachine = null;
            AttackManager.Dispose();
            AttackManager = null;
            avatar = null;
            Action_Change = null;
            Action_DistanceChange = null;
        }
        ~Hero()
        {
            sexybacklog.Console("Hero 소멸");
        }

    }
}
