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

            owner.avatar.transform.parent.GetComponent<Rigidbody>().velocity = new Vector3(2, 2, 0);
            owner.avatar.transform.parent.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 5);
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