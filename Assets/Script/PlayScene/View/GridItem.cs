using System;
using UnityEngine;


namespace SexyBackPlayScene
    {

    public class GridItem : IDisposable //TODO : 두가지타입 상속으로바꾼다.
    {
        public enum Type
        {
            LevelUp,
            Research,
            Consumable
        }

        Type type;
        GameObject avatar;
        Transform gridPanel; // parent
        UISprite background;

        // LevelUp Bar member
        UILabel LevelText;

        // research bar member
        UIProgressBar ResearchBar;
        UILabel RBar_Time;
        UISprite RBar_Fill1;

        // consumable
        UIProgressBar CoolTimeBar;
        UILabel StackText;


        bool isEnable = true;

        ~GridItem() { sexybacklog.Console("아이템소멸"); }
        public GridItem(Type type, string id, NestedIcon icon, IHasGridItem eventListner)
        {
            GameObject prefab;
            this.type = type;

            if (type == Type.Research)
            {
                prefab = Resources.Load<GameObject>("Prefabs/UI/ResearchGridItem") as GameObject;
                gridPanel = Singleton<ResearchManager>.getInstance().Tab2Container.transform;
            }
            else if(type == Type.LevelUp)
            {
                prefab = Resources.Load<GameObject>("Prefabs/UI/LevelUpGridItem") as GameObject;
                gridPanel = Singleton<LevelUpManager>.getInstance().Tab1Container.transform;
            }
            else // consumable
            {
                prefab = Resources.Load<GameObject>("Prefabs/UI/ConsumableGridItem") as GameObject;
                gridPanel = Singleton<ConsumableManager>.getInstance().Tab3Container.transform;
            }

            // Instantiate object
            avatar = GameObject.Instantiate<GameObject>(prefab) as GameObject;
            avatar.name = id;
            NestedIcon.Draw(icon, avatar.transform.FindChild("Icon").gameObject);

            // set parents
            avatar.transform.parent = gridPanel;
            avatar.transform.localScale = gridPanel.localScale;
    
            // set event;
            avatar.GetComponent<GridItemView>().Action_SelectGridItem += eventListner.onSelect; // 매니져가 안받는다.

            SetMember();
        }

        private void SetMember()
        {
            background = avatar.transform.FindChild("Back").GetComponent<UISprite>();
            if (type == Type.Research)
            {
                ResearchBar = avatar.transform.FindChild("RBar").GetComponent<UIProgressBar>();
                RBar_Time = avatar.transform.FindChild("RBar").FindChild("RBar_Time").GetComponent<UILabel>();
                RBar_Fill1 = avatar.transform.FindChild("RBar").FindChild("RBar_Fill1").GetComponent<UISprite>();
            }
            else if (type == Type.LevelUp)
            {
                LevelText = avatar.transform.FindChild("Label").GetComponent<UILabel>();
            }
            else //consumable
            {
                CoolTimeBar = avatar.GetComponent<UIProgressBar>();
                StackText = avatar.transform.FindChild("Stack").GetComponent<UILabel>();
            }
        }
        public void Disable()
        {
            if (!isEnable)
                return;
                background.applyGradient = true;
            isEnable = false;
        }
        public void Enable()
        {
            if (isEnable)
                return;
                background.applyGradient = false;
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
        public void DrawRBar(float progress, int time, bool colorflag)
        {
            if (type != Type.Research)
                return;
            ResearchBar.value = progress;
            RBar_Time.text = time.ToString() + " sec";
            if (colorflag)
                RBar_Fill1.color = new Color(1, 0, 0, 1);
            else
                RBar_Fill1.color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
        public void HideRBar()
        {
            if (type != Type.Research)
                return;
            ResearchBar.gameObject.transform.GetComponent<UIWidget>().height = 15;
            ResearchBar.gameObject.SetActive(false);
        }

        // only for level up
        public void DrawLevel(string buttontext, bool bonuses)
        {
            if (type != Type.LevelUp)
                return;
            if (bonuses)
                LevelText.text = "[F9DB11FF]" + buttontext + "[-]";
            else
                LevelText.text = buttontext; // 최초에 그리기용
        }

        // only for Consumable
        public void DrawStack(int stack)
        {
            if (type != Type.Consumable)
                return;
            StackText.text = StringParser.GetStackText(stack); // 최초에 그리기용
        }
        public void DrawCoolMask(float progress)
        {
            if (type != Type.Consumable)
                return;
            CoolTimeBar.value = progress;
        }

        public void Dispose()
        {
            GameObject.Destroy(avatar);
        }

    }
}