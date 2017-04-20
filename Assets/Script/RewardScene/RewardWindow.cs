using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackRewardScene
{
    internal class RewardWindow : IDisposable
    {
        GameObject Window;
        LinkedList<ShowObjects> showLists;
        LinkedListNode<ShowObjects> current;
        bool endShow = false;
        public bool endTween = false;
        double timer = 0;
        const double Tick = 0.5f;

        public void Dispose()
        {
            GameObject.Destroy(Window);
            showLists.Clear();
            showLists = null;
            current = null;
        }

        internal void InitWindow()
        {
            Window = GameObject.Find("RewardWindow");
            Window.transform.FindChild("Title").GetComponent<UILabel>().text = "" + "완료";
            Window.SetActive(false);
            endShow = false;
            endTween = false;
            timer = 0;
        }

        internal void SetResult(Result result) //             FillWindow();
        {
            if (Window == null)
                InitWindow();

            showLists = new LinkedList<ShowObjects>();

            GameObject time = Window.transform.FindChild("Box1/Time").gameObject;
            time.GetComponent<UILabel>().text = result.TimeText;
            showLists.AddLast(new ShowObjects(time));

            GameObject scores = Window.transform.FindChild("Box1/ScoreScrollView/Scores").gameObject;
            showLists.AddLast(new ShowTextObject(result.ScoreText, scores));

            GameObject totalScore = Window.transform.FindChild("Box1/TotalScore").gameObject;
            totalScore.GetComponent<UILabel>().text = result.TotalScoreText;
            showLists.AddLast(new ShowObjects(totalScore));

            GameObject rank = Window.transform.FindChild("Box1/Rank").gameObject;
            rank.GetComponent<UILabel>().text = result.RankText;
            showLists.AddLast(new ShowObjects(rank));

            GameObject rankIcon = Window.transform.FindChild("Box1/RankIcon").gameObject;
            rankIcon.GetComponent<UISprite>().spriteName = result.RankIcon;
            showLists.AddLast(new ShowObjects(rankIcon));
        }

        internal bool ShowAll()
        {
            if (Window == null || current == null)
                return false;

            while (true)
            {
                if (current.Value.Show())
                {
                    if (current.Next == null)
                    {
                        endShow = true;
                        break;
                    }
                    else
                        current = current.Next;
                }
            }
            endShow = true;
            endTween = true;

            return true;
        }

        internal void SetReward(Reward currentReward)
        {

        }


        internal void SetShowSequence()
        {
            current = showLists.First;
        }


        internal void ActiveWindow()
        {
            if (!Window.activeInHierarchy)
            {
                Window.SetActive(true);
                endTween = false;
                Window.GetComponent<UITweener>().PlayForward();
            }
        }

        internal void Update()
        {
            if (endShow || !endTween)
                return;

            timer += Time.deltaTime;

            if (timer > Tick)
            {
                timer -= Tick;
                if (current.Value.Show())
                {
                    if (current.Next == null)
                    {
                        endShow = true;
                        return;
                    }
                    else
                        current = current.Next;
                }
            }
        }

    }

    public class ShowObjects
    {
        protected GameObject view;

        public ShowObjects ( GameObject view )
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
        public ShowTextObject(LinkedList<string> text, GameObject view) : base(view)
        {
            sequencelabel = text;
            current = sequencelabel.First;
            end = false;
        }
        public override bool Show()
        {
            if(view.activeInHierarchy == false)
                view.SetActive(true);

            view.GetComponent<UILabel>().text += current.Value;

            if (current.Next == null)
                end = true;
            else
                current = current.Next;

            return end;
        }
    }

}