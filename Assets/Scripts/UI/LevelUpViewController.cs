using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SexyBackPlayScene
{
    public class LevelUpViewController : MonoBehaviour
    {
        // TODO: 탭컨트롤러로 수정해야함.
        //LevelUpItem은 이 ui에서 나타날 수 있는 것을 포함한 객체;

        // this class is event Publisher
        // for view 

        List<GameObject> ItemViewList = new List<GameObject>();
        GameObject SelectedItemView = null;

        // view 프리팹
        private void Awake()
        {
            // this class is event listner
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

        /// <summary>
        /// receive event from data
        /// </summary>


        // 여기 얹혀서 처리 ㅠ
        public void OnDamageFontFinish()
        {
            // 이것만 직접처리한다. 귀찮엉...
            ViewLoader.DamageFont.SetActive(false);
        }
    }
}