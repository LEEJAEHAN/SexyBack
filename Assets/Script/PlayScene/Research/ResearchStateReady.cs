using System;

namespace SexyBackPlayScene
{
    internal class ResearchStateReady : BaseState<Research>
    {
        InstanceStatus player = Singleton<InstanceStatus>.getInstance();
        ResearchManager manager = Singleton<ResearchManager>.getInstance();
        ICanLevelUp target;
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

            if (owner.baseData.requireID == "hero")
                target = Singleton<HeroManager>.getInstance().GetHero();
            else if (Singleton<ElementalManager>.getInstance().elementals.ContainsKey(owner.baseData.requireID))
                target = Singleton<ElementalManager>.getInstance().elementals[owner.baseData.requireID];

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
            bool CanStart = (target == null ? true : target.GetLevel >= owner.baseData.Level);

            InstantModeCheck();

            if (!Instantbuy)
            {
                owner.View.DrawRBar(0, (int)owner.ReducedTime, false);
                if (CanBuy && CanStart && ThreadEmpty)
                    owner.View.Enable();
                else
                    owner.View.Disable();
            }
            else
            {
                owner.View.HideRBar();
                if (CanBuy && CanStart)
                    owner.View.Enable();
                else
                    owner.View.Disable();
            }


            // 여기서부터는 하단뷰. ( 선택한것만 )
            if (!owner.Selected)
                return;
            if (!Instantbuy)
                owner.Panel.SetButton1(owner.Selected, CanBuy && ThreadEmpty, CanStart);
            else
                owner.Panel.SetButton1(owner.Selected, CanBuy, CanStart);
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