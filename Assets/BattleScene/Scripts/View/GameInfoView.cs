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
        GameObject LABEL_FLOOR;

        UITable sorter;


        internal void Init()
        {
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += BindHero;
            Singleton<ElementalManager>.getInstance().Action_ElementalCreateEvent += BindElemental;
            Singleton<StatManager>.getInstance().Action_ExpChange += PrintExp;
            Set();
        }

        public void Set()
        {
            sorter = GameObject.Find("Table_MiddleText").GetComponent<UITable>();
            LABEL_EXP = GameObject.Find("1EXP");
            LABEL_DPC = GameObject.Find("2DPC");
            LABEL_DPS = GameObject.Find("3DPS");
            LABEL_MINUSDPS = GameObject.Find("4MDPS");

            LABEL_DPS.SetActive(false);

            LABEL_FLOOR = GameObject.Find("FLOOR");
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
        void PrintDpc(Hero hero)
        {
            string context = hero.DPC.To5String() + " / 탭";
            SetMiddleText(hero.DPC, context, LABEL_DPC);
        }
        public void PrintDps(Elemental elemenetal)
        {
            BigInteger totalDps = Singleton<ElementalManager>.getInstance().GetTotalDps();
            string context = totalDps.To5String() + " / 초";
            SetMiddleText(totalDps, context, LABEL_DPS);
        }
        public void PrintMinusDps(BigInteger totalMinusDps)
        {
            string context = "-" + totalMinusDps.To5String() + " / 초";
            SetMiddleText(totalMinusDps, context, LABEL_MINUSDPS);
        }
        internal void PrintStage(int floor)
        {
            LABEL_FLOOR.GetComponent<UILabel>().text = floor.ToString();
        }

        void BindHero(Hero hero)
        {
            hero.Action_Change += PrintDpc;
        }

        private void BindElemental(Elemental elemental)
        {
            elemental.Action_Change += PrintDps;
        }


    }
}