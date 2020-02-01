using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : CustomerBehaviour
{
    private float _patrolRadius = 10f;
    public Vector3 ShopPos;

    public override void Enable()
    {
        base.Enable();
        GetComponent<Customer>().State = CustomerBehaviourStates.Patrol;
    }

    private void Update()
    {
        if (enabled)
        {
            if (ShopPos != null && Vector3.Distance(transform.position, _destinationPos) < 0.01f)
            {
                SetNewRandomDestionationAroundShop();
            }
            Move(Color.red);
        }
    }

    public void SetNewRandomDestionationAroundShop()
    {
        Vector3 newDestination = RandomNavSphere(ShopPos, _patrolRadius);

        if (newDestination.Equals(Vector3.zero))
        {
            Debug.Log("Couldn't find a suitable random position after 30 tries.");
        }

        SendToLocation(newDestination);
    }

    private Vector3 RandomNavSphere(Vector3 originPos, float dist)
    {
        var result = Vector3.zero;
        for (int i = 0; i < 30; i++)
        {
            var randomPoint = originPos + Random.insideUnitSphere * dist;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 5f, NavMesh.AllAreas))
            {
                result = hit.position;
                return result;
            }
        }
        return result;
    }

    private void SendToLocation(Vector3 targetPos)
    {
        _cornerIndex = 0;
        CalculatePath(targetPos);
    }
}
