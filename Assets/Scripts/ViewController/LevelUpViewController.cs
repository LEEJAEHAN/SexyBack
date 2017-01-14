using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SexyBackPlayScene
{
    public class LevelUpViewController : MonoBehaviour
    {

        public delegate void Tap_Event_Handler(bool toggleState);
        public event Tap_Event_Handler TapChange;

        public delegate void ItemButton_Event_Handler(string ItemButtonName, bool toggleState);
        public event ItemButton_Event_Handler ItemChange;


        public EventDelegate eventdel;


        // Use this for initialization
        void Start()
        {
            eventdel = new EventDelegate(this, "OnSelectItemButton");
        }


        // 게임로직에서 호출이있을때만 ( 변화가생겼다는것 ) 동작

        // Update is called once per frame
        void Update()
        {

        }

        public void OnLevelUpTapButton(bool toggleState)
        {
            TapChange(toggleState);
        }

        public void OnSelectItemButton(string ItemButtonName, bool toggleState)
        {
            ItemChange(ItemButtonName, toggleState);
        }

        public void OnConfirmButton()
        {
            Singleton<LevelUpManager>.getInstance().Confirm();
        }
    }
}