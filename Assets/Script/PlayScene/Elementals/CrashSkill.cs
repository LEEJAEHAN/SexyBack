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

        public CrashSkill(string ownerID, string prefab, DamageType ability, int damageRatio, Debuff.Type debuff, float Speed, bool direction)
            : base(ownerID, prefab, ability, damageRatio, debuff)
        {
            this.Speed = Speed;
            isTargetEnemy = true;
            this.direction = direction;
        }

        internal override void ReLoad(double timer)
        {
            if (timer > ReLoadInterval && !ReLoaded)
            {
                Vector3 summonPoint = new Vector3((direction == true) ? 7.2f : -7.2f, 12.8f, 0);
                projectile = Shooter.LoadProjectile(this.ownerID, prefabname, ViewLoader.projectiles.transform, summonPoint);
                projectile.tag = "MeteorProjectile";
                projectile.GetComponent<ProjectileView>().floorOnly = true;
                ReLoaded = true;
                return;
            }
        }

        internal override void SetInterval(double castInterval)
        {
            CASTINTERVAL = castInterval;
            ReLoadInterval = UnityEngine.Mathf.Max((float)(castInterval - 1f), (float)(castInterval * 0.5));
        }

        // cast의 경우는 몬스터에게 바로 debuff를 건다.
        internal override bool Shoot(double timer, string targetID) // 큐에서 첫번쨰꺼날림
        {
            if (timer > CASTINTERVAL && ReLoaded)
            {
                if (targetID != null)
                {
                    Shooter.Shoot(Singleton<MonsterManager>.getInstance().GetMonster().CenterPosition, Speed, projectile);
                    projectile = null;
                    ReLoaded = false;
                    return true;
                }
                else if (targetID == null) { } //타겟이생길떄까지 대기한다. 
            }
            return false;
        }
    }

}