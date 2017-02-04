﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class LevelUpManager
    {
        Dictionary<string, LevelUpItem> levelUpItems = new Dictionary<string, LevelUpItem>();

        internal void Init()
        {
            // this class is event listner
            Singleton<ElementalManager>.getInstance().Action_ElementalCreateEvent += this.onElementalCreate;
            Singleton<HeroManager>.getInstance().Action_HeroCreateEvent += onHeroCreate;
        }
        internal void Update()
        {
            foreach (LevelUpItem item in levelUpItems.Values)
                item.Update();
        }
        // recieve event
        void onHeroCreate(Hero sender) // create and bind heroitem
        {
            if (Singleton<TableLoader>.getInstance().leveluptable.ContainsKey(sender.ID) == false)
                return;

            HeroLevelUpItem levelupItem = new HeroLevelUpItem(Singleton<TableLoader>.getInstance().leveluptable[sender.ID]);
            sender.Action_HeroChange += levelupItem.onHeroChange;
            levelUpItems.Add(sender.ID, levelupItem);
        }
        void onElementalCreate(Elemental sender) // create and bind element item
        {
            if (Singleton<TableLoader>.getInstance().leveluptable.ContainsKey(sender.ID) == false)
                return;

            ElementalLevelUpItem levelupItem = new ElementalLevelUpItem(Singleton<TableLoader>.getInstance().leveluptable[sender.ID]);
            sender.Action_ElementalChange += levelupItem.onElementalChange;
            levelUpItems.Add(sender.ID, levelupItem);
        }
        ///  view로부터의 이벤트 . 해당 메서드 런타임에서 절대 data를 변경해선 안된다. ( 데이터는 update에서 변경되어야 한다.)
    }
}