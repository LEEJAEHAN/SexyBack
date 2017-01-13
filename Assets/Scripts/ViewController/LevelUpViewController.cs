using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    public class LevelUpViewController : MonoBehaviour
    {



        // Use this for initialization
        void Start()
        {


        }

        // 게임로직에서 호출이있을때만 ( 변화가생겼다는것 ) 동작
        // 모듈로부터 데이터를 가져와 리스트를갱신
        // 지금은 임시

        // Update is called once per frame
        void Update()
        {

        }

        public void OnLevelUpTapButton(bool toggleState)
        {
            if (toggleState == true)
            {
                Singleton<LevelUpManager>.getInstance().ShowItemView();

                // confirm button delegate attach;
                ViewLoader.Button_Confirm.GetComponent<UIButton>().onClick.Add(new EventDelegate(this, "OnConfirmButton"));
                //Singleton<LevelUpManager>.getInstance().AttachDelegate();
            }
            if (toggleState == false)
            {
                Singleton<LevelUpManager>.getInstance().HideItemView();
                ViewLoader.Button_Confirm.GetComponent<UIButton>().onClick.Clear();
            }
        }

        public void OnSelectItemButton(string ItemButtonName, bool toggleState)
        {
            if(toggleState == false)
            {
                Singleton<LevelUpManager>.getInstance().UnselectItem(ItemButtonName);
            }

            else
            {
                Singleton<LevelUpManager>.getInstance().SelectItem(ItemButtonName);
            }
        }

        public void OnConfirmButton()
        {
            Singleton<LevelUpManager>.getInstance().Confirm();
        }
    }
}