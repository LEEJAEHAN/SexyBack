using System.Collections.Generic;

namespace SexyBackPlayScene
{
    public class ResearchData
    {
        public string ID; // 리서치아이디

        public BigIntExpression price;
        public BigIntExpression pot; // price over time
        public int time;

        public List<Bonus> bonuses = new List<Bonus>();

        public string requireID;
        public int requeireLevel;

        public string IconName;
        public string InfoName;
        public string InfoDescription;

        public ResearchData()
        {
        }

        public ResearchData(string ID, string requireID, int requeireLevel, Bonus bonuse, BigIntExpression price, BigIntExpression pot,
            int time, string IconName, string InfoName, string InfoDescription)
        {
            this.ID = ID;
            this.requireID = requireID;
            this.requeireLevel = requeireLevel;
            this.bonuses.Add(bonuse);
            this.price = price;
            this.pot = pot;
            this.time = time;
            this.IconName = IconName;
            this.InfoName = InfoName;
            this.InfoDescription = InfoDescription;
        }
    }

    public class Bonus
    {
        public string targetID;  // both hero and elemental
        public string attribute;
        public int value;
        public string strvalue;

        public Bonus(string targetID, string attribute, int value, string strvalue)
        {
            this.targetID = targetID;
            this.attribute = attribute;
            this.value = value;
            this.strvalue = strvalue;
        }
    }
    //DpsXPer5LV,
    //DpsX,
    //attackspeedXH,

}