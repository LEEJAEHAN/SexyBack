using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class Elemental : ICanLevelUp// base class of Elementals
    {
        public string GetID { get { return ID; } }
        public string targetID;

        // 변수
        private int level = 0;
        public BigInteger DpsX = new BigInteger(1);
        public int attackspeedXH = 100;
        public BigInteger DPS = new BigInteger();
        public BigInteger DAMAGE { get { return (DPS * AttackIntervalK) / (10 * attackspeedXH); } } //  dps * attackinterval
        public double AttackInterval { get { return AttackIntervalK / (double)(10 * attackspeedXH); } } // (attackinterval1k / 1000) * ( 100 / attackspeed1h ) 

        // 고정수
        readonly string ID;
        readonly int DpsShiftDigit;
        readonly string NAME;
        readonly int AttackIntervalK;
        readonly BigInteger BaseDps;
        readonly BigInteger BaseExp;
        readonly double GrowthRate;

        // for projectile action;
        private Transform ElementalArea; // avatar
        private Projectile CurrentProjectile;
        private GameObject ProjectilePrefab;
        private double AttackTimer;

        // ICanLevelUp
        public int LEVEL { get { return level; } }
        public event LevelUp_EventHandler Action_LevelUp = delegate {};
        public BigInteger LevelUpPrice { get { return BigInteger.PowerByGrowth(BaseExp, level, GrowthRate); } }
        public string LevelUpDescription{ get {
                string text = "Damage : " + DPS.To5String() + "/sec\n";
                text += "Next : +" + (DpsX * BaseDps / DpsShiftDigit).To5String() + "/sec\n";
                return text;
            } }

        public delegate void ElementalChange_EventHandler(Elemental elemental);
        public event ElementalChange_EventHandler Action_DamageChange;

        // status property
        private bool NoProjectile { get { return CurrentProjectile == null; } }


        //change event sender
        public delegate void ElementalChangeEvent_Handler(Elemental sender);

        public Elemental(ElementalData data, Transform area)
        {
            NAME = data.Name;
            AttackIntervalK = data.AttackIntervalK;
            BaseDps = data.BaseDps;
            DpsShiftDigit = data.FloatDigit;
            BaseExp = data.BaseExp;
            GrowthRate = data.GrowthRate;

            ID = data.ID;
            ElementalArea = area;
            ProjectilePrefab = Resources.Load(ElementalData.ProjectilePrefabName(ID)) as GameObject;
        }

        internal void CreateProjectile()
        {
            Vector3 genPosition = RandomRangeVector3(ElementalArea.position, ElementalArea.localScale / 2);
            CurrentProjectile = new Projectile(this, ProjectilePrefab, genPosition);
        }

        internal void onDestroyProjectile()
        {
            CurrentProjectile = null;
        }

        public void Shoot(Vector3 target)
        {
            if (CurrentProjectile.Shoot(target, 0.25f))
                AttackTimer = 0; // 정상적으로 발사 완료 후 타이머리셋
        }

        internal bool Upgrade(Bonus bonus)
        {
            bool result = true;
            switch (bonus.attribute)
            {
                case "DpsX":
                    {
                        DpsX *= bonus.value;
                        break;
                    }
                case "attackspeedXH":
                    {
                        attackspeedXH += bonus.value;
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
            {
                CalDPS(); // calexp는 일단안넣는다.
                Action_DamageChange(this);
            }
            return result;
        }
        public void LevelUp(int amount)
        {
            level += amount;
            CalDPS();
            Action_LevelUp(this);
            Action_DamageChange(this);
        }
        void CalDPS()
        {
            if (DpsShiftDigit == 1)
                DPS = level * DpsX * BaseDps;
            else
                DPS = level * DpsX * BaseDps / DpsShiftDigit;
            //            DPS = level * DpsX * BigInteger.PowerByGrowth(baseData.BaseDps, (level / 5), DpsXPer5LV);
        }

        // TODO : 여기도 언젠간 statemachine작업을 해야할듯 ㅠㅠ
        internal void Update()
        {
            AttackTimer += Time.deltaTime;

            // 만들어진다.
            if (AttackTimer > AttackInterval - 1 && NoProjectile)
            {
                CreateProjectile();
            }
            if (AttackTimer > AttackInterval - 1 && !NoProjectile)
            {
                // 대기중
            }

            if (AttackTimer > AttackInterval)
            {
                if (targetID != null)
                {
                    Vector3 destination = calDestination(targetID);
                    Shoot(destination);
                }
                else if (targetID == null)
                {
                    //타겟이생길떄까지 대기한다.
                }
            }
        }

        public void onTargetStateChange(string monsterID, string stateID)
        {
            if (stateID == "Ready")
                this.targetID = monsterID;
            else
                targetID = null;
        }

        private Vector3 calDestination(string targetID)
        {
            Monster target = Singleton<MonsterManager>.getInstance().GetMonster(targetID);
            Vector3 center = target.CenterPosition;
            Vector3 extend = target.Size / 2;
            Vector3 dest = RandomRangeVector3(center, extend);
            return dest;
        }
        private Vector3 RandomRangeVector3(Vector3 center, Vector3 extend)
        {
            Vector3 min = center - extend;
            Vector3 max = center + extend;

            return new Vector3(UnityEngine.Random.Range(min.x, max.x),
                UnityEngine.Random.Range(min.y, max.y),
                UnityEngine.Random.Range(min.z, max.z));
        }
        public virtual void Cast()
        {

        }

    }
}