using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SexyBackPlayScene
{
    class ResearchManager
    {
        public Dictionary<string, Research> researches = new Dictionary<string, Research>();

        public void Init()
        {
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += onHeroCreate;
            Singleton<ElementalManager>.getInstance().Action_ElementalCreateEvent += onElementalCreate;
        }

        private void onElementalCreate(Elemental elemental)
        {
            foreach (ResearchData item in Singleton<TableLoader>.getInstance().researchtable)
            {
                if (item.requireID == elemental.ID)
                {
                    Research research = new Research(item);
                    elemental.Action_ElementalChange += research.onElementalChange;
                    researches.Add(item.ID, research);
                }
            }
        }

        private void onHeroCreate(Hero hero)
        {
            foreach (ResearchData item in Singleton<TableLoader>.getInstance().researchtable)
            {
                if(item.requireID == hero.ID)
                {
                    Research research = new Research(item);
                    hero.Action_HeroChange += research.onHeroChange;
                    researches.Add(item.ID, research);
                }
            }

        }

        public void Update()
        {
            foreach (Research research in researches.Values)
            {
                research.Update();
            }
        }
    }
}
