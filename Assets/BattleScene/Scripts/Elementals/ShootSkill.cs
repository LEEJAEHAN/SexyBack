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

        public Queue<GameObject> projectiles = new Queue<GameObject>(100);
//        public int remainCount = 0;
        internal double ticktimer = 0;
        double ReLoadInterval;

        int ReloadCount = 0;
        int ShootCount = 0;

        bool RandomGen = false;
        bool RandomTarget = false;

        Vector3 center;
        Vector3 extend; 

        public ShootSkill(string ownerID, string prefab, DamageType ability, int damageRatio, Debuff.Type debuff, float Speed, int Amount, double Tick, bool randomgen, bool randomtarget)
            : base(ownerID, prefab, ability, damageRatio, debuff)
        {
            this.Speed = Speed;
            this.Unit = Amount;
            this.Tick = Tick;
            isTargetEnemy = true;
            RandomGen = randomgen;
            RandomTarget = randomtarget;
        }

        internal override void ReLoad(double timer)
        {   
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
                        view = Shooter.LoadProjectile(this.ownerID, prefabname, ViewLoader.shooter.transform ,randPosition);
                    }
                    else
                        view = Shooter.LoadProjectile(this.ownerID, prefabname, ViewLoader.shooter.transform, shootZone.position);
                    ReloadCount++;
                    view.tag = "SkillProjectile";
                    projectiles.Enqueue(view);

                    ticktimer -= Tick;
                    if(ReloadCount >= Unit)
                    {
                        ReloadCount = 0;
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
                    ShootCount = Unit;
                    center = Singleton<MonsterManager>.getInstance().GetMonster(targetID).CenterPosition;
                    extend = Singleton<MonsterManager>.getInstance().GetMonster(targetID).Size / 2;
                    if (RandomTarget)
                        Shooter.Shoot(Shooter.RandomRangeVector3(center, extend), Speed, projectiles.Dequeue());
                    else
                        Shooter.Shoot(center, Speed, projectiles.Dequeue());
                    ShootCount--;
                    ReLoaded = false;
                    return true;
                }
                else if (targetID == null) { } //타겟이생길떄까지 대기한다. 
            }
            return false;
        }

        internal override void Update() // 큐에서 나머지남은것들 날림
        {
            if (ShootCount <= 0)
                return;

            ticktimer += Time.deltaTime;
            while(ticktimer > Tick)
            {
                ticktimer -= Tick;
                if (RandomTarget)
                    Shooter.Shoot(Shooter.RandomRangeVector3(center, extend), Speed, projectiles.Dequeue());
                else
                    Shooter.Shoot(center, Speed, projectiles.Dequeue());
                ShootCount--;
                if (ShootCount <= 0)
                    return;
            }
        }

        internal override void CalDamage(BigInteger elementaldmg)
        {
            DAMAGE = elementaldmg * DAMAGERATIO / (100 * this.Unit);
        }
    }

}