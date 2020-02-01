using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipForSalesmenEvent : SpecialEvent
{
    public override void Activate()
    {
        base.Activate();
        Debug.Log("Tip For Salesmen Started...");
        gameObject.GetComponent<CountDown>().enabled = true;
        //TODO: Increment Shopping speed
        _timer.Timer(TimeSpan.FromSeconds(60), Deactivate);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        Debug.Log("Tip For Salesmen Ended...");
        gameObject.GetComponent<CountDown>().enabled = false;
    }
}
