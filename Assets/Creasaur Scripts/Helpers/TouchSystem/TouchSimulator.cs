using UnityEngine;

namespace TouchSystem
{
    public class TouchSimulator : TouchInteractor
    {
        private Touch _tempTouch;

        public override int TouchCount
        {
            get { return 1; }
        }

        public override Touch[] Touches
        {
            get
            {
                var touches = new Touch[1];
                touches[0] = _tempTouch;
                return touches;
            }
        }

        #region UnityEvents

        private void Start()
        {
            //Initialize
            _tempTouch = new Touch
            {
                fingerId = 0,
                phase = TouchPhase.Canceled
            };
//            OnTouchBeginEvent += delegate(Touch touch) 
//            {  
//                Debug.Log("Touch Begin");
//            };
//            
//            OnTouchEndedEvent += delegate(Touch touch)
//            {
//                Debug.Log("Touch Ended");
//            };
//            
//            OnTouchMovedEvent += delegate(Touch touch)
//            {
//                Debug.Log("Touch moved");
//            };
//
//            OnSwipeEvent += direction =>
//            {
//                Debug.Log("Swipe => " + direction);
//            };
        }

        protected override void Update()
        {
            SimulateTouch(ref _tempTouch);
            base.Update();
        }

        private void LateUpdate()
        {
            ResetTouch(ref _tempTouch);
        }

        #endregion

        #region TouchEvents

        public override Touch GetTouch(int index)
        {
            return _tempTouch;
        }

        #endregion

        private void ResetTouch(ref Touch touch)
        {
            touch.phase = TouchPhase.Canceled;
        }
        
        private void SimulateTouch(ref Touch touch)
        {
            var mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            
            if (Input.GetMouseButtonDown(0))
            {
                touch.phase = TouchPhase.Began;
                touch.position = mousePosition;
                touch.deltaPosition = Vector2.zero;
            }
            else if(Input.GetMouseButtonUp(0))
            {
                touch.phase = TouchPhase.Ended;
                touch.position = mousePosition;
                touch.deltaPosition = Vector2.zero;
            }
            else if(Input.GetMouseButton(0))
            {
                touch.phase = TouchPhase.Moved;
                touch.deltaPosition = mousePosition - touch.position;
                touch.position = mousePosition;
            }
        }
    }
}