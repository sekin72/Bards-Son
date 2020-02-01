using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UpgradeParkingLotButton : UpgradeButton
{
    private void OnEnable()
    {
        OnClickEvent = UpgradeParkingLot;
    }

    public void UpgradeParkingLot()
    {
        transform.parent.GetComponent<ParkingLotPopup>().UpgradeParkingLot();
    }
}
