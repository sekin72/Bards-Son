using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionHelper
{
   
    public static void SafeInvoke(this System.Action callback)
    {
        callback?.Invoke();
    }

    public static void SafeInvoke(this System.Action<Vector3> callback, Vector3 position)
    {
        callback?.Invoke(position);
    }

    public static void SafeInvoke(this System.Action<ShopVariables> callback, ShopVariables shop)
    {
        callback?.Invoke(shop);
    }

    public static void SafeInvoke(this System.Action<SpecialEvent> callback, SpecialEvent param)
    {
        callback?.Invoke(param);
    }

    public static Customer SafeInvoke(this System.Func<Customer> callback)
    {
        return callback?.Invoke();
    }

    public static void SafeInvoke(this System.Action<int> callback, int lvl)
    {
        callback?.Invoke(lvl);
    }
}
