using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CloseButton : ButtonBase
{
    [Inject] private PopupController _popupController;
    private PopupSystem.PopupBase _popUp;


    private void OnEnable()
    {       
        OnClickEvent = CloseShopPopUp;
    }

    public void CloseShopPopUp()
    {
        _popUp = transform.parent.GetComponent<PopupSystem.PopupBase>();

        _popupController.HidePopup(_popUp.PopupType, () =>
        {
        });
    }
}
