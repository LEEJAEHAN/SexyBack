using UnityEngine;

namespace SexyBackPlayScene
{
    public abstract class Elemental // base class of Elementals
    {
        protected double AttackTimer;
        protected double AttackInterval;
        protected double CreateActionTime;

        protected double DPS;
        protected double Damage;

        protected GameObject shooter; // avatar
        protected GameObject projectile;
        protected GameObject target;

        protected string ProjectilePrefabName;
        protected string ProjectileReadyStateName;
        protected string ShooterName;

        public Elemental()
        {
            AttackInterval = 5;
            CreateActionTime = 5;
            Damage = 10;
            DPS = Damage / AttackInterval;

            // default = airblock
            ShooterName = "shooter_airball";
            ProjectilePrefabName = "prefabs/airball";
            ProjectileReadyStateName = "airball_spot";
            // default = airblock

            target = GameObject.Find("monster");
        }
        void CreateProjectile()
        {
            // init shooter
            shooter = GameObject.Find(ShooterName);
            //createProjectile
            projectile = GameObject.Instantiate<GameObject>(Resources.Load(ProjectilePrefabName) as GameObject);
            //projectile.GetComponent<Projectile>().damage = Damage;
            projectile.transform.parent = shooter.transform;
            projectile.transform.localPosition = Vector3.zero;
            projectile.SetActive(true);
        }
        public virtual void Update()
        {
            AttackTimer += Time.deltaTime;

            // 만들어진다.
            if (AttackTimer > AttackInterval - 2 && projectile == null)
            {
                CreateProjectile();
            }

            if (AttackTimer > AttackInterval)
            {
                Shoot();
            }
        }

        public virtual void Shoot()
        {
            if (projectile != null && projectile.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(ProjectileReadyStateName))
            {
                //projectile = GameObject.Instantiate<GameObject>(Resources.Load("prefabs/fireball") as GameObject);
                projectile.GetComponent<Animator>().SetBool("Shoot", true);
                projectile.GetComponent<Rigidbody>().useGravity = true;

                float xDistance, yDistance, zDistance;

                xDistance = target.transform.position.x - shooter.transform.position.x;
                yDistance = target.transform.position.y - shooter.transform.position.y;
                zDistance = target.transform.position.z - shooter.transform.position.z;

                float throwangle_xy;

                throwangle_xy = Mathf.Atan((yDistance + (-Physics.gravity.y / 2)) / xDistance);

                float totalVelo = xDistance / Mathf.Cos(throwangle_xy);

                float xVelo, yVelo;
                xVelo = xDistance;
                yVelo = xDistance * Mathf.Tan(throwangle_xy);
                float zVelo = zDistance;

                projectile.GetComponent<Rigidbody>().velocity = new Vector3(xVelo, yVelo, zVelo);

                AttackTimer -= AttackInterval; // 정상적으로 발사 완료 후 타이머리셋
            }
        }

        public virtual void Cast()
        {

        }

    }
}