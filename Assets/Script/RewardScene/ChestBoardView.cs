using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackRewardScene
{
    internal class ChestBoardView
    {
        public GameObject view;

        public ChestBoardView(GameObject v)
        {
            view = v;
            view.transform.DestroyChildren();
        }

        internal void SetResultView(ResultReward result)
        {

            foreach (Equipment equip in result.NormalEquipments)
                MakeChestIcon(ChestSource.Normal, result.Rank, equip);
            foreach (KeyValuePair<ChestSource, Equipment> pair in result.BonusEquipments)
                MakeChestIcon(pair.Key, result.Rank, pair.Value);
            if(result.GemEquipment != null)
                MakeChestIcon(ChestSource.GemOpen, result.Rank, result.GemEquipment);

            view.transform.GetComponent<UITable>().Reposition();
        }

        private void MakeChestIcon(ChestSource source, RewardRank rank, Equipment result)
        {
            GameObject chestview = SexyBackMenuScene.ViewLoader.InstantiatePrefab(view.transform, "상자", "Prefabs/UI/EquipReward");
            chestview.GetComponent<ChestOpenScript>().SetRank(rank);
            chestview.GetComponent<ChestOpenScript>().SetMode(source);
            chestview.GetComponent<ChestOpenScript>().SetEquip(result);
        }
    }
}