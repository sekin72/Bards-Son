using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Timer;
using System;

public class VIPShipEvent : SpecialEvent
{
    private int _income = 200; 

    public override void Activate()
    {
        base.Activate();
        _userDataManager.ChangeCurrencyGold(_income);
        _userDataManager.SaveLocalData();
        Deactivate();
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }
}
