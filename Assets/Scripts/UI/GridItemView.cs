using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class GridItemView : MonoBehaviour
    {
        // researchview 가 아닌 iconview 범용 스크립트로 만들각오.
        public delegate void GridItemSelect_Event(string name);
        public event GridItemSelect_Event Action_SelectGridItem;

        public delegate void GridItemConfirm_Event(string name);
        public event GridItemConfirm_Event Action_ConfirmGridItem;

        public delegate void GridItemPause_Event(string name);
        public event GridItemPause_Event Action_PauseGridItem;

        public void onItemSelect() //, string ItemButtonName, bool toggleState
        {
            if (GetComponent<UIToggle>().value == false) //// toggle off
            {
                Action_SelectGridItem(null);
            }

            else if (GetComponent<UIToggle>().value == true) // toggle on
            {
                //Singleton<InfoPanel>.getInstance().SetButton1Event(new EventDelegate(this, "onConfirm"));
                //Singleton<InfoPanel>.getInstance().SetButton2Event(new EventDelegate(this, "onPause"));
                Action_SelectGridItem(this.name);
            }
        }

        public void onConfirm()
        {
            Action_ConfirmGridItem(this.name);
        }
        public void onPause()
        {
            Action_PauseGridItem(this.name);
        }
        private void OnDestroy()
        {
            Action_SelectGridItem = null;
            Action_ConfirmGridItem = null;
            Action_PauseGridItem = null;
            sexybacklog.Console("게임오브젝트소멸");
        }
        void OnDisable()
        {
            if (GetComponent<UIToggle>().value == true) //// toggle off when disable
            {
                GetComponent<UIToggle>().value = false;
                Action_SelectGridItem(null);
            }
        }
    }
}