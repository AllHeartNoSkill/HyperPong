using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class DebugLogger
{
    [Conditional("DEBUG")]
    public static void Log(object message)
    {
        Debug.Log(message);
    }

    [Conditional("DEBUG")]
    public static void Log(object message, Object context)
    {
        Debug.Log(message, context);
    }
    
    [Conditional("DEBUG")]
    public static void LogWarning(object message)
    {
        Debug.LogWarning(message);
    }

    [Conditional("DEBUG")]
    public static void LogWarning(object message, Object context)
    {
        Debug.LogWarning(message, context);
    }
    
    [Conditional("DEBUG")]
    public static void LogError(object message)
    {
        Debug.LogError(message);
    }

    [Conditional("DEBUG")]
    public static void LogError(object message, Object context)
    {
        Debug.LogError(message, context);
    }
}
