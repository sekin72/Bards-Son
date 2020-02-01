using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsEnemyBehaviour : Behaviour
{
    public override void OnBehaviourChange()
    {
        enemy.State = BehaviourState.MoveTowardsEnemy;
        MoveTowardsEnemy();
    }

    private void MoveTowardsEnemy()
    {
        SendToLocation(FindPlayerPosition());
    }

    private void Update()
    {
        if (enemy.State == BehaviourState.MoveTowardsEnemy)
        {
            if (Vector3.Distance(transform.position, enemy._destinationPos) < 0.01f)
            {
                enemy.AttackBehaviour.OnBehaviourChange();
            }
            Move(Color.red);
        }
    }
    private void SendToLocation(Vector3 targetPos)
    {
        _cornerIndex = 0;
        CalculatePath(targetPos);
    }

    private Vector3 FindPlayerPosition()
    {
        return GameObject.FindGameObjectWithTag("Player").transform.position;
    }

}
