using PopupSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ClubPopup : ShopPopup
{
    public override PopupType PopupType
    {
        get
        {
            return PopupSystem.PopupType.Club;
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void Hide(Action callback = null)
    {
        base.Hide(callback);
    }
}

