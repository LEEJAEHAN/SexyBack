using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SexyBackMenuScene;
using System;

namespace SexyBackMenuScene
{
    public class TalentWindow : MonoBehaviour
    {
        Transform Slot;
        GameObject LearnButton;
        GameObject JobChangeButton;

        string selectedID;

        public void Awake()
        {
            Slot = transform.FindChild("Slot/ScrollView/Table");
            LearnButton = transform.FindChild("ButtonSet/Button1").gameObject;
            JobChangeButton = transform.FindChild("ButtonSet/Button2").gameObject;
            Singleton<TalentManager>.getInstance().BindView(this);
        }
        public void OnEnable()
        {
            SetPosition();
            RefreshList(Singleton<TalentManager>.getInstance().Talents, Singleton<TalentManager>.getInstance().TotalLevel);
            // initaa button state;
        }

        internal void SetPosition()
        {
            transform.localPosition = Vector3.zero;
        }

        public void onToggle(string id, bool toggle)
        {
            if (toggle)
            {
                selectedID = id;
                LearnButton.SetActive(!Singleton<TalentManager>.getInstance().IsMaxLevel(selectedID));
                LearnButton.GetComponent<UIButton>().isEnabled = Singleton<TalentManager>.getInstance().CanBuy(selectedID);
                JobChangeButton.SetActive(Singleton<TalentManager>.getInstance().CanChangeJob(selectedID));
            }
        }

        public void onLearnButton()
        {
            ViewLoader.PopUpPanel.SetActive(true);
            ViewLoader.MakePopUp("주의", "[000000]선택항목을 구매하시겠습니까?\n(명성이 소모됩니다)[-]", LearnConfirm, onCancel);

        }
        private void onCancel()
        {
            ViewLoader.PopUpPanel.SetActive(false);
        }
        public void onJobChange()
        {
            Singleton<TalentManager>.getInstance().JobChange(selectedID);
        }
        private void LearnConfirm()
        {
            Singleton<TalentManager>.getInstance().Learn(selectedID);
            ViewLoader.PopUpPanel.SetActive(false);
        }

        public void onResetButton()
        {
            Singleton<TalentManager>.getInstance().Reset();
        }

        /// <summary>
        /// 드로잉 메서드
        /// </summary>
        /// 
        internal void RefreshList(SortedDictionary<string, Talent> MyTalent, int totalLevel)
        {
            Slot.DestroyChildren();
            selectedID = null;
            LearnButton.GetComponent<UIButton>().isEnabled = false;
            JobChangeButton.SetActive(false);

            foreach (var a in Singleton<TableLoader>.getInstance().talenttable)
            {
                if (MyTalent.ContainsKey(a.Key))
                    FillTalent(MyTalent[a.Key]);
                else
                    FillTalent(new Talent(a.Value, 0));
            }
            Slot.GetComponent<UITable>().Reposition();
        }

        private void FillTalent(Talent talent)
        {
            GameObject EquipView = ViewLoader.InstantiatePrefab(Slot, talent.ID, "Prefabs/UI/TalentGridITem");
            EquipView.transform.FindChild("Class").GetComponent<UILabel>().text =
                talent.Level > 0 ? string.Format("{0} Lv.{1}", talent.Name, talent.Level) : talent.Name;
            EquipView.transform.FindChild("Bonus").GetComponent<UILabel>().text = StringParser.GetAttributeString(talent.BonusPerLevel);
            EquipView.transform.FindChild("JobBonus").GetComponent<UILabel>().text = StringParser.GetAttributeString(talent.JobBonus);

            if(talent.IsMaxLevel)
            {
                EquipView.transform.FindChild("Price").GetComponent<UILabel>().text = "최대레벨";
                EquipView.transform.FindChild("Price/Icon").gameObject.SetActive(false);
            }
            else
            {
                EquipView.transform.FindChild("Price").GetComponent<UILabel>().text = talent.NextPrice.ToString();
            }
            SetToggleEvent(EquipView, "onToggle");
        }

        public void SetToggleEvent(GameObject equipView, string eventMethod)
        {
            EventDelegate Ehandler = new EventDelegate(this, eventMethod);
            EventDelegate.Parameter Eparam1 = new EventDelegate.Parameter();
            Eparam1.obj = equipView;
            Eparam1.field = "name";
            EventDelegate.Parameter Eparam2 = new EventDelegate.Parameter();
            Eparam2.obj = equipView.GetComponent<UIToggle>();
            Eparam2.field = "value";
            Ehandler.parameters[0] = Eparam1;
            Ehandler.parameters[1] = Eparam2;
            equipView.GetComponent<UIToggle>().onChange.Add(Ehandler);
        }


    }
}
