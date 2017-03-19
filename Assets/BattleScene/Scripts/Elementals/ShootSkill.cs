using System;
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

        public Queue<GameObject> projectiles = new Queue<GameObject>();
//        public int remainCount = 0;
        internal double ticktimer = 0;
        double ReLoadInterval;
        bool duringShoot = false;
        
        public ShootSkill(string ownerID, string prefab, DamageType ability, int damageRatio, Debuff debuff, float Speed, int Amount, double Tick)
            : base(ownerID, prefab, ability, damageRatio, debuff)
        {
            this.Speed = Speed;
            this.Unit = Amount;
            this.Tick = Tick;
            isTargetEnemy = true;
        }

        internal override void ReLoad(double timer)
        {   // 리로드 없다.
            if (timer > ReLoadInterval && !ReLoaded)
            {
                ticktimer += Time.deltaTime;
                while (ticktimer > Tick)
                {
                    GameObject view = Shooter.LoadProjectile(this.ownerID, prefabname);
                    view.tag = "SkillProjectile";
                    projectiles.Enqueue(view);
                    ticktimer -= Tick;
                    if(projectiles.Count >= Unit)
                    {
                        ReLoaded = true;
                        return;
                    }
                }
            }
        }

        internal override void SetInterval(double castInterval)
        {
            CASTINTERVAL = castInterval;
            ReLoadInterval = UnityEngine.Mathf.Max((float)(castInterval - 1f - Tick*(Unit)), 0);
        }

        // cast의 경우는 몬스터에게 바로 debuff를 건다.
        internal override bool Shoot(double timer, string targetID) // 큐에서 첫번쨰꺼날림
        {
            if (timer > CASTINTERVAL && ReLoaded)
            {
                if (targetID != null)
                {
                    Shooter.Shoot(targetID, Speed, projectiles.Dequeue());
                    ReLoaded = false;
                    duringShoot = true;
                    return true;
                }
                else if (targetID == null) { } //타겟이생길떄까지 대기한다. 
            }
            return false;
        }

        internal override void Update(string targetID) // 큐에서 나머지남은것들 날림
        {
            if (projectiles.Count <= 0 || !duringShoot)
                return;

            ticktimer += Time.deltaTime;
            while (ticktimer > Tick)
            {
                ticktimer -= Tick;
                Shooter.Shoot(targetID, Speed, projectiles.Dequeue());
                if (projectiles.Count <= 0)
                {
                    duringShoot = false;
                    return;
                }
            }
        }

        internal override void CalDamage(BigInteger elementaldmg)
        {
            DAMAGE = elementaldmg * DAMAGERATIO / (100 * this.Unit);
            if (debuff != null)
                debuff.DAMAGE = elementaldmg * debuff.DAMAGERATIO / (100 * this.Unit);
        }
    }

}