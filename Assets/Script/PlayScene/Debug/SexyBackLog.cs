using UnityEngine;
using System.Collections;
using System;

public static class sexybacklog
{
    public static bool DebugMode = true;

    public static void Console(object msg)
    {
        if(DebugMode)
            Debug.Log(msg);
    }
    public static void InGame(object msg)
    {
        if (DebugMode)
            SexyBackPlayScene.DebugText.getInstance.Append(msg.ToString() + "\n");
    }

    internal static void Error(object msg)
    {
        if (DebugMode)
            Debug.LogError(msg);
    }
}