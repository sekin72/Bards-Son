using System;

namespace Helpers.Utils
{
    public static class EventExtensionMethods
    {
        public static T Chain<T>(this T source, Action<T> action)
        {
            action(source);
            return source;
        }

        #region Invoke Extensions

        public static void SafeInvoke(this Action source)
        {
            if (source != null) source.Invoke();
        }

        public static void SafeInvoke<T>(this Action<T> source, T value)
        {
            if (source != null) source.Invoke(value);
        }

        public static void SafeInvoke<T1, T2>(this Action<T1, T2> source, T1 firstValue, T2 secondValue)
        {
            if (source != null) source.Invoke(firstValue, secondValue);
        }

        public static void SafeInvoke<T1, T2, T3>(this Action<T1, T2, T3> source, T1 firstValue, T2 secondValue, T3 thirdValue)
        {
            if (source != null) source.Invoke(firstValue, secondValue, thirdValue);
        }

        public static void SafeDispose(this IDisposable disposable)
        {
            if(disposable != null)
                disposable.Dispose();
        }
        #endregion

    }
}