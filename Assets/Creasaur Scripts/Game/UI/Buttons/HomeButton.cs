using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HomeButton : ButtonBase
{
    [Inject] protected PopupController _popupController;

    private void OnEnable()
    {
        OnClickEvent = OpenCompanies;
    }

    private void OpenCompanies()
    {
        _popupController.ShowPopup(PopupSystem.PopupType.Companies,true);
    }
}
