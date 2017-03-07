using System;

namespace SexyBackPlayScene
{
    internal class ResearchStateNone : BaseState<Research>
    {

        public ResearchStateNone(Research owner, StateMachine<Research> statemachine) : base(owner, statemachine)
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