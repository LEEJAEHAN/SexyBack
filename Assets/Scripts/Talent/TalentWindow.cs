using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class TalentWindow : MonoBehaviour
    {
        UILabel TalentA_Level;
        UILabel TalentE_Level;
        UILabel TalentU_Level;

        Transform TalentA_Slot;
        Transform TalentE_Slot;
        Transform TalentU_Slot;

        GameObject RefreshButton;

        private static TalentWindow instance;
        public static TalentWindow getInstance
        {
            get
            {
                if (instance == null)
                {
                    GameObject owner = ViewLoader.InstantiatePrefab(GameObject.Find("Middle_Area").transform, "TalentWindow", "Prefabs/UI/TalentWindow");
                    owner.transform.localPosition = new Vector3(24, -24, 0);
                    instance = owner.AddComponent<TalentWindow>();
                }

                return instance;
            }
        }

        internal void Awake()
        {
            TalentA_Level = gameObject.transform.FindChild("Container/Table/Grid/TalentA_Dialog/TalentA_Value").GetComponent<UILabel>();
            TalentE_Level = gameObject.transform.FindChild("Container/Table/Grid/TalentE_Dialog/TalentE_Value").GetComponent<UILabel>();
            TalentU_Level = gameObject.transform.FindChild("Container/Table/Grid/TalentU_Dialog/TalentU_Value").GetComponent<UILabel>();

            TalentA_Slot = gameObject.transform.FindChild("Container/Table/Grid/TalentA_Dialog/TalentA_Slot").transform;
            TalentE_Slot = gameObject.transform.FindChild("Container/Table/Grid/TalentE_Dialog/TalentE_Slot").transform;
            TalentU_Slot = gameObject.transform.FindChild("Container/Table/Grid/TalentU_Dialog/TalentU_Slot").transform;

            RefreshButton = gameObject.transform.FindChild("Container/Table/Container/Button_Change").gameObject;

            TalentA_Slot.parent.gameObject.GetComponent<UIButton>().onClick.Add(new EventDelegate(this, "onClickA"));
            TalentE_Slot.parent.gameObject.GetComponent<UIButton>().onClick.Add(new EventDelegate(this, "onClickE"));
            TalentU_Slot.parent.gameObject.GetComponent<UIButton>().onClick.Add(new EventDelegate(this, "onClickU"));
            RefreshButton.GetComponent<UIButton>().onClick.Add(new EventDelegate(this, "Refresh"));

            Hide();
        }

        internal void Hide()
        {
            ClaerSlot();
            gameObject.SetActive(false);
        }

        internal void ClaerSlot()
        {
            TalentA_Slot.transform.DestroyChildren();
            TalentE_Slot.transform.DestroyChildren();
            TalentU_Slot.transform.DestroyChildren();
        }

        internal void FillTalents(Talent talent1, Talent talent2, Talent talent3)
        {
            if(talent1 != null)
            {
                GameObject talentAView = ViewLoader.InstantiatePrefab(TalentA_Slot, "TalentA", "Prefabs/UI/TalentView");
                talentAView.transform.FindChild("Description").GetComponent<UILabel>().text = talent1.Description;
                GridItemIcon.Draw(talent1.Icon, talentAView.transform.FindChild("Icon").gameObject);
            }
            if(talent2 != null)
            {
                GameObject talentEView = ViewLoader.InstantiatePrefab(TalentE_Slot, "TalentE", "Prefabs/UI/TalentView");
                talentEView.transform.FindChild("Description").GetComponent<UILabel>().text = talent2.Description;
                GridItemIcon.Draw(talent2.Icon, talentEView.transform.FindChild("Icon").gameObject);
            }
            if(talent3 != null)
            {
                GameObject talentUView = ViewLoader.InstantiatePrefab(TalentU_Slot, "TalentU", "Prefabs/UI/TalentView");
                talentUView.transform.FindChild("Description").GetComponent<UILabel>().text = talent3.Description;
                GridItemIcon.Draw(talent3.Icon, talentUView.transform.FindChild("Icon").gameObject);
            }
        }
        public void onClickA()
        {
            Singleton<TalentManager>.getInstance().Confirm(TalentType.Attack);
        }
        public void onClickE()
        {
            Singleton<TalentManager>.getInstance().Confirm(TalentType.Elemental);
        }
        public void onClickU()
        {
            Singleton<TalentManager>.getInstance().Confirm(TalentType.Util);
        }

        internal void Show()
        {
            gameObject.SetActive(true);
        }

        internal void Refresh()
        {
            Singleton<TalentManager>.getInstance().Refresh();
        }

        internal void FillWindow(int floor, int alevel, int elevel, int ulevel)
        {
            gameObject.transform.FindChild("Container/Table/Talent_Title").GetComponent<UILabel>().text = floor.ToString() + "스테이지 클리어!";
            TalentA_Level.text = alevel.ToString();
            TalentE_Level.text = elevel.ToString();
            TalentU_Level.text = ulevel.ToString();
        }
    }
}