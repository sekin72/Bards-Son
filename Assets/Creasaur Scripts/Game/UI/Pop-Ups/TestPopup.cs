using System;
using System.Collections;
using System.Collections.Generic;
using PopupSystem;
using UnityEngine;

public class TestPopup : PopupSystem.PopupBase
{
    public override PopupType PopupType
    {
        get
        {
            return PopupSystem.PopupType.Test;
        }
    }

    public override void Show(Action callback = null, Hashtable parameters = null)
    {
        base.Show(callback, parameters);

        //var param = parameters["hello"];
        //Debug.Log(param);
    }

    public override void Hide(Action callback = null)
    {
        base.Hide(callback);
    }


}
