using System;
using System.Collections;
using DG.Tweening;
using Helpers.Utils;
using UnityEngine;
using Zenject;

namespace PopupSystem
{
    public class PopupBase : MonoBehaviour
    {
//        [Inject] private readonly AnalyticsManager _analyticsManager;

        [SerializeField] protected RectTransform _backgroundObject;
        
        protected PopupController _popupController;
        
        public virtual PopupType PopupType
        {
            get { return PopupType.None; }
        }

        public bool IsActive;
        public bool IsHideAnimationStarted;
        
        protected float HideDuration = 0.2f;
        protected Sequence _animationTween;

        private float _firstPos;
        private float _firstScale;

        protected virtual Ease _showEaseType
        {
            get { return Ease.OutBack; }
        }
        protected virtual Ease _hideEaseType
        {
            get { return Ease.InBack; }
        }
        
        [Inject]
        private void Installer(PopupController popupController)
        {
            _popupController = popupController;
        }
        
        public virtual void OnInitialize()
        {
            _firstPos = transform.localPosition.x;
            _firstScale = transform.localScale.x;
        }
        
        public void AnalyticsEvent()
        {
            string stringValue = Enum.GetName(typeof(PopupType), PopupType);
            
//            _analyticsManager.ScreenVisit(stringValue);
        }

        public virtual void Show(Action callback = null, Hashtable parameters = null)
        {

            if (_backgroundObject != null)
            {
                _backgroundObject.sizeDelta = new Vector2(GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.width, GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.height);
            }
            
            IsHideAnimationStarted = false;
            transform.localScale = Vector3.zero;
            transform.localPosition = Vector3.zero;

            if (_animationTween != null)
            {
                _animationTween.Kill();
            }

            gameObject.SetActive(true);
            _animationTween = DOTween.Sequence();
            _animationTween.Append(
                    transform.DOScale(Vector3.one * _firstScale, HideDuration)
                    .SetSpeedBased(true)
                    .OnStart(() => gameObject.SetActive(true)))
                .OnComplete(() =>
                {
                    IsActive = true;
                    callback.SafeInvoke();
                });
        }

        public virtual void Hide(Action callback = null)
        {
        
            if (IsHideAnimationStarted)
                return;
            
            IsHideAnimationStarted = true;
            if (_animationTween != null)
            {
                _animationTween.Kill();
            }

            _animationTween = DOTween.Sequence();
            _animationTween
                .Append(transform.DOScale(Vector3.zero, HideDuration).SetEase(_hideEaseType))
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    IsActive = false;
                    Resources.UnloadUnusedAssets();
                    callback.SafeInvoke();
                });
        }

        public virtual void HideWithoutAnim(Action callback = null, int slideDir = 0)
        {
            if (IsHideAnimationStarted)
                return;

            IsHideAnimationStarted = true;
            if (_animationTween != null)
            {
                _animationTween.Kill();
            }

            _animationTween = DOTween.Sequence();
            _animationTween
                .Append(transform.DOLocalMoveX((1100 * slideDir), HideDuration * 2f))
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    IsActive = false;
                    Resources.UnloadUnusedAssets();
                    callback.SafeInvoke();
                });
        }

        public virtual void SlideFrom(Action callback = null, Hashtable parameters = null, int slideDir = 0)
        {
            if (_backgroundObject != null)
            {
                _backgroundObject.sizeDelta = new Vector2(GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.width, GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.height);
            }

            IsHideAnimationStarted = false;
            transform.localScale = Vector3.one;

            var rectTransform = GetComponent<RectTransform>();
            if(slideDir == -1) { slideDir *= 2; }
            rectTransform.anchoredPosition = new Vector2(400 * slideDir, 0);

            if (_animationTween != null)
            {
                _animationTween.Kill();
            }

            gameObject.SetActive(true);
            _animationTween = DOTween.Sequence();
            _animationTween.Append(
                    transform.DOLocalMoveX(0, HideDuration * 2)
                    .SetSpeedBased(true)
                    .OnStart(() => gameObject.SetActive(true)))
                .OnComplete(() =>
                {
                    IsActive = true;
                    callback.SafeInvoke();
                });
        }
    }
}

