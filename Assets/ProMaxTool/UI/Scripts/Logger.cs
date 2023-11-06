using System.Diagnostics;
using UnityEngine;

public static class Logger
{

    [Conditional("ENABLE_LOGS")]

    public static void Debug(object logMsg)
    {

        UnityEngine.Debug.Log(logMsg);

    }
    public static void DebugWarning(object obj)
    {
        UnityEngine.Debug.LogWarning(obj);
        
    }

}