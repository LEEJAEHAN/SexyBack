using System;

namespace SexyBackPlayScene
{
    internal class ResearchStateNone : BaseState<Research>
    {
        bool ShowCondition1 = false;

        public ResearchStateNone(Research owner, StateMachine<Research> statemachine) : base(owner, statemachine)
        {
        }

        internal void onLevelUp(ICanLevelUp sender)
        {
            ShowCondition1 = sender.LEVEL >= owner.RequireLevel;
        }

        internal override void Begin()
        {
            (owner.owner.Target as ICanLevelUp).Action_LevelUpInfoChange += this.onLevelUp;
            owner.itemView.SetActive(false);
        }

        internal override void End()
        {
            owner.itemView.SetActive(true); // 한번 active되면 false되지않는다.
            (owner.owner.Target as ICanLevelUp).Action_LevelUpInfoChange -= this.onLevelUp;
        }

        internal override void Update()
        {
            if (ShowCondition1)
            {
                stateMachine.ChangeState("Ready");
            }
        }
    }
}