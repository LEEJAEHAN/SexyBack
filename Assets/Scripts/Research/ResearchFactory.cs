using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SexyBackPlayScene
{
    internal class ResearchFactory
    {
        internal Research CreateNewResearch(ResearchData data, ICanLevelUp root)
        {
            BigInteger totalPrice = CalPrice(data.level, data.baselevel, data.baseprice);
            double temptime = CalTime(data.level + data.baselevel, data.baseprice, data.rate, data.basetime);
            // avatar생성

            GridItem griditem = new GridItem("Research", data.ID, data.IconName, ViewLoader.Tab3Container);
            griditem.SetActive(false);

            Research newone = new Research(data, root, griditem, temptime, totalPrice); //,StartPrice, ResearchPrice, PricePerSec, ResearchTime

            newone.StateMachine.ChangeState("None");
            newone.SetStat(Singleton<Player>.getInstance().GetResearchStat);

            return newone;
        }
        private BigInteger CalPrice(int level, int baselevel, int baseprice)
        {
            int reallevel = level + baselevel;
            int baseDamageRateXK = 1000 + 5 * baselevel; // 원래 double값은 1 + baselevel/20;
            BigInteger TotalPrice = BigInteger.PowerByGrowth(baseDamageRateXK * baseprice, reallevel, ResearchData.GrowthRate) / 1000;
            return TotalPrice;
        }
        private double CalTime(int reallevel, int baseprice, int rate, int basetime)
        { // TODO : 중요한공식
            double growth = Math.Min(Math.Pow(ResearchData.TimeGrothRate, reallevel - 40), Math.Pow(2, 13)); // 40층 부터 기준, 그전까진 지수적감소 max값은 2의13승.
            double bonus = (double)reallevel / 30;  // rearlevel * 30(기준연구시간 / 900 , bonus는 0~30초까지.
            double time = growth * (30 + bonus); // growth는 2의 3승까지는 업그레이드로 감소시킬수 있으므로, 8 * ( 30 + bonus ). 240~480초
            return time;
        }
    }
}
