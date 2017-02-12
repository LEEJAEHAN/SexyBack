using System;

namespace SexyBackPlayScene
{
    internal class ResearchStateReady : BaseState<Research>
    {
        bool Instantbuy = false;

        public ResearchStateReady(Research owner, StateMachine<Research> statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
            Instantbuy = false;
            Singleton<Player>.getInstance().Action_ExpChange += this.onExpChange;
            owner.itemView.ShowRBar(0, (int)owner.ReducedTime, false);
        }

        internal override void End()
        {
            Singleton<Player>.getInstance().Action_ExpChange -= this.onExpChange;
            Singleton<InfoPanel>.getInstance().SetConfirmButton(owner.Selected, false);
        }

        private void onExpChange(BigInteger exp)
        {
            if (owner.Purchase)
                return;

            if (exp >= owner.StartPrice)
            {
                Singleton<InfoPanel>.getInstance().SetConfirmButton(owner.Selected, true);
                owner.itemView.Enable();
            }
            else
            {
                Singleton<InfoPanel>.getInstance().SetConfirmButton(owner.Selected, false);
                owner.itemView.Disable();
            }
        }

        internal override void Update()
        {
            if (!Instantbuy && owner.ReducedTime <= Research.ResearchTick) // instant buy mode
            {
                Instantbuy = true;
                owner.StartPrice = owner.StartPrice + owner.ResearchPrice;
                owner.ResearchPrice = 0;
                owner.ReducedTime = 0;
                owner.PricePerSec = 0;
                owner.itemView.HideRBar();
            }

            if (owner.Purchase)
            {
                if(Instantbuy)
                {
                    if(Singleton<Player>.getInstance().ExpUse(owner.StartPrice))
                    {
                        owner.DoUpgrade();
                        stateMachine.ChangeState("Destroy");
                    }
                    else
                        owner.Purchase = false;
                }
                else
                {
                    if (Singleton<Player>.getInstance().ExpUse(owner.StartPrice))
                    {
                        owner.RemainTime = owner.ReducedTime;
                        stateMachine.ChangeState("Work");
                    }
                    else
                        owner.Purchase = false;
                }
            }

        }
    }
}