using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class Shooter // TODO : 스킬까지포함하여, 슈터의 느낌으로 하자.
    {
        public double CastInterval;
        public string ownerID;
        bool ReLoaded = false;
        public bool Enable = false;
        public string targetID;

        // base projectile info
        double ReLoadInterval;
        public GameObject projectile;
        string prefabname;

        private double AttackTimer = 0;

        public Shooter(string ownerID, string normalprefab)
        {
            this.ownerID = ownerID;
            prefabname = normalprefab;
        }
        internal void Update()
        {
            if(Enable)
            {
                AttackTimer += Time.deltaTime;
                ReLoad();
                Shoot();
            }
        }
        internal void ReLoad()
        {
            if (AttackTimer > ReLoadInterval && !ReLoaded)
            {
                Transform shootZone = ViewLoader.area_elemental.transform;
                Vector3 genPosition = RandomRangeVector3(shootZone.position, shootZone.localScale / 2);
                projectile = LoadProjectile(ownerID, prefabname, ViewLoader.shooter.transform, genPosition);
                ReLoaded = true;
            }
        }
        internal void Shoot()
        {
            if (AttackTimer > CastInterval && ReLoaded)
            {
                if (targetID != null)
                {
                    Monster target = Singleton<MonsterManager>.getInstance().GetBattleMonster();
                    Shoot(RandomRangeVector3(target.CenterPosition, target.Size / 2), 1f, projectile);
                    ReLoaded = false;
                    AttackTimer = 0;
                    Enable = false;
                }
            }
        }

        internal void SetInterval(double castInterval)
        {
            CastInterval = castInterval;
            ReLoadInterval = UnityEngine.Mathf.Max((float)(castInterval - 1f), (float)(castInterval * 0.5));
        }

        public static Vector3 RandomRangeVector3(Vector3 center, Vector3 extend)
        {
            Vector3 min = center - extend;
            Vector3 max = center + extend;

            return new Vector3(UnityEngine.Random.Range(min.x, max.x),
                UnityEngine.Random.Range(min.y, max.y),
                UnityEngine.Random.Range(min.z, max.z));
        }

        internal static GameObject LoadProjectile(string id, string prefabpath, Transform parent, Vector3 genPosition)
        {
            GameObject view = GameObject.Instantiate<GameObject>(Resources.Load(prefabpath) as GameObject);
            view.name = id;
            view.tag = "Projectile";
            view.transform.parent = parent;
            view.transform.position = genPosition;// not local position
            view.GetComponent<SphereCollider>().enabled = false;
            view.SetActive(true);
            return view;
            // position 은 월드포지션. localpositoin은 부모에서 얼마떨어져있냐, localScale은 그 객체클릭했을때 나오는 사이즈값. lossyscale은 최고 root로빠졌을때의 사이즈값.
            //view.GetComponent<ProjectileView>().noticeDestroy += owner.onDestroyProjectile;
        }

        internal static bool Shoot(Vector3 target, float speed, GameObject view) // 1개 쏘기
        {
            if (view == null)
                return false;

            view.transform.parent = ViewLoader.projectiles.transform; // 슈터에서 빠진다.
                                                                      // Shootfunc
            view.GetComponent<Animator>().SetBool("Shoot", true);
            view.GetComponent<SphereCollider>().enabled = true;
            view.GetComponent<Rigidbody>().useGravity = true;

            float xDistance = target.x - view.transform.position.x;
            float yDistance = target.y - view.transform.position.y;
            float zDistance = target.z - view.transform.position.z;

            float throwangle_xy;

            if (xDistance == 0) // angle is 90
                xDistance = -0.001f;
            throwangle_xy = Mathf.Atan((yDistance + (-Physics.gravity.y * Mathf.Pow(1f/speed, 2) / 2)) / xDistance);

            //float totalVelo = xDistance / Mathf.Cos(throwangle_xy);

            float xVelo, yVelo, zVelo;
            xVelo = xDistance * speed;
            yVelo = xDistance * Mathf.Tan(throwangle_xy) * speed;
            zVelo = zDistance * speed;

            view.GetComponent<Rigidbody>().velocity = new Vector3(xVelo, yVelo, zVelo);

            return true;
        }



        // 0.8초 제약 둔이유
        //private bool isReadyState
        //{
        //    get
        //    {
        //        string ready = ElementalData.ProjectileReadyStateName(ownerID);
        //        return view.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(ready);
        //    }
        //}

    }
}