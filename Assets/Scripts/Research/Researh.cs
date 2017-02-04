using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class Research : IDisposable
    {
        string ID;
        string IconName;
        string InfoName;
        string Description;

        GameObject avatar;
        public ResearchView view;

        BigInteger StartPrice;
        BigInteger PricePerSec;
        int ResearchTime;
        public List<Bonus> bonuses;

        double RemainTime;
        
        //state
        bool Begin = false;
        bool Researching = false;
        bool End = false;
        bool CanBuy = false;
        bool Selected = false;

        public Research(ResearchData data)
        {
            ID = data.ID;
            StartPrice = new BigInteger(data.price);
            PricePerSec = new BigInteger(data.pot);
            ResearchTime = data.time;
            IconName = data.IconName;
            InfoName = data.InfoName;
            Description = data.InfoDescription;

            InitAvatar();
            bonuses = data.bonuses;
            Singleton<StageManager>.getInstance().Action_ExpChange += this.onExpChange;
        }

        private void onExpChange(BigInteger exp)
        {
            if (exp >= StartPrice)
                CanBuy = true;
            else
                CanBuy = false;
            SetConfirmButtonState();
            SetIconButtonState(exp);
            SetActive(exp);
        }

        private void SetActive(BigInteger exp)
        {
            if (avatar.activeInHierarchy)
                return;

            if (exp > StartPrice / 2)
                ShowItem();
        }

        private void InitAvatar()
        {
            // Instantiate object
            GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/ResearchView") as GameObject;
            avatar = GameObject.Instantiate<GameObject>(prefab) as GameObject;
            view = avatar.GetComponent<ResearchView>();

            GameObject iconObject = view.transform.FindChild("Icon").gameObject;
            iconObject.GetComponent<UISprite>().atlas = Resources.Load("Atlas/IconImage", typeof(UIAtlas)) as UIAtlas;
            iconObject.GetComponent<UISprite>().spriteName = IconName;

            view.Action_ResearchStart += onConfirm;
            view.Action_ResearchSelect += onSelect; // 매니져가 안받는다.

            avatar.name = ID;

            SetRBar(0, ResearchTime, false);
            HideItem();
            //// delegate 붙이기
            //EventDelegate eventdel = new EventDelegate(view, "onItemSelect");
            //EventDelegate.Parameter p0 = new EventDelegate.Parameter();
            //p0.obj = itemView;
            //eventdel.parameters[0] = p0;
            //itemView.GetComponent<UIToggle>().onChange.Add(eventdel);
        }

        private void HideItem()
        {
            avatar.SetActive(false);
            avatar.transform.parent = ViewLoader.Item_Disable.transform;
            avatar.transform.localScale = ViewLoader.Item_Disable.transform.localScale;
            ViewLoader.Item_Enable.GetComponent<UIGrid>().Reposition();
        }

        private void SetRBar(float progress, int time, bool colorflag)
        {
            avatar.transform.FindChild("RBar").GetComponent<UIProgressBar>().value = progress;
            avatar.transform.FindChild("RBar").FindChild("RBar_Time").GetComponent<UILabel>().text = time.ToString() + " sec";
            if(colorflag)
                avatar.transform.FindChild("RBar").FindChild("RBar_Fill1").GetComponent<UISprite>().color = new Color(1,0,0,1);
            else
                avatar.transform.FindChild("RBar").FindChild("RBar_Fill1").GetComponent<UISprite>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }

        private void ShowItem()
        {
            avatar.SetActive(true);
            avatar.transform.parent = ViewLoader.Item_Enable.transform;
            avatar.transform.localScale = ViewLoader.Item_Enable.transform.localScale;
            ViewLoader.Item_Enable.GetComponent<UIGrid>().Reposition();
        }

        void onSelect(string id)
        {
            if(id == null)
            {
                Selected = false;
                ClearInfo();
                return;
            }


            Selected = true;
            ViewLoader.Info_Context.SetActive(true);
            FillInfoView();
            // TODO : 작성해야함
        }
        void ClearInfo()
        {
            if (ViewLoader.Info_Context.activeInHierarchy)
                ViewLoader.Info_Context.SetActive(false);
        }


        private void FillInfoView()
        { 
            if (!Selected)
                return;

            ViewLoader.Info_Icon.GetComponent<UISprite>().spriteName = IconName;
            ViewLoader.Info_Description.GetComponent<UILabel>().text = InfoName + Description;

            SetConfirmButtonState();
        }

        private void SetConfirmButtonState()
        { 
            if (!Selected)
                return;

            UIButton Confirm = ViewLoader.Button_Confirm.GetComponent<UIButton>();

            if (CanBuy && !Researching)
            {
                Confirm.enabled = true;
                Confirm.SetState(UIButtonColor.State.Normal, true);
            }
            else
            {
                Confirm.enabled = false;
                Confirm.SetState(UIButtonColor.State.Disabled, true);
            }
        }

        private void SetIconButtonState(BigInteger exp)
        {
            if(!Researching)
            {
                if (exp >= StartPrice)
                    avatar.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                else
                    avatar.GetComponent<UISprite>().color = new Color(0.5f, 0.5f, 0.5f, 0.8f);
            }
            else if (Researching)
            {
                avatar.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                if(exp >= PricePerSec)
                {

                }
               
            }
        }

        public void Update()
        {

            if(Begin)
            {
                if(Singleton<StageManager>.getInstance().ExpUse(StartPrice))
                    Researching = true;
                Begin = false;
            }

            if (Researching)
            {
                StepResearch(Time.deltaTime);
                if(RemainTime <= 0)
                {
                    Researching = false;
                    End = true;
                    RemainTime = 0;
                }
            }

            if(End)
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
            foreach(Bonus b in bonuses)
            {
                if (b.targetID == "hero")
                    result = Singleton<HeroManager>.getInstance().Upgrade(b);
                else
                    result = Singleton<ElementalManager>.getInstance().Upgrade(b);
            }
            return result;
        }

        void onConfirm(string id) // begin research
        {
            RemainTime = ResearchTime;
            Begin = true;
            SetRBar(1, (int)RemainTime, true);

            // TODO: confirm 버튼을 cancel로바꾼다? 혹은 비활성화한다. sell만남기고,
            // progressbar 보여야한다.

        }

        float TickTimer = 0;

        private void StepResearch(float deltaTime)
        {
            TickTimer += deltaTime;
            if (TickTimer >= 1)
            {
                TickTimer -= 1;
                bool result;
                if (result = Singleton<StageManager>.getInstance().ExpUse(PricePerSec)) //if (Singleton<StageManager>.getInstance().ExpUse(PricePerSec * (int)(tick * 10000) / 10000))
                    RemainTime -= 1;
                SetRBar((float)RemainTime / ResearchTime, (int)RemainTime, result);
            }

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}