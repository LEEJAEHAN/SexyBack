using System;

namespace SexyBackPlayScene
{
    internal class ResearchStateReady : BaseState<Research>
    {
        Player player = Singleton<Player>.getInstance();
        ResearchManager manager = Singleton<ResearchManager>.getInstance();
        bool Instantbuy = false;
        bool ThreadEmpty = false;
        bool CanBuy = false;

        public ResearchStateReady(Research owner, StateMachine<Research> statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
            manager.DrawNewMark();
            Instantbuy = false;
            player.Action_ExpChange += this.onExpChange;
            onExpChange(player.EXP);
            manager.Action_ThreadChange += this.onThreadEmpty;
            onThreadEmpty(manager.CanUseThread);
            Singleton<InfoPanel>.getInstance().SetPauseButton(owner.Selected, false, "");
        }
        internal void onThreadEmpty(bool value)
        {
            ThreadEmpty = value;
            Refresh();
        }

        internal override void End()
        {
            player.Action_ExpChange -= this.onExpChange;
            manager.Action_ThreadChange -= this.onThreadEmpty;
            player = null;
            manager = null;
            Singleton<InfoPanel>.getInstance().SetConfirmButton(owner.Selected, false);
        }

        private void onExpChange(BigInteger exp)
        {
            if (owner.Purchase)
                return;

            CanBuy = (exp >= owner.StartPrice);
            Refresh();
        }

        private void Refresh()
        {
            if (!Instantbuy)
                InstantModeCheck();

            if (!Instantbuy)
                owner.itemView.ShowRBar(0, (int)owner.ReducedTime, false);
            else
                owner.itemView.HideRBar();

            if(CanBuy && ThreadEmpty)
                owner.itemView.Enable();
            else
                owner.itemView.Disable();

            if (!owner.Selected)
                return;

            owner.FillInfoView(Instantbuy);
            Singleton<InfoPanel>.getInstance().SetConfirmButton(owner.Selected, CanBuy && ThreadEmpty);
        }

        private void InstantModeCheck()
        {
            if (owner.ReducedTime <= 1) // instant buy mode
            {
                Instantbuy = true;
                owner.StartPrice = owner.StartPrice + owner.ResearchPrice;
                owner.ResearchPrice = 0;
                owner.ReducedTime = 0;
                owner.PricePerSec = 0;
            }
        }

        internal override void Update()
        {
            if (owner.RefreshFlag)
            {
                Refresh();
                owner.RefreshFlag = false;
            }
            if (owner.Purchase)
            {
                if(Instantbuy)
                {
                    if(player.ExpUse(owner.StartPrice))
                    {
                        owner.DoUpgrade();
                        stateMachine.ChangeState("Destroy");
                    }
                    else
                        owner.Purchase = false;
                }
                else
                {
                    if (player.ExpUse(owner.StartPrice))
                    {
                        owner.RemainTime = owner.ReducedTime;
                        manager.UseThread(true);
                        stateMachine.ChangeState("Work");
                    }
                    else
                        owner.Purchase = false;
                }
            }
        }
    }
}