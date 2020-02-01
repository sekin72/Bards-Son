using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;

namespace Drink
{
    public class DrinkShop : Shop
    {
        protected void Awake()
        {
            ShopPopupObjects.ProductNames = GameRules.ShopDrinkProducts.Values.ToArray();
            ShopPopupObjects.Sprites = _gameDataManager.GetAtlas("softdrink", 1);
            ShopPopupObjects.LockedSprites = _gameDataManager.GetAtlas("softdrink", 0);
            OpenPopUp = OpenShopPopUp;
            _waitingQueueDirection = Vector3.left;
            ParentStart();

            SetupPopup(PopupSystem.PopupType.Drink);
        }

        protected void FixedUpdate()
        {
            ParentFixedUpdate();
        }

        public void OpenShopPopUp()
        {
            ParentOpenPopUp(PopupSystem.PopupType.Drink);
        }

    }
}