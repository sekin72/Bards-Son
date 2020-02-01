using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdvertisementCampaignEvent : SpecialEvent
{

    public override void Activate()
    {
        base.Activate();
        Debug.Log("Advertisement Started...");
        gameObject.GetComponent<CountDown>().enabled = true;
        //TODO: Increment Customer coming rate
        _timer.Timer(TimeSpan.FromSeconds(60), Deactivate);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        Debug.Log("Advertisement Ended...");
        gameObject.GetComponent<CountDown>().enabled = false;
    }
}
