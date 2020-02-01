using System.Collections;
using System.Collections.Generic;
using TouchSystem;
using UnityEngine;
using Zenject;

public class CameraZoomInOut : MonoBehaviour
{
    private TouchInteractor _touchInteractor;
    private Camera _mainCamera;
    [SerializeField] private float _zoomModifierSpeed = 0.1f;

    [Inject]
    public void Initializor(TouchInteractor touchInteractor)
    {
        _touchInteractor = touchInteractor;
        _mainCamera = GetComponent<Camera>();

        _touchInteractor.OnZoomEvent += (float zoomModifier) =>
        {
            ZoomInOrOut(zoomModifier);
        };
    }

    private void ZoomInOrOut(float zoomModifier)
    {
        _touchInteractor.IsZooming = true;
        _mainCamera.orthographicSize += zoomModifier * _zoomModifierSpeed;
        _mainCamera.orthographicSize = Mathf.Clamp(_mainCamera.orthographicSize, 2f, 10f);
    }
}
