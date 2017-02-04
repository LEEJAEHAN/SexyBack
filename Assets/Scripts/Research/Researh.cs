using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class Research : IDisposable, IHasGridItem
    {
        string ID;
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

        //state flag
        bool Begin = false;
        bool Researching = false;
        bool End = false;
        bool Selected = false;

        // show, enable condition
        bool CanBuy = false;
        bool ShowCondition1 = false;
        bool ShowCondition2 = false;

        public Research(ResearchData data)
        {
            ID = data.ID;
            bonuses = data.bonuses;
            StartPrice = new BigInteger(data.price);
            PricePerSec = new BigInteger(data.pot);
            RequireLevel = data.requeireLevel;
            ResearchTime = data.time;
            IconName = data.IconName;
            InfoName = data.InfoName;
            Description = data.InfoDescription;

            itemView = new GridItem("Research", ID, IconName, this); // avatar생성
            itemView.SetRBar(0, ResearchTime, false);
            itemView.Hide();

            Singleton<StageManager>.getInstance().Action_ExpChange += this.onExpChange;
        }

        public void Update()
        {
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
                    Dispose();
                }
            }
        }

        private bool TryToUpgrade()
        {
            bool result = false;
            foreach (Bonus bonus in bonuses)
            {
                if (bonus.targetID == "hero")
                    result = Singleton<HeroManager>.getInstance().Upgrade(bonus);
                else
                    result = Singleton<ElementalManager>.getInstance().Upgrade(bonus);
            }
            return result;
        }

        private void StepResearch(float deltaTime)
        {
            TickTimer += deltaTime;
            if (TickTimer >= 1)
            {
                TickTimer -= 1;
                bool result;
                if (result = Singleton<StageManager>.getInstance().ExpUse(PricePerSec)) //if (Singleton<StageManager>.getInstance().ExpUse(PricePerSec * (int)(tick * 10000) / 10000))
                    RemainTime -= 1;
                itemView.SetRBar((float)RemainTime / ResearchTime, (int)RemainTime, result);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
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
            UpdateInfoView();
        }

        public void onConfirm(string id)
        {
            RemainTime = ResearchTime;
            Begin = true;
            itemView.SetRBar(1, (int)RemainTime, true);
            // TODO: confirm 버튼을 cancel로바꾼다? 혹은 비활성화한다. sell만남기고,
        }
        internal void onElementalChange(Elemental sender)
        {
            ShowCondition1 = sender.LEVEL >= RequireLevel;
            CheckShow();
        }

        internal void onHeroChange(Hero hero)
        {
            ShowCondition1 = hero.LEVEL >= RequireLevel;
            CheckShow();
        }
        private void CheckShow()
        {
            if (itemView.Active == false && ShowCondition1 && ShowCondition2)
                itemView.Show();
        }

        private void onExpChange(BigInteger exp)
        {
            CanBuy = exp > StartPrice;
            UpdateInfoView();
            UpdateItemView();

            ShowCondition2 = exp > StartPrice / 2;
            CheckShow();
        }

        private void UpdateInfoView()
        {
            if (CanBuy && !Researching)
                itemView.ConfirmEnable(Selected);
            else
                itemView.ConfirmDisable(Selected);
        }
        private void UpdateItemView()
        {
            if (!Researching)
            {
                if (CanBuy)
                    itemView.Enable();
                else
                    itemView.Disable();
            }
            else // resaerching
            {
                itemView.Enable();
            }
        }


    }
}