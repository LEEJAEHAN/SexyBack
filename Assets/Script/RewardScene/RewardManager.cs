using System;
using UnityEngine;

namespace SexyBackRewardScene
{
    internal class RewardManager : IDisposable
    {
        double TweenDelay = 1.0f;
        RewardWindow Window;
        Result CurrentResult;
        Reward CurrentReward;

        internal void Init()
        {
            Singleton<TableLoader>.getInstance().Init();

            // for test
            CurrentResult = new Result("Map01", true, 10, 1200, 0);
            CurrentReward = new Reward("Map01", new Result("Map01", true, 10, 1200, 0));
        }

        internal void GiveReward(string mapID, bool clear, int floor, int clearTime, int bonus)
        {
            CurrentResult = new Result(mapID, clear, floor, clearTime, bonus);
            CurrentReward = new Reward(mapID, CurrentResult);

            //Singleton<PlayerStatus>.getInstance().GetReward(CurrentReward);

            //giveitem(mapid, rank); // mapdata has itemlist
            //giveskillpoint(mapid, totalscore); // mapdata has defaultskillpoint
            //givegem(playerstatus.mapIDList, mapID);   // playser
            //giveresearch(playerstatus.researchList, mapid); // mapdata has researchlist, 순차적획득 
        }

        internal bool ShowAll()
        {
            return Window.ShowAll();
        }

        internal void InitWindow()
        {
            Window = new RewardWindow();
            Window.InitWindow();
            Window.SetResult(CurrentResult);
            Window.SetReward(CurrentReward);
            Window.SetShowSequence();

        }

        internal void TweenComplete()
        {
            Window.endTween = true;
        }

        public void Update()
        {
            TweenDelay -= Time.deltaTime;
            if (TweenDelay < 0)
            {
                Window.ActiveWindow();
                Window.Update();
            }
        }

        public void Dispose()
        {
            Window.Dispose();
            Window = null;
        }
    }

}