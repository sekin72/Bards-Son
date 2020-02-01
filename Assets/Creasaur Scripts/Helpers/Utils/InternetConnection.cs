using System;
using Helpers.Utils;
using Installer;
using Timer;
using UnityEngine;

public class InternetConnection : MonoBehaviour
{
    [SerializeField] private float _checkDuration;

    private bool _currentInternetStatus;

    public Action OnInternetConnectionLost;
    public Action OnInternetReconnected;
    
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
    
    private void Start()
    {
        _currentInternetStatus = CreaUtils.HasInternetConnection();

        CreaCoroutine.Timer(TimeSpan.FromSeconds(_checkDuration), () =>
        {
            var isInternetConnected = CreaUtils.HasInternetConnection();
            if (_currentInternetStatus != isInternetConnected)
            {
                _currentInternetStatus = isInternetConnected;

                if (_currentInternetStatus)
                {
                    Debug.Log("==> [INTERNET] Internet Reconnected!!");
                    OnInternetReconnected.SafeInvoke();
                }
                else
                {
                    Debug.Log("==> [INTERNET] Internet Connection Lost!!");
                    OnInternetConnectionLost.SafeInvoke();
                }
            }
            
            CreaCoroutine.Timer(TimeSpan.FromSeconds(_checkDuration), () =>
            {
                var isInternetConnected2 = CreaUtils.HasInternetConnection();
                if (_currentInternetStatus != isInternetConnected2)
                {
                    _currentInternetStatus = isInternetConnected2;

                    if (_currentInternetStatus)
                    {
                        Debug.Log("==> [INTERNET] Internet Reconnected!!");
                        OnInternetReconnected.SafeInvoke();
                    }
                    else
                    {
                        Debug.Log("==> [INTERNET] Internet Connection Lost!!");
                        OnInternetConnectionLost.SafeInvoke();
                    }
                }
            });
        });
    }
}
