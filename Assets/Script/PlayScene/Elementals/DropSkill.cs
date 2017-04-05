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

        public int remainCount = 0;
        internal double timer = 0;
        Vector3 SpawnCenter;
        Vector3 SpawnSize;

        public DropSkill(string ownerID, string prefab, DamageType ability, int damageRatio, Debuff.Type debuff, int Amount, double Tick )
            : base(ownerID, prefab, ability, damageRatio, debuff)
        {
            Unit = Amount;
            this.Tick = Tick;
            isTargetEnemy = true;
        }

        internal override void ReLoad(double timer)
        {   // 리로드 없다.
            if (!ReLoaded)
            {
                ReLoaded = true;
            }
        }

        // cast의 경우는 몬스터에게 바로 debuff를 건다.
        internal override bool Shoot(double timer, string targetID)
        {
            if (timer > CASTINTERVAL && ReLoaded)
            {
                if (targetID != null)
                {
                    SetSpawnZone(targetID);
                    remainCount += Unit;
                    return true;
                }
                else if (targetID == null) { } //타겟이생길떄까지 대기한다. 
            }
            return false;
        }

        private void SetSpawnZone(string targetID)
        {
            SpawnCenter = Singleton<MonsterManager>.getInstance().GetMonster(targetID).CenterPosition;
            SpawnSize = Singleton<MonsterManager>.getInstance().GetMonster(targetID).Size;
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

        internal override void Update()
        {
            if (remainCount <= 0)
                return;

            timer += Time.deltaTime;
            while(timer > Tick)
            {
                LoadProjectile(prefabname);
                remainCount--;
                timer -= Tick;
            }
        }

        internal override void CalDamage(BigInteger elementaldmg)
        {
            DAMAGE = elementaldmg * DAMAGERATIO / (100 * this.Unit);
        }
    }

}