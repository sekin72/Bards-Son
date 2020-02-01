using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Helpers.Utils;
using LogSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBase : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    public Action OnClickEvent;

    private bool _isPressed;
    private Tween _tween;

    public Transform AnimationObject;
    private float _firstScale;

    public bool Enable = true;

    protected Sprite _pressedSprite;
    protected Sprite _releasedSprite;

    private void Start()
    {
        if (AnimationObject != null)
        {
            _firstScale = AnimationObject.localScale.x;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Enable)
            return;

        if (_isPressed)
        {
            _isPressed = false;
            if (_tween != null)
            {
                _tween.Kill();
            }

            if (AnimationObject != null)
            {
                _tween = AnimationObject.DOScale(_firstScale, 0.2f);
            }
        }

        OnClickEvent.SafeInvoke();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Enable)
            return;

        _isPressed = true;
        if (_tween != null)
        {
            _tween.Kill();
        }

        if (AnimationObject != null)
        {
            _tween = AnimationObject.DOScale(_firstScale * 0.9f, 0.2f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isPressed)
        {

            _isPressed = false;
            if (_tween != null)
            {
                _tween.Kill();
            }

            if (AnimationObject != null)
            {
                _tween = AnimationObject.DOScale(_firstScale, 0.2f);
            }
        }
    }

    internal void OnPointerClick(object e)
    {
        throw new NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!Enable)
            return;
        if (_releasedSprite != null)
        {
            SetSprite(_releasedSprite, false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Enable)
            return;

        if (_pressedSprite != null)
        {
            SetSprite(_pressedSprite, false);
        }
    }

    public void SetSprite(Sprite sprite, bool nativeSize)
    {
        var Image = GetComponent<Image>();
        Image.sprite = sprite;
        if (nativeSize)
        {
            Image.SetNativeSize();
        }
    }
}
