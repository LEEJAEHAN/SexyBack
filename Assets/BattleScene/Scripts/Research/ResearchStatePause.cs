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
            owner.Panel.SetButton2(owner.Selected, true, "재개");
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
            owner.Panel.SetButton2(owner.Selected, true, "재개");
            owner.Panel.SetButton1(owner.Selected, false, false); // 중복입력 막는다.
        }
    }
}