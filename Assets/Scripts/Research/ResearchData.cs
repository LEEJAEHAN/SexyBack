using System.Collections.Generic;

namespace SexyBackPlayScene
{
    public class ResearchData
    {
        public string ID; // 리서치아이디

        public BigIntExpression price;
        public BigIntExpression pot; // price over time
        public int time;

        public string requireElemetalID;
        public int requeireLevel;
        public string preResearchID;

        public List<StatBonus> bonus = new List<StatBonus>();

        public string IconName;
        public string InfoName;

        public ResearchData()
        {
        }
    }
}