using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SceneUtils;
using JetBrains.Annotations;
using LogSystem;
using UnityEngine;

namespace AssetBundleSystem
{
    public class CreaAssetBundleSceneController : MonoBehaviour
    {
        [SerializeField]
        public List<CreaAssetBundle> SceneBundles;

        private CreaSceneInstaller _sceneInstaller;

        public virtual void OnInitialize([NotNull]CreaSceneInstaller sceneInstaller)
        {
            CreaSceneManager.OnSceneLoadStarted += OnSceneLoadingBegin;
            _sceneInstaller = sceneInstaller;

            foreach (var sceneBundle in SceneBundles)
            {
                this.Log("==>>> Installing Bundle => " + sceneBundle.Name);
                StartCoroutine(sceneBundle.OpenAssetBundle());
            }

            StartCoroutine(OnAssetBundleInitializeCompleted());
        }

        private void OnSceneLoadingBegin(CreaScene scene)
        {
            try
            {
                foreach (var creaAssetBundle in SceneBundles)
                {
                    creaAssetBundle.UnLoadAssetBundle();
                }
            
                this.Log("All Bundles Unloaded!");
            }
            catch (Exception e)
            {
                this.LogError("OnSceneLoading Error => " + e);
            }
        }

        private IEnumerator OnAssetBundleInitializeCompleted()
        {
            yield return SceneBundles.Where(x => !x.IsLoaded).ToList().Count == 0;
            _sceneInstaller.SendMessage("SceneReady");
        }

        private void OnDestroy()
        {
            CreaSceneManager.OnSceneLoadStarted -= OnSceneLoadingBegin;
        }
    }
}