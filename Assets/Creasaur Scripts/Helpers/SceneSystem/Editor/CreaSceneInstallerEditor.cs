using System.Collections.Generic;
using System.IO;
using System.Net;
using AssetBundleSystem;
using Installer;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SceneUtils
{
    [CustomEditor(typeof(CreaSceneInstaller))]
    public class CreaSceneInstallerEditor : Editor
    {
        private CreaSceneInstaller _creaSceneInstaller;

        private bool _listSceneInstallers;
        private ReorderableList _installerList;
        private SerializedProperty _assetBundleController;
        
        private void OnEnable()
        {
            _creaSceneInstaller = (CreaSceneInstaller) target;
            _assetBundleController = serializedObject.FindProperty("_sceneAssetBundleController");
        
            if (_creaSceneInstaller.Installers == null)
            {
                _creaSceneInstaller.Installers = new List<CreaMonoInstaller>();
            }
            
            _installerList = new ReorderableList(_creaSceneInstaller.Installers, typeof(CreaMonoInstaller), true, false, true, true);
            _installerList.drawElementCallback += delegate(Rect rect, int index, bool active, bool focused)
                {
                    _creaSceneInstaller.Installers[index] = (CreaMonoInstaller)EditorGUI.ObjectField(rect, _creaSceneInstaller.Installers[index], typeof(CreaMonoInstaller), true);
                };
        }

        public override void OnInspectorGUI()
        {
            if (_creaSceneInstaller == null)
            {
                OnEnable();
                return;
            }

            ListSceneInstallersGUI();
        }

        private void ListSceneInstallersGUI()
        {
            serializedObject.Update();

            ChangeBackGroundColor(Color.gray);
            
            var installerMenu = new GenericMenu();
            
            _listSceneInstallers = EditorGUILayout.Foldout(_listSceneInstallers, "Installers");
            if (_listSceneInstallers)
            {   
                _installerList.DoLayoutList();
            }
            EndBackgroundColor();
            
            EditorGUILayout.Space();
            
            ChangeBackGroundColor(Color.black);
            EditorGUILayout.BeginVertical("box");
            EndBackgroundColor();
            
            EditorGUILayout.PropertyField(_assetBundleController, new GUIContent("Asset Bundle Controller"));
            EditorGUILayout.EndVertical();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void ChangeBackGroundColor(Color color)
        {
            GUI.backgroundColor = color;
        }

        private void EndBackgroundColor()
        {
            GUI.backgroundColor = Color.white;
        }

//        [MenuItem("Assets/Create/Creasaur/SceneInstaller", false, 0)]
//        private static void CreateSceneInstallers()
//        {
//            var path = "Assets";
//
//            var selections = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
//            foreach (var selection in selections)
//            {
//                var currentPath = AssetDatabase.GetAssetPath(selection);
//                if (!string.IsNullOrEmpty(currentPath) || File.Exists(currentPath))
//                {
//                    path = currentPath;
//                    break;
//                }
//            }
//        }
    }
}