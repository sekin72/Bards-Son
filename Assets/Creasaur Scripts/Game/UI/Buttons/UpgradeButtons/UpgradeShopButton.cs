using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UpgradeShopButton : UpgradeButton
{
    private void OnEnable()
    {
        OnClickEvent = UpgradeShop;
    }

    public void UpgradeShop()
    {
        transform.parent.GetComponent<ShopPopup>().UpgradeShop();
    }
}
