﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SexyBackPlayScene
{
    internal class ResearchFactory
    {
        ~ResearchFactory()
        {
            sexybacklog.Console("ResearchFactory 소멸");
        }

        internal Research SummonNewResearch(ResearchData data)
        {
            BigInteger totalPrice = CalPrice(data.Level, data.baselevel, data.baseprice);
            double temptime = CalTime(data.Level + data.baselevel, data.baseprice, data.rate, data.basetime);
            double researchTick;

            // avatar생성
            if (temptime > 0 && totalPrice / (int)temptime < 100)
                researchTick = 1f;
            else
                researchTick = 0.1f;

            Research newone = new Research(data, temptime, totalPrice, researchTick); //,StartPrice, ResearchPrice, PricePerSec, ResearchTime
            newone.StateMachine.ChangeState("Ready");
            newone.SetStat();

            return newone;
        }
        private BigInteger CalPrice(int level, int baselevel, int baseprice)
        {
            double BasePriceDensity = InstanceStatus.GetTotalDensityPerLevel( baselevel + level );
            double growth = InstanceStatus.CalInstanceGrowth(baselevel + level);
            double doubleC = baseprice * BasePriceDensity * growth;
            BigInteger TotalPrice = BigInteger.FromDouble(doubleC);
            return TotalPrice;
        }
        private double CalTime(int reallevel, int baseprice, int rate, int basetime)
        { // TODO : 중요한공식
            double growth = ResearchManager.CalTimeGrowth(reallevel - 40); // 40층 부터 기준, 그전까진 지수적감소 max값은 2의13승.
            double time = growth * basetime; // growth는 2의 3승까지는 업그레이드로 감소시킬수 있으므로, 8 * ( 30 + bonus ). 240~480초
            return time;
        }
    }
}
