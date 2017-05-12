using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

namespace SexyBackPlayScene
{
    [Serializable]
    internal class Hero : IStateOwner, IDisposable, ISerializable
    {
        // id
        readonly string ID;
        public readonly string Name;
        public string GetID { get { return ID; } }

        // manager
        public HeroStateMachine StateMachine;
        public Animator Animator;
        public HeroAttackManager AttackManager;
        // view
        private GameObject avatar;

        // 계산되는값.
        public BigInteger PRICE = new BigInteger();
        public BigInteger DPC = new BigInteger();
        public BigInteger DPCTick = new BigInteger();

        // 변수
        public int AttackCount; // 누를시 바로 소모된다.
        public string targetID;
        // public int buffduration

        // 스텟
        public int LEVEL;
        BigInteger dpcX = new BigInteger();
        public int DpcIncreaseXH;
        
        public int MAXATTACKCOUNT;
        public double ATTACKINTERVAL;
        public double MOVESPEED;
        public double ATTACKSPEED; // for view action, state
        public double CRIRATE;
        public int CRIDAMAGEXH;
        public double BaseDmg;
        readonly int BaseLevel;
        readonly int BasePrice;
        readonly double AttackInterval;
        readonly float MoveSpeed;

        public string CurrentState { get { return StateMachine.currStateID; } }
        // function property
        private bool JudgeCritical { get { return CRIRATE > UnityEngine.Random.Range(0.0f, 1.0f); } }


        public delegate void HeroChange_Event(Hero hero);
        public event HeroChange_Event Action_Change = delegate { };

        public delegate void DistanceChange_Event(double distance);
        public event DistanceChange_Event Action_DistanceChange = delegate { };

        public Hero(HeroData data)
        {
            ID = data.ID;
            Name = data.Name;
            BasePrice = data.BasePrice;
            BaseLevel = data.BaseLevel;
            AttackInterval = data.AttackInterval;
            MoveSpeed = data.MoveSpeed;
            BaseDmg = data.BaseDmg;

            avatar = GameObject.Find("HeroPanel"); // 동적으로 생성하지 않는다.
            Animator = GameObject.Find("hero_sprite").GetComponent<Animator>();
            AttackManager = new HeroAttackManager(this);
            StateMachine = new HeroStateMachine(this);
        }
        
        void CalDpc()
        {
            double growth = InstanceStatus.CalGrowthPower(HeroData.GrowthRate, BaseLevel); // 
            double doubleC = 5 * BaseDmg * growth * LEVEL * DpcIncreaseXH / 100;
            BigInteger Coefficient = BigInteger.FromDouble(doubleC);
            DPC = dpcX * Coefficient;
            if (LEVEL > 0)
                DPCTick = DPC / LEVEL;
        }

        private void CalPrice()
        {
            double BasePriceDensity = InstanceStatus.GetTotalDensityPerLevel(BaseLevel + LEVEL);
            // cal price
            double growth = InstanceStatus.CalGrowthPower(ElementalData.GrowthRate, BaseLevel + LEVEL);
            double doubleC = BasePrice * BasePriceDensity * growth;
            PRICE = BigInteger.FromDouble(doubleC);
        }

        internal void SetStat(HeroStat herostat)
        {
            dpcX = herostat.DpcX;
            DpcIncreaseXH = herostat.DpcIncreaseXH;
            ATTACKINTERVAL = AttackInterval * 100 / herostat.AttackSpeedXH;
            MOVESPEED = MoveSpeed * herostat.MovespeedXH / 100;
            MAXATTACKCOUNT = herostat.AttackCapacity;
            ATTACKSPEED = (double)herostat.AttackSpeedXH / 100;
            CRIRATE = (double)herostat.CriticalRateXH / 100;
            CRIDAMAGEXH = herostat.CriticalDamageXH;
            //send event
            CalDpc();
            CalPrice();
            Action_Change(this);
        }

        internal void Enchant(string elementID)
        {
            BaseDmg += Singleton<TableLoader>.getInstance().elementaltable[elementID].BaseDmg;
            CalDpc();
            Action_Change(this);
        }
        internal void LevelUp(int value)
        {
            LEVEL += value;
            CalDpc();
            CalPrice();
            Action_Change(this);
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
            Monster target = Singleton<MonsterManager>.getInstance().GetMonster();
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

            target.Hit(atkPlan.PosInEffectCam, damage, isCritical);

            // make attack effect
            Vector3 targetpos = target.CenterPosition;
            AttackManager.MakeSlashEffect(atkPlan, targetpos, isCritical);
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
