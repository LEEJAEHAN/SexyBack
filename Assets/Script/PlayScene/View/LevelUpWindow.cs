﻿using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class LevelUpWindow : MonoBehaviour// singleton 사용
    {
        LevelUpWindow()
        {
            sexybacklog.Console("새로운 LevelUpWindow 생성 ");
        }
        ~LevelUpWindow()
        {
            sexybacklog.Console("LevelUpWindow 소멸");
        }

        GameObject Icon;

        UILabel Name;
        UILabel StatName;
        UILabel StatValue;
        UILabel PriceValue;
        UILabel Damage;

        UIButton Button1;

        public delegate void Confirm_Event();
        public event Confirm_Event Action_Confirm;

        private void Awake()
        {
            Icon = gameObject.transform.FindChild("Icon").gameObject;
            Button1 = gameObject.transform.FindChild("Right/Button").GetComponent<UIButton>();
            Damage = gameObject.transform.FindChild("Right/Damage").GetComponent<UILabel>();
            PriceValue = gameObject.transform.FindChild("Right/Price").GetComponent<UILabel>();
            Name = gameObject.transform.FindChild("Middle/Name").GetComponent<UILabel>();
            StatName = gameObject.transform.FindChild("Middle/StatName").GetComponent<UILabel>();
            StatValue = gameObject.transform.FindChild("Middle/StatValue").GetComponent<UILabel>();

            gameObject.SetActive(false);
            Button1.onClick.Clear();
            Button1.onClick.Add(new EventDelegate(this, "onButton1"));
        }

        public void onButton1()
        {
            Action_Confirm();
        }

        public void Show(bool selected, NestedIcon Icon, string name, string statname, string statvalue,
            string pricevalue, string damage)
        {   // infoview는 select상태에서만 갱신해야한다.
            if (!selected)
                return;
            gameObject.SetActive(true);
            NestedIcon.Draw(Icon, this.Icon);

            Name.text = name;
            StatName.text = statname;
            StatValue.text = statvalue;
            PriceValue.text = pricevalue;
            Damage.text = damage;
        }

        public void Hide()
        {
            if (gameObject.activeInHierarchy)
                gameObject.SetActive(false);
        }

        public void SetButton1(bool selected, bool value)
        {
            if (!selected || Button1.enabled == value)
                return;

            Button1.enabled = value;
            if (value)
                Button1.SetState(UIButtonColor.State.Normal, true);
            else
                Button1.SetState(UIButtonColor.State.Disabled, true);
        }

    }
}
