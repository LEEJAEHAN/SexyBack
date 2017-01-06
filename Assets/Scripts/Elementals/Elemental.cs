using UnityEngine;

namespace SexyBackPlayScene
{
    public class Elemental // base class of Elementals
    {
        // for damage and exp
        protected int level;
        protected double Dps; // BaseDps* level 값.
        protected double Damage; // dps / attackinterval                                                                 // 계산되는값.
        protected int ExpforNthLevel; // n번째 레벨을 올리기 위한 exp                                                      // 계산되는값.

        // for projectile action;
        public double AttackTimer;

        protected GameObject Shooter; // avatar
        protected GameObject Projectile;
        protected GameObject Target;

        ElementalData ElementalData;
        
        public Elemental(string name, ElementalData data, GameObject shooter)
        {

            //GameObject.Find(shooterName);
            Shooter = shooter;
            ElementalData = data;
            Target = GameObject.Find("monster");
        }

        void CreateProjectile()
        {
            //createProjectile
            Projectile = GameObject.Instantiate<GameObject>(Resources.Load(ElementalData.ProjectilePrefabName) as GameObject);
            //projectile.GetComponent<Projectile>().damage = Damage;
            Projectile.transform.parent = Shooter.transform;
            Projectile.transform.localPosition = Vector3.zero;
            Projectile.SetActive(true);
        }
        public virtual void Update()
        {
            AttackTimer += Time.deltaTime;

            // 만들어진다.
            if (AttackTimer > ElementalData.AttackInterval - 2 && Projectile == null)
            {
                CreateProjectile();
            }

            if (AttackTimer > ElementalData.AttackInterval)
            {
                Shoot();
            }
        }

        public virtual void Shoot()
        {
            if (Projectile != null && Projectile.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(ElementalData.ProjectileReadyStateName))
            {
                //projectile = GameObject.Instantiate<GameObject>(Resources.Load("prefabs/fireball") as GameObject);
                Projectile.GetComponent<Animator>().SetBool("Shoot", true);
                Projectile.GetComponent<Rigidbody>().useGravity = true;

                float xDistance, yDistance, zDistance;

                xDistance = Target.transform.position.x - Shooter.transform.position.x;
                yDistance = Target.transform.position.y - Shooter.transform.position.y;
                zDistance = Target.transform.position.z - Shooter.transform.position.z;

                float throwangle_xy;

                throwangle_xy = Mathf.Atan((yDistance + (-Physics.gravity.y / 2)) / xDistance);

                float totalVelo = xDistance / Mathf.Cos(throwangle_xy);

                float xVelo, yVelo;
                xVelo = xDistance;
                yVelo = xDistance * Mathf.Tan(throwangle_xy);
                float zVelo = zDistance;

                Projectile.GetComponent<Rigidbody>().velocity = new Vector3(xVelo, yVelo, zVelo);

                AttackTimer -= ElementalData.AttackInterval; // 정상적으로 발사 완료 후 타이머리셋
            }
        }

        public virtual void Cast()
        {

        }

    }
}