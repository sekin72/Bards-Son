using System;
using System.Collections.Generic;
using System.Globalization;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public static class CreaUtils
{
    public static string GetDeviceId()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
           AndroidJavaClass up = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
           AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
           AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");
           AndroidJavaClass secure = new AndroidJavaClass("android.provider.Settings$Secure");
           return secure.CallStatic<string>("getString", contentResolver, "android_id");
#else
        return SystemInfo.deviceUniqueIdentifier;
#endif
    }
    
    public static T RandomElement<T>(this IList<T> list)
    {
        return list.Count > 0 ? list[Random.Range(0, list.Count)] : default(T);
    }

    public static bool IsTablet()
    {
        float ssw = Screen.width > Screen.height ? Screen.width : Screen.height;

        if (ssw < 800) return false;

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            float screenWidth = Screen.width / Screen.dpi;
            float screenHeight = Screen.height / Screen.dpi;
            float size = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
            if (size >= 6.5f) return true;
        }

        return false;
    }
    /// <summary>
    /// Returns default value if the list is empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T RandomElementRemove<T>(this IList<T> list)
    {
        if (list.Count > 0)
        {
            var eleNo = Random.Range(0, list.Count);
            var retVal = list[eleNo];
            list.RemoveAt(eleNo);
            return retVal;
        }
        return default(T);
    }

    public static bool HasInternetConnection()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) return false;

        return true;
    }

    public static PlatformType GetPlatformType()
    {
        #if UNITY_EDITOR
            return PlatformType.Editor;

        #elif UNITY_ANDROID && !UNITY_EDITOR
            return PlatformType.Andorid;
        
        #elif UNITY_IPHONE
            return PlatformType.Apple;
        
        #else 
            return PlatformType.Unknown;
        
        #endif

    }

    public static float GetNotchHeight(float referenceHeight = 1920f)
    {
        var safeArea = Screen.safeArea;
        return (Screen.height - safeArea.yMax) * (referenceHeight / Screen.height);
    }
    
    public static Vector3 Vector3One = Vector3.one;
    
    public static int FloorToInt(this float val)
    {
        return Mathf.FloorToInt(val);
    }

    public static string ConvertToCoinText(this double val, bool removeDecimal = false)
    {
        const double ab = 1000000000000000000;
        const double aa = 1000000000000000;
        const double t  = 1000000000000;
        const double b  = 1000000000;
        const double m  = 1000000;
        const double k  = 1000;

        var text = "";
        
        string format = "N";
        if (removeDecimal)
            format = "N0";
        
        if ((int)(val / ab) > 0)
        {
            text = (val / ab).ToString(format, new CultureInfo("en-US")) + "AB";
        }
        else if ((int)(val / aa) > 0)
        {
            text = (val / aa).ToString(format, new CultureInfo("en-US")) + "AA";
        }
        else if ((int) (val / t) > 0)
        {
            text = (val / t).ToString(format, new CultureInfo("en-US")) + "T";
        }
        else if ((int)(val / b) > 0)
        {
            text = (val / b).ToString(format, new CultureInfo("en-US")) + "B";
        }
        else if ((int)(val / m) > 0)
        {
            text = (val / m).ToString(format, new CultureInfo("en-US")) + "M";
        }
        else if ((int)(val / k) > 0)
        {
            text = (val / k).ToString(format, new CultureInfo("en-US")) + "K";
        }
        else
        {
            text = Mathf.FloorToInt((float)val).ToString(format, new CultureInfo("en-US"));
        }

        return text;
    }

    public static bool IsPointInsideOfRectTransform(this Vector3 point, RectTransform rt)
    {
        var rect = rt.rect;

        var globalCorners = new Vector3[4];
        rt.GetWorldCorners(globalCorners);

        var leftSide = globalCorners[1].x;
        var rightSide = globalCorners[2].x;
        var topSide = globalCorners[2].y;
        var bottomSide = globalCorners[0].y;

        if (point.x >= leftSide &&
            point.x <= rightSide &&
            point.y >= bottomSide &&
            point.y <= topSide)
        {
            return true;
        }
        return false;
    }
    
    public static void SetAlpha(this Graphic image, float a)
    {
        var color = image.color;
        color.a = a;
        image.color = color;
    }
}

public enum PlatformType
{
    Editor = 0,
    Andorid = 1,
    Apple = 2,
    Unknown = 3
}
