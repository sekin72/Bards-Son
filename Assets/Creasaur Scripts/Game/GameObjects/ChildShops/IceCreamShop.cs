using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;

namespace IceCream
{
    public class IceCreamShop : Shop
    {
        protected void Awake()
        {
            ShopPopupObjects.ProductNames = GameRules.ShopIceCreamProducts.Values.ToArray();
            ShopPopupObjects.Sprites = _gameDataManager.GetAtlas("icecream", 1);
            ShopPopupObjects.LockedSprites = _gameDataManager.GetAtlas("icecream", 0);
            OpenPopUp = OpenShopPopUp;
            _waitingQueueDirection = Vector3.left;
            ParentStart();

            SetupPopup(PopupSystem.PopupType.IceCream);
        }

        protected void FixedUpdate()
        {
            ParentFixedUpdate();
        }

        public void OpenShopPopUp()
        {
            ParentOpenPopUp(PopupSystem.PopupType.IceCream);
        }
    }
}
