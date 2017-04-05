using System;

namespace SexyBackPlayScene
{
    internal class ResearchStateDestroy : BaseState<Research>
    {
        public ResearchStateDestroy(Research owner, StateMachine<Research> statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
            owner.itemView.SetActive(false);
            Singleton<ResearchManager>.getInstance().Destory(owner.GetID);

        }

        internal override void End()
        {
        }

        internal override void Update()
        {
        }
    }
}