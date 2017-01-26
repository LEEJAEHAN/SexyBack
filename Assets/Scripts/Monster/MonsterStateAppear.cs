using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class MonsterStateAppear : BaseState<Monster>
    {
        public MonsterStateAppear(Monster owner, MonsterStateMachine statemachine) : base(owner, statemachine)
        {

        }

        internal override void Begin()
        {
            owner.Animator.SetTrigger("Appear");
            sexybacklog.Console("Appear Begin");
        }

        internal override void End()
        {
            owner.Animator.SetTrigger("Ready");
            sexybacklog.Console("Appear End");
        }

        internal override void Update()
        {
        }
    }
}