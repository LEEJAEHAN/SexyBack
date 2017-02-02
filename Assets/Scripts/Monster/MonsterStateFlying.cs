using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class MonsterStateFlying: BaseState<Monster>
    {
        float flyingTime = 3.75f;

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
            flyingTime -= Time.deltaTime;
            if(flyingTime <= 0)
                stateMachine.ChangeState("Death");
        }
    }
}