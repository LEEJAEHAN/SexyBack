using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    public class Elemental // base class of Elementals
    {
        public string ID;
        private ElementalData baseData;
        public Monster target;
        private int level;
        public BigInteger DpsX = new BigInteger(1);

        // data property
        public string NAME { get { return baseData.Name; } }
        public int LEVEL { get { return level; } }
        public BigInteger DPS { get { return level * baseData.BaseDps * DpsX; } }
        public BigInteger BASEDPS { get { return level * baseData.BaseDps; } } // BaseDps* level 값.               // 계산되는값
        public BigInteger NEXTDPS { get { return baseData.BaseDps; } }
        public BigInteger DAMAGE { get { return (DPS * baseData.AttackIntervalK) / 1000; } } //  dps / attackinterval    // 계산되는값
        public double AttackInterval { get { return (double)baseData.AttackIntervalK / (double)1000; } }

        // for projectile action;
        private Transform ElementalArea; // avatar
        private Projectile CurrentProjectile;
        private GameObject ProjectilePrefab;
        private double AttackTimer;

        // status property
        private bool NoProjectile { get { return CurrentProjectile == null; } }

        public Elemental(ElementalData data, Transform area)
        {
            ID = data.ID;
            baseData = data;
            ElementalArea = area;
            ProjectilePrefab = Resources.Load(ElementalData.ProjectilePrefabName(ID)) as GameObject;

            LevelUp(1);
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

        private Vector3 RandomRangeVector3(Vector3 center, Vector3 extend)
        {
            Vector3 min = center - extend;
            Vector3 max = center + extend;

            return new Vector3(UnityEngine.Random.Range(min.x, max.x),
                UnityEngine.Random.Range(min.y, max.y),
                UnityEngine.Random.Range(min.z, max.z));
        }

        public void Shoot(Vector3 target)
        {
            if (CurrentProjectile.Shoot(target))
                AttackTimer = 0; // 정상적으로 발사 완료 후 타이머리셋
        }

        internal void LevelUp(int toLevel)
        {
            level += toLevel;
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
                if (target != null)
                {
                    Vector3 destination = calDestination(target.avatarCollision);
                    Shoot(destination);
                }
                else if (target == null)
                {
                    //타겟이생길떄까지 대기한다.
                }
            }
        }

        private Vector3 calDestination(BoxCollider monsterCollision)
        {
            Vector3 center = monsterCollision.transform.position + monsterCollision.center;
            Vector3 extend = (monsterCollision.size / 2);
            Vector3 dest = RandomRangeVector3(center, extend);
            return dest;
        }

        public virtual void Cast()
        {

        }

        public BigInteger NEXTEXP
        {
            get
            {
                double growth = Mathf.Pow(baseData.GrowthRate, level);
                int intgrowth = 0;
                BigInteger result;

                if ((int)growth < int.MaxValue / 10000)
                {
                    intgrowth = (int)(growth * 10000);
                    result = baseData.BaseExp * intgrowth / 10000;
                }
                else
                {
                    intgrowth = (int)growth;
                    result = baseData.BaseExp * intgrowth;
                }
                return result;
            }
        }
    }
}