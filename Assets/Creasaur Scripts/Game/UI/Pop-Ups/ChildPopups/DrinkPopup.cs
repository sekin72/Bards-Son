﻿using Drink;
using PopupSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrinkPopup : ShopPopup
{
    public override PopupType PopupType
    {
        get
        {
            return PopupSystem.PopupType.Drink;
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