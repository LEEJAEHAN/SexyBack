using System;
using System.Collections.Generic;

using SexyBackRewardScene;

internal class EquipmentManager
{
    public Dictionary<string, Equipment> equipments;
    public List<Equipment> inventory;
    EquipmentWindow view;
    public Equipment Selected;
    public int SelectIndex {  get { return inventory.IndexOf(Selected); } }

    internal void Init()
    {
        equipments = new Dictionary<string, Equipment>();
        inventory = new List<Equipment>(20);

        inventory.Add(EquipFactory.CraftEquipment("E01"));
        inventory.Add(EquipFactory.CraftEquipment("E02"));
        inventory.Add(EquipFactory.CraftEquipment("E03"));

        //        equipments.Add("Weapon", new Equipment());
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
//        view.mode = EquipmentWindow.State.InvenSelected;
        view.FillSelected(Selected, Selected.Exp, Selected.evolution);
        return true;
    }
    internal bool SelectEquipment(string part)
    {
        if (equipments.ContainsKey(part) == false)
            return false;

        Selected = equipments[part];
 //       view.mode = EquipmentWindow.State.EquipSelected;
        view.FillSelected(Selected, Selected.Exp, Selected.evolution);
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
        view.ForceToggle(false, part);
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
        view.ForceToggle(true, (inventory.Count-1).ToString());
        //view.mode = EquipmentWindow.State.InvenSelected;
        //Selected = target;
        //view.FillSelected(Selected, Selected.Exp, Selected.evolution);
    }

    internal void Enchant()
    {
            // enchant
            //
//            view.
    }

    internal void ApplyCheckList(List<int> checkList)
    {
        int expSum = Selected.Exp; 
        foreach( int index in checkList)
        {
            expSum += EquipmentWiki.GetMaterialExp(inventory[index].grade, inventory[index].evolution);
        }

        view.FillSelected(Selected, expSum, Selected.evolution);
    }

    internal void reSelect()
    {
        if(equipments.ContainsValue(Selected))
        {
            string part = Selected.type.ToString();
            view.ForceToggle(false, part);
        }
        else if (inventory.Contains(Selected))
        {
            view.ForceToggle(true, inventory.IndexOf(Selected).ToString());
        }
    }
}