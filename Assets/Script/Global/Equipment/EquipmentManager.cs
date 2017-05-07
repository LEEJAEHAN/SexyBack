using System;
using System.Collections.Generic;
using UnityEngine;
using SexyBackRewardScene;
using SexyBackMenuScene;

internal class EquipmentManager
{
    public List<Dictionary<string, Equipment>> equipSets;
    public List<Equipment> inventory;

    EquipmentWindow view;
    TopWindow topView;

    public Dictionary<string, Equipment> currentEquipSet;
    public int EquipSetIndex { get { return equipSets.IndexOf(currentEquipSet); } }
    public Equipment Focused;
    public int SelectIndex {  get { return inventory.IndexOf(Focused); } }

    internal bool AddEquipment(Equipment e)
    {
        if (inventory.Count + 1 > Singleton<PlayerStatus>.getInstance().MaxInventory)
            return false;

        inventory.Add(e);
        return true;
    }

    internal void Init()
    {
        equipSets = new List<Dictionary<string, Equipment>>(3);
        for(int i = 0; i < equipSets.Capacity; i++)
            equipSets.Add(new Dictionary<string, Equipment>());
        currentEquipSet = equipSets[0];

        inventory = new List<Equipment>();

        int loop = 100;
        while(loop>0)
        {
            AddEquipment(EquipFactory.CraftEquipment("E01"));
            AddEquipment(EquipFactory.CraftEquipment("E02"));
            AddEquipment(EquipFactory.CraftEquipment("E03"));
            loop--;
        }

        topView.DrawEquipments(Singleton<EquipmentManager>.getInstance().currentEquipSet);
    }

    internal void BindTopView(TopWindow view)
    {
        topView = view;
    }
    internal void BindView(EquipmentWindow view)
    {
        this.view = view;
    }

    internal bool SelectInventory(string indexstring)
    {
        int i;
        if (int.TryParse(indexstring, out i) == false)
            return false;
        if (inventory[i] == null)
            return false;

        Focused = inventory[i];
        return true;
    }
    internal bool SelectEquipment(string part)
    {
        if (currentEquipSet.ContainsKey(part) == false)
            return false;

        Focused = currentEquipSet[part];
        return true;
    }

    internal void Unselect()
    {
        Focused = null;
    }

    internal Equipment Craft(int level, RewardRank rank)
    {
        // random gen "Equip01" from level and rank;
        // random gen Type;
        // random 
        //        return new Equipment("앨런블랙", "Equip01", Equipment.Type.Weapon, );

        return null;
    }

    internal void GetReward(Reward currentReward)
    {
        //inventory.Add(Singleton<EquipmentManager>.getInstance().Items[currentReward.ItemID])
        //currentReward.
        //equipments.


        // 
    }
    internal bool Lock()
    {
        Focused.Lock = !Focused.Lock;
        view.FillSelected(Focused);
        return Focused.Lock;
    }

    internal void ChangeEquipSet(bool next)
    {
        if (equipSets.Count <= 1)
            return;

        int toIndex;
        if(next)
        {
            toIndex = EquipSetIndex + 1;
            if (toIndex >= equipSets.Count)
                toIndex = 0;
        }
        else
        {
            toIndex = EquipSetIndex - 1;
            if (toIndex < 0)
                toIndex = equipSets.Count - 1;
        }

        currentEquipSet = equipSets[toIndex];
        view.FillEquipments(currentEquipSet, toIndex);
        topView.DrawEquipments(currentEquipSet);
    }

    internal void Equip()
    {
        string part = Focused.type.ToString();

        if (currentEquipSet.ContainsKey(part))
        {
            Equipment old = currentEquipSet[part];
            currentEquipSet.Remove(part);
            inventory.Add(old);
        }

        currentEquipSet.Add(part, Focused);
        inventory.Remove(Focused);
        view.FillInventory(inventory);
        view.FillEquipments(currentEquipSet, EquipSetIndex);
        topView.DrawEquipments(currentEquipSet);
        view.ForceSelect(false, part);
    }

    internal void UnEquip()
    {
        if (currentEquipSet.ContainsValue(Focused) == false)
            return;

        string part = Focused.type.ToString();
        Equipment target = currentEquipSet[part];
        currentEquipSet.Remove(part);
        inventory.Add(target);

        view.FillInventory(inventory);
        view.FillEquipments(currentEquipSet, EquipSetIndex);
        topView.DrawEquipments(currentEquipSet);
        view.ForceSelect(true, (inventory.Count-1).ToString());
    }


    internal EquipmentState GetPrevMode()
    {
        if (currentEquipSet.ContainsValue(Focused))
            return EquipmentState.EquipSelected;
        else if (inventory.Contains(Focused))
            return EquipmentState.InvenSelected;
        else
            return EquipmentState.None;
    }

    internal void Enchant(List<int> checkList)
    {
        checkList.Sort((x, y) => y.CompareTo(x));

        int sum = 0;
        foreach(int i in checkList)
        {
            sum += inventory[i].MaterialExp;
            if (inventory[i].skillName.Equals(Focused.skillName))
                if (Focused.skillLevel < 10)
                    if (UnityEngine.Random.Range(0, 10) >= 8)
                        Focused.skillLevel++;
            inventory.RemoveAt(i);
        }
        Focused.AddExp(sum);
    }
    internal void Evolution(int meterialIndex)
    {
        if (!isEvolutionMaterial(meterialIndex))
            return;

        inventory.RemoveAt(meterialIndex);
        Focused.Evolution();
        Focused.Exp = 0;
    }

    internal void CalExpectedView(List<int> checkList, EquipmentState mode)
    {
        if (checkList.Count <= 0)
        {
            view.FillSelected(Focused);
            return;
        }

        int expSum = Focused.Exp; 
        foreach( int index in checkList)
            expSum += inventory[index].MaterialExp;

        if (mode == EquipmentState.EnchantMode)
            view.FillExpectedSelect(Focused, expSum, Focused.evolution);
        else if (mode == EquipmentState.EvolutionMode)
            view.FillExpectedSelect(Focused, 0, Focused.evolution + 1);
    }

    internal void reSelect()
    {
    }

    internal bool isEvolutionMaterial(int meterialIndex)
    {
        if (Focused.Compare(inventory[meterialIndex]) == false)
            return false;
        if (inventory[meterialIndex].isMaxExp == false)
            return false;

        return true;
    }

}