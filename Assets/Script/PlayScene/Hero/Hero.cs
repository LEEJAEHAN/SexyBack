using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

namespace SexyBackPlayScene
{
    [Serializable]
    internal class Hero : ICanLevelUp, IStateOwner, IDisposable, ISerializable
    {
        public string GetID { get { return baseData.ID; } }
        public int GetLevel { get { return OriginalLevel + BonusLevel; } }
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
        public int OriginalLevel;
        public int BonusLevel;
        public double DamageDensity;
        public double EnchantDmgDensity;
        BigInteger DpcX = new BigInteger();
        public int DpcXH;
        public int MaxAttackCount;
        public int AttackSpeedXH;
        public double AttackInterval;   // 밑에것과 역수관계
        public double AttackSpeed; // for view action, state
        public double MoveSpeed;
        public int CriRateXK;
        public int CriDamageXH;
        public int BuffCoef = 1;

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
            EnchantDmgDensity += Singleton<TableLoader>.getInstance().elementaltable[elementID].BaseDmgDensity;
            RefreshStat = true;
        }
        internal void LevelUp(int value)
        {
            OriginalLevel += value;
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
            // 레벨업패널에 출력되는정보
            DamageDensity = baseData.BaseDmgDensity + herostat.BaseDensityAdd + EnchantDmgDensity;
            DpcXH = (100 + herostat.DpcIncreaseXH) * (100 + basestat.Str) / 100; // 곱하기 힘
            AttackSpeedXH = (100 + herostat.AttackSpeedXH) * (200 + basestat.Spd) / 200;
            CriRateXK = baseData.BaseSkillRateXK * (100 + herostat.CriticalRateXH) / 100;
            CriDamageXH = baseData.BaseSkillDamageXH * (100 + herostat.CriticalDamageXH) / 100;
            BonusLevel = herostat.BonusLevel;
            // 아닌정보
            AttackSpeed = (double)AttackSpeedXH / 100;
            AttackInterval = baseData.AttackInterval * 100 / AttackSpeedXH;
            MoveSpeed = baseData.MoveSpeed * (100 + herostat.MovespeedXH) / 100;
            MaxAttackCount = herostat.AttackCapacity;
        }

        void CalDpc()
        {
            // growth == 1 이고 globalgrowth 역시 1이므로 생략한다.
            double EtcCoef = DpcXH * BuffCoef / 100; // 매우작은수.
            double DmgWithoutResearch = (baseData.BaseDamage * baseData.AttackInterval) * DamageDensity * GetLevel * EtcCoef;

            BigInteger IntDmg = BigInteger.FromDouble(DmgWithoutResearch);
            DPC = DpcX * IntDmg;
            if (GetLevel > 0)
                DPCTick = DPC / GetLevel;
        }

        internal void Buff(bool on, int xtimes)
        {
            if (on)
                BuffCoef = xtimes;
            else
                BuffCoef = 1;
            RefreshStat = true;
        }

        private void CalPrice()
        {
            double BasePriceDensity = InstanceStatus.GetTotalDensityPerLevel(baseData.BaseLevel + OriginalLevel); //baseData.BaseDmgDensity;
            // cal price
            double growth = InstanceStatus.CalInstanceGrowth(baseData.BaseLevel + OriginalLevel);
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
