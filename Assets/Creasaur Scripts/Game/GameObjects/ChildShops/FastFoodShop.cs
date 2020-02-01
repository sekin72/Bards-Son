using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;

public class FastFoodShop : Shop
{
    protected void Awake()
    {
        ShopPopupObjects.ProductNames = GameRules.ShopFastFoodProducts.Values.ToArray();
        ShopPopupObjects.Sprites = _gameDataManager.GetAtlas("fastfood", 1);
        ShopPopupObjects.LockedSprites = _gameDataManager.GetAtlas("fastfood", 0);
        OpenPopUp = OpenShopPopUp;
        _waitingQueueDirection = Vector3.left;
        ParentStart();

        SetupPopup(PopupSystem.PopupType.FastFood);
    }

    protected void FixedUpdate()
    {
        ParentFixedUpdate();
    }

    public void OpenShopPopUp()
    {
        ParentOpenPopUp(PopupSystem.PopupType.FastFood);
    }

}