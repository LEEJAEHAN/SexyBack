using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class LevelUpManager
    {
        List<LevelUpItem> items;
        LevelUpItem selectedItem = null;
        bool ConfirmFlag;
        bool ReadyToProcess { get { return ConfirmFlag && selectedItem != null; } }

        internal void Init()
        {
            items = new List<LevelUpItem>();
            Load();
            MakeItemView();
        }
        void Load()
        {
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[0], "SexyBackIcon_FireElemental", "LevelUpItem0", 1, 100));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[1], "SexyBackIcon_WaterElemental", "LevelUpItem1", 1, 500));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[2], "SexyBackIcon_RockElemental", "LevelUpItem2", 1, 4000));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[3], "SexyBackIcon_ElectricElemental", "LevelUpItem3", 1, 37500));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[4], "SexyBackIcon_SnowElemental", "LevelUpItem4", 1, 450000));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[5], "SexyBackIcon_EarthElemental", "LevelUpItem5", 1, 7250000));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[6], "SexyBackIcon_AirElemental", "LevelUpItem6", 1, 150000000));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[7], "SexyBackIcon_IceElemental", "LevelUpItem7", 1, 4000000000));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[8], "SexyBackIcon_MagmaElemental", "LevelUpItem8", 1, 140000000000));
        }

        internal void Update()
        {
            /// ui에서 confirm 클릭이들어오면 flag를 변경해놓을것이다.
            /// 그럴때 변경된걸 업한다.
            /// 

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

            SexyBackLog.InGame(items[8].PriceToNextLv.ToSexyBackString());

        }

        public List<LevelUpItem> GetItemList()
        {
            return items;
        }

        /// <summary>
        ///  이하는 view 표시 관련, view 단의 작업. 절대 data를 변경해선 안된다. ( 데이터는 update에서 변경되어야 한다.)
        /// </summary>
        GameObject LevelUpItemViewPrefab = Resources.Load<GameObject>("Prefabs/UI/LevelUpItemView") as GameObject;
        List<GameObject> LevelUpItemViewList;

        GameObject grid = ViewLoader.Item_Enable;
        GameObject grid_disable = ViewLoader.Item_Disable;
        GameObject info_window = ViewLoader.Info_Context;
        GameObject view_controller = ViewLoader.LevelUpViewController;

        public void Confirm()
        {
            ConfirmFlag = true;
        }

        internal void UnselectItem(string itemButtonName)
        {
            if (selectedItem == null)
                return;

            selectedItem = null;
        }
        internal void SelectItem(string ItemButtonName)
        {
            if (selectedItem == null)
            {
                info_window.SetActive(true);
            }
            foreach(LevelUpItem item in items)
            {
                if (item.ID == ItemButtonName)
                    selectedItem = item;
            }
            FillInfo();
        }

        void ClearInfo()
        {
            if(info_window.activeInHierarchy)
            {
                info_window.SetActive(false);
                SexyBackLog.Console("ClearInfo");
            }
        }
        void FillInfo()
        {
            SexyBackLog.Console(selectedItem.IconName);
        }

        void MakeItemView() // draw()
        {
            LevelUpItemViewList = new List<GameObject>();

            foreach (LevelUpItem item in items)
            {
                GameObject temp = GameObject.Instantiate<GameObject>(LevelUpItemViewPrefab) as GameObject;
                temp.SetActive(false);
                temp.name = item.ID;

                GameObject iconObject = temp.transform.FindChild("Icon").gameObject;
                iconObject.GetComponent<UISprite>().atlas = Resources.Load("Atlas/IconImage", typeof(UIAtlas)) as UIAtlas;
                iconObject.GetComponent<UISprite>().spriteName = item.IconName;

                GameObject labelObject = temp.transform.FindChild("Label").gameObject;
                labelObject.GetComponent<UILabel>().text = item.GetTargetDamage();

                // delegate 붙이기
                EventDelegate eventdel = new EventDelegate(view_controller.GetComponent<LevelUpViewController>(), "OnSelectItemButton");
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

        internal void UpdateItemView() // updateView
        {
            SexyBackLog.Console("Update ItemViewList");
            for(int i = 0; i < items.Count; i++)
            {
                LevelUpItemViewList[i].name = items[i].ID;

                GameObject labelObject = LevelUpItemViewList[i].transform.FindChild("Label").gameObject;
                labelObject.GetComponent<UILabel>().text = items[i].GetTargetDamage();
            }
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
            foreach (GameObject itemView in LevelUpItemViewList)
            {
                itemView.transform.parent = grid_disable.transform;
                itemView.SetActive(false);
                //itemView.transform.localScale = grid.transform.localScale;
            }
            selectedItem = null;

            //grid_disable.GetComponent<UIGrid>().Reposition();
        }

    }
}