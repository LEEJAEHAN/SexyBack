﻿using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class Elemental : CanLevelUp // base class of Elementals
    {
        // for damage and exp
        // for projectile action;
        public double AttackTimer;

        protected GameObject Shooter; // avatar
        protected GameObject ProjectilePrefab;
        protected GameObject CurrentProjectile;

        ElementalData ElementalData;

        bool isReadyAction { get { return CurrentProjectile.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(ElementalData.ProjectileReadyStateName); } }
        public BigInteger Dps { get { return level * ElementalData.BaseDps; } } // BaseDps* level 값.               // 계산되는값
        BigInteger Damage { get { return (Dps * ElementalData.AttackIntervalK) / 1000; } } //  dps / attackinterval    // 계산되는값
        public double AttackInterval { get { return (double)ElementalData.AttackIntervalK / (double)1000; } private set { } }
        //BigInteger ExpforNextLevel
        //{
        //    get
        //    {
        //        double growth = Mathf.Pow(ElementalData.GrowthRate, level);
        //        int intgrowth = 0;
        //        BigInteger result;

        //        if ((int)growth < int.MaxValue / 10000)
        //        {
        //            intgrowth = (int)(growth * 10000);
        //            result = ElementalData.BaseExp * intgrowth / 10000;
        //        }
        //        else
        //        {
        //            intgrowth = (int)growth;
        //            result = ElementalData.BaseExp * intgrowth;
        //        }
        //        return result;
        //    }
        //}

        public bool NoProjectile { get { return CurrentProjectile == null; } }

        //        int ExpforNextLevel { get {  return (int)(ElementalData.BaseExp * Mathf.Pow(ElementalData.GrowthRate,level)); } }

        //        protected int ExpforNextLevel; // n+1번째 레벨을 올리기 위한 exp                                // 계산되는값.

        public Elemental(string name, ElementalData data, GameObject projectileprefab, GameObject shooter)
        {
            level = 0;
            Shooter = shooter;
            ProjectilePrefab = projectileprefab;
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

        public virtual void Cast()
        {

        }

        public override void LevelUp(int lv)
        {
            level = lv;

            // 레벨오르면 알아서 데미지계산됨

            Singleton<HeroManager>.getInstance().noticeDamageChanged();
            Singleton<LevelUpManager>.getInstance().UpdateItemView();
        }

        public override string GetDamageString()
        {
            return Dps.ToSexyBackString();
        }
    }
}