using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitInQueue : CustomerBehaviour
{
    public override void Enable()
    {
        base.Enable();
        GetComponent<Customer>().State = CustomerBehaviourStates.WaitInQueue;
    }

    private void Update()
    {
        if (enabled)
        {
            Move(Color.green);
        }
    }

    public void SendToWaitInQueue(Vector3 targetPos)
    {
        CalculatePath(targetPos);
    }

    public void MoveOneLineForward(Vector3 targetPos)
    {
        CalculatePath(targetPos);
        _cornerIndex = 0;
    }
}