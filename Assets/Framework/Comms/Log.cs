using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Log
{
    static public void I(object message)
    {
        #if PRINTLOG
        Debug.Log(message);
        #endif
    }

    static public void E(object message)
    {
        #if PRINTLOG
        Debug.LogError(message);
        #endif
    }

    static public void W(object message)
    {
        #if PRINTLOG
        Debug.LogWarning(message);
        #endif
    }

}
