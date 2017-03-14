using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class GridItem : IDisposable //TODO : 두가지타입 상속으로바꾼다.
    {
        string Type;

        GameObject avatar;
        Transform gridPanel; // parent
        GridItemView eventScript; // viewScript

        //member
        // research bar
        UIProgressBar ResearchBar;
        UILabel RBar_Time;
        UISprite RBar_Fill1;

        bool isEnable = true;

        ~GridItem() { sexybacklog.Console("아이템소멸"); }
        public GridItem(string type, string id, GridItemIcon icon, GameObject parent)
        {
            GameObject prefab;
            Type = type;

            if (type == "Research")
                prefab = Resources.Load<GameObject>("Prefabs/UI/ResearchGridItem") as GameObject;
            else
                prefab = Resources.Load<GameObject>("Prefabs/UI/LevelUpGridItem") as GameObject;

            // Instantiate object
            avatar = GameObject.Instantiate<GameObject>(prefab) as GameObject;
            avatar.name = id;

            // set icon
            GameObject iconObject = avatar.transform.FindChild("Icon").gameObject;
            GridItemIcon.Draw(icon, iconObject);

            // set event
            eventScript = avatar.GetComponent<GridItemView>();

            // set parents
            gridPanel = parent.transform;
            avatar.transform.parent = gridPanel;
            avatar.transform.localScale = gridPanel.localScale;

            SetMember();
        }
        public void AttachEventListner(IHasGridItem eventListner)
        {
            eventScript.Action_SelectGridItem += eventListner.onSelect; // 매니져가 안받는다.
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
                avatar.transform.FindChild("Back").GetComponent<UISprite>().applyGradient = true;
            isEnable = false;
        }
        public void Enable()
        {
            if (isEnable)
                return;
                avatar.transform.FindChild("Back").GetComponent<UISprite>().applyGradient = false;
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
        public void ShowRBar(float progress, int time, bool colorflag)
        {
            ResearchBar.value = progress;
            RBar_Time.text = time.ToString() + " sec";
            if (colorflag)
                RBar_Fill1.color = new Color(1, 0, 0, 1);
            else
                RBar_Fill1.color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
        public void HideRBar()
        {
            ResearchBar.gameObject.transform.GetComponent<UIWidget>().height = 15;
            ResearchBar.gameObject.SetActive(false);
        }

        // only for level up
        public void FillItemContents(string buttontext)
        {
            GameObject labelObject = avatar.transform.FindChild("Label").gameObject;
            labelObject.GetComponent<UILabel>().text = buttontext; // 최초에 그리기용
        }

        public void Dispose()
        {
            GameObject.Destroy(avatar);
        }

    }
}