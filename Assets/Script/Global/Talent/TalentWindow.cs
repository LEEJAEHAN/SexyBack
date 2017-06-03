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
                LearnButton.GetComponent<UIButton>().isEnabled = Singleton<TalentManager>.getInstance().CanBuy(selectedID);
                JobChangeButton.SetActive(Singleton<TalentManager>.getInstance().CanChangeJob(selectedID));
            }
        }
        
        public void onLearnButton()
        {
            Singleton<TalentManager>.getInstance().Learn(selectedID);
        }
        public void onJobChange()
        {
            Singleton<TalentManager>.getInstance().JobChange(selectedID);
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
            EquipView.transform.FindChild("Class").GetComponent<UILabel>().text = talent.Name + " LV." + (talent.Level+1).ToString();
            EquipView.transform.FindChild("Bonus").GetComponent<UILabel>().text = StringParser.GetAttributeString(talent.BonusPerLevel);
            EquipView.transform.FindChild("JobBonus").GetComponent<UILabel>().text = StringParser.GetAttributeString(talent.JobBonus);
            EquipView.transform.FindChild("Price").GetComponent<UILabel>().text = 
                Singleton<TalentManager>.getInstance().GetNextPrice(talent.PriceCoef).ToString();
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
