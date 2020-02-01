using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ClubShop : Shop
{
    protected void Awake()
    {
        ShopPopupObjects.ProductNames = GameRules.ShopClubProducts.Values.ToArray();
        ShopPopupObjects.Sprites = _gameDataManager.GetAtlas("club", 1);
        ShopPopupObjects.LockedSprites = _gameDataManager.GetAtlas("club", 0);
        OpenPopUp = OpenShopPopUp;
        _waitingQueueDirection = Vector3.left;
        ParentStart();

        SetupPopup(PopupSystem.PopupType.Club);
    }

    protected void FixedUpdate()
    {
        ParentFixedUpdate();
    }

    public void OpenShopPopUp()
    {
        ParentOpenPopUp(PopupSystem.PopupType.Club);
    }

}