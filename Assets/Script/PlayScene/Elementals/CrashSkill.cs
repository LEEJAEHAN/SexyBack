using System;
using UnityEngine;
using System.Collections.Generic;


namespace SexyBackPlayScene
{

    internal class CrashSkill : Skill
    {
        //skilldata
        internal float Speed;
        public GameObject projectile;
        double ReLoadInterval;
        bool direction;

        public CrashSkill(string ownerID, string prefab, DamageType ability, Debuff.Type debuff, float Speed, bool direction)
            : base(ownerID, prefab, ability, debuff)
        {
            this.Speed = Speed;
            isTargetEnemy = true;
            this.direction = direction;
        }
        internal override void SetInterval(double castInterval)
        {
            CastInterval = castInterval;
            ReLoadInterval = UnityEngine.Mathf.Max((float)(castInterval - 1f), (float)(castInterval * 0.5));
        }
        internal override void Start(bool NoReloadTime)
        {
            Enable = true;
            ReLoaded = false;
            if (NoReloadTime)
                AttackTimer = ReLoadInterval;
            else
                AttackTimer = 0;
        }
        internal override void ReLoad()
        {
            if (AttackTimer > ReLoadInterval && !ReLoaded)
            {
                Vector3 summonPoint = new Vector3((direction == true) ? 7.2f : -7.2f, 12.8f, 0);
                projectile = Shooter.LoadProjectile(this.ownerID, prefabname, ViewLoader.projectiles.transform, summonPoint);
                projectile.tag = "MeteorProjectile";
                projectile.GetComponent<ProjectileView>().floorOnly = true;
                ReLoaded = true;
                return;
            }
        }
        // cast의 경우는 몬스터에게 바로 debuff를 건다.
        internal override void FirstShoot() // 큐에서 첫번쨰꺼날림
        {
            if (AttackTimer > CastInterval && ReLoaded)
            {
                if (targetID != null)
                {
                    Shooter.Shoot(Singleton<MonsterManager>.getInstance().GetMonster().CenterPosition, Speed, projectile);
                    projectile = null;
                    AttackTimer = 0;
                    ReLoaded = false;
                    Enable = false;
                }
                else if (targetID == null) { } //타겟이생길떄까지 대기한다. 
            }
        }

        internal override bool CheckFinish()
        {
            return !Enable;
        }

       
    }

}