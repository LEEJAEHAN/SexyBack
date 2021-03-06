﻿using System;
using UnityEngine;
using System.Collections.Generic;


namespace SexyBackPlayScene
{

    internal class ShootSkill : Skill
    {
        //skilldata
        internal float Speed;
        internal int Unit;
        internal double Tick;

        public Queue<GameObject> projectiles = new Queue<GameObject>(100);
        internal double PostTimer = 0;
        internal double ReloadTimer = 0;
        double ReLoadInterval;

        int ReloadCount = 0;
        int ShootCount = 0;

        bool RandomGen = false;
        bool RandomTarget = false;

        Vector3 center;
        Vector3 extend; 

        public ShootSkill(string ownerID, string prefab, DamageType ability, Debuff.Type debuff, float Speed, int Amount, double Tick, bool randomgen, bool randomtarget)
            : base(ownerID, prefab, ability, debuff)
        {
            this.Speed = Speed;
            this.Unit = Amount;
            this.Tick = Tick;
            isTargetEnemy = true;
            RandomGen = randomgen;
            RandomTarget = randomtarget;
        }
        internal override void SetInterval(double castInterval)
        {
            CastInterval = castInterval;
            ReLoadInterval = UnityEngine.Mathf.Max((float)(castInterval - 1f - Tick * (Unit)), 0);
            // 0.1초틱 10발일시 -1  -1 =>  2초전부터 소환. 만약 공속이 2초보다짧으면 0초부터바로소환.
            // 단 소환하는데는 0.1초 * 10 = 1초가 걸린다.
        }
        internal override void CalDamage(BigInteger elementaldmg)
        {
            DAMAGE = elementaldmg * DAMAGERATIO / (100 * this.Unit);
        }
        internal override void Start(bool NoReloadTime)
        {
            Enable = true;
            ReLoaded = false;
            ReloadTimer = 0;
            if (NoReloadTime)
                AttackTimer = ReLoadInterval;
            else
                AttackTimer = 0;
        }
        internal override void ReLoad()
        {   
            if (AttackTimer > ReLoadInterval && !ReLoaded)
            {
                ReloadTimer += Time.deltaTime;
                while (ReloadTimer > Tick)
                {
                    Transform shootZone = ViewLoader.area_elemental.transform;
                    GameObject view;
                    if(RandomGen)
                    {
                        Vector3 randPosition = Shooter.RandomRangeVector3(shootZone.position, shootZone.localScale / 2);
                        view = Shooter.LoadProjectile(this.ownerID, prefabname, ViewLoader.shooter.transform ,randPosition);
                    }
                    else
                        view = Shooter.LoadProjectile(this.ownerID, prefabname, ViewLoader.shooter.transform, shootZone.position);
                    ReloadCount++;
                    view.tag = "SkillProjectile";
                    projectiles.Enqueue(view);

                    ReloadTimer -= Tick;
                    if(ReloadCount >= Unit)
                    {
                        ReloadCount = 0;
                        ReLoaded = true;
                        return;
                    }
                }
            }
        }

        // cast의 경우는 몬스터에게 바로 debuff를 건다.
        internal override void FirstShoot() // 큐에서 첫번쨰꺼날림
        {
            if (AttackTimer > CastInterval && ReLoaded)
            {
                if (targetID != null)
                {
                    ShootCount = Unit;
                    center = Singleton<MonsterManager>.getInstance().GetBattleMonster().CenterPosition;
                    extend = Singleton<MonsterManager>.getInstance().GetBattleMonster().Size / 2;
                    if (RandomTarget)
                        Shooter.Shoot(Shooter.RandomRangeVector3(center, extend), Speed, projectiles.Dequeue());
                    else
                        Shooter.Shoot(center, Speed, projectiles.Dequeue());
                    ShootCount--;
                    AttackTimer = 0;
                    ReLoaded = false;
                    Enable = false;
                }
            }
        }

        internal override void PostUpdate() // 큐에서 나머지남은것들 날림
        {
            if (ShootCount <= 0)
                return;

            PostTimer += Time.deltaTime;
            while(PostTimer > Tick)
            {
                PostTimer -= Tick;
                if (RandomTarget)
                    Shooter.Shoot(Shooter.RandomRangeVector3(center, extend), Speed, projectiles.Dequeue());
                else
                    Shooter.Shoot(center, Speed, projectiles.Dequeue());
                ShootCount--;
                if (ShootCount <= 0)
                    return;
            }
        }

        internal override bool CheckFinish()
        {
            return (!Enable && ShootCount <= 0);
        }
    }

}