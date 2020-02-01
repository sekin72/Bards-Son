using UnityEditor;
using UnityEngine;

namespace AssetBundleSystem
{
    static class CreateAssetBundles
    {
        [MenuItem ("Assets/Build AssetBundles [Android]")]
        static void BuildAllAssetBundlesAndroid ()
        {
            BuildPipeline.BuildAssetBundles (Application.streamingAssetsPath + "/AssetBundles/", BuildAssetBundleOptions. None, BuildTarget.Android);
        }
        
        [MenuItem ("Assets/Build AssetBundles [iOS]")]
        static void BuildAllAssetBundlesIOS ()
        {
            BuildPipeline.BuildAssetBundles (Application.streamingAssetsPath + "/AssetBundles/", BuildAssetBundleOptions.None, BuildTarget.iOS);
        }
        
        [MenuItem ("Assets/Build AssetBundles [Windows]")]
        static void BuildAllAssetBundlesWindows ()
        {
            BuildPipeline.BuildAssetBundles (Application.streamingAssetsPath + "/AssetBundles/", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        }
    }
}