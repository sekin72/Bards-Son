using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsEnemyBehaviour : Behaviour
{
    public override void OnBehaviourChange()
    {
        MoveTowardsEnemy();
    }

    private void MoveTowardsEnemy()
    {
        CharacterController.Move(FindPlayerPosition());
    }

    private Vector3 FindPlayerPosition()
    {
        return GameObject.FindGameObjectWithTag("Player").transform.position;
    }

}
