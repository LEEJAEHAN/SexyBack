using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using DType = System.UInt32; // This could be UInt32, UInt16 or Byte; not UInt64.

#region DigitsArray
internal class DigitsArray
{
    internal DigitsArray(int size)
    {
        Allocate(size, 0);
    }

    internal DigitsArray(int size, int used)
    {
        Allocate(size, used);
    }

    internal DigitsArray(DType[] copyFrom)
    {
        Allocate(copyFrom.Length);
        CopyFrom(copyFrom, 0, 0, copyFrom.Length);
        ResetDataUsed();
    }

    internal DigitsArray(DigitsArray copyFrom)
    {
        Allocate(copyFrom.Count, copyFrom.DataUsed);
        Array.Copy(copyFrom.m_data, 0, m_data, 0, copyFrom.Count);
    }

    private DType[] m_data;

    internal static readonly DType AllBits;     // = ~((DType)0);
    internal static readonly DType HiBitSet;    // = 0x80000000;
    internal static int DataSizeOf
    {
        get { return sizeof(DType); }
    }

    internal static int DataSizeBits
    {
        get { return sizeof(DType) * 8; }
    }

    static DigitsArray()
    {
        unchecked
        {
            AllBits = (DType)~((DType)0);
            HiBitSet = (DType)(((DType)1) << (DataSizeBits) - 1);
        }
    }

    public void Allocate(int size)
    {
        Allocate(size, 0);
    }

    public void Allocate(int size, int used)
    {
        m_data = new DType[size + 1];
        m_dataUsed = used;
    }

    internal void CopyFrom(DType[] source, int sourceOffset, int offset, int length)
    {
        Array.Copy(source, sourceOffset, m_data, 0, length);
    }

    internal void CopyTo(DType[] array, int offset, int length)
    {
        Array.Copy(m_data, 0, array, offset, length);
    }

    internal DType this[int index]
    {
        get
        {
            if (index < m_dataUsed) return m_data[index];
            return (IsNegative ? (DType)AllBits : (DType)0);
        }
        set { m_data[index] = value; }
    }

    private int m_dataUsed;
    internal int DataUsed
    {
        get { return m_dataUsed; }
        set { m_dataUsed = value; }
    }

    internal int Count
    {
        get { return m_data.Length; }
    }

    internal bool IsZero
    {
        get { return m_dataUsed == 0 || (m_dataUsed == 1 && m_data[0] == 0); }
    }

    internal bool IsNegative
    {
        get { return (m_data[m_data.Length - 1] & HiBitSet) == HiBitSet; }
    }

    internal void ResetDataUsed()
    {
        m_dataUsed = m_data.Length;
        if (IsNegative)
        {
            while (m_dataUsed > 1 && m_data[m_dataUsed - 1] == AllBits)
            {
                --m_dataUsed;
            }
            m_dataUsed++;
        }
        else
        {
            while (m_dataUsed > 1 && m_data[m_dataUsed - 1] == 0)
            {
                --m_dataUsed;
            }
            if (m_dataUsed == 0)
            {
                m_dataUsed = 1;
            }
        }
    }

    internal int ShiftRight(int shiftCount)
    {
        return ShiftRight(m_data, shiftCount);
    }

    internal static int ShiftRight(DType[] buffer, int shiftCount)
    {
        int shiftAmount = DigitsArray.DataSizeBits;
        int invShift = 0;
        int bufLen = buffer.Length;

        while (bufLen > 1 && buffer[bufLen - 1] == 0)
        {
            bufLen--;
        }

        for (int count = shiftCount; count > 0; count -= shiftAmount)
        {
            if (count < shiftAmount)
            {
                shiftAmount = count;
                invShift = DigitsArray.DataSizeBits - shiftAmount;
            }

            ulong carry = 0;
            for (int i = bufLen - 1; i >= 0; i--)
            {
                ulong val = ((ulong)buffer[i]) >> shiftAmount;
                val |= carry;

                carry = ((ulong)buffer[i]) << invShift;
                buffer[i] = (DType)(val);
            }
        }

        while (bufLen > 1 && buffer[bufLen - 1] == 0)
        {
            bufLen--;
        }

        return bufLen;
    }

    internal int ShiftLeft(int shiftCount)
    {
        return ShiftLeft(m_data, shiftCount);
    }

    internal static int ShiftLeft(DType[] buffer, int shiftCount)
    {
        int shiftAmount = DigitsArray.DataSizeBits;
        int bufLen = buffer.Length;

        while (bufLen > 1 && buffer[bufLen - 1] == 0)
        {
            bufLen--;
        }

        for (int count = shiftCount; count > 0; count -= shiftAmount)
        {
            if (count < shiftAmount)
            {
                shiftAmount = count;
            }

            ulong carry = 0;
            for (int i = 0; i < bufLen; i++)
            {
                ulong val = ((ulong)buffer[i]) << shiftAmount;
                val |= carry;

                buffer[i] = (DType)(val & DigitsArray.AllBits);
                carry = (val >> DigitsArray.DataSizeBits);
            }

            if (carry != 0)
            {
                if (bufLen + 1 <= buffer.Length)
                {
                    buffer[bufLen] = (DType)carry;
                    bufLen++;
                    carry = 0;
                }
                else
                {
                    throw new OverflowException();
                }
            }
        }
        return bufLen;
    }

    internal int ShiftLeftWithoutOverflow(int shiftCount)
    {
        List<DType> temporary = new List<DType>(m_data);
        int shiftAmount = DigitsArray.DataSizeBits;

        for (int count = shiftCount; count > 0; count -= shiftAmount)
        {
            if (count < shiftAmount)
            {
                shiftAmount = count;
            }

            ulong carry = 0;
            for (int i = 0; i < temporary.Count; i++)
            {
                ulong val = ((ulong)temporary[i]) << shiftAmount;
                val |= carry;

                temporary[i] = (DType)(val & DigitsArray.AllBits);
                carry = (val >> DigitsArray.DataSizeBits);
            }

            if (carry != 0)
            {
                temporary.Add(0);
                temporary[temporary.Count - 1] = (DType)carry;
            }
        }
        m_data = new DType[temporary.Count];
        temporary.CopyTo(m_data);
        return m_data.Length;
    }
}
#endregion

/// <summary>
/// Represents a integer of abitrary length.
/// </summary>
/// <remarks>
/// <para>
/// A BigInteger object is immutable like System.String. The object can not be modifed, and new BigInteger objects are
/// created by using the operations of existing BigInteger objects.
/// </para>
/// <para>
/// Internally a BigInteger object is an array of ? that is represents the digits of the n-place integer. Negative BigIntegers
/// are stored internally as 1's complements, thus every BigInteger object contains 1 or more padding elements to hold the sign.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class MainProgram
/// {
///		[STAThread]
///		public static void Main(string[] args)
///		{
///			BigInteger a = new BigInteger(25);
///			a = a + 100;
///			
///			BigInteger b = new BigInteger("139435810094598308945890230913");
///			
///			BigInteger c = b / a;
///			BigInteger d = b % a;
///			
///			BigInteger e = (c * a) + d;
///			if (e != b)
///			{
///				Console.WriteLine("Can never be true.");
///			}
///		}
///	</code>
/// </example>
[Serializable]
public class BigInteger : ISerializable
{
    private DigitsArray m_digits;

