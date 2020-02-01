using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Helpers.Utils;
using Installer;
using Timer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneUtils
{
    public static class CreaSceneManager
    {
        public static string ActiveScene
        {
            get { return SceneManager.GetActiveScene().name; }
        }

        public static Action<CreaScene> OnSceneLoadStarted;
        public static Action<CreaScene> OnSceneLoadCompleted;
        public static Action<float> OnProgressChangedCallback;

        private static Queue<CreaScene> _sceneList = new Queue<CreaScene>();

        private static CreaCoroutine _creaCoroutine;
        public static CreaCoroutine CreaCoroutine
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

        public static void LoadScene(CreaScene scene)
        {
            Resources.UnloadUnusedAssets();
            
            if (scene == CreaScene.None)
            {
                return;
            }
            
            if (OnSceneLoadStarted != null)
            {
                OnSceneLoadStarted(scene);
            }

            CreaCoroutine.Coroutine(LoadSceneAsync(scene.ToString(), LoadSceneMode.Single, operation =>
            {
                operation.allowSceneActivation = true;

                if (OnSceneLoadCompleted != null)
                {
                    OnSceneLoadCompleted(scene);
                }
                    
                _sceneList.Enqueue(scene);
            }));
        }

        public static void LoadSceneWithLoadingView(CreaScene scene, SceneRequireMethod[] requireMethods = null)
        {
            Resources.UnloadUnusedAssets();
            
            if (scene == CreaScene.None)
            {
                return;
            }
            
            if (OnSceneLoadStarted != null)
            {
                OnSceneLoadStarted(scene);
            }

            CreaCoroutine.Coroutine(LoadSceneWithLoadingViewAsync(scene.ToString(), () =>
            {
                _sceneList.Enqueue(scene);
                if (OnSceneLoadCompleted != null)
                {
                    OnSceneLoadCompleted(scene);
                }
            }, requireMethods));
        }

        private static IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode mode, Action<AsyncOperation> callback)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName, mode);
            asyncOperation.allowSceneActivation = false;
            
            yield return asyncOperation;

            if (callback != null)
            {
                callback(asyncOperation);
            }
        }

        private static IEnumerator LoadSceneWithLoadingViewAsync(string sceneName, Action onCompleteCallback, SceneRequireMethod[] requireMethods = null)
        {   
            var loadingSceneOperator = SceneManager.LoadSceneAsync("Loading");
            loadingSceneOperator.allowSceneActivation = true;

            while (!loadingSceneOperator.isDone)
            {
                yield return new WaitForSeconds(0);
            }

            var asyncOperator = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            asyncOperator.allowSceneActivation = false;
            
            Debug.Log("Loading Scene Loaded");
            while (asyncOperator.progress < 0.9f)
            {
                onCompleteCallback.SafeInvoke();
                if (OnProgressChangedCallback != null)
                {
                    if(requireMethods == null)
                        OnProgressChangedCallback(asyncOperator.progress * 100);
                    else
                        OnProgressChangedCallback(asyncOperator.progress * 50);
                }
                
                yield return new WaitForSeconds(0);
            }

            if(requireMethods == null)
            {
                asyncOperator.allowSceneActivation = true;
                yield break;
            }

            var progress = 50f;
            foreach (var sceneRequireMethod in requireMethods)
            {
                OnProgressChangedCallback.SafeInvoke(progress);
                yield return sceneRequireMethod.Callback;
                progress += sceneRequireMethod.PercentageValue;
                OnProgressChangedCallback.SafeInvoke(progress);
            }

            asyncOperator.allowSceneActivation = true;
        }
    }
}
