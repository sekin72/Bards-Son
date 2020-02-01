using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSalaryButton : UpgradeButton
{
    private void OnEnable()
    {
        OnClickEvent = UpgradeSalary;
    }

    public void UpgradeSalary()
    {
        transform.parent.GetComponent<ShopPopup>().UpgradeSalary();
    }
}
