using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class ConsumableWindow : MonoBehaviour// singleton 사용
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
        GameObject Constraint;
        UILabel CoolTime;

        public delegate void Confirm_Event();
        public event Confirm_Event Action_Confirm;

        private void Awake()
        {
            Icon = gameObject.transform.FindChild("Icon").gameObject;
            Button1 = gameObject.transform.FindChild("Content/Button").GetComponent<UIButton>();
            Name = gameObject.transform.FindChild("Content/Name").GetComponent<UILabel>();
            Description = gameObject.transform.FindChild("Content/Description").GetComponent<UILabel>();
            Constraint = gameObject.transform.FindChild("Content/Constraint").gameObject;
            CoolTime = gameObject.transform.FindChild("Content/CoolTime").GetComponent<UILabel>();
            gameObject.SetActive(false);
            Button1.onClick.Add(new EventDelegate(this, "onButton1"));
        }

        public void onButton1()
        {
            Action_Confirm();
        }

        public void Show(bool selected, ConsumableData data, bool isBuff)//NestedIcon Icon, string name, string description, int cooltime)
        {   // infoview는 select상태에서만 갱신해야한다.
            if (!selected)
                return;

            gameObject.SetActive(true);
            NestedIcon.Draw(data.icon, this.Icon);
            Name.text = data.name;
            Description.text = data.description;
            if(isBuff)
            {
                CoolTime.text = "지속시간 " + data.CoolTime.ToString() + "초";
            }
            else
            {
                if (data.CoolTime > 1)
                    CoolTime.text = "쿨타임 " + data.CoolTime.ToString() + "초";
                else
                    CoolTime.text = "";
            }

            Constraint.SetActive(false);
        }

        public void PrintConstraint(string constrainttext)
        {
            Constraint.GetComponent<UILabel>().text = constrainttext;
            Constraint.SetActive(true);
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
