using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

namespace SexyBackPlayScene
{
    [Serializable]
    internal class Hero : IStateOwner, IDisposable, ISerializable
    {
        ~Hero()
        {
            sexybacklog.Console("Hero 소멸");
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
        BigInteger dpcX = new BigInteger();
        public int DpcIncreaseXH;

        // 기타변수
        public int LEVEL = 0;
        public BigInteger PRICE = new BigInteger();
        public BigInteger DPC = new BigInteger();
        public BigInteger DPCTick = new BigInteger();

        public int MAXATTACKCOUNT;
        public double ATTACKINTERVAL;
        public double MOVESPEED;
        public double ATTACKSPEED; // for view action, state
        public double CRIRATE;
        public int CRIDAMAGEXH;

        // basestat
        readonly double BaseDmgDensity;
        readonly string Enchant;
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
            BaseDmgDensity = data.BaseDmgDensity;
            BasePrice = data.BasePrice;
            BaseLevel = data.BaseLevel;
            AttackInterval = data.AttackInterval;
            MoveSpeed = data.MoveSpeed;

            avatar = GameObject.Find("HeroPanel"); // 동적으로 생성하지 않는다.
            Animator = GameObject.Find("hero_sprite").GetComponent<Animator>();
            AttackManager = new HeroAttackManager(this);
            StateMachine = new HeroStateMachine(this);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("level", LEVEL);
            info.AddValue("Enchant", Enchant);
        }
        public Hero(SerializationInfo info, StreamingContext context)
        {
            LEVEL = (int)info.GetValue("level", typeof(int));
            Enchant = (string)info.GetValue("Enchant", typeof(string));
        }

        void CalDpc()
        {
            double growth = StatManager.Growth(HeroData.GrowthRate, BaseLevel); // 
            double doubleC = 5 * BaseDmgDensity * growth * LEVEL * DpcIncreaseXH / 100;
            BigInteger Coefficient = BigInteger.FromDouble(doubleC);
            DPC = dpcX * Coefficient;
            DPCTick = DPC / LEVEL;
        }
        public void LevelUp(int amount)
        {
            LEVEL += amount;
            CalDpc();
            CalPrice();
            Action_Change(this);
        }

        private void CalPrice()
        {
            double BasePriceDensity = StatManager.GetTotalDensityPerLevel(BaseLevel + LEVEL);
            // cal price
            double growth = StatManager.Growth(ElementalData.GrowthRate, BaseLevel + LEVEL);
            double doubleC = BasePrice * BasePriceDensity * growth;
            PRICE = BigInteger.FromDouble(doubleC);
        }

        internal void SetStat(HeroStat herostat, bool CalDamage)
        {
            dpcX = herostat.DpcX;
            DpcIncreaseXH = herostat.DpcIncreaseXH;
            ATTACKINTERVAL = AttackInterval * 100 / herostat.AttackSpeedXH;
            MOVESPEED = MoveSpeed * herostat.MovespeedXH / 100;
            MAXATTACKCOUNT = herostat.AttackCount;
            ATTACKSPEED = (double)herostat.AttackSpeedXH / 100;
            CRIRATE = (double)herostat.CriticalRateXH / 100;
            CRIDAMAGEXH = herostat.CriticalDamageXH;
            //send event
            if (CalDamage)
                CalDpc();
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

    }
}
