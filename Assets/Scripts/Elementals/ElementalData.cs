﻿using System;

namespace SexyBackPlayScene
{
    public class ElementalData // 엘레멘탈의 콘스턴트 데이타 ( 변하지않는 )
    {
        public double CreateActionTime;

        public const float GrowthRate = 1.17f;                                                                         // from Excel
        public BigInteger BaseExp; // 베이스exp로부터 level과 GrowthRate를 통해 ExpForNthLevel을 구함.                          // from Excel
        public int ExpForBaseUnitDps; // 베이스기준 dps 1올리기위해 들어가는 exp ( 높을수록 가성비가 구리다는 것 )      // from Excel
        public BigInteger BaseDps; // baseExp / expForBaseUnitDps(exp per dps) = dps; 레벨이 올라가면 항상베이스dps 만큼 올라간다// 계산되는값.
        public int AttackIntervalK; //  elemental 마다 고유하다.                                                     // from excel

        public string ProjectilePrefabName;
        public string ProjectileReadyStateName;
        public string ShooterName;

        public ElementalData(string elementname = "fireball", int attackIntervalK = 3000,  BigInteger baseexp = null, int expperdps = 100 )
        {
            BaseExp = baseexp;
            ExpForBaseUnitDps = expperdps;
            AttackIntervalK = attackIntervalK;
            BaseDps = BaseExp / ExpForBaseUnitDps;

            ShooterName = "shooter_" + elementname;
            ProjectilePrefabName = "prefabs/" + elementname;
            ProjectileReadyStateName = elementname + "_spot";
        }

    }
}