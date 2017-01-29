using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class MonsterStateFlying: BaseState<Monster>
    {
        public MonsterStateFlying(Monster owner, MonsterStateMachine statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
            owner.avatar.GetComponent<MonsterView>().Fly();
        }

        internal override void End()
        {

        }

        internal override void Update()
        {
            stateMachine.ChangeState("Death");
        }
    }
}