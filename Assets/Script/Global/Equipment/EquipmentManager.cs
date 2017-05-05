using System;
using System.Collections.Generic;
using UnityEngine;
using SexyBackRewardScene;

internal class EquipmentManager
{
    public Dictionary<string, Equipment> equipments;
    public List<Equipment> inventory;

    EquipmentWindow view;
    public Equipment Selected;
    public int SelectIndex {  get { return inventory.IndexOf(Selected); } }

    internal bool AddEquipment(Equipment e)
    {
        if (inventory.Count + 1 > Singleton<PlayerStatus>.getInstance().MaxInventory)
            return false;

        inventory.Add(e);
        return true;
    }

    internal void Init()
    {
        equipments = new Dictionary<string, Equipment>();
        inventory = new List<Equipment>();

        int loop = 100;
        while(loop>0)
        {
            AddEquipment(EquipFactory.CraftEquipment("E01"));
            AddEquipment(EquipFactory.CraftEquipment("E02"));
            AddEquipment(EquipFactory.CraftEquipment("E03"));
            loop--;
        }


    }

    internal void BindView(EquipmentWindow equipmentWindow)
    {
        view = equipmentWindow;
    }

    internal bool SelectInventory(string indexstring)
    {
        int i;
        if (int.TryParse(indexstring, out i) == false)
            return false;
        if (inventory[i] == null)
            return false;

        Selected = inventory[i];
        return true;
    }
    internal bool SelectEquipment(string part)
    {
        if (equipments.ContainsKey(part) == false)
            return false;

        Selected = equipments[part];
        return true;
    }

    internal void Unselect()
    {
        Selected = null;
  //      view.mode = EquipmentWindow.State.Normal;
    }



    internal Equipment Craft(int level, RewardRank rank)
    {
        // random gen "Equip01" from level and rank;
        // random gen Type;
        // random 

        return null;


//        return new Equipment("앨런블랙", "Equip01", Equipment.Type.Weapon, );
    }


    internal void GetReward(Reward currentReward)
    {
        //inventory.Add(Singleton<EquipmentManager>.getInstance().Items[currentReward.ItemID])
        //currentReward.
        //equipments.


        // 
    }

    internal void Equip()
    {
        string part = Selected.type.ToString();

        if (equipments.ContainsKey(part))
        {
            Equipment old = equipments[part];
            equipments.Remove(part);
            inventory.Add(old);
        }

        equipments.Add(part, Selected);
        inventory.Remove(Selected);
        view.FillInventory(inventory);
        view.FillEquipments(equipments);
        view.ForceSelect(false, part);
        //view.mode = EquipmentWindow.State.EquipSelected;
        //Selected = equipments[part];
        //view.FillSelected(Selected, Selected.Exp, Selected.evolution);
    }

    internal void UnEquip()
    {
        if (equipments.ContainsValue(Selected) == false)
            return;

        string part = Selected.type.ToString();
        Equipment target = equipments[part];
        equipments.Remove(part);
        inventory.Add(target);

        view.FillInventory(inventory);
        view.FillEquipments(equipments);
        view.ForceSelect(true, (inventory.Count-1).ToString());
        //view.mode = EquipmentWindow.State.InvenSelected;
        //Selected = target;
        //view.FillSelected(Selected, Selected.Exp, Selected.evolution);
    }

    internal EquipmentWindow.State GetPrevMode()
    {
        if (equipments.ContainsValue(Selected))
            return EquipmentWindow.State.EquipSelected;
        else if (inventory.Contains(Selected))
            return EquipmentWindow.State.InvenSelected;
        else
            return EquipmentWindow.State.None;
    }

    internal void Enchant(List<int> checkList)
    {
        checkList.Sort((x, y) => y.CompareTo(x));

        int sum = 0;
        foreach(int i in checkList)
        {
            sum += inventory[i].MaterialExp;
            if (inventory[i].skillName.Equals(Selected.skillName))
            {
                if (Selected.skillLevel < 10)
                    if(UnityEngine.Random.Range(1,10) > 8)
                        Selected.skillLevel++;                    
            }
            inventory.RemoveAt(i);
        }

        Selected.Exp += sum;
        if (Selected.Exp > Selected.MaxExp)
            Selected.Exp = Selected.MaxExp;
    }

    internal void CheckInventory(List<int> checkList)
    {
        int expSum = Selected.Exp; 
        foreach( int index in checkList)
        {
            expSum += inventory[index].MaterialExp;
        }

        view.FillExpectedSelect(Selected, expSum, Selected.evolution);
    }

    internal void reSelect()
    {
    }



}