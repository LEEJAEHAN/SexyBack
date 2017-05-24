using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class Consumable : IDisposable, IHasGridItem   // stack 항목.
    {
        internal enum Type
        {
            AttackCount,        // int value
            Skill,              // string ID
            Buff,               // buff id or 버프능력세부
            ResearchTime,       // int value
            LevelUp,            // int value
            Exp                 // 아예 획득시부터 루틴이다름.
        }

        public string GetID { get { return baseData.id; } }
        public readonly ConsumableData baseData;
        public int Stack;
        public double RemainCoolTime;

        bool Confirm = false;
        protected GridItem itemView;

        public Consumable(ConsumableData data, int s = 0)
        {
            baseData = data;
            Stack = s;
            if (s == 0)
                Stack = baseData.stackPerChest;
        }
       
        public string Description
        {
            get
            {
                //return StringParser.ReplaceString(description, bonus.strvalue);
                //return StringParser.ReplaceString(description, bonus.value.ToString());
                return null;
            }
        }

        public void Update()
        {   // state machine
            if(RemainCoolTime > 0 )
            {
                RemainCoolTime -= Time.deltaTime;
                itemView.DrawCoolMask((float)(RemainCoolTime / baseData.CoolTime));
            }

            if (Confirm)
                Use();

            if( Stack <= 0 && RemainCoolTime <= 0 )     // 스택만 0이고 쿨은 남아있을때는 객체는 남아있다. 단 view는 off상태다.
            {
                // Destroy();
            }
        }
        internal void onConfirm()       // Window로부터 받는 이벤트
        {
            if (RemainCoolTime > 0)
                return;
            Stack--;
            RemainCoolTime = baseData.CoolTime;

            ViewRefresh();
            Confirm = true;
        }

        internal void ActiveView()
        {
            itemView = new GridItem(GridItem.Type.Consumable, GetID, baseData.icon, this);
            ViewRefresh();
        }

        internal void Merge(Consumable consumable)
        {
            if (GetID != consumable.GetID)
                return;

            Stack += consumable.Stack;
            ViewRefresh();
        }

        public void Use()
        {
            //Singleton<InstanceStatus>.getInstance().ApplyBonusWithIcon(bonus, Icon);
            Confirm = false;
        }

        // function
        public void Dispose()
        {

        }

        public void onSelect(string id)
        {
            sexybacklog.Console("소모품" + id + " 고름");
        }

        // update view state
        public void ViewRefresh()
        {
            itemView.DrawStack(Stack);
            itemView.DrawCoolMask((float)(RemainCoolTime / baseData.CoolTime));
            if (this.Stack > 0)
                itemView.SetActive(true);
            else
                itemView.SetActive(false);
        }
    }
}