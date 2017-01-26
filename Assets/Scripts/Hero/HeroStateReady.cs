using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class HeroStateReady : BaseState<Hero>
    {
        public HeroStateReady(Hero owner, HeroStateMachine stateMachine) : base(owner, stateMachine)
        {
            sexybacklog.Console("HeroStateReady 생성");
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
        }
        internal void onTouch(TapPoint pos)
        {   
            if (owner.targetID != null)
            {
                if(owner.AttackManager.CanMakePlan)
                {
                    owner.AttackManager.MakeAttackPlan(pos);
                    stateMachine.ChangeState("Attack");
                }
            }
        }
        ~HeroStateReady()
        {
            sexybacklog.Console("HeroStateReady 소멸");
        }

    }
}