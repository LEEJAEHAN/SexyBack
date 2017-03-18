using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class MonsterStateReady: BaseState<Monster>
    {
        List<HitPlan> hitplans = new List<HitPlan>();
        List<Debuff> debuffs = new List<Debuff>();

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
                return;
            }
            switch(elemental.skill.ability)
            {
                case DamageType.DeBuff:
                    {
                        //elemental.skill.debuff
                        //debuffs.Apply();
                        return;
                    }
                case DamageType.Hit:
                    {
                        hitplans.Add(new HitPlan(hitWorldPosition, elemental.skill.DAMAGE));
                        return;
                    }
                case DamageType.HitDebuff:
                    {
                        return;
                    }
                case DamageType.HitPerHPHigh:
                    {
                        return;
                    }
                case DamageType.HitPerHPLow:
                    {
                        return;
                    }
            }
        }

        internal override void Update()
        {
            if (hitplans.Count == 0)
                return;

            //
            //buff loop
            //


            foreach(HitPlan a in hitplans)
            {
                if (owner.Hit(a.hitWorldPosition, a.damage, false) == false) // enumarator 돌고있을때 죽으면
                    break;
                // if ( scalebyhp )
                // 
            }

            foreach(Debuff debuff in debuffs)
            {
                debuff.Update();
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