using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class InfoPanel // singleton 사용
    {
        GameObject Info_Context;
        GameObject Button_Confirm;
        GameObject Button_Pause;
        GameObject Info_Icon;
        GameObject Info_Description;

        UIButton Button1;
        UIButton Button2;

        UILabel Info_Description_Label;
        public InfoPanel()
        {
            Info_Context = GameObject.Find("Info_Context");
            Info_Icon = GameObject.Find("Info_Icon");
            Info_Description = GameObject.Find("Info_Description");

            Button_Confirm = GameObject.Find("Button_Confirm");
            Button_Pause = GameObject.Find("Button_Pause");
            Button_Pause.SetActive(false);

            Button1 = Button_Confirm.GetComponent<UIButton>();
            Button2 = Button_Pause.GetComponent<UIButton>();
            Info_Description_Label = Info_Description.GetComponent<UILabel>();
        }

        public void Show(bool selected, GridItemIcon Icon, string Descrption)
        {   // infoview는 select상태에서만 갱신해야한다.
            if (!selected)
                return;
            Info_Context.SetActive(true);
            Icon.Draw(Info_Icon);
            Info_Description_Label.text = Descrption;
        }

        internal void SetButton1Event(EventDelegate eventDelegate)
        {
            Button1.GetComponent<UIButton>().onClick.Clear();
            Button1.onClick.Add(eventDelegate);
        }

        internal void SetButton2Event(EventDelegate eventDelegate)
        {
            Button2.onClick.Clear();
            Button2.onClick.Add(eventDelegate);
        }

        public void Hide()
        {
            if (Info_Context.activeInHierarchy)
                Info_Context.SetActive(false);
        }

        internal void SetPauseButton(bool selected, bool value, string text)
        {
            if (!selected)
                return;

            Button_Pause.SetActive(value);
            if (value)
                Button_Pause.transform.GetChild(0).GetComponent<UILabel>().text = text;
        }

        public void SetConfirmButton(bool selected, bool value)
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
