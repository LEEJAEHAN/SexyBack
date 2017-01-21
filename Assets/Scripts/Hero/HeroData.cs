using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal class HeroData
    {
        public string ID = "hero";
        public string Name = "이재한";

        public double ATTACKINTERVAL = 5;
        public double CRIRATE = 0.15;
        public int CRIDAMAGE = 200;
        public int MOVESPEED = 1;
        public int ATTACKCOUNT = 1;

        public int MaxLevel;
        public BigIntExpression[] BaseDpcPool;
        public BigIntExpression[] BaseExpPool;


        public HeroData()
        {
            MaxLevel = 10;

            BaseDpcPool = new BigIntExpression[MaxLevel+1];
            BaseExpPool = new BigIntExpression[MaxLevel+1];

            BaseDpcPool[0] = new BigIntExpression(1, "zero"); // 1레벨이 될때 올라가는 데미지
            BaseDpcPool[1] = new BigIntExpression(10, "zero"); // 다음레벨이될때 올라가는 데미지
            BaseDpcPool[2] = new BigIntExpression(20, "zero");
            BaseDpcPool[3] = new BigIntExpression(40, "zero");
            BaseDpcPool[4] = new BigIntExpression(80, "zero");
            BaseDpcPool[5] = new BigIntExpression(160, "zero");
            BaseDpcPool[6] = new BigIntExpression(320, "zero");
            BaseDpcPool[7] = new BigIntExpression(640, "zero");
            BaseDpcPool[8] = new BigIntExpression(1280, "zero");
            BaseDpcPool[9] = new BigIntExpression(2560, "zero");
            BaseDpcPool[10] = new BigIntExpression(5120, "zero");

            BaseExpPool[0] = new BigIntExpression(0, "zero");
            BaseExpPool[1] = new BigIntExpression(10, "zero"); // 1에서 2되기 위한 필요량
            BaseExpPool[2] = new BigIntExpression(20, "m");
            BaseExpPool[3] = new BigIntExpression(40, "m");
            BaseExpPool[4] = new BigIntExpression(80, "m");
            BaseExpPool[5] = new BigIntExpression(160, "b");
            BaseExpPool[6] = new BigIntExpression(320, "b");
            BaseExpPool[7] = new BigIntExpression(640, "b");
            BaseExpPool[8] = new BigIntExpression(1280, "t");
            BaseExpPool[9] = new BigIntExpression(2560, "t");
            BaseExpPool[10] = new BigIntExpression(5120, "t");

        }
    }
}