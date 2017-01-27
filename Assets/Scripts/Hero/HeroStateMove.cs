using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class HeroStateMove : BaseState<Hero>
    {
        public HeroStateMove(Hero owner, HeroStateMachine stateMachine) : base(owner, stateMachine)
        {
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