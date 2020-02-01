using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RestaurantShop : Shop
{
    protected void Awake()
    {
        ShopPopupObjects.ProductNames = GameRules.ShopRestaurantProducts.Values.ToArray();
        ShopPopupObjects.Sprites = _gameDataManager.GetAtlas("restaurant", 1);
        ShopPopupObjects.LockedSprites = _gameDataManager.GetAtlas("restaurant", 0);
        OpenPopUp = OpenShopPopUp;
        _waitingQueueDirection = Vector3.left;
        ParentStart();

        SetupPopup(PopupSystem.PopupType.Restaurant);
    }

    protected void FixedUpdate()
    {
        ParentFixedUpdate();
    }

    public void OpenShopPopUp()
    {
        ParentOpenPopUp(PopupSystem.PopupType.Restaurant);
    }

}