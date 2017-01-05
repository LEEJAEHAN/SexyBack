using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class FireElemental : Elemental
    {
        double AttackTimer;
        double AttackInterval;
        double CreateActionTime;

        double DPS;
        double Damage;

        GameObject shooter; // avatar
        GameObject projectile;
        GameObject target;

        public FireElemental()
        {
            AttackInterval = 10;
            CreateActionTime = 5;
            Damage = 10;
            DPS = Damage / AttackInterval;

            shooter = GameObject.Find("shooter_fireball");
            //projectile = GameObject.Instantiate<GameObject>(Resources.Load("prefabs/fireball") as GameObject);
            target = GameObject.Find("monster");

        }
        public override void Cast()
        {
        }

        void CreateProjectile()
        {
            //createProjectile
            projectile = GameObject.Instantiate<GameObject>(Resources.Load("prefabs/fireball") as GameObject);
            //projectile.GetComponent<Projectile>().damage = Damage;
            //projectile.transform.localScale = shooter.transform.localScale;
            projectile.transform.parent = shooter.transform;
            projectile.transform.localPosition = Vector3.zero;

//            projectile.transform.position = shooter.transform.position;
            projectile.SetActive(true);

            GameManager.SexyBackLog("Create fire");
        }
        public override void Update()
        {

            AttackTimer += Time.deltaTime;
            if (AttackTimer > AttackInterval - CreateActionTime)
            {
                // 만들어진다.
                if (projectile == null)
                {
                    CreateProjectile();
                }
                if (AttackTimer > AttackInterval)
                {
                    Shoot();
                    AttackTimer -= AttackInterval;
                }
            }
        }

        public override void Shoot()
        {
            if (projectile != null && projectile.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("fireball_spot"))
            {

                //            projectile = GameObject.Instantiate<GameObject>(Resources.Load("prefabs/fireball") as GameObject);
                projectile.GetComponent<Animator>().SetBool("Shoot", true);
                projectile.GetComponent<Rigidbody>().useGravity = true;


                float xDistance;
                float yDistance;
                float zDistance;

                xDistance = target.transform.position.x - shooter.transform.position.x;
                yDistance = target.transform.position.y - shooter.transform.position.y;
                zDistance = target.transform.position.z - shooter.transform.position.z;

                // yz 앵글이랑
                // xy 앵글
                float throwangle_xy;

                throwangle_xy = Mathf.Atan((yDistance + (-Physics.gravity.y / 2)) / xDistance);

                float totalVelo = xDistance / Mathf.Cos(throwangle_xy);

                float xVelo, yVelo;
                xVelo = xDistance;
                yVelo = xDistance * Mathf.Tan(throwangle_xy);
                float zVelo = zDistance;

                projectile.GetComponent<Rigidbody>().velocity = new Vector3(xVelo, yVelo, zVelo);
            }

        }

    }
}