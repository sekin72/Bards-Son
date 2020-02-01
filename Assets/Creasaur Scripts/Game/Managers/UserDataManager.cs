using Assets.Scripts.Game.SceneChangers;
using Helpers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TouchSystem;
using UnityEngine;
using Zenject;

public class UserDataManager
{
    public UserData UserData;
    private Action OnUserLoaded;
    private PrefsManager _prefsManager;
    public Action OnUserDataLoaded;
    public Action OnCurrencyGoldChanged;
    public Action OnNewShopPurchased;

    public UserDataManager(PrefsManager pref)
    {
        _prefsManager = pref;
    }

    public void OnInitialize()
    {
        Register();
        if (!_prefsManager.HasUserData())
        {
            Register();
        }
        else
        {
            UserData = LoadLocalData();
        }

        if (UserData != null)
        {
            OnUserDataLoaded.SafeInvoke();
        }
        else
        {
            Debug.Log("User data couldn't be load.");
        }
    }

    public UserData LoadLocalData()
    {
        return _prefsManager.GetUserData();
    }

    public void SaveLocalData()
    {
        _prefsManager.SaveUserData(UserData);
    }

    public void Register()
    {
        UserData = new UserData();
        UserData.NewUser();
        SaveLocalData();
    }


    #region Shop Related Functions

    private int GetIndexOfShop(Shop shop)
    {
        return UserData.ShopVariablesList.IndexOf(UserData.ShopVariablesList.FirstOrDefault(x => x.ShopType == shop.ShopVariables.ShopType));
    }

    public void UpgradeShop(Shop shop)
    {
        var shopVariable = UserData.ShopVariablesList.FirstOrDefault(x => x.ShopType == shop.ShopVariables.ShopType);
        if (CheckCurrencyGoldIsEnoughForUpgradeShop(shopVariable))
        {
            shopVariable.CurrentLevel++;
            UserData.ShopVariablesList[GetIndexOfShop(shop)] = shopVariable;
            DecreaseCurrencyGoldByShopUpgrades(shopVariable);
            SaveLocalData();
            shop.SetShopDataUpdateLevel(shopVariable);
        }
    }

    public void UpgradeSalary(Shop shop)
    {
        var shopVariable = UserData.ShopVariablesList.FirstOrDefault(x => x.ShopType == shop.ShopVariables.ShopType);
        if (CheckCurrencyGoldIsEnoughForUpgradeSalary(shopVariable))
        {
            shopVariable.SalaryLevel++;
            UserData.ShopVariablesList[GetIndexOfShop(shop)] = shopVariable;
            DecreaseCurrencyGoldBySalaryUpgrades(shopVariable);
            SaveLocalData();
            shop.SetShopDataUpdateSalary(shopVariable);
        }
    }

    public void HireNewEmployee(Shop shop)
    {
        var shopVariable = UserData.ShopVariablesList.FirstOrDefault(x => x.ShopType == shop.ShopVariables.ShopType);
        if (CheckCurrencyGoldIsEnoughForHireNewEmployee(shopVariable))
        {
            shopVariable.CurrentEmployeeCount++;
            UserData.ShopVariablesList[GetIndexOfShop(shop)] = shopVariable;
            DecreaseCurrencyGoldByHiringNewEmp(shopVariable);
            SaveLocalData();
            shop.SetShopDataHireEmp(shopVariable);
        }
    }
    public void PurchaseNewShop()
    {
        if (CheckCurrencyGoldIsEnoughForPurchaseNewShop())
        {
            var i = UserData.ShopVariablesList.Count;
            ShopType type;
            Enum.TryParse<ShopType>(i.ToString(), out type);
            UserData.ShopVariablesList.Add(new ShopVariables(1, type, 1, 1));

            OnNewShopPurchased.SafeInvoke();

            ChangeCurrencyGold(-GameRules.PurchaseCosts.Values.ElementAt(UserData.ShopVariablesList.Count() - 1));
        }
    }

    #endregion

