using System.Collections.Generic;

namespace SexyBackPlayScene
{
    public class ResearchData
    {
        public string ID; // 리서치아이디

        public BigIntExpression price;
        public BigIntExpression pot; // price over time
        public int time;

        public List<Bonus> bonus = new List<Bonus>();

        public string requireID;
        public int requeireLevel;

        public string IconName;
        public string InfoName;

        public ResearchData()
        {
        }
    }

    public class Bonus
    {
        public string targetID;  // both hero and elemental
        public string attribute;
        public int value;
    }
    //DpsXPer5LV,
    //DpsX,
    //attackspeedXH,

}