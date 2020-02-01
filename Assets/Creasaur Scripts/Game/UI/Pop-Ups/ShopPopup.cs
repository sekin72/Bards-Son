using System;
using System.Collections;
using System.Collections.Generic;
using PopupSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ShopPopup : PopupSystem.PopupBase
{
    protected Shop _shop;
    public Sprite[] ProductSprite;
    public Sprite[] LockedProductSprite;
    public string[] ProductNames;
    public int AvailableProductIndex;
    [Inject] protected UserDataManager _userDataManager;
    [SerializeField] protected Text _barName;
    [SerializeField] protected Text _currentLevel;
    [SerializeField] protected Text _employeeCount;
    [SerializeField] protected Text _processTime;
    [SerializeField] protected Text _customersServed;
    [SerializeField] protected Text _salaryLevel;
    [SerializeField] protected Text _productName;
    [SerializeField] protected Image _productImage;

    public void SetupShopPopup(Shop shop)
    {
        _shop = shop;
        AvailableProductIndex = _shop.ShopVariables.AvailableProductIndexAndVersion;
        var sprites = shop.ShopPopupObjects.Sprites;
        var lockedSprites = shop.ShopPopupObjects.LockedSprites;
        var productNames = shop.ShopPopupObjects.ProductNames;
        ProductSprite = sprites;
        ProductNames = productNames;
        LockedProductSprite = lockedSprites;
    }

    private void SetTexts()
    {
        _currentLevel.text = _shop.ShopVariables.CurrentLevel.ToString();
        _employeeCount.text = _shop.ShopVariables.CurrentEmployeeCount.ToString();
        _processTime.text = _shop.ShopVariables.ProcessTime.ToString();
        _customersServed.text = _shop.ShopVariables.CustomersServed.ToString();
        _salaryLevel.text = _shop.ShopVariables.SalaryLevel.ToString();
        _barName.text = _shop.ShopVariables.ShopType.ToString();
        _productName.text = _shop.ShopVariables.ProductName.ToString();
        _productImage.sprite = ProductSprite[_shop.ShopVariables.AvailableProductIndexAndVersion];
    }

    public override void Show(Action callback = null, Hashtable parameters = null)
    {
        SetTexts();
        base.Show(callback, parameters);
    }

    public override void Hide(Action callback = null)
    {
        base.Hide(callback);
    }

    public void UpgradeShop()
    {
        _userDataManager.UpgradeShop(_shop);
        _currentLevel.text = _shop.ShopVariables.CurrentLevel.ToString();
        _productName.text = _shop.ShopVariables.ProductName.ToString();
        _productImage.sprite = ProductSprite[_shop.ShopVariables.AvailableProductIndexAndVersion];
    }

    public void UpgradeSalary()
    {
        _userDataManager.UpgradeSalary(_shop);
        _salaryLevel.text = _shop.ShopVariables.SalaryLevel.ToString();
        _processTime.text = _shop.ShopVariables.ProcessTime.ToString();
        _customersServed.text = _shop.ShopVariables.CustomersServed.ToString();
    }

    public void HireNewEmployee()
    {
        _userDataManager.HireNewEmployee(_shop);
        _employeeCount.text = _shop.ShopVariables.CurrentEmployeeCount.ToString();
    }
}
