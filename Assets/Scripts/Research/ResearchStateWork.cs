using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class ResearchStateWork : BaseState<Research>
    {
        double TickTimer = 0;
        bool Result = true;
        bool InstantFinish = false;

        public ResearchStateWork(Research owner, StateMachine<Research> statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
            owner.itemView.Enable();
            owner.Action_InstantFinish += onInstantFinish;
            Refresh();
        }

        private void Refresh()
        {
            owner.itemView.ShowRBar((float)owner.RemainTime / (float)owner.ReducedTime, (int)owner.RemainTime, Result);

            if (!owner.Selected)
                return;

            owner.FillInfoView(false);
            Singleton<InfoPanel>.getInstance().SetConfirmButton(owner.Selected, false);
            Singleton<InfoPanel>.getInstance().SetPauseButton(owner.Selected, true, "Pause");
        }

        internal override void End()
        {
            owner.Action_InstantFinish -= onInstantFinish;
        }

        void onInstantFinish()
        {
            InstantFinish = true;
        }

        internal override void Update()
        {
            if (owner.RefreshFlag)
            {
                Refresh();
                owner.RefreshFlag = false;
            }

            if (owner.RemainTime <= 0 || InstantFinish)
            {
                owner.DoUpgrade();
                Singleton<ResearchManager>.getInstance().UseThread(false);
                stateMachine.ChangeState("Destroy");
            }
            else
            {
                TickTimer += Time.deltaTime;
                double mintick = Math.Min(owner.RemainTime, owner.ResearchTick);
                if (TickTimer >= mintick)
                {
                    if (Result = Singleton<StatManager>.getInstance().ExpUse((owner.PricePerSec * (int)(mintick * 100)) / 100)) //if (Singleton<StageManager>.getInstance().ExpUse(PricePerSec * (int)(tick * 10000) / 10000))
                        owner.RemainTime -= TickTimer;
                    owner.itemView.ShowRBar((float)owner.RemainTime / (float)owner.ReducedTime, (int)owner.RemainTime, Result);
                    TickTimer -= owner.ResearchTick;                    
                }
            }
        }
    }
}