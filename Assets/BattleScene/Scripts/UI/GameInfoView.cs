using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class GameInfoView
    {
        GameObject label_herodmg;
        GameObject label_elementaldmg;
        GameObject label_minusdps;
        GameObject label_exp;
        GameObject label_floor;

        UILabel TotalDpsLabel;
        UILabel MinusDpsLabel;
        UILabel HeroDpcLabel;
        UILabel ExpLabel;
        UILabel FloorLabel;       

        internal void Init()
        {
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += BindHero;
            Singleton<ElementalManager>.getInstance().Action_ElementalCreateEvent += BindElemental;
            Singleton<StatManager>.getInstance().Action_ExpChange += PrintExp;
            Set();
        }

        public void Set()
        {
            label_herodmg = GameObject.Find("label_herodmg");
            label_elementaldmg = GameObject.Find("label_elementaldmg");
            label_floor = GameObject.Find("label_floor");
            label_exp = GameObject.Find("label_exp");
            label_minusdps = GameObject.Find("label_minusdps");

            TotalDpsLabel = label_elementaldmg.GetComponent<UILabel>();
            MinusDpsLabel = label_minusdps.GetComponent<UILabel>();
            HeroDpcLabel = label_herodmg.GetComponent<UILabel>();
            ExpLabel = label_exp.GetComponent<UILabel>();
            FloorLabel = label_floor.GetComponent<UILabel>();

            TotalDpsLabel.text = "";
            HeroDpcLabel.text = "";
            ExpLabel.text = "";
            MinusDpsLabel.text = "";
            FloorLabel.text = "";
        }

        public void PrintDps(Elemental elemenetal)
        {
            string dpsString = Singleton<ElementalManager>.getInstance().GetTotalDps().To5String() + " /Second";
            TotalDpsLabel.GetComponent<UILabel>().text = dpsString;
        }

        public void PrintMinusDps(BigInteger dps)
        {
            if (dps <= 0 )
            {
                MinusDpsLabel.text = "";
                return;
            }
            string minusdps = "-" + dps.To5String() + " /Second";
            MinusDpsLabel.text = minusdps;
        }

        void PrintDpc(Hero hero)
        {
            string dpsString = hero.DPC.To5String() + " /Touch";
            HeroDpcLabel.text = dpsString;
        }

        public void PrintExp(BigInteger exp)
        {
            string expstring = exp.To5String() + "";
            ExpLabel.text = expstring;
        }
        void BindHero(Hero hero)
        {
            hero.Action_Change += PrintDpc;
        }

        private void BindElemental(Elemental elemental)
        {
            elemental.Action_Change += PrintDps;
        }

        internal void PrintStage(int currentFloor)
        {
            FloorLabel.text = currentFloor + " Floor";
        }

    }
}