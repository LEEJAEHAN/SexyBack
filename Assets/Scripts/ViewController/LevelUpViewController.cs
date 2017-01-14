using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SexyBackPlayScene
{
    public class LevelUpViewController : MonoBehaviour
    {
        public delegate void ConfirmButton_Event_Handler(string id);
        public event ConfirmButton_Event_Handler Confirm;

        public delegate void FillInfo_Event_Handler(string id);
        public event FillInfo_Event_Handler FillInfoFunction;


        //public EventDelegate eventdel;

        /// <summary>
        /// ///////////////
        /// </summary>
        List<GameObject> ItemViewList = new List<GameObject>();
        GameObject SelectedItemView = null;
        string PrevEventViewName; // for toggle none

        // view 프리팹

        GameObject grid;
        GameObject grid_disable;
        GameObject info_window;


        // Use this for initialization
        void Start()
        {
            grid = ViewLoader.Item_Enable;
            grid_disable = ViewLoader.Item_Disable;
            info_window = ViewLoader.Info_Context;

            //eventdel = new EventDelegate(this, "OnSelectItemButton");
        }

        // 게임로직에서 호출이있을때만 ( 변화가생겼다는것 ) 동작
// Update is called once per frame
        void Update()
        {

        }

        //LevelUpItem item  은 크게 이 ui에서 나타날 수 있는 item이라는 것의 자식객체;
        public void AddItemView(LevelUpItem item, GameObject itemViewPrefab)
        {
            GameObject temp = GameObject.Instantiate<GameObject>(itemViewPrefab) as GameObject;
            temp.SetActive(false);
            temp.name = item.ItemViewID;

            GameObject iconObject = temp.transform.FindChild("Icon").gameObject;
            iconObject.GetComponent<UISprite>().atlas = Resources.Load("Atlas/IconImage", typeof(UIAtlas)) as UIAtlas;
            iconObject.GetComponent<UISprite>().spriteName = item.IconName;

            GameObject labelObject = temp.transform.FindChild("Label").gameObject;
            labelObject.GetComponent<UILabel>().text = item.GetTargetDamage(); // 최초에 그리기용

            // delegate 붙이기
            EventDelegate eventdel = new EventDelegate(this, "onItemChange");
            EventDelegate.Parameter p0 = new EventDelegate.Parameter();
            p0.obj = temp;
            eventdel.parameters[0] = p0;

            temp.GetComponent<UIToggle>().onChange.Add(eventdel);

            ItemViewList.Add(temp);
        }

        //private GameObject FindItemView(string id)
        //{
        //    foreach (GameObject itemView in ItemViewList)
        //    {
        //        if (itemView.transform.name == id)
        //            return itemView;
        //    }
        //    return null;
        //}

        public void onTapChange(bool value)
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

        internal void ShowItemViewList()
        {
            foreach (GameObject itemView in ItemViewList)
            {
                itemView.SetActive(true);
                itemView.transform.parent = grid.transform;
                itemView.transform.localScale = grid.transform.localScale;
            }
            grid.GetComponent<UIGrid>().Reposition();
        }
        internal void HideItemViewList()
        {
            // 탭넘어가기전에 현재 선택한 item 해제한다.
            if (SelectedItemView != null)
                SelectedItemView.GetComponent<UIToggle>().value = false;

            foreach (GameObject itemView in ItemViewList)
            {
                itemView.transform.parent = grid_disable.transform;
                itemView.SetActive(false);
                //itemView.transform.localScale = grid.transform.localScale;
            }

            // 상태값 초기화
            SelectedItemView = null;
            PrevEventViewName = null;
            ClearInfo();
            //grid_disable.GetComponent<UIGrid>().Reposition();
        }


        public void onItemChange(GameObject sender) //, string ItemButtonName, bool toggleState
        {
            if (sender.GetComponent<UIToggle>().value == false) //// toggle off
            {
                SelectedItemView = null;
                if (PrevEventViewName == sender.transform.name) // state of none;
                    ClearInfo();
            }

            else if (sender.GetComponent<UIToggle>().value == true) // toggle on
            {
                SelectedItemView = sender;
                FillInfo();
            }
            //                    PrevEventViewName = sender.transform.name;
        }

        void ClearInfo()
        {
            if (info_window.activeInHierarchy)
            {
                info_window.SetActive(false);
                SexyBackLog.Console("ClearInfo");
            }
        }
        void FillInfo()
        {
            if (SelectedItemView != null)
                info_window.SetActive(true);

            FillInfoFunction(SelectedItemView.name);
            // 구현해야함
        }

        public void onConfirm()
        {
            Confirm(SelectedItemView.name);
        }

    }
}