using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SexyBackMenuScene;
using System;

namespace SexyBackMenuScene
{
    public class EquipmentWindow : MonoBehaviour
    {
        public List<int> CheckList = new List<int>();
        public EquipmentStateMachine statemachine;

        public void Awake()
        {
            statemachine = new EquipmentStateMachine(transform, this);
            Singleton<EquipmentManager>.getInstance().BindView(this);
        }
        public void OnEnable()
        {
            SetPosition();
            ClearWindow();
            FillInventory(Singleton<EquipmentManager>.getInstance().inventory, Singleton<EquipmentManager>.getInstance().MaxInventory);
            FillEquipments(Singleton<EquipmentManager>.getInstance().currentEquipSet, Singleton<EquipmentManager>.getInstance().EquipSetIndex);
            statemachine.ChangeMode(EquipmentState.None);
        }

        internal void ForceSelect(bool isInventory, string index)
        {
            if (isInventory)
            {
                transform.FindChild("인벤토리/ScrollView/Grid/" + index).gameObject.GetComponent<UIToggle>().Set(true, true);
            }
            else
            {
                //transform.FindChild("장비슬롯/Slot_" + index + "/" + index).gameObject.GetComponent<UIToggle>().value = true;
                transform.FindChild("장비슬롯/Slot_" + index + "/" + index).gameObject.GetComponent<UIToggle>().Set(true, true);
            }
        }

        public void onEquipToggle(string part, bool toggle)
        {
            if (toggle)
            {
                if (Singleton<EquipmentManager>.getInstance().SelectEquipment(part))
                    statemachine.ChangeMode(EquipmentState.EquipSelected);
            }
        }

        public void onInvenToggle(string index, bool toggle)
        {
            if (toggle)
            {
                if (Singleton<EquipmentManager>.getInstance().SelectInventory(index))
                    statemachine.ChangeMode(EquipmentState.InvenSelected);
            }
        }

        public void onInvenCheck(string index, bool check) // no state transition
        {
            int i = int.Parse(index);
            if (check && !CheckList.Contains(i))
                CheckList.Add(i);
            else if (!check && CheckList.Contains(i))
                CheckList.Remove(i);
            else
                return;

            Singleton<EquipmentManager>.getInstance().CalExpectedView(CheckList, statemachine.Mode);
            transform.FindChild("ButtonSet/Set2/Button1").GetComponent<UIButton>().isEnabled = CheckList.Count > 0;
        }
        public void onRightButton()
        {
            Singleton<EquipmentManager>.getInstance().SwapEquipSet(true);
            if (statemachine.Mode == EquipmentState.EquipSelected)
                statemachine.ChangeMode(EquipmentState.None);
        }
        public void onLeftButton()
        {
            Singleton<EquipmentManager>.getInstance().SwapEquipSet(false);
            if (statemachine.Mode == EquipmentState.EquipSelected)
                statemachine.ChangeMode(EquipmentState.None);
        }

        public void onEquipButton()
        {
            Singleton<EquipmentManager>.getInstance().EquipSelectedItem();
        }
        public void onUnEquipButton()
        {
            Singleton<EquipmentManager>.getInstance().UnEquipSelectedItem();
        }
        public void onEnchantButton()
        {
            statemachine.ChangeMode(EquipmentState.EnchantMode);
        }
        public void onEvolutionButton()
        {
            statemachine.ChangeMode(EquipmentState.EvolutionMode);
        }
        public void onLock()
        {
            bool islock = Singleton<EquipmentManager>.getInstance().Lock();
            transform.FindChild("ButtonSet/Set1/Table/Button4/Label").GetComponent<UILabel>().text = EquipmentWiki.LockToString(islock);
        }
        public void onEnchantConfirm()
        {
            statemachine.SubMode(EquipmentState.Working, true);
            Singleton<EquipmentManager>.getInstance().Enchant(CheckList); // 인첸트, 드로잉안함.
        }
        public void onEvolutionConfirm()
        {
            statemachine.SubMode(EquipmentState.Working, true);
            Singleton<EquipmentManager>.getInstance().Evolution(CheckList[0]); // 인첸트, 드로잉안함.
        }
        public void onFinishWork()
        {
            statemachine.SubMode(EquipmentState.Working, false);
            FillInventory(Singleton<EquipmentManager>.getInstance().inventory, Singleton<EquipmentManager>.getInstance().MaxInventory); // 인첸트 후 드로잉
            statemachine.UndoMode();
        }
        public void onCancel() // CancleMode
        {
            statemachine.UndoMode();
        }

