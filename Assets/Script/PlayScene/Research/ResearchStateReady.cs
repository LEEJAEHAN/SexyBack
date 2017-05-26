using System;

namespace SexyBackPlayScene
{
    internal class ResearchStateReady : BaseState<Research>
    {
        InstanceStatus player = Singleton<InstanceStatus>.getInstance();
        ResearchManager manager = Singleton<ResearchManager>.getInstance();
        bool Instantbuy = false;
        //bool ThreadEmpty = false;
        //bool CanBuy { get { return player.EXP >= owner.StartPrice; } } 

        public ResearchStateReady(Research owner, StateMachine<Research> statemachine) : base(owner, statemachine)
        {
        }

        internal override void Begin()
        {
            owner.Panel.SetButton2(owner.Selected, false, "");
            Instantbuy = false;
            manager.Action_ThreadChange += this.onThreadEmpty;
            player.Action_ExpChange += this.onExpChange;
            Refresh();
            //onThreadEmpty(manager.CanUseThread);
            //onExpChange(player.EXP);
        }
        internal void onThreadEmpty(bool value)
        {
            //ThreadEmpty = value;
            owner.ViewRefresh();
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
            owner.ViewRefresh();
            //Refresh();
        }

        private void Refresh()
        {
            bool ThreadEmpty = manager.CanUseThread;
            bool CanBuy = player.EXP >= owner.StartPrice;

            InstantModeCheck();

            if (!Instantbuy)
            {
                owner.View.DrawRBar(0, (int)owner.ReducedTime, false);
                if (CanBuy && ThreadEmpty)
                    owner.View.Enable();
                else
                    owner.View.Disable();
                owner.Panel.SetButton1(owner.Selected, CanBuy && ThreadEmpty, true);
            }
            else
            {
                owner.View.HideRBar();
                if (CanBuy)
                    owner.View.Enable();
                else
                    owner.View.Disable();
                owner.Panel.SetButton1(owner.Selected, CanBuy, true);
            }

            if (!owner.Selected)
                return;

            owner.FillInfoView(Instantbuy);
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