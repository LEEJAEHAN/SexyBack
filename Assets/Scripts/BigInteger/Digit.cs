using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SexyBackPlayScene
{
    public enum Digit
    {
        zero = 0,
        k = 1,
        m = 2,
        b = 3,
        t = 4,
        q = 5,
        A,
        B,
        C,
        D,
        E
    }
    public struct BigIntExpression
    {
        public int value;
        public string digit;

        public BigIntExpression(int value, string digit)
        {
            this.value = value;
            this.digit = digit;
        }
        public string ToSexyBackString()
        {
            if (digit == Digit.zero.ToString())
                return value.ToString() + " ";
            else
                return value.ToString() + " "+ digit;
        }
    }
}
