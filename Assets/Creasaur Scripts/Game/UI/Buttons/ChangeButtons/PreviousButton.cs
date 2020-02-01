using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PreviousButton : ChangeButton
{

    private void OnEnable()
    {
        OnClickEvent += PreviousShop;
    }

    private void PreviousShop()
    {
        var popupType = transform.parent.GetComponent<ShopPopup>().PopupType;

        //switch (popupType)
        //{
        //    case PopupSystem.PopupType.IceCream:
        //        _popupController.SlidePopup(popupType - 1, true, null, null, false, -1);
        //        break;

        //}

        /*
         *Two different methods, upper is Switch below is one trick 
         */

        _popupController.SlidePopup(popupType - 1, true, null, null, false, -1);
    }
}
