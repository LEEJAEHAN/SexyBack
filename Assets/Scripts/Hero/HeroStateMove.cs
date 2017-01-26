using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class HeroStateMove : BaseState<Hero>
    {
        bool TouchTrigger = false;
        public HeroStateMove(Hero owner, HeroStateMachine stateMachine) : base(owner, stateMachine)
        {
            TouchTrigger = false;
        }

        internal override void Begin()
        {
            owner.Animator.SetBool("Move", true);
        }

        internal override void End()
        {

        }

        internal override void Update()
        {

        }
    }
}