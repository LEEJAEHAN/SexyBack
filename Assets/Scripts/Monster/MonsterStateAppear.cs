using System;

namespace SexyBackPlayScene
{
    internal class MonsterStateAppear : BaseState<Monster>
    {
        public MonsterStateAppear(Monster owner, MonsterStateMachine statemachine) : base(owner, statemachine)
        {

        }

        internal override void Begin()
        {
        }

        internal override void End()
        {
        }

        internal override void Update()
        {
            stateMachine.ChangeState("Ready");
        }
    }
}