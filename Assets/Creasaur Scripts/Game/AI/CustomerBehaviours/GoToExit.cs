using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToExit : CustomerBehaviour
{
    private Action OnGoToExitFinished;

    public override void OnInitialize()
    {
        OnGoToExitFinished = GetComponent<Customer>().OnGoToExitFinished;
    }

    public override void Enable()
    {
        base.Enable();
        GetComponent<Customer>().State = CustomerBehaviourStates.GoToExit;
    }

    private void Update()
    {
        if (enabled)
        {
            Move(Color.blue);

            if (Vector3.Distance(transform.position, _navMeshPath.corners[_navMeshPath.corners.Length - 1]) < 0.01f)
            {
                OnGoToExitFinished.SafeInvoke();
            }
        }
    }

    public void SendToExit(Vector3 targetPos)
    {
        CalculatePath(targetPos);
    }
}
