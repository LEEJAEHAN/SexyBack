using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackRewardScene
{

    public enum RewardRank
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
    public class Result
    {
        public int time;
        public Dictionary<string, int> score = new Dictionary<string, int>();
        public RewardRank rank = RewardRank.F;
        MapData mapData;

        //int clearScore = 0;     // 0 - 50   // 50이 기본
        //int floorScore = 0;     // 0 - 50   // 50이 기본
        //int timeScore = 0;      // 0 - 50   // 0이 기본
        //int levelScore = 0;     // 0 이기본 +-
        //int ResearchScore = 0;  // 0 이기본 +-
        //int bonusScore = 0;     // 0 이기본 +

        internal Result(string mapID, bool clear, int floor, int clearTime, int bonus)
        {
            mapData = Singleton<TableLoader>.getInstance().mapTable[mapID];
            time = clearTime;
            score.Clear();

            if (clear)
            {
                score.Add("clearScore", 50);
                score.Add("floorScore", 50 * floor / mapData.MaxFloor);
                score.Add("timeScore", mapData.LimitTime - clearTime);
                int lScore;
                int rScore;
                CalGoal(floor, out lScore, out rScore);
                score.Add("levelScore", lScore);
                score.Add("researchScore", rScore);
                score.Add("bonusScore", bonus);
                rank = CalRank();
            }
            else
            {
                score["clearScore"] = 0;
                score["floorScore"] = 50 * floor / mapData.MaxFloor;
                score["timeScore"] = 0;
                score["levelScore"] = 0;
                score["researchScore"] = 0;
                score["bonusScore"] = bonus;
                rank = RewardRank.F;
            }

        }




        public void CalGoal(int clearFloor, out int l, out int r)
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
            l = lCount;
            r = rCount;
        }
        private RewardRank CalRank()
        {
            return RewardRank.S;
        }


        public string TimeText { get { return "1시간27분88초"; } }
        public string TotalScoreText
        {
            get
            {
                int sum = 0;
                foreach (int score in score.Values)
                {
                    sum += score;
                }
                return "총점 " + sum.ToString();
            }
        }
        internal LinkedList<string> ScoreText
        {
            get
            {

                LinkedList<string> texts = new LinkedList<string>();

                texts.AddLast("클리어 점수 " + score["clearScore"].ToString() + "\n");
                texts.AddLast("돌파 점수 " + score["floorScore"].ToString() + "\n");
                texts.AddLast("시간 점수 " + score["timeScore"].ToString() + "\n");
                texts.AddLast("레벨업 정도에 따라 " + score["levelScore"].ToString() + "\n");
                texts.AddLast("연구 정도에 따라 " + score["researchScore"].ToString() + "\n");
                texts.AddLast("특성에 의하여 " + score["bonusScore"].ToString());

                return texts;
            }
        }
        public string RankText { get { return "랭크 " + rank.ToString(); } }
        public string RankIcon { get { return "Icon_01"; } }
    }
}