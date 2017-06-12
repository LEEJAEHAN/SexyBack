using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class Consumable : IDisposable, IHasGridItem   // stack 항목.
    {

        ~Consumable()
        {
            sexybacklog.Console("Consumable 소멸");
        }
        internal enum Type
        {
            AttackCount,        // int value
            Skill,              // string ID
            HeroBuff,               // buff id or 버프능력세부
            ElementalBuff,
            ExpBuff,
            ResearchTime,       // int value
            ElementalLevelUp,            // int value
            HeroLevelUp,
            Exp,                 // 아예 획득시부터 루틴이다름.
            Gem
        }

        public string GetID { get { return baseData.id; } }
        internal int SortOrder { get { return baseData.order; } }
        public readonly ConsumableData baseData;
        public int Stack;
        public double RemainTime;
        protected GridItem itemView;
        ConsumableWindow Panel;

        bool Selected;

        bool isBuffItem
        {
            get
            {
                return baseData.type == Type.ElementalBuff ||
                    baseData.type == Type.HeroBuff ||
                    baseData.type == Type.ExpBuff;
            }
        }

        public Consumable(ConsumableData data, int s = -1)
        {
            sexybacklog.Console("Consumable 생성");
            baseData = data;
            Stack = s;
            if (s == -1)
                Stack = baseData.stackPerChest;
        }


        public void Update()
        {   // state machine
            if (RemainTime > 0)
            {
                RemainTime -= Time.deltaTime;
                itemView.DrawCoolMask((float)(RemainTime / baseData.CoolTime));
                if (RemainTime <= 0)
                {
                    RemainTime = 0;
                    EndUse();
                }
            }

            if (Stack <= 0 && RemainTime <= 0)     // 스택만 0이고 쿨은 남아있을때는 객체는 남아있다. 단 view는 off상태다.
            {
                Singleton<ConsumableManager>.getInstance().Destory(GetID);
            }
        }

        internal void ActiveView()
        {
            itemView = new GridItem(GridItem.Type.Consumable, GetID, baseData.icon, this);
            Panel = Singleton<ConsumableManager>.getInstance().Panel;
            ViewRefresh();
        }


        internal void Merge(Consumable consumable)
        {
            if (GetID != consumable.GetID)
                return;

            Stack += consumable.Stack;
            ViewRefresh();
        }

        internal void onConfirm()       // Window로부터 받는 이벤트
        {
            if (RemainTime > 0)
                return;

            if (Use())
            {
                Stack--;
                RemainTime = baseData.CoolTime;
                EffectController.getInstance.AddBuffEffect(baseData.icon);
                ViewRefresh();
            }
            else
            {
                if(baseData.strValue != null)
                    Panel.PrintConstraint(ConsumableManager.MakeConstrantText(baseData.strValue));
            }
        }
        public bool Use()
        {
            switch (baseData.type)
            {
                case Type.AttackCount:
                    Singleton<HeroManager>.getInstance().GetHero().AttackManager.AddAttackCount(baseData.value);
                    return true;
                case Type.Skill:
                    if (Singleton<ElementalManager>.getInstance().elementals.ContainsKey(baseData.strValue))
                    {
                        Singleton<ElementalManager>.getInstance().elementals[baseData.strValue].CastSkillItem();
                        return true;
                    }
                    break;
                case Type.ElementalBuff:
                case Type.HeroBuff:
                case Type.ExpBuff:
                    return ConsumableManager.Buff(true, baseData.type, baseData.strValue, baseData.value);
                case Type.ResearchTime:
                    return Singleton<ResearchManager>.getInstance().ShiftFrontOne(baseData.value);
                case Type.HeroLevelUp:
                    Singleton<HeroManager>.getInstance().LevelUp(baseData.value);
                    return true;
                case Type.ElementalLevelUp:
                    return Singleton<ElementalManager>.getInstance().LevelUp(baseData.strValue, baseData.value);
            }
            return false;
        }
        private void EndUse()
        {
            ConsumableManager.Buff(false, baseData.type, baseData.strValue, baseData.value);
            ViewRefresh();          // 쿨다찼을때 1회 작동.
        }

        internal void LoadUseState(double remainTime)
        {
            RemainTime = remainTime;
            if (RemainTime > 0)
                ConsumableManager.Buff(true, baseData.type, baseData.strValue, baseData.value);
        }

        // function
        public void Dispose()
        {
            itemView.Dispose();
        }

        public void onSelect(string id)
        {
            if (id == null)
            {
                Selected = false;
                Panel.Action_Confirm -= this.onConfirm;
                Panel.Hide();
                return;
            }

            Selected = true;
            Panel.Action_Confirm += this.onConfirm;
            ViewRefresh();
        }

        // update view state
        public void ViewRefresh()
        {
            itemView.DrawStack(Stack);
            itemView.DrawCoolMask((float)(RemainTime / baseData.CoolTime));
            if (Stack <= 0 && RemainTime <= 0)
                itemView.SetActive(false);
            else
                itemView.SetActive(true);

            if (Selected)
            {
                if (RemainTime <= 0)
                    Panel.SetButton1(Selected, true);
                else
                    Panel.SetButton1(Selected, false);

                Panel.Show(Selected, baseData, isBuffItem);
            }
        }

    }
}