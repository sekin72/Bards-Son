using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HireNewEmpButton : UpgradeButton
{
    private void OnEnable()
    {
        OnClickEvent = HireNewEmployee;
    }

    public void HireNewEmployee()
    {
        transform.parent.GetComponent<ShopPopup>().HireNewEmployee();
    }
}
