using System;
using System.Collections.Generic;

namespace SexyBackPlayScene
{
    internal static class StringParser
    {
        internal static string ReplaceString(string text, string arg1)
        {
            string temp = text.Replace("$1$", arg1);
            return temp;
        }
        internal static string ReplaceString(string text, string arg1, string arg2)
        {
            string temp = text.Replace("$1$", arg1);
            temp = temp.Replace("$2$", arg2);
            return temp;
        }

        private static string GetName(object iD)
        {
            throw new NotImplementedException();
        }
    }
}