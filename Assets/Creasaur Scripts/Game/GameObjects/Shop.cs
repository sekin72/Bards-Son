using Assets.Scripts.Game.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using PopupSystem;
using Timer;
using System.Linq;
using UnityEngine;
using Zenject;
using UnityEngine.AI;

public class Shop : UpgradeableModel
{
    private List<Cashier> CashierScriptList;

    [Inject] protected PopupController _popupController;
    [Inject] protected ShopQueueController _queueManager;
    [Inject] protected CreaCoroutine creaCoroutine;
    [Inject] protected UserDataManager _userDataManager;
    [Inject] protected GameDataManager _gameDataManager;
    [Inject] protected NavMeshSurface _navMeshSurface;

    private Action<Customer> CallCustomerToCashier;
    private Action CallCustomerToWaitingLine;
    private Func<Customer> CallCustomerFromWaitingLine;
    private int _cashierMaxCustomerCount;
    protected Vector3 _waitingQueueDirection;

    private double _TPV;

    public ShopVariables ShopVariables;

    public ShopPopupObjects ShopPopupObjects;
    protected AtlasLoader _atlasLoader;

    private GameObject _myGameObject;

    public Shop() { }

    protected void ParentStart()
    {
        _cashierMaxCustomerCount = GameRules.StandMaxLineCount[ShopVariables.ShopType];
        _queueManager.CashierMaxCustomerCount = _cashierMaxCustomerCount;
        _queueManager.WaitingQueueDirection = _waitingQueueDirection;
        CallCustomerFromWaitingLine = _queueManager.CallCustomerFromWaitingLineToCashier;
        CallCustomerToWaitingLine = _queueManager.CallCustomerFromPatrolToWaitingLine;
    }

    private void SetCashier(int index)
    {
        var cashier = CashierScriptList[index];
        var cashierPos = cashier.transform.Find("CashierPoint").position;
        cashier.OnInitialize(this, ShopVariables.ProcessTime, cashierPos);
        cashier.SendNextInline = _queueManager.MoveWaitingCustomersOnePlaceForward;
        CashierScriptList[index].gameObject.SetActive(true);
    }

    protected void ParentFixedUpdate()
    {
        _queueManager.FixedUpdate();
        CallCustomerFromWaitingLineToCashier();
    }

    public void GainMoney()
    {
        _userDataManager.ChangeCurrencyGold(+_TPV);
    }

    public void AddCustomerToQueue(Customer customer)
    {
        _queueManager.AddNewCustomerToQueue(customer);
    }

    public void CallCustomerFromWaitingLineToCashier()
    {
        var cashierWhoHasRoom = CashierScriptList.FirstOrDefault(x => x.gameObject.activeInHierarchy && !x.IsActive());
        if (_queueManager.CustomerWaitsInTheShop && cashierWhoHasRoom != null)
        {
            CallCustomerToCashier = cashierWhoHasRoom.CallCustomerToCashier;
            CallCustomerToCashier(CallCustomerFromWaitingLine.SafeInvoke());
        }
    }

    public void SetShopDataFromZero(ShopVariables variable)
    {
        //Set shop data, and variables
        ShopVariables = variable;
        SetShopDataUpdateLevel(variable);

        ChangeShopAssetVersion();
        SetCashierList();
    }

    public void ShopAssetVersionChange()
    {
        ChangeShopAssetVersion();

        //Finish old cashiers' jobs
        foreach (var cashier in CashierScriptList.FindAll(x => x.gameObject.activeInHierarchy && x.IsActive()))
        {
            cashier.FinishShopping();
        }

        SetCashierList();
    }

    public void SetShopDataUpdateLevel(ShopVariables variable)
    {
        var oldVersion = ShopVariables.AvailableProductIndexAndVersion;
        ShopVariables = variable;
        _TPV = GameRules.TPV(ShopVariables.ShopType, ShopVariables.CurrentLevel);

        if (oldVersion != ShopVariables.AvailableProductIndexAndVersion)
        {
            ShopAssetVersionChange();
        }
    }

    public void SetShopDataUpdateSalary(ShopVariables variable)
    {
        //Set shop data, and variables
        ShopVariables = variable;

        for (int i = 0; i < ShopVariables.CurrentEmployeeCount; i++)
        {
            CashierScriptList[i].SetCustomerServingTime(ShopVariables.ProcessTime);
        }
    }

    public void SetShopDataHireEmp(ShopVariables variable)
    {
        //Set shop data, and variables
        ShopVariables = variable;
        SetCashier(ShopVariables.CurrentEmployeeCount - 1);
    }

    public void ChangeShopAssetVersion()
    {
        //Set shop object
        Destroy(_myGameObject);
        var obj = Resources.Load("Prefabs/ShopPrefabs/" + ShopVariables.ShopType.ToString() + (ShopVariables.AvailableProductIndexAndVersion + 1).ToString());
        _myGameObject = GameObject.Instantiate(obj) as GameObject;
        _myGameObject.transform.parent = transform;
        _navMeshSurface.BuildNavMesh();
    }

    public void SetCashierList()
    {
        //Set waiting line position and cashiers
        _queueManager.WaitingQueuePoint = _myGameObject.transform.Find("WaitingPoint").transform.position;
        CashierScriptList = _myGameObject.transform.GetComponentsInChildren<Cashier>().ToList();
        for (int i = 0; i < CashierScriptList.Count; i++)
        {
            if (i < ShopVariables.CurrentEmployeeCount)
            {
                SetCashier(i);
            }
            else
            {
                CashierScriptList[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetupPopup(PopupSystem.PopupType popupType)
    {
        _popupController.SetupShopPopup(popupType, this);
    }

    public void ParentOpenPopUp(PopupSystem.PopupType popupType)
    {
        Hashtable _hashTable = new Hashtable();
        _popupController.ShowPopup(popupType, true, () =>
            {

            }, _hashTable);
    }
}

public struct ShopPopupObjects
{
    public Sprite[] Sprites;
    public Sprite[] LockedSprites;
    public string[] ProductNames;
}

public enum ShopType
{
    Drinks,
    IceCream,
    FastFood,
    Cocktail,
    SeasideShop,
    Sushi,
    NightClub,
    LuxuryRestaurant,
    Hotel
}
