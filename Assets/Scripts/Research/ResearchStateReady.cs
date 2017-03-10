using System;

namespace SexyBackPlayScene
{
    internal class ResearchStateReady : BaseState<Research>
    {
        StatManager player = Singleton<StatManager>.getInstance();
        ResearchManager manager = Singleton<ResearchManager>.getInstance();
        bool Instantbuy = false;
        bool ThreadEmpty = false;
        bool CanBuy { get { return player.EXP >= owner.StartPrice; } } 

        public ResearchStateReady(Research owner, StateMachine<Research> statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
            owner.itemView.SetActive(true);
            owner.Panel.SetButton2(owner.Selected, false, "");

            manager.DrawNewMark();
            Instantbuy = false;
            manager.Action_ThreadChange += this.onThreadEmpty;
            onThreadEmpty(manager.CanUseThread);
            player.Action_ExpChange += this.onExpChange;
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
        }

        private void onExpChange(BigInteger exp)
        {
            if (owner.Purchase)
                return;
            Refresh();
        }

        private void Refresh()
        {
            InstantModeCheck();

            if (!Instantbuy)
                owner.itemView.ShowRBar(0, (int)owner.ReducedTime, false);
            else
                owner.itemView.HideRBar();

            if (CanBuy && ThreadEmpty)
                owner.itemView.Enable();
            else
                owner.itemView.Disable();

            if (!owner.Selected)
                return;

            owner.FillInfoView(Instantbuy);
            owner.Panel.SetButton1(owner.Selected, CanBuy && ThreadEmpty, true);
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
                owner.Panel.SetButton1(owner.Selected, false, false);
                if (Instantbuy)
                {
                    if(player.ExpUse(owner.StartPrice,false))
                    {
                        owner.DoUpgrade();
                        stateMachine.ChangeState("Destroy");
                    }
                    else
                        owner.Purchase = false;
                }
                else
                {
                    if (player.ExpUse(owner.StartPrice,false))
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