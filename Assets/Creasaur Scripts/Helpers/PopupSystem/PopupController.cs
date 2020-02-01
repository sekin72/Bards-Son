using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helpers.Utils;
using LogSystem;
using PopupSystem;
using UnityEngine;
using Zenject;

public class PopupController : MonoBehaviour
{
    [SerializeField] private List<PopupBase> _allPopups;

    private Queue<PopupBase> _popupQueue;
    private Queue<PopupBase> _waitingPopups;

    private Dimmer _dimmer;

    #region PrivateMethods

    private Dictionary<PopupType, Action> _popupHideEvents;
    private Dictionary<PopupType, Action> _popupShowEvents;

    [SerializeField] private Transform _popupParents;

    public Action<PopupType> AnyPopupOpened;

    [Inject]
    private void OnInitialize()
    {
        _allPopups = new List<PopupBase>();
        _popupQueue = new Queue<PopupBase>();
        _waitingPopups = new Queue<PopupBase>();

        _popupHideEvents = new Dictionary<PopupType, Action>();
        _popupShowEvents = new Dictionary<PopupType, Action>();

        for (var ii = 0; ii < _popupParents.childCount; ii++)
        {
            var child = _popupParents.GetChild(ii);
            var popupComponent = child.GetComponent<PopupBase>();
            if (popupComponent != null)
            {
                child.gameObject.SetActive(false);
                _allPopups.Add(child.GetComponent<PopupBase>());
                popupComponent.OnInitialize();
            }
        }

        _dimmer = GetComponentInChildren<Dimmer>();
        _dimmer.gameObject.SetActive(false);
        _dimmer.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        _dimmer.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
    }

