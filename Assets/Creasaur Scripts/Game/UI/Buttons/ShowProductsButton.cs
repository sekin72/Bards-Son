using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ShowProductsButton : ButtonBase
{

    [Inject] private PopupController _popupController;   

    private Hashtable hashTable;

    private void OnEnable()
    {
        OnClickEvent = OpenProductsPopUp;
    }

    public void OpenProductsPopUp()
    {
        var shopPopup = transform.parent.GetComponent<ShopPopup>();
        var productSprites = shopPopup.ProductSprite;
        var lockedProductSprites = shopPopup.LockedProductSprite;
        var productNames = shopPopup.ProductNames;

        hashTable = new Hashtable
        {
            {"Sprites", productSprites},
            {"LockedSprites", lockedProductSprites },
            {"ProductNames", productNames },
            {"ProductIndex", shopPopup.AvailableProductIndex }
        };

        _popupController.ShowPopup(PopupSystem.PopupType.Products, false, () =>
        {

           
        }, hashTable);
    }
}
