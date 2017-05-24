using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class TalentData
    {
        internal string id;
        internal GridItemIcon icon;
        internal string description;
        internal BonusStat bonus;
        internal int rate;
        internal bool absrate;

        public TalentData(string id, GridItemIcon icon, string description, BonusStat bonus, int rate, bool absrate)
        {
            this.id = id;
            this.icon = icon;
            this.description = description;
            this.bonus = bonus;
            this.rate = rate;
            this.absrate = absrate;
        }
    }
}