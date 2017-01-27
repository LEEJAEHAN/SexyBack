using System;
using UnityEngine;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    public class GameProgress
    {
        UILabel TotalDpsLabel = ViewLoader.label_elementaldmg.GetComponent<UILabel>();
        UILabel HeroDpcLabel = ViewLoader.label_herodmg.GetComponent<UILabel>();

        internal void Init()
        {
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += BindHero;
            Singleton<ElementalManager>.getInstance().Action_ElementalListChangeEvent += PrintDps;
        }
        public void Start()
        {
        }
        internal void Update()
        {
        }

        internal BigInteger GetTotalDPS(List<Elemental> sender)
        {
            BigInteger result = new BigInteger();
            foreach (Elemental elemental in sender)
            {
                result += elemental.DPS;
            }
            return result;
        }
        public void PrintDps(List<Elemental> sender)
        {
            string dpsString = GetTotalDPS(sender).To5String() + " /Sec";
            TotalDpsLabel.GetComponent<UILabel>().text = dpsString;
        }

        void PrintDpc(Hero hero)
        {
            string dpsString = hero.DPC.To5String() + " /Tap";
            HeroDpcLabel.text = dpsString;
        }

        void BindHero(Hero hero)
        {
            hero.Action_HeroChange += PrintDpc;
        }

    }
}