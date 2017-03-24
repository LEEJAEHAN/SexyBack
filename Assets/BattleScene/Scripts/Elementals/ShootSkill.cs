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
        bool PostUpdate = false;
        bool RandomGen = false;

        Vector3 target;
        
        public ShootSkill(string ownerID, string prefab, DamageType ability, int damageRatio, string debuff, float Speed, int Amount, double Tick, bool randomgen)
            : base(ownerID, prefab, ability, damageRatio, debuff)
        {
            this.Speed = Speed;
            this.Unit = Amount;
            this.Tick = Tick;
            isTargetEnemy = true;
            RandomGen = randomgen;
        }

        internal override void ReLoad(double timer)
        {   // 리로드 없다.
            if (timer > ReLoadInterval && !ReLoaded)
            {
                ticktimer += Time.deltaTime;
                while (ticktimer > Tick)
                {
                    Transform shootZone = ViewLoader.area_elemental.transform;
                    GameObject view;
                    if(RandomGen)
                    {
                        Vector3 randPosition = Shooter.RandomRangeVector3(shootZone.position, shootZone.localScale / 2);
                        view = Shooter.LoadProjectile(this.ownerID, prefabname, randPosition);
                    }
                    else
                        view = Shooter.LoadProjectile(this.ownerID, prefabname, shootZone.position);
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
                    target = Shooter.calPosition(targetID, false);
                    Shooter.Shoot(target, Speed, projectiles.Dequeue());
                    ReLoaded = false;
                    if(projectiles.Count > 0)
                        PostUpdate = true;
                    return true;
                }
                else if (targetID == null) { } //타겟이생길떄까지 대기한다. 
            }
            return false;
        }

        internal override void Update() // 큐에서 나머지남은것들 날림
        {
            if (projectiles.Count <= 0 || !PostUpdate)
                return;

            ticktimer += Time.deltaTime;
            while (ticktimer > Tick)
            {
                ticktimer -= Tick;
                Shooter.Shoot(target, Speed, projectiles.Dequeue());
                if (projectiles.Count <= 0)
                {
                    PostUpdate = false;
                    return;
                }
            }
        }

        internal override void CalDamage(BigInteger elementaldmg)
        {
            DAMAGE = elementaldmg * DAMAGERATIO / (100 * this.Unit);
        }
    }

}