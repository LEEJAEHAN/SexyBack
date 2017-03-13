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
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z
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
