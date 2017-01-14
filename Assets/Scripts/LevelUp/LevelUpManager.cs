using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class LevelUpManager
    {

        List <LevelUpItem> items;
        LevelUpItem selectedItem = null;
        bool ConfirmFlag;
        bool ReadyToProcess { get { return ConfirmFlag && selectedItem != null; } }


        // view 프리팹
        GameObject LevelUpItemViewPrefab = Resources.Load<GameObject>("Prefabs/UI/LevelUpItemView") as GameObject;
        List<GameObject> LevelUpItemViewList;

        // view 관련 ( 이벤트핸들러)
        GameObject levelview_event_handler_monobehav = ViewLoader.LevelUpViewController;
        LevelUpViewController levelview_event_handler;
        // view 관련 ( 컴포넌트 )
        GameObject grid = ViewLoader.Item_Enable;
        GameObject grid_disable = ViewLoader.Item_Disable;
        GameObject info_window = ViewLoader.Info_Context;



        internal void Init()
        {
            levelview_event_handler = levelview_event_handler_monobehav.GetComponent<LevelUpViewController>();

            // view와 이벤트 연결
            levelview_event_handler.TapChange += this.onTapChange;
            levelview_event_handler.ItemChange += this.onItemChange;



            items = new List<LevelUpItem>();
            Load();
            MakeItemView();
        }

        void Load()
        {
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[0], "SexyBackIcon_FireElemental", 1, 100));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[1], "SexyBackIcon_WaterElemental", 1, 500));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[2], "SexyBackIcon_RockElemental", 1, 4000));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[3], "SexyBackIcon_ElectricElemental", 1, 37500));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[4], "SexyBackIcon_SnowElemental", 1, 450000));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[5], "SexyBackIcon_EarthElemental", 1, 7250000));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[6], "SexyBackIcon_AirElemental", 1, 150000000));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[7], "SexyBackIcon_IceElemental", 1, 4000000000));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[8], "SexyBackIcon_MagmaElemental", 1, 140000000000));
        }

        internal void Update()
        {
            /// ui에서 confirm 클릭이들어오면 flag를 변경해놓을것이다.
            if(ReadyToProcess)
            {
                selectedItem.Buy();     // levelup 만 함;
                ConfirmFlag = false;
            }

            if(selectedItem == null)
            {
                ClearInfo();
            }
            foreach (LevelUpItem item in items)
            {
                item.ApplyProcess(); // 업한 level을 elemental에 적용시키는것;
            }
        }

        public List<LevelUpItem> GetItemList()
        {
            return items;
        }
        private LevelUpItem FindItem(string id)
        {
            foreach (LevelUpItem item in items)
            {
                if (item.ItemViewID == id)
                    return item;
            }
            return null;
        }

        internal void onDpsChanged(Elemental sender)
        {
            // updateitemview
            GameObject item = FindItemView(sender.ItemViewID);
            GameObject label = item.transform.FindChild("Label").gameObject;
            label.GetComponent<UILabel>().text = sender.Dps.ToSexyBackString();

            // UpdateInfoView if selected
            // 구현해야함
        }

        internal void onExpChanged()
        {
            // 구현해야함
            // 업글할 수 있는것만 것만 표시;
            // confirm 버튼 활성화 비활성화;
        }


        ///  이하는 view 표시 관련, view 단의 작업. 절대 data를 변경해선 안된다. ( 데이터는 update에서 변경되어야 한다.)



        void MakeItemView() // draw()
        {
            LevelUpItemViewList = new List<GameObject>();

            foreach (LevelUpItem item in items)
            {
                GameObject temp = GameObject.Instantiate<GameObject>(LevelUpItemViewPrefab) as GameObject;
                temp.SetActive(false);
                temp.name = item.ItemViewID;

                GameObject iconObject = temp.transform.FindChild("Icon").gameObject;
                iconObject.GetComponent<UISprite>().atlas = Resources.Load("Atlas/IconImage", typeof(UIAtlas)) as UIAtlas;
                iconObject.GetComponent<UISprite>().spriteName = item.IconName;

                GameObject labelObject = temp.transform.FindChild("Label").gameObject;
                labelObject.GetComponent<UILabel>().text = item.GetTargetDamage(); // 최초에 그리기용

                // delegate 붙이기
                EventDelegate eventdel = new EventDelegate(levelview_event_handler, "OnSelectItemButton");
                EventDelegate.Parameter param = new EventDelegate.Parameter();
                param.obj = temp.transform;
                param.field = "name";
                EventDelegate.Parameter param1 = new EventDelegate.Parameter();
                param1.obj = temp.GetComponent<UIToggle>();
                param1.field = "value";
                eventdel.parameters[0] = param;
                eventdel.parameters[1] = param1;
                temp.GetComponent<UIToggle>().onChange.Add(eventdel);

                LevelUpItemViewList.Add(temp);
            }
        }

        void onTapChange(bool value)
        {
            if (value == true)
            {
                ShowItemView();
                // confirm button delegate attach;
                ViewLoader.Button_Confirm.GetComponent<UIButton>().onClick.Add(new EventDelegate(levelview_event_handler, "OnConfirmButton"));
            }
            if (value == false)
            {
                HideItemView();
                ViewLoader.Button_Confirm.GetComponent<UIButton>().onClick.Clear();
            }
        }
        void onItemChange(string ItemButtonName, bool toggleState)
        {
            if (toggleState == false)
            {
                UnselectItem(ItemButtonName);
            }

            else
            {
                SelectItem(ItemButtonName);
            }
        }
        internal void UnselectItem(string itemButtonName)
        {
            if (selectedItem == null)
                return;

            selectedItem = null;
        }
        internal void SelectItem(string ItemButtonName)
        {
            selectedItem = FindItem(ItemButtonName);
            FillInfo(selectedItem);
        }

        public void Confirm()
        {
            ConfirmFlag = true;
        }

        void ClearInfo()
        {
            if(info_window.activeInHierarchy)
            {
                info_window.SetActive(false);
                SexyBackLog.Console("ClearInfo");
            }
        }
        void FillInfo(LevelUpItem item)
        {
            if (selectedItem != null)
                info_window.SetActive(true);

            // 구현해야함
        }

        internal void ShowItemView()
        {
            foreach (GameObject itemView in LevelUpItemViewList)
            {
                itemView.SetActive(true);
                itemView.transform.parent = grid.transform;
                itemView.transform.localScale = grid.transform.localScale;
            }
            grid.GetComponent<UIGrid>().Reposition();

        }
        internal void HideItemView()
        {
            // unslelect item;
            if(selectedItem != null)
            {
                GameObject item = FindItemView(selectedItem.ItemViewID);
                item.GetComponent<UIToggle>().value = false;
            }

            foreach (GameObject itemView in LevelUpItemViewList)
            {
                itemView.transform.parent = grid_disable.transform;
                itemView.SetActive(false);
                //itemView.transform.localScale = grid.transform.localScale;
            }
            selectedItem = null;
            //grid_disable.GetComponent<UIGrid>().Reposition();
        }

        private GameObject FindItemView(string id)
        {
            foreach (GameObject itemView in LevelUpItemViewList)
            {
                if (itemView.transform.name == id)
                    return itemView;
            }
            return null;
        }


    }
}