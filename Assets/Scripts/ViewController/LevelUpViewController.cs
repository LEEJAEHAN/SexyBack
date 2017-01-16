using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SexyBackPlayScene
{
    public class LevelUpViewController : MonoBehaviour
    {
        //LevelUpItem은 이 ui에서 나타날 수 있는 것을 포함한 객체;

        // send
        public delegate void ConfirmButton_Event_Handler();
        public event ConfirmButton_Event_Handler Confirm;

        public delegate void SelectEvent_Handler(string id);
        public event SelectEvent_Handler Select;
        
        // for view
        List<GameObject> ItemViewList = new List<GameObject>();
        GameObject LevelUpItemViewPrefab;
        GameObject SelectedItemView = null;

        // view 프리팹
        GameObject Enable_Window;
        GameObject Disable_Window;
        GameObject Info_Window;
        GameObject Info_Icon;
        GameObject Info_Description;

        private void Awake()
        {
            LevelUpItemViewPrefab = Resources.Load<GameObject>("Prefabs/UI/LevelUpItemView") as GameObject;

            Enable_Window = ViewLoader.Item_Enable;
            Disable_Window = ViewLoader.Item_Disable;
            Info_Window = ViewLoader.Info_Context;
            Info_Icon = ViewLoader.Info_Icon;
            Info_Description = ViewLoader.Info_Description;


            Singleton<LevelUpManager>.getInstance().onLevelUpCreate += this.onLevelUpCreate;
            Singleton<LevelUpManager>.getInstance().onLevelUpSelect += this.onLevelUpSelect;
            Singleton<LevelUpManager>.getInstance().onLevelUpChange += this.onLevelUpChange;
        }
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (SelectedItemView == null)
                ClearInfo();
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
                Select(null);
            }

            else if (sender.GetComponent<UIToggle>().value == true) // toggle on
            {
                SelectedItemView = sender;
                Select(SelectedItemView.name);
            }
        }

        public void onConfirm()
        {
            Confirm();
        }

        /// <summary>
        /// fuction
        /// </summary>
        internal void ShowItemViewList()
        {
            foreach (GameObject itemView in ItemViewList)
            {
                itemView.SetActive(true);
                itemView.transform.parent = Enable_Window.transform;
                itemView.transform.localScale = Enable_Window.transform.localScale;
            }
            Enable_Window.GetComponent<UIGrid>().Reposition();
        }

        internal void HideItemViewList()
        {
            // 탭넘어가기전에 현재 선택한 item 해제한다.
            if (SelectedItemView != null)
                SelectedItemView.GetComponent<UIToggle>().value = false;

            foreach (GameObject itemView in ItemViewList)
            {
                itemView.transform.parent = Disable_Window.transform;
                itemView.SetActive(false);
                //itemView.transform.localScale = grid.transform.localScale;
            }

            // 상태값 초기화
            SelectedItemView = null;
            ClearInfo();
            //grid_disable.GetComponent<UIGrid>().Reposition();
        }

        void ClearInfo()
        {
            if (Info_Window.activeInHierarchy)
            {
                Info_Window.SetActive(false);
                sexybacklog.Console("ClearInfo");
            }
        }

        private GameObject FindItemView(string id)
        {
            foreach (GameObject itemview in ItemViewList)
            {
                if (itemview.transform.name == id)
                    return itemview;
            }
            return null;
        }

        /// <summary>
        /// receive event from data
        /// </summary>
        void onLevelUpCreate(LevelUpItem item) // create item view
        {
            GameObject itemView = GameObject.Instantiate<GameObject>(LevelUpItemViewPrefab) as GameObject;
            itemView.SetActive(false);

            // delegate 붙이기
            EventDelegate eventdel = new EventDelegate(this, "onItemSelect");
            EventDelegate.Parameter p0 = new EventDelegate.Parameter();
            p0.obj = itemView;
            eventdel.parameters[0] = p0;
            itemView.GetComponent<UIToggle>().onChange.Add(eventdel);

            // item 내용 채우기
            FillItemContents(itemView, item);

            ItemViewList.Add(itemView);
        }
        void onLevelUpSelect(LevelUpItem selecteditem) // fill info view
        {
            if (SelectedItemView != null)
                Info_Window.SetActive(true);

            FillInfoView(selecteditem);
        }
        // 데이터 체인지로부터 인한 이벤트
        internal void onLevelUpChange(LevelUpItem item) // change item text, refill info view
        {
            // updateitemview
            GameObject itemView = FindItemView(item.ID);
            if (itemView == null)
                return;

            // Update itemView
            FillItemContents(itemView, item);

            // UpdateInfoView if selected
            if (itemView == SelectedItemView)
                FillInfoView(item);
        }
        private void FillItemContents(GameObject itemView, LevelUpItem item)
        {
            itemView.name = item.ID; // 버튼의 이름은

            GameObject iconObject = itemView.transform.FindChild("Icon").gameObject;
            iconObject.GetComponent<UISprite>().atlas = Resources.Load("Atlas/IconImage", typeof(UIAtlas)) as UIAtlas;
            iconObject.GetComponent<UISprite>().spriteName = item.IconName;

            GameObject labelObject = itemView.transform.FindChild("Label").gameObject;
            labelObject.GetComponent<UILabel>().text = item.Item_Text; // 최초에 그리기용
        }
        private void FillInfoView(LevelUpItem item)
        {
            string description = item.Info_Name + " LV" + item.PurchaseCount.ToString();
            description += "\n";
            description += item.Info_Description;
            description += "\n";
            description += "Cost : " + item.Info_Price.ToSexyBackString() + " EXP";

            Info_Icon.GetComponent<UISprite>().spriteName = item.IconName;
            Info_Description.GetComponent<UILabel>().text = description;
        }

    }
}