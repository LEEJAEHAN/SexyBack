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

        int ResearchTime; // 
        BigInteger StartPrice = new BigInteger(1); // 
        BigInteger PricePerSec = new BigInteger(1); // 
        public List<Bonus> bonuses;

        public int SortOrder;
        double RemainTime;
        float TickTimer = 0;
        float ResearchTick = 1f;

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
            (owner.Target as ICanLevelUp).Action_LevelUpInfoChange += onLevelUp;
            //root.Action_LevelUp += onLevelUp;

            ID = data.ID;
            bonuses = data.bonuses;
            RequireLevel = data.requeireLevel;
            IconName = data.IconName;
            InfoName = data.InfoName;
            Description = data.InfoDescription;
            SortOrder = data.level + data.baselevel;

            FillPrice(data.level, data.baselevel, data.baseprice, data.rate, data.basetime);

            itemView = new GridItem("Research", ID, IconName, ViewLoader.Tab3Container, this); // avatar생성
            itemView.SetRBar(0, ResearchTime, false);
            itemView.SetActive(false);
            Refresh();

            Singleton<StageManager>.getInstance().Action_ExpChange += this.onExpChange;
        }

        private void FillPrice(int level, int baselevel, int baseprice, int rate, int basetime)
        { // TODO : 중요한공식
            int reallevel = level + baselevel;

            int baseDamageRateXK = 1000 + 5 * baselevel; // 원래 double값은 1 + baselevel/20;

            BigInteger TotalPrice = BigInteger.PowerByGrowth(baseDamageRateXK * baseprice, reallevel, ResearchData.GrowthRate) / 1000;

            BigInteger ResarchPrice = rate * TotalPrice / 100;
            StartPrice = ((100 - rate) * TotalPrice) / 100;

            double growth = Math.Pow(ResearchData.TimeGrothRate, reallevel - 40); // 40층 부터 기준, 그전까진 지수적감소
            double bonus = (double)reallevel / 30;  // rearlevel * 30(기준연구시간 / 900 , bonus는 0~30초까지.

            double time = growth * (30 + bonus); // growth는 2의 3승까지는 업그레이드로 감소시킬수 있으므로, 8 * ( 30 + bonus ). 240~480초

            ResearchTime = (int)time;
            PricePerSec = ResarchPrice / ResearchTime;
        }

        public void Dispose()
        {
            Singleton<StageManager>.getInstance().Action_ExpChange -= this.onExpChange;
            (owner.Target as ICanLevelUp).Action_LevelUpInfoChange -= onLevelUp;
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
                DoUpgrade();
                End = false;
                itemView.SetActive(false);
                Singleton<ResearchManager>.getInstance().Destroy(ID);
            }
        }

        private void DoUpgrade()
        {
            foreach (Bonus bonus in bonuses)
                Singleton<Player>.getInstance().Upgrade(bonus);
        }

        private void StepResearch(float deltaTime)
        {
            TickTimer += deltaTime;
            if (TickTimer >= ResearchTick)
            {
                bool result;
                if (result = Singleton<StageManager>.getInstance().ExpUse((PricePerSec * (int)(ResearchTick * 100)) / 100)) //if (Singleton<StageManager>.getInstance().ExpUse(PricePerSec * (int)(tick * 10000) / 10000))
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
        {  // 중복입력 막는다.
            if (Begin || Researching || End)
                return;
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