using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DiscoShop : Shop
{
    protected void Awake()
    {
        ShopPopupObjects.ProductNames = GameRules.ShopDiscoProducts.Values.ToArray();
        ShopPopupObjects.Sprites = _gameDataManager.GetAtlas("disco", 1);
        ShopPopupObjects.LockedSprites = _gameDataManager.GetAtlas("disco", 0);
        OpenPopUp = OpenShopPopUp;
        _waitingQueueDirection = Vector3.left;
        ParentStart();

        SetupPopup(PopupSystem.PopupType.Disco);
    }

    protected void FixedUpdate()
    {
        ParentFixedUpdate();
    }

    public void OpenShopPopUp()
    {
        ParentOpenPopUp(PopupSystem.PopupType.Disco);
    }

}