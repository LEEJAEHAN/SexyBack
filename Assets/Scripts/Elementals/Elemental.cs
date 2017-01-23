using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    public class Elemental // base class of Elementals
    {
        private ElementalData baseData;
        public Monster target;
        private int level;
        public BigInteger DpsX;

        // data property
        public string ID { get { return baseData.ID; } }
        public string NAME { get { return baseData.Name; } }
        public int LEVEL { get { return level; } }
        public BigInteger DPS { get { return level * baseData.BaseDps * DpsX; } }
        public BigInteger BASEDPS { get { return level * baseData.BaseDps; } } // BaseDps* level 값.               // 계산되는값
        public BigInteger NEXTDPS { get { return baseData.BaseDps; } }
        public BigInteger DAMAGE { get { return (DPS * baseData.AttackIntervalK) / 1000; } } //  dps / attackinterval    // 계산되는값
        public double AttackInterval { get { return (double)baseData.AttackIntervalK / (double)1000; } }

        // for projectile action;
        private Transform ElementalArea; // avatar
        private GameObject CurrentProjectile;
        private GameObject ProjectilePrefab;
        private double AttackTimer;

        // status property
        private bool isReadyAction { get { return CurrentProjectile.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(baseData.ProjectileReadyStateName); } }
        private bool NoProjectile { get { return CurrentProjectile == null; } }

        public Elemental(ElementalData data, Transform area)
        {
            LevelUp(1);
            ElementalArea = area;
            ProjectilePrefab = Resources.Load(data.ProjectilePrefabName) as GameObject;
            baseData = data;

            DpsX = new BigInteger(1);
        }

        internal void CreateProjectile()
        {
            CurrentProjectile = GameObject.Instantiate<GameObject>(ProjectilePrefab);

            CurrentProjectile.transform.name = this.ID;

            Vector3 genPosition = RandomRangeVector3(ElementalArea.position, ElementalArea.localScale / 2);
            // position 은 월드포지션. localpositoin은 부모에서 얼마떨어져있냐, localScale은 그 객체클릭했을때 나오는 사이즈값. lossyscale은 최고 root로빠졌을때의 사이즈값.
            CurrentProjectile.transform.position = genPosition;
            CurrentProjectile.transform.localScale = ViewLoader.projectiles.transform.lossyScale;
            CurrentProjectile.transform.parent = ViewLoader.shooter.transform;

            //TODO : 아직 리팩토링할곳이많은부분, 바로윗줄은 shooter object의 scale을 world에서 조정해야함

            CurrentProjectile.SetActive(true);
        }

        public void Shoot(Vector3 target)
        {
            if (CurrentProjectile != null && isReadyAction)
            {
                CurrentProjectile.transform.parent = ViewLoader.projectiles.transform ; // 슈터에서 빠진다.
                //SetDamage
                CurrentProjectile.GetComponent<Projectile>().Damage = DAMAGE;
                //TODO: 여기바꺼야함
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

                AttackTimer = 0; // 정상적으로 발사 완료 후 타이머리셋
            }
        }

        internal void LevelUp(int toLevel)
        {
            level+= toLevel;
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
            if(AttackTimer > AttackInterval -1 && !NoProjectile) 
            {
                // 대기중
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