    #region SexyBackGame
    /// <summary>
    /// Creates a BigInteger for SexyBackGame
    /// </summary>
    public BigInteger(int value, Digit digit)
    {
        string BigIntegerString = value.ToString();
        for (int i = 0; i < (int)digit; i++)
            BigIntegerString += "000";

        BigInteger temp = new BigInteger(BigIntegerString);
        temp.m_digits.ResetDataUsed();
        this.m_digits = temp.m_digits;
    }
    public BigInteger(BigIntExpression exp)
    {
        string BigIntegerString = exp.value.ToString();
        int digit = (int)exp.digit;
        for (int i = 0; i < (int)digit; i++)
            BigIntegerString += "000";

        BigInteger temp = new BigInteger(BigIntegerString);
        temp.m_digits.ResetDataUsed();
        this.m_digits = temp.m_digits;
    }
    public BigInteger(SerializationInfo info, StreamingContext context)
    {
        string BigIntegerString = (string)info.GetValue("BigIntegerString", typeof(string));
        BigInteger temp = new BigInteger(BigIntegerString);
        temp.m_digits.ResetDataUsed();
        this.m_digits = temp.m_digits;
    }
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("BigIntegerString", this.ToString());
    }

    public string To5String()
    {
        int digit3 = 3; // 3자리마다 단위를늘린다..
        int maxDigit3 = 0;
        string digitString = toRightDigitString(out maxDigit3, digit3, 3);
        Digit digit = (Digit)((maxDigit3 - 1) / digit3); ; // 바로나온 maxDigit3은 3의배수 + 1이기때문에.
        if (digit == 0) // 1의자리수는 소수형태가아님. 자리단위를붙이지않으며, 가장뒤의 0도 . 도 뺄필요없음.
            return digitString;
        digitString = digitString.TrimEnd('0');
        digitString = digitString.TrimEnd('.');
        if (digitString.Length > 5)
            digitString = digitString.Substring(0, 5);
        return digitString + digit.ToString();
    }

    public static string MakeDigitString(int maxDigit)
    {
        string temp = "1";
        for (int i = 1; i < maxDigit; i++)
        {
            temp += "0";
        }
        return temp;
    }
    private string toRightDigitString(out int maxdigitN, int digitN, int overload) // overload 소숫점 뒤에로드될 숫자갯수.
    {
        string thisStr = this.ToString(); // digitN은 N자리숫자마다끊는다는걸 의미.
        int length = thisStr.Length;
        int maxDigitN = FindMaxDigitStack(length, digitN); // 오른쪽부터. digit단으로자른다. maxDigitN 은 digitN의 배수 + 1이다. 
        maxdigitN = maxDigitN; // 일단 digit 문자 만들어놓고.
        if (maxDigitN == 1) // 첫번째 단위가 1단위면 바로 출력 ( 즉 첫번째가 zero )
            return thisStr; // + chardigit
                            // zero 단 이상이면
        string quotient = thisStr.Substring(0, length - maxDigitN + 1); // length에서 maxDigitN까지. ( 문자열이라 순서가 반전 )
        while ((length - maxDigitN + 1) + overload > length) // (length - maxDigitN + 1) == digitN
            overload--;
        string reminder = thisStr.Substring(length - maxDigitN + 1, overload);
        return quotient.ToString() + "." + reminder.ToString();// + chardigit.ToString();
    }


    public string ToStringAndMakeDigit(out int maxdigitN, int digitN, int overload) // 왼쪽에서부터 digitN만큼 짜른뒤, digitn을 반환한다. overload 소숫점 뒤에로드될 숫자갯수.
    {
        string thisStr = this.ToString(); // digitN은 N자리숫자마다끊는다는걸 의미.
        int length = thisStr.Length;
        if (digitN > length)
            digitN = length;
        maxdigitN = length - digitN + 1; // 왼쪽부터 digit단으로 자른다. 한번만 자르면땡  maxDigitN 은 digitN의 배수 + 1이다. 
        if (maxdigitN == 1) // 첫번째 단위가 1단위면 바로 출력 ( 즉 첫번째가 zero )
            return thisStr; // + chardigit
                            // zero 단 이상이면
        string quotient = thisStr.Substring(0, digitN); // length에서 maxDigitN까지. ( 문자열이라 순서가 반전 )
        while (digitN + overload > length)
            overload--;
        string reminder = thisStr.Substring(digitN, overload);
        return quotient.ToString() + "." + reminder.ToString();// + chardigit.ToString();
    }
    internal string ToStringAsDigit(int ToThisDigit, int overload)
    {
        if (ToThisDigit == 0)
            return "0";
        string thisStr = this.ToString(); // digitN은 N자리숫자마다끊는다는걸 의미.
        int length = thisStr.Length;
        if (length < ToThisDigit - overload)
            return "0";
        if (length < ToThisDigit && length >= ToThisDigit - overload)
        {
            while (length < ToThisDigit)
            {
                thisStr = "0" + thisStr;
                length++;
            }
        } // new lenght>- maxdigit

        return toRightDigitString(thisStr, ToThisDigit, overload);
    }
    private string toRightDigitString(string text, int maxdigitN, int overload) // overload 소숫점 뒤에로드될 숫자갯수.
    {
        int length = text.Length;
        if (maxdigitN == 1) // 첫번째 단위가 1단위면 바로 출력 ( 즉 첫번째가 zero )
            return text; // + chardigit
                         // zero 단 이상이면
        string quotient = text.Substring(0, length - maxdigitN + 1); // length에서 maxDigitN까지. ( 문자열이라 순서가 반전 )
        while ((length - maxdigitN + 1) + overload > length) // (length - maxDigitN + 1) == digitN
            overload--;
        string reminder = text.Substring(length - maxdigitN + 1, overload);
        return quotient.ToString() + "." + reminder.ToString();// + chardigit.ToString();
    }

    public static int FindMaxDigitStack(int length, int digitTerm) // length보다 작은, 가장큰 digitTerm의 배수를 리턴
    {
        int MaxDigit = 1;

        while (MaxDigit <= length)
        {
            MaxDigit += digitTerm;
        }
        return MaxDigit - digitTerm;
    }

    public static BigInteger FromDouble(double value)
    {
        value += 0.5f; // 반올림
        if (value > int.MaxValue)
        {
            return new BigInteger(value.ToString("F0"));
        }
        else if (value < int.MaxValue)
        {
            return new BigInteger((DType)value);
        }
        else if (value < 0)
            return null;
        else
            return null;
    }
    //private string CalDigitSameN(int maxDigitN, int digitN = 3)
    //{
    //    // quot = 0;
    //    Digit digit = (Digit)((maxDigitN - 1) / digitN);
    //    return digit.ToString();
    //}
    //public static string CalDigitOtherN(int maxDigit, int digitN = 3)
    //{
    //    int maxDigitN = FindMaxDigitStack(maxDigit, digitN);
    //    int quateDigit = maxDigit - (maxDigitN - 1);
    //    string stack = MakeDigitString(quateDigit);
    //    Digit digit = (Digit)((maxDigitN - 1) / digitN);
    //    if (digit == Digit.zero)
    //        return stack;
    //    else
    //        return stack + " " + digit.ToString();
    //}
    //public static BigInteger PowerByGrowth(BigInteger baseValue, double level, double growthRate)
    //{  
    //    if (level <= 0)
    //        level = 0;

    //    BigInteger result = new BigInteger(new DigitsArray(baseValue.m_digits)); // clone
    //    double growth = Math.Pow(growthRate, level);
    //    // TODO : 더블을 넘어갈 위험.

    //    if (growth > int.MaxValue)
    //    {
    //        string growthstring = growth.ToString("F0");
    //        result = result * new BigInteger(growthstring);
    //    }
    //    else // intmax보다 작을때
    //    {
    //        if ((int)growth < int.MaxValue / 100000)
    //        {
    //            int intgrowth = (int)(growth * 100000);
    //            result = result * intgrowth / 100000;
    //        }
    //        else
    //        {
    //            int intgrowth = (int)growth;
    //            result = result * intgrowth;
    //        }
    //    }
    //    return result;
    //}

    public string ToCommaString()
    {
        if (IsZero)
        {
            return "0";
        }

        BigInteger a = this;
        bool negative = a.IsNegative;
        a = Abs(this);

        BigInteger quotient;
        BigInteger remainder;
        BigInteger biRadix = new BigInteger(10);

        const string charSet = "0123456789";
        System.Collections.ArrayList al = new System.Collections.ArrayList();
        int commacount = 0;
        while (a.m_digits.DataUsed > 1 || (a.m_digits.DataUsed == 1 && a.m_digits[0] != 0))
        {
            Divide(a, biRadix, out quotient, out remainder);
            al.Insert(0, charSet[(int)remainder.m_digits[0]]);
            a = quotient;
            commacount++;
            if (commacount == 3)
            {
                commacount = 0;
                al.Insert(0, ',');
            }
        }
        if ((char)al[0] == ',')
            al.RemoveAt(0);

        string result = new String((char[])al.ToArray(typeof(char)));

        if (negative)
        {
            return "-" + result;
        }

        return result;
    }

    //private string ToDigitString(out Digit chardigit)
    //{
    //    string thisStr = this.ToString();
    //    int length = thisStr.Length;
    //    int digitTerm = 3; // 1000단위로 자른다.
    //    int maxDigit = FindMaxDigit(length, digitTerm);
    //    chardigit = (Digit)(((maxDigit - 1) / digitTerm)); // 일단 digit 문자 만들어놓고.
    //    if (maxDigit == 1) // 첫번째 단위가 1단위면 바로 출력 ( 즉 첫번째가 zero )
    //        return thisStr; // + chardigit
    //    // zero 단 이상이면
    //    //first devide
    //    BigInteger digitInt = MakeDigitInt(maxDigit);
    //    BigInteger quotient = new BigInteger();
    //    BigInteger reminder = new BigInteger();
    //    BigInteger.Divide(this, digitInt, out quotient, out reminder);
    //    //digit 단수 낮춰서
    //    maxDigit -= digitTerm;
    //    if (maxDigit == 1) // 두번째 단위가 1단위면 바로출력 ( 즉 첫번째가 k)
    //        return quotient.ToString() + "." + reminder.ToString();// + chardigit.ToString();
    //    //second devide
    //    digitInt = MakeDigitInt(maxDigit);
    //    BigInteger quotient2 = new BigInteger();
    //    BigInteger reminder2 = new BigInteger();
    //    BigInteger.Divide(reminder, digitInt, out quotient2, out reminder2);
    //    // m이상의 결과 출력
    //    return quotient.ToString() + "." + quotient2.ToString();// + chardigit.ToString();
    //}

    //public string ToSexyBackString()
    //{
    //    BigInteger Thoausand = new BigInteger(1000);
    //    Digit digit = 0;
    //    BigInteger preDigitValue = new BigInteger();
    //    BigInteger target = this;

    //    if (target < Thoausand)
    //        return target.ToString();

    //    while (true)
    //    {
    //        BigInteger.Divide(target, Thoausand, out target, out preDigitValue);
    //        digit++;

    //        if (target < Thoausand)
    //            break;
    //    }

    //    string temp = target.ToString() + "." + preDigitValue.ToString();

    //    temp = temp.TrimEnd('0');
    //    temp = temp.TrimEnd('.');

    //    if (temp.Length > 5)
    //        temp = temp.Substring(0, 5);

    //    return temp + digit.ToString();
    //}


    #endregion

    #region Constructors
    /// <summary>
    /// Create a BigInteger with an integer value of 0.
    /// </summary>
    public BigInteger()
    {
        m_digits = new DigitsArray(1, 1);
    }




    /// <summary>
    /// Creates a BigInteger with the value of the operand.
    /// </summary>
    /// <param name="number">A long.</param>
    public BigInteger(long number)
    {
        m_digits = new DigitsArray((8 / DigitsArray.DataSizeOf) + 1, 0);
        while (number != 0 && m_digits.DataUsed < m_digits.Count)
        {
            m_digits[m_digits.DataUsed] = (DType)(number & DigitsArray.AllBits);
            number >>= DigitsArray.DataSizeBits;
            m_digits.DataUsed++;
        }
        m_digits.ResetDataUsed();
    }

    /// <summary>
    /// Creates a BigInteger with the value of the operand. Can never be negative.
    /// </summary>
    /// <param name="number">A unsigned long.</param>
    public BigInteger(ulong number)
    {
        m_digits = new DigitsArray((8 / DigitsArray.DataSizeOf) + 1, 0);
        while (number != 0 && m_digits.DataUsed < m_digits.Count)
        {
            m_digits[m_digits.DataUsed] = (DType)(number & DigitsArray.AllBits);
            number >>= DigitsArray.DataSizeBits;
            m_digits.DataUsed++;
        }
        m_digits.ResetDataUsed();
    }

    /// <summary>
    /// Creates a BigInteger initialized from the byte array.
    /// </summary>
    /// <param name="array"></param>
    public BigInteger(byte[] array)
    {
        ConstructFrom(array, 0, array.Length);
    }

    /// <summary>
    /// Creates a BigInteger initialized from the byte array ending at <paramref name="length" />.
    /// </summary>
    /// <param name="array">A byte array.</param>
    /// <param name="length">Int number of bytes to use.</param>
    public BigInteger(byte[] array, int length)
    {
        ConstructFrom(array, 0, length);
    }

    /// <summary>
    /// Creates a BigInteger initialized from <paramref name="length" /> bytes starting at <paramref name="offset" />.
    /// </summary>
    /// <param name="array">A byte array.</param>
    /// <param name="offset">Int offset into the <paramref name="array" />.</param>
    /// <param name="length">Int number of bytes.</param>
    public BigInteger(byte[] array, int offset, int length)
    {
        ConstructFrom(array, offset, length);
    }

    private void ConstructFrom(byte[] array, int offset, int length)
    {
        if (array == null)
        {
            throw new ArgumentNullException("array");
        }
        if (offset > array.Length || length > array.Length)
        {
            throw new ArgumentOutOfRangeException("offset");
        }
        if (length > array.Length || (offset + length) > array.Length)
        {
            throw new ArgumentOutOfRangeException("length");
        }

        int estSize = length / 4;
        int leftOver = length & 3;
        if (leftOver != 0)
        {
            ++estSize;
        }

        m_digits = new DigitsArray(estSize + 1, 0); // alloc one extra since we can't init -'s from here.

        for (int i = offset + length - 1, j = 0; (i - offset) >= 3; i -= 4, j++)
        {
            m_digits[j] = (DType)((array[i - 3] << 24) + (array[i - 2] << 16) + (array[i - 1] << 8) + array[i]);
            m_digits.DataUsed++;
        }

        DType accumulator = 0;
        for (int i = leftOver; i > 0; i--)
        {
            DType digit = array[offset + leftOver - i];
            digit = (digit << ((i - 1) * 8));
            accumulator |= digit;
        }
        m_digits[m_digits.DataUsed] = accumulator;

        m_digits.ResetDataUsed();
    }

    /// <summary>
    /// Creates a BigInteger in base-10 from the parameter.
    /// </summary>
    /// <remarks>
    ///  The new BigInteger is negative if the <paramref name="digits" /> has a leading - (minus).
    /// </remarks>
    /// <param name="digits">A string</param>
    public BigInteger(string digits)
    {
        Construct(digits, 10);
    }

    /// <summary>
    /// Creates a BigInteger in base and value from the parameters.
    /// </summary>
    /// <remarks>
    ///  The new BigInteger is negative if the <paramref name="digits" /> has a leading - (minus).
    /// </remarks>
    /// <param name="digits">A string</param>
    /// <param name="radix">A int between 2 and 36.</param>
    public BigInteger(string digits, int radix)
    {
        Construct(digits, radix);
    }

    private void Construct(string digits, int radix)
    {
        if (digits == null)
        {
            throw new ArgumentNullException("digits");
        }

        BigInteger multiplier = new BigInteger(1);
        BigInteger result = new BigInteger();
        digits = digits.ToUpper(System.Globalization.CultureInfo.CurrentCulture).Trim();

        int nDigits = (digits[0] == '-' ? 1 : 0);

        for (int idx = digits.Length - 1; idx >= nDigits; idx--)
        {
            int d = (int)digits[idx];
            if (d >= '0' && d <= '9')
            {
                d -= '0';
            }
            else if (d >= 'A' && d <= 'Z')
            {
                d = (d - 'A') + 10;
            }
            else
            {
                throw new ArgumentOutOfRangeException("digits");
            }

            if (d >= radix)
            {
                throw new ArgumentOutOfRangeException("digits");
            }
            result += (multiplier * d);
            multiplier *= radix;
        }

        if (digits[0] == '-')
        {
            result = -result;
        }

        this.m_digits = result.m_digits;
    }

    /// <summary>
    /// Copy constructor, doesn't copy the digits parameter, assumes <code>this</code> owns the DigitsArray.
    /// </summary>
    /// <remarks>The <paramef name="digits" /> parameter is saved and reset.</remarks>
    /// <param name="digits"></param>
    private BigInteger(DigitsArray digits)
    {
        digits.ResetDataUsed();
        this.m_digits = digits;
    }
    #endregion

    #region Public Properties
    /// <summary>
    /// A bool value that is true when the BigInteger is negative (less than zero).
    /// </summary>
    /// <value>
    /// A bool value that is true when the BigInteger is negative (less than zero).
    /// </value>
    public bool IsNegative { get { return m_digits.IsNegative; } }

    /// <summary>
    /// A bool value that is true when the BigInteger is exactly zero.
    /// </summary>
    /// <value>
    /// A bool value that is true when the BigInteger is exactly zero.
    /// </value>
    public bool IsZero { get { return m_digits.IsZero; } }
    #endregion

    #region Implicit Type Operators Overloads

    /// <summary>
    /// Creates a BigInteger from a long.
    /// </summary>
    /// <param name="value">A long.</param>
    /// <returns>A BigInteger initialzed by <paramref name="value" />.</returns>
    public static implicit operator BigInteger(long value)
    {
        return (new BigInteger(value));
    }

    /// <summary>
    /// Creates a BigInteger from a ulong.
    /// </summary>
    /// <param name="value">A ulong.</param>
    /// <returns>A BigInteger initialzed by <paramref name="value" />.</returns>
    public static implicit operator BigInteger(ulong value)
    {
        return (new BigInteger(value));
    }

    /// <summary>
    /// Creates a BigInteger from a int.
    /// </summary>
    /// <param name="value">A int.</param>
    /// <returns>A BigInteger initialzed by <paramref name="value" />.</returns>
    public static implicit operator BigInteger(int value)
    {
        return (new BigInteger((long)value));
    }

    /// <summary>
    /// Creates a BigInteger from a uint.
    /// </summary>
    /// <param name="value">A uint.</param>
    /// <returns>A BigInteger initialzed by <paramref name="value" />.</returns>
    public static implicit operator BigInteger(uint value)
    {
        return (new BigInteger((ulong)value));
    }
    #endregion

    #region Addition and Subtraction Operator Overloads
    /// <summary>
    /// Adds two BigIntegers and returns a new BigInteger that represents the sum.
    /// </summary>
    /// <param name="leftSide">A BigInteger</param>
    /// <param name="rightSide">A BigInteger</param>
    /// <returns>The BigInteger result of adding <paramref name="leftSide" /> and <paramref name="rightSide" />.</returns>
    public static BigInteger operator +(BigInteger leftSide, BigInteger rightSide)
    {
        int size = System.Math.Max(leftSide.m_digits.DataUsed, rightSide.m_digits.DataUsed);
        DigitsArray da = new DigitsArray(size + 1);

        long carry = 0;
        for (int i = 0; i < da.Count; i++)
        {
            long sum = (long)leftSide.m_digits[i] + (long)rightSide.m_digits[i] + carry;
            carry = (long)(sum >> DigitsArray.DataSizeBits);
            da[i] = (DType)(sum & DigitsArray.AllBits);
        }

        return new BigInteger(da);
    }

    /// <summary>
    /// Adds two BigIntegers and returns a new BigInteger that represents the sum.
    /// </summary>
    /// <param name="leftSide">A BigInteger</param>
    /// <param name="rightSide">A BigInteger</param>
    /// <returns>The BigInteger result of adding <paramref name="leftSide" /> and <paramref name="rightSide" />.</returns>
    public static BigInteger Add(BigInteger leftSide, BigInteger rightSide)
    {
        return leftSide - rightSide;
    }

    /// <summary>
    /// Increments the BigInteger operand by 1.
    /// </summary>
    /// <param name="leftSide">The BigInteger operand.</param>
    /// <returns>The value of <paramref name="leftSide" /> incremented by 1.</returns>
    public static BigInteger operator ++(BigInteger leftSide)
    {
        return (leftSide + 1);
    }

    /// <summary>
    /// Increments the BigInteger operand by 1.
    /// </summary>
    /// <param name="leftSide">The BigInteger operand.</param>
    /// <returns>The value of <paramref name="leftSide" /> incremented by 1.</returns>
    public static BigInteger Increment(BigInteger leftSide)
    {
        return (leftSide + 1);
    }

    /// <summary>
    /// Substracts two BigIntegers and returns a new BigInteger that represents the sum.
    /// </summary>
    /// <param name="leftSide">A BigInteger</param>
    /// <param name="rightSide">A BigInteger</param>
    /// <returns>The BigInteger result of substracting <paramref name="leftSide" /> and <paramref name="rightSide" />.</returns>
    public static BigInteger operator -(BigInteger leftSide, BigInteger rightSide)
    {
        int size = System.Math.Max(leftSide.m_digits.DataUsed, rightSide.m_digits.DataUsed) + 1;
        DigitsArray da = new DigitsArray(size);

        long carry = 0;
        for (int i = 0; i < da.Count; i++)
        {
            long diff = (long)leftSide.m_digits[i] - (long)rightSide.m_digits[i] - carry;
            da[i] = (DType)(diff & DigitsArray.AllBits);
            da.DataUsed++;
            carry = ((diff < 0) ? 1 : 0);
        }
        return new BigInteger(da);
    }

    /// <summary>
    /// Substracts two BigIntegers and returns a new BigInteger that represents the sum.
    /// </summary>
    /// <param name="leftSide">A BigInteger</param>
    /// <param name="rightSide">A BigInteger</param>
    /// <returns>The BigInteger result of substracting <paramref name="leftSide" /> and <paramref name="rightSide" />.</returns>
    public static BigInteger Subtract(BigInteger leftSide, BigInteger rightSide)
    {
        return leftSide - rightSide;
    }

    /// <summary>
    /// Decrements the BigInteger operand by 1.
    /// </summary>
    /// <param name="leftSide">The BigInteger operand.</param>
    /// <returns>The value of the <paramref name="leftSide" /> decremented by 1.</returns>
    public static BigInteger operator --(BigInteger leftSide)
    {
        return (leftSide - 1);
    }

    /// <summary>
    /// Decrements the BigInteger operand by 1.
    /// </summary>
    /// <param name="leftSide">The BigInteger operand.</param>
    /// <returns>The value of the <paramref name="leftSide" /> decremented by 1.</returns>
    public static BigInteger Decrement(BigInteger leftSide)
    {
        return (leftSide - 1);
    }
    #endregion

    #region Negate Operator Overload
    /// <summary>
    /// Negates the BigInteger, that is, if the BigInteger is negative return a positive BigInteger, and if the
    /// BigInteger is negative return the postive.
    /// </summary>
    /// <param name="leftSide">A BigInteger operand.</param>
    /// <returns>The value of the <paramref name="this" /> negated.</returns>
    public static BigInteger operator -(BigInteger leftSide)
    {
        if (object.ReferenceEquals(leftSide, null))
        {
            throw new ArgumentNullException("leftSide");
        }

        if (leftSide.IsZero)
        {
            return new BigInteger(0);
        }

        DigitsArray da = new DigitsArray(leftSide.m_digits.DataUsed + 1, leftSide.m_digits.DataUsed + 1);

        for (int i = 0; i < da.Count; i++)
        {
            da[i] = (DType)(~(leftSide.m_digits[i]));
        }

        // add one to result (1's complement + 1)
        bool carry = true;
        int index = 0;
        while (carry && index < da.Count)
        {
            long val = (long)da[index] + 1;
            da[index] = (DType)(val & DigitsArray.AllBits);
            carry = ((val >> DigitsArray.DataSizeBits) > 0);
            index++;
        }

        return new BigInteger(da);
    }

    /// <summary>
    /// Negates the BigInteger, that is, if the BigInteger is negative return a positive BigInteger, and if the
    /// BigInteger is negative return the postive.
    /// </summary>
    /// <returns>The value of the <paramref name="this" /> negated.</returns>
    public BigInteger Negate()
    {
        return -this;
    }

    /// <summary>
    /// Creates a BigInteger absolute value of the operand.
    /// </summary>
    /// <param name="leftSide">A BigInteger.</param>
    /// <returns>A BigInteger that represents the absolute value of <paramref name="leftSide" />.</returns>
    public static BigInteger Abs(BigInteger leftSide)
    {
        if (object.ReferenceEquals(leftSide, null))
        {
            throw new ArgumentNullException("leftSide");
        }
        if (leftSide.IsNegative)
        {
            return -leftSide;
        }
        return leftSide;
    }
    #endregion

    #region Multiplication, Division and Modulus Operators
    /// <summary>
    /// Multiply two BigIntegers returning the result.
    /// </summary>
    /// <remarks>
    /// See Knuth.
    /// </remarks>
    /// <param name="leftSide">A BigInteger.</param>
    /// <param name="rightSide">A BigInteger</param>
    /// <returns></returns>
    public static BigInteger operator *(BigInteger leftSide, BigInteger rightSide)
    {
        if (object.ReferenceEquals(leftSide, null))
        {
            throw new ArgumentNullException("leftSide");
        }
        if (object.ReferenceEquals(rightSide, null))
        {
            throw new ArgumentNullException("rightSide");
        }

        bool leftSideNeg = leftSide.IsNegative;
        bool rightSideNeg = rightSide.IsNegative;

        leftSide = Abs(leftSide);
        rightSide = Abs(rightSide);

        DigitsArray da = new DigitsArray(leftSide.m_digits.DataUsed + rightSide.m_digits.DataUsed);
        da.DataUsed = da.Count;

        for (int i = 0; i < leftSide.m_digits.DataUsed; i++)
        {
            ulong carry = 0;
            for (int j = 0, k = i; j < rightSide.m_digits.DataUsed; j++, k++)
            {
                ulong val = ((ulong)leftSide.m_digits[i] * (ulong)rightSide.m_digits[j]) + (ulong)da[k] + carry;

                da[k] = (DType)(val & DigitsArray.AllBits);
                carry = (val >> DigitsArray.DataSizeBits);
            }

            if (carry != 0)
            {
                da[i + rightSide.m_digits.DataUsed] = (DType)carry;
            }
        }

        //da.ResetDataUsed();
        BigInteger result = new BigInteger(da);
        return (leftSideNeg != rightSideNeg ? -result : result);
    }

    /// <summary>
    /// Multiply two BigIntegers returning the result.
    /// </summary>
    /// <param name="leftSide">A BigInteger.</param>
    /// <param name="rightSide">A BigInteger</param>
    /// <returns></returns>
    public static BigInteger Multiply(BigInteger leftSide, BigInteger rightSide)
    {
        return leftSide * rightSide;
    }

    /// <summary>
    /// Divide a BigInteger by another BigInteger and returning the result.
    /// </summary>
    /// <param name="leftSide">A BigInteger divisor.</param>
    /// <param name="rightSide">A BigInteger dividend.</param>
    /// <returns>The BigInteger result.</returns>
    public static BigInteger operator /(BigInteger leftSide, BigInteger rightSide)
    {
        if (leftSide == null)
        {
            throw new ArgumentNullException("leftSide");
        }
        if (rightSide == null)
        {
            throw new ArgumentNullException("rightSide");
        }

        if (rightSide.IsZero)
        {
            throw new DivideByZeroException();
        }

        bool divisorNeg = rightSide.IsNegative;
        bool dividendNeg = leftSide.IsNegative;

        leftSide = Abs(leftSide);
        rightSide = Abs(rightSide);

        if (leftSide < rightSide)
        {
            return new BigInteger(0);
        }

        BigInteger quotient;
        BigInteger remainder;
        Divide(leftSide, rightSide, out quotient, out remainder);

        return (dividendNeg != divisorNeg ? -quotient : quotient);
    }

    /// <summary>
    /// Divide a BigInteger by another BigInteger and returning the result.
    /// </summary>
    /// <param name="leftSide">A BigInteger divisor.</param>
    /// <param name="rightSide">A BigInteger dividend.</param>
    /// <returns>The BigInteger result.</returns>
    public static BigInteger Divide(BigInteger leftSide, BigInteger rightSide)
    {
        return leftSide / rightSide;
    }

    public static void Divide(BigInteger leftSide, BigInteger rightSide, out BigInteger quotient, out BigInteger remainder)
    {
        if (leftSide.IsZero)
        {
            quotient = new BigInteger();
            remainder = new BigInteger();
            return;
        }

        if (rightSide.m_digits.DataUsed == 1)
        {
            SingleDivide(leftSide, rightSide, out quotient, out remainder);
        }
        else
        {
            MultiDivide(leftSide, rightSide, out quotient, out remainder);
        }
    }

    public static void MultiDivide(BigInteger leftSide, BigInteger rightSide, out BigInteger quotient, out BigInteger remainder)
    {
        if (rightSide.IsZero)
        {
            throw new DivideByZeroException();
        }

        DType val = rightSide.m_digits[rightSide.m_digits.DataUsed - 1];
        int d = 0;
        for (uint mask = DigitsArray.HiBitSet; mask != 0 && (val & mask) == 0; mask >>= 1)
        {
            d++;
        }

        int remainderLen = leftSide.m_digits.DataUsed + 1;
        DType[] remainderDat = new DType[remainderLen];
        leftSide.m_digits.CopyTo(remainderDat, 0, leftSide.m_digits.DataUsed);

        DigitsArray.ShiftLeft(remainderDat, d);
        rightSide = rightSide << d;

        ulong firstDivisor = rightSide.m_digits[rightSide.m_digits.DataUsed - 1];
        ulong secondDivisor = (rightSide.m_digits.DataUsed < 2 ? (DType)0 : rightSide.m_digits[rightSide.m_digits.DataUsed - 2]);

        int divisorLen = rightSide.m_digits.DataUsed + 1;
        DigitsArray dividendPart = new DigitsArray(divisorLen, divisorLen);
        DType[] result = new DType[leftSide.m_digits.Count + 1];
        int resultPos = 0;

        ulong carryBit = (ulong)0x1 << DigitsArray.DataSizeBits; // 0x100000000
        for (int j = remainderLen - rightSide.m_digits.DataUsed, pos = remainderLen - 1; j > 0; j--, pos--)
        {
            ulong dividend = ((ulong)remainderDat[pos] << DigitsArray.DataSizeBits) + (ulong)remainderDat[pos - 1];
            ulong qHat = (dividend / firstDivisor);
            ulong rHat = (dividend % firstDivisor);

            while (pos >= 2)
            {
                if (qHat == carryBit || (qHat * secondDivisor) > ((rHat << DigitsArray.DataSizeBits) + remainderDat[pos - 2]))
                {
                    qHat--;
                    rHat += firstDivisor;
                    if (rHat < carryBit)
                    {
                        continue;
                    }
                }
                break;
            }

            for (int h = 0; h < divisorLen; h++)
            {
                dividendPart[divisorLen - h - 1] = remainderDat[pos - h];
            }

            BigInteger dTemp = new BigInteger(dividendPart);
            BigInteger rTemp = rightSide * (long)qHat;
            while (rTemp > dTemp)
            {
                qHat--;
                rTemp -= rightSide;
            }

            rTemp = dTemp - rTemp;
            for (int h = 0; h < divisorLen; h++)
            {
                remainderDat[pos - h] = rTemp.m_digits[rightSide.m_digits.DataUsed - h];
            }

            result[resultPos++] = (DType)qHat;
        }

        Array.Reverse(result, 0, resultPos);
        quotient = new BigInteger(new DigitsArray(result));

        int n = DigitsArray.ShiftRight(remainderDat, d);
        DigitsArray rDA = new DigitsArray(n, n);
        rDA.CopyFrom(remainderDat, 0, 0, rDA.DataUsed);
        remainder = new BigInteger(rDA);
    }

    private static void SingleDivide(BigInteger leftSide, BigInteger rightSide, out BigInteger quotient, out BigInteger remainder)
    {
        if (rightSide.IsZero)
        {
            throw new DivideByZeroException();
        }

        DigitsArray remainderDigits = new DigitsArray(leftSide.m_digits);
        remainderDigits.ResetDataUsed();

        int pos = remainderDigits.DataUsed - 1;
        ulong divisor = (ulong)rightSide.m_digits[0];
        ulong dividend = (ulong)remainderDigits[pos];

        DType[] result = new DType[leftSide.m_digits.Count];
        leftSide.m_digits.CopyTo(result, 0, result.Length);
        int resultPos = 0;

        if (dividend >= divisor)
        {
            result[resultPos++] = (DType)(dividend / divisor);
            remainderDigits[pos] = (DType)(dividend % divisor);
        }
        pos--;

        while (pos >= 0)
        {
            dividend = ((ulong)(remainderDigits[pos + 1]) << DigitsArray.DataSizeBits) + (ulong)remainderDigits[pos];
            result[resultPos++] = (DType)(dividend / divisor);
            remainderDigits[pos + 1] = 0;
            remainderDigits[pos--] = (DType)(dividend % divisor);
        }
        remainder = new BigInteger(remainderDigits);

        DigitsArray quotientDigits = new DigitsArray(resultPos + 1, resultPos);
        int j = 0;
        for (int i = quotientDigits.DataUsed - 1; i >= 0; i--, j++)
        {
            quotientDigits[j] = result[i];
        }
        quotient = new BigInteger(quotientDigits);
    }

    /// <summary>
    /// Perform the modulus of a BigInteger with another BigInteger and return the result.
    /// </summary>
    /// <param name="leftSide">A BigInteger divisor.</param>
    /// <param name="rightSide">A BigInteger dividend.</param>
    /// <returns>The BigInteger result.</returns>
    public static BigInteger operator %(BigInteger leftSide, BigInteger rightSide)
    {
        if (leftSide == null)
        {
            throw new ArgumentNullException("leftSide");
        }

        if (rightSide == null)
        {
            throw new ArgumentNullException("rightSide");
        }

        if (rightSide.IsZero)
        {
            throw new DivideByZeroException();
        }

        BigInteger quotient;
        BigInteger remainder;

        bool dividendNeg = leftSide.IsNegative;
        leftSide = Abs(leftSide);
        rightSide = Abs(rightSide);

        if (leftSide < rightSide)
        {
            return leftSide;
        }

        Divide(leftSide, rightSide, out quotient, out remainder);

        return (dividendNeg ? -remainder : remainder);
    }

    /// <summary>
    /// Perform the modulus of a BigInteger with another BigInteger and return the result.
    /// </summary>
    /// <param name="leftSide">A BigInteger divisor.</param>
    /// <param name="rightSide">A BigInteger dividend.</param>
    /// <returns>The BigInteger result.</returns>
    public static BigInteger Modulus(BigInteger leftSide, BigInteger rightSide)
    {
        return leftSide % rightSide;
    }
    #endregion

    #region Bitwise Operator Overloads

    public static BigInteger operator &(BigInteger leftSide, BigInteger rightSide)
    {
        int len = System.Math.Max(leftSide.m_digits.DataUsed, rightSide.m_digits.DataUsed);
        DigitsArray da = new DigitsArray(len, len);
        for (int idx = 0; idx < len; idx++)
        {
            da[idx] = (DType)(leftSide.m_digits[idx] & rightSide.m_digits[idx]);
        }
        return new BigInteger(da);
    }

    public static BigInteger BitwiseAnd(BigInteger leftSide, BigInteger rightSide)
    {
        return leftSide & rightSide;
    }

    public static BigInteger operator |(BigInteger leftSide, BigInteger rightSide)
    {
        int len = System.Math.Max(leftSide.m_digits.DataUsed, rightSide.m_digits.DataUsed);
        DigitsArray da = new DigitsArray(len, len);
        for (int idx = 0; idx < len; idx++)
        {
            da[idx] = (DType)(leftSide.m_digits[idx] | rightSide.m_digits[idx]);
        }
        return new BigInteger(da);
    }

    public static BigInteger BitwiseOr(BigInteger leftSide, BigInteger rightSide)
    {
        return leftSide | rightSide;
    }

    public static BigInteger operator ^(BigInteger leftSide, BigInteger rightSide)
    {
        int len = System.Math.Max(leftSide.m_digits.DataUsed, rightSide.m_digits.DataUsed);
        DigitsArray da = new DigitsArray(len, len);
        for (int idx = 0; idx < len; idx++)
        {
            da[idx] = (DType)(leftSide.m_digits[idx] ^ rightSide.m_digits[idx]);
        }
        return new BigInteger(da);
    }

    public static BigInteger Xor(BigInteger leftSide, BigInteger rightSide)
    {
        return leftSide ^ rightSide;
    }

    public static BigInteger operator ~(BigInteger leftSide)
    {
        DigitsArray da = new DigitsArray(leftSide.m_digits.Count);
        for (int idx = 0; idx < da.Count; idx++)
        {
            da[idx] = (DType)(~(leftSide.m_digits[idx]));
        }

        return new BigInteger(da);
    }

    public static BigInteger OnesComplement(BigInteger leftSide)
    {
        return ~leftSide;
    }

    #endregion

    #region Left and Right Shift Operator Overloads
    public static BigInteger operator <<(BigInteger leftSide, int shiftCount)
    {
        if (leftSide == null)
        {
            throw new ArgumentNullException("leftSide");
        }

        DigitsArray da = new DigitsArray(leftSide.m_digits);
        da.DataUsed = da.ShiftLeftWithoutOverflow(shiftCount);

        return new BigInteger(da);
    }

    public static BigInteger LeftShift(BigInteger leftSide, int shiftCount)
    {
        return leftSide << shiftCount;
    }

    public static BigInteger operator >>(BigInteger leftSide, int shiftCount)
    {
        if (leftSide == null)
        {
            throw new ArgumentNullException("leftSide");
        }

        DigitsArray da = new DigitsArray(leftSide.m_digits);
        da.DataUsed = da.ShiftRight(shiftCount);

        if (leftSide.IsNegative)
        {
            for (int i = da.Count - 1; i >= da.DataUsed; i--)
            {
                da[i] = DigitsArray.AllBits;
            }

            DType mask = DigitsArray.HiBitSet;
            for (int i = 0; i < DigitsArray.DataSizeBits; i++)
            {
                if ((da[da.DataUsed - 1] & mask) == DigitsArray.HiBitSet)
                {
                    break;
                }
                da[da.DataUsed - 1] |= mask;
                mask >>= 1;
            }
            da.DataUsed = da.Count;
        }

        return new BigInteger(da);
    }

    public static BigInteger RightShift(BigInteger leftSide, int shiftCount)
    {
        if (leftSide == null)
        {
            throw new ArgumentNullException("leftSide");
        }

        return leftSide >> shiftCount;
    }
    #endregion

    #region Relational Operator Overloads

    /// <summary>
    /// Compare this instance to a specified object and returns indication of their relative value.
    /// </summary>
    /// <param name="value">An object to compare, or a null reference (<b>Nothing</b> in Visual Basic).</param>
    /// <returns>A signed number indicating the relative value of this instance and <i>value</i>.
    /// <list type="table">
    ///		<listheader>
    ///			<term>Return Value</term>
    ///			<description>Description</description>
    ///		</listheader>
    ///		<item>
    ///			<term>Less than zero</term>
    ///			<description>This instance is less than <i>value</i>.</description>
    ///		</item>
    ///		<item>
    ///			<term>Zero</term>
    ///			<description>This instance is equal to <i>value</i>.</description>
    ///		</item>
    ///		<item>
    ///			<term>Greater than zero</term>
    ///			<description>
    ///				This instance is greater than <i>value</i>. 
    ///				<para>-or-</para>
    ///				<i>value</i> is a null reference (<b>Nothing</b> in Visual Basic).
    ///			</description>
    ///		</item>
    /// </list>
    /// </returns>
    public int CompareTo(BigInteger value)
    {
        return Compare(this, value);
    }

    /// <summary>
    /// Compare two objects and return an indication of their relative value.
    /// </summary>
    /// <param name="leftSide">An object to compare, or a null reference (<b>Nothing</b> in Visual Basic).</param>
    /// <param name="rightSide">An object to compare, or a null reference (<b>Nothing</b> in Visual Basic).</param>
    /// <returns>A signed number indicating the relative value of this instance and <i>value</i>.
    /// <list type="table">
    ///		<listheader>
    ///			<term>Return Value</term>
    ///			<description>Description</description>
    ///		</listheader>
    ///		<item>
    ///			<term>Less than zero</term>
    ///			<description>This instance is less than <i>value</i>.</description>
    ///		</item>
    ///		<item>
    ///			<term>Zero</term>
    ///			<description>This instance is equal to <i>value</i>.</description>
    ///		</item>
    ///		<item>
    ///			<term>Greater than zero</term>
    ///			<description>
    ///				This instance is greater than <i>value</i>. 
    ///				<para>-or-</para>
    ///				<i>value</i> is a null reference (<b>Nothing</b> in Visual Basic).
    ///			</description>
    ///		</item>
    /// </list>
    /// </returns>
    public static int Compare(BigInteger leftSide, BigInteger rightSide)
    {
        if (object.ReferenceEquals(leftSide, rightSide))
        {
            return 0;
        }

        if (object.ReferenceEquals(leftSide, null))
        {
            throw new ArgumentNullException("leftSide");
        }

        if (object.ReferenceEquals(rightSide, null))
        {
            throw new ArgumentNullException("rightSide");
        }

        if (leftSide > rightSide) return 1;
        if (leftSide == rightSide) return 0;
        return -1;
    }

    public static bool operator ==(BigInteger leftSide, BigInteger rightSide)
    {
        if (object.ReferenceEquals(leftSide, rightSide))
        {
            return true;
        }

        if (object.ReferenceEquals(leftSide, null) || object.ReferenceEquals(rightSide, null))
        {
            return false;
        }

        if (leftSide.IsNegative != rightSide.IsNegative)
        {
            return false;
        }

        return leftSide.Equals(rightSide);
    }

    public static bool operator !=(BigInteger leftSide, BigInteger rightSide)
    {
        return !(leftSide == rightSide);
    }

    public static bool operator >(BigInteger leftSide, BigInteger rightSide)
    {
        if (object.ReferenceEquals(leftSide, null))
        {
            throw new ArgumentNullException("leftSide");
        }

        if (object.ReferenceEquals(rightSide, null))
        {
            throw new ArgumentNullException("rightSide");
        }

        if (leftSide.IsNegative != rightSide.IsNegative)
        {
            return rightSide.IsNegative;
        }

        if (leftSide.m_digits.DataUsed != rightSide.m_digits.DataUsed)
        {
            return leftSide.m_digits.DataUsed > rightSide.m_digits.DataUsed;
        }

        for (int idx = leftSide.m_digits.DataUsed - 1; idx >= 0; idx--)
        {
            if (leftSide.m_digits[idx] != rightSide.m_digits[idx])
            {
                return (leftSide.m_digits[idx] > rightSide.m_digits[idx]);
            }
        }
        return false;
    }

    public static bool operator <(BigInteger leftSide, BigInteger rightSide)
    {
        if (object.ReferenceEquals(leftSide, null))
        {
            throw new ArgumentNullException("leftSide");
        }

        if (object.ReferenceEquals(rightSide, null))
        {
            throw new ArgumentNullException("rightSide");
        }

        if (leftSide.IsNegative != rightSide.IsNegative)
        {
            return leftSide.IsNegative;
        }

        if (leftSide.m_digits.DataUsed != rightSide.m_digits.DataUsed)
        {
            return leftSide.m_digits.DataUsed < rightSide.m_digits.DataUsed;
        }

        for (int idx = leftSide.m_digits.DataUsed - 1; idx >= 0; idx--)
        {
            if (leftSide.m_digits[idx] != rightSide.m_digits[idx])
            {
                return (leftSide.m_digits[idx] < rightSide.m_digits[idx]);
            }
        }
        return false;
    }

    public static bool operator >=(BigInteger leftSide, BigInteger rightSide)
    {
        return Compare(leftSide, rightSide) >= 0;
    }

    public static bool operator <=(BigInteger leftSide, BigInteger rightSide)
    {
        return Compare(leftSide, rightSide) <= 0;
    }
    #endregion

    #region Object Overrides
    /// <summary>
    /// Determines whether two Object instances are equal.
    /// </summary>
    /// <param name="obj">An <see cref="System.Object">Object</see> to compare with this instance.</param>
    /// <returns></returns>
    /// <seealso cref="System.Object">System.Object</seealso> 
    public override bool Equals(object obj)
    {
        if (object.ReferenceEquals(obj, null))
        {
            return false;
        }

        if (object.ReferenceEquals(this, obj))
        {
            return true;
        }

        BigInteger c = (BigInteger)obj;
        if (this.m_digits.DataUsed != c.m_digits.DataUsed)
        {
            return false;
        }

        for (int idx = 0; idx < this.m_digits.DataUsed; idx++)
        {
            if (this.m_digits[idx] != c.m_digits[idx])
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>A 32-bit signed integer has code.</returns>
    /// <seealso cref="System.Object">System.Object</seealso> 
    public override int GetHashCode()
    {
        return this.m_digits.GetHashCode();
    }

    /// <summary>
    /// Converts the numeric value of this instance to its equivalent base 10 string representation.
    /// </summary>
    /// <returns>A <see cref="System.String">String</see> in base 10.</returns>
    /// <seealso cref="System.Object">System.Object</seealso> 
    public override string ToString()
    {
        return ToString(10);
    }
    #endregion

    #region Type Conversion Methods
    /// <summary>
    /// Converts the numeric value of this instance to its equivalent string representation in specified base.
    /// </summary>
    /// <param name="radix">Int radix between 2 and 36</param>
    /// <returns>A string.</returns>
    public string ToString(int radix)
    {
        if (radix < 2 || radix > 36)
        {
            throw new ArgumentOutOfRangeException("radix");
        }

        if (IsZero)
        {
            return "0";
        }

        BigInteger a = this;
        bool negative = a.IsNegative;
        a = Abs(this);

        BigInteger quotient;
        BigInteger remainder;
        BigInteger biRadix = new BigInteger(radix);

        const string charSet = "0123456789abcdefghijklmnopqrstuvwxyz";
        System.Collections.ArrayList al = new System.Collections.ArrayList();
        while (a.m_digits.DataUsed > 1 || (a.m_digits.DataUsed == 1 && a.m_digits[0] != 0))
        {
            Divide(a, biRadix, out quotient, out remainder);
            al.Insert(0, charSet[(int)remainder.m_digits[0]]);
            a = quotient;
        }

        string result = new String((char[])al.ToArray(typeof(char)));
        if (radix == 10 && negative)
        {
            return "-" + result;
        }

        return result;
    }

    /// <summary>
    /// Returns string in hexidecimal of the internal digit representation.
    /// </summary>
    /// <remarks>
    /// This is not the same as ToString(16). This method does not return the sign, but instead
    /// dumps the digits array into a string representation in base 16.
    /// </remarks>
    /// <returns>A string in base 16.</returns>
    public string ToHexString()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendFormat("{0:X}", m_digits[m_digits.DataUsed - 1]);

        string f = "{0:X" + (2 * DigitsArray.DataSizeOf) + "}";
        for (int i = m_digits.DataUsed - 2; i >= 0; i--)
        {
            sb.AppendFormat(f, m_digits[i]);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Returns BigInteger as System.Int16 if possible.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>Int value of BigInteger</returns>
    /// <exception cref="System.Exception">When BigInteger is too large to fit into System.Int16</exception>
    public static int ToInt16(BigInteger value)
    {
        if (object.ReferenceEquals(value, null))
        {
            throw new ArgumentNullException("value");
        }
        return System.Int16.Parse(value.ToString(), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Returns BigInteger as System.UInt16 if possible.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception">When BigInteger is too large to fit into System.UInt16</exception>
    public static uint ToUInt16(BigInteger value)
    {
        if (object.ReferenceEquals(value, null))
        {
            throw new ArgumentNullException("value");
        }
        return System.UInt16.Parse(value.ToString(), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Returns BigInteger as System.Int32 if possible.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception">When BigInteger is too large to fit into System.Int32</exception>
    public static int ToInt32(BigInteger value)
    {
        if (object.ReferenceEquals(value, null))
        {
            throw new ArgumentNullException("value");
        }
        return System.Int32.Parse(value.ToString(), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Returns BigInteger as System.UInt32 if possible.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception">When BigInteger is too large to fit into System.UInt32</exception>
    public static uint ToUInt32(BigInteger value)
    {
        if (object.ReferenceEquals(value, null))
        {
            throw new ArgumentNullException("value");
        }
        return System.UInt32.Parse(value.ToString(), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Returns BigInteger as System.Int64 if possible.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception">When BigInteger is too large to fit into System.Int64</exception>
    public static long ToInt64(BigInteger value)
    {
        if (object.ReferenceEquals(value, null))
        {
            throw new ArgumentNullException("value");
        }
        return System.Int64.Parse(value.ToString(), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Returns BigInteger as System.UInt64 if possible.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception">When BigInteger is too large to fit into System.UInt64</exception>
    public static ulong ToUInt64(BigInteger value)
    {
        if (object.ReferenceEquals(value, null))
        {
            throw new ArgumentNullException("value");
        }
        return System.UInt64.Parse(value.ToString(), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.CurrentCulture);
    }
    #endregion
}


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


