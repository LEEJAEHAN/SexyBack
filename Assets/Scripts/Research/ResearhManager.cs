using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SexyBackPlayScene
{
    class ResearhManager
    {
        public Dictionary<string, Research> researches = new Dictionary<string, Research>();

        public void Init()
        {
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += onHeroCreate;
        }

        private void onHeroCreate(Hero hero)
        {
            foreach (ResearchData item in Singleton<TableLoader>.getInstance().researchtable)
            {
                if(item.ID == hero.ID)
                {
                    Research research = new Research(item);
                    research.view.Action_ResearchSelect += onResearchSelect;
                    research.view.Action_ResearchStart += onResarchStart;
                }
                    researches.Add(item.ID,);
            }

        }

        private void onResearchSelect(string name)
        {
            throw new NotImplementedException();
        }

        private void onResarchStart(string name)
        {
            throw new NotImplementedException();
        }

    }
}
