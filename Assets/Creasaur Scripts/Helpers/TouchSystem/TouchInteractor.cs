using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TouchSystem
{
    public class TouchInteractor : MonoBehaviour
    {
        #region Variables

        public virtual int TouchCount
        {
            get { return Input.touchCount; }
        }

        public virtual Touch[] Touches
        {
            get { return Input.touches; }
        }

        public bool IsZooming;
        private bool _isSwiping;
        private bool _isSwipeEnded = true;
        private bool _eventSend;
        private Vector2 _lastPosition;    // When swipe is start, it gives first position of finger

        #endregion

        #region TouchActions

        public Action<float> OnZoomEvent;
        public Action<Touch> OnTouchBeginEvent;
        public Action<Touch> OnTouchMovedEvent;
        public Action<Touch> OnTouchStationaryEvent;
        public Action<Touch> OnTouchEndedEvent;
        public Action<Touch> OnTouchCanceledEvent;

        public Action<SwipeDirection> OnSwipeEvent;

        #endregion

        public virtual Touch GetTouch(int index)
        {
            return Input.GetTouch(0);
        }

        #region EventMethods

        private float _zoomModifier;

        private void OnZoom(Touch[] touches)
        {
            if (OnTouchBeginEvent != null)
            {
                IsZooming = true;

                Touch firstTouch = touches[0];
                Touch secondTouch = touches[1];

                var firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
                var secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

                var touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
                var touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

                _zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude;

                if (touchesPrevPosDifference < touchesCurPosDifference)
                    _zoomModifier = -_zoomModifier;

                if (_zoomModifier >= 1f)
                {
                    OnZoomEvent(_zoomModifier);
                }
            }
        }

        private void OnTouchBegin(Touch touch)
        {
            if (OnTouchBeginEvent != null)
            {
                OnTouchBeginEvent(touch);
            }
        }

        private void OnTouchMoved(Touch touch)
        {
            if (OnTouchMovedEvent != null)
            {
                OnTouchMovedEvent(touch);
            }
        }

        private void OnTouchStationary(Touch touch)
        {
            if (OnTouchStationaryEvent != null)
            {
                OnTouchStationaryEvent(touch);
            }
        }

        private void OnTouchEnded(Touch touch)
        {
            if (OnTouchEndedEvent != null)
            {
                OnTouchEndedEvent(touch);
            }
        }

        private void OnTouchCanceled(Touch touch)
        {
            if (OnTouchCanceledEvent != null)
            {
                OnTouchCanceledEvent(touch);
            }
        }

        #endregion

        #region UnityEvents

        //private void Awake()
        //{
        //    Input.multiTouchEnabled = false;
        //}

        protected virtual void Update()
        {
            if (TouchCount == 0)
            {
                IsZooming = false;
                return;
            }

            if (TouchCount == 2)
            {
                OnZoom(Touches);
            }

            // Process Touch Events
            if (!IsZooming)
            {
                TouchEvents(Touches);

                // Process Swipe Events
                SwipeEvents();
            }

        }

        #endregion

        #region PublicMethods

        public TouchPhase GetTouchState(int index)
        {
            if (index <= TouchCount - 1)
            {
                var touch = GetTouch(index);
                return touch.phase;
            }

            return TouchPhase.Canceled;
        }

        #endregion
        #region PrivateMethods

        private void TouchEvents(Touch[] touches)
        {
            foreach (var touch in touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        OnTouchBegin(touch);
                        break;
                    case TouchPhase.Moved:
                        OnTouchMoved(touch);
                        break;
                    case TouchPhase.Stationary:
                        OnTouchStationary(touch);
                        break;
                    case TouchPhase.Ended:
                        _isSwipeEnded = true;
                        OnTouchEnded(touch);
                        break;
                    case TouchPhase.Canceled:
                        _isSwipeEnded = true;
                        OnTouchCanceled(touch);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void SwipeEvents()
        {
            if (!_isSwipeEnded)
            {
                return;
            }

            var _firstTouch = GetTouch(0);
            //#if !UNITY_EDITOR      
            //#endif
            //            var touchPointerData = new PointerEventData(EventSystem.current);
            //            touchPointerData.position = _firstTouch.position;
            //            var uiHits = new List<RaycastResult>();
            //            EventSystem.current.RaycastAll(touchPointerData,  uiHits);
            //            uiHits = uiHits.Where(x => x.gameObject.GetComponent<ButtonBase>()).ToList();
            //            
            //            foreach (var uiHit in uiHits)
            //            {
            //                uiHit.gameObject.GetComponent<ButtonBase>().Disable = true;
            //            }

            if (Math.Abs(_firstTouch.deltaPosition.sqrMagnitude) < 80f)
            {
                _isSwiping = false;
                _eventSend = false;
                return;
            }

            if (!_isSwiping)
            {
                _isSwiping = true;
                _lastPosition = _firstTouch.position;
                return;
            }
            else
            {
                if (_eventSend || OnSwipeEvent == null)
                {
                    return;
                }

                var direction = _firstTouch.position - _lastPosition;
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    OnSwipeEvent(direction.x > 0 ? SwipeDirection.Right : SwipeDirection.Left);
                }
                else
                {
                    OnSwipeEvent(direction.y > 0 ? SwipeDirection.Up : SwipeDirection.Down);
                }
                _eventSend = true;
                _isSwipeEnded = false;
            }
        }

        #endregion
    }
}
