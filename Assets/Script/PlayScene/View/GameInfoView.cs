using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class GameInfoView
    {
        GameObject LABEL_DPC;
        GameObject LABEL_DPS;
        GameObject LABEL_MINUSDPS;
        GameObject LABEL_EXP;

        UITable sorter;

        internal void Init()
        {
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += BindHero;
            Singleton<ElementalManager>.getInstance().Action_ElementalCreateEvent += BindElemental;
            Singleton<InstanceStatus>.getInstance().Action_ExpChange += PrintExp;
            Singleton<InstanceStatus>.getInstance().Action_Buff += onExpBuff;
            Set();
        }

        public void Set()
        {
            sorter = GameObject.Find("Table_MiddleText").GetComponent<UITable>();
            LABEL_EXP = GameObject.Find("1EXP");
            LABEL_DPC = GameObject.Find("2EPC");
            LABEL_DPS = GameObject.Find("3EPS");
            LABEL_MINUSDPS = GameObject.Find("4MEPS");

            LABEL_EXP.SetActive(false);
            LABEL_DPS.SetActive(false);
            LABEL_DPC.SetActive(false);
            LABEL_MINUSDPS.SetActive(false);
        }

        public void SetMiddleText(BigInteger value, string context, GameObject tableobject)
        {
            if(value <= 0)
            {
                if(tableobject.activeInHierarchy)
                {
                    tableobject.SetActive(false);
                    sorter.Reposition();
                }
                return;
            }
            else
            {
                if(!tableobject.activeInHierarchy)
                {
                    tableobject.SetActive(true);
                    sorter.Reposition();
                }
                tableobject.GetComponent<UILabel>().text = context;
            }
        }
        public void PrintExp(BigInteger exp)
        {
            string context = exp.To5String() + " 경험치";
            SetMiddleText(exp, context, LABEL_EXP);
        }
        void PrintEpc(BigInteger Epc, bool buff)
        {
            if (buff)
                SetMiddleText(Epc, string.Format("[FFFF00]{0} 경험치 / 탭[-]", Epc.To5String()), LABEL_DPC);
            else
                SetMiddleText(Epc, string.Format("{0} 경험치 / 탭", Epc.To5String()), LABEL_DPC);
        }
        public void PrintEps(BigInteger Eps, bool buff)
        {
            if (buff)
                SetMiddleText(Eps, string.Format("[FFFF00]{0} 경험치 / 초[-]", Eps.To5String()), LABEL_DPS);
            else
                SetMiddleText(Eps, string.Format("{0} 경험치 / 초", Eps.To5String()), LABEL_DPS);
        }
        public void PrintMinusEps(BigInteger totalMinusDps)
        {
            SetMiddleText(totalMinusDps, string.Format("-{0} 경험치 / 초", totalMinusDps.To5String()), LABEL_MINUSDPS);
        }

        void BindHero(Hero hero)
        {
            hero.Action_Change += onHeroChange;
        }

        private void BindElemental(Elemental elemental)
        {
            elemental.Action_Change += onElementalChange;
        }

        private void onElementalChange(Elemental elemental)
        {
            bool elementalbuff;
            var expInfo = Singleton<InstanceStatus>.getInstance();
            BigInteger totalDps = Singleton<ElementalManager>.getInstance().GetTotalDps(out elementalbuff);
            var expbuff = Singleton<InstanceStatus>.getInstance().BuffCoef;

            PrintEps(totalDps * expInfo.BuffCoef, elementalbuff || expInfo.BuffCoef > 1);
        }

        public void onHeroChange(Hero hero)
        {
            var expInfo = Singleton<InstanceStatus>.getInstance();
            PrintEpc(hero.DPC * expInfo.BuffCoef, hero.BuffCoef > 1 || expInfo.BuffCoef > 1);
        }

        public void onExpBuff(int buffCoef)
        {
            bool elementalbuff;
            BigInteger totalDps = Singleton<ElementalManager>.getInstance().GetTotalDps(out elementalbuff);
            var expbuff = Singleton<InstanceStatus>.getInstance().BuffCoef;
            var hero = Singleton<HeroManager>.getInstance().GetHero();

            PrintEps(totalDps * buffCoef, elementalbuff || buffCoef > 1);
            PrintEpc(hero.DPC * buffCoef, hero.BuffCoef > 1 || buffCoef > 1);
        }

    }
}