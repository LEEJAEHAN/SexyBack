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
            owner.SetActionTrigger("Appear");
            sexybacklog.Console("Appear Begin");
        }

        internal override void End()
        {
            owner.SetActionTrigger("Ready");
            sexybacklog.Console("Appear End");
        }

        internal override void Update()
        {
        }
    }
}