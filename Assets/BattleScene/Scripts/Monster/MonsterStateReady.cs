using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class MonsterStateReady: BaseState<Monster>
    {
        List<HitPlan> hitplans = new List<HitPlan>();
        List<Debuff> debuffs = new List<Debuff>();
        Queue<Debuff> finishDebuffs= new Queue<Debuff>();

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
                        debuffs.Add(SkillFactory.CreateDebuff(elemental.skill.debuff, elemental.skill.DAMAGE));
                        return;
                    }
                case DamageType.Hit:
                    {
                        hitplans.Add(new HitPlan(hitWorldPosition, elemental.skill.DAMAGE));
                        return;
                    }
                case DamageType.HitDebuff:
                    {
                        hitplans.Add(new HitPlan(hitWorldPosition, elemental.skill.DAMAGE));
                        debuffs.Add(SkillFactory.CreateDebuff(elemental.skill.debuff, elemental.skill.DAMAGE));
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
            if (debuffs.Count > 0)
            {
                debuffTimer += Time.deltaTime;
                if (debuffTimer > tick)
                {
                    debuffTimer -= tick;
                    BigInteger sum = new BigInteger(0);
                    foreach (Debuff debuff in debuffs)
                    {
                        sum += debuff.PopOneTickDamage();
                        if (debuff.CheckEnd())
                            finishDebuffs.Enqueue(debuff);
                    }
                    hitplans.Add(new HitPlan(owner.CenterPosition, sum));
                }
                while (finishDebuffs.Count > 0)
                    debuffs.Remove(finishDebuffs.Dequeue());
            }

            if (hitplans.Count == 0)
                return;

            
            foreach (HitPlan a in hitplans)
            {
                if (owner.Hit(a.hitWorldPosition, a.damage, false) == false) //enumarator 돌고있을때 죽으면
                    break;
            }
            hitplans.Clear();

            // 1초마다
            // sum = foreach(도트합산) { remaintime-1.}
            // hit(sum, noposition, 칼라오버레이dotteffect);
            // remaintime == 0 이면 dotplan에서 뺌.

        }

        double debuffTimer;
        double tick = 1f;
    }
}