using System;
using UnityEngine;
using System.Collections.Generic;


namespace SexyBackPlayScene
{

    internal class DropSkill : Skill
    {
        //skilldata
        // member of DropSkill
        internal int Unit;
        internal double Tick;
        internal double PostTimer = 0;

        public int ShootCount = 0;
        Vector3 SpawnCenter;
        Vector3 SpawnSize;

        public DropSkill(string ownerID, string prefab, DamageType ability,  Debuff.Type debuff, int Amount, double Tick )
            : base(ownerID, prefab, ability, debuff)
        {
            Unit = Amount;
            this.Tick = Tick;
            isTargetEnemy = true;
        }
        internal override void Start(bool NoReloadTime)
        {
            Enable = true;
            ReLoaded = false;
            if (NoReloadTime)
                AttackTimer = CastInterval;
            else
                AttackTimer = 0;
        }
        internal override void ReLoad()
        {   // 리로드 없다.
            if (!ReLoaded)
                ReLoaded = true;
        }

        // cast의 경우는 몬스터에게 바로 debuff를 건다.
        internal override void FirstShoot()
        {
            if (AttackTimer > CastInterval && ReLoaded)
            {
                if (targetID != null)
                {
                    SetSpawnZone(targetID);
                    ShootCount += Unit;
                    AttackTimer = 0;
                    ReLoaded = false;
                    Enable = false;
                }
                else if (targetID == null) { } //타겟이생길떄까지 대기한다. 
            }
        }

        private void SetSpawnZone(string targetID)
        {
            SpawnCenter = Singleton<MonsterManager>.getInstance().GetBattleMonster().CenterPosition;
            SpawnSize = Singleton<MonsterManager>.getInstance().GetBattleMonster().Size;
            SpawnCenter.y += 6.4f; // 천장에서떨군다.
        }

        internal void LoadProjectile(string prefabpath)
        {
            GameObject view = GameObject.Instantiate<GameObject>(Resources.Load(prefabpath) as GameObject);
            view.name = ownerID;
            view.tag = "SkillProjectile";
            view.transform.parent = ViewLoader.projectiles.transform;
            view.transform.position = Shooter.RandomRangeVector3(SpawnCenter, SpawnSize / 2);
            view.GetComponent<Animator>().SetBool("Shoot", true);
            view.SetActive(true);
        }

        internal override void PostUpdate()
        {
            if (ShootCount <= 0)
                return;

            PostTimer += Time.deltaTime;
            while(PostTimer > Tick)
            {
                LoadProjectile(prefabname);
                ShootCount--;
                PostTimer -= Tick;
                if (ShootCount <= 0)
                    return;
            }
        }

        internal override void CalDamage(BigInteger elementaldmg)
        {
            DAMAGE = elementaldmg * DAMAGERATIO / (100 * this.Unit);
        }


        internal override bool CheckFinish()
        {
            return (!Enable && ShootCount <= 0 );
        }
    }

}