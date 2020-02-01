using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AssetBundleSystem
{
    public static class CreaAssetBundleManager
    {
        private static List<CreaAssetBundle> _allAssetBundles;

        public static List<CreaAssetBundle> GetAssetBundles()
        {
            var assetBundlesPath = Application.streamingAssetsPath + "/AssetBundles/";
            
            var allFiles = new DirectoryInfo(assetBundlesPath).GetFiles().ToList();
            allFiles = allFiles.Where(x => x.Name != "AssetBundles" && 
                                           !x.Name.Contains("manifest") &&
                                           !x.Name.Contains(".meta")).ToList();


            if (_allAssetBundles == null)
            {
                _allAssetBundles = new List<CreaAssetBundle>();
            }

            foreach (var fileInfo in allFiles)
            {
                var currentBundle = _allAssetBundles.FirstOrDefault(x => x.AssetBundlePath == fileInfo.FullName);
                if (currentBundle != null)
                {
                    continue;
                }

                var newBundle = new CreaAssetBundle(fileInfo.Name, fileInfo.Name);
                _allAssetBundles.Add(newBundle);
            }
            
            return _allAssetBundles;
        }
    }
}