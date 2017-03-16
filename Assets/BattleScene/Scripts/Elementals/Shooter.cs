using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class Shooter // TODO : 스킬까지포함하여, 슈터의 느낌으로 하자.
    {
        public string ownerID;
        internal bool isReady = false;

        // base projectile info
        string prefabname;
        private Transform shootZone; // avatar
        public Queue<GameObject> projectiles = new Queue<GameObject>();

        public Shooter(string ownerID, string normalprefab)
        {
            this.ownerID = ownerID;
            prefabname = normalprefab;
            this.shootZone = ViewLoader.area_elemental.transform;
        }

        private Vector3 RandomRangeVector3(Vector3 center, Vector3 extend)
        {
            Vector3 min = center - extend;
            Vector3 max = center + extend;

            return new Vector3(UnityEngine.Random.Range(min.x, max.x),
                UnityEngine.Random.Range(min.y, max.y),
                UnityEngine.Random.Range(min.z, max.z));
        }

        public Vector3 calDestination(string targetID)
        {
            Monster target = Singleton<MonsterManager>.getInstance().GetMonster(targetID);
            Vector3 center = target.CenterPosition;
            Vector3 extend = target.Size / 2;
            Vector3 dest = RandomRangeVector3(center, extend);
            return dest;
        }

        internal void reload(string prefabpath)
        {
            Vector3 genPosition = RandomRangeVector3(shootZone.position, shootZone.localScale / 2);

            GameObject view = GameObject.Instantiate<GameObject>(Resources.Load(prefabpath) as GameObject);
            view.name = ownerID;
            view.tag = "Projectile";
            view.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            view.transform.parent = ViewLoader.shooter.transform;
            view.transform.position = genPosition; // not local position
            view.GetComponent<SphereCollider>().enabled = false;
            view.SetActive(true);

            projectiles.Enqueue(view);
            isReady = true;
            // position 은 월드포지션. localpositoin은 부모에서 얼마떨어져있냐, localScale은 그 객체클릭했을때 나오는 사이즈값. lossyscale은 최고 root로빠졌을때의 사이즈값.
            //view.GetComponent<ProjectileView>().noticeDestroy += owner.onDestroyProjectile;
        }

        internal bool Shoot(string targetID, float speed) // 1개 쏘기
        {
            if (projectiles.Count <= 0)
                return false;

            Vector3 target = calDestination(targetID);

            GameObject view = projectiles.Dequeue();
            view.transform.parent = ViewLoader.projectiles.transform; // 슈터에서 빠진다.
                                                                      // Shootfunc
            view.GetComponent<Animator>().SetBool("Shoot", true);
            view.GetComponent<SphereCollider>().enabled = true;
            view.GetComponent<Rigidbody>().useGravity = true;

            float xDistance = target.x - view.transform.position.x;
            float yDistance = target.y - view.transform.position.y;
            float zDistance = target.z - view.transform.position.z;

            float throwangle_xy;

            throwangle_xy = Mathf.Atan((yDistance + (-Physics.gravity.y * Mathf.Pow(speed, 2) / 2)) / xDistance);

            //float totalVelo = xDistance / Mathf.Cos(throwangle_xy);

            float xVelo, yVelo, zVelo;
            xVelo = xDistance / speed;
            yVelo = xDistance * Mathf.Tan(throwangle_xy) / speed;
            zVelo = zDistance / speed;

            view.GetComponent<Rigidbody>().velocity = new Vector3(xVelo, yVelo, zVelo);

            isReady = false;
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