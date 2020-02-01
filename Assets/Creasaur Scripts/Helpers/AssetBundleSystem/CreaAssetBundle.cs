using System;
using System.Collections;
using LogSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetBundleSystem
{
    [Serializable]
    public class CreaAssetBundle
    {
        public AssetBundle AssetBundle;

        public string AssetBundlePath;
        public string Name;
        public string BundleId;
        
        public bool IsLoaded;
        
        public CreaAssetBundle(string bundlePath, string name)
        {
            AssetBundlePath = bundlePath;
            Name = name;
        }

        public IEnumerator OpenAssetBundle()
        {
            var bundleRequest = new WWW(Application.streamingAssetsPath + "/AssetBundles/"+AssetBundlePath);

            yield return bundleRequest;

            if (!string.IsNullOrEmpty(bundleRequest.error))
            {
                this.LogError("Open Bundle Error => " + bundleRequest.error);
                yield break;
            }

            AssetBundle = bundleRequest.assetBundle;
            IsLoaded = true;
        }

        public void UnLoadAssetBundle()
        {
            if (AssetBundle != null)
            {
                AssetBundle.Unload(true);
            }
        }
    }
}