using PopupSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ParkingLotPopup : PopupSystem.PopupBase
{
    [Inject] protected UserDataManager _userDataManager;
    private ParkingLot _parkingLot;

    [SerializeField] protected Text _currentLevel;
    [SerializeField] protected Text _parkingSpaces;
    [SerializeField] protected Text _advertisingLevel;
    [SerializeField] protected Text _carEntry;

    public void SetupParkingLotPopup(ParkingLot lot)
    {
        _parkingLot = lot;
    }

    private void SetTexts()
    {
        _currentLevel.text = _parkingLot.ParkingLotData.Level.ToString();
        _advertisingLevel.text = _parkingLot.ParkingLotData.AdvertisementLevel.ToString();
        _parkingSpaces.text = _parkingLot.ParkingLotData.MaxParkingCapacity.ToString();
        _carEntry.text = _parkingLot.ParkingLotData.CarsEntry.ToString();
    }

    public override PopupType PopupType
    {
        get
        {
            return PopupSystem.PopupType.ParkingLot;
        }
    }

    public override void Show(Action callback = null, Hashtable parameters = null)
    {
        SetTexts();
        base.Show(callback, parameters);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void Hide(Action callback = null)
    {
        base.Hide(callback);
    }

    public void UpgradeParkingLot()
    {
        _userDataManager.UpgradeParkingLot(_parkingLot);
        _currentLevel.text = _parkingLot.ParkingLotData.Level.ToString();
        _parkingSpaces.text = _parkingLot.ParkingLotData.MaxParkingCapacity.ToString();
    }

    public void UpgradeAdvertisementLevel()
    {
        _userDataManager.UpgradeAdvertisingLevel(_parkingLot);
        _advertisingLevel.text = _parkingLot.ParkingLotData.AdvertisementLevel.ToString();
        _carEntry.text = _parkingLot.ParkingLotData.CarsEntry.ToString();
    }
}
