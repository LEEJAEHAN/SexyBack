using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    
    internal class Research : IDisposable, IHasGridItem
    {
        ~Research()
        {
            sexybacklog.Console("리서치 제 ㅋ 거 ㅋ");
        }
        WeakReference owner;
        public string ID;
        string IconName;
        string InfoName;
        string Description;

        GridItem itemView;

        int RequireLevel;
        BigInteger StartPrice;
        BigInteger PricePerSec;
        int ResearchTime;
        public List<Bonus> bonuses;

        double RemainTime;
        float TickTimer = 0;
        float ResearchTick = 0.1f;
        //state flag
        bool Begin = false;
        bool Researching = false;
        bool End = false;
        bool Selected = false;
        bool Learn = false;

        // show, enable condition
        bool CanBuy = false;
        bool ShowCondition1 = false;
        bool ShowCondition2 = false;

        public Research(ResearchData data, ICanLevelUp root)
        {
            owner = new WeakReference(root);
            (owner.Target as ICanLevelUp).Action_LevelUp += onLevelUp;
                //root.Action_LevelUp += onLevelUp;

            ID = data.ID;
            bonuses = data.bonuses;
            StartPrice = new BigInteger(data.price);
            PricePerSec = new BigInteger(data.pot);
            RequireLevel = data.requeireLevel;
            ResearchTime = data.time;
            IconName = data.IconName;
            InfoName = data.InfoName;
            Description = data.InfoDescription;

            itemView = new GridItem("Research", ID, IconName, ViewLoader.Tab3Container, this); // avatar생성
            itemView.SetRBar(0, ResearchTime, false);
            itemView.SetActive(false);
            Refresh();

            Singleton<StageManager>.getInstance().Action_ExpChange += this.onExpChange;
        }

        public void Dispose()
        {
            Singleton<StageManager>.getInstance().Action_ExpChange -= this.onExpChange;
            (owner.Target as ICanLevelUp).Action_LevelUp -= onLevelUp;
            itemView.Dispose();
        }

        public void Update()
        {   // state machine
            if (Begin)
            {
                if (Singleton<StageManager>.getInstance().ExpUse(StartPrice))
                    Researching = true;
                Begin = false;
            }

            if (Researching)
            {
                StepResearch(Time.deltaTime);
                if (RemainTime <= 0)
                {
                    Researching = false;
                    End = true;
                    RemainTime = 0;
                }
            }

            if (End)
            {
                if (TryToUpgrade())
                {
                    End = false;
                    itemView.SetActive(false);
                    Singleton<ResearchManager>.getInstance().Destroy(ID);
                }
            }
        }

        private bool TryToUpgrade()
        {
            bool result = false;
            foreach (Bonus bonus in bonuses)
            {
                if (bonus.targetID == "hero") // target 타입으로 구분해야겠지만..
                    result = Singleton<HeroManager>.getInstance().Upgrade(bonus);
                else
                    result = Singleton<ElementalManager>.getInstance().Upgrade(bonus);
            }
            return result;
        }

        private void StepResearch(float deltaTime)
        {
            TickTimer += deltaTime;
            if (TickTimer >= ResearchTick)
            {
                bool result;
                if (result = Singleton<StageManager>.getInstance().ExpUse((PricePerSec * (int)(ResearchTick * 100)) / 100 )) //if (Singleton<StageManager>.getInstance().ExpUse(PricePerSec * (int)(tick * 10000) / 10000))
                    RemainTime -= ResearchTick;
                itemView.SetRBar((float)RemainTime / ResearchTime, (int)RemainTime, result);
                TickTimer -= ResearchTick;
            }
        }

 
        public void onSelect(string id)
        {
            if (id == null)
            {
                Selected = false;
                itemView.ClearInfo();
                return;
            }

            Selected = true;
            itemView.FillInfo(Selected, IconName, InfoName + Description);
            Refresh();
        }

        public void onConfirm(string id)
        {
            RemainTime = ResearchTime;
            Begin = true;
            itemView.SetRBar(1, (int)RemainTime, true);
            // TODO: confirm 버튼을 cancel로바꾼다? 혹은 비활성화한다. sell만남기고,
        }
        internal void onLevelUp(ICanLevelUp sender)
        {
            ShowCondition1 = sender.LEVEL >= RequireLevel;
            Refresh();
        }

        private void onExpChange(BigInteger exp)
        {
            CanBuy = exp > StartPrice;
            ShowCondition2 = exp > StartPrice / 2;
            Refresh();
        }

        public void Refresh()
        {
            if (CanBuy && !Researching)
                itemView.ConfirmEnable(Selected);
            else
                itemView.ConfirmDisable(Selected);

            if (!Researching)
            {
                if (CanBuy)
                    itemView.Enable();
                else
                    itemView.Disable();
            }
            else // resaerching
                itemView.Enable();

            if (!Learn && ShowCondition1 && ShowCondition2)
            {
                itemView.SetActive(true); // 한번 active되면 false되지않는다.
                Learn = true;
            }
        }
    }
}