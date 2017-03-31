using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class ConsumableManager : IDisposable
    {
        ~ConsumableManager()
        {
            sexybacklog.Console("TalentManager 소멸");
        }

        public void Dispose()
        {
            ConsumableWindow.Clear();
            window = null;
        }

        ConsumableWindow window;
        int CurrentFloor;
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
            window = ConsumableWindow.getInstance;
            MakeTalent();
        }

        private void MakeTalent()
        {
            foreach (TalentData table in Singleton<TableLoader>.getInstance().talenttable)
            {
                if (table.type == TalentType.Attack)
                    AttackTalents.Add(new Talent(table));
                else if (table.type == TalentType.Elemental)
                    ElementTalents.Add(new Talent(table));
                else if (table.type == TalentType.Util)
                    UtilTalents.Add(new Talent(table));
            }
        }

        public void ShowNewTalentWindow(int floor)
        {
            CurrentFloor = floor;
            window.FillWindow(CurrentFloor, Alevel, Elevel, Ulevel);
            CurrentATalent = PickRandomTalent(CurrentFloor, AttackTalents);
            CurrentETalent = PickRandomTalent(CurrentFloor, ElementTalents);
            CurrentUTalent = PickRandomTalent(CurrentFloor, UtilTalents);

            window.FillTalents(CurrentATalent, CurrentETalent, CurrentUTalent);
            window.Show();
        }

        private Talent PickRandomTalent(int floor, List<Talent> list)
        {
            Talent result = PickRandomOne(list);
            if (result != null)
                result.SetFloor(floor);
            return result;
        }
        private Talent PickRandomOne(List<Talent> list)
        {
            Talent AbsResult = CheckAbsouluteRate(list);
            if (AbsResult == null)
                return CheckRelativeRate(list);
            return AbsResult;
        }
        private Talent CheckAbsouluteRate(List<Talent> list)
        {
            int rand = UnityEngine.Random.Range(0, 100);
            foreach (Talent one in list)
            {
                if (one.AbsRate == true)
                {
                    rand -= one.Rate;
                    if (rand < 0)
                        return one;
                }
            }
            return null;
        }
        private Talent CheckRelativeRate(List<Talent> list)
        {
            int sumDensity = GetTotalDensity(list);
            int rand = UnityEngine.Random.Range(0, sumDensity);

            foreach (Talent one in list)
            {
                if (one.AbsRate == false)
                {
                    rand -= one.Rate;
                    if (rand < 0)
                        return one;
                }
            }
            return null;
        }
        private int GetTotalDensity(List<Talent> list)
        {
            int result = 0;
            foreach (Talent one in list)
            {
                if (one.AbsRate == false)
                    result += one.Rate;
            }
            return result;
        }

        internal void Refresh()
        {
            window.ClaerSlot();
            CurrentATalent = PickRandomTalent(CurrentFloor, AttackTalents);
            CurrentETalent = PickRandomTalent(CurrentFloor, ElementTalents);
            CurrentUTalent = PickRandomTalent(CurrentFloor, UtilTalents);
            window.FillTalents(CurrentATalent, CurrentETalent, CurrentUTalent);
        }
        public void Update()
        {
            UpdateTalents(AttackTalents, TalentType.Attack);
            UpdateTalents(ElementTalents, TalentType.Elemental);
            UpdateTalents(UtilTalents, TalentType.Util);
        }

        private void UpdateTalents(List<Talent> talents, TalentType type)
        {
            if(confirm == type)
            {
                UpgradeTalentBonus(confirm);
                confirm = 0;
            }

            foreach (Talent t in talents)
                t.Update();
        }

        void UpgradeTalentBonus(TalentType type)
        {
            Bonus typebonus = null;
            GridItemIcon typeicon = new GridItemIcon();
            switch (type)
            {
                case TalentType.Attack:
                    {
                        Alevel++;
                        typebonus = new Bonus("hero", "DpcIncreaseXH", 1, null);
                        typeicon = new GridItemIcon("Icon_12", Alevel.ToString());
                        break;
                    }
                case TalentType.Elemental:
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
            Action_ConfirmTalent();
        }

        TalentType confirm = 0;

        internal void Confirm(TalentType type)
        {
            confirm = type;
            switch (type)
            {
                case TalentType.Attack:
                    {
                        if (CurrentATalent != null)
                            CurrentATalent.Confirm();
                        break;
                    }
                case TalentType.Elemental:
                    {
                        if (CurrentETalent != null)
                            CurrentETalent.Confirm();
                        break;
                    }
                case TalentType.Util:
                    {
                        if (CurrentUTalent != null)
                            CurrentUTalent.Confirm();
                        break;
                    }
            }
            CurrentATalent = null;
            CurrentETalent = null;
            CurrentUTalent = null;
            window.Hide();
        }
    }

}
