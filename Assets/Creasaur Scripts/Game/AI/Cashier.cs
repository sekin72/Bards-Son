using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Cashier : PoolItem
{
    #region Variables
    private bool _someoneShops = false;
    private bool _isInitialized = false;

    private double _customerProcessTime;
    private double _currentProcessTime;

    private Customer _currentCustomer;
    private Vector3 _cashierWaitPoint;

    public Action SendNextInline;
    private Action OnShoppingEnded;
    private Action CustomerShopHasEnded;
    #endregion

    public void OnInitialize(Shop shop, double _servingTime, Vector3 pos)
    {
        _customerProcessTime = _servingTime;
        _currentProcessTime = _customerProcessTime;
        _cashierWaitPoint = pos;
        OnShoppingEnded = shop.GainMoney;
        Disable();
        _isInitialized = true;
    }

    public override void Enable()
    {
        _isActive = true;
    }

    public override void Disable()
    {
        _isActive = false;
    }

    public void FixedUpdate()
    {
        if (_isActive && _isInitialized)
        {
            if (_someoneShops)
            {
                _currentProcessTime -= 1;
                if (_currentProcessTime <= 0)
                {
                    FinishShopping();
                }
            }
        }
    }

    public void SetCustomerServingTime(double f)
    {
        _customerProcessTime = f;
        _currentProcessTime = _customerProcessTime;
    }

    public void FinishShopping()
    {
        ShoppingEnded();
        CustomerShopHasEnded.SafeInvoke();
    }

    public void CallCustomerToCashier(Customer customer)
    {
        if (customer)
        {
            _currentCustomer = customer;
            Enable();
            customer.OnMoveToCashierEnabled(this, _cashierWaitPoint);
            SendNextInline.SafeInvoke();
            CustomerShopHasEnded = _currentCustomer.OnGoToExitEnabled;
        }
    }

    public void StartCustomersShopping()
    {
        if (_currentCustomer)
        {
            _currentCustomer.OnShoppingEnabled();
            _someoneShops = true;
        }
    }

    private void ShoppingEnded()
    {
        _someoneShops = false;
        _currentCustomer = null;
        _currentProcessTime = _customerProcessTime;
        OnShoppingEnded.SafeInvoke();
        Disable();
    }

    public bool GetSomeoneShops()
    {
        return _someoneShops;
    }
}
