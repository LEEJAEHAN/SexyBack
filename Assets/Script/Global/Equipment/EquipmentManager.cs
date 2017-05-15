using System;
using System.Collections.Generic;
using UnityEngine;
using SexyBackRewardScene;
using SexyBackMenuScene;
using System.Xml;

internal class EquipmentManager
{
    public int MaxInventory;
    public int MaxEquipSet;
    public List<Dictionary<Equipment.Type, Equipment>> equipSets;
    public Dictionary<Equipment.Type, Equipment> currentEquipSet;
    public List<Equipment> inventory;
    public Equipment Focused;
    public int EquipSetIndex { get { return equipSets.IndexOf(currentEquipSet); } }
    public int SelectIndex { get { return inventory.IndexOf(Focused); } }

    EquipmentWindow view;
    TopWindow topView;
    
    internal void Init()
    {
        sexybacklog.Console("EquipmentManager 로드 및 초기화");
        if (equipSets != null)
            return;

        if (SaveSystem.GlobalDataExist)
        {
            sexybacklog.Console("EquipmentManager 파일로부터 로드.");
            //for test
            //NewData();
            Load();
        }
        else
        {
            sexybacklog.Console("EquipmentManager 시작초기 데이터로 생성.");
            NewData();
        }
    }
    private void Load()
    {
        XmlDocument doc = SaveSystem.LoadXml(SaveSystem.SaveDataPath);
        XmlNode statNodes = doc.SelectSingleNode("PlayerStatus/Equipments");

        MaxInventory = int.Parse(statNodes.Attributes["MaxInventory"].Value);
        MaxEquipSet = int.Parse(statNodes.Attributes["MaxEquipSet"].Value);
        int CurrentSet = int.Parse(statNodes.Attributes["CurrentSet"].Value);

        equipSets = new List<Dictionary<Equipment.Type, Equipment>>(MaxEquipSet);
        for (int i = 0; i < equipSets.Capacity; i++)
            equipSets.Add(new Dictionary<Equipment.Type, Equipment>());
        currentEquipSet = equipSets[CurrentSet];
        {
            XmlNodeList eSetNodes = statNodes.SelectNodes("EquipmentSet");
            int index = 0;
            foreach(XmlNode eSetNode in eSetNodes)
            {
                foreach (Equipment.Type type in Enum.GetValues(typeof(Equipment.Type)))
                {
                    XmlNode partNode = eSetNode.SelectSingleNode(type.ToString());
                    if(partNode.HasChildNodes)
                    {
                        Equipment e = EquipFactory.LoadEquipment(partNode.FirstChild);
                        equipSets[index].Add(type, e);
                    }
                }
                index++;
            }
        }
        inventory = new List<Equipment>(MaxInventory);
        {
            XmlNode inventoryNode = statNodes.SelectSingleNode("Inventory");
            foreach (XmlNode eNode in inventoryNode.ChildNodes)
            {
                Equipment e = EquipFactory.LoadEquipment(eNode);
                inventory.Add(e);
            }
        }
    }

    public void ReCheckStat()
    {
        EquipSetchange(currentEquipSet, true);
    }
    internal void NewData()    // 최초 게임 실행시 셋팅되는 장비.
    {
        // for test now
        MaxInventory = 20;
        MaxEquipSet = 2;
        equipSets = new List<Dictionary<Equipment.Type, Equipment>>(MaxEquipSet);
        for(int i = 0; i < equipSets.Capacity; i++)
            equipSets.Add(new Dictionary<Equipment.Type, Equipment>());
        currentEquipSet = equipSets[0];
        inventory = new List<Equipment>(MaxInventory);

        int loop = 100;
        while(loop>0)
        {
            AddEquipment(EquipFactory.CraftEquipment("E01"));
            AddEquipment(EquipFactory.CraftEquipment("E02"));
            AddEquipment(EquipFactory.CraftEquipment("E03"));
            loop--;
        }
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
        Equipment.Type t = (Equipment.Type)Enum.Parse(typeof(Equipment.Type), part);
        if (currentEquipSet.ContainsKey(t) == false)
            return false;

        Focused = currentEquipSet[t];
        return true;
    }

    internal void Unselect()
    {
        Focused = null;
    }

    internal bool AddEquipment(Equipment e)
    {
        if (inventory.Count + 1 > inventory.Capacity)
            return false;
        inventory.Add(e);
        return true;
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
        Focused.isLock = !Focused.isLock;
        view.FillSelected(Focused);
        return Focused.isLock;
    }

    internal void SwapEquipSet(bool next)
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

        EquipSetchange(currentEquipSet, false);
        currentEquipSet = equipSets[toIndex];
        EquipSetchange(currentEquipSet, true);

        if(view != null)
            view.FillEquipments(currentEquipSet, toIndex);
        topView.DrawEquipments(currentEquipSet);
    }


    internal void EquipSelectedItem()
    {
        var part = Focused.type;
        if (currentEquipSet.ContainsKey(part))
        {
            Equipment old = currentEquipSet[part];
            EquipChange(old, false);
            currentEquipSet.Remove(part);
            inventory.Add(old);
        }

        EquipChange(Focused, true);
        currentEquipSet.Add(part, Focused);
        inventory.Remove(Focused);
        view.FillInventory(inventory);
        view.FillEquipments(currentEquipSet, EquipSetIndex);
        topView.DrawEquipments(currentEquipSet);
        view.ForceSelect(false, part.ToString());
    }

    private void EquipChange(Equipment item, bool sign)
    {
        foreach(BonusStat specialStat in item.SkillStat)
            Singleton<PlayerStatus>.getInstance().ApplySpecialStat(specialStat, sign);
        Singleton<PlayerStatus>.getInstance().ApplyBaseStat(item.Stat, sign);
    }
    void EquipSetchange(Dictionary<Equipment.Type, Equipment> itemSet, bool sign)
    {
        foreach(Equipment item in itemSet.Values)
            EquipChange(item, sign);
    }

    internal void UnEquipSelectedItem()
    {
        if (currentEquipSet.ContainsValue(Focused) == false)
            return;

        var part = Focused.type;
        Equipment target = currentEquipSet[part];
        EquipChange(target, false);
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
        Focused.exp = 0;
    }

    internal void CalExpectedView(List<int> checkList, EquipmentState mode)
    {
        if (checkList.Count <= 0)
        {
            view.FillSelected(Focused);
            return;
        }

        int expSum = Focused.exp; 
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