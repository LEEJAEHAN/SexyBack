using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class HeroStateReady : HeroState
    {
        bool TouchTrigger = false;
        public HeroStateReady(HeroStateMachine stateMachine, Hero owner) : base(stateMachine, owner)
        {
            TouchTrigger = false;
        }

        internal override void Begin()
        {
            ViewLoader.hero_sprite.GetComponent<Animator>().SetBool("Move", false);
        }

        internal override void End()
        {

        }

        internal override void OnTouch(TapPoint pos)
        {
            if (!TouchTrigger && hero.AttackManager.CanMakePlan)
            {
                hero.AttackManager.MakeAttackPlan(pos);
                TouchTrigger = true;

                //sexybacklog.Console("Tap:"+pos.EffectPos);//WorldPos
            }
        }

        internal override void Update()
        {
            if (TouchTrigger)
            {
                TouchTrigger = false;
                //CheckMonster
                if (hero.targetID != null)
                {
                    stateMachine.ChangeState(new HeroStateAttack(stateMachine, hero));
                }
            }

        }
    }
}