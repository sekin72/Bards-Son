using AssetBundleSystem;
using Installer;
using Timer;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(AssetBundleItem))]
public class AssetBundleItemEditor : Editor
{
    private AssetBundleItem _item;
    private Object[] _items;

    private GenericMenu _bundleMenu;
    private GenericMenu _parentItemMenu;
    private CreaAssetBundleSceneController _assetBundleController;

    private AssetBundle _assetBundle;
    
    private CreaCoroutine _creaCoroutine;
    private CreaCoroutine CreaCoroutine
    {
        get
        {
            if (_creaCoroutine == null && ContainerManager.MainContainer != null)
            {
                _creaCoroutine = ContainerManager.MainContainer.TryResolve<CreaCoroutine>();
            }
            return _creaCoroutine;
        }
    }
    
    private void OnEnable()
    {
        _assetBundleController = FindObjectOfType<CreaAssetBundleSceneController>();
        _item = (AssetBundleItem) target;
        _items = targets;
        foreach (var bundleItem in _items)
        {
            if(((AssetBundleItem)bundleItem).GetComponent<Image>().sprite == null)        return;
            ((AssetBundleItem)bundleItem).ItemId = ((AssetBundleItem)bundleItem).GetComponent<Image>().sprite.name;
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (GUILayout.Button("Bundle"))
        {
            _bundleMenu = new GenericMenu();
            foreach (var creaAssetBundle in _assetBundleController.SceneBundles)
            {
                _bundleMenu.AddItem(new GUIContent(creaAssetBundle.BundleId), true, () =>
                    {
                        foreach (var bundleItem in _items)
                        {
                            if(((AssetBundleItem)bundleItem).GetComponent<Image>().sprite == null)        return;
    
                            ((AssetBundleItem)bundleItem).BundleId = creaAssetBundle.BundleId;
                            if (creaAssetBundle.AssetBundle == null)
                            {
//                                Observable.FromCoroutine(() => creaAssetBundle.OpenAssetBundle())
//                                    .SubscribeOn(Scheduler.MainThreadIgnoreTimeScale).Subscribe(
//                                        _ =>
//                                        {
//                                            if (creaAssetBundle.AssetBundle != null)
//                                            {
//                                                _assetBundle = creaAssetBundle.AssetBundle;
//                                            }
//                                        });
                            }
                            else
                            {
                                _assetBundle = creaAssetBundle.AssetBundle;
                            }
                        }
                        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                    });
            }
            
            _bundleMenu.ShowAsContext();
        }

        if (GUILayout.Button("Parent Item Name") && _assetBundle != null)
        {
            _parentItemMenu = new GenericMenu();
            var assetNames = _assetBundle.GetAllAssetNames();
            foreach (var assetName in assetNames)
            {
                var assetNameparts = assetName.Split('/');
                var asset = assetNameparts[assetNameparts.Length - 1].Split('.')[0];
                _parentItemMenu.AddItem(new GUIContent(asset),true, () =>
                {
                    _item.ParentItemName = asset;
                    foreach (var bundleItem in _items)
                    {
                        if(((AssetBundleItem)bundleItem).GetComponent<Image>().sprite == null)        return;
                        ((AssetBundleItem)bundleItem).ParentItemName = asset;
                    }
                });
            }
            _parentItemMenu.ShowAsContext();
        }
        EditorGUILayout.LabelField("Bundle Id => " + _item.BundleId);
        EditorGUILayout.LabelField("Item Name => " + _item.ItemId);
        var tempParentItemName = _item.ParentItemName;
        _item.ParentItemName = EditorGUILayout.TextField("Parent Item Name", _item.ParentItemName);
        foreach (var bundleItem in _items)
        {
            if(((AssetBundleItem)bundleItem).GetComponent<Image>().sprite == null)        return;
            ((AssetBundleItem)bundleItem).ParentItemName = _item.ParentItemName;
        }
        if (_item.ParentItemName != tempParentItemName)
        {
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
        serializedObject.ApplyModifiedProperties();
    }
}
