﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class TalentManager
    {
        TalentPanel window;
        public List<Talent> AttackTalents = new List<Talent>();
        public List<Talent> ElementTalents = new List<Talent>();
        public List<Talent> UtilTalents = new List<Talent>();
        int Alevel = 0;
        int Elevel = 0;
        int Ulevel = 0;
        public Talent CurrentATalent;
        public Talent CurrentETalent;
        public Talent CurrentUTalent;

        public delegate void ConfirmTalent_Event();
        public event ConfirmTalent_Event Action_ConfirmTalent;

        internal void Init()
        {
            window = TalentPanel.getInstance;
            window.Init();
            MakeTalent();
        }

        private void MakeTalent()
        {
            foreach (TalentData table in Singleton<TableLoader>.getInstance().talenttable)
            {
                if (table.type == TalentType.Attack)
                    AttackTalents.Add(new Talent(table));
                else if (table.type == TalentType.Element)
                    ElementTalents.Add(new Talent(table));
                else if (table.type == TalentType.Util)
                    UtilTalents.Add(new Talent(table));
            }
        }


        public void ShowNewTalentWindow(int floor)
        {
            window.FillWindow(floor, Alevel, Elevel, Ulevel);
            CurrentATalent = PickRandomTalent(AttackTalents);
            CurrentETalent = PickRandomTalent(ElementTalents);
            CurrentUTalent = PickRandomTalent(UtilTalents);

            window.FillTalents(CurrentATalent, CurrentETalent, CurrentUTalent);
            window.Show();
        }

        private Talent PickRandomTalent(List<Talent> list)
        {
            if (list.Count == 0)
                return null;
            int rndindex = UnityEngine.Random.Range(0, list.Count);
            return list[rndindex];
        }

        internal void Refresh()
        {
            window.ClaerSlot();
            CurrentATalent = PickRandomTalent(AttackTalents);
            CurrentETalent = PickRandomTalent(ElementTalents);
            CurrentUTalent = PickRandomTalent(UtilTalents);
            window.FillTalents(CurrentATalent, CurrentETalent, CurrentUTalent);
        }
        public void Update()
        {
            UpdateTalents(AttackTalents);
            UpdateTalents(ElementTalents);
            UpdateTalents(UtilTalents);
        }

        private void UpdateTalents(List<Talent> talents)
        {
            foreach(Talent t in talents)
                t.Update();
        }

        internal void UpgradeTalentBonus(TalentType type)
        {
            Bonus typebonus = null;
            GridItemIcon typeicon = null;
            switch (type)
            {
                case TalentType.Attack:
                    {
                        Alevel++;
                        typebonus = new Bonus("hero", "DpcIncreaseXH", 1, null);
                        typeicon = new GridItemIcon("Icon_12", Alevel.ToString());
                        break;
                    }
                case TalentType.Element:
                    {
                        Elevel++;
                        typebonus = new Bonus("elementals", "DpsIncreaseXH", 1, null);
                        typeicon = new GridItemIcon("Icon_13", Elevel.ToString());
                        break;
                    }
                case TalentType.Util:
                    {
                        Ulevel++;
                        typebonus = new Bonus("player", "ExpIncreaseXH", 1, null);
                        typeicon = new GridItemIcon("Icon_17", Ulevel.ToString());
                        break;
                    }
            }
            Singleton<StatManager>.getInstance().Upgrade(typebonus, typeicon);
        }

        internal void Confirm(TalentType type)
        {
            switch (type)
            {
                case TalentType.Attack:
                    {
                        CurrentATalent.Confirm();
                        break;
                    }
                case TalentType.Element:
                    {
                        CurrentETalent.Confirm();
                        break;
                    }
                case TalentType.Util:
                    {
                        CurrentUTalent.Confirm();
                        break;
                    }
            }
            CurrentATalent = null;
            CurrentETalent = null;
            CurrentUTalent = null;
            window.Hide();
            Action_ConfirmTalent();
        }
    }

}
