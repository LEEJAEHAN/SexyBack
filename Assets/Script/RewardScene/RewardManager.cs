using System;
using UnityEngine;

namespace SexyBackRewardScene
{
    internal class RewardManager
    {
        double WaitingTime;
        RewardWindow Window;
        ResultReward CurrentResult;

        internal void Init()
        {
            if (CurrentResult == null)
                RecordResult(Singleton<TableLoader>.getInstance().mapTable["Map01"], true, 10, 0, 200, 6);
            WaitingTime = 1.0f;
            Window = new RewardWindow();
            Window.SetWindowView(CurrentResult);
        }

        internal void GetGemChest()
        {
            // 추가상자 아이템 지급.
            Singleton<EquipmentManager>.getInstance().AddEquipment(CurrentResult.GemEquipment);
        }

        // 플레이신끝날때 호출.
        // 이때  이미 리워드를 playerstatus에 지급하기로 하자.
        // 단 rewardscene에서 프리미엄추가상자를 깔경우, 다시 result로부터 chest lottery를 한번더돌려서 지급하자.            
        // 그러므로 reward신에서 나가면, 프리미엄상자보상을 제외한 모든 보상은 받아져있다.
        // 실제로 open하는 행위는. 이미정해져있는아이템을 확인하는것.
        internal void RecordResult(MapData mapInfo, bool clear, int floor, int clearTime, int totalElementLevel, int FinishResearchCount)
        {
            bool isFirstClear = false;
            if (clear)
                isFirstClear = Singleton<PlayerStatus>.getInstance().CheckFirstClear(mapInfo.ID);

            CurrentResult = new ResultReward(mapInfo, clear, isFirstClear,
                floor, clearTime, totalElementLevel, FinishResearchCount);

            CurrentResult.MakeReward();
        }
        
        internal void ShowNext()
        {
            Window.ShowNextImmediatly();
        }
        internal void TweenComplete()
        {
            Window.EndTweenStartShow();
        }
        internal void OpenNormalChest()
        {
            Window.OpenNormalChest();
        }
        public void Update()
        {
            WaitingTime -= Time.deltaTime;
            if (WaitingTime < 0)
                Window.StartTween();

            Window.Update();
        }

    }

}