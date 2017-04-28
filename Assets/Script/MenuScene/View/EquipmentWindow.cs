using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SexyBackMenuScene;
using System;

public class EquipmentWindow : MonoBehaviour
{
    public State mode;
    public enum State
    {
        Normal,
        EquipSelected,
        InvenSelected,
        EnchantMode,
        EvolutionMode
    }

    public void OnEnable()
    {
        setPosition();
        ClearWindow();
        FillInventory(Singleton<EquipmentManager>.getInstance().inventory);
        FillEquipments(Singleton<EquipmentManager>.getInstance().equipments);
        Singleton<EquipmentManager>.getInstance().BindView(this);
        mode = State.Normal;
    }

    public void FillSelected(Equipment e, int NextExp, int NextEvolution) // 강화나 각성시 사전정보 들어오기.
    {
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
        FillEquipment(transform.FindChild("장비슬롯/Slot1").gameObject, Equipment.Type.Weapon, equipments);
        FillEquipment(transform.FindChild("장비슬롯/Slot2").gameObject, Equipment.Type.Staff, equipments);
        FillEquipment(transform.FindChild("장비슬롯/Slot3").gameObject, Equipment.Type.Ring, equipments);
    }

    internal void ForceToggle(bool isInventory, string index)
    {
        if(isInventory)
            transform.FindChild("인벤토리/Grid/"+index).gameObject.GetComponent<UIToggle>().value = true;
        else
            transform.FindChild("장비슬롯/Slot1/"+index).gameObject.GetComponent<UIToggle>().value = true;
    }

    private void FillEquipment(GameObject Slot, Equipment.Type part, Dictionary<string, Equipment> equipments)
    {
        Slot.transform.DestroyChildren();
        if (equipments.ContainsKey(part.ToString()))
        {
            GameObject Weapon = MakeGridView(Slot.transform, part.ToString(), "onEquipTouch");
            Weapon.transform.GetComponent<UIWidget>().width = Slot.transform.GetComponent<UIWidget>().width;
        }
    }

    private GameObject MakeGridView(Transform Parent, string Name, string EventMethod )
    {
        GameObject EquipView = ViewLoader.InstantiatePrefab(Parent, Name, "Prefabs/UI/Equipment");
        EventDelegate Ehandler = new EventDelegate(this, EventMethod);
        EventDelegate.Parameter Eparam1 = new EventDelegate.Parameter();
        Eparam1.obj = EquipView;
        Eparam1.field = "name";
        EventDelegate.Parameter Eparam2 = new EventDelegate.Parameter();
        Eparam2.obj = EquipView.GetComponent<UIToggle>();
        Eparam2.field = "value";
        Ehandler.parameters[0] = Eparam1;
        Ehandler.parameters[1] = Eparam2;
        EquipView.GetComponent<UIToggle>().onChange.Add(Ehandler);
        return EquipView;
    }

    public void FillInventory(List<Equipment> items)
    {
        GameObject inventoryView = transform.FindChild("인벤토리/Grid").gameObject;
        inventoryView.transform.DestroyChildren();

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
                continue;
            GameObject EquipView = MakeGridView(inventoryView.transform, i.ToString(), "onInvenTouch");
       }
        inventoryView.GetComponent<UIGrid>().Reposition();
    }
    public void onEquipTouch(string part, bool toggle)
    {
        if (toggle)
        {
            Singleton<EquipmentManager>.getInstance().SelectEquipment(part);
            mode = State.EquipSelected;
        }
        else
        {
            Singleton<EquipmentManager>.getInstance().Unselect();
            mode = State.Normal;
        }
    }

    public void onInvenTouch(string index, bool toggle)
    {
        if (toggle)
        {
            Singleton<EquipmentManager>.getInstance().SelectInventory(index);
            mode = EquipmentWindow.State.InvenSelected;
        }
        else
        {
            Singleton<EquipmentManager>.getInstance().Unselect();
            mode = State.Normal;
        }

        //else
        //    Singleton<EquipmentManager>.getInstance().Unselect();
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

        transform.FindChild("장비슬롯/Slot1").DestroyChildren();
        transform.FindChild("장비슬롯/Slot2").DestroyChildren();
        transform.FindChild("장비슬롯/Slot3").DestroyChildren();
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

    private void Update()
    {
        if(mode == State.Normal)
        {
            transform.FindChild("아이템정보").gameObject.SetActive(false);
            transform.FindChild("ButtonSet/Set1").gameObject.SetActive(false);
            transform.FindChild("ButtonSet/Set2").gameObject.SetActive(false);
        }
        else if (mode == State.InvenSelected)
        {
            transform.FindChild("아이템정보").gameObject.SetActive(true);
            transform.FindChild("ButtonSet/Set1").gameObject.SetActive(true);
            transform.FindChild("ButtonSet/Set1/Table/Button1/Label").GetComponent<UILabel>().text = "장착";
            transform.FindChild("ButtonSet/Set1/Table/Button1").GetComponent<UIButton>().onClick.Clear();
            transform.FindChild("ButtonSet/Set1/Table/Button1").GetComponent<UIButton>().onClick.Add(new EventDelegate(onEquipButton));
        }
        else if (mode == State.EquipSelected)
        {
            transform.FindChild("아이템정보").gameObject.SetActive(true);
            transform.FindChild("ButtonSet/Set1").gameObject.SetActive(true);
            transform.FindChild("ButtonSet/Set1/Table/Button1/Label").GetComponent<UILabel>().text = "해제";
            transform.FindChild("ButtonSet/Set1/Table/Button1").GetComponent<UIButton>().onClick.Clear();
            transform.FindChild("ButtonSet/Set1/Table/Button1").GetComponent<UIButton>().onClick.Add(new EventDelegate(onUnEquipButton));
        }
    }


}
