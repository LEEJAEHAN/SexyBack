using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class HeroStateFastMove : BaseState<Hero>
    {
        public HeroStateFastMove(Hero owner, HeroStateMachine stateMachine) : base(owner, stateMachine)
        {
        }
        internal override void Begin()
        {
            owner.Animator.SetBool("Move", true);
            owner.Animator.speed = 2f;
        }

        internal override void End()
        {
            owner.Animator.speed = 1f;
        }

        internal override void Update()
        {
            owner.FakeMove(owner.MoveSpeed * 10 * Time.deltaTime);
        }
    }
}