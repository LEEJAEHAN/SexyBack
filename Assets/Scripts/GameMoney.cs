using System;

namespace SexyBackPlayScene
{
    internal class GameMoney
    {
        private BigInteger exp = new BigInteger(0);
        private int gold;
        public int gem; // 유료캐시

        public delegate void ExpChange_Event(BigInteger exp);
        public event ExpChange_Event noticeEXPChange;

        public void Init()
        {

        }
        public void Start()
        {

        }
        public void Update()
        {

        }
        // get set private

        public void ExpGain(BigInteger e)
        {
            exp += e;
            noticeEXPChange(exp);
        }
        internal void ExpUse(BigInteger e)
        {
            exp -= e;
            noticeEXPChange(exp);
        }

        internal bool CanBuy(BigInteger price)
        {
            return exp > price;
        }
    }
}