using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class Elemental // base class of Elementals
    {
        // for damage and exp
        protected int level;

        // for projectile action;
        public double AttackTimer;

        protected GameObject Shooter; // avatar
        protected GameObject ProjectilePrefab;
        protected GameObject CurrentProjectile;
        protected GameObject Target;

        ElementalData ElementalData;

        bool isReadyAction { get { return CurrentProjectile.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(ElementalData.ProjectileReadyStateName); } }
        double Dps { get { return level * ElementalData.BaseDps; } } // BaseDps* level 값.               // 계산되는값
        double Damage { get { return Dps * ElementalData.AttackInterval; } } //  dps / attackinterval    // 계산되는값
        int ExpforNextLevel { get {  return (int)(ElementalData.BaseExp * Mathf.Pow(ElementalData.GrowthRate,level)); } }
//        protected int ExpforNextLevel; // n+1번째 레벨을 올리기 위한 exp                                // 계산되는값.


        public Elemental(string name, ElementalData data, GameObject projectileprefab, GameObject shooter)
        {
            level = 0;
            Shooter = shooter;
            ProjectilePrefab = projectileprefab;
            ElementalData = data;
            Target = GameObject.Find("monster");
        }

        public virtual void Update()
        {
            AttackTimer += Time.deltaTime;

            // 만들어진다.
            if (AttackTimer > ElementalData.AttackInterval - 1 && CurrentProjectile == null)
            {
                CurrentProjectile = CreateProjectile(ProjectilePrefab);
            }

            if (AttackTimer > ElementalData.AttackInterval)
            {
                Shoot(CurrentProjectile, Target.transform.position);
            }
        }

        GameObject CreateProjectile(GameObject prefab)
        {
            GameObject Projectile = GameObject.Instantiate<GameObject>(prefab);
            Projectile.transform.parent = Shooter.transform; // 자기자신의위치에만든다.
            Projectile.transform.localPosition = Vector3.zero;
            Projectile.SetActive(true);

            return Projectile;
        }

        public virtual void Shoot(GameObject projectile, Vector3 target)
        {
            if (projectile != null && isReadyAction)
            {
                //SetDamage
                GameManager.SexyBackLog(Damage);
                projectile.GetComponent<Projectile>().Damage = Damage;

                // Shootfunc
                projectile.GetComponent<Animator>().SetBool("Shoot", true);
                projectile.GetComponent<Rigidbody>().useGravity = true;

                float xDistance, yDistance, zDistance;

                xDistance = target.x - Shooter.transform.position.x;
                yDistance = target.y - Shooter.transform.position.y;
                zDistance = target.z - Shooter.transform.position.z;

                float throwangle_xy;

                throwangle_xy = Mathf.Atan((yDistance + (-Physics.gravity.y / 2)) / xDistance);

                float totalVelo = xDistance / Mathf.Cos(throwangle_xy);

                float xVelo, yVelo, zVelo;
                xVelo = xDistance;
                yVelo = xDistance * Mathf.Tan(throwangle_xy);
                zVelo = zDistance;

                projectile.GetComponent<Rigidbody>().velocity = new Vector3(xVelo, yVelo, zVelo);

                AttackTimer -= ElementalData.AttackInterval; // 정상적으로 발사 완료 후 타이머리셋
            }
        }

        internal void LevelUp()
        {
            level++;
//            GameManager.SexyBackLog("level : " + level + " Dps : " + Dps + " ReqExpToCurrentLv : " + ExpforNextLevel);
        }

        public void ShootForDebug()
        {
            Shoot(CurrentProjectile, Target.transform.position);
        }
        public virtual void Cast()
        {

        }

    }
}