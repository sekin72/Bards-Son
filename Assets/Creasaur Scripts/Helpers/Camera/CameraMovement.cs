using System;
using System.Collections;
using System.Collections.Generic;
using TouchSystem;
using UnityEngine;
using Zenject;

public class CameraMovement : MonoBehaviour
{
    private TouchInteractor _touchInteractor;
    private float _speed = 10f;
    private Vector3 _movementXAxis, _movementYAxis;
    [SerializeField] private GameObject _plane;
    private Touch _touch;
    public bool IsMoving;
    private Vector2 _lastPosition;

    [Inject]
    public void Initializor(TouchInteractor touchInteractor)
    {
        var temp = transform.rotation;
        _movementXAxis = transform.right.normalized;
        transform.rotation = new Quaternion(0, temp.y, 0, temp.w);
        _movementYAxis = transform.forward.normalized;
        transform.rotation = temp;

        _touchInteractor = touchInteractor;
        _touchInteractor.OnTouchMovedEvent += (Touch t) =>
        {
            IsMoving = true;
            CameraMove(t);
        };

        _touchInteractor.OnTouchEndedEvent += (Touch t) =>
        {
            IsMoving = false;
        };
    }

    private void CameraMove(Touch t)
    {
        _touch = t;
    }

    private void FixedUpdate()
    {
        if (IsMoving)
        {
            var tempDelta = _touch.deltaPosition.normalized * 3;
            Vector3 TargetPosition = transform.position - tempDelta.x * _movementXAxis - tempDelta.y * _movementYAxis;
            transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime * _speed);
        }
    }

}
