﻿using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class ResearchWindow : MonoBehaviour// singleton 사용
    {
        ~ResearchWindow()
        {
            sexybacklog.Console("ResearchWindow 소멸");
        }
        //private static ResearchWindow instance;
        //public static ResearchWindow getInstance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            GameObject owner = ViewLoader.InstantiatePrefab(GameObject.Find("Bottom_Window").transform, "ResearchWindow", "Prefabs/UI/ResearchWindow");
        //            instance = owner.AddComponent<ResearchWindow>();
        //        }
        //        return instance;
        //    }
        //}
        //internal static void Clear()
        //{
        //    Destroy(instance);
        //    instance = null;
        //}

        GameObject Icon;

        UILabel Name;
        UILabel Description;
        UILabel TimeLabel;
        UILabel TimeValue;
        UILabel PriceValue;
        UILabel Require;
        UIButton Button1;
        UIButton Button2;

        public delegate void Confirm_Event();
        public event Confirm_Event Action_Confirm = delegate { };

        public delegate void Pause_Event();
        public event Pause_Event Action_Pause = delegate { };

        private void Awake()
        {
            Icon = gameObject.transform.FindChild("Icon").gameObject;

            Button1 = gameObject.transform.FindChild("Right/Button1").GetComponent<UIButton>();
            Button2 = gameObject.transform.FindChild("Right/Button2").GetComponent<UIButton>();
            TimeLabel = gameObject.transform.FindChild("Right/Label2").GetComponent<UILabel>();
            TimeValue = gameObject.transform.FindChild("Right/Time").GetComponent<UILabel>();
            PriceValue = gameObject.transform.FindChild("Right/Price").GetComponent<UILabel>();
            Require = gameObject.transform.FindChild("Right/Requirement").GetComponent<UILabel>();
            Name = gameObject.transform.FindChild("Middle/Name").GetComponent<UILabel>();
            Description = gameObject.transform.FindChild("Middle/Description").GetComponent<UILabel>();

            gameObject.SetActive(false);

            Button1.onClick.Clear();
            Button1.onClick.Add(new EventDelegate(this, "onButton1"));
            Button2.onClick.Clear();
            Button2.onClick.Add(new EventDelegate(this, "onButton2"));
        }


        public void onButton1()
        {
            Action_Confirm();
        }
        public void onButton2()
        {
            Action_Pause();
        }

        public void Show(bool selected, ResearchData data, int time, BigInteger price, BigInteger pricepersec, bool instancemode)
        {   // infoview는 select상태에서만 갱신해야한다.
            if (!selected)
                return;

            if (instancemode)
            {
                TimeValue.gameObject.SetActive(false);
                TimeLabel.gameObject.SetActive(false);
                PriceValue.text = string.Format("{0}", price.To5String());
            }
            else
            {
                TimeValue.gameObject.SetActive(true);
                TimeLabel.gameObject.SetActive(true);
                PriceValue.text = string.Format("{0}\n{1} / 초", price.To5String(), pricepersec.To5String());
            }

            gameObject.SetActive(true);
            NestedIcon.Draw(data.icon, this.Icon);
            Name.text = data.Name;
            Description.text = data.Description;
            TimeValue.text = string.Format("{0}초", time); 
            Require.text = StringParser.GetRequireText(data.requireID, data.requireLevel);
        }

        public void Hide()
        {
            if (gameObject.activeInHierarchy)
                gameObject.SetActive(false);
        }

        internal void SetButton2(bool selected, bool active, string text)
        {
            if (!selected)
                return;

            Button2.gameObject.SetActive(active);
            if (active)
                Button2.gameObject.transform.GetChild(0).GetComponent<UILabel>().text = text;
        }

        public void SetButton1(bool selected, bool enable, bool active)
        {

            if (!selected)
                return;

            Button1.gameObject.SetActive(active);
            Button1.enabled = enable;
            if (enable)
                Button1.SetState(UIButtonColor.State.Normal, true);
            else
                Button1.SetState(UIButtonColor.State.Disabled, true);

        }

    }
}
