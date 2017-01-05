using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class FireElemental : Elemental
    {
        double AttackTimer;
        double AttackInterval;
        double DPS;
        double Damage;

        GameObject shooter;
        GameObject projectile;
        GameObject target;

        public FireElemental()
        {
            AttackInterval = 1.5;
            Damage = 10;
            DPS = Damage / AttackInterval;

            shooter = GameObject.Find("shooter_fire");
            projectile = GameObject.Instantiate<GameObject>(Resources.Load("prefabs/fireball") as GameObject);
            target = GameObject.Find("monster");

        }
        public override void Cast()
        {
        }

        public override void Shoot()
        {
            projectile.GetComponent<Projectile>().damage = Damage;
            projectile.transform.position = shooter.transform.position;
            projectile.transform.localScale = shooter.transform.localScale;

            projectile.SetActive(true);
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

        public override void Update()
        {
            AttackTimer += Time.deltaTime;
            if(AttackTimer > AttackInterval)
            {
                Shoot();
                AttackTimer -= AttackInterval;
            }
        }
    }
}