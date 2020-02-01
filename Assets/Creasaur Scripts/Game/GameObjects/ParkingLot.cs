using Assets.Scripts.Game.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ParkingLot : UpgradeableModel
{
    private int _currentParkedVehicles;
    private int _advertisementCost;
    public ParkingLotData ParkingLotData;
    [Inject] protected PopupController _popupController;
    [Inject] protected UserDataManager _userDataManager;

    private void Awake()
    {
        ParkingLotData = _userDataManager.UserData.ParkingLot;
        OpenPopUp = OpenParkingLotPopUp;

        SetupPopup(PopupSystem.PopupType.ParkingLot);
    }

    public void OpenParkingLotPopUp()
    {
        var _hashTable = new Hashtable();
        _popupController.ShowPopup(PopupSystem.PopupType.ParkingLot, true, () =>
        {

        }, _hashTable);
    }

    public void SetupPopup(PopupSystem.PopupType popupType)
    {
        _popupController.SetupParkingLotPopup(popupType, this);
    }

}