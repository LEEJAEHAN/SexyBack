using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class ResearchWindow : MonoBehaviour// singleton 사용
    {
        ~ResearchWindow()
        {
            sexybacklog.Console("ResearchWindow 소멸");
        }

        internal static void Clear()
        {
            Destroy(instance);
            instance = null;
        }

        GameObject Icon;

        UILabel Name;
        UILabel Description;
        UILabel PriceName;
        UILabel PriceValue;

        UIButton Button1;
        UIButton Button2;

        public delegate void Confirm_Event();
        public event Confirm_Event Action_Confirm;

        public delegate void Pause_Event();
        public event Pause_Event Action_Pause;


        private static ResearchWindow instance;
        public static ResearchWindow getInstance
        {
            get
            {
                if (instance == null)
                {
                    GameObject owner = ViewLoader.InstantiatePrefab(GameObject.Find("Bottom_Window").transform, "ResearchWindow", "Prefabs/UI/ResearchWindow");
                    instance = owner.AddComponent<ResearchWindow>();
                }
                return instance;
            }
        }

        private void Awake()
        {
            Icon = gameObject.transform.FindChild("Icon").gameObject;

            Button1 = gameObject.transform.FindChild("Right/Button1").GetComponent<UIButton>();
            Button2 = gameObject.transform.FindChild("Right/Button2").GetComponent<UIButton>();

            PriceName = gameObject.transform.FindChild("Right/PriceName").GetComponent<UILabel>();
            PriceValue = gameObject.transform.FindChild("Right/PriceValue").GetComponent<UILabel>();

            Name = gameObject.transform.FindChild("Middle/Name").GetComponent<UILabel>();
            Description = gameObject.transform.FindChild("Middle/Description").GetComponent<UILabel>();

            gameObject.SetActive(false);

            Button1.onClick.Add(new EventDelegate(this, "onButton1"));
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

        public void Show(bool selected, NestedIcon Icon, string name, string description, string pricename, string pricevalue)
        {   // infoview는 select상태에서만 갱신해야한다.
            if (!selected)
                return;
            gameObject.SetActive(true);
            NestedIcon.Draw(Icon, this.Icon);

            Name.text = name;
            Description.text = description;
            PriceName.text = pricename;
            PriceValue.text = pricevalue;
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
            if (!selected || Button1.enabled == enable)
                return;

            Button1.enabled = enable;
            if (enable)
                Button1.SetState(UIButtonColor.State.Normal, true);
            else
                Button1.SetState(UIButtonColor.State.Disabled, true);

            Button1.gameObject.SetActive(active);
        }


    }
}
