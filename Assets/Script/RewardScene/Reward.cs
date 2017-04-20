using System;

namespace SexyBackRewardScene
{
    internal class Reward
    {
        public string ItemID;
        int SkillPoint;
        string ResearchID;

        public Reward(string mapID, Result currentResult)
        {   
            MapData mapData = Singleton<TableLoader>.getInstance().mapTable[mapID];

            Equipment Equipment = Singleton<EquipmentManager>.getInstance().Craft(mapData.Level, currentResult.rank);

            //giveskillpoint(mapid, totalscore); // mapdata has defaultskillpoint
            //givegem(playerstatus.mapIDList, mapID);   // playser
            //giveresearch(playerstatus.researchList, mapid); // mapdata has researchlist, 순차적획득 

        }

        internal void GiveAll()
        {
            
            //            giveitem(mapid, rank); // mapdata has itemlist


        }
    }
}