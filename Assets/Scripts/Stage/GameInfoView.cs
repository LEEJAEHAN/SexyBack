using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class GameInfoView
    {
        UILabel TotalDpsLabel = ViewLoader.label_elementaldmg.GetComponent<UILabel>();
        UILabel HeroDpcLabel = ViewLoader.label_herodmg.GetComponent<UILabel>();
        UILabel label_exp = ViewLoader.label_exp.GetComponent<UILabel>();

        internal void Init()
        {
            Set();
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += BindHero;
            Singleton<ElementalManager>.getInstance().Action_ElementalCreateEvent += BindElemental;
            Singleton<StageManager>.getInstance().Action_ExpChange += PrintExp;
        }


        public void Set()
        { 
            TotalDpsLabel.text = "";
            HeroDpcLabel.text = "";
            label_exp.text = "";
        }

        public void PrintDps(Elemental elemenetal)
        {
            string dpsString = Singleton<ElementalManager>.getInstance().GetTotalDps().To5String() + " /Sec";
            TotalDpsLabel.GetComponent<UILabel>().text = dpsString;
        }

        void PrintDpc(Hero hero)
        {
            string dpsString = hero.DPC.To5String() + " /Tap";
            HeroDpcLabel.text = dpsString;
        }

        void PrintExp(BigInteger exp)
        {
            string expstring = exp.To5String() + " EXP";
            label_exp.text = expstring;
        }
        void BindHero(Hero hero)
        {
            hero.Action_HeroChange += PrintDpc;
        }

        private void BindElemental(Elemental sender)
        {
            sender.Action_ElementalChange += PrintDps;
        }

    }
}