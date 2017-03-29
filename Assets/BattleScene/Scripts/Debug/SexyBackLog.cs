using UnityEngine;
using System.Collections;
using System;

public static class sexybacklog
{
    public static void Console(object msg)
    {
        Debug.Log(msg);
    }
    public static void InGame(object msg)
    {
        SexyBackPlayScene.DebugText.getInstance.Append(msg.ToString() + "\n");
    }

    internal static void Error(object msg)
    {
        Debug.LogError(msg);
    }
}