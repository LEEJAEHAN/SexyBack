using System;

namespace SexyBackPlayScene
{
    internal class LevelUpProcess
    {
        private bool isBuy;
        private LevelUpItem levelUpItem;


        public LevelUpProcess(LevelUpItem levelUpItem, bool isBuy)
        {
            this.levelUpItem = levelUpItem;
            this.isBuy = isBuy;
        }

        internal void LevelUp()
        {
            if(isBuy)
                levelUpItem.Buy();     // levelup 만 함;
        }
    }
}