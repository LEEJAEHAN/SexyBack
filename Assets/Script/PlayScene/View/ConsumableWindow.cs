using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class ConsumableWindow : MonoBehaviour// singleton 사용
    {
        ConsumableWindow()
        {
            sexybacklog.Console("새로운 ConsumableWindow 생성 ");
        }
        ~ConsumableWindow()
        {
            sexybacklog.Console("ConsumableWindow 소멸");
        }

        GameObject Icon;
        UILabel Name;
        UILabel Description;
        UIButton Button1;

        public delegate void Confirm_Event();
        public event Confirm_Event Action_Confirm;

        private void Awake()
        {
            Icon = gameObject.transform.FindChild("Icon").gameObject;
            Button1 = gameObject.transform.FindChild("Content/Button").GetComponent<UIButton>();
            Name = gameObject.transform.FindChild("Content/Name").GetComponent<UILabel>();
            Description = gameObject.transform.FindChild("Content/Description").GetComponent<UILabel>();

            gameObject.SetActive(false);
            Button1.onClick.Add(new EventDelegate(this, "onButton1"));
        }

        public void onButton1()
        {
            Action_Confirm();
        }

        public void Show(bool selected, NestedIcon Icon, string name, string description)
        {   // infoview는 select상태에서만 갱신해야한다.
            if (!selected)
                return;
            gameObject.SetActive(true);
            NestedIcon.Draw(Icon, this.Icon);
            Name.text = name;
            Description.text = description;
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
