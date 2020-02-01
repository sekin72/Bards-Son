using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AssetBundleSystem;
using Installer;
using LogSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AssetBundleItem : CreaMonoInstaller
{
    [SerializeField] public string BundleId;
    [SerializeField] public string ItemId;
    [SerializeField] public string ParentItemName;

    private CreaAssetBundleSceneController _assetBundleSceneController;
    
    [Inject]
    private void OnInitialize(CreaAssetBundleSceneController assetBundleSceneController)
    {
        _assetBundleSceneController = assetBundleSceneController;
    }
    
    public override void CreaAwake()
    {
//        var currentBundle = _assetBundleSceneController.SceneBundles.FirstOrDefault(x => x.BundleId == BundleId);
//        if (currentBundle != null)
//        {
//            var image = currentBundle.AssetBundle.LoadAssetWithSubAssets<Sprite>(ParentItemName);
//            if(image != null)
//            {
//                var currentImage = image.FirstOrDefault(x => x.name == ItemId);
//                if (currentImage != null)
//                {
//                    GetComponent<Image>().sprite = currentImage;
//                }
//            }
//        }
    }
}
