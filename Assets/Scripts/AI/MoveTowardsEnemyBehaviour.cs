using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsEnemyBehaviour : Behaviour
{
    private Player _player;
    public override void OnBehaviourChange()
    {
        enemy.State = BehaviourState.MoveTowardsEnemy;
    }

    private void Awake()
    {
        _player = FindPlayerPosition();
    }

    private void Update()
    {
        if (enemy.State == BehaviourState.MoveTowardsEnemy)
        {
            if (Vector3.Distance(transform.position, _player.transform.position) < 0.01f)
            {
                enemy.AttackBehaviour.OnBehaviourChange();
            }
            SendToLocation(_player.transform.position);
            Move(Color.red);
        }
    }
    private void SendToLocation(Vector3 targetPos)
    {
        _cornerIndex = 0;
        CalculatePath(targetPos);
    }

    private Player FindPlayerPosition()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

}
