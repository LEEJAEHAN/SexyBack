using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class HeroStateReady : BaseState<Hero>
    {
        bool nextstate = false;
        public HeroStateReady(Hero owner, HeroStateMachine stateMachine) : base(owner, stateMachine)
        {
        }

        internal override void Begin()
        {
            Singleton<GameInput>.getInstance().Action_TouchEvent += onTouch;
            owner.Animator.SetBool("Move", false);
        }

        internal override void End()
        {
            Singleton<GameInput>.getInstance().Action_TouchEvent -= onTouch;
        }

        internal override void Update()
        {
            if (Singleton<HeroManager>.getInstance().AutoAttack)
                onTouch(new TapPoint(ViewLoader.monsterbucket.transform.position, ViewLoader.monsterbucket.transform.position));

            if (nextstate)
            {
                stateMachine.ChangeState("Attack");
            }
        }
        internal void onTouch(TapPoint pos)
        {
            if (owner.TargetID != null)
            {
                if(owner.AttackManager.CanMakePlan)
                {
                    owner.AttackManager.MakeAttackPlan(pos);
                    nextstate = true;
                }
            }
        }
    }
}