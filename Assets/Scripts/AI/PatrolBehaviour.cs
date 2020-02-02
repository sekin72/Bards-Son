using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehaviour : Behaviour
{
    public override void OnBehaviourChange()
    {
        enemy.Animator.Play("Walk", 0, 0);
        enemy.State = BehaviourState.Patrol;
    }

    public float _patrolRadius = 4f;

    private void Update()
    {
        if (enemy.State == BehaviourState.Patrol)
        {
            if (Vector3.Distance(transform.position, enemy._destinationPos) < 0.01f)
            {
                SetNewRandomDestionationAroundShop();
            }
            Move(Color.red);
            Collider[] enemies = Physics.OverlapSphere(transform.position, _patrolRadius * 4, enemy._playerLayer);
            for (int i = 0; i < enemies.Length; i++)
            {
                enemy.MoveTowardsEnemyBehaviour.OnBehaviourChange();
            }
        }
    }

    public void SetNewRandomDestionationAroundShop()
    {
        Vector3 newDestination = RandomNavSphere(enemy.StartPos, _patrolRadius);

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
