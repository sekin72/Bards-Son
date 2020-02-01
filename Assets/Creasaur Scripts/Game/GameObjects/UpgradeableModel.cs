using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeableModel : Interactable
{
    private int _purchaseCost; //take from config

    private int _currentUpgradeCost; //take from config
    private int _costRate; //take from config
    private int _nextUpgradeCost; //_currentUpgradeCost*_costRate

    private int _maxUpgradeCount; //take from config

    private int _currentUpgradeLevel;
    private int _houseLevel; //GDD'de işletme seviyesi olarak geçiyor
}