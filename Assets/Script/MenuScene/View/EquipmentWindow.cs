using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SexyBackMenuScene;
using System;

public class EquipmentWindow : MonoBehaviour
{
    //    public State mode;
    public State Mode;
    public State prevMode;
    public List<int> CheckList = new List<int>();

    public enum State
    {
        Normal = 1,
        EquipSelected,
        InvenSelected,
        EnchantMode,
        Enchanting,
        EvolutionMode
    }

    public void OnEnable()
    {
        setPosition();
        ClearWindow();
        FillInventory(Singleton<EquipmentManager>.getInstance().inventory);
        FillEquipments(Singleton<EquipmentManager>.getInstance().equipments);
        Singleton<EquipmentManager>.getInstance().BindView(this);
        ChangeMode(State.Normal);
    }

    public void FillSelected(Equipment e, int NextExp, int NextEvolution) // 강화나 각성시 사전정보 들어오기.
    {
        if (NextExp > EquipmentWiki.GetMaxExp(e.grade, e.evolution))
            NextExp = EquipmentWiki.GetMaxExp(e.grade, e.evolution);

        Transform selected = transform.FindChild("아이템정보");
        selected.gameObject.SetActive(true);

        selected.FindChild("Icon").GetComponent<UISprite>().spriteName = e.iconID;
        selected.FindChild("Name").GetComponent<UILabel>().text = e.name + EquipmentWiki.EvToString(NextEvolution);

        if (NextExp > e.Exp || NextEvolution > e.evolution)
            selected.FindChild("Stat").GetComponent<UILabel>().color = Color.blue;

        if (NextEvolution > e.evolution) // 각성 예
        {
            selected.FindChild("Name").GetComponent<UILabel>().color = Color.blue;
            NextExp = 0;
            selected.FindChild("EnchantBar").GetComponent<UIProgressBar>().value = 0;
        }
        else
            selected.FindChild("EnchantBar").GetComponent<UIProgressBar>().value = (float)e.Exp / (float)EquipmentWiki.GetMaxExp(e.grade, NextEvolution);

        selected.FindChild("Stat").GetComponent<UILabel>().text = e.GetStat(NextExp, NextEvolution).ToString();
        selected.FindChild("EnchantBar/RBar_Fill2").GetComponent<UISprite>().fillAmount = (float)NextExp / (float)EquipmentWiki.GetMaxExp(e.grade, NextEvolution);
        selected.FindChild("EnchantBar/RBar_Text").GetComponent<UILabel>().text = NextExp.ToString() + "% 강화";

        selected.FindChild("SkillName").GetComponent<UILabel>().text = e.skillName + " Lv." + e.skillLevel.ToString();
        selected.FindChild("SkillStat").GetComponent<UILabel>().text = EquipmentWiki.SkillStatToString(e.GetSkillStat());
    }

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

    internal void ForceToggle(bool isInventory, string index)
    {
        if (isInventory)
        {
            transform.FindChild("인벤토리/Grid/" + index).gameObject.GetComponent<UIToggle>().Set(true, true);
//            transform.FindChild("인벤토리/Grid/" + index).gameObject.GetComponent<UIToggle>().value = true;
        }
        else
        {
            //transform.FindChild("장비슬롯/Slot_" + index + "/" + index).gameObject.GetComponent<UIToggle>().value = true;
            transform.FindChild("장비슬롯/Slot_" + index + "/" + index).gameObject.GetComponent<UIToggle>().Set(true,true);
        }
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

    public void FillInventory(List<Equipment> items)
    {
        GameObject inventoryView = transform.FindChild("인벤토리/Grid").gameObject;
        inventoryView.transform.DestroyChildren();

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
                continue;
            GameObject EquipView = MakeGridView(inventoryView.transform, i.ToString(), items[i].iconID, "onInvenToggle");
        }
        inventoryView.GetComponent<UIGrid>().Reposition();
    }


    public void onEquipToggle(string part, bool toggle)
    {
        if(toggle)
        {
            Singleton<EquipmentManager>.getInstance().SelectEquipment(part);
            ChangeMode(State.EquipSelected);
        }
    }

    public void onInvenToggle(string index, bool toggle)
    {
        if(toggle)
        {
            Singleton<EquipmentManager>.getInstance().SelectInventory(index);
            ChangeMode(State.InvenSelected);
        }
    }

    public void onInvenCheck(string index, bool check)
    {
        int i = int.Parse(index);
        if (check && !CheckList.Contains(i))
            CheckList.Add(i);
        else if (!check && CheckList.Contains(i))
            CheckList.Remove(i);
        else
            return;

        Singleton<EquipmentManager>.getInstance().ApplyCheckList(CheckList);
        transform.FindChild("ButtonSet/Set2/Button1").GetComponent<UIButton>().isEnabled = CheckList.Count > 0;
    }

    internal void setPosition()
    {
        transform.localPosition = Vector3.zero;
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
        transform.FindChild("인벤토리/Grid").DestroyChildren();
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
        prevMode = Mode;
        ChangeMode(State.EnchantMode);
    }
    public void onEnchantConfirm()
    {
        ChangeMode(State.Enchanting);
    }
    public void onEnchantFinish()
    {
        ChangeMode(State.EnchantMode);
    }
    public void onCancel()
    {
        ChangeMode(prevMode);
        Singleton<EquipmentManager>.getInstance().reSelect(); // invenselected로 state가 넘어감.
    }

    private void ChangeMode(State state)
    {
        EndMode(Mode);
        StartMode(state);
        Mode = state;
    }

