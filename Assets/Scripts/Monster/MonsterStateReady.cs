using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class MonsterStateReady: BaseState<Monster>
    {
        List<HitPlan> hitplans = new List<HitPlan>();
        struct HitPlan
        {
            public Vector3 hitWorldPosition;
            public BigInteger damage;

            public HitPlan(Vector3 pos, BigInteger dmg)
            {
                hitWorldPosition = pos;
                damage = dmg;
            }
        }

        public MonsterStateReady(Monster owner, MonsterStateMachine statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
            owner.sprite.GetComponent<Animator>().SetTrigger("Ready");
            owner.avatar.GetComponent<MonsterView>().Action_HitEvent += onHitByProjectile;
            BackCollision.Action_HitEvent += onHitByProjectile;
        }

        internal override void End()
        {
            owner.avatar.GetComponent<MonsterView>().Action_HitEvent -= onHitByProjectile;
            BackCollision.Action_HitEvent -= onHitByProjectile;
        }

        public void onHitByProjectile(Vector3 hitWorldPosition, string elementalID)
        {   // view를 통해서 받은것.
            BigInteger damage = Singleton<ElementalManager>.getInstance().GetElementalDamage(elementalID);
            hitplans.Add(new HitPlan(hitWorldPosition, damage));
        }

        internal override void Update()
        {
            if (hitplans.Count == 0)
                return;

            foreach(HitPlan a in hitplans)
            {
                if (owner.Hit(a.hitWorldPosition, a.damage, false) == false) // enumarator 돌고있을때 죽으면
                    break;
            }
            hitplans.Clear();
        }
    }
}