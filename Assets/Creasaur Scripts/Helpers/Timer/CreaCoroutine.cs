using System;
using System.Collections;
using Helpers.Utils;
using UnityEngine;

namespace Timer
{
    public class CreaCoroutine : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private IEnumerator TimerCoroutine(TimeSpan time, Action callback)
        {
            yield return new WaitForSeconds((float)time.TotalSeconds);
            callback.SafeInvoke();
        }
        
        public Coroutine Timer(TimeSpan time, Action callback)
        {
            return StartCoroutine(TimerCoroutine(time, callback));
        }

        private IEnumerator TimerCoroutine(TimeSpan time, Action<SpecialEvent> callback, SpecialEvent param)
        {
            yield return new WaitForSeconds((float)time.TotalSeconds);
            callback.SafeInvoke(param);
        }

        public Coroutine Timer(TimeSpan time, Action<SpecialEvent> callback, SpecialEvent param)
        {
            return StartCoroutine(TimerCoroutine(time, callback, param));
        }

        public void StopTimer(Coroutine coroutine)
        {
            StopCoroutine(coroutine);
        }

        public Coroutine Coroutine(IEnumerator method)
        {
            return StartCoroutine(method);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}