    private void StartMode(State state)
    {
        if (state == State.Normal)
        {
            transform.FindChild("아이템정보").gameObject.SetActive(false);
            transform.FindChild("ButtonSet/Set1").gameObject.SetActive(false);
            transform.FindChild("ButtonSet/Set2").gameObject.SetActive(false);
        }
        else if (state == State.InvenSelected)
        {
            transform.FindChild("ButtonSet/Set1").gameObject.SetActive(true);
            transform.FindChild("ButtonSet/Set1/Table/Button1/Label").GetComponent<UILabel>().text = "장착";
            transform.FindChild("ButtonSet/Set1/Table/Button1").GetComponent<UIButton>().onClick.Add(new EventDelegate(onEquipButton));
        }
        else if (state == State.EquipSelected)
        {
            transform.FindChild("ButtonSet/Set1").gameObject.SetActive(true);
            transform.FindChild("ButtonSet/Set1/Table/Button1/Label").GetComponent<UILabel>().text = "해제";
            transform.FindChild("ButtonSet/Set1/Table/Button1").GetComponent<UIButton>().onClick.Add(new EventDelegate(onUnEquipButton));
        }
        else if (state == State.EnchantMode)
        {
            transform.FindChild("ButtonSet/Set2").gameObject.SetActive(true);
            transform.FindChild("ButtonSet/Set2/Button1").GetComponent<UIButton>().onClick.Add(new EventDelegate(onEnchantConfirm));
            transform.FindChild("ButtonSet/Set2/Button1").GetComponent<UIButton>().isEnabled = false;
            transform.FindChild("ButtonSet/Set2/Button2").GetComponent<UIButton>().onClick.Add(new EventDelegate(onCancel));
            transform.FindChild("장비슬롯/Mask").gameObject.SetActive(true);
            CheckList.Clear();
            Transform inventory = transform.FindChild("인벤토리/Grid");
            for (int i = 0; i < inventory.childCount; i++)
            {
                Transform view = inventory.GetChild(i);
                if (i == Singleton<EquipmentManager>.getInstance().SelectIndex)
                    view.FindChild("Mask").gameObject.SetActive(true);
                if (Singleton<EquipmentManager>.getInstance().inventory[i].Lock)
                    view.FindChild("Lock").gameObject.SetActive(true);

                // change select event and toggle event
                view.GetComponent<UIToggle>().onChange.Clear();
                view.GetComponent<UIToggle>().activeSprite = view.FindChild("Check").GetComponent<UISprite>();
                SetEvent(view.gameObject, "onInvenCheck");

                // set init state false
                view.GetComponent<UIToggle>().group = 0;
                view.GetComponent<UIToggle>().Set(false, false);
            }
        }
        else if (state == State.Enchanting)
        {
            sexybacklog.Console("인챈팅시작");
            transform.FindChild("아이템정보배경/Effect").gameObject.SetActive(true);
            transform.FindChild("아이템정보배경/Effect").GetComponent<TweenAlpha>().PlayForward();
            GameObject.Find("UICamera").GetComponent<UICamera>().eventReceiverMask = 0;


        }
    }
    private void EndMode(State state)
    {
        if (state == State.Normal)
        {
            transform.FindChild("아이템정보").gameObject.SetActive(true);
        }
        else if (state == State.InvenSelected)
        {
            transform.FindChild("ButtonSet/Set1").gameObject.SetActive(false);
            transform.FindChild("ButtonSet/Set1/Table/Button1").GetComponent<UIButton>().onClick.Clear();
        }
        else if (state == State.EquipSelected)
        {
            transform.FindChild("ButtonSet/Set1").gameObject.SetActive(false);
            transform.FindChild("ButtonSet/Set1/Table/Button1").GetComponent<UIButton>().onClick.Clear();
        }
        else if (state == State.EnchantMode)
        {
            transform.FindChild("ButtonSet/Set2").gameObject.SetActive(false);
            transform.FindChild("ButtonSet/Set2/Button1").GetComponent<UIButton>().onClick.Clear();
            transform.FindChild("ButtonSet/Set2/Button2").GetComponent<UIButton>().onClick.Clear();
            transform.FindChild("장비슬롯/Mask").gameObject.SetActive(false);
            CheckList.Clear();
            Singleton<EquipmentManager>.getInstance().ApplyCheckList(CheckList);

            Transform inventory = transform.FindChild("인벤토리/Grid");
            for (int i = 0; i < inventory.childCount; i++)
            {
                Transform view = inventory.GetChild(i);
                // set init state false
                view.GetComponent<UIToggle>().Set(false, false);
                view.GetComponent<UIToggle>().group = 2;

                view.FindChild("Mask").gameObject.SetActive(false);
                view.FindChild("Lock").gameObject.SetActive(false);

                // change select event and toggle event
                view.GetComponent<UIToggle>().onChange.Clear();
                view.GetComponent<UIToggle>().activeSprite = view.FindChild("Selected").GetComponent<UISprite>();
                SetEvent(view.gameObject, "onInvenToggle");

            }
        }
        else if (state == State.Enchanting)
        {
            sexybacklog.Console("인챈팅끝");
            transform.FindChild("아이템정보배경/Effect").gameObject.SetActive(false);
            transform.FindChild("아이템정보배경/Effect").GetComponent<TweenAlpha>().ResetToBeginning();
            GameObject.Find("UICamera").GetComponent<UICamera>().eventReceiverMask = -1;

        }
    }
    private void Update()
    {

    }


}
