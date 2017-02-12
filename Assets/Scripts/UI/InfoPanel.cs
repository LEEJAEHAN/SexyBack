using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class InfoPanel // singleton 사용
    {
        UIButton Confirm = ViewLoader.Button_Confirm.GetComponent<UIButton>();
        GameObject Pause = ViewLoader.Button_Pause;

        GameObject Info_Window = ViewLoader.Info_Context;
        UISprite Info_Icon = ViewLoader.Info_Icon.GetComponent<UISprite>();
        UILabel Info_Description = ViewLoader.Info_Description.GetComponent<UILabel>();


        public void Show(bool selected, string Icon, string Descrption)
        {   // infoview는 select상태에서만 갱신해야한다.
            if (!selected)
                return;
            Info_Window.SetActive(true);
            Info_Icon.spriteName = Icon;
            Info_Description.text = Descrption;
        }

        public void Hide()
        {
            if (Info_Window.activeInHierarchy)
                Info_Window.SetActive(false);
        }

        internal void SetPauseButton(bool selected, bool value, string text)
        {
            if (!selected)
                return;

            Pause.SetActive(value);
            if (value)
                Pause.transform.GetChild(0).GetComponent<UILabel>().text = text;
        }

        public void SetConfirmButton(bool selected, bool value)
        {
            if (!selected || Confirm.enabled == value)
                return;

            Confirm.enabled = value;
            if (value)
                Confirm.SetState(UIButtonColor.State.Normal, true);
            else
                Confirm.SetState(UIButtonColor.State.Disabled, true);
        }


    }
}
