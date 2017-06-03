﻿using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackRewardScene
{
    public enum ChestSource
    {
        Event = 0,
        Bonus = 1,
        Premium = 2,
        GemOpen = 3,
        Normal
    }

    public enum RewardRank
    {
        SSS,
        SS,
        S,
        A,
        B,
        C,
        D,
        E,
        F,
    }

    public class ResultReward
    {
        // result
        public Dictionary<string, int> Score;
        public RewardRank Rank;
        public Map MapInfo;
        public int LastFloor;
        public int ClearTime;
        public bool isClear;
        public bool wasFirstClear = false;
        public bool wasNewRecord = false;

        // reward
        // 명성
        internal int Reputation;
        // 기본보상상자.
        internal List<Equipment> NormalEquipments;
        // event. treasurehunter. 이미 결제된 premium상자 등.
        internal Dictionary<ChestSource, Equipment> BonusEquipments;
        // 젬으로 추가구매 해야하는 상자.
        internal Equipment GemEquipment;

        internal ResultReward(Map mapInfo, bool clear, int floor, int clearTime, int totalElementLevel, int FinishResearchCount)
        {
            Score = new Dictionary<string, int>();
            MapInfo = mapInfo;
            isClear = clear;
            LastFloor = floor;
            ClearTime = clearTime;
            Score.Clear();

            CalScore(totalElementLevel, FinishResearchCount);
        }

        internal void Record()
        {
            if (isClear)
            {
                MapInfo.ClearCount++;
                if (MapInfo.ClearCount == 1)
                    wasFirstClear = true;
                if (MapInfo.BestTime > ClearTime || MapInfo.BestTime == 0)
                {
                    MapInfo.BestTime = ClearTime;
                    wasNewRecord = true;
                }
            }
        }

        internal void MakeReward()
        {
            MakeAndGiveReputation(MapInfo.baseData.RewardData);
            MakeAndGiveEquipments(MapInfo.ID, MapInfo.baseData.RewardData);
            // TODO: record clear data;
        }

        private void MakeAndGiveReputation(MapRewardData rewardInfo)
        {
            // make
            int difficulty = rewardInfo.ReputationLevel;       // 스텐다드 difficult 계수
            double coef = Mathf.Pow(1.2f, difficulty); // 추가계수 ( 3노멀보다 1하드가 1.2배 효율적)
            Reputation = (int)((double)TotalScore * (double)difficulty * coef);
            // give
            Singleton<TalentManager>.getInstance().Reputation += Reputation;
        }

        private void MakeAndGiveEquipments(string mapid, MapRewardData rewardInfo)
        {
            EquipmentManager eManager = Singleton<EquipmentManager>.getInstance();
            NormalEquipments = new List<Equipment>();
            BonusEquipments = new Dictionary<ChestSource, Equipment>();

            if (Rank == RewardRank.F)   // no장비보상
                return;

            for (int i = 0; i < rewardInfo.ItemCount; i++)
            {
                Equipment newOne = EquipFactory.LotteryEquipment(mapid, rewardInfo, Rank);
                NormalEquipments.Add(newOne);
                eManager.AddEquipment(newOne);
            }

            int bonus = Singleton<PlayerStatus>.getInstance().GetGlobalStat.BonusEquipment;
            for ( int i = 0; i < bonus; i++)
            {
                BonusEquipments.Add(ChestSource.Bonus, EquipFactory.LotteryEquipment(mapid, rewardInfo, Rank));
            }
            //if (iseventnow == true)
            //    Equipments.Add(ChestSource.Event, EquipFactory.LotteryEquipment(rewardInfo, Rank));

            if (Singleton<PremiumManager>.getInstance().PremiumUser) //
            {
                Equipment newOne = EquipFactory.LotteryEquipment(mapid, rewardInfo, Rank);
                BonusEquipments.Add(ChestSource.Premium, newOne);
                eManager.AddEquipment(newOne);
            }
            else
            {
                GemEquipment = EquipFactory.LotteryEquipment(mapid, rewardInfo, Rank);
                // 아직 지급하지는 않는다.
            }
        }

        public void CalScore(int totalElementLevel, int FinishResearchCount)
        {
            int bonus = Singleton<PlayerStatus>.getInstance().GetGlobalStat.RankBonus;

            if (isClear)
            {
                Score.Add("clearScore", 50);                                // 만점 50
                Score.Add("floorScore", 50);     // 만점 50
                // 클리어시 위의 100점은 안고감.
                Score.Add("timeScore", CalTimeScore(MapInfo.baseData.LimitTime, ClearTime));      // 0~100점
                int levelScore;
                int researchScore;
                CalLRScore(LastFloor, out levelScore, out researchScore, totalElementLevel, FinishResearchCount);
                Score.Add("levelScore", levelScore);        // 레벨업을 덜한만큼.   최소0점~거의없음.
                Score.Add("researchScore", researchScore);  // 리서치를 덜한만큼. 최소0점~거의없음.
                Score.Add("bonusScore", bonus);             // 0~30점
                Rank = CalRank(TotalScore);
            }
            else
            {
                Score.Add("clearScore", 0);
                Score.Add("floorScore", 0);
                Score.Add("timeScore", 0);
                Score.Add("levelScore", 0);
                Score.Add("researchScore", 0);
                Score.Add("bonusScore", 0);
                Rank = RewardRank.F;
            }
        }
        private int CalTimeScore(int limitTime, int clearTime)
        {
            return Mathf.Max(0, 100 - 100 * clearTime / limitTime);
        }

        public void CalLRScore(int clearFloor, out int l, out int r, int totalLevelCount, int totalResearchCount)
        {
            int MapLevel = clearFloor * MapInfo.baseData.LevelPerFloor;
            int RecommendLevel = MapLevel;  // 초기값은 hero의 검술레벨. ( 뒤에 계산으로 element level합을 계속더한다)
            int RecommendResearch = 0;

            foreach (SexyBackPlayScene.ResearchData data in Singleton<TableLoader>.getInstance().researchtable.Values)
            {
                int ContentsLevel = data.baselevel + data.Level;
                if (ContentsLevel <= MapLevel)   // 이 리서치는 배웠어야 정상이다.
                {
                    RecommendResearch++;

                    if (data.bonus.attribute == Attribute.Active)
                    {
                        if (ContentsLevel <= MapLevel) // 이 element는 MapLevel-contentsLevel 레벨까지 배웠어야 정상이다.
                        {
                            RecommendLevel += (MapLevel - ContentsLevel);
                        }
                    }
                }

            }
            l = Mathf.Max(0, (RecommendLevel - totalLevelCount) / 20);     // 평균 20레벨마다 1리서치니까 1레벨업당 1/20점
            r = Mathf.Max(0, RecommendResearch - totalResearchCount);       // 1리서치당 1점
        }
        private RewardRank CalRank(int score)
        {
            if (score >= 190)
                return RewardRank.SSS;
            if (score >= 175)
                return RewardRank.SS;
            if (score >= 150)
                return RewardRank.S;
            if (score >= 140)
                return RewardRank.A;
            if (score >= 130)
                return RewardRank.B;
            if (score >= 120)
                return RewardRank.C;
            if (score >= 110)
                return RewardRank.D;
            if (score >= 100)
                return RewardRank.E;
            else
                return RewardRank.F;
        }

        public int TotalScore
        {
            get
            {
                int sum = 0;
                foreach (int score in Score.Values)
                    sum += score;
                return sum;
            }
        }
    }
}