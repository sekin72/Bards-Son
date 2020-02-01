using Assets.Scripts.Game.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;

public class ShopManager : MonoBehaviour
{
    [Inject] private UserDataManager _userDataManager;
    [Inject] GameDataManager _gameDataManager;
    public List<Shop> ShopGameObjectsList;

    public void Awake()
    {
        ShopGameObjectsList = GetComponentsInChildren<Shop>().ToList();
        _userDataManager.OnNewShopPurchased = AddNewShop;

        for (int i = 0; i < ShopGameObjectsList.Count; i++)
        {
            var shop = ShopGameObjectsList[i];
            if (i >= _userDataManager.UserData.ShopVariablesList.Count)
            {
                ShopGameObjectsList[i].gameObject.SetActive(false);
                continue;
            }

            shop.SetShopDataFromZero(_userDataManager.UserData.ShopVariablesList[i]);
        }
    }

    public void AddNewShop()
    {
        var i = _userDataManager.UserData.ShopVariablesList.Count - 1;

        var shop = ShopGameObjectsList[i];
        shop.SetShopDataFromZero(_userDataManager.UserData.ShopVariablesList[i]);
        ShopGameObjectsList[i] = shop;
        ShopGameObjectsList[i].gameObject.SetActive(true);
    }
}
