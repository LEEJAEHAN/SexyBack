using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class GridItem : IDisposable
    {
        string Type;
        GameObject avatar;
        Transform gridPanel;
        GridItemView view; // viewScript

        //member
        UIButton Confirm = ViewLoader.Button_Confirm.GetComponent<UIButton>();
        GameObject Info_Window = ViewLoader.Info_Context;
        UISprite Info_Icon = ViewLoader.Info_Icon.GetComponent<UISprite>();
        UILabel Info_Description = ViewLoader.Info_Description.GetComponent<UILabel>();

        // research bar
        UIProgressBar ResearchBar;
        UILabel RBar_Time;
        UISprite RBar_Fill1;

        bool isEnable = true;

        public GridItem(string type, string id, string IconName, GameObject parent, IHasGridItem eventListner)
        {
            GameObject prefab;
            Type = type;

            if (type == "Research")
                prefab = Resources.Load<GameObject>("Prefabs/UI/ResearchView") as GameObject;
            else
                prefab = Resources.Load<GameObject>("Prefabs/UI/LevelUpItemView") as GameObject;

            // Instantiate object
            avatar = GameObject.Instantiate<GameObject>(prefab) as GameObject;
            avatar.name = id;

            GameObject iconObject = avatar.transform.FindChild("Icon").gameObject;
            iconObject.GetComponent<UISprite>().atlas = Resources.Load("Atlas/IconImage", typeof(UIAtlas)) as UIAtlas;
            iconObject.GetComponent<UISprite>().spriteName = IconName;

            // set event
            view = avatar.GetComponent<GridItemView>();
            view.Action_ConfirmGridItem += eventListner.onConfirm;
            view.Action_SelectGridItem += eventListner.onSelect; // 매니져가 안받는다.

            // set parents
            gridPanel = parent.transform;
            avatar.transform.parent = gridPanel;
            avatar.transform.localScale = gridPanel.localScale;

            SetMember();
        }


        private void SetMember()
        {
            if (Type == "Research")
            {
                ResearchBar = avatar.transform.FindChild("RBar").GetComponent<UIProgressBar>();
                RBar_Time = avatar.transform.FindChild("RBar").FindChild("RBar_Time").GetComponent<UILabel>();
                RBar_Fill1 = avatar.transform.FindChild("RBar").FindChild("RBar_Fill1").GetComponent<UISprite>();
            }
        }
        public void Disable()
        {
            if (!isEnable)
                return;
            avatar.GetComponent<UISprite>().color = new Color(0.5f, 0.5f, 0.5f, 0.8f);
            isEnable = false;
        }
        public void Enable()
        {
            if (isEnable)
                return;
            avatar.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
            isEnable = true;
        }
        public void SetActive(bool value)
        {
            if (value && !avatar.activeInHierarchy) // Active가 아닐때에만 Active시킨다.
            {
                avatar.SetActive(true);
            }
            if (!value)
            {
                avatar.SetActive(false);
            }
        }

        // only for research prefab;
        public void SetRBar(float progress, int time, bool colorflag)
        {
            ResearchBar.value = progress;
            RBar_Time.text = time.ToString() + " sec";
            if (colorflag)
                RBar_Fill1.color = new Color(1, 0, 0, 1);
            else
                RBar_Fill1.color = new Color(0.5f, 0.5f, 0.5f, 1);
        }

        public void ConfirmEnable(bool selected)
        { // infoview는 select상태에서만 갱신해야한다.
            if (!selected || Confirm.enabled)
                return;

            Confirm.enabled = true;
            Confirm.SetState(UIButtonColor.State.Normal, true);
        }

        public void ConfirmDisable(bool selected)
        { // infoview는 select상태에서만 갱신해야한다.
            if (!selected || !Confirm.enabled)
                return;
            Confirm.enabled = false;
            Confirm.SetState(UIButtonColor.State.Disabled, true);
        }

        public void FillInfo(bool selected, string Icon, string Descrption)
        {   // infoview는 select상태에서만 갱신해야한다.
            if (!selected)
                return;
            Info_Window.SetActive(true);
            Info_Icon.spriteName = Icon;
            Info_Description.text = Descrption;
        }

        // only for level up
        public void FillItemContents(string buttontext)
        {
            GameObject labelObject = avatar.transform.FindChild("Label").gameObject;
            labelObject.GetComponent<UILabel>().text = buttontext; // 최초에 그리기용
        }

        public void ClearInfo()
        {
            if (Info_Window.activeInHierarchy)
                Info_Window.SetActive(false);
        }

        public void Dispose()
        {
            GameObject.Destroy(avatar);
        }

    }
}