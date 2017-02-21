using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class Research : IDisposable, IHasGridItem, IStateOwner
    {
        public WeakReference owner;
        string ID;

        public GridItem itemView;
        GridItemIcon icon;
        string InfoName;
        string Description;
        string ViewText;

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
        public double ResearchTick;

        public bool Selected = false;
        public bool Purchase = false;
        public bool RefreshFlag = false;

        public ResearchStateMachine StateMachine;
        public string GetID { get { return ID; } }
        public string CurrentState { get { return StateMachine.currStateID; } }

        public Research(ResearchData data, ICanLevelUp root, GridItem itemview, double time, BigInteger totalprice, double tick)
        {
            ID = data.ID;
            bonuses = data.bonuses;
            RequireLevel = data.requeireLevel;
            InfoName = data.InfoName;
            Description = data.InfoDescription;
            SortOrder = data.level + data.baselevel;

            ResearchTime = time;
            ReducedTime = ResearchTime;
            ReduceTimeX = 1;
            ReduceTime = 1;
            ResearchTick = tick;

            StartPrice = ((100 - data.rate) * totalprice) / 100;
            ResearchPrice = data.rate * totalprice / 100;
            //PricePerSec = ResearchPrice / (int)ResearchTime; setstat이나 instantcheck시 설정된다.

            icon = data.icon;
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
            Refresh();
        }

        public void FillInfoView(bool InstanceBuy)
        {
            if (!Selected)
                return;

            InfoPanel panel = Singleton<InfoPanel>.getInstance();
            ViewText = MakeDescriptionText(InstanceBuy);
            panel.Show(Selected, icon, ViewText);
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
            ReduceTimeX = researchStat.ReduceTimeX;
            ReduceTime = researchStat.ReduceTime;
            double PrevTime = ReducedTime;
            ReducedTime = ResearchTime / researchStat.ReduceTimeX - researchStat.ReduceTime;
            if (ReducedTime <= 0)
            {
                ReducedTime = 0;
                RemainTime = 0;
            }
            if (RemainTime > 0)
                RemainTime = RemainTime * ReducedTime / PrevTime; // 연구중인 시간도 준다.
            if (ReducedTime >= 1) // 최소 1초는 보장해야함.
                PricePerSec = ResearchPrice / (int)ReducedTime;

            Refresh();
        }

        // function
        private string MakeDescriptionText(bool InstanceBuy)
        {
            string temp = "";
            temp += InfoName + "\n";
            temp += Description + "\n";
            temp += "비용 : " + StartPrice.To5String() + "\n";

            if (!InstanceBuy)
            {
                temp += "연구시간 : " + ((int)ReducedTime).ToString() + " Sec\n";
                temp += "연구비용 : " + PricePerSec.To5String() + " /Sec";
            }

            return temp;
        }

        public void Refresh()
        {
            RefreshFlag = true;
        }


        // update view state
    }
}