using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SexyBackPlayScene
{
    public class LevelUpViewController : MonoBehaviour
    {
        //LevelUpItem은 이 ui에서 나타날 수 있는 것을 포함한 객체;

        // this class is event Publisher
        public delegate void ConfirmButton_Event_Handler();
        public event ConfirmButton_Event_Handler noticeConfirm;

        public delegate void SelectEvent_Handler(string id);
        public event SelectEvent_Handler noticeSelect;

        // for view 
        List<GameObject> ItemViewList = new List<GameObject>();
        GameObject LevelUpItemViewPrefab;
        GameObject SelectedItemView = null;

        // view 프리팹


        private void Awake()
        {
            LevelUpItemViewPrefab = Resources.Load<GameObject>("Prefabs/UI/LevelUpItemView") as GameObject;

            // this class is event listner
            Singleton<LevelUpManager>.getInstance().Action_LevelUpCreate += this.onLevelUpCreate;
            Singleton<LevelUpManager>.getInstance().noticeLevelUpSelect += this.onLevelUpSelect;
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }

        /// <summary>
        // recieve event from view input
        /// </summary>
        public void onTapSelect(bool value)
        {
            if (value == true)
            {
                ShowItemViewList();
                // confirm button delegate attach;
                ViewLoader.Button_Confirm.GetComponent<UIButton>().onClick.Add(new EventDelegate(this, "onConfirm"));
            }
            if (value == false)
            {
                HideItemViewList();
                ViewLoader.Button_Confirm.GetComponent<UIButton>().onClick.Clear();
            }
        }

        public void onItemSelect(GameObject sender) //, string ItemButtonName, bool toggleState
        {
            if (sender.GetComponent<UIToggle>().value == false) //// toggle off
            {
                SelectedItemView = null;
                noticeSelect(null);
            }

            else if (sender.GetComponent<UIToggle>().value == true) // toggle on
            {
                SelectedItemView = sender;
                noticeSelect(SelectedItemView.name);
            }
        }

        public void onConfirm()
        {
            noticeConfirm();
        }

        /// <summary>
        /// fuction
        /// </summary>
        internal void ShowItemViewList()
        {
            foreach (GameObject itemView in ItemViewList)
            {
                itemView.SetActive(true);
                itemView.transform.parent = ViewLoader.Item_Enable.transform;
                itemView.transform.localScale = ViewLoader.Item_Enable.transform.localScale;
            }
            ViewLoader.Item_Enable.GetComponent<UIGrid>().Reposition();
        }
        internal void HideItemViewList()
        {
            // 탭넘어가기전에 현재 선택한 item 해제한다.
            if (SelectedItemView != null)
                SelectedItemView.GetComponent<UIToggle>().value = false;

            foreach (GameObject itemView in ItemViewList)
            {
                itemView.transform.parent = ViewLoader.Item_Disable.transform;
                itemView.SetActive(false);
            }

            // 상태값 초기화
            SelectedItemView = null;
            ClearInfo();
        }
        void ClearInfo()
        {
            if (ViewLoader.Info_Context.activeInHierarchy)
                ViewLoader.Info_Context.SetActive(false);
        }

        private GameObject FindItemView(string viewName)
        {
            foreach (GameObject itemview in ItemViewList)
            {
                if (itemview.transform.name == viewName)
                    return itemview;
            }
            return null;
        }

        /// <summary>
        /// receive event from data
        /// </summary>
        void onLevelUpCreate(LevelUpItem item) // create item view
        {
            // bind
            item.Action_LevelUpChange += this.onLevelUpChange;

            // Instantiate object
            GameObject itemView = GameObject.Instantiate<GameObject>(LevelUpItemViewPrefab) as GameObject;
            itemView.name = item.ViewName;
            itemView.SetActive(false);

            // delegate 붙이기
            EventDelegate eventdel = new EventDelegate(this, "onItemSelect");
            EventDelegate.Parameter p0 = new EventDelegate.Parameter();
            p0.obj = itemView;
            eventdel.parameters[0] = p0;
            itemView.GetComponent<UIToggle>().onChange.Add(eventdel);

            ItemViewList.Add(itemView);
        }
        internal void onLevelUpChange(LevelUpItem item) // change item text, refill info view
        {
            // updateitemview
            GameObject itemView = FindItemView(item.ViewName);
            if (itemView == null)
                return;

            // Update itemView
            FillItemContents(itemView, item);

            // UpdateInfoView if selected
            if (itemView == SelectedItemView)
                FillInfoView(item);
        }
        void onLevelUpSelect(LevelUpItem selecteditem) // fill info view
        {
            if (selecteditem == null)
            {
                ClearInfo();
                return;
            }

            if (SelectedItemView != null)
                ViewLoader.Info_Context.SetActive(true);

            FillInfoView(selecteditem);
        }
        private void CheckOnOff(GameObject item, bool On)
        {

            //Enable_Window.GetComponent<UIGrid>().Reposition();
        }
        private void FillItemContents(GameObject itemView, LevelUpItem item)
        {
            GameObject iconObject = itemView.transform.FindChild("Icon").gameObject;
            iconObject.GetComponent<UISprite>().atlas = Resources.Load("Atlas/IconImage", typeof(UIAtlas)) as UIAtlas;
            iconObject.GetComponent<UISprite>().spriteName = item.Icon;

            GameObject labelObject = itemView.transform.FindChild("Label").gameObject;
            labelObject.GetComponent<UILabel>().text = item.Button_Text; // 최초에 그리기용

            if (item.CanBuy)
                itemView.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
            else
                itemView.GetComponent<UISprite>().color = new Color(0.5f, 0.5f, 0.5f, 0.8f);
        }
        private void FillInfoView(LevelUpItem item)
        {
            UIButton Confirm = ViewLoader.Button_Confirm.GetComponent<UIButton>();

            ViewLoader.Info_Icon.GetComponent<UISprite>().spriteName = item.Icon;
            ViewLoader.Info_Description.GetComponent<UILabel>().text = item.Info_Text;
            if (item.CanBuy)
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

        // 여기 얹혀서 처리 ㅠ
        public void OnDamageFontFinish()
        {
            // 이것만 직접처리한다. 귀찮엉...
            ViewLoader.DamageFont.SetActive(false);
        }
    }
}