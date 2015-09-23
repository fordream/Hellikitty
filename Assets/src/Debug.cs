using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngineInternal;

public static class Debug
{
    public static bool isDebugBuild = true;

    public static new void Log(object message)
    {
        UnityEngine.Debug.Log (message.ToString ());
    }

    public static new void Log(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.Log (message.ToString (), context);
    }

    public static new void LogError(object message)
    {
        UnityEngine.Debug.LogError (message.ToString ());
    }

    public static new void LogError(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogError (message.ToString (), context);
    }

    public static new void LogWarning(object message)
    {
        UnityEngine.Debug.LogWarning(message.ToString());
    }

    public static new void LogWarning(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogWarning(message.ToString(), context);
    }

    public static new void Assert(bool condition)
    {
        UnityEngine.Debug.Assert(condition);
    }

    public static new void Assert(bool condition, string message)
    {
        UnityEngine.Debug.Assert(condition, message);
    }

    public static new void Assert(bool condition, string message, params object[] args)
    {
        UnityEngine.Debug.Assert(condition, message, args);
    }
}