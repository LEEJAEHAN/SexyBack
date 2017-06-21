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
    public List<Dictionary<Equipment.Slot, Equipment>> equipSets;
    public Dictionary<Equipment.Slot, Equipment> currentEquipSet;
    public List<Equipment> inventory;
    public Equipment Focused;
    public int EquipSetIndex { get { return equipSets.IndexOf(currentEquipSet); } }
    public int SelectIndex { get { return inventory.IndexOf(Focused); } }

    EquipmentWindow view;
    TopWindow topView;

    internal void InitOrLoad()
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

    internal string GetTotalBonus()
    {
        string temp = "";
        foreach(var a in currentEquipSet.Values)
        {
            foreach(var b in a.SkillStat)
            {
                temp +=  StringParser.GetAttributeString(b) + "\n";
            }
        }
        return temp;
    }

    public void Load()
    {
        XmlDocument doc = SaveSystem.LoadXml(SaveSystem.SaveDataPath);
        XmlNode statNodes = doc.SelectSingleNode("PlayerStatus/Equipments");

        MaxInventory = int.Parse(statNodes.Attributes["MaxInventory"].Value);
        MaxEquipSet = int.Parse(statNodes.Attributes["MaxEquipSet"].Value);
        int CurrentSet = int.Parse(statNodes.Attributes["CurrentSet"].Value);

        equipSets = new List<Dictionary<Equipment.Slot, Equipment>>(MaxEquipSet);
        for (int i = 0; i < equipSets.Capacity; i++)
            equipSets.Add(new Dictionary<Equipment.Slot, Equipment>());
        currentEquipSet = equipSets[CurrentSet];
        {
            XmlNodeList eSetNodes = statNodes.SelectNodes("EquipmentSet");
            int index = 0;
            foreach(XmlNode eSetNode in eSetNodes)
            {
                foreach (Equipment.Slot type in Enum.GetValues(typeof(Equipment.Slot)))
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
        equipSets = new List<Dictionary<Equipment.Slot, Equipment>>(MaxEquipSet);
        for(int i = 0; i < equipSets.Capacity; i++)
            equipSets.Add(new Dictionary<Equipment.Slot, Equipment>());
        currentEquipSet = equipSets[0];
        inventory = new List<Equipment>();

        int loop = 0;
        while (loop > 0)
        {
            AddEquipment(EquipFactory.LotteryEquipment(new MapRewardData(), RewardRank.A, 50));
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
        Equipment.Slot t = (Equipment.Slot)Enum.Parse(typeof(Equipment.Slot), part);
        if (currentEquipSet.ContainsKey(t) == false)
            return false;

        Focused = currentEquipSet[t];
        return true;
    }

    internal void Unselect()
    {
        Focused = null;
    }


    internal void AddEquipment(Equipment e)
    {
        inventory.Add(e);
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
        Equipment.Slot slot;
        if (Focused.type == Equipment.Type.Ring)
        {
            if (currentEquipSet.ContainsKey(Equipment.Slot.Ring))   // 1에 껴져있으면 2에 있건말건 2를바꾼다.
                slot = Equipment.Slot.Ring2;
            else // 1에 안껴져있단소리니까 1에 낀다.
                slot = Equipment.Slot.Ring;
        }
        else
        {
            string slotstr = Focused.type.ToString();
            slot = (Equipment.Slot)Enum.Parse(typeof(Equipment.Slot), slotstr);
        }

        if (currentEquipSet.ContainsKey(slot))
        {
            Equipment old = currentEquipSet[slot];
            EquipChange(old, false);
            currentEquipSet.Remove(slot);
            inventory.Add(old);
        }

        EquipChange(Focused, true);
        currentEquipSet.Add(slot, Focused);
        inventory.Remove(Focused);
        view.FillInventory(inventory, MaxInventory);
        view.FillEquipments(currentEquipSet, EquipSetIndex);
        topView.DrawEquipments(currentEquipSet);
        view.ForceSelect(false, slot.ToString());
    }

    private void EquipChange(Equipment item, bool sign)
    {
        foreach(BonusStat specialStat in item.SkillStat)
            Singleton<PlayerStatus>.getInstance().ApplySpecialStat(specialStat, sign);
        foreach (BonusStat baseStat in item.DmgStat)
            Singleton<PlayerStatus>.getInstance().ApplySpecialStat(baseStat, sign);
    }
    void EquipSetchange(Dictionary<Equipment.Slot, Equipment> itemSet, bool sign)
    {
        foreach(Equipment item in itemSet.Values)
            EquipChange(item, sign);
    }

    internal void UnEquipSelectedItem()
    {
        if (currentEquipSet.ContainsValue(Focused) == false)
            return;

        //var part = Focused.type;
        var part = TryGetKey(currentEquipSet, Focused);
        Equipment target = currentEquipSet[part];
        EquipChange(target, false);
        currentEquipSet.Remove(part);
        inventory.Add(target);
        view.FillInventory(inventory, MaxInventory);
        view.FillEquipments(currentEquipSet, EquipSetIndex);
        topView.DrawEquipments(currentEquipSet);
        view.ForceSelect(true, (inventory.Count-1).ToString());
    }

    public static Equipment.Slot TryGetKey(Dictionary<Equipment.Slot, Equipment> dictionary, Equipment Value)
    {
        List<Equipment.Slot> KeyList = new List<Equipment.Slot>(dictionary.Keys);
        foreach (var key in KeyList)
            if (dictionary[key].Equals(Value))
                return key;
        throw new KeyNotFoundException();
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

        double sum = 0;
        foreach(int i in checkList)
        {
            sum += inventory[i].GetMaterialExp(Focused);
            if (inventory[i].skillName.Equals(Focused.skillName))
                if (Focused.skillLevel < 10)
                    //if (UnityEngine.Random.Range(0, 10) >= 8) 
                        Focused.skillLevel++;
            inventory.RemoveAt(i);
        }
        Focused.AddExp(sum);
    }
    internal void UnLimit(int meterialIndex)
    {
        if (!isUnLimitMaterial(meterialIndex))
            return;

        inventory.RemoveAt(meterialIndex);
        Focused.UnLimit();
        Focused.exp = 0;
    }
    internal void GradeUp(int meterialIndex)
    {
        if (!isGradeUpMaterial(meterialIndex))
            return;

        inventory.RemoveAt(meterialIndex);
        Focused.GradeUp();
        Focused.exp = 0;
    }



    internal void CalExpectedView(List<int> checkList, EquipmentState mode)
    {
        if (checkList.Count <= 0)
        {
            view.FillSelected(Focused);
            return;
        }

        double expSum = Focused.exp; 
        foreach( int index in checkList)
            expSum += inventory[index].GetMaterialExp(Focused);

        if (mode == EquipmentState.EnchantMode)
            view.FillExpectedSelect(Focused, expSum, mode);
        else if (mode == EquipmentState.GradeUpMode)
            view.FillExpectedSelect(Focused, 0, mode);
    }

    internal void reSelect()
    {
    }

    internal bool isUnLimitMaterial(int meterialIndex)
    {
        if (Focused.isSameItem(inventory[meterialIndex]) == false)
            return false;
        //if (inventory[meterialIndex].isMaxExp() == false )
        //    return false;

        return true;
    }

    internal bool isGradeUpMaterial(int meterialIndex)
    {
        if (Focused.isSameItem(inventory[meterialIndex]) == false)
            return false;
        if (inventory[meterialIndex].level < Focused.level)
            return false;
        //if (inventory[meterialIndex].isMaxExp() == false)
        //    return false;

        return true;
    }
}