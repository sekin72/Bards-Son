using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NextButton : ChangeButton
{
    private void OnEnable()
    {
        OnClickEvent += NextShop;
    }

    private void NextShop()
    {
        var popupType = transform.parent.GetComponent<ShopPopup>().PopupType;

        //switch (popupType)
        //{
        //    case PopupSystem.PopupType.Drink:
        //        _popupController.SlidePopup(popupType + 1, true, null, null, false, 1);
        //        break;
        //}

        /*
         * Same thing for the Next Button
         */
        _popupController.SlidePopup(popupType + 1, true, null, null, false, 1);



    }
}
