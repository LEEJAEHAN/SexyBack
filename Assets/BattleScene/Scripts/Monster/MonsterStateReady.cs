using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class MonsterStateReady: BaseState<Monster>
    {
        List<HitPlan> hitplans = new List<HitPlan>();
        List<HitPlan> dotplans = new List<HitPlan>();

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

        public void onHitByProjectile(Vector3 hitWorldPosition, string elementalID, bool skillattack)
        {   // view를 통해서 받은것.
            Elemental elemental = Singleton<ElementalManager>.getInstance().elementals[elementalID];
//            Skill skill = elemental.skill;

            if(!skillattack)
            {
                hitplans.Add(new HitPlan(hitWorldPosition, elemental.DAMAGE));
            }
            else if(skillattack)
            {
             //   skill.Apply(owner , this);
            }
        }

        internal override void Update()
        {
            if (hitplans.Count == 0)
                return;

            foreach(HitPlan a in hitplans)
            {
                if (owner.Hit(a.hitWorldPosition, a.damage, false) == false) // enumarator 돌고있을때 죽으면
                    break;
                // if ( scalebyhp )
                // 
            }

            // if ( burn)
            // if ( poisoned)
            // 1초마다
            // sum = foreach(도트합산) { remaintime-1.}
            // hit(sum, noposition, 칼라오버레이dotteffect);
            // remaintime == 0 이면 dotplan에서 뺌.

            hitplans.Clear();
        }
    }
}