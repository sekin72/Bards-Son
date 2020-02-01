using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToCashierBehaviour : CustomerBehaviour
{
    private Action OnMoveToCashierFinished;

    public override void Enable()
    {
        base.Enable();
        GetComponent<Customer>().State = CustomerBehaviourStates.FirstInQueue;
    }

    private void FixedUpdate()
    {
        if (enabled)
        {
            Move(Color.green);

            if (Vector3.Distance(transform.position, _navMeshPath.corners[_navMeshPath.corners.Length - 1]) < 0.01f)
            {
                OnMoveToCashierFinished.SafeInvoke();
            }
        }
    }

    public void SendInFrontOfCashier(Cashier cashier, Vector3 pos)
    {
        CalculatePath(pos);
        OnMoveToCashierFinished = cashier.StartCustomersShopping;
    }
}
