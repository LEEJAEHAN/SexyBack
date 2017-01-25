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
            ViewLoader.label_debug.GetComponent<UILabel>().text = msg.ToString();
        }

        internal static void Error()
        {
            Debug.LogError("error 발생");
        }
    }
}