using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CruiseShipEvent : SpecialEvent
{
    private GameObject _entrancePoint;

    public override void Activate()
    {
        base.Activate();
        var _pointList = Resources.LoadAll("Points/Queues");
        _entrancePoint = (GameObject)_pointList.First(x => x.name.StartsWith("Ship Entry"));
        _spawnManager.CallAGroupofCustomers(20, _entrancePoint.transform.position);
        Deactivate();
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

}
