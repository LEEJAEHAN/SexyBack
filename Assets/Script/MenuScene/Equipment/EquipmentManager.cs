using System;
using System.Collections.Generic;

using SexyBackRewardScene;

internal class EquipmentManager
{
    Dictionary<string, Equipment> equipments;
    public List<Equipment> inventory;

    internal void Init()
    {
        equipments = new Dictionary<string, Equipment>();
        inventory = new List<Equipment>(20);

        inventory.Add(EquipFactory.CraftEquipment("E01"));
        inventory.Add(EquipFactory.CraftEquipment("E02"));

        //        equipments.Add("Weapon", new Equipment());
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

}