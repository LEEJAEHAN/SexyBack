using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class ResearchStateWork : BaseState<Research>
    {
        double TickTimer = 0;
        bool Result = true;

        public ResearchStateWork(Research owner, StateMachine<Research> statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
            owner.View.Enable();
            Refresh();
        }

        private void Refresh()
        {
            owner.View.DrawRBar((float)owner.RemainTime / (float)owner.ReducedTime, (int)owner.RemainTime, Result);

            if (!owner.Selected)
                return;

            owner.FillInfoView(false);
            owner.Panel.SetButton2(owner.Selected, true, "일시정지");
            owner.Panel.SetButton1(owner.Selected, false, false); // 중복입력 막는다.
        }

        internal override void End()
        {
        }
        
        internal override void Update()
        {
            if (owner.RefreshFlag)
            {
                Refresh();
                owner.RefreshFlag = false;
            }

            if (owner.RemainTime <= 0)
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
                    if (Result = Singleton<InstanceStatus>.getInstance().ExpUse((owner.PricePerSec * (int)(mintick * 100)) / 100, false)) //if (Singleton<StageManager>.getInstance().ExpUse(PricePerSec * (int)(tick * 10000) / 10000))
                        owner.RemainTime -= TickTimer;
                    owner.View.DrawRBar((float)owner.RemainTime / (float)owner.ReducedTime, (int)owner.RemainTime, Result);
                    TickTimer -= owner.ResearchTick;                    
                }
            }
        }
    }
}