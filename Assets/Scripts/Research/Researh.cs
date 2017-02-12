using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class Research : IDisposable, IHasGridItem, IStateOwner
    {
        public WeakReference owner;
        string ID;
        string IconName;
        string InfoName;
        string Description;
        string ViewText;

        public GridItem itemView;

        public int RequireLevel;

        public BigInteger ResearchPrice = new BigInteger(1);
        public BigInteger StartPrice = new BigInteger(1); // 
        public BigInteger PricePerSec = new BigInteger(1); // 

        public readonly double ResearchTime; // 
        public double ReducedTime; // 
        public int ReduceTimeX;
        public int ReduceTime;

        List<Bonus> bonuses;

        public int SortOrder;
        public double RemainTime;
        public static double ResearchTick = 1f;
        //// 변수 
        //int ReduceTimeX = 1;
        //int ReduceTime = 0;

        // show, enable condition
        public bool Selected = false;
        public bool Purchase = false;

        public ResearchStateMachine StateMachine;

        public string GetID { get { return ID; } }
        public string CurrentState { get { return StateMachine.currStateID; } }

        public Research(ResearchData data, ICanLevelUp root, GridItem itemview, double time, BigInteger totalprice)
        {
            ID = data.ID;
            bonuses = data.bonuses;
            RequireLevel = data.requeireLevel;
            IconName = data.IconName;
            InfoName = data.InfoName;
            Description = data.InfoDescription;
            SortOrder = data.level + data.baselevel;

            ResearchTime = time;
            StartPrice = ((100 - data.rate) * totalprice) / 100;
            ResearchPrice = data.rate * totalprice / 100;
            PricePerSec = ResearchPrice / (int)ResearchTime;

            itemView = itemview;
            itemview.AttachEventListner(this);

            StateMachine = new ResearchStateMachine(this);
            owner = new WeakReference(root);
        }
        public void Dispose()
        {
            itemView.Dispose();
            StateMachine = null;
        }
        ~Research() { sexybacklog.Console("리서치 제 ㅋ 거 ㅋ"); }

        public void Update()
        {   // state machine
            StateMachine.Update();
        }
        public void DoUpgrade()
        {
            foreach (Bonus bonus in bonuses)
                Singleton<Player>.getInstance().Upgrade(bonus);
        }

        public void onSelect(string id)
        {
            if (id == null)
            {
                InfoPanel panel = Singleton<InfoPanel>.getInstance();
                panel.SetPauseButton(Selected, false, "");
                panel.SetConfirmButton(Selected, true);
                panel.Hide();
                Selected = false;
                return;
            }

            Selected = true;
            FillInfoView();
        }

        void FillInfoView()
        {
            if (!Selected)
                return;

            InfoPanel panel = Singleton<InfoPanel>.getInstance();

            ViewText = MakeDescriptionText(InfoName, Description, StartPrice, (int)ReduceTime, PricePerSec);
            panel.Show(Selected, IconName, ViewText);

            if (CurrentState == "Ready")
                panel.SetConfirmButton(Selected, Singleton<Player>.getInstance().EXP >= StartPrice);
            else
                panel.SetConfirmButton(Selected, false);

            if (CurrentState == "Pause")
                panel.SetPauseButton(Selected, true, "Work");
            else if (CurrentState == "Work")
                panel.SetPauseButton(Selected, true, "Pause");
            else
                panel.SetPauseButton(Selected, false, "");
        }


        public void onConfirm(string id)
        {
            Purchase = true;
            Singleton<InfoPanel>.getInstance().SetConfirmButton(Selected, false); // 중복입력 막는다.
        }

        public void onPause(string id)
        {
            if (CurrentState == "Pause")
                StateMachine.ChangeState("Work");
            else if (CurrentState == "Work")
                StateMachine.ChangeState("Pause");
        }

        internal void SetStat(ResearchUpgradeStat researchStat)
        {
            if (CurrentState == "Ready" || CurrentState == "None")
            {
                ReduceTimeX = researchStat.ReduceTimeX;
                ReduceTime = researchStat.ReduceTime;
                ReducedTime = ResearchTime / researchStat.ReduceTimeX - researchStat.ReduceTime;
                if(ReducedTime > ResearchTick) // 최소 1틱은 보장해야함.
                    PricePerSec = ResearchPrice / (int)ReducedTime;
            }
            else if (CurrentState == "Work" || CurrentState == "Pause")
            {
                //TODO : 여기해야할차례
//                ResearchTime /= researchStat.;
  //              re
            }
        }

        // function
        private string MakeDescriptionText(string infoName, string description, BigInteger startPrice, int researchTime, BigInteger pricePerSec)
        {
            string temp = "";
            temp += infoName + "\n";
            temp += description + "\n";
            temp += "비용 : " + startPrice.To5String() + " EXP\n";

            if (researchTime > ResearchTick)
            {
                temp += "연구시간 : " + researchTime.ToString() + " 초\n";
                temp += "연구비용 : 초당 " + pricePerSec.To5String() + " EXP";
            }

            return temp;
        }

        // update view state
        public void Refresh()
        {
        }

    }
}