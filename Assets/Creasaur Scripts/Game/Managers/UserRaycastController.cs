using System;
using System.Collections;
using System.Collections.Generic;
using Timer;
using TouchSystem;
using UnityEngine;
using Zenject;

public class UserRaycastController : MonoBehaviour
{
    [Inject] private PopupController _popupController;
    [Inject] private CameraMovement _cameraMovement;
    [Inject] private CreaCoroutine _creaCoroutine;
    private TouchInteractor _touchInteractor;
    private Action<Touch> _castRayFromScreenAction;

    [Inject]
    public void Initializor(TouchInteractor touchInteractor)
    {
        _touchInteractor = touchInteractor;

        _castRayFromScreenAction = CastRayFromScreen;

        _touchInteractor.OnTouchEndedEvent += (Touch t) =>
        {
            //_creaCoroutine.Timer(TimeSpan.FromSeconds(0.1), _castRayFromScreenAction, t);
            CastRayFromScreen(t);
        };
    }

    private void CastRayFromScreen(Touch t)
    {
        if (!_popupController.IsAnyPopupOpen())
        {
#if !UNITY_EDITOR
            if(!_cameraMovement.IsMoving)
            {            
                RaycastHit hit;
                int layerMask = LayerMask.GetMask("RayInteractor");
                Ray ray = Camera.main.ScreenPointToRay(t.position);
                if (Physics.Raycast(ray, out hit, layerMask) && hit.collider.GetComponent<Interactable>() != null)
                {
                    hit.collider.GetComponent<Interactable>().OnClicked();
                }
            }
#else
            RaycastHit hit;
            int layerMask = LayerMask.GetMask("RayInteractor");
            Ray ray = Camera.main.ScreenPointToRay(t.position);
            if (Physics.Raycast(ray, out hit, layerMask) && hit.collider.GetComponent<Interactable>() != null)
            {
                hit.collider.GetComponent<Interactable>().OnClicked();
            }
#endif
        }
    }
}
