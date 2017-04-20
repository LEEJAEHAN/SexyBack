using System;
using System.Collections.Generic;

using SexyBackRewardScene;

internal class EquipmentManager
{

    Dictionary<string, Equipment> equipments = new Dictionary<string, Equipment>();
    List<Equipment> inventory = new List<Equipment>(20);

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