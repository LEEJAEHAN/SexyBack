﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
namespace SexyBackPlayScene
{
    [Serializable]
    internal class Research : IDisposable, IHasGridItem, IStateOwner, ISerializable
    {
        //public WeakReference owner;
        string ID;

        public GridItem itemView;
        GridItemIcon icon;
        public ResearchWindow Panel = ResearchWindow.getInstance;

        string Name;
        string Description;
        public int RequireLevel;
        Bonus bonus;

        // 오리지널 값.
        public BigInteger researchprice = new BigInteger(1);
        public BigInteger startprice = new BigInteger(1);

        // stat에 의해 계산되는값.
        public BigInteger ResearchPrice;
        public BigInteger StartPrice;
        public BigInteger PricePerSec;
        public readonly double ResearchTime;
        public double ReducedTime;

        public int SortOrder;
        public double RemainTime;
        public double ResearchTick;

        public bool Selected = false;
        public bool Purchase = false;
        public bool RefreshFlag = false;

        public ResearchStateMachine StateMachine;

        public string SavedState;
        public string GetID { get { return ID; } }
        public string CurrentState { get { return StateMachine.currStateID; } }

        public delegate void InstantFinish_Event();
        public event InstantFinish_Event Action_InstantFinish = delegate { };


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SavedState", CurrentState);
            info.AddValue("RemainTime", RemainTime);

        }
        public Research(SerializationInfo info, StreamingContext context)
        {
            SavedState = (string)info.GetValue("SavedState", typeof(string));
            RemainTime = (double)info.GetValue("RemainTime", typeof(double));
        }


        public Research(ResearchData data, GridItem itemview, double time, BigInteger totalprice, double tick)
        {
            ID = data.ID;
            bonus = data.bonus;
            RequireLevel = data.requeireLevel;
            Name = data.InfoName;
            Description = data.InfoDescription;
            SortOrder = data.level + data.baselevel;

            ResearchTime = time;
            ReducedTime = ResearchTime;
            ResearchTick = tick;

            startprice = ((100 - data.rate) * totalprice) / 100;
            researchprice = data.rate * totalprice / 100;

            icon = data.icon;
            itemView = itemview;
            itemview.AttachEventListner(this);

            StateMachine = new ResearchStateMachine(this);
        }
        public void Dispose()
        { // 완전히 해제할때
            itemView.Dispose();
            itemView = null;
            StateMachine = null;
            Action_InstantFinish = null;
            Panel = null;
        }
        
        ~Research() { sexybacklog.Console("리서치소멸"); }

        public void Update()
        {   // state machine
            StateMachine.Update();
        }
        public void DoUpgrade()
        {
            Singleton<StatManager>.getInstance().Upgrade(bonus, icon);
        }

        public void onSelect(string id)
        {
            if (id == null)
            {
                Panel.SetButton2(Selected, false, "");
                Panel.SetButton1(Selected, true, true);

                Panel.Action_Confirm -= this.onConfirm;
                Panel.Action_Pause -= this.onPause;

                Panel.Hide();

                Selected = false;
                return;
            }

            Selected = true;
            Panel.Action_Confirm += this.onConfirm;
            Panel.Action_Pause += this.onPause;
            Refresh();
        }

        public void FillInfoView(bool InstanceBuy)
        {
            if (!Selected)
                return;

            string pricename = "";
            string pricevalue = "";

            if (!InstanceBuy)
            {
                pricename = "비용\n\n시간";
                pricevalue = StartPrice.To5String() + " 경험치\n" + PricePerSec.To5String() + " 경험치 / 초\n" + ((int)ReducedTime).ToString() + " 초";
            }
            else
            {
                pricename = "비용";
                pricevalue = StartPrice.To5String() + " 경험치";
            }

            Panel.Show(Selected, icon, Name, Description, pricename, pricevalue);
        }

        public void onConfirm()
        {
            if (CurrentState == "Ready")
            {
                Purchase = true;
                Panel.SetButton1(Selected, false, false); // 중복입력 막는다.
            }
        }

        public void onPause()
        {
            if (CurrentState == "Pause")
                StateMachine.ChangeState("Work");
            else if (CurrentState == "Work")
                StateMachine.ChangeState("Pause");
        }

        internal void SetStat(PlayerStat stat)
        {
            double PrevTime = ReducedTime;
            ReducedTime = ResearchTime / stat.ResearchTimeX - stat.ResearchTime;
            ResearchPrice = researchprice * stat.ResearchPriceXH / 100;
            StartPrice = startprice * stat.ResearchPriceXH / 100;

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
        
        internal void Finish()
        {
            if (CurrentState == "Work")
                Action_InstantFinish();
        }

        public void Refresh()
        {
            RefreshFlag = true;
        }


    }
}