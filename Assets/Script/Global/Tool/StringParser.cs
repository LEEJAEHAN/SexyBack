using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class StringParser
{
    internal static string GetTimeText(int second)
    {
        int remain = second;
        int h = remain / 3600;
        remain -= h * 3600;
        int m = remain / 60;
        remain -= m * 60;
        int s = remain;

        return String.Format("{0}시간 {1}분 {2}초", h, m, s);
    }
}
