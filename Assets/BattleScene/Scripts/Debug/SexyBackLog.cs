using UnityEngine;
using System.Collections;
using System;

namespace SexyBackPlayScene
{
    public static class sexybacklog
    {
        public static void Console(object msg)
        {
            Debug.Log(msg);
        }
        public static void InGame(object msg)
        {
            DebugText.getInstance.Append(msg.ToString() + "\n");
        }

        internal static void Error(object msg)
        {
            Debug.LogError(msg);
        }
    }
}