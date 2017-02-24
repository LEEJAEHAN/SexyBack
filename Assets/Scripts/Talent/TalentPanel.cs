using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class TalentPanel : MonoBehaviour
    {
        GameObject PanelObject;

        UILabel TalentA_Level;
        UILabel TalentE_Level;
        UILabel TalentU_Level;

        Transform TalentA_Slot;
        Transform TalentE_Slot;
        Transform TalentU_Slot;

        GameObject RefreshButton;

        private static TalentPanel instance;
        public static TalentPanel getInstance
        {
            get
            {
                if (instance == null)
                    instance = GameObject.Find("Talent_PopUp").AddComponent<TalentPanel>();

                return instance;
            }
        }

        internal void Init()
        {
            PanelObject = GameObject.Find("Talent_PopUp");
            TalentA_Level = PanelObject.transform.FindChild("Container/Table/Grid/TalentA_Dialog/TalentA_Value").GetComponent<UILabel>();
            TalentE_Level = PanelObject.transform.FindChild("Container/Table/Grid/TalentE_Dialog/TalentE_Value").GetComponent<UILabel>();
            TalentU_Level = PanelObject.transform.FindChild("Container/Table/Grid/TalentU_Dialog/TalentU_Value").GetComponent<UILabel>();

            TalentA_Slot = PanelObject.transform.FindChild("Container/Table/Grid/TalentA_Dialog/TalentA_Slot").transform;
            TalentE_Slot = PanelObject.transform.FindChild("Container/Table/Grid/TalentE_Dialog/TalentE_Slot").transform;
            TalentU_Slot = PanelObject.transform.FindChild("Container/Table/Grid/TalentU_Dialog/TalentU_Slot").transform;

            RefreshButton = PanelObject.transform.FindChild("Container/Table/Container/Button_Change").gameObject;

            TalentA_Slot.parent.gameObject.GetComponent<UIButton>().onClick.Add(new EventDelegate(this, "onClickA"));
            TalentE_Slot.parent.gameObject.GetComponent<UIButton>().onClick.Add(new EventDelegate(this, "onClickE"));
            TalentU_Slot.parent.gameObject.GetComponent<UIButton>().onClick.Add(new EventDelegate(this, "onClickU"));
            RefreshButton.GetComponent<UIButton>().onClick.Add(new EventDelegate(this, "Refresh"));

            Hide();
        }

        internal void Hide()
        {
            ClaerSlot();
            PanelObject.SetActive(false);
        }

        internal void ClaerSlot()
        {
            TalentA_Slot.transform.DestroyChildren();
            TalentE_Slot.transform.DestroyChildren();
            TalentU_Slot.transform.DestroyChildren();
        }

        internal void FillTalents(Talent talent1, Talent talent2, Talent talent3)
        {
            GameObject talentAView = ViewLoader.InstantiatePrefab(TalentA_Slot, "TalentA", "Prefabs/UI/TalentView");
            GameObject talentEView = ViewLoader.InstantiatePrefab(TalentE_Slot, "TalentE", "Prefabs/UI/TalentView");
            GameObject talentUView = ViewLoader.InstantiatePrefab(TalentU_Slot, "TalentU", "Prefabs/UI/TalentView");

            talentAView.transform.FindChild("Description").GetComponent<UILabel>().text = talent1.Description;
            talentEView.transform.FindChild("Description").GetComponent<UILabel>().text = talent2.Description;
            talentUView.transform.FindChild("Description").GetComponent<UILabel>().text = talent3.Description;

            talent1.Icon.Draw(talentAView.transform.FindChild("Icon").gameObject);
            talent2.Icon.Draw(talentEView.transform.FindChild("Icon").gameObject);
            talent3.Icon.Draw(talentUView.transform.FindChild("Icon").gameObject);

        }
        public void onClickA()
        {
            Singleton<TalentManager>.getInstance().Confirm(TalentType.Attack);
        }
        public void onClickE()
        {
            Singleton<TalentManager>.getInstance().Confirm(TalentType.Element);
        }
        public void onClickU()
        {
            Singleton<TalentManager>.getInstance().Confirm(TalentType.Util);
        }

        internal void Show()
        {
            PanelObject.SetActive(true);
        }

        internal void Refresh()
        {
            Singleton<TalentManager>.getInstance().Refresh();
        }

        internal void FillWindow(int floor, int alevel, int elevel, int ulevel)
        {
            PanelObject.transform.FindChild("Container/Table/Talent_Title").GetComponent<UILabel>().text = floor.ToString() + "스테이지 클리어!";
            TalentA_Level.text = alevel.ToString();
            TalentE_Level.text = elevel.ToString();
            TalentU_Level.text = ulevel.ToString();
        }
    }
}