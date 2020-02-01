using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BuyShopButton : ButtonBase
{
    [Inject] protected UserDataManager _userDataManager;

    private void Awake()
    {
        OnClickEvent = BuyShop;
        Enable = false;
    }

    public void SetPressedReleasedSprites(Sprite pressed, Sprite released)
    {
        _pressedSprite = pressed;
        _releasedSprite = released;
        SetSprite(_releasedSprite, false);
    }

    public void PurchaseCompleted()
    {
        Enable = false;
    }

    public void BuyShop()
    {
        if (_userDataManager.CheckCurrencyGoldIsEnoughForPurchaseNewShop())
        {
            _userDataManager.PurchaseNewShop();
            PurchaseCompleted();
            GetComponentInParent<ButtonEnabler>().enabled = true;
        }
        else
        {
        }
    }
}
