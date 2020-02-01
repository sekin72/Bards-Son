using UnityEngine;

namespace LogSystem
{
    public static class LogManager
    {
        public static void Log(this object mono, object message)
        {
            Debug.Log("[" +mono.GetType() + "] => " + message);
        }
    
        public static void LogWarning(this object mono, object message)
        {
            Debug.LogWarning("[" +mono.GetType() + "] => " + message);
        }
    
        public static void LogError(this object mono, object message)
        {
            Debug.LogError("[" +mono.GetType() + "] => " + message);
        }
    }
}