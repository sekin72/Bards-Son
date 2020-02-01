using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SeasideShop : Shop
{
    protected void Awake()
    {
        ShopPopupObjects.ProductNames = GameRules.ShopSeasideProducts.Values.ToArray();
        ShopPopupObjects.Sprites = _gameDataManager.GetAtlas("seasideshop", 1);
        ShopPopupObjects.LockedSprites = _gameDataManager.GetAtlas("seasideshop", 0);
        OpenPopUp = OpenShopPopUp;
        _waitingQueueDirection = Vector3.left;
        ParentStart();

        SetupPopup(PopupSystem.PopupType.Seaside);
    }

    protected void FixedUpdate()
    {
        ParentFixedUpdate();
    }

    public void OpenShopPopUp()
    {
        ParentOpenPopUp(PopupSystem.PopupType.Seaside);
    }

}