        /// <summary>
        /// 드로잉 메서드
        /// </summary>
        internal void FillEquipments(Dictionary<Equipment.Type, Equipment> equipments, int setIndex)
        {
            transform.FindChild("장비슬롯/Slot_Name").GetComponent<UILabel>().text = "장비편성 " + (setIndex + 1).ToString();
            FillEquipment(transform.FindChild("장비슬롯/Slot_Weapon").gameObject, Equipment.Type.Weapon, equipments);
            FillEquipment(transform.FindChild("장비슬롯/Slot_Staff").gameObject, Equipment.Type.Staff, equipments);
            FillEquipment(transform.FindChild("장비슬롯/Slot_Ring").gameObject, Equipment.Type.Ring, equipments);
        }
        private void FillEquipment(GameObject Slot, Equipment.Type part, Dictionary<Equipment.Type, Equipment> equipments)
        {
            Slot.transform.DestroyChildren();
            if (equipments.ContainsKey(part))
            {
                GameObject Weapon = MakeGridView(Slot.transform, part.ToString(), equipments[part].iconID, "onEquipToggle");
                Weapon.transform.GetComponent<UIWidget>().width = Slot.transform.GetComponent<UIWidget>().width;
            }
        }
        public void FillInventory(List<Equipment> items, int maxCount)
        {
            GameObject inventoryView = transform.FindChild("인벤토리/ScrollView/Grid").gameObject;
            inventoryView.transform.DestroyChildren();
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == null)
                    continue;
                GameObject EquipView = MakeGridView(inventoryView.transform, i.ToString(), items[i].iconID, "onInvenToggle");
            }
            inventoryView.GetComponent<UIGrid>().Reposition();
            UILabel capacity = transform.FindChild("인벤토리/ScrollView/Capacity").GetComponent<UILabel>();
            capacity.text = "최대 소지 한도 수 " + items.Count + " / " + maxCount;
            if (items.Count >= maxCount)
            {
                capacity.color = Color.red;
                // 경고메시지 출력
            }
            else
                capacity.color = Color.yellow;

            transform.FindChild("인벤토리/ScrollView").GetComponent<UIScrollView>().ResetPosition();
        }
        public void FillExpectedSelect(Equipment e, int NextExp, int NextEvolution) // 강화나 각성시 사전정보 들어오기.
        {
            if (NextExp > e.MaxExp)
                NextExp = e.MaxExp;
            int MaxExp = EquipmentWiki.CalMaxExp(e.grade, NextEvolution);

            Transform info = transform.FindChild("아이템정보");
            info.gameObject.SetActive(true);
            e.DrawIconView(info.FindChild("Icon").GetComponent<UISprite>(), info.FindChild("Name").GetComponent<UILabel>(), NextEvolution);
            info.FindChild("Lock").GetComponent<UISprite>().gameObject.SetActive(e.isLock);
            if (NextExp > e.exp || NextEvolution > e.evolution)
                info.FindChild("Stat").GetComponent<UILabel>().color = Color.blue;
            else
                info.FindChild("Stat").GetComponent<UILabel>().color = Color.black;
            if (NextEvolution > e.evolution) // 각성 예
            {
                //info.FindChild("Name").GetComponent<UILabel>().color = Color.blue;
                NextExp = 0;
                info.FindChild("EnchantBar").GetComponent<UIProgressBar>().value = 0;
            }
            else // 강화 예
            {
                //info.FindChild("Name").GetComponent<UILabel>().color = Color.black;
                info.FindChild("EnchantBar").GetComponent<UIProgressBar>().value = (float)e.exp / (float)MaxExp;
            }
            info.FindChild("Stat").GetComponent<UILabel>().text = e.ExpectStat(NextExp, NextEvolution).ToString();
            info.FindChild("EnchantBar/RBar_Fill2").GetComponent<UISprite>().fillAmount = (float)NextExp / (float)EquipmentWiki.CalMaxExp(e.grade, NextEvolution);
            info.FindChild("EnchantBar/RBar_Text").GetComponent<UILabel>().text = EquipmentWiki.CalExpPercent(NextExp, MaxExp) + " 강화";

            info.FindChild("SkillName").GetComponent<UILabel>().text = e.skillName + " Lv." + e.skillLevel.ToString();
            info.FindChild("SkillStat").GetComponent<UILabel>().text = EquipmentWiki.AttributeBox(e.SkillStat);
        }
        public void FillSelected(Equipment selected)
        {
            FillExpectedSelect(selected, selected.exp, selected.evolution);
        }
        private void ClearWindow()
        {
            transform.FindChild("아이템정보").gameObject.SetActive(false);
            transform.FindChild("장비슬롯/Mask").gameObject.SetActive(false);
            transform.FindChild("Text").GetComponent<UILabel>().text = "";
            transform.FindChild("ButtonSet/Set1").gameObject.SetActive(false);
            transform.FindChild("ButtonSet/Set2").gameObject.SetActive(false);
            transform.FindChild("장비슬롯/Slot_Weapon").DestroyChildren();
            transform.FindChild("장비슬롯/Slot_Staff").DestroyChildren();
            transform.FindChild("장비슬롯/Slot_Ring").DestroyChildren();
            transform.FindChild("인벤토리/ScrollView/Grid").DestroyChildren();
        }
        private GameObject MakeGridView(Transform Parent, string Name, string iconID, string EventMethod)
        {
            GameObject EquipView = ViewLoader.InstantiatePrefab(Parent, Name, "Prefabs/UI/Equipment");
            SetToggleEvent(EquipView, EventMethod);
            EquipView.transform.FindChild("Icon").GetComponent<UISprite>().spriteName = iconID;
            return EquipView;
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
        internal void SetPosition()
        {
            transform.localPosition = Vector3.zero;
        }


    }
}
