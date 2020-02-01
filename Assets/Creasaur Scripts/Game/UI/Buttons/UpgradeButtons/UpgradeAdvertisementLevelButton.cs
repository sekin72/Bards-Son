using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UpgradeAdvertisementLevelButton : UpgradeButton
{
    private void OnEnable()
    {
        OnClickEvent = UpgradeAdvertisementLevel;
    }

    public void UpgradeAdvertisementLevel()
    {
        transform.parent.GetComponent<ParkingLotPopup>().UpgradeAdvertisementLevel();
    }
}