    #region Parking Lot Related Functions
    public void UpgradeParkingLot(ParkingLot lot)
    {
        if (CheckCurrencyGoldIsEnoughForUpgradeParkingLot())
        {
            UserData.ParkingLot.Level++;
            ChangeCurrencyGold(-GameRules.GetParkingLotUpgradeCost(UserData.ParkingLot.Level));
            SaveLocalData();
            lot.ParkingLotData = UserData.ParkingLot;
        }
    }

    public void UpgradeAdvertisingLevel(ParkingLot lot)
    {
        if (CheckCurrencyGoldIsEnoughForUpgradeAdvertisementLevel())
        {
            UserData.ParkingLot.AdvertisementLevel++;
            ChangeCurrencyGold(-2 * 2 ^ UserData.ParkingLot.AdvertisementLevel);
            SaveLocalData();
            lot.ParkingLotData = UserData.ParkingLot;
        }
    }
    #endregion

    #region Currency Functions
    public void DecreaseCurrencyGoldByShopUpgrades(ShopVariables shopData)
    {
        var moneyBefore = UserData.CurrencyGold;
        UserData.CurrencyGold -= GameRules.GetUpgradeShopCost(shopData.ShopType, shopData.CurrentLevel);
        SaveLocalData();
        OnCurrencyGoldChanged.SafeInvoke();
    }

    private void DecreaseCurrencyGoldBySalaryUpgrades(ShopVariables shopData)
    {
        var moneyBefore = UserData.CurrencyGold;
        UserData.CurrencyGold -= GameRules.GetSalaryCost(shopData.ShopType, shopData.SalaryLevel);
        SaveLocalData();
        OnCurrencyGoldChanged.SafeInvoke();
    }

    private void DecreaseCurrencyGoldByHiringNewEmp(ShopVariables shopData)
    {
        var moneyBefore = UserData.CurrencyGold;
        UserData.CurrencyGold -= GameRules.GetHireCost(shopData.ShopType, shopData.CurrentEmployeeCount);
        SaveLocalData();
        OnCurrencyGoldChanged.SafeInvoke();
    }

    public void ChangeCurrencyGold(double x)
    {
        var moneyBefore = UserData.CurrencyGold;
        UserData.CurrencyGold += x;
        SaveLocalData();
        OnCurrencyGoldChanged.SafeInvoke();
    }
    #endregion

    #region Checkers
    public bool CheckCurrencyGoldIsEnoughForUpgradeShop(ShopVariables shopData)
    {
        if (shopData.CurrentLevel != 1000 && UserData.CurrencyGold >= GameRules.GetUpgradeShopCost(shopData.ShopType, shopData.CurrentLevel + 1))
            return true;
        return false;

    }

    public bool CheckCurrencyGoldIsEnoughForUpgradeSalary(ShopVariables shopData)
    {
        if (shopData.SalaryLevel != 100 && UserData.CurrencyGold >= GameRules.GetSalaryCost(shopData.ShopType, shopData.CurrentLevel + 1))
            return true;
        return false;
    }

    public bool CheckCurrencyGoldIsEnoughForHireNewEmployee(ShopVariables shopData)
    {
        if (shopData.CurrentEmployeeCount != 5 && UserData.CurrencyGold >= GameRules.GetHireCost(shopData.ShopType, shopData.CurrentEmployeeCount + 1))
            return true;
        return false;
    }

    public bool CheckCurrencyGoldIsEnoughForUpgradeParkingLot()
    {
        if (UserData.ParkingLot.Level != 22 && UserData.CurrencyGold >= GameRules.GetParkingLotUpgradeCost(UserData.ParkingLot.Level + 1))
            return true;
        return false;
    }

    public bool CheckCurrencyGoldIsEnoughForUpgradeAdvertisementLevel()
    {
        if (UserData.ParkingLot.AdvertisementLevel != 10 && UserData.CurrencyGold >= GameRules.ParkingLotAdvertisingCost(UserData.ParkingLot.AdvertisementLevel))
            return true;
        return false;
    }

    public bool CheckCurrencyGoldIsEnoughForPurchaseNewShop()
    {
        if (UserData.ShopVariablesList.Count != 10 && UserData.CurrencyGold >= GameRules.PurchaseCosts.Values.ElementAt(UserData.ShopVariablesList.Count()))
            return true;
        return false;
    }
    #endregion
}
