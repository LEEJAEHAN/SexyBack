using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SexyBackMenuScene;
using System;

public class EquipmentWindow : MonoBehaviour
{
    public State Mode;
    public List<int> CheckList = new List<int>();

    public enum State
    {
        None = 1,
        EquipSelected,
        InvenSelected,
        EnchantMode,
//        Enchanting,
        EvolutionMode
    }

    public void OnEnable()
    {
        SetPosition();
        ClearWindow();
        FillInventory(Singleton<EquipmentManager>.getInstance().inventory);
        FillEquipments(Singleton<EquipmentManager>.getInstance().equipments);
        Singleton<EquipmentManager>.getInstance().BindView(this);
        ChangeMode(State.None);
    }

    internal void ForceSelect(bool isInventory, string index)
    {
        if (isInventory)
        {
            transform.FindChild("인벤토리/ScrollView/Grid/" + index).gameObject.GetComponent<UIToggle>().Set(true, true);
        }
        else
        {
            //transform.FindChild("장비슬w롯/Slot_" + index + "/" + index).gameObject.GetComponent<UIToggle>().value = true;
            transform.FindChild("장비슬롯/Slot_" + index + "/" + index).gameObject.GetComponent<UIToggle>().Set(true, true);
        }
    }

    public void onEquipToggle(string part, bool toggle)
    {
        if(toggle)
        {
            if(Singleton<EquipmentManager>.getInstance().SelectEquipment(part))
                ChangeMode(State.EquipSelected);
        }
    }

