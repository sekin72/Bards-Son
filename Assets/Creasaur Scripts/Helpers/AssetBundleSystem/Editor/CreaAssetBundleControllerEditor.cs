using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AssetBundleSystem
{
    [CanEditMultipleObjects, Serializable]
    [CustomEditor(typeof(CreaAssetBundleSceneController))]
    public class CreaAssetBundleControllerEditor : Editor
    {
        private CreaAssetBundleSceneController _creaAssetBundleSceneController;

        private ReorderableList _assetBundleList;
        
        private void OnEnable()
        {
            _creaAssetBundleSceneController = (CreaAssetBundleSceneController) target;
            
            if (_creaAssetBundleSceneController.SceneBundles == null)
            {
                _creaAssetBundleSceneController.SceneBundles = new List<CreaAssetBundle>();
            }
            
            _assetBundleList = new ReorderableList(_creaAssetBundleSceneController.SceneBundles, typeof(CreaAssetBundle), false, true, true, false);
            _assetBundleList.drawHeaderCallback += DrawHeaderCallback;
            _assetBundleList.onAddCallback += OnAssetBundleListAddCallback;
            _assetBundleList.drawElementCallback += OnAssetBundleListDrawElementCallback;
        }

        private void DrawHeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect, "Scene Asset Bundles");
        }

        private void OnDisable()
        {
            OnDestroy();
        }

        private void OnDestroy()
        {
            _assetBundleList.drawHeaderCallback -= DrawHeaderCallback;
            _assetBundleList.onAddCallback -= OnAssetBundleListAddCallback;
            _assetBundleList.drawElementCallback -= OnAssetBundleListDrawElementCallback;
        }

        public override void OnInspectorGUI()
        {
            if (_creaAssetBundleSceneController == null)
            {
                OnEnable();
                return;
            }

            if (GUILayout.Button("ResetScene"))
            {
                var allImages = Resources.FindObjectsOfTypeAll<AssetBundleItem>();
                foreach (var image in allImages)
                {
                    image.GetComponent<Image>().sprite = null;
                }
            }
            EditorGUILayout.Space();
            _assetBundleList.DoLayoutList();
        }
        
        private void OnAssetBundleListAddCallback(ReorderableList list)
        {
            var bundleList = CreaAssetBundleManager.GetAssetBundles();
            
            var genericAssetBundleMenu = new GenericMenu();
            foreach (var creaAssetBundle in bundleList)
            {
                if (_creaAssetBundleSceneController.SceneBundles.Contains(creaAssetBundle))
                {
                    continue;
                }
                
                genericAssetBundleMenu.AddItem(new GUIContent(creaAssetBundle.Name), true, bundle =>
                {
                    _creaAssetBundleSceneController.SceneBundles.Add(creaAssetBundle);
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                }, creaAssetBundle);
            }
            genericAssetBundleMenu.ShowAsContext();
        }
        
        private void OnAssetBundleListDrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
        {
            var labelRect = rect;
            labelRect.x += 40;
            labelRect.y += 2;
            EditorGUI.LabelField(labelRect, _creaAssetBundleSceneController.SceneBundles[index].Name);
            GUI.backgroundColor = Color.red;
            rect.size = new Vector2(25, rect.size.y);
            if (GUI.Button(rect, "-"))
            {
                _creaAssetBundleSceneController.SceneBundles.RemoveAt(index);
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }

            labelRect.x += 150;
            EditorGUI.LabelField(labelRect, "BundleId");
            labelRect.x += 90;
            _creaAssetBundleSceneController.SceneBundles[index].BundleId = EditorGUI.TextField(labelRect, _creaAssetBundleSceneController.SceneBundles[index].BundleId);
            GUI.backgroundColor = Color.white;
        }
        
    }
}