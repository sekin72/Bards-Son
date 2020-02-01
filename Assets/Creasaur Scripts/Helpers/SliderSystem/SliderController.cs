using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helpers.Utils;
using LogSystem;
using PopupSystem;
using UnityEngine;
using Zenject;

public class SliderController : MonoBehaviour
{
    [SerializeField] private List<PopupBase> _allSlider;

    private Queue<PopupBase> _sliderQueue;
    private Queue<PopupBase> _waitingSliders;

    private Dimmer _dimmer;

    #region PrivateMethods

    private Dictionary<PopupType, Action> _sliderHideEvents;
    private Dictionary<PopupType, Action> _sliderShowEvents;

    [SerializeField] private Transform _sliderParents;

    public Action<PopupType> AnSliderOpened;

    [Inject]
    private void OnInitialize()
    {
        _allSlider = new List<PopupBase>();
        _sliderQueue = new Queue<PopupBase>();
        _waitingSliders = new Queue<PopupBase>();

        _sliderHideEvents = new Dictionary<PopupType, Action>();
        _sliderShowEvents = new Dictionary<PopupType, Action>();

        for (var ii = 0; ii < _sliderParents.childCount; ii++)
        {
            var child = _sliderParents.GetChild(ii);
            var popupComponent = child.GetComponent<PopupBase>();
            if (popupComponent != null)
            {
                child.gameObject.SetActive(false);
                _allSlider.Add(child.GetComponent<PopupBase>());
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
        _dimmer.transform.SetParent(_sliderParents);
        _dimmer.transform.SetSiblingIndex(0);
    }

    #endregion

    #region PublicMethods

    public T GetPopup<T>(PopupType popupType)
    {
        return _allSlider.FirstOrDefault(x => x.PopupType == popupType).GetComponent<T>();
    }

    public bool IsPopupActive(PopupType popupType)
    {
        return _sliderQueue.FirstOrDefault(x => x.PopupType == popupType) != null;
    }

    public bool IsAnyPopupOpen()
    {
        return _allSlider.Count(x => x.gameObject.activeSelf) != 0;
    }

    public void ShowPopup(PopupType type, bool hideOthers, Action callback = null, Hashtable parameters = null,
        bool waitOtherPopupsToClose = false)
    {
        if (IsPopupActive(type))
        {
            return;
        }

        var currentPopup = _allSlider.FirstOrDefault(x => x.PopupType == type);
        if (currentPopup == null)
        {
            this.Log("Show Popup => Popup doesn't exist! + " + type);
            return;
        }

        if (waitOtherPopupsToClose)
        {
            if (_sliderQueue.Count != 0)
            {
                _waitingSliders.Enqueue(currentPopup);
                return;
            }
        }

        if (hideOthers)
        {
            for (var ii = 0; ii < _sliderQueue.Count; ii++)
            {
                var activePopup = _sliderQueue.Dequeue();
                if (activePopup != null)
                {
                    HidePopup(activePopup.PopupType);
                    if (_sliderHideEvents.ContainsKey(activePopup.PopupType))
                    {
                        _sliderHideEvents[activePopup.PopupType].SafeInvoke();
                    }
                }
            }
        }

        _sliderQueue.Enqueue(currentPopup);
        var childIndex = currentPopup.transform.GetSiblingIndex();
        _dimmer.Fade(0.9f);
        _dimmer.transform.SetSiblingIndex((childIndex - 1) < 0 ? 0 : childIndex - 1);
        if (_sliderShowEvents.ContainsKey(type))
        {
            _sliderShowEvents[type].SafeInvoke();
        }

        currentPopup.AnalyticsEvent();
        currentPopup.Show(callback, parameters);
        AnSliderOpened.SafeInvoke(currentPopup.PopupType);
    }

    public void HidePopup(PopupType type, Action callback = null)
    {
        var currentPopup = _allSlider.FirstOrDefault(x => x.PopupType == type);
        if (currentPopup == null)
        {
            this.Log("Hide Popup => Popup doesn't exist! + " + type);
            return;
        }

        for (int i = 0; i < _sliderQueue.Count; i++)
        {
            if (i != _sliderQueue.Count - 1)
            {
                var item = _sliderQueue.Dequeue();
                if (item != null)
                {
                    _dimmer.transform.SetSiblingIndex((item.transform.GetSiblingIndex() - 1) < 0
                        ? 0
                        : item.transform.GetSiblingIndex() - 1);
                    _dimmer.Fade(0.9f);
                }

                _sliderQueue.Enqueue(item);
            }
            else
            {
                _sliderQueue.Dequeue();
            }
        }

        if (_sliderHideEvents.ContainsKey(currentPopup.PopupType))
        {
            currentPopup.Hide(_sliderHideEvents[currentPopup.PopupType]);
        }
        else
        {
            currentPopup.Hide(callback);
        }

        if (_sliderQueue.Count == 0)
        {
            _sliderQueue = new Queue<PopupBase>();
            if (_waitingSliders.Count == 0)
            {
                _dimmer.Fade(0);
            }
            else
            {
                var nextPopup = _waitingSliders.Dequeue();
                ShowPopup(nextPopup.PopupType, true);
            }
        }
    }

    private IEnumerator WaitForAllPopup(Action callback)
    {
        yield return new WaitForSeconds(0.02f);
        yield return new WaitUntil(() => _sliderQueue.Count == 0 && _waitingSliders.Count == 0);
        callback.SafeInvoke();
    }

    public void WaitForAllPopupClosed(Action callback)
    {
        StartCoroutine(WaitForAllPopup(callback));
    }

    public void AddHideListener(PopupType popupType, Action hideCallback)
    {
        if (_sliderHideEvents.ContainsKey(popupType))
        {
            _sliderHideEvents[popupType] += hideCallback;
            return;
        }

        _sliderHideEvents.Add(popupType, hideCallback);
    }

    public void RemoveAllHideListener(PopupType popupType)
    {
        if (_sliderHideEvents.ContainsKey(popupType))
        {
            _sliderHideEvents[popupType] = null;
        }
    }

    public void RemoveHideListener(PopupType popupType, Action hideCallback)
    {
        if (_sliderHideEvents.ContainsKey(popupType))
        {
            _sliderHideEvents[popupType] -= hideCallback;
        }
    }

    public void AddShowListener(PopupType popupType, Action showCallback)
    {
        if (_sliderShowEvents.ContainsKey(popupType))
        {
            _sliderShowEvents[popupType] += showCallback;
            return;
        }

        _sliderShowEvents.Add(popupType, showCallback);
    }

    public void RemoveShowListener(PopupType popupType, Action showCallback)
    {
        if (_sliderShowEvents.ContainsKey(popupType))
        {
            _sliderShowEvents[popupType] -= showCallback;
        }
    }

    #endregion
}