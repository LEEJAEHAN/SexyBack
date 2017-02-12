using System;

namespace SexyBackPlayScene
{
    internal class ResearchStatePause : BaseState<Research>
    {
        public ResearchStatePause(Research owner, StateMachine<Research> statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
            Singleton<InfoPanel>.getInstance().SetPauseButton(owner.Selected, true, "Play");
            owner.itemView.ShowRBar((float)owner.RemainTime / (float)owner.ReducedTime, (int)owner.RemainTime, false);
        }

        internal override void End()
        {
        }

        internal override void Update()
        {
        }
    }
}