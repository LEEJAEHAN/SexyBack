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
        BigInteger DpsX = new BigInteger(1);
        public BigInteger DPS = new BigInteger();
        public BigInteger DAMAGE = new BigInteger();//  dps * attackinterval
        public double ATTACKINTERVAL = 1000; // (attackinterval1k / 1000) * ( 100 / attackspeed1h ) 

        // 고정수
        readonly string ID;
        readonly int DpsShiftDigit;
        readonly string NAME;
        readonly int BaseAttackInterval1K;
        readonly BigInteger BaseDps;
        readonly BigInteger BaseExp;
        readonly double GrowthRate;

        // for projectile action;
        private Transform ElementalArea; // avatar
        private Projectile CurrentProjectile;
        private GameObject ProjectilePrefab;
        private double AttackTimer;
        private bool NoProjectile { get { return CurrentProjectile == null; } }

        // ICanLevelUp
        public int LEVEL { get { return level; } }
        public event LevelUp_EventHandler Action_LevelUpInfoChange = delegate { };
        public BigInteger LevelUpPrice { get { return BigInteger.PowerByGrowth(BaseExp, level, GrowthRate); } }
        public string LevelUpDescription
        {
            get
            {
                string text = "Damage : " + DPS.To5String() + "/sec\n";
                text += "Next : +" + (DpsX * BaseDps / DpsShiftDigit).To5String() + "/sec\n";
                return text;
            }
        }
        // event
        public delegate void ElementalChange_EventHandler(Elemental elemental);
        public event ElementalChange_EventHandler Action_DamageChange = delegate { };

        public Elemental(ElementalData data, Transform area)
        {
            ID = data.ID;
            NAME = data.Name;
            BaseAttackInterval1K = data.AttackIntervalK;
            BaseDps = data.BaseDps;
            DpsShiftDigit = data.FloatDigit;
            BaseExp = data.BaseExp;
            GrowthRate = data.GrowthRate;

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
            if (CurrentProjectile.Shoot(target, 1f))
                AttackTimer = 0; // 정상적으로 발사 완료 후 타이머리셋
        }

        public void LevelUp(int amount)
        {
            level += amount;
            CalDPS();
            Action_LevelUpInfoChange(this);
            Action_DamageChange(this);
        }
        internal void SetDamageX(BigInteger dpsx) // total
        {
            DpsX = dpsx;
            CalDPS();
            Action_LevelUpInfoChange(this);
            Action_DamageChange(this);
        }
        internal void SetStat(ElementalUpgradeStat elementalstat)
        {
            ATTACKINTERVAL = BaseAttackInterval1K / (double)(10 * elementalstat.ElementalAttackSpeedXH); // (attackinterval1k / 1000) * ( 100 / attackspeed1h ) 
        }

        void CalDPS()
        {
            if (DpsShiftDigit == 1)
                DPS = level * DpsX * BaseDps;
            else
                DPS = level * DpsX * BaseDps / DpsShiftDigit;
            DAMAGE = (DPS * BaseAttackInterval1K / 1000); //  dps * attackinterval
            //            DPS = level * DpsX * BigInteger.PowerByGrowth(baseData.BaseDps, (level / 5), DpsXPer5LV);
        }

        // TODO : 여기도 언젠간 statemachine작업을 해야할듯 ㅠㅠ
        internal void Update()
        {
            AttackTimer += Time.deltaTime;

            // 만들어진다.
            if (AttackTimer > ATTACKINTERVAL - 1 && NoProjectile)
            {
                CreateProjectile();
            }
            if (AttackTimer > ATTACKINTERVAL - 1 && !NoProjectile)
            {
                // 대기중
            }

            if (AttackTimer > ATTACKINTERVAL)
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
    }
}