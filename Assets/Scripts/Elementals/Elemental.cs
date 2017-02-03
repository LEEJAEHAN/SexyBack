using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    public class Elemental // base class of Elementals
    {
        public string ID;
        private ElementalData baseData;
        public string targetID;

        // 변수
        private int level = 0;
        public BigInteger DpsX = new BigInteger(1);
        public double DpsXPer5LV = 1;
        public int attackspeedXH = 100;

        // data property
        public string NAME { get { return baseData.Name; } }
        public int LEVEL { get { return level; } }
        public BigInteger DPS = new BigInteger();
        public BigInteger DAMAGE { get { return (DPS * baseData.AttackIntervalK) / 1000; } } //  dps / attackinterval    // 계산되는값
        public double AttackInterval { get { return baseData.AttackIntervalK / (double)(10 * attackspeedXH); } }
        public BigInteger NEXTEXP = new BigInteger();
        public BigInteger BASEDPS { get { return level * baseData.BaseDps; } } // BaseDps* level 값.               // 계산되는값
        public BigInteger NEXTDPS { get { return baseData.BaseDps; } }

        // for projectile action;
        private Transform ElementalArea; // avatar
        private Projectile CurrentProjectile;
        private GameObject ProjectilePrefab;
        private double AttackTimer;

        // status property
        private bool NoProjectile { get { return CurrentProjectile == null; } }

        //change event sender
        public delegate void ElementalChangeEvent_Handler(Elemental sender);
        public event ElementalChangeEvent_Handler Action_ElementalChange;// = delegate (object sender) { };

        public Elemental(ElementalData data, Transform area)
        {
            ID = data.ID;
            baseData = data;
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

        internal void LevelUp(int amount)
        {
            level += amount;
            CalDPS();
            CalEXP();
            Action_ElementalChange(this);
        }
        private void CalEXP()
        {
            NEXTEXP = BigInteger.PowerByGrowth(baseData.BaseExp, level, baseData.GrowthRate);
        }
        void CalDPS()
        {
            DPS = level * DpsX * BigInteger.PowerByGrowth(baseData.BaseDps, (level / 5), DpsXPer5LV);
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