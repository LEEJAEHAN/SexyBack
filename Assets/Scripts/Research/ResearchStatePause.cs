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
            Singleton<InfoPanel>.getInstance().SetConfirmButton(owner.Selected, false);
            Refresh();
        }

        internal override void End()
        {
        }

        internal override void Update()
        {
            if(owner.RefreshFlag)
            {
                Refresh();
                owner.RefreshFlag = false;
            }
        }

        private void Refresh()
        {
            owner.itemView.ShowRBar((float)owner.RemainTime / (float)owner.ReducedTime, (int)owner.RemainTime, false);

            if (!owner.Selected)
                return;
            owner.FillInfoView(false);
            Singleton<InfoPanel>.getInstance().SetPauseButton(owner.Selected, true, "Work");
        }
    }
}