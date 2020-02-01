using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SushiShop : Shop
{
    protected void Awake()
    {
        ShopPopupObjects.ProductNames = GameRules.ShopSushiProducts.Values.ToArray();
        ShopPopupObjects.Sprites = _gameDataManager.GetAtlas("sushi", 1);
        ShopPopupObjects.LockedSprites = _gameDataManager.GetAtlas("sushi", 0);
        OpenPopUp = OpenShopPopUp;
        _waitingQueueDirection = Vector3.left;
        ParentStart();

        SetupPopup(PopupSystem.PopupType.Sushi);
    }

    protected void FixedUpdate()
    {
        ParentFixedUpdate();
    }

    public void OpenShopPopUp()
    {
        ParentOpenPopUp(PopupSystem.PopupType.Sushi);
    }

}