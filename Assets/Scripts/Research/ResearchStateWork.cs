using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class ResearchStateWork : BaseState<Research>
    {
        double TickTimer = 0;

        public ResearchStateWork(Research owner, StateMachine<Research> statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
            owner.itemView.Enable();
            //owner.itemView.ShowRBar((float)owner.RemainTime / (float)owner.ResearchTime, (int)owner.RemainTime, true);
            Singleton<InfoPanel>.getInstance().SetPauseButton(owner.Selected, true, "Pause");
        }

        internal override void End()
        {
        }

        internal override void Update()
        {
            if (owner.RemainTime <= 0)
            {
                owner.DoUpgrade();
                stateMachine.ChangeState("Destroy");
            }
            else
            {
                TickTimer += Time.deltaTime;
                double mintick = Math.Min(owner.RemainTime, Research.ResearchTick);
                if (TickTimer >= mintick)
                {
                    bool result;
                    if (result = Singleton<Player>.getInstance().ExpUse((owner.PricePerSec * (int)(mintick * 100)) / 100)) //if (Singleton<StageManager>.getInstance().ExpUse(PricePerSec * (int)(tick * 10000) / 10000))
                        owner.RemainTime -= TickTimer;
                    owner.itemView.ShowRBar((float)owner.RemainTime / (float)owner.ReducedTime, (int)owner.RemainTime, result);
                    TickTimer -= Research.ResearchTick;
                }
            }
        }
    }
}