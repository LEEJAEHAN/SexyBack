using System;

namespace SexyBackPlayScene
{
    internal class GameMoney
    {
        private BigInteger exp = new BigInteger(0);
        //private int gold;
        //public int gem; // 유료캐시

        public delegate void ExpChange_Event(BigInteger exp);
        public event ExpChange_Event noticeEXPChange;

        UILabel label_exp = ViewLoader.label_exp.GetComponent<UILabel>();

        public void Init()
        {
            noticeEXPChange += PrintExp;
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
        internal bool ExpUse(BigInteger e)
        {
            bool result;

            if (exp - e < 0)
                result = false;
            else
            {
                exp -= e;
                result = true;
            }

            noticeEXPChange(exp);
            return result;
        }

        void PrintExp(BigInteger exp)
        {
            string expstring = exp.To5String() + " EXP";
            label_exp.text = expstring;
        }
    }
}