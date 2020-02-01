using System.Collections.Generic;
using AssetBundleSystem;
using Installer;
using UnityEngine;

namespace SceneUtils
{
    public class CreaSceneInstaller : MonoBehaviour
    {
        [HideInInspector] public List<CreaMonoInstaller> Installers;

        [SerializeField] private CreaAssetBundleSceneController _sceneAssetBundleController;

        private void Awake()
        {
            _sceneAssetBundleController.OnInitialize(this);
        }

        private void SceneReady()
        {
            var allInstallers = Resources.FindObjectsOfTypeAll<CreaMonoInstaller>();
            foreach (var creaMonoInstaller in allInstallers)
            {
                creaMonoInstaller.CreaAwake();
            }
            
            foreach (var creaMonoInstaller in allInstallers)
            {
                creaMonoInstaller.CreaStart();
            }
        }
    }
}