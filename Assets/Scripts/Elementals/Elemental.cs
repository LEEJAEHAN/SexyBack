using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    public class Elemental : CanLevelUp // base class of Elementals
    {
        ElementalData ElementalData;
        public SexyBackMonster target;

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
        private GameObject Shooter; // avatar
        private GameObject ProjectilePrefab;
        private GameObject CurrentProjectile;
        private double AttackTimer;

        // status property
        private bool isReadyAction { get { return CurrentProjectile.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(ElementalData.ProjectileReadyStateName); } }
        private bool NoProjectile { get { return CurrentProjectile == null; } }

        public Elemental(ElementalData data)
        {
            LevelCount = 0;
            Shooter = GameObject.Find(data.ShooterName); // 수정해야함
            ProjectilePrefab = Resources.Load(data.ProjectilePrefabName) as GameObject;
            ElementalData = data;
        }

        internal void CreateProjectile()
        {
            CurrentProjectile = GameObject.Instantiate<GameObject>(ProjectilePrefab);
            CurrentProjectile.transform.parent = Shooter.transform; // 자기자신의위치에만든다.
            CurrentProjectile.transform.localPosition = Vector3.zero;
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

                xDistance = target.x - Shooter.transform.position.x;
                yDistance = target.y - Shooter.transform.position.y;
                zDistance = target.z - Shooter.transform.position.z;

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
                    Shoot(target.Position);
                else if (target == null)
                {
                    AttackTimer = AttackInterval; // 타겟이생길떄까지 대기한다.
                }
            }
        }

        public virtual void Cast()
        {

        }

    }
}