    private void Start()
    {
        _dimmer.GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height);
        _dimmer.transform.localPosition = Vector3.zero;
        _dimmer.transform.SetParent(_popupParents);
        _dimmer.transform.SetSiblingIndex(0);
    }

    #endregion

    #region PublicMethods

    public T GetPopup<T>(PopupType popupType)
    {
        return _allPopups.FirstOrDefault(x => x.PopupType == popupType).GetComponent<T>();
    }

    public bool IsPopupActive(PopupType popupType)
    {
        return _popupQueue.FirstOrDefault(x => x.PopupType == popupType) != null;
    }

    public bool IsAnyPopupOpen()
    {
        return _allPopups.Count(x => x.gameObject.activeSelf) != 0;
    }

    public void SetupShopPopup(PopupType type, Shop shop)
    {
        var currentPopup = (ShopPopup)_allPopups.FirstOrDefault(x => x.PopupType == type);
        if (currentPopup == null)
        {
            this.Log("Show Popup => Popup doesn't exist! + " + type);
            return;
        }

        currentPopup.SetupShopPopup(shop);
    }

    public void SetupParkingLotPopup(PopupType type, ParkingLot lot)
    {
        var currentPopup = (ParkingLotPopup)_allPopups.FirstOrDefault(x => x.PopupType == type);
        if (currentPopup == null)
        {
            this.Log("Show Popup => Popup doesn't exist! + " + type);
            return;
        }

        currentPopup.SetupParkingLotPopup(lot);
    }

    public void ShowPopup(PopupType type, bool hideOthers, Action callback = null, Hashtable parameters = null,
        bool waitOtherPopupsToClose = false)
    {

        if (IsPopupActive(type))
        {
            return;
        }

        var currentPopup = _allPopups.FirstOrDefault(x => x.PopupType == type);
        if (currentPopup == null)
        {
            this.Log("Show Popup => Popup doesn't exist! + " + type);
            return;
        }

        if (waitOtherPopupsToClose)
        {
            if (_popupQueue.Count != 0)
            {
                _waitingPopups.Enqueue(currentPopup);
                return;
            }
        }

        if (hideOthers)
        {
            for (var ii = 0; ii < _popupQueue.Count; ii++)
            {
                var activePopup = _popupQueue.Dequeue();
                if (activePopup != null)
                {
                    HidePopup(activePopup.PopupType);
                    if (_popupHideEvents.ContainsKey(activePopup.PopupType))
                    {
                        _popupHideEvents[activePopup.PopupType].SafeInvoke();
                    }
                }
            }
        }

        _popupQueue.Enqueue(currentPopup);

        if (_popupQueue.Count == 1)
        {
            Camera.main.GetComponent<CameraMovement>().enabled = false;
            Camera.main.GetComponent<CameraZoomInOut>().enabled = false;
        }

        var childIndex = currentPopup.transform.GetSiblingIndex();
        _dimmer.Fade(0.9f);
        _dimmer.transform.SetSiblingIndex((childIndex - 1) < 0 ? 0 : childIndex - 1);
        if (_popupShowEvents.ContainsKey(type))
        {
            _popupShowEvents[type].SafeInvoke();
        }

        currentPopup.AnalyticsEvent();
        currentPopup.Show(callback, parameters);
        AnyPopupOpened.SafeInvoke(currentPopup.PopupType);
    }

    public void SlidePopup(PopupType type, bool hideOthers, Action callback = null, Hashtable parameters = null,
        bool waitOtherPopupsToClose = false, int slideDir = 0)
    {
        if (IsPopupActive(type))
        {
            return;
        }

        var currentPopup = _allPopups.FirstOrDefault(x => x.PopupType == type);
        if (currentPopup == null)
        {
            this.Log("Show Popup => Popup doesn't exist! + " + type);
            return;
        }

        if (waitOtherPopupsToClose)
        {
            if (_popupQueue.Count != 0)
            {
                _waitingPopups.Enqueue(currentPopup);
                return;
            }
        }

        if (hideOthers)
        {
            for (var ii = 0; ii < _popupQueue.Count; ii++)
            {
                var activePopup = _popupQueue.Dequeue();
                if (activePopup != null)
                {
                    HideWithoutAnimPopup(activePopup.PopupType, null, slideDir);
                    if (_popupHideEvents.ContainsKey(activePopup.PopupType))
                    {
                        _popupHideEvents[activePopup.PopupType].SafeInvoke();
                    }
                }
            }
        }

        _popupQueue.Enqueue(currentPopup);
        var childIndex = currentPopup.transform.GetSiblingIndex();
        _dimmer.Fade(0.9f);
        _dimmer.transform.SetSiblingIndex((childIndex - 1) < 0 ? 0 : childIndex - 1);
        if (_popupShowEvents.ContainsKey(type))
        {
            _popupShowEvents[type].SafeInvoke();
        }

        currentPopup.AnalyticsEvent();
        currentPopup.SlideFrom(callback, parameters, slideDir);

        AnyPopupOpened.SafeInvoke(currentPopup.PopupType);
    }

    public void HidePopup(PopupType type, Action callback = null)
    {
        WaitForAllPopupClosed(() =>
        {
            Camera.main.GetComponent<CameraMovement>().enabled = true;
            Camera.main.GetComponent<CameraZoomInOut>().enabled = true;
        });

        var currentPopup = _allPopups.FirstOrDefault(x => x.PopupType == type);
        if (currentPopup == null)
        {
            this.Log("Hide Popup => Popup doesn't exist! + " + type);
            return;
        }

        for (int i = 0; i < _popupQueue.Count; i++)
        {
            if (i != _popupQueue.Count - 1)
            {
                var item = _popupQueue.Dequeue();
                if (item != null)
                {
                    _dimmer.transform.SetSiblingIndex((item.transform.GetSiblingIndex() - 1) < 0
                        ? 0
                        : item.transform.GetSiblingIndex() - 1);
                    _dimmer.Fade(0.9f);
                }

                _popupQueue.Enqueue(item);
            }
            else
            {
                _popupQueue.Dequeue();
            }
        }

        if (_popupHideEvents.ContainsKey(currentPopup.PopupType))
        {
            currentPopup.Hide(_popupHideEvents[currentPopup.PopupType]);
        }
        else
        {
            currentPopup.Hide(callback);
        }

        if (_popupQueue.Count == 0)
        {
            _popupQueue = new Queue<PopupBase>();
            if (_waitingPopups.Count == 0)
            {
                _dimmer.Fade(0);
            }
            else
            {
                var nextPopup = _waitingPopups.Dequeue();
                ShowPopup(nextPopup.PopupType, true);
            }
        }
    }

    public void HideWithoutAnimPopup(PopupType type, Action callback = null, int slideDir = 0)
    {
        var currentPopup = _allPopups.FirstOrDefault(x => x.PopupType == type);
        if (currentPopup == null)
        {
            this.Log("Hide Popup => Popup doesn't exist! + " + type);
            return;
        }

        for (int i = 0; i < _popupQueue.Count; i++)
        {
            if (i != _popupQueue.Count - 1)
            {
                var item = _popupQueue.Dequeue();
                if (item != null)
                {
                    _dimmer.transform.SetSiblingIndex((item.transform.GetSiblingIndex() - 1) < 0
                        ? 0
                        : item.transform.GetSiblingIndex() - 1);
                    _dimmer.Fade(0.9f);
                }

                _popupQueue.Enqueue(item);
            }
            else
            {
                _popupQueue.Dequeue();
            }
        }

        if (_popupHideEvents.ContainsKey(currentPopup.PopupType))
        {
            currentPopup.HideWithoutAnim(_popupHideEvents[currentPopup.PopupType], slideDir);
        }
        else
        {
            currentPopup.HideWithoutAnim(callback, slideDir);
        }

        if (_popupQueue.Count == 0)
        {
            _popupQueue = new Queue<PopupBase>();
            if (_waitingPopups.Count == 0)
            {
                _dimmer.Fade(0);
            }
            else
            {
                var nextPopup = _waitingPopups.Dequeue();
                ShowPopup(nextPopup.PopupType, true);
            }
        }
    }

    private IEnumerator WaitForAllPopup(Action callback)
    {
        yield return new WaitForSeconds(0.02f);
        yield return new WaitUntil(() => _popupQueue.Count == 0 && _waitingPopups.Count == 0);
        callback.SafeInvoke();
    }

    public void WaitForAllPopupClosed(Action callback)
    {
        StartCoroutine(WaitForAllPopup(callback));
    }

    public void AddHideListener(PopupType popupType, Action hideCallback)
    {
        if (_popupHideEvents.ContainsKey(popupType))
        {
            _popupHideEvents[popupType] += hideCallback;
            return;
        }

        _popupHideEvents.Add(popupType, hideCallback);
    }

    public void RemoveAllHideListener(PopupType popupType)
    {
        if (_popupHideEvents.ContainsKey(popupType))
        {
            _popupHideEvents[popupType] = null;
        }
    }

    public void RemoveHideListener(PopupType popupType, Action hideCallback)
    {
        if (_popupHideEvents.ContainsKey(popupType))
        {
            _popupHideEvents[popupType] -= hideCallback;
        }
    }

    public void AddShowListener(PopupType popupType, Action showCallback)
    {
        if (_popupShowEvents.ContainsKey(popupType))
        {
            _popupShowEvents[popupType] += showCallback;
            return;
        }

        _popupShowEvents.Add(popupType, showCallback);
    }

    public void RemoveShowListener(PopupType popupType, Action showCallback)
    {
        if (_popupShowEvents.ContainsKey(popupType))
        {
            _popupShowEvents[popupType] -= showCallback;
        }
    }

    #endregion
}