    public void onInvenToggle(string index, bool toggle)
    {
        if(toggle)
        {
            if (Singleton<EquipmentManager>.getInstance().SelectInventory(index))
                ChangeMode(State.InvenSelected);
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

        Singleton<EquipmentManager>.getInstance().CheckInventory(CheckList);
        transform.FindChild("ButtonSet/Set2/Button1").GetComponent<UIButton>().isEnabled = CheckList.Count > 0;
    }

    public void onEquipButton()
    {
        Singleton<EquipmentManager>.getInstance().Equip();
    }
    public void onUnEquipButton()
    {
        Singleton<EquipmentManager>.getInstance().UnEquip();
    }
    public void onEnchantButton()
    {
        ChangeMode(State.EnchantMode);
    }
    public void onEnchantConfirm()
    {
        sexybacklog.Console("인챈팅시작");
        FillSelected(Singleton<EquipmentManager>.getInstance().Selected);
        transform.FindChild("아이템정보배경/Effect").gameObject.SetActive(true);
        transform.FindChild("아이템정보배경/Effect").GetComponent<TweenAlpha>().PlayForward();
        GameObject.Find("UICamera").GetComponent<UICamera>().eventReceiverMask = 0; // nothing

        Singleton<EquipmentManager>.getInstance().Enchant(CheckList); // 인첸트, 드로잉안함.
    }
    public void onEnchantFinish()
    {
        transform.FindChild("아이템정보배경/Effect").gameObject.SetActive(false);
        transform.FindChild("아이템정보배경/Effect").GetComponent<TweenAlpha>().ResetToBeginning();
        GameObject.Find("UICamera").GetComponent<UICamera>().eventReceiverMask = -1; // everything
        sexybacklog.Console("인챈팅끝");

        FillInventory(Singleton<EquipmentManager>.getInstance().inventory); // 인첸트 후 드로잉
        onCancel();
    }
    public void onCancel() // CancleMode
    {
        EndMode(Mode);
        Mode = Singleton<EquipmentManager>.getInstance().GetPrevMode();
        StartMode(Mode);
    }

    public void ChangeMode(State state)
    {
        EndMode(Mode);
        Mode = state;
        StartMode(Mode);
    }

    private void StartMode(State state)
    {
        if (state == State.None)
        {
            transform.FindChild("아이템정보").gameObject.SetActive(false);
            transform.FindChild("ButtonSet/Set1").gameObject.SetActive(false);
            transform.FindChild("ButtonSet/Set2").gameObject.SetActive(false);
        }
        else if (state == State.InvenSelected)
        {
            FillSelected(Singleton<EquipmentManager>.getInstance().Selected);
            transform.FindChild("ButtonSet/Set1").gameObject.SetActive(true);
            transform.FindChild("ButtonSet/Set1/Table/Button1/Label").GetComponent<UILabel>().text = "장착";
            transform.FindChild("ButtonSet/Set1/Table/Button1").GetComponent<UIButton>().onClick.Add(new EventDelegate(onEquipButton));
        }
        else if (state == State.EquipSelected)
        {
            FillSelected(Singleton<EquipmentManager>.getInstance().Selected);
            transform.FindChild("ButtonSet/Set1").gameObject.SetActive(true);
            transform.FindChild("ButtonSet/Set1/Table/Button1/Label").GetComponent<UILabel>().text = "해제";
            transform.FindChild("ButtonSet/Set1/Table/Button1").GetComponent<UIButton>().onClick.Add(new EventDelegate(onUnEquipButton));
        }
        else if (state == State.EnchantMode)
        {
            FillSelected(Singleton<EquipmentManager>.getInstance().Selected);
            transform.FindChild("Text").GetComponent<UILabel>().text = "재료로 사용할 장비를 선택해 주세요.\n<주의> 재료로 사용되는 장비는 사라지며 복구되지 않습니다.";
            transform.FindChild("ButtonSet/Set2").gameObject.SetActive(true);
            transform.FindChild("ButtonSet/Set2/Button1").GetComponent<UIButton>().onClick.Add(new EventDelegate(onEnchantConfirm));
            transform.FindChild("ButtonSet/Set2/Button1").GetComponent<UIButton>().isEnabled = false;
            transform.FindChild("ButtonSet/Set2/Button2").GetComponent<UIButton>().onClick.Add(new EventDelegate(onCancel));
            transform.FindChild("장비슬롯/Mask").gameObject.SetActive(true);
            CheckList.Clear();
            Transform inventory = transform.FindChild("인벤토리/ScrollView/Grid");
            for (int i = 0; i < inventory.childCount; i++)
            {
                Transform view = inventory.GetChild(i);
                if(i == Singleton<EquipmentManager>.getInstance().SelectIndex)
                {
                    view.FindChild("Mask").gameObject.SetActive(true);
                    view.GetComponent<UIToggle>().Set(true, false);
                }
                else
                {
                    // change select event and toggle event
                    view.GetComponent<UIToggle>().group = 0;
                    view.GetComponent<UIToggle>().Set(false, false);
                    view.GetComponent<UIToggle>().onChange.Clear();
                    view.GetComponent<UIToggle>().activeSprite = view.FindChild("Check").GetComponent<UISprite>();
                    SetEvent(view.gameObject, "onInvenCheck");
                }
                if (Singleton<EquipmentManager>.getInstance().inventory[i].Lock)
                    view.FindChild("Lock").gameObject.SetActive(true);
            }
        }
    }


    private void EndMode(State state)
    {
        if (state == State.None)
        {
            transform.FindChild("아이템정보").gameObject.SetActive(true);
        }
        else if (state == State.InvenSelected || state == State.EquipSelected)
        {
            transform.FindChild("ButtonSet/Set1").gameObject.SetActive(false);
            transform.FindChild("ButtonSet/Set1/Table/Button1").GetComponent<UIButton>().onClick.Clear();
        }
        else if (state == State.EnchantMode)
        {
            transform.FindChild("Text").GetComponent<UILabel>().text = "";
            transform.FindChild("ButtonSet/Set2").gameObject.SetActive(false);
            transform.FindChild("ButtonSet/Set2/Button1").GetComponent<UIButton>().onClick.Clear();
            transform.FindChild("ButtonSet/Set2/Button2").GetComponent<UIButton>().onClick.Clear();
            transform.FindChild("장비슬롯/Mask").gameObject.SetActive(false);
            CheckList.Clear();

            Transform inventory = transform.FindChild("인벤토리/ScrollView/Grid");
            for (int i = 0; i < inventory.childCount; i++)
            {
                Transform view = inventory.GetChild(i);
                view.GetComponent<UIToggle>().group = 2;

                if (i == Singleton<EquipmentManager>.getInstance().SelectIndex)
                {
                    view.GetComponent<UIToggle>().Set(true, false);
                }
                else
                {
                    // change select event and toggle event
                    view.GetComponent<UIToggle>().Set(false, false);
                    view.GetComponent<UIToggle>().onChange.Clear();
                    view.GetComponent<UIToggle>().activeSprite = view.FindChild("Selected").GetComponent<UISprite>();
                    SetEvent(view.gameObject, "onInvenToggle");
                }

                view.FindChild("Mask").gameObject.SetActive(false);
                view.FindChild("Lock").gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 드로잉 메서드
    /// </summary>
    internal void FillEquipments(Dictionary<string, Equipment> equipments)
    {
        FillEquipment(transform.FindChild("장비슬롯/Slot_Weapon").gameObject, Equipment.Type.Weapon, equipments);
        FillEquipment(transform.FindChild("장비슬롯/Slot_Staff").gameObject, Equipment.Type.Staff, equipments);
        FillEquipment(transform.FindChild("장비슬롯/Slot_Ring").gameObject, Equipment.Type.Ring, equipments);
    }
    private void FillEquipment(GameObject Slot, Equipment.Type part, Dictionary<string, Equipment> equipments)
    {
        Slot.transform.DestroyChildren();
        if (equipments.ContainsKey(part.ToString()))
        {
            GameObject Weapon = MakeGridView(Slot.transform, part.ToString(), equipments[part.ToString()].iconID, "onEquipToggle");
            Weapon.transform.GetComponent<UIWidget>().width = Slot.transform.GetComponent<UIWidget>().width;
        }
    }
    public void FillInventory(List<Equipment> items)
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
        capacity.text = "최대 소지 한도 수 " + items.Count + " / " + Singleton<PlayerStatus>.getInstance().MaxInventory;
        if (items.Count >= Singleton<PlayerStatus>.getInstance().MaxInventory)
            capacity.color = Color.red;
        else
            capacity.color = Color.yellow;
    }
    public void FillExpectedSelect(Equipment e, int NextExp, int NextEvolution) // 강화나 각성시 사전정보 들어오기.
    {
        if (NextExp > e.MaxExp)
            NextExp = e.MaxExp;
        int MaxExp = EquipmentWiki.CalMaxExp(e.grade, NextEvolution);

        Transform info = transform.FindChild("아이템정보");
        info.gameObject.SetActive(true);
        info.FindChild("Icon").GetComponent<UISprite>().spriteName = e.iconID;
        info.FindChild("Name").GetComponent<UILabel>().text = e.name + EquipmentWiki.EvToString(NextEvolution);
        if (NextExp > e.Exp || NextEvolution > e.evolution)
            info.FindChild("Stat").GetComponent<UILabel>().color = Color.blue;
        else
            info.FindChild("Stat").GetComponent<UILabel>().color = Color.black;
        if (NextEvolution > e.evolution) // 각성 예
        {
            info.FindChild("Name").GetComponent<UILabel>().color = Color.blue;
            NextExp = 0;
            info.FindChild("EnchantBar").GetComponent<UIProgressBar>().value = 0;
        }
        else // 강화 예
        {
            info.FindChild("Name").GetComponent<UILabel>().color = Color.black;
            info.FindChild("EnchantBar").GetComponent<UIProgressBar>().value = (float)e.Exp / (float)MaxExp;
        }
        info.FindChild("Stat").GetComponent<UILabel>().text = e.ExpectStat(NextExp, NextEvolution).ToString();
        info.FindChild("EnchantBar/RBar_Fill2").GetComponent<UISprite>().fillAmount = (float)NextExp / (float)EquipmentWiki.CalMaxExp(e.grade, NextEvolution);
        info.FindChild("EnchantBar/RBar_Text").GetComponent<UILabel>().text = EquipmentWiki.CalExpPercent(NextExp, MaxExp) + " 강화";

        info.FindChild("SkillName").GetComponent<UILabel>().text = e.skillName + " Lv." + e.skillLevel.ToString();
        info.FindChild("SkillStat").GetComponent<UILabel>().text = EquipmentWiki.SkillStatToString(e.SkillStat);
    }
    private void FillSelected(Equipment selected)
    {
        FillExpectedSelect(selected, selected.Exp, selected.evolution);
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
        SetEvent(EquipView, EventMethod);
        EquipView.transform.FindChild("Icon").GetComponent<UISprite>().spriteName = iconID;
        return EquipView;
    }
    private void SetEvent(GameObject equipView, string eventMethod)
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
