using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackRewardScene
{
    public class ShowObjects
    {
        protected GameObject view;

        public ShowObjects(GameObject view)
        {
            this.view = view;
            view.SetActive(false);
        }
        public virtual bool Show()
        {
            if (view.activeInHierarchy == false)
                view.SetActive(true);

            return true;
        }
    }

    public class ShowTextObject : ShowObjects
    {
        LinkedList<string> sequencelabel;
        LinkedListNode<string> current;
        bool end = false;
        public ShowTextObject(ResultReward result, GameObject view) : base(view)
        {
            sequencelabel = new LinkedList<string>();
            //sequencelabel.AddLast("돌파 점수 +" + result.Score["floorScore"].ToString() + "\n");
            //sequencelabel.AddLast("클리어 점수 +" + result.Score["clearScore"].ToString() + "\n");
            sequencelabel.AddLast("시간 점수 +" + result.Score["timeScore"].ToString() + "\n");
            sequencelabel.AddLast("레벨업 정도에 따라 +" + result.Score["levelScore"].ToString() + "\n");
            sequencelabel.AddLast("연구 정도에 따라 +" + result.Score["researchScore"].ToString() + "\n");
            sequencelabel.AddLast("랭크보너스 +" + result.Score["bonusScore"].ToString());
            current = sequencelabel.First;
            end = false;
        }

        public override bool Show()
        {
            if (view.activeInHierarchy == false)
                view.SetActive(true);

            view.GetComponent<UILabel>().text += current.Value;

            if (current.Next == null)
                end = true;
            else
                current = current.Next;

            return end;
        }
    }

    internal class ScoreBoardView
    {
        LinkedList<ShowObjects> RankResults;
        LinkedListNode<ShowObjects> Current;

        GameObject view;

        double TickTimer = 0;
        const double Tick = 0.1f;
        public bool isFinish = false;

        public ScoreBoardView(GameObject v)
        {
            view = v;
            TickTimer = 0;
            isFinish = false;
         }

        internal void SetResultView(ResultReward result) //             FillWindow();
        {
            RankResults = new LinkedList<ShowObjects>();

            GameObject time = view.transform.FindChild("Time").gameObject;
            time.GetComponent<UILabel>().text = StringParser.GetTimeText(result.ClearTime);
            RankResults.AddLast(new ShowObjects(time));

            GameObject scores = view.transform.FindChild("ScoreScrollView/Scores").gameObject;
            RankResults.AddLast(new ShowTextObject(result, scores));

            GameObject totalScore = view.transform.FindChild("TotalScore").gameObject;
            totalScore.GetComponent<UILabel>().text = "총점 " + result.TotalScore.ToString();
            RankResults.AddLast(new ShowObjects(totalScore));

            //GameObject rank = view.transform.FindChild("Rank").gameObject;
            //rank.GetComponent<UILabel>().text = "랭크 " + result.rank.ToString();
            //RankResults.AddLast(new ShowObjects(rank));

            GameObject rankIcon = view.transform.FindChild("RankIcon").gameObject;
            rankIcon.GetComponent<UISprite>().spriteName = "Rank_" + result.Rank.ToString();
            RankResults.AddLast(new ShowObjects(rankIcon));

            Current = RankResults.First;
        }
        internal void FinishShow()
        {
            while(Current != null)
            {
                if (Current.Value.Show())
                        Current = Current.Next;
            }
            isFinish = true;
        }
        internal void Update()
        {
            TickTimer += Time.deltaTime;
            if (TickTimer > Tick)
            {
                TickTimer -= Tick;
                if (Current.Value.Show())
                {
                    if (Current.Next == null)
                    {
                        isFinish = true;
                        return;
                    }
                    else
                        Current = Current.Next;
                }
            }
        }
    }
}