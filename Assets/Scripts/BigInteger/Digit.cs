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
    public class BigIntExpression
    {
        public int value;
        public Digit digit;

        public BigIntExpression(int value, string d)
        {
            this.value = value;
            this.digit = (Digit)Enum.Parse(typeof(Digit), d);
        }
        public string ToSexyBackString()
        {
            if (digit == Digit.zero)
                return value.ToString();
            else
                return value.ToString() + digit.ToString();
        }



    }
}
