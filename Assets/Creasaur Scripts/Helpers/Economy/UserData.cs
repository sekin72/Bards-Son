using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    [JsonProperty(PropertyName = "user_ID")]
    public string UserID { get; set; }

    [JsonProperty(PropertyName = "currency_gold")]
    public double CurrencyGold { get; set; }

    [JsonProperty(PropertyName = "currency_diamond")]
    public double CurrencyDiamond { get; set; }

    [JsonProperty(PropertyName = "home_level")]
    public int HomeLevel { get; set; }

    [JsonProperty(PropertyName = "parking_lot")]
    public ParkingLotData ParkingLot;

    [JsonProperty(PropertyName = "shop_data_list")]
    public List<ShopVariables> ShopVariablesList;

    public UserData()
    {
        UserID = CreaUtils.GetDeviceId();
        CurrencyGold = 0;
        CurrencyDiamond = 0;
        HomeLevel = 1;
        ShopVariablesList = new List<ShopVariables>();
    }

    public void NewUser()
    {
        ShopVariablesList.Add(new ShopVariables(1, ShopType.Drinks, 1, 1));
        ParkingLot = new ParkingLotData(1, 1);
    }
}

public struct ShopVariables
{
    private int _currentLevel;
    private int _currentEmployeeCount;
    private int _salaryLevel;
    private ShopType _shopType;

    public Action OnCurrentLevelChanged;
    public Action OnShopTypeChanged;
    public Action OnSalaryLevelChanged;
    public Action OnEmployeeCountChanged;

    public int CurrentLevel
    {
        set
        {
            _currentLevel = value;
            OnCurrentLevelChanged.SafeInvoke();
        }
        get
        {
            return _currentLevel;
        }
    }
    public ShopType ShopType
    {
        set
        {
            _shopType = value;
            OnShopTypeChanged.SafeInvoke();
        }
        get
        {
            return _shopType;
        }
    }
    public int CurrentEmployeeCount
    {
        set
        {
            _currentEmployeeCount = value;
            OnEmployeeCountChanged.SafeInvoke();
        }
        get
        {
            return _currentEmployeeCount;
        }
    }
    public int SalaryLevel
    {
        set
        {
            _salaryLevel = value;
            OnSalaryLevelChanged.SafeInvoke();
        }
        get
        {
            return _salaryLevel;
        }
    }

    public double ProcessTime
    {
        get { return GameRules.GetProcessingTime(ShopType, SalaryLevel); }
    }
    public double CustomersServed
    {
        get { return GameRules.CustomerServed(CurrentEmployeeCount, ProcessTime); }
    }
    public string ProductName
    {
        get { return GameRules.ShopProductNames[ShopType][AvailableProductIndexAndVersion]; }
    }
    public int AvailableProductIndexAndVersion
    {
        get { return GameRules.GetAvailableProduct(CurrentLevel); }
    }

    public ShopVariables(int level, ShopType type, int empCount, int salaryLevel)
    {
        _currentLevel = 0;
        _currentEmployeeCount = 0;
        _salaryLevel = 0;
        _shopType = ShopType.Drinks;

        OnCurrentLevelChanged = null;
        OnShopTypeChanged = null;
        OnSalaryLevelChanged = null;
        OnEmployeeCountChanged = null;

        CurrentLevel = level;
        ShopType = type;
        CurrentEmployeeCount = empCount;
        SalaryLevel = salaryLevel;
    }
}

public struct ParkingLotData
{
    public int Level;
    public int AdvertisementLevel;

    public ParkingLotData(int level, int adLevel)
    {
        Level = level;
        AdvertisementLevel = adLevel;
    }
    public int MaxParkingCapacity
    {
        get { return 6 * Level; }
    }
    public double CarsEntry
    {
        get { return GameRules.ParkingLotCarsEntry(AdvertisementLevel); }
    }
}