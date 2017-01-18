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
        }

        internal override void End()
        {
        }

        internal override void OnTouch()
        {
            owner.SetSwordEffectPosition();
            TouchTrigger = true;
        }

        internal override void Update()
        {
            if (TouchTrigger)
            {
                TouchTrigger = false;
                //CheckMonster
                if (owner.targetID != null)
                {
                    stateMachine.ChangeState(new HeroAttackState(stateMachine, owner));
                }
            }

        }
    }
}