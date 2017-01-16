using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    public class Elemental : CanLevelUp // base class of Elementals
    {
        ElementalData ElementalData;
        public Monster target;

        // public property
        public override string ID { get { return ElementalData.ID; } }
        public override string Name { get { return ElementalData.Name; } }

        public BigInteger Dps { get { return LevelCount * ElementalData.BaseDps; } } // BaseDps* level 값.               // 계산되는값
        public BigInteger Damage { get { return (Dps * ElementalData.AttackIntervalK) / 1000; } } //  dps / attackinterval    // 계산되는값
        public double AttackInterval { get { return (double)ElementalData.AttackIntervalK / (double)1000; } }

        public override string Item_Text { get { return Dps.ToSexyBackString(); } } // 아이템버튼 우하단 텍스트
        public override string Info_Description { get { return "Damage : " + Dps.ToSexyBackString() + "/sec\n" + "Next : +" + ElementalData.BaseDps.ToSexyBackString() + "/sec"; } }

        public override BigInteger PriceToNextLv
        { get
            {
                double growth = Mathf.Pow(ElementalData.GrowthRate, LevelCount);
                int intgrowth = 0;
                BigInteger result;

                if ((int)growth < int.MaxValue / 10000)
                {
                    intgrowth = (int)(growth * 10000);
                    result = ElementalData.BaseExp * intgrowth / 10000;
                }
                else
                {
                    intgrowth = (int)growth;
                    result = ElementalData.BaseExp * intgrowth;
                }
                return result;
            }
        }

        // for projectile action;
        private Transform ProjectileZone; // avatar
        private GameObject CurrentProjectile;
        private GameObject ProjectilePrefab;
        private double AttackTimer;

        // status property
        private bool isReadyAction { get { return CurrentProjectile.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(ElementalData.ProjectileReadyStateName); } }
        private bool NoProjectile { get { return CurrentProjectile == null; } }

        public Elemental(ElementalData data, Transform area)
        {
            LevelCount = 0;
            ProjectileZone = area;
            ProjectilePrefab = Resources.Load(data.ProjectilePrefabName) as GameObject;
            ElementalData = data;
        }

        internal void CreateProjectile()
        {
            CurrentProjectile = GameObject.Instantiate<GameObject>(ProjectilePrefab);
            CurrentProjectile.transform.parent = ViewLoader.projectiles.transform;

            Vector3 genPosition = RandomRangeVector3(ProjectileZone.localPosition, ProjectileZone.localScale / 2);
            CurrentProjectile.transform.localPosition = genPosition;

            CurrentProjectile.SetActive(true);
        }

        public void Shoot(Vector3 target)
        {
            if (CurrentProjectile != null && isReadyAction)
            {
                //SetDamage
                CurrentProjectile.GetComponent<Projectile>().Damage = Damage;

                // Shootfunc
                CurrentProjectile.GetComponent<Animator>().SetBool("Shoot", true);
                CurrentProjectile.GetComponent<Rigidbody>().useGravity = true;

                float xDistance, yDistance, zDistance;

                xDistance = target.x - CurrentProjectile.transform.position.x;
                yDistance = target.y - CurrentProjectile.transform.position.y;
                zDistance = target.z - CurrentProjectile.transform.position.z;

                float throwangle_xy;

                throwangle_xy = Mathf.Atan((yDistance + (-Physics.gravity.y / 2)) / xDistance);

                //float totalVelo = xDistance / Mathf.Cos(throwangle_xy);

                float xVelo, yVelo, zVelo;
                xVelo = xDistance;
                yVelo = xDistance * Mathf.Tan(throwangle_xy);
                zVelo = zDistance;

                CurrentProjectile.GetComponent<Rigidbody>().velocity = new Vector3(xVelo, yVelo, zVelo);

                AttackTimer -= AttackInterval; // 정상적으로 발사 완료 후 타이머리셋
            }
        }

        internal void Update()
        {
            AttackTimer += Time.deltaTime;

            // 만들어진다.
            if (AttackTimer > AttackInterval - 1 && NoProjectile)
            {
                CreateProjectile();
            }

            if (AttackTimer > AttackInterval)
            {
                if(target != null)
                {
                    Vector3 destination = calDestination(target.avatarCollision);
                    Shoot(destination);
                }
                else if (target == null)
                {
                    AttackTimer = AttackInterval; // 타겟이생길떄까지 대기한다.
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

        private Vector3 RandomRangeVector3(Vector3 center, Vector3 extend)
        {
            Vector3 min = center - extend;
            Vector3 max = center + extend;

            float x = UnityEngine.Random.Range(min.x, max.x);
            float y = UnityEngine.Random.Range(min.y, max.y);
            float z = UnityEngine.Random.Range(min.z, max.z);

            Vector3 temp = new Vector3(x, y, z);
            return temp;
        }

        public virtual void Cast()
        {

        }

    }
}