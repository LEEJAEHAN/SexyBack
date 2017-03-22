using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class MonsterStateReady : BaseState<Monster>
    {
        List<HitPlan> hitplans = new List<HitPlan>();
        List<Debuff> debuffs = new List<Debuff>();
        Queue<Debuff> finishDebuffs = new Queue<Debuff>();
        Vector3 debuffposition;

        struct HitPlan
        {
            public Vector3 hitWorldPosition;
            public BigInteger damage;
            public bool isCritical;

            public HitPlan(Vector3 pos, BigInteger dmg, bool critical)
            {
                hitWorldPosition = pos;
                damage = dmg;
                isCritical = critical;
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
            debuffposition = owner.CenterPosition + new Vector3(0, 0.2f, 0);
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

            if (!skillattack)
            {
                hitplans.Add(new HitPlan(hitWorldPosition, elemental.DAMAGE, false));
                return;
            }
            switch (elemental.skill.ability)
            {
                case DamageType.DeBuff:
                    {
                        AttachDebuff(SkillFactory.CreateDebuff(elemental.skill.debuff, elemental.skill.DAMAGE));
                        return;
                    }
                case DamageType.Hit:
                    {
                        hitplans.Add(new HitPlan(hitWorldPosition, elemental.skill.DAMAGE, true));
                        return;
                    }
                case DamageType.HitDebuff:
                    {
                        hitplans.Add(new HitPlan(hitWorldPosition, elemental.skill.DAMAGE, true));
                        AttachDebuff(SkillFactory.CreateDebuff(elemental.skill.debuff, elemental.skill.DAMAGE));
                        return;
                    }
                case DamageType.HitPerHPHigh:
                    {
                        int RemainHpPercent = Singleton<MonsterManager>.getInstance().GetHPPercent();
                        BigInteger Damage = elemental.skill.DAMAGE * (25 + RemainHpPercent) / 25; // 5~1배수 == * 100 / 4(1당4%)
                        hitplans.Add(new HitPlan(hitWorldPosition, Damage, true));
                        return;
                    }
                case DamageType.HitPerHPLow:
                    {
                        int RemainHpPercent = Singleton<MonsterManager>.getInstance().GetHPPercent();
                        BigInteger Damage = elemental.skill.DAMAGE * (125 - RemainHpPercent) / 25; // 1~5배수
                        hitplans.Add(new HitPlan(hitWorldPosition, Damage, true));
                        return;
                    }
            }
        }

        private void AttachDebuff(Debuff debuff)
        {
            debuffs.Add(debuff);
            SetMask(debuffs);
        }
        private void DetachDebuff(Debuff debuff)
        {
            debuffs.Remove(debuff);
            SetMask(debuffs);
        }
        private void SetMask(List<Debuff> debuffs)
        {
            int front = 0;
            foreach (Debuff d in debuffs)
            {
                front = Mathf.Max(front, (int)d.type);
            }
            owner.sprite.GetComponent<SpriteRenderer>().material.color = Debuff.GetMask((Debuff.Type)front);
            return;
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
                    hitplans.Add(new HitPlan(debuffposition, sum, false));
                }
                while (finishDebuffs.Count > 0)
                    DetachDebuff(finishDebuffs.Dequeue());
            }

            if (hitplans.Count == 0)
                return;


            foreach (HitPlan a in hitplans)
            {
                if (owner.Hit(a.hitWorldPosition, a.damage, a.isCritical) == false) //enumarator 돌고있을때 죽으면
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