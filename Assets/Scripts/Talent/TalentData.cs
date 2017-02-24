using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class TalentData
    {
        internal string id;
        internal GridItemIcon icon;
        internal string description;
        internal List<Bonus> bonuses;
        internal TalentType type;
        internal int rate;

        public TalentData(string id, GridItemIcon icon, string description, List<Bonus> bonuses, TalentType type, int rate)
        {
            this.id = id;
            this.icon = icon;
            this.description = description;
            this.bonuses = bonuses;
            this.type = type;
            this.rate = rate;
        }
    }
}