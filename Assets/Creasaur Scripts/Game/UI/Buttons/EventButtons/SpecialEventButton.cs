using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEventButton : ButtonBase
{
    [SerializeField] private SpecialEvent _specialEvent;

    private void OnEnable()
    {
        OnClickEvent = _specialEvent.Activate;
    }

}
