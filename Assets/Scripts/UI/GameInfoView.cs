using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class GameInfoView
    {
        UILabel TotalDpsLabel = ViewLoader.label_elementaldmg.GetComponent<UILabel>();
        UILabel MinusDpsLabel = ViewLoader.label_minusdps.GetComponent<UILabel>();

        UILabel HeroDpcLabel = ViewLoader.label_herodmg.GetComponent<UILabel>();
        UILabel label_exp = ViewLoader.label_exp.GetComponent<UILabel>();

        UILabel label_floor = ViewLoader.label_floor.GetComponent<UILabel>();

        internal void Init()
        {
            Set();
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += BindHero;
            Singleton<ElementalManager>.getInstance().Action_ElementalCreateEvent += BindElemental;
            Singleton<Player>.getInstance().Action_ExpChange += PrintExp;
        }

        public void Set()
        { 
            TotalDpsLabel.text = "";
            HeroDpcLabel.text = "";
            label_exp.text = "";
            MinusDpsLabel.text = "";
            label_floor.text = "1 Floor";

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

        void PrintExp(BigInteger exp)
        {
            string expstring = exp.To5String() + "";
            label_exp.text = expstring;
        }
        void BindHero(Hero hero)
        {
            hero.Action_DamageChange += PrintDpc;
        }

        private void BindElemental(Elemental elemental)
        {
            elemental.Action_DamageChange += PrintDps;
        }

        internal void PrintStage(int currentFloor)
        {
            label_floor.text = currentFloor + " Floor";
        }
    }
}