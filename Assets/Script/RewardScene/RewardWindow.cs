using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackRewardScene
{
    enum RewardWindowState
    {
        PrintScore,
        PrintObject
    }
    internal class RewardWindow 
    {
        GameObject view;
        ScoreBoardView ScoreBoard;
        LinkedList<GameObject> RewardViews;
        LinkedListNode<GameObject> Current;
        ChestBoardView ChestBoard;
        GameObject OkayButton;

        int remainChestCount;
        // state
        public enum State
        {
            None,
            Tween,
            Show,
            Open,
            EndOpen,
        }
        public State currState;

        // timer
        double Timer;
        const double Tick = 1f;


        public RewardWindow()
        {
            view = GameObject.Find("RewardWindow");
            ScoreBoard = new ScoreBoardView(view.transform.FindChild("ScoreBoard").gameObject);
            ChestBoard = new ChestBoardView(view.transform.FindChild("ChestBoard/Chests/Table").gameObject);
            OkayButton = view.transform.FindChild("OKButton").gameObject;
            view.SetActive(false);
            currState = State.None;
            Timer = 0;
        }

        internal void SetWindowView(ResultReward currentResult)
        {
            SetTitle(currentResult);
            remainChestCount = currentResult.NormalEquipments.Count + currentResult.BonusEquipments.Count;
            ScoreBoard.SetResultView(currentResult);
            ChestBoard.SetResultView(currentResult);

            RewardViews = new LinkedList<GameObject>();

            // set FirstClear View
            GameObject firstClear = view.transform.FindChild("ScoreBoard/FirstClearReward").gameObject;
            firstClear.SetActive(false);
            if (currentResult.wasFirstClear)
                RewardViews.AddLast(firstClear);

            //Set Research View
            RewardViews.AddLast(view.transform.FindChild("Box2/Research").gameObject);
            //Set Reputation View
            GameObject rView = view.transform.FindChild("Box3/Reputation").gameObject;
            rView.GetComponent<UILabel>().text = currentResult.Reputation.ToString();
            RewardViews.AddLast(rView);
            //Set Chests View
            RewardViews.AddLast(ChestBoard.view);

            // set index
            Current = RewardViews.First;
            // off all
            foreach (GameObject obj in RewardViews)
                obj.SetActive(false);
        }

        internal void OpenNormalChest()
        {
            remainChestCount--;
        }

        private void SetTitle(ResultReward currentResult)
        {
            string title;
            if (currentResult.isClear)
                title = currentResult.LastFloor.ToString() + "층돌파 완료!";
            else
                title = "인스턴스를 포기하셨습니다.";

            view.transform.FindChild("Title").GetComponent<UILabel>().text = title;
        }

        internal void ShowNextImmediatly()
        {
            if (currState != State.Show)
                return;

            if (ScoreBoard.isFinish == false)
                ScoreBoard.FinishShow();
            else
            {
                if(Current != null)
                {
                    Current.Value.SetActive(true);
                    Current = Current.Next;
                }
            }
        }


        // set State fuction
        public void StartTween()
        {
            view.SetActive(true);
            if (currState != State.None)
                return;
            currState = State.Tween;
            GameObject.Find("UICamera").GetComponent<UICamera>().eventReceiverMask = 0; // nothing
            OkayButton.GetComponent<UIButton>().isEnabled = false;
            OkayButton.transform.FindChild("Label").GetComponent<UILabel>().text = "상자를 터치해 확인해주세요";
            view.GetComponent<UITweener>().PlayForward();
        }
        internal void EndTweenStartShow()
        {
            currState = State.Show;
            GameObject.Find("UICamera").GetComponent<UICamera>().eventReceiverMask = -1; // everything
            view.transform.FindChild("ColliderMask").gameObject.SetActive(true);
        }
        internal void EndShowStartOpen()
        {
            currState = State.Open;
            view.transform.FindChild("ColliderMask").gameObject.SetActive(false);
        }
        internal void EndOpen()
        {
            if (currState != State.Open)
                return;
            currState = State.EndOpen;
            OkayButton.GetComponent<UIButton>().isEnabled = true;
            OkayButton.transform.FindChild("Label").GetComponent<UILabel>().text = "메뉴로 돌아가기";
        }

        internal void Update()
        {
            if (currState == State.Show)
            {
                if (ScoreBoard.isFinish == false)
                    ScoreBoard.Update();
                else
                {
                    Timer += Time.deltaTime;
                    if (Timer > Tick)
                    {
                        Timer -= Tick;
                        Current.Value.SetActive(true);
                        Current = Current.Next;
                    }
                }
                if (Current == null)
                    EndShowStartOpen();
            }
            else if (currState == State.Open)
            {
                if (remainChestCount <= 0)
                    EndOpen();
            }
        }

    }


}