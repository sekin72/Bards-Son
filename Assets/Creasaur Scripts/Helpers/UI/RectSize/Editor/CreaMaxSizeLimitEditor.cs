using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Helpers.UI.RectSize
{
    [CustomEditor(typeof(CreaMaxSizeLimit))]
    public class CreaMaxSizeLimitEditor : Editor
    {
        private bool _isActive;
        private CreaMaxSizeLimit _target;
        
        private void OnEnable()
        {
            _isActive = false;
            _target = target as CreaMaxSizeLimit;
        }


        public override void OnInspectorGUI()
        {
            var tempIsActive = _isActive;
            _isActive = EditorGUILayout.Toggle(new GUIContent("Show"), _isActive);
            
            if(!_isActive)
                return;

            if (!tempIsActive && _isActive)
            {
                _target.MaxSizeDelta = _target.GetComponent<RectTransform>().sizeDelta;
            }
            
            EditorGUI.BeginChangeCheck();
           _target.MaxSizeDelta = EditorGUILayout.Vector2Field("Max Size Delta", _target.MaxSizeDelta);
           if (EditorGUI.EndChangeCheck())
           {
               _target.GetComponent<RectTransform>().sizeDelta = _target.MaxSizeDelta;
               EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
           }
        }
    }
}