using System;
using UnityEngine;
using System.Collections.Generic;
namespace SexyBackRewardScene
{
    enum RewardRank
    {
        SSS = 155,
        SS = 145,
        S = 140,
        A = 135,
        B = 130,
        C = 120,
        D = 110,
        E = 100,
        F = 0
    }
    internal class RewardManager
    {
        GameObject Window;
        MapData mapData;

        int clearScore = 0;     // 0 - 50   // 50이 기본
        int floorScore = 0;     // 0 - 50   // 50이 기본
        int timeScore = 0;      // 0 - 50   // 0이 기본
        int levelScore = 0;     // 0 이기본 +-
        int ResearchScore = 0;  // 0 이기본 +-
        int bonusScore = 0;     // 0 이기본 +

        List<GameObject> ShowList = new List<GameObject>();

        internal void Init()
        {
            Singleton<TableLoader>.getInstance().Init();
        }
        internal void CalReward(string mapID, bool clear, int floor, int clearTime, int bonus)
        {
            mapData = Singleton<TableLoader>.getInstance().mapTable[mapID];

            if (clear)
            {
                clearScore = 50;
                floorScore = 50 * floor / mapData.MaxFloor;
                timeScore = mapData.LimitTime - clearTime;
                GetGoal(floor, out levelScore, out ResearchScore);
                bonusScore = bonus;
            }
            else
            {
                clearScore = 0;
                floorScore = 50 * floor / mapData.MaxFloor;
                levelScore = 0;     // 0 이기본 +-
                ResearchScore = 0;  // 0 이기본 +-
                bonusScore = 0;     // 0 이기본 +

            }
        }

        public void GetGoal(int clearFloor, out int levelGoal, out int ResearchGoal)
        {
            int recommendLevel = (clearFloor + 1) * 5;
            int lCount = recommendLevel; // recommend hero attack = recommendlevel;
            int rCount = 0;

            foreach (ResearchData data in Singleton<TableLoader>.getInstance().researchtable.Values)
            {
                if (data.baselevel + data.level < recommendLevel)
                {
                    rCount++;

                    if (data.bonus.attribute == "ActiveElement")
                    {
                        if (data.baselevel + data.level < recommendLevel)
                        {
                            int sub = recommendLevel - data.baselevel + data.level;
                            lCount += sub;
                        }
                    }
                }

            }
            levelGoal = lCount;
            ResearchGoal = rCount;
        }

        internal void InitWindow()
        {
            Window = GameObject.Find("RewardWindow");



            Window.transform.FindChild("Title").GetComponent<UILabel>().text = "" + "완료";

            Window.SetActive(false);
        }

        internal bool NextShow()
        {
            Debug.Log("NextShow!");
            return false;
        }

        internal void ActiveWindow()
        {
            if (!Window.activeInHierarchy)
            {
                Window.SetActive(true);
                Window.GetComponent<UITweener>().PlayForward();
            }
        }
    }
}