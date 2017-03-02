using System;

namespace SexyBackPlayScene
{
    internal static class StringParser
    {
        internal static string ReplaceString(string text, string arg1)
        {
            string temp = text.Replace("$s", arg1);
            return temp;
        }
    }
}