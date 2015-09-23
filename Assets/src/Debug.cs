using UnityEngine;
using System.Collections;

/*
* Class that overrides UnityEngine's Debug methods.
* This allows us to have more freedom when logging, such as writing to a file
* or having a verbose log that can be turned on or off
*/
public static class Debug
{
    public static bool VERBOSE_LOG = true;

    /*
    * Logs an info message to the console
    */
    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    /*
    * Logs an info message to the console
    */
    public static void Log(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.Log(message, context);
    }

    /*
    * Logs an error message to the console
    */
    public static void LogError(object message)
    {
        UnityEngine.Debug.LogError(message);
    }

    /*
    * Logs an error message to the console
    */
    public static void LogError(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogError(message, context);
    }

    /*
    * Logs a warning message to the console
    */
    public static void LogWarning(object message)
    {
        UnityEngine.Debug.LogWarning(message);
    }

    /*
    * Logs a warning message to the console
    */
    public static void LogWarning(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogWarning(message, context);
    }

    /*
    * Logs a verbose message to the console if VERBOSE_LOG is true
    */
    public static void LogVerbose(object message)
    {
        if (VERBOSE_LOG) UnityEngine.Debug.Log("[verbose]: " + message);
    }

    /*
    * Logs a verbose message to the console if VERBOSE_LOG is true
    */
    public static void LogVerbose(object message, UnityEngine.Object context)
    {
        if (VERBOSE_LOG) UnityEngine.Debug.LogWarning(message, context);
    }

    /*
    * Unity assert
    */
    public static void Assert(bool condition)
    {
        UnityEngine.Debug.Assert(condition);
    }

    /*
    * Unity assert
    */
    public static void Assert(bool condition, string message)
    {
        UnityEngine.Debug.Assert(condition, message);
    }

    /*
    * Unity assert
    */
    public static void Assert(bool condition, string message, params object[] args)
    {
        UnityEngine.Debug.Assert(condition, message, args);
    }
}