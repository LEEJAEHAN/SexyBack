using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class TalentData
    {
        internal string id;
        internal GridItemIcon icon;
        internal string description;
        internal Bonus bonus;
        internal TalentType type;
        internal int rate;

        public TalentData(string id, GridItemIcon icon, string description, Bonus bonus, TalentType type, int rate)
        {
            this.id = id;
            this.icon = icon;
            this.description = description;
            this.bonus = bonus;
            this.type = type;
            this.rate = rate;
        }
    }
}