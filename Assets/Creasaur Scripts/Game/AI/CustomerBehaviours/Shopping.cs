using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopping : CustomerBehaviour
{
    public override void Enable()
    {
        base.Enable();
        GetComponent<Customer>().State = CustomerBehaviourStates.Shopping;
